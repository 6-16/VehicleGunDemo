using System;
using UnityEngine;
using Zenject;

namespace ProBase
{
    public class GameplayState : IPayloadAppState<GameplayRequest>
    {
        private readonly SceneTransitionService _sceneTransition;
        private readonly SceneCatalog _sceneCatalog;

        public GameplayState(SceneTransitionService sceneTransition, SceneCatalog sceneCatalog)
        {
            _sceneTransition = sceneTransition ?? throw new ArgumentNullException(nameof(sceneTransition));
            _sceneCatalog = sceneCatalog != null ? sceneCatalog : throw new ArgumentNullException(nameof(sceneCatalog));
        }

        public Awaitable EnterAsync(GameplayRequest payload)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));

            return _sceneTransition.LoadAsync(
                _sceneCatalog.Gameplay,
                container => container.Bind<GameplayRequest>().FromInstance(payload).AsSingle());
        }

        public Awaitable ExitAsync()
        {
            return Awaitable.NextFrameAsync();
        }
    }
}
