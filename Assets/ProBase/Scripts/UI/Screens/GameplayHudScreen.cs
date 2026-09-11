using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ProBase
{
    public class GameplayHudScreen : UiScreen
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _goButton;
        [SerializeField] private GameObject _progressRoot;
        [SerializeField] private Image _progressFill;
        [SerializeField] private TMP_Text _levelLabel;

        private IUiService _uiService;
        private RunController _runController;
        private LevelCatalog _levelCatalog;
        private LevelConfig _level;
        private SignalBus _signalBus;
        private float _shownProgress;

        [Inject]
        private void Construct(
            IUiService uiService,
            RunController runController,
            LevelCatalog levelCatalog,
            LevelConfig level,
            SignalBus signalBus)
        {
            _uiService = uiService ?? throw new ArgumentNullException(nameof(uiService));
            _runController = runController ?? throw new ArgumentNullException(nameof(runController));
            _levelCatalog = levelCatalog != null ? levelCatalog : throw new ArgumentNullException(nameof(levelCatalog));
            _level = level != null ? level : throw new ArgumentNullException(nameof(level));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
        }

        protected override void OnOpening()
        {
            _progressRoot.SetActive(false);
            _goButton.gameObject.SetActive(true);
            _levelLabel.text = _levelCatalog.NumberOf(_level).ToString();
        }

        protected override void OnOpened()
        {
            _pauseButton.onClick.AddListener(OnPauseClicked);
            _goButton.onClick.AddListener(OnGoClicked);

            _signalBus.Subscribe<RunFinishedSignal>(OnRunFinished);
        }

        protected override void OnClosing()
        {
            _pauseButton.onClick.RemoveListener(OnPauseClicked);
            _goButton.onClick.RemoveListener(OnGoClicked);

            _signalBus.Unsubscribe<RunFinishedSignal>(OnRunFinished);
        }

        private void Update()
        {
            if (!_runController.IsRunning) return;

            float progress = _runController.NormalizedProgress;

            if (progress == _shownProgress) return;

            _shownProgress = progress;
            _progressFill.fillAmount = progress;
        }

        private void OnGoClicked()
        {
            _goButton.gameObject.SetActive(false);
            _progressRoot.SetActive(true);

            _runController.StartRun();
        }

        private void OnRunFinished(RunFinishedSignal signal)
        {
            _progressRoot.SetActive(false);
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
