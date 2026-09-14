using System;
using UnityEngine;
using Zenject;

public class TurretView : MonoBehaviour
{
    [SerializeField] private Transform _pivot;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private LineRenderer _laser;
    [SerializeField] private Light _muzzleFlash;

    private SignalBus _signalBus;

    public Transform Pivot => _pivot;
    public Transform Muzzle => _muzzle;

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
    }

    private void Start()
    {
        _laser.enabled = false;

        SetMuzzleFlash(false);

        _signalBus.Subscribe<RunStartedSignal>(OnRunStarted);
        _signalBus.Subscribe<RunFinishedSignal>(OnRunFinished);
    }

    public void SetMuzzleFlash(bool isEnabled)
    {
        if (_muzzleFlash == null) return;

        _muzzleFlash.enabled = isEnabled;
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<RunStartedSignal>(OnRunStarted);
        _signalBus.Unsubscribe<RunFinishedSignal>(OnRunFinished);
    }

    private void OnRunStarted()
    {
        _laser.enabled = true;
    }

    private void OnRunFinished(RunFinishedSignal signal)
    {
        _laser.enabled = false;
    }
}
