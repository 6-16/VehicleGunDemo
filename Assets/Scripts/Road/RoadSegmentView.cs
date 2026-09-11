using UnityEngine;
using Zenject;

public class RoadSegmentView : MonoBehaviour
{
    private Transform _transform;
    private float _length;

    public Transform Transform => _transform;
    public float Length => _length;

    private void Awake()
    {
        _transform = transform;
        _length = Measure();
    }

    private float Measure()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0) return 0f;

        Bounds bounds = renderers[0].bounds;

        for (int index = 1; index < renderers.Length; index++)
        {
            bounds.Encapsulate(renderers[index].bounds);
        }

        return bounds.size.z;
    }

    public class Pool : MonoMemoryPool<RoadSegmentView>
    {
    }
}
