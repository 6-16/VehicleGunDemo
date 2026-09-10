using System;
using UnityEngine;
using Zenject;

namespace ProBase
{
    public class PauseService : IDisposable
    {
        private const float PausedTimeScale = 0f;
        private const float RunningTimeScale = 1f;

        private readonly SignalBus _signalBus;

        private bool _isPaused;

        public bool IsPaused => _isPaused;

        public PauseService(SignalBus signalBus)
        {
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
        }

        public void Pause()
        {
            SetPaused(true);
        }

        public void Resume()
        {
            SetPaused(false);
        }

        public void Dispose()
        {
            Time.timeScale = RunningTimeScale;
        }

        private void SetPaused(bool isPaused)
        {
            if (_isPaused == isPaused) return;

            _isPaused = isPaused;
            Time.timeScale = isPaused ? PausedTimeScale : RunningTimeScale;

            _signalBus.Fire(new PauseChangedSignal(isPaused));
        }
    }
}
