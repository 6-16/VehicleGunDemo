using System;
using UnityEngine;

namespace ProBase
{
    public class UiRootView : MonoBehaviour
    {
        [SerializeField] private UiLayerView _screenLayer;
        [SerializeField] private UiLayerView _hudLayer;
        [SerializeField] private UiLayerView _overlayLayer;
        [SerializeField] private UiInputBlocker _inputBlocker;

        public UiInputBlocker InputBlocker => _inputBlocker;

        public UiLayerView GetLayer(UiLayer layer)
        {
            switch (layer)
            {
                case UiLayer.Screen: return _screenLayer;
                case UiLayer.Hud: return _hudLayer;
                case UiLayer.Overlay: return _overlayLayer;
                default: throw new ArgumentOutOfRangeException(nameof(layer), layer, "Unhandled UI layer.");
            }
        }
    }
}
