using UnityEngine;

[CreateAssetMenu(fileName = "TurretConfig", menuName = "Game/Turret Config")]
public class TurretConfig : ScriptableObject
{
    [SerializeField] private float _degreesPerScreenWidth = 120f;
    [SerializeField] private float _keyRotationSpeed = 90f;
    [SerializeField] private float _coneHalfAngle = 45f;
    [SerializeField] private float _fireInterval = 0.25f;

    public float DegreesPerScreenWidth => _degreesPerScreenWidth;
    public float KeyRotationSpeed => _keyRotationSpeed;
    public float ConeHalfAngle => _coneHalfAngle;
    public float FireInterval => _fireInterval;
}
