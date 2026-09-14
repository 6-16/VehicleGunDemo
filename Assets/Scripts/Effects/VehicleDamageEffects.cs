using System;
using UnityEngine;
using Zenject;

public class VehicleDamageEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem _fire;
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private ParticleSystem[] _popups;

    private Vector3[] _popupOrigins;
    private VehicleConfig _config;
    private IHealth _source;
    private float _shown;
    private bool _isBurning;

    [Inject]
    private void Construct(VehicleConfig config)
    {
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
    }

    private void Awake()
    {
        _popupOrigins = new Vector3[_popups.Length];

        for (int index = 0; index < _popups.Length; index++)
        {
            _popupOrigins[index] = _popups[index] != null
                ? _popups[index].transform.localPosition
                : Vector3.zero;
        }
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
    }

    private void OnDestroy()
    {
        Unbind();
    }

    private void OnHealthChanged()
    {
        float previous = _shown;

        _shown = _source.Normalized;

        UpdateFire();

        if (_shown >= previous) return;

        PlayPopup();

        if (_shown > 0f) return;

        Play(_explosion);
    }

    private void UpdateFire()
    {
        bool shouldBurn = _shown > 0f && _shown <= _config.LowHealthThreshold;

        if (shouldBurn == _isBurning) return;

        _isBurning = shouldBurn;

        if (shouldBurn)
        {
            Play(_fire);
            return;
        }

        if (_fire == null) return;

        _fire.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    private void PlayPopup()
    {
        if (_popups.Length == 0) return;

        int index = UnityEngine.Random.Range(0, _popups.Length);
        ParticleSystem popup = _popups[index];

        if (popup == null) return;

        popup.transform.localPosition = _popupOrigins[index] + UnityEngine.Random.insideUnitSphere * _config.PopupRadius;

        Play(popup);
    }

    private void Play(ParticleSystem effect)
    {
        if (effect == null) return;

        effect.gameObject.SetActive(true);
        effect.Play(true);
    }
}
