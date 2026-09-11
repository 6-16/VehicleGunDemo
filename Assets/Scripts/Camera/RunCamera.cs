using System;
using UnityEngine;
using Zenject;

public class RunCamera : IInitializable, IDisposable
{
    private readonly Camera _camera;
    private readonly VehicleView _vehicle;
    private readonly CameraConfig _config;
    private readonly SignalBus _signalBus;

    private Transform _transform;

    public RunCamera(Camera camera, VehicleView vehicle, CameraConfig config, SignalBus signalBus)
    {
        _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
        _vehicle = vehicle != null ? vehicle : throw new ArgumentNullException(nameof(vehicle));
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
        _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
    }

    public void Initialize()
    {
        _transform = _camera.transform;
        _transform.SetParent(_vehicle.Transform, false);
        _transform.SetLocalPositionAndRotation(_config.SpectatePosition, Quaternion.Euler(_config.SpectateRotation));

        _signalBus.Subscribe<RunStartedSignal>(OnRunStarted);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<RunStartedSignal>(OnRunStarted);
    }

    private async void OnRunStarted()
    {
        try
        {
            await BlendToChaseAsync();
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    private async Awaitable BlendToChaseAsync()
    {
        Vector3 targetPosition = _config.ChasePosition;
        Quaternion targetRotation = Quaternion.Euler(_config.ChaseRotation);

        if (_config.BlendDuration <= 0f)
        {
            _transform.SetLocalPositionAndRotation(targetPosition, targetRotation);
            return;
        }

        Vector3 startPosition = _transform.localPosition;
        Quaternion startRotation = _transform.localRotation;
        float elapsed = 0f;

        while (elapsed < _config.BlendDuration)
        {
            elapsed += Time.deltaTime;

            float eased = Mathf.SmoothStep(0f, 1f, elapsed / _config.BlendDuration);

            _transform.SetLocalPositionAndRotation(
                Vector3.Lerp(startPosition, targetPosition, eased),
                Quaternion.Slerp(startRotation, targetRotation, eased));

            await Awaitable.NextFrameAsync();
        }

        _transform.SetLocalPositionAndRotation(targetPosition, targetRotation);
    }
}
