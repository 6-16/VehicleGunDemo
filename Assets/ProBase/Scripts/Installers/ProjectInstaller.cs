using UnityEngine;
using Zenject;

namespace ProBase
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private SceneCatalog _sceneCatalog;
        [SerializeField] private LevelCatalog _levelCatalog;

        public override void InstallBindings()
        {
            Container.BindInstance(_sceneCatalog).AsSingle();
            Container.BindInstance(_levelCatalog).AsSingle();

            Container.Bind<LoadingProgress>().AsSingle();
            Container.Bind<SceneTransitionService>().AsSingle();

            InstallAppStates();

            Container.BindInterfacesTo<AppBootstrapper>().AsSingle();
        }

        private void InstallAppStates()
        {
            Container.Bind<IAppState>().To<MainMenuState>().AsSingle();
            Container.Bind<IAppState>().To<GameplayState>().AsSingle();

            Container.Bind<AppStateMachine>().AsSingle();
        }
    }
}
