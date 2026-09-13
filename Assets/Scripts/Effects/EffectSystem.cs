using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EffectSystem : ITickable
{
    private readonly DeathEffectPool _pool;
    private readonly List<ParticleSystem> _active = new List<ParticleSystem>();
    private readonly List<float> _remaining = new List<float>();
    private readonly List<int> _tokens = new List<int>();

    private int _nextToken;

    public EffectSystem(DeathEffectPool pool)
    {
        _pool = pool ?? throw new ArgumentNullException(nameof(pool));
    }

    public int Play(Vector3 position, Vector3 direction)
    {
        ParticleSystem effect = _pool.Spawn();

        effect.transform.SetPositionAndRotation(position, Facing(direction));
        effect.Play(true);

        _nextToken++;

        _active.Add(effect);
        _remaining.Add(Lifetime(effect));
        _tokens.Add(_nextToken);

        return _nextToken;
    }

    public void Release(int token)
    {
        int index = _tokens.IndexOf(token);

        if (index < 0) return;

        ReleaseAt(index);
    }

    public void Tick()
    {
        float delta = Time.deltaTime;

        for (int index = _active.Count - 1; index >= 0; index--)
        {
            _remaining[index] -= delta;

            if (_remaining[index] > 0f) continue;

            ReleaseAt(index);
        }
    }

    private Quaternion Facing(Vector3 direction)
    {
        Vector3 flattened = new Vector3(direction.x, 0f, direction.z);

        return flattened.sqrMagnitude > Mathf.Epsilon
            ? Quaternion.LookRotation(flattened)
            : Quaternion.identity;
    }

    private float Lifetime(ParticleSystem effect)
    {
        ParticleSystem.MainModule main = effect.main;

        return main.duration + main.startLifetime.constantMax;
    }

    private void ReleaseAt(int index)
    {
        ParticleSystem effect = _active[index];
        int last = _active.Count - 1;

        _active[index] = _active[last];
        _remaining[index] = _remaining[last];
        _tokens[index] = _tokens[last];

        _active.RemoveAt(last);
        _remaining.RemoveAt(last);
        _tokens.RemoveAt(last);

        effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        _pool.Despawn(effect);
    }
}
