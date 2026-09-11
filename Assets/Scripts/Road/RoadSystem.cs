using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RoadSystem : IInitializable, ITickable
{
    private readonly RoadConfig _config;
    private readonly VehicleView _vehicle;
    private readonly RoadSegmentView.Pool _pool;
    private readonly Queue<RoadSegmentView> _active = new Queue<RoadSegmentView>();

    private Vector3 _basePosition;
    private float _length;
    private float _nextZ;

    public RoadSystem(RoadConfig config, VehicleView vehicle, RoadSegmentView.Pool pool)
    {
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
        _vehicle = vehicle != null ? vehicle : throw new ArgumentNullException(nameof(vehicle));
        _pool = pool ?? throw new ArgumentNullException(nameof(pool));
    }

    public void Initialize()
    {
        RoadSegmentView first = _pool.Spawn();

        _length = first.Length;
        _basePosition = first.Transform.position;

        if (_length <= 0f)
        {
            throw new InvalidOperationException(
                $"'{_config.Prefab.name}' has no renderer to measure a segment length from.");
        }

        _nextZ = _vehicle.Transform.position.z - _length * _config.SegmentsBehind;

        Place(first);

        int remaining = _config.SegmentsAhead + _config.SegmentsBehind;

        for (int index = 0; index < remaining; index++)
        {
            Place(_pool.Spawn());
        }
    }

    public void Tick()
    {
        float vehicleZ = _vehicle.Transform.position.z;
        float trailingDistance = _length * _config.SegmentsBehind;

        while (vehicleZ - _active.Peek().Transform.position.z > trailingDistance)
        {
            Place(_active.Dequeue());
        }
    }

    private void Place(RoadSegmentView segment)
    {
        segment.Transform.position = new Vector3(_basePosition.x, _basePosition.y, _nextZ);

        _active.Enqueue(segment);
        _nextZ += _length;
    }
}
