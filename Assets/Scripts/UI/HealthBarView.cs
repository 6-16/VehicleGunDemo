using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] private HealthBarConfig _config;
    [SerializeField] private GameObject _root;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _damageFill;

    private Camera _camera;
    private Transform _rootTransform;
    private IHealth _source;
    private int _drainId;
    private int _visibilityId;

    [Inject]
    private void Construct(Camera camera)
    {
        _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
    }

    private void Awake()
    {
        _rootTransform = _root.transform;

        _fill.color = _config.FillColor;
        _damageFill.color = _config.DamageFillColor;
    }

    public void Bind(IHealth source)
    {
        Unbind();

        _source = source ?? throw new ArgumentNullException(nameof(source));
        _source.Changed += OnHealthChanged;

        Snap();
    }

    public void Unbind()
    {
        if (_source == null) return;

        _source.Changed -= OnHealthChanged;
        _source = null;
        _drainId++;
        _visibilityId++;
    }

    public void Show()
    {
        Snap();

        _root.SetActive(true);
    }

    public void Hide()
    {
        _visibilityId++;

        _root.SetActive(false);
    }

    private void LateUpdate()
    {
        if (!_config.Billboard) return;
        if (!_root.activeSelf) return;

        _rootTransform.rotation = _camera.transform.rotation;
    }

    private void OnDestroy()
    {
        Unbind();
    }

    private void Snap()
    {
        if (_source == null) return;

        _fill.fillAmount = _source.Normalized;
        _damageFill.fillAmount = _source.Normalized;
    }

    private async void OnHealthChanged()
    {
        float previous = _fill.fillAmount;
        float current = _source.Normalized;

        _fill.fillAmount = current;

        if (_config.HideWhenUndamaged && current < previous)
        {
            RevealBriefly();
        }

        if (_damageFill.fillAmount <= current)
        {
            _damageFill.fillAmount = current;
            return;
        }

        _drainId++;

        try
        {
            await DrainAsync(_drainId);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    private async void RevealBriefly()
    {
        _root.SetActive(true);

        int id = ++_visibilityId;

        try
        {
            await HoldAsync(_config.VisibleDuration);

            if (id != _visibilityId) return;

            _root.SetActive(false);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    private async Awaitable DrainAsync(int id)
    {
        await HoldAsync(_config.HoldDuration);

        if (id != _drainId) return;

        float duration = _config.DrainDuration;

        if (duration <= 0f)
        {
            _damageFill.fillAmount = _fill.fillAmount;
            return;
        }

        float from = _damageFill.fillAmount;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            _damageFill.fillAmount = Mathf.Lerp(from, _fill.fillAmount, elapsed / duration);

            await Awaitable.NextFrameAsync(destroyCancellationToken);

            if (id != _drainId) return;
        }

        _damageFill.fillAmount = _fill.fillAmount;
    }

    private async Awaitable HoldAsync(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            await Awaitable.NextFrameAsync(destroyCancellationToken);
        }
    }
}
