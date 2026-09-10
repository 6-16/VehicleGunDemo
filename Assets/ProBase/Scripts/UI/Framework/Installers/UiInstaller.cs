using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ProBase
{
    public class UiInstaller : MonoInstaller
    {
        [SerializeField] private UiSceneConfig _sceneConfig;
        [SerializeField] private UiRootView _rootPrefab;
        [SerializeField] private InputActionReference _cancelAction;

        public override void InstallBindings()
        {
            InstallSignals();

            Container.BindInstance(_sceneConfig).AsSingle();

            Container.Bind<UiRootView>()
                .FromComponentInNewPrefab(_rootPrefab)
                .AsSingle()
                .NonLazy();

            Container.Bind<UiPrefabFactory>().AsSingle();
            Container.Bind<UiScreenRegistry>().AsSingle();
            Container.Bind<UiScreenFactory>().AsSingle();
            Container.Bind<UiNavigationStack>().AsSingle();

            Container.BindInterfacesAndSelfTo<UiService>().AsSingle();
            Container.BindInterfacesTo<UiSceneBootstrapper>().AsSingle();

            if (_cancelAction == null) return;

            Container.BindInterfacesTo<UiBackInputListener>().AsSingle().WithArguments(_cancelAction);
        }

        private void InstallSignals()
        {
            if (!Container.HasBinding<SignalBus>())
            {
                SignalBusInstaller.Install(Container);
            }

            Container.DeclareSignal<ScreenOpenedSignal>();
            Container.DeclareSignal<ScreenClosedSignal>();
        }
    }
}
