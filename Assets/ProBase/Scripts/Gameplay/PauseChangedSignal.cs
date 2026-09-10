namespace ProBase
{
    public readonly struct PauseChangedSignal
    {
        public readonly bool IsPaused;

        public PauseChangedSignal(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }
}
