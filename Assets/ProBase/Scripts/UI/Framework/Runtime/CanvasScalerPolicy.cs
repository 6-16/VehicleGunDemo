using UnityEngine;
using UnityEngine.UI;

namespace ProBase
{
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasScalerPolicy : MonoBehaviour
    {
        private const float MatchWidth = 0f;
        private const float MatchHeight = 1f;

        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private Vector2 _referenceResolution = new Vector2(1080f, 1920f);

        private Vector2Int _appliedScreenSize;

        private void Awake()
        {
            Apply();
        }

        private void Update()
        {
            if (_appliedScreenSize.x == Screen.width && _appliedScreenSize.y == Screen.height) return;

            Apply();
        }

        private void Apply()
        {
            if (Screen.width <= 0 || Screen.height <= 0) return;

            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvasScaler.referenceResolution = _referenceResolution;
            _canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            _canvasScaler.matchWidthOrHeight = IsWiderThanReference() ? MatchHeight : MatchWidth;

            _appliedScreenSize = new Vector2Int(Screen.width, Screen.height);
        }

        private bool IsWiderThanReference()
        {
            float screenAspect = (float)Screen.width / Screen.height;
            float referenceAspect = _referenceResolution.x / _referenceResolution.y;

            return screenAspect > referenceAspect;
        }
    }
}
