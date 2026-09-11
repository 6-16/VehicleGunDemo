using UnityEngine;

[CreateAssetMenu(fileName = "RoadConfig", menuName = "Game/Road Config")]
public class RoadConfig : ScriptableObject
{
    [SerializeField] private RoadSegmentView _prefab;
    [SerializeField] private int _segmentsAhead = 5;
    [SerializeField] private int _segmentsBehind = 2;

    public RoadSegmentView Prefab => _prefab;
    public int SegmentsAhead => _segmentsAhead;
    public int SegmentsBehind => _segmentsBehind;
    public int PoolSize => _segmentsAhead + _segmentsBehind + 1;
}
