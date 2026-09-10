using UnityEngine;

namespace ProBase
{
    public class UiLayerView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _container;

        public Canvas Canvas => _canvas;
        public RectTransform Container => _container;
    }
}
