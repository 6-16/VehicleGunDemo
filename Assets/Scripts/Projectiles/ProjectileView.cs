using UnityEngine;
using Zenject;

public class ProjectileView : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trail;

    private Transform _transform;

    public Transform Transform => _transform;
    public float Remaining { get; set; }
    public int ActiveIndex { get; set; }

    private void Awake()
    {
        _transform = transform;
    }

    public void BeginTrail()
    {
        if (_trail == null) return;

        _trail.Clear();
        _trail.emitting = true;
    }

    public void EndTrail()
    {
        if (_trail == null) return;

        _trail.emitting = false;
    }

    public class Pool : MonoMemoryPool<ProjectileView>
    {
    }
}
