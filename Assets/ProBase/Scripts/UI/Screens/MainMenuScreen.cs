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
        private LevelCatalog _levelCatalog;

        [Inject]
        private void Construct(AppStateMachine stateMachine, LevelCatalog levelCatalog)
        {
            _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
            _levelCatalog = levelCatalog != null ? levelCatalog : throw new ArgumentNullException(nameof(levelCatalog));
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
                await _stateMachine.EnterAsync<GameplayState, GameplayRequest>(new GameplayRequest(_levelCatalog.First));
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}
