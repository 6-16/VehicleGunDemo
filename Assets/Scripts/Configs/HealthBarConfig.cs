using UnityEngine;

[CreateAssetMenu(fileName = "HealthBarConfig", menuName = "Game/Health Bar Config")]
public class HealthBarConfig : ScriptableObject
{
    [SerializeField] private Color _fillColor = new Color(0.25f, 0.85f, 0.35f, 1f);
    [SerializeField] private Color _damageFillColor = new Color(0.9f, 0.2f, 0.2f, 1f);
    [SerializeField] private float _holdDuration = 0.25f;
    [SerializeField] private float _drainDuration = 0.35f;
    [SerializeField] private bool _hideWhenUndamaged;
    [SerializeField] private float _visibleDuration = 2f;
    [SerializeField] private bool _billboard;

    public Color FillColor => _fillColor;
    public Color DamageFillColor => _damageFillColor;
    public float HoldDuration => _holdDuration;
    public float DrainDuration => _drainDuration;
    public bool HideWhenUndamaged => _hideWhenUndamaged;
    public float VisibleDuration => _visibleDuration;
    public bool Billboard => _billboard;
}
