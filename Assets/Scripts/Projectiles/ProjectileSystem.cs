using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectileSystem : ITickable
{
    private const int Inactive = -1;

    private readonly ProjectileConfig _config;
    private readonly ProjectileView.Pool _pool;
    private readonly List<ProjectileView> _active = new List<ProjectileView>();

    public ProjectileSystem(ProjectileConfig config, ProjectileView.Pool pool)
    {
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
        _pool = pool ?? throw new ArgumentNullException(nameof(pool));
    }

    public void Fire(Transform muzzle)
    {
        ProjectileView projectile = _pool.Spawn();

        projectile.Transform.SetPositionAndRotation(muzzle.position, muzzle.rotation);
        projectile.BeginTrail();
        projectile.Remaining = _config.Lifetime;
        projectile.ActiveIndex = _active.Count;

        _active.Add(projectile);
    }

    public void Release(ProjectileView projectile)
    {
        if (projectile.ActiveIndex == Inactive) return;

        ReleaseAt(projectile.ActiveIndex);
    }

    public void Tick()
    {
        float delta = Time.deltaTime;
        float step = _config.Speed * delta;

        for (int index = _active.Count - 1; index >= 0; index--)
        {
            ProjectileView projectile = _active[index];

            projectile.Remaining -= delta;

            if (projectile.Remaining <= 0f)
            {
                ReleaseAt(index);
                continue;
            }

            projectile.Transform.Translate(0f, 0f, step, Space.Self);
        }
    }

    private void ReleaseAt(int index)
    {
        ProjectileView projectile = _active[index];
        int last = _active.Count - 1;

        if (index != last)
        {
            _active[index] = _active[last];
            _active[index].ActiveIndex = index;
        }

        _active.RemoveAt(last);

        projectile.ActiveIndex = Inactive;
        projectile.EndTrail();

        _pool.Despawn(projectile);
    }
}
