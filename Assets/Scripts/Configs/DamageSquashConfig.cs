using UnityEngine;

[CreateAssetMenu(fileName = "DamageSquashConfig", menuName = "Game/Damage Squash Config")]
public class DamageSquashConfig : ScriptableObject
{
    [SerializeField] private Vector3 _squashScale = new Vector3(1.04f, 0.92f, 1.04f);
    [SerializeField] private float _duration = 0.15f;
    [SerializeField] private AnimationCurve _curve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.3f, 1f),
        new Keyframe(1f, 0f));

    public Vector3 SquashScale => _squashScale;
    public float Duration => _duration;
    public AnimationCurve Curve => _curve;
}
