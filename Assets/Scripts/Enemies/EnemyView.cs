using UnityEngine;

public class EnemyView : MonoBehaviour
{
    private Transform _transform;

    public Transform Transform => _transform;

    private void Awake()
    {
        _transform = transform;
    }
}
