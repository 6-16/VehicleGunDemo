using System;
using ProBase;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RunResultScreen : UiScreenWithArgs<RunResultArgs>
{
    [SerializeField] private GameObject _victoryRoot;
    [SerializeField] private GameObject _defeatRoot;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _menuButton;

    private AppStateMachine _stateMachine;
    private LevelConfig _level;

    [Inject]
    private void Construct(AppStateMachine stateMachine, LevelConfig level)
    {
        _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
        _level = level != null ? level : throw new ArgumentNullException(nameof(level));
    }

    protected override void OnArgsReceived()
    {
        bool isVictory = Args.Result == RunResult.Victory;

        _victoryRoot.SetActive(isVictory);
        _defeatRoot.SetActive(!isVictory);
    }

    protected override void OnOpened()
    {
        _retryButton.onClick.AddListener(OnRetryClicked);
        _menuButton.onClick.AddListener(OnMenuClicked);
    }

    protected override void OnClosing()
    {
        _retryButton.onClick.RemoveListener(OnRetryClicked);
        _menuButton.onClick.RemoveListener(OnMenuClicked);
    }

    private async void OnRetryClicked()
    {
        try
        {
            await _stateMachine.EnterAsync<GameplayState, GameplayRequest>(new GameplayRequest(_level));
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    private async void OnMenuClicked()
    {
        try
        {
            await _stateMachine.EnterAsync<MainMenuState>();
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}
