using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Game/Projectile Config")]
public class ProjectileConfig : ScriptableObject
{
    [SerializeField] private ProjectileView _prefab;
    [SerializeField] private float _speed = 40f;
    [SerializeField] private int _damage = 25;
    [SerializeField] private float _lifetime = 2f;
    [SerializeField] private int _poolSize = 32;

    public ProjectileView Prefab => _prefab;
    public float Speed => _speed;
    public int Damage => _damage;
    public float Lifetime => _lifetime;
    public int PoolSize => _poolSize;
}
