using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private float _distance = 600f;
    [SerializeField] private EnemyConfig _enemy;
    [SerializeField] private float _firstSpawnDistance = 40f;
    [SerializeField] private float _spawnAheadDistance = 60f;
    [SerializeField] private float _despawnBehindDistance = 30f;
    [SerializeField] private Vector2 _spawnIntervalRange = new Vector2(12f, 22f);
    [SerializeField] private Vector2Int _groupSizeRange = new Vector2Int(1, 3);
    [SerializeField] private float _lateralSpread = 6f;

    public float Distance => _distance;
    public EnemyConfig Enemy => _enemy;
    public float FirstSpawnDistance => _firstSpawnDistance;
    public float SpawnAheadDistance => _spawnAheadDistance;
    public float DespawnBehindDistance => _despawnBehindDistance;
    public Vector2 SpawnIntervalRange => _spawnIntervalRange;
    public Vector2Int GroupSizeRange => _groupSizeRange;
    public float LateralSpread => _lateralSpread;
}
