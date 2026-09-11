using UnityEngine;

[CreateAssetMenu(fileName = "VehicleConfig", menuName = "Game/Vehicle Config")]
public class VehicleConfig : ScriptableObject
{
    [SerializeField] private float _forwardSpeed = 12f;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private float _weaveAmplitude = 1.2f;
    [SerializeField] private float _weaveFrequency = 0.35f;
    [SerializeField] private float _weaveYawAngle = 4f;

    public float ForwardSpeed => _forwardSpeed;
    public int MaxHealth => _maxHealth;
    public float WeaveAmplitude => _weaveAmplitude;
    public float WeaveFrequency => _weaveFrequency;
    public float WeaveYawAngle => _weaveYawAngle;
}
