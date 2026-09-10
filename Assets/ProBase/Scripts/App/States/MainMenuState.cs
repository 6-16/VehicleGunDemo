using System;
using UnityEngine;

namespace ProBase
{
    public class MainMenuState : ISimpleAppState
    {
        private readonly SceneTransitionService _sceneTransition;
        private readonly SceneCatalog _sceneCatalog;

        public MainMenuState(SceneTransitionService sceneTransition, SceneCatalog sceneCatalog)
        {
            _sceneTransition = sceneTransition ?? throw new ArgumentNullException(nameof(sceneTransition));
            _sceneCatalog = sceneCatalog != null ? sceneCatalog : throw new ArgumentNullException(nameof(sceneCatalog));
        }

        public Awaitable EnterAsync()
        {
            return _sceneTransition.LoadAsync(_sceneCatalog.MainMenu);
        }

        public Awaitable ExitAsync()
        {
            return Awaitable.NextFrameAsync();
        }
    }
}
