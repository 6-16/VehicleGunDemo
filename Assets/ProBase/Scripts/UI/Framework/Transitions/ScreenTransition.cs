using UnityEngine;

namespace ProBase
{
    public abstract class ScreenTransition : MonoBehaviour
    {
        public abstract Awaitable PlayShowAsync(CanvasGroup canvasGroup);
        public abstract Awaitable PlayHideAsync(CanvasGroup canvasGroup);
    }
}
