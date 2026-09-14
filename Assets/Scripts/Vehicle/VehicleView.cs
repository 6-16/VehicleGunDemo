using System;
using UnityEngine;
using Zenject;

public class VehicleView : MonoBehaviour
{
    [SerializeField] private Transform _body;
    [SerializeField] private TurretView _turret;
    [SerializeField] private DamageFlashView _damageFlash;
    [SerializeField] private DamageSquashView _damageSquash;
    [SerializeField] private VehicleDamageEffects _damageEffects;
    [SerializeField] private ParticleSystem[] _driveEffects;
    [SerializeField] private Transform[] _wheels;

    private SignalBus _signalBus;
    private Transform _transform;
    private float _wheelRadius;

    public Transform Transform => _transform;
    public Transform Body => _body;
    public TurretView Turret => _turret;
    public Transform[] Wheels => _wheels;
    public float WheelRadius => _wheelRadius;

    [Inject]
    private void Construct(VehicleHealth health, SignalBus signalBus)
    {
        if (health == null) throw new ArgumentNullException(nameof(health));

        _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));

        _damageFlash.Bind(health);
        _damageSquash.Bind(health);
        _damageEffects.Bind(health);
    }

    private void Awake()
    {
        _transform = transform;
        _wheelRadius = MeasureWheelRadius();
    }

    private float MeasureWheelRadius()
    {
        if (_wheels.Length == 0) return 0f;

        Renderer[] renderers = _wheels[0].GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0) return 0f;

        Bounds bounds = renderers[0].bounds;

        for (int index = 1; index < renderers.Length; index++)
        {
            bounds.Encapsulate(renderers[index].bounds);
        }

        return Mathf.Max(bounds.size.y, bounds.size.z) * 0.5f;
    }

    private void Start()
    {
        StopDriveEffects();

        _signalBus.Subscribe<RunStartedSignal>(OnRunStarted);
        _signalBus.Subscribe<RunFinishedSignal>(OnRunFinished);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<RunStartedSignal>(OnRunStarted);
        _signalBus.Unsubscribe<RunFinishedSignal>(OnRunFinished);
    }

    private void OnRunStarted()
    {
        foreach (ParticleSystem effect in _driveEffects)
        {
            effect.Play(true);
        }
    }

    private void OnRunFinished(RunFinishedSignal signal)
    {
        StopDriveEffects();
    }

    private void StopDriveEffects()
    {
        foreach (ParticleSystem effect in _driveEffects)
        {
            effect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
