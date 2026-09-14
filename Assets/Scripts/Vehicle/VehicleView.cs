using System;
using UnityEngine;
using Zenject;

public class VehicleView : MonoBehaviour
{
    [SerializeField] private Transform _body;
    [SerializeField] private TurretView _turret;
    [SerializeField] private DamageFlashView _damageFlash;
    [SerializeField] private DamageSquashView _damageSquash;
    [SerializeField] private ParticleSystem[] _driveEffects;

    private SignalBus _signalBus;
    private Transform _transform;

    public Transform Transform => _transform;
    public Transform Body => _body;
    public TurretView Turret => _turret;

    [Inject]
    private void Construct(VehicleHealth health, SignalBus signalBus)
    {
        if (health == null) throw new ArgumentNullException(nameof(health));

        _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));

        _damageFlash.Bind(health);
        _damageSquash.Bind(health);
    }

    private void Awake()
    {
        _transform = transform;
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
