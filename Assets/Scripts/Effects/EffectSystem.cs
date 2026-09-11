using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EffectSystem : ITickable
{
    private readonly DeathEffectPool _pool;
    private readonly List<ParticleSystem> _active = new List<ParticleSystem>();
    private readonly List<float> _remaining = new List<float>();

    public EffectSystem(DeathEffectPool pool)
    {
        _pool = pool ?? throw new ArgumentNullException(nameof(pool));
    }

    public float Play(Vector3 position)
    {
        ParticleSystem effect = _pool.Spawn();

        effect.transform.position = position;
        effect.Play(true);

        float lifetime = Lifetime(effect);

        _active.Add(effect);
        _remaining.Add(lifetime);

        return lifetime;
    }

    public void Tick()
    {
        float delta = Time.deltaTime;

        for (int index = _active.Count - 1; index >= 0; index--)
        {
            _remaining[index] -= delta;

            if (_remaining[index] > 0f) continue;

            Release(index);
        }
    }

    private float Lifetime(ParticleSystem effect)
    {
        ParticleSystem.MainModule main = effect.main;

        return main.duration + main.startLifetime.constantMax;
    }

    private void Release(int index)
    {
        ParticleSystem effect = _active[index];
        int last = _active.Count - 1;

        _active[index] = _active[last];
        _remaining[index] = _remaining[last];

        _active.RemoveAt(last);
        _remaining.RemoveAt(last);

        effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        _pool.Despawn(effect);
    }
}
