using System;
using ProBase;
using UnityEngine;
using Zenject;

public class RunResultPresenter : IInitializable, IDisposable
{
    private readonly IUiService _uiService;
    private readonly SignalBus _signalBus;

    public RunResultPresenter(IUiService uiService, SignalBus signalBus)
    {
        _uiService = uiService ?? throw new ArgumentNullException(nameof(uiService));
        _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
    }

    public void Initialize()
    {
        _signalBus.Subscribe<RunFinishedSignal>(OnRunFinished);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<RunFinishedSignal>(OnRunFinished);
    }

    private async void OnRunFinished(RunFinishedSignal signal)
    {
        try
        {
            await _uiService.ShowAsync<RunResultScreen, RunResultArgs>(new RunResultArgs(signal.Result));
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}
