using UnityEngine;

public class ProjectileView : MonoBehaviour
{
    private Transform _transform;

    public Transform Transform => _transform;

    private void Awake()
    {
        _transform = transform;
    }
}
