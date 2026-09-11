using System;
using UnityEngine;
using Zenject;

public class VehicleHealthBar : MonoBehaviour
{
    [SerializeField] private HealthBarView _bar;

    private VehicleHealth _health;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(VehicleHealth health, SignalBus signalBus)
    {
        _health = health ?? throw new ArgumentNullException(nameof(health));
        _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
    }

    private void Start()
    {
        _bar.Bind(_health);
        _bar.Hide();

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
        _bar.Show();
    }

    private void OnRunFinished(RunFinishedSignal signal)
    {
        _bar.Hide();
    }
}
