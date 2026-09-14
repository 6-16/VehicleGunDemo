using UnityEngine;

[CreateAssetMenu(fileName = "CameraConfig", menuName = "Game/Camera Config")]
public class CameraConfig : ScriptableObject
{
    [SerializeField] private Vector3 _spectatePosition = new Vector3(4.5f, 2f, -3.5f);
    [SerializeField] private Vector3 _spectateRotation = new Vector3(8f, -52f, 0f);
    [SerializeField] private Vector3 _chasePosition = new Vector3(0f, 6f, -9f);
    [SerializeField] private Vector3 _chaseRotation = new Vector3(22f, 0f, 0f);
    [SerializeField] private float _blendDuration = 1.2f;
    [SerializeField] private float _shakeAmplitude = 0.15f;
    [SerializeField] private float _shakeDuration = 0.25f;

    public Vector3 SpectatePosition => _spectatePosition;
    public Vector3 SpectateRotation => _spectateRotation;
    public Vector3 ChasePosition => _chasePosition;
    public Vector3 ChaseRotation => _chaseRotation;
    public float BlendDuration => _blendDuration;
    public float ShakeAmplitude => _shakeAmplitude;
    public float ShakeDuration => _shakeDuration;
}
