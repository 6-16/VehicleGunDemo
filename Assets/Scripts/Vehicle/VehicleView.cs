using UnityEngine;

public class VehicleView : MonoBehaviour
{
    [SerializeField] private Transform _body;

    private Transform _transform;

    public Transform Transform => _transform;
    public Transform Body => _body;

    private void Awake()
    {
        _transform = transform;
    }
}
