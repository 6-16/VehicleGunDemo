using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlan
{
    private const int PlacementAttempts = 12;

    private readonly List<SpawnPoint> _points = new List<SpawnPoint>();

    public IReadOnlyList<SpawnPoint> Points => _points;

    public SpawnPlan(LevelConfig level)
    {
        if (level == null) throw new ArgumentNullException(nameof(level));

        Build(level);
    }

    private void Build(LevelConfig level)
    {
        System.Random random = new System.Random();
        float distance = level.FirstSpawnDistance;

        while (distance < level.Distance)
        {
            AddGroup(level, random, distance);

            float step = Mathf.Lerp(level.SpawnIntervalRange.x, level.SpawnIntervalRange.y, (float)random.NextDouble());

            if (step <= 0f) return;

            distance += step;
        }

        _points.Sort(CompareByDistance);
    }

    private int CompareByDistance(SpawnPoint first, SpawnPoint second)
    {
        return first.Distance.CompareTo(second.Distance);
    }

    private void AddGroup(LevelConfig level, System.Random random, float distance)
    {
        int count = random.Next(level.GroupSizeRange.x, level.GroupSizeRange.y + 1);
        int groupStart = _points.Count;

        for (int index = 0; index < count; index++)
        {
            if (!TryPlace(level, random, distance, groupStart, out SpawnPoint point)) continue;

            _points.Add(point);
        }
    }

    private bool TryPlace(LevelConfig level, System.Random random, float distance, int groupStart, out SpawnPoint point)
    {
        float minimumSqr = level.MinSpacing * level.MinSpacing;

        for (int attempt = 0; attempt < PlacementAttempts; attempt++)
        {
            float lateral = Spread(random, level.LateralSpread);
            float depth = distance + Spread(random, level.GroupDepth);

            point = new SpawnPoint(depth, lateral);

            if (IsClear(point, groupStart, minimumSqr)) return true;
        }

        point = default;

        return false;
    }

    private float Spread(System.Random random, float extent)
    {
        return ((float)random.NextDouble() * 2f - 1f) * extent;
    }

    private bool IsClear(SpawnPoint candidate, int groupStart, float minimumSqr)
    {
        for (int index = groupStart; index < _points.Count; index++)
        {
            float lateral = _points[index].Lateral - candidate.Lateral;
            float depth = _points[index].Distance - candidate.Distance;

            if (lateral * lateral + depth * depth < minimumSqr) return false;
        }

        return true;
    }
}
