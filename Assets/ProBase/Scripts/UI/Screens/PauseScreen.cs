using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ProBase
{
    public class PauseScreen : UiScreen
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private string _quitMessage = "Quit to the main menu?";

        private PauseService _pauseService;
        private IUiService _uiService;
        private AppStateMachine _stateMachine;

        [Inject]
        private void Construct(PauseService pauseService, IUiService uiService, AppStateMachine stateMachine)
        {
            _pauseService = pauseService ?? throw new ArgumentNullException(nameof(pauseService));
            _uiService = uiService ?? throw new ArgumentNullException(nameof(uiService));
            _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
        }

        protected override void OnOpening()
        {
            _pauseService.Pause();
        }

        protected override void OnOpened()
        {
            _resumeButton.onClick.AddListener(OnResumeClicked);
            _quitButton.onClick.AddListener(OnQuitClicked);
        }

        protected override void OnClosing()
        {
            _resumeButton.onClick.RemoveListener(OnResumeClicked);
            _quitButton.onClick.RemoveListener(OnQuitClicked);
        }

        protected override void OnClosed()
        {
            _pauseService.Resume();
        }

        private async void OnResumeClicked()
        {
            try
            {
                await _uiService.CloseAsync<PauseScreen>();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        private async void OnQuitClicked()
        {
            try
            {
                bool isConfirmed = await _uiService.ShowForResultAsync<ConfirmScreen, ConfirmArgs, bool>(
                    new ConfirmArgs(_quitMessage));

                if (!isConfirmed) return;

                await _stateMachine.EnterAsync<MainMenuState>();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}
