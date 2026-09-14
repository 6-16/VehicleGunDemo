using System;
using UnityEngine;
using Zenject;

public class VehicleView : MonoBehaviour
{
    [SerializeField] private Transform _body;
    [SerializeField] private TurretView _turret;
    [SerializeField] private DamageFlashView _damageFlash;
    [SerializeField] private DamageSquashView _damageSquash;

    private Transform _transform;

    public Transform Transform => _transform;
    public Transform Body => _body;
    public TurretView Turret => _turret;

    [Inject]
    private void Construct(VehicleHealth health)
    {
        if (health == null) throw new ArgumentNullException(nameof(health));

        _damageFlash.Bind(health);
        _damageSquash.Bind(health);
    }

    private void Awake()
    {
        _transform = transform;
    }
}
