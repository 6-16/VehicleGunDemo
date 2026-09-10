using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ProBase
{
    public class LoadingScreen : UiScreen
    {
        [SerializeField] private Image _progressFill;

        private LoadingProgress _progress;
        private bool _isSubscribed;

        [Inject]
        private void Construct(LoadingProgress progress)
        {
            _progress = progress ?? throw new ArgumentNullException(nameof(progress));
        }

        protected override void OnOpening()
        {
            Subscribe();
            OnProgressChanged();
        }

        protected override void OnClosing()
        {
            Unsubscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            if (_isSubscribed) return;

            _progress.Changed += OnProgressChanged;
            _isSubscribed = true;
        }

        private void Unsubscribe()
        {
            if (!_isSubscribed) return;

            _progress.Changed -= OnProgressChanged;
            _isSubscribed = false;
        }

        private void OnProgressChanged()
        {
            _progressFill.fillAmount = _progress.Value;
        }
    }
}
