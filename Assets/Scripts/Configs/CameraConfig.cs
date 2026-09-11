using UnityEngine;

[CreateAssetMenu(fileName = "CameraConfig", menuName = "Game/Camera Config")]
public class CameraConfig : ScriptableObject
{
    [SerializeField] private Vector3 _spectateOffset = new Vector3(4.5f, 2f, -3.5f);
    [SerializeField] private Vector3 _spectateLookOffset = new Vector3(0f, 1f, 0f);
    [SerializeField] private Vector3 _chaseOffset = new Vector3(0f, 6f, -9f);
    [SerializeField] private Vector3 _chaseLookOffset = new Vector3(0f, 1.5f, 8f);
    [SerializeField] private float _blendDuration = 1.2f;

    public Vector3 SpectateOffset => _spectateOffset;
    public Vector3 SpectateLookOffset => _spectateLookOffset;
    public Vector3 ChaseOffset => _chaseOffset;
    public Vector3 ChaseLookOffset => _chaseLookOffset;
    public float BlendDuration => _blendDuration;
}
