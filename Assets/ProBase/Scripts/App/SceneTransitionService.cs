using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace ProBase
{
    public class SceneTransitionService
    {
        private const float ActivationProgress = 0.9f;

        private readonly ZenjectSceneLoader _sceneLoader;
        private readonly SceneCatalog _sceneCatalog;
        private readonly LoadingProgress _progress;

        public SceneTransitionService(ZenjectSceneLoader sceneLoader, SceneCatalog sceneCatalog, LoadingProgress progress)
        {
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _sceneCatalog = sceneCatalog != null ? sceneCatalog : throw new ArgumentNullException(nameof(sceneCatalog));
            _progress = progress ?? throw new ArgumentNullException(nameof(progress));
        }

        public Awaitable LoadAsync(SceneReference target)
        {
            return LoadAsync(target, null);
        }

        public async Awaitable LoadAsync(SceneReference target, Action<DiContainer> extraBindings)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            if (!target.IsAssigned)
            {
                throw new InvalidOperationException($"A scene is not assigned in {nameof(SceneCatalog)}.");
            }

            _progress.Reset();

            await LoadLoadingSceneAsync();
            await LoadTargetAsync(target, extraBindings);

            _progress.Report(1f);
        }

        private async Awaitable LoadLoadingSceneAsync()
        {
            SceneReference loading = _sceneCatalog.Loading;

            if (loading == null || !loading.IsAssigned) return;

            AsyncOperation operation = _sceneLoader.LoadSceneAsync(loading.SceneName, LoadSceneMode.Single);

            while (!operation.isDone)
            {
                await Awaitable.NextFrameAsync();
            }
        }

        private async Awaitable LoadTargetAsync(SceneReference target, Action<DiContainer> extraBindings)
        {
            AsyncOperation operation = _sceneLoader.LoadSceneAsync(target.SceneName, LoadSceneMode.Single, extraBindings);

            while (!operation.isDone)
            {
                _progress.Report(operation.progress / ActivationProgress);

                await Awaitable.NextFrameAsync();
            }
        }
    }
}
