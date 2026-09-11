using System;
using UnityEngine;
using Zenject;

public class EnemySpawner : IInitializable, ITickable
{
    private readonly LevelConfig _level;
    private readonly EnemyConfig _config;
    private readonly SpawnPlan _plan;
    private readonly EnemySystem _enemies;
    private readonly VehicleView _vehicle;

    private float _groundHeight;
    private int _next;

    public EnemySpawner(LevelConfig level, EnemyConfig config, SpawnPlan plan, EnemySystem enemies, VehicleView vehicle)
    {
        _level = level != null ? level : throw new ArgumentNullException(nameof(level));
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
        _plan = plan ?? throw new ArgumentNullException(nameof(plan));
        _enemies = enemies ?? throw new ArgumentNullException(nameof(enemies));
        _vehicle = vehicle != null ? vehicle : throw new ArgumentNullException(nameof(vehicle));
    }

    public void Initialize()
    {
        _groundHeight = _config.Prefab.transform.position.y;
    }

    public void Tick()
    {
        float reach = _vehicle.Transform.position.z + _level.SpawnAheadDistance;

        while (_next < _plan.Points.Count && _plan.Points[_next].Distance <= reach)
        {
            SpawnPoint point = _plan.Points[_next];

            _enemies.Spawn(new Vector3(point.Lateral, _groundHeight, point.Distance));

            _next++;
        }
    }
}
