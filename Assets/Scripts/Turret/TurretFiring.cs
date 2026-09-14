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
    private float _flashRemaining;

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

        float delta = Time.deltaTime;

        FadeMuzzleFlash(delta);

        if (_config.FireInterval <= 0f) return;

        _cooldown -= delta;

        if (_cooldown > 0f) return;

        _cooldown = _config.FireInterval;

        Fire();
    }

    private void Fire()
    {
        _projectiles.Fire(_view.Muzzle);

        _flashRemaining = _config.MuzzleFlashDuration;

        _view.SetMuzzleFlash(_flashRemaining > 0f);
    }

    private void FadeMuzzleFlash(float delta)
    {
        if (_flashRemaining <= 0f) return;

        _flashRemaining -= delta;

        if (_flashRemaining > 0f) return;

        _view.SetMuzzleFlash(false);
    }
}
