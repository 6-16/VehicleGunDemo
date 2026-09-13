using System;
using UnityEngine;

public class DamageFlashView : MonoBehaviour
{
    [SerializeField] private DamageFlashConfig _config;
    [SerializeField] private Renderer[] _renderers;

    private Material[][] _original;
    private Material[][] _flash;
    private IHealth _source;
    private float _shown;
    private int _flashId;
    private bool _isFlashing;

    private void Awake()
    {
        CacheMaterials();
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
        _flashId++;
    }

    private void OnDisable()
    {
        _flashId++;

        Restore();
    }

    private void OnDestroy()
    {
        Unbind();
    }

    private void CacheMaterials()
    {
        _original = new Material[_renderers.Length][];
        _flash = new Material[_renderers.Length][];

        for (int index = 0; index < _renderers.Length; index++)
        {
            Material[] shared = _renderers[index].sharedMaterials;
            Material[] flash = new Material[shared.Length];

            for (int slot = 0; slot < flash.Length; slot++)
            {
                flash[slot] = _config.FlashMaterial;
            }

            _original[index] = shared;
            _flash[index] = flash;
        }
    }

    private void OnHealthChanged()
    {
        float previous = _shown;

        _shown = _source.Normalized;

        if (_shown >= previous) return;

        Flash();
    }

    private async void Flash()
    {
        int id = ++_flashId;

        try
        {
            await PulseAsync(id);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    private async Awaitable PulseAsync(int id)
    {
        for (int pulse = 0; pulse < _config.PulseCount; pulse++)
        {
            Apply(_flash);

            await HoldAsync(_config.OnDuration);

            if (id != _flashId) return;

            Restore();

            if (pulse + 1 == _config.PulseCount) return;

            await HoldAsync(_config.OffDuration);

            if (id != _flashId) return;
        }
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

    private void Apply(Material[][] materials)
    {
        for (int index = 0; index < _renderers.Length; index++)
        {
            _renderers[index].sharedMaterials = materials[index];
        }

        _isFlashing = true;
    }

    private void Restore()
    {
        if (!_isFlashing) return;

        for (int index = 0; index < _renderers.Length; index++)
        {
            _renderers[index].sharedMaterials = _original[index];
        }

        _isFlashing = false;
    }
}
