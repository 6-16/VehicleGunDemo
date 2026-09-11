using UnityEngine;

[CreateAssetMenu(fileName = "VehicleConfig", menuName = "Game/Vehicle Config")]
public class VehicleConfig : ScriptableObject
{
    [SerializeField] private float _forwardSpeed = 12f;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private float _driftAmplitude = 1.5f;
    [SerializeField] private float _driftLength = 30f;
    [SerializeField] private float _driftMaxYawAngle = 8f;

    public float ForwardSpeed => _forwardSpeed;
    public int MaxHealth => _maxHealth;
    public float DriftAmplitude => _driftAmplitude;
    public float DriftLength => _driftLength;
    public float DriftMaxYawAngle => _driftMaxYawAngle;
}
