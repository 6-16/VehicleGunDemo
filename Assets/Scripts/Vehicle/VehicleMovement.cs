using System;
using UnityEngine;
using Zenject;

public class VehicleMovement : IInitializable, ITickable
{
    private const float HeadingSampleDistance = 2f;
    private const float SeedRange = 1000f;

    private readonly VehicleView _view;
    private readonly VehicleConfig _config;

    private Vector3 _origin;
    private float _seed;
    private bool _isMoving;
    private float _travelled;

    public float Travelled => _travelled;

    public VehicleMovement(VehicleView view, VehicleConfig config)
    {
        _view = view != null ? view : throw new ArgumentNullException(nameof(view));
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
    }

    public void Initialize()
    {
        _origin = _view.Transform.position;
        _seed = UnityEngine.Random.Range(0f, SeedRange);
    }

    public void Begin()
    {
        _isMoving = true;
    }

    public void Stop()
    {
        _isMoving = false;
    }

    public void Tick()
    {
        if (!_isMoving) return;

        _travelled += _config.ForwardSpeed * Time.deltaTime;

        float lateral = SampleLateral(_travelled);
        float ahead = SampleLateral(_travelled + HeadingSampleDistance);
        float heading = Mathf.Atan2(ahead - lateral, HeadingSampleDistance) * Mathf.Rad2Deg;
        float yaw = Mathf.Clamp(heading, -_config.DriftMaxYawAngle, _config.DriftMaxYawAngle);

        _view.Transform.position = new Vector3(_origin.x + lateral, _origin.y, _origin.z + _travelled);
        _view.Body.localRotation = Quaternion.Euler(0f, yaw, 0f);
    }

    private float SampleLateral(float distance)
    {
        if (_config.DriftLength <= 0f) return 0f;

        float noise = Mathf.PerlinNoise(distance / _config.DriftLength, _seed);

        return (noise * 2f - 1f) * _config.DriftAmplitude;
    }
}
