using ProBase;
using UnityEngine;
using Zenject;

public class RunInstaller : MonoInstaller
{
    [SerializeField] private VehicleConfig _vehicleConfig;
    [SerializeField] private TurretConfig _turretConfig;
    [SerializeField] private ProjectileConfig _projectileConfig;
    [SerializeField] private RoadConfig _roadConfig;
    [SerializeField] private CameraConfig _cameraConfig;

    public override void InstallBindings()
    {
        InstallSignals();
        InstallConfigs();
    }

    private void InstallSignals()
    {
        if (!Container.HasBinding<SignalBus>())
        {
            SignalBusInstaller.Install(Container);
        }

        Container.DeclareSignal<RunStartedSignal>();
        Container.DeclareSignal<RunFinishedSignal>();
    }

    private void InstallConfigs()
    {
        Container.BindInstance(_vehicleConfig).AsSingle();
        Container.BindInstance(_turretConfig).AsSingle();
        Container.BindInstance(_projectileConfig).AsSingle();
        Container.BindInstance(_roadConfig).AsSingle();
        Container.BindInstance(_cameraConfig).AsSingle();

        Container.Bind<LevelConfig>().FromResolveGetter<GameplayRequest>(request => request.Level).AsSingle();
    }
}
