using System;
using UnityEngine;
using Zenject;

public class TurretAiming : ITickable
{
    private readonly TurretView _view;
    private readonly TurretConfig _config;
    private readonly AimInput _input;

    private float _angle;

    public float Angle => _angle;

    public TurretAiming(TurretView view, TurretConfig config, AimInput input)
    {
        _view = view != null ? view : throw new ArgumentNullException(nameof(view));
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
        _input = input ?? throw new ArgumentNullException(nameof(input));
    }

    public void Tick()
    {
        float drag = _input.HorizontalDrag;

        if (drag == 0f) return;

        float screenWidth = Screen.width;

        if (screenWidth <= 0f) return;

        float step = drag / screenWidth * _config.DegreesPerScreenWidth;
        float clamped = Mathf.Clamp(_angle + step, -_config.ConeHalfAngle, _config.ConeHalfAngle);

        if (clamped == _angle) return;

        _angle = clamped;
        _view.Pivot.localRotation = Quaternion.Euler(0f, _angle, 0f);
    }
}
