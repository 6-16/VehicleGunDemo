using System;
using UnityEngine;
using Zenject;

public class RunController : ITickable
{
    private readonly VehicleMovement _movement;
    private readonly LevelConfig _level;
    private readonly SignalBus _signalBus;

    private bool _isRunning;
    private bool _isFinished;

    public bool IsRunning => _isRunning;
    public float NormalizedProgress => Mathf.Clamp01(_movement.Travelled / _level.Distance);

    public RunController(VehicleMovement movement, LevelConfig level, SignalBus signalBus)
    {
        _movement = movement ?? throw new ArgumentNullException(nameof(movement));
        _level = level != null ? level : throw new ArgumentNullException(nameof(level));
        _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
    }

    public void StartRun()
    {
        if (_isRunning || _isFinished) return;

        _isRunning = true;
        _movement.Begin();

        _signalBus.Fire(new RunStartedSignal());
    }

    public void Finish(RunResult result)
    {
        if (!_isRunning) return;

        _isRunning = false;
        _isFinished = true;
        _movement.Stop();

        _signalBus.Fire(new RunFinishedSignal(result));
    }

    public void Tick()
    {
        if (!_isRunning) return;
        if (_movement.Travelled < _level.Distance) return;

        Finish(RunResult.Victory);
    }
}
