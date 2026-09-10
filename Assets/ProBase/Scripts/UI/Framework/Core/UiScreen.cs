using UnityEngine;

namespace ProBase
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UiScreen : MonoBehaviour, IUiScreen
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private ScreenTransition _transition;

        private UiScreenDefinition _definition;

        public UiScreenDefinition Definition => _definition;
        public GameObject GameObject => gameObject;

        protected CanvasGroup CanvasGroup => _canvasGroup;

        public void Bind(UiScreenDefinition definition)
        {
            _definition = definition;
        }

        public async Awaitable OpenAsync()
        {
            gameObject.SetActive(true);
            SetInteractable(false);

            OnOpening();

            if (_transition != null)
            {
                await _transition.PlayShowAsync(_canvasGroup);
            }
            else
            {
                SetVisible(true);
            }

            SetInteractable(true);
            OnOpened();
        }

        public async Awaitable CloseAsync()
        {
            SetInteractable(false);
            OnClosing();

            if (_transition != null)
            {
                await _transition.PlayHideAsync(_canvasGroup);
            }
            else
            {
                SetVisible(false);
            }

            gameObject.SetActive(false);
            OnClosed();
        }

        public void SetVisible(bool isVisible)
        {
            _canvasGroup.alpha = isVisible ? 1f : 0f;
        }

        public void SetInteractable(bool isInteractable)
        {
            _canvasGroup.interactable = isInteractable;
            _canvasGroup.blocksRaycasts = isInteractable;
        }

        protected virtual void OnOpening()
        {
        }

        protected virtual void OnOpened()
        {
        }

        protected virtual void OnClosing()
        {
        }

        protected virtual void OnClosed()
        {
        }
    }
}
