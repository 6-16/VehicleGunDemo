namespace ProBase
{
    public readonly struct ConfirmArgs
    {
        public readonly string Message;

        public ConfirmArgs(string message)
        {
            Message = message;
        }
    }
}
