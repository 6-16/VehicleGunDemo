using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ProBase
{
    public class GameplayHudScreen : UiScreen
    {
        [SerializeField] private Button _pauseButton;

        private IUiService _uiService;

        [Inject]
        private void Construct(GameplayRequest request, IUiService uiService)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            _uiService = uiService ?? throw new ArgumentNullException(nameof(uiService));
        }

        protected override void OnOpened()
        {
            _pauseButton.onClick.AddListener(OnPauseClicked);
        }

        protected override void OnClosing()
        {
            _pauseButton.onClick.RemoveListener(OnPauseClicked);
        }

        private async void OnPauseClicked()
        {
            try
            {
                await _uiService.ShowAsync<PauseScreen>();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}
