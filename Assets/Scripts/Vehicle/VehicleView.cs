using UnityEngine;

public class VehicleView : MonoBehaviour
{
    [SerializeField] private Transform _body;
    [SerializeField] private TurretView _turret;

    private Transform _transform;

    public Transform Transform => _transform;
    public Transform Body => _body;
    public TurretView Turret => _turret;

    private void Awake()
    {
        _transform = transform;
    }
}
