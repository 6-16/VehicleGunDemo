using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ProBase
{
    public class GameplayHudScreen : UiScreen
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _goButton;

        private IUiService _uiService;
        private RunController _runController;

        [Inject]
        private void Construct(GameplayRequest request, IUiService uiService, RunController runController)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            _uiService = uiService ?? throw new ArgumentNullException(nameof(uiService));
            _runController = runController ?? throw new ArgumentNullException(nameof(runController));
        }

        protected override void OnOpened()
        {
            _pauseButton.onClick.AddListener(OnPauseClicked);
            _goButton.onClick.AddListener(OnGoClicked);
        }

        protected override void OnClosing()
        {
            _pauseButton.onClick.RemoveListener(OnPauseClicked);
            _goButton.onClick.RemoveListener(OnGoClicked);
        }

        private void OnGoClicked()
        {
            _goButton.gameObject.SetActive(false);
            _runController.StartRun();
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
