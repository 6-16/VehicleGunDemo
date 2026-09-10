using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ProBase
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private InputActionReference _cancelAction;

        public override void InstallBindings()
        {
            InstallSignals();

            Container.BindInterfacesAndSelfTo<PauseService>().AsSingle();

            if (_cancelAction == null) return;

            Container.BindInterfacesTo<PauseInputListener>().AsSingle().WithArguments(_cancelAction);
        }

        private void InstallSignals()
        {
            if (!Container.HasBinding<SignalBus>())
            {
                SignalBusInstaller.Install(Container);
            }

            Container.DeclareSignal<PauseChangedSignal>();
        }
    }
}
