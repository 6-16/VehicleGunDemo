public readonly struct RunFinishedSignal
{
    public readonly RunResult Result;

    public RunFinishedSignal(RunResult result)
    {
        Result = result;
    }
}
