using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlan
{
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
    }

    private void AddGroup(LevelConfig level, System.Random random, float distance)
    {
        int count = random.Next(level.GroupSizeRange.x, level.GroupSizeRange.y + 1);

        for (int index = 0; index < count; index++)
        {
            float lateral = ((float)random.NextDouble() * 2f - 1f) * level.LateralSpread;

            _points.Add(new SpawnPoint(distance, lateral));
        }
    }
}
