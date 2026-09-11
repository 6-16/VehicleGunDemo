using ProBase;
using UnityEngine;
using Zenject;

public class RunInstaller : MonoInstaller
{
    private const string RoadParentName = "Road";

    [SerializeField] private VehicleConfig _vehicleConfig;
    [SerializeField] private TurretConfig _turretConfig;
    [SerializeField] private ProjectileConfig _projectileConfig;
    [SerializeField] private RoadConfig _roadConfig;
    [SerializeField] private CameraConfig _cameraConfig;
    [SerializeField] private VehicleView _vehiclePrefab;
    [SerializeField] private Camera _runCamera;

    public override void InstallBindings()
    {
        InstallSignals();
        InstallConfigs();
        InstallViews();
        InstallPools();
        InstallSystems();
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

    private void InstallViews()
    {
        Container.BindInstance(_runCamera).AsSingle();

        Container.Bind<VehicleView>()
            .FromComponentInNewPrefab(_vehiclePrefab)
            .AsSingle()
            .NonLazy();
    }

    private void InstallPools()
    {
        Container.BindMemoryPool<RoadSegmentView, RoadSegmentView.Pool>()
            .WithInitialSize(_roadConfig.PoolSize)
            .FromComponentInNewPrefab(_roadConfig.Prefab)
            .UnderTransformGroup(RoadParentName);
    }

    private void InstallSystems()
    {
        Container.BindInterfacesAndSelfTo<VehicleMovement>().AsSingle();
        Container.BindInterfacesAndSelfTo<RunController>().AsSingle();
        Container.BindInterfacesTo<RoadSystem>().AsSingle();
        Container.BindInterfacesTo<RunCamera>().AsSingle();
    }
}
