using System;
using UnityEngine;
using Zenject;

public class EnemyView : MonoBehaviour
{
    private const int BaseLayer = 0;

    [SerializeField] private Animator _animator;
    [SerializeField] private Collider _collider;

    private Transform _transform;
    private int _idleHash;
    private int _runHash;
    private int _hitHash;

    public Transform Transform => _transform;
    public int Health { get; set; }
    public EnemyState State { get; set; }
    public bool IsFlinching { get; set; }
    public float EffectRemaining { get; set; }
    public int ActiveIndex { get; set; }

    public event Action<EnemyView, ProjectileView> ProjectileHit;
    public event Action<EnemyView> VehicleHit;

    private void Awake()
    {
        _transform = transform;
        _idleHash = Animator.StringToHash("Idle");
        _runHash = Animator.StringToHash("Run");
        _hitHash = Animator.StringToHash("GotHit");
    }

    public void PlayIdle()
    {
        _animator.Play(_idleHash);
    }

    public void PlayRun()
    {
        _animator.Play(_runHash);
    }

    public void PlayHit()
    {
        _animator.Play(_hitHash);
    }

    public bool IsHitFinished()
    {
        AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(BaseLayer);

        return state.shortNameHash == _hitHash && state.normalizedTime >= 1f;
    }

    public void SetCollisionEnabled(bool isEnabled)
    {
        _collider.enabled = isEnabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        ProjectileView projectile = other.GetComponentInParent<ProjectileView>();

        if (projectile != null)
        {
            ProjectileHit?.Invoke(this, projectile);
            return;
        }

        VehicleHit?.Invoke(this);
    }

    public class Pool : MonoMemoryPool<EnemyView>
    {
    }
}
