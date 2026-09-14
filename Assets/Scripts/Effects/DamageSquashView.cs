using System;
using UnityEngine;

public class DamageSquashView : MonoBehaviour
{
    [SerializeField] private DamageSquashConfig _config;
    [SerializeField] private Transform _target;

    private Vector3 _originalScale;
    private Vector3 _squashedScale;
    private IHealth _source;
    private float _shown;
    private int _punchId;

    private void Awake()
    {
        _originalScale = _target.localScale;
        _squashedScale = Vector3.Scale(_originalScale, _config.SquashScale);
    }

    public void Bind(IHealth source)
    {
        Unbind();

        _source = source ?? throw new ArgumentNullException(nameof(source));
        _source.Changed += OnHealthChanged;
        _shown = _source.Normalized;
    }

    public void Unbind()
    {
        if (_source == null) return;

        _source.Changed -= OnHealthChanged;
        _source = null;
        _punchId++;
    }

    private void OnDisable()
    {
        _punchId++;

        _target.localScale = _originalScale;
    }

    private void OnDestroy()
    {
        Unbind();
    }

    private void OnHealthChanged()
    {
        float previous = _shown;

        _shown = _source.Normalized;

        if (_shown >= previous) return;

        Punch();
    }

    private async void Punch()
    {
        int id = ++_punchId;

        try
        {
            await PunchAsync(id);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    private async Awaitable PunchAsync(int id)
    {
        float duration = _config.Duration;

        if (duration <= 0f) return;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float amount = _config.Curve.Evaluate(elapsed / duration);

            _target.localScale = Vector3.Lerp(_originalScale, _squashedScale, amount);

            await Awaitable.NextFrameAsync(destroyCancellationToken);

            if (id != _punchId) return;
        }

        _target.localScale = _originalScale;
    }
}
