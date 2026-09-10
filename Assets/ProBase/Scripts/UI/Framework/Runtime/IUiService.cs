using UnityEngine;

namespace ProBase
{
    public interface IUiService
    {
        Awaitable<TScreen> ShowAsync<TScreen>() where TScreen : UiScreen;
        Awaitable<TScreen> ShowAsync<TScreen, TArgs>(TArgs args) where TScreen : UiScreenWithArgs<TArgs>;

        Awaitable<TResult> ShowForResultAsync<TScreen, TArgs, TResult>(TArgs args)
            where TScreen : UiScreenWithResult<TArgs, TResult>;

        Awaitable CloseAsync<TScreen>() where TScreen : UiScreen;
        Awaitable CloseTopAsync();
        Awaitable BackAsync();
        Awaitable CloseAllAsync();
        Awaitable ShowEntryScreenAsync();

        bool IsOpen<TScreen>() where TScreen : UiScreen;
        TScreen Get<TScreen>() where TScreen : UiScreen;
    }
}
