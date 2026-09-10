using UnityEngine;

namespace ProBase
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaFitter : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Rect _appliedSafeArea;
        private Vector2Int _appliedScreenSize;

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
            Apply();
        }

        private void Update()
        {
            if (IsUpToDate()) return;

            Apply();
        }

        private bool IsUpToDate()
        {
            return _appliedSafeArea == Screen.safeArea
                   && _appliedScreenSize.x == Screen.width
                   && _appliedScreenSize.y == Screen.height;
        }

        private void Apply()
        {
            if (Screen.width <= 0 || Screen.height <= 0) return;

            Rect safeArea = Screen.safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;
            _rectTransform.offsetMin = Vector2.zero;
            _rectTransform.offsetMax = Vector2.zero;

            _appliedSafeArea = safeArea;
            _appliedScreenSize = new Vector2Int(Screen.width, Screen.height);
        }
    }
}
