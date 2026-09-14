using System;
using UnityEngine;

[Serializable]
public class GradientStep
{
    [SerializeField] private Color _top = Color.white;
    [SerializeField] private Color _bottom = Color.black;
    [SerializeField] [Min(0.01f)] private float _duration = 3f;

    public Color Top => _top;
    public Color Bottom => _bottom;
    public float Duration => _duration;
}
