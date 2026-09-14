using System;
using System.Threading;
using UnityEngine;
using Zenject;

public class RunCamera : IInitializable, IDisposable
{
    private readonly Camera _camera;
    private readonly VehicleView _vehicle;
    private readonly VehicleHealth _health;
    private readonly CameraConfig _config;
    private readonly SignalBus _signalBus;

    private readonly CancellationTokenSource _lifetime = new CancellationTokenSource();

    private Transform _transform;
    private float _shownHealth;
    private int _shakeId;
    private bool _isBlending;

    public RunCamera(
        Camera camera,
        VehicleView vehicle,
        VehicleHealth health,
        CameraConfig config,
        SignalBus signalBus)
    {
        _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
        _vehicle = vehicle != null ? vehicle : throw new ArgumentNullException(nameof(vehicle));
        _health = health ?? throw new ArgumentNullException(nameof(health));
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
        _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
    }

    public void Initialize()
    {
        _transform = _camera.transform;
        _transform.SetParent(_vehicle.Transform, false);
        _transform.SetLocalPositionAndRotation(_config.SpectatePosition, Quaternion.Euler(_config.SpectateRotation));

        _shownHealth = _health.Normalized;

        _signalBus.Subscribe<RunStartedSignal>(OnRunStarted);
        _health.Changed += OnHealthChanged;
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<RunStartedSignal>(OnRunStarted);
        _health.Changed -= OnHealthChanged;

        _lifetime.Cancel();
        _lifetime.Dispose();
    }

    private void OnHealthChanged()
    {
        float previous = _shownHealth;

        _shownHealth = _health.Normalized;

        if (_shownHealth >= previous) return;
        if (_isBlending) return;

        Shake();
    }

    private async void OnRunStarted()
    {
        try
        {
            await BlendToChaseAsync();
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    private async void Shake()
    {
        int id = ++_shakeId;

        try
        {
            await ShakeAsync(id);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    private async Awaitable ShakeAsync(int id)
    {
        float duration = _config.ShakeDuration;

        if (duration <= 0f) return;

        Vector3 anchor = _config.ChasePosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float falloff = 1f - elapsed / duration;
            Vector3 offset = UnityEngine.Random.insideUnitSphere * (_config.ShakeAmplitude * falloff);

            _transform.localPosition = anchor + offset;

            await Awaitable.NextFrameAsync(_lifetime.Token);

            if (id != _shakeId) return;
        }

        _transform.localPosition = anchor;
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

        _isBlending = true;

        try
        {
            while (elapsed < _config.BlendDuration)
            {
                elapsed += Time.deltaTime;

                float eased = Mathf.SmoothStep(0f, 1f, elapsed / _config.BlendDuration);

                _transform.SetLocalPositionAndRotation(
                    Vector3.Lerp(startPosition, targetPosition, eased),
                    Quaternion.Slerp(startRotation, targetRotation, eased));

                await Awaitable.NextFrameAsync(_lifetime.Token);
            }
        }
        finally
        {
            _isBlending = false;
        }

        _transform.SetLocalPositionAndRotation(targetPosition, targetRotation);
    }
}
