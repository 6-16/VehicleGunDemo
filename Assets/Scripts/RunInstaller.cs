using ProBase;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class RunInstaller : MonoInstaller
{
    private const string RoadParentName = "Road";
    private const string ProjectileParentName = "Projectiles";

    [SerializeField] private VehicleConfig _vehicleConfig;
    [SerializeField] private TurretConfig _turretConfig;
    [SerializeField] private ProjectileConfig _projectileConfig;
    [SerializeField] private RoadConfig _roadConfig;
    [SerializeField] private CameraConfig _cameraConfig;
    [SerializeField] private VehicleView _vehiclePrefab;
    [SerializeField] private Camera _runCamera;
    [SerializeField] private InputActionReference _aimDragAction;
    [SerializeField] private InputActionReference _aimHoldAction;

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

        Container.Bind<TurretView>().FromResolveGetter<VehicleView>(vehicle => vehicle.Turret).AsSingle();
    }

    private void InstallPools()
    {
        Container.BindMemoryPool<RoadSegmentView, RoadSegmentView.Pool>()
            .WithInitialSize(_roadConfig.PoolSize)
            .FromComponentInNewPrefab(_roadConfig.Prefab)
            .UnderTransformGroup(RoadParentName);

        Container.BindMemoryPool<ProjectileView, ProjectileView.Pool>()
            .WithInitialSize(_projectileConfig.PoolSize)
            .FromComponentInNewPrefab(_projectileConfig.Prefab)
            .UnderTransformGroup(ProjectileParentName);
    }

    private void InstallSystems()
    {
        Container.BindInterfacesAndSelfTo<VehicleMovement>().AsSingle();
        Container.BindInterfacesAndSelfTo<RunController>().AsSingle();
        Container.BindInterfacesTo<RoadSystem>().AsSingle();
        Container.BindInterfacesTo<RunCamera>().AsSingle();

        Container.BindInterfacesAndSelfTo<AimInput>()
            .AsSingle()
            .WithArguments(_aimDragAction, _aimHoldAction);

        Container.BindInterfacesTo<TurretAiming>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectileSystem>().AsSingle();
        Container.BindInterfacesTo<TurretFiring>().AsSingle();
    }
}
