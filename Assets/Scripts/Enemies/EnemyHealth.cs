using System;
using UnityEngine;

public class EnemyHealth : IHealth, IDamageable
{
    private int _max;
    private int _current;

    public bool IsAlive => _current > 0;
    public float Normalized => _max > 0 ? (float)_current / _max : 0f;

    public event Action Changed;

    public void Restore(int max)
    {
        _max = max;
        _current = max;

        Changed?.Invoke();
    }

    public void TakeDamage(int amount)
    {
        if (_current <= 0) return;

        _current = Mathf.Max(0, _current - amount);

        Changed?.Invoke();
    }
}
