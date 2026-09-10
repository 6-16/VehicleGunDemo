namespace ProBase
{
    public readonly struct ScreenClosedSignal
    {
        public readonly IUiScreen Screen;

        public ScreenClosedSignal(IUiScreen screen)
        {
            Screen = screen;
        }
    }
}
