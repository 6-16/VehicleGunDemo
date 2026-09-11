using UnityEngine;

[CreateAssetMenu(fileName = "TurretConfig", menuName = "Game/Turret Config")]
public class TurretConfig : ScriptableObject
{
    [SerializeField] private float _aimSensitivity = 0.25f;
    [SerializeField] private float _coneHalfAngle = 45f;
    [SerializeField] private float _fireInterval = 0.25f;

    public float AimSensitivity => _aimSensitivity;
    public float ConeHalfAngle => _coneHalfAngle;
    public float FireInterval => _fireInterval;
}
