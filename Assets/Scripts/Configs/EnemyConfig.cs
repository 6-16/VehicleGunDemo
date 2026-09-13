using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Game/Enemy Config")]
public class EnemyConfig : ScriptableObject
{
    [SerializeField] private EnemyView _prefab;
    [SerializeField] private int _maxHealth = 50;
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _detectionRange = 18f;
    [SerializeField] private int _contactDamage = 10;
    [SerializeField] private float _deathDuration = 0.35f;
    [SerializeField] private int _poolSize = 24;
    [SerializeField] private ParticleSystem _deathEffect;
    [SerializeField] private int _deathEffectPoolSize = 8;

    public EnemyView Prefab => _prefab;
    public int MaxHealth => _maxHealth;
    public float MoveSpeed => _moveSpeed;
    public float DetectionRange => _detectionRange;
    public int ContactDamage => _contactDamage;
    public float DeathDuration => _deathDuration;
    public int PoolSize => _poolSize;
    public ParticleSystem DeathEffect => _deathEffect;
    public int DeathEffectPoolSize => _deathEffectPoolSize;
}
