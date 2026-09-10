namespace ProBase
{
    public readonly struct ScreenOpenedSignal
    {
        public readonly IUiScreen Screen;

        public ScreenOpenedSignal(IUiScreen screen)
        {
            Screen = screen;
        }
    }
}
