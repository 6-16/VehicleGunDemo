using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ProBase
{
    public class MainMenuScreen : UiScreen
    {
        [SerializeField] private Button _playButton;

        private AppStateMachine _stateMachine;

        [Inject]
        private void Construct(AppStateMachine stateMachine)
        {
            _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
        }

        protected override void OnOpened()
        {
            _playButton.onClick.AddListener(OnPlayClicked);
        }

        protected override void OnClosing()
        {
            _playButton.onClick.RemoveListener(OnPlayClicked);
        }

        private async void OnPlayClicked()
        {
            try
            {
                await _stateMachine.EnterAsync<GameplayState, GameplayRequest>(new GameplayRequest());
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}
