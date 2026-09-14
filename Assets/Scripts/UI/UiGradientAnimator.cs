using UnityEngine;
using UnityEngine.UI;

public class UiGradientAnimator : MonoBehaviour
{
    private static readonly int TopColorId = Shader.PropertyToID("_TopColor");
    private static readonly int BottomColorId = Shader.PropertyToID("_BottomColor");

    [SerializeField] private Image _image;
    [SerializeField] private GradientStep[] _steps;

    private Material _materialInstance;
    private float _elapsed;
    private int _currentIndex;

    private void Awake()
    {
        _materialInstance = new Material(_image.material);
        _image.material = _materialInstance;
    }

    private void OnDestroy()
    {
        if (_materialInstance == null) return;

        Destroy(_materialInstance);
    }

    private void Update()
    {
        if (_steps == null || _steps.Length < 2) return;

        Advance();

        GradientStep current = _steps[_currentIndex];
        GradientStep next = _steps[NextIndex(_currentIndex)];
        float progress = Mathf.SmoothStep(0f, 1f, _elapsed / current.Duration);

        _materialInstance.SetColor(TopColorId, Color.Lerp(current.Top, next.Top, progress));
        _materialInstance.SetColor(BottomColorId, Color.Lerp(current.Bottom, next.Bottom, progress));
    }

    private void Advance()
    {
        _elapsed += Time.unscaledDeltaTime;

        if (_elapsed < _steps[_currentIndex].Duration) return;

        _elapsed -= _steps[_currentIndex].Duration;
        _currentIndex = NextIndex(_currentIndex);
    }

    private int NextIndex(int index)
    {
        return (index + 1) % _steps.Length;
    }
}
