namespace ProBase
{
    public abstract class UiScreenWithArgs<TArgs> : UiScreen
    {
        private TArgs _args;

        protected TArgs Args => _args;

        public void SetArgs(TArgs args)
        {
            _args = args;
            OnArgsReceived();
        }

        protected virtual void OnArgsReceived()
        {
        }
    }
}
