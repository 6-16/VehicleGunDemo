using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemySystem : ITickable
{
    private readonly EnemyConfig _config;
    private readonly ProjectileConfig _projectileConfig;
    private readonly EnemyView.Pool _pool;
    private readonly VehicleView _vehicle;
    private readonly VehicleHealth _vehicleHealth;
    private readonly ProjectileSystem _projectiles;
    private readonly EffectSystem _effects;
    private readonly List<EnemyView> _active = new List<EnemyView>();

    public EnemySystem(
        EnemyConfig config,
        ProjectileConfig projectileConfig,
        EnemyView.Pool pool,
        VehicleView vehicle,
        VehicleHealth vehicleHealth,
        ProjectileSystem projectiles,
        EffectSystem effects)
    {
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
        _projectileConfig = projectileConfig != null ? projectileConfig : throw new ArgumentNullException(nameof(projectileConfig));
        _pool = pool ?? throw new ArgumentNullException(nameof(pool));
        _vehicle = vehicle != null ? vehicle : throw new ArgumentNullException(nameof(vehicle));
        _vehicleHealth = vehicleHealth ?? throw new ArgumentNullException(nameof(vehicleHealth));
        _projectiles = projectiles ?? throw new ArgumentNullException(nameof(projectiles));
        _effects = effects ?? throw new ArgumentNullException(nameof(effects));
    }

    public void Spawn(Vector3 position)
    {
        EnemyView enemy = _pool.Spawn();

        enemy.Transform.SetPositionAndRotation(position, Quaternion.identity);
        enemy.Restore(_config.MaxHealth);
        enemy.State = EnemyState.Idle;
        enemy.IsFlinching = false;
        enemy.ActiveIndex = _active.Count;
        enemy.SetCollisionEnabled(true);
        enemy.PlayIdle();

        enemy.ProjectileHit += OnProjectileHit;
        enemy.VehicleHit += OnVehicleHit;

        _active.Add(enemy);
    }

    public void Tick()
    {
        float delta = Time.deltaTime;
        Vector3 vehiclePosition = _vehicle.Transform.position;
        float detectionSqr = _config.DetectionRange * _config.DetectionRange;

        for (int index = _active.Count - 1; index >= 0; index--)
        {
            EnemyView enemy = _active[index];

            if (enemy.State == EnemyState.Dying)
            {
                Decay(enemy, index, delta);
                continue;
            }

            Vector3 toVehicle = vehiclePosition - enemy.Transform.position;
            toVehicle.y = 0f;

            if (enemy.State == EnemyState.Idle)
            {
                if (toVehicle.sqrMagnitude > detectionSqr)
                {
                    Settle(enemy);
                    continue;
                }

                BeginChase(enemy);
            }

            Chase(enemy, toVehicle, delta);
        }
    }

    private void Settle(EnemyView enemy)
    {
        if (!enemy.IsFlinching) return;
        if (!enemy.IsHitFinished()) return;

        enemy.IsFlinching = false;
        enemy.PlayIdle();
    }

    private void BeginChase(EnemyView enemy)
    {
        enemy.State = EnemyState.Chasing;
        enemy.IsFlinching = false;
        enemy.PlayRun();
    }

    private void Decay(EnemyView enemy, int index, float delta)
    {
        enemy.EffectRemaining -= delta;

        if (enemy.EffectRemaining > 0f) return;
        if (!enemy.IsHitFinished()) return;

        ReleaseAt(index);
    }

    private void Chase(EnemyView enemy, Vector3 toVehicle, float delta)
    {
        if (toVehicle.sqrMagnitude <= Mathf.Epsilon) return;

        Vector3 direction = toVehicle.normalized;

        enemy.Transform.position += direction * (_config.MoveSpeed * delta);
        enemy.Transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnProjectileHit(EnemyView enemy, ProjectileView projectile)
    {
        _projectiles.Release(projectile);

        if (enemy.State == EnemyState.Dying) return;

        enemy.Health.TakeDamage(_projectileConfig.Damage);

        if (!enemy.Health.IsAlive)
        {
            BeginDeath(enemy);
            return;
        }

        if (enemy.State != EnemyState.Idle) return;
        if (enemy.IsFlinching) return;

        enemy.PlayHit();
        enemy.IsFlinching = true;
    }

    private void OnVehicleHit(EnemyView enemy)
    {
        if (enemy.State == EnemyState.Dying) return;

        _vehicleHealth.TakeDamage(_config.ContactDamage);

        ReleaseAt(enemy.ActiveIndex);
    }

    private void BeginDeath(EnemyView enemy)
    {
        enemy.State = EnemyState.Dying;
        enemy.SetCollisionEnabled(false);
        enemy.HideHealthBar();
        enemy.PlayHit();

        enemy.EffectRemaining = _effects.Play(enemy.Transform.position);
    }

    private void ReleaseAt(int index)
    {
        EnemyView enemy = _active[index];
        int last = _active.Count - 1;

        if (index != last)
        {
            _active[index] = _active[last];
            _active[index].ActiveIndex = index;
        }

        _active.RemoveAt(last);

        enemy.ProjectileHit -= OnProjectileHit;
        enemy.VehicleHit -= OnVehicleHit;
        enemy.ActiveIndex = -1;

        _pool.Despawn(enemy);
    }
}
