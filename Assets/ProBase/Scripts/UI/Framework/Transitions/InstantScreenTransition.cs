using UnityEngine;

namespace ProBase
{
    public class InstantScreenTransition : ScreenTransition
    {
        public override Awaitable PlayShowAsync(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 1f;

            return Awaitable.NextFrameAsync();
        }

        public override Awaitable PlayHideAsync(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0f;

            return Awaitable.NextFrameAsync();
        }
    }
}
