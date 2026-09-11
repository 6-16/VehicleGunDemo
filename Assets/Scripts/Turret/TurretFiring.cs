using System;
using UnityEngine;
using Zenject;

public class TurretFiring : ITickable
{
    private readonly TurretView _view;
    private readonly TurretConfig _config;
    private readonly ProjectileSystem _projectiles;
    private readonly RunController _runController;

    private float _cooldown;

    public TurretFiring(TurretView view, TurretConfig config, ProjectileSystem projectiles, RunController runController)
    {
        _view = view != null ? view : throw new ArgumentNullException(nameof(view));
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
        _projectiles = projectiles ?? throw new ArgumentNullException(nameof(projectiles));
        _runController = runController ?? throw new ArgumentNullException(nameof(runController));
    }

    public void Tick()
    {
        if (!_runController.IsRunning) return;
        if (_config.FireInterval <= 0f) return;

        _cooldown -= Time.deltaTime;

        if (_cooldown > 0f) return;

        _cooldown = _config.FireInterval;
        _projectiles.Fire(_view.Muzzle);
    }
}
