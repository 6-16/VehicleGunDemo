using System;
using UnityEngine;
using Zenject;

public class VehicleHealth : IInitializable, IDamageable, IHealth
{
    private readonly VehicleConfig _config;

    private int _current;

    public int Current => _current;
    public float Normalized => _config.MaxHealth > 0 ? (float)_current / _config.MaxHealth : 0f;
    public bool IsAlive => _current > 0;

    public event Action Changed;
    public event Action Died;

    public VehicleHealth(VehicleConfig config)
    {
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
    }

    public void Initialize()
    {
        _current = _config.MaxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (_current <= 0) return;

        _current = Mathf.Max(0, _current - amount);

        Changed?.Invoke();

        if (_current > 0) return;

        Died?.Invoke();
    }
}
