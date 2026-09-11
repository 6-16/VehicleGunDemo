using UnityEngine;
using Zenject;

public class ProjectileView : MonoBehaviour
{
    private Transform _transform;

    public Transform Transform => _transform;
    public float Remaining { get; set; }
    public int ActiveIndex { get; set; }

    private void Awake()
    {
        _transform = transform;
    }

    public class Pool : MonoMemoryPool<ProjectileView>
    {
    }
}
