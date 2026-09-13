using UnityEngine;

[CreateAssetMenu(fileName = "DamageFlashConfig", menuName = "Game/Damage Flash Config")]
public class DamageFlashConfig : ScriptableObject
{
    [SerializeField] private Material _flashMaterial;
    [SerializeField] private int _pulseCount = 1;
    [SerializeField] private float _onDuration = 0.06f;
    [SerializeField] private float _offDuration = 0.06f;

    public Material FlashMaterial => _flashMaterial;
    public int PulseCount => _pulseCount;
    public float OnDuration => _onDuration;
    public float OffDuration => _offDuration;
}
