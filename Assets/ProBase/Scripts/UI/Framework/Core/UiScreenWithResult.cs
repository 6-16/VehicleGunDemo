using UnityEngine;

namespace ProBase
{
    public abstract class UiScreenWithResult<TArgs, TResult> : UiScreenWithArgs<TArgs>, IUiScreenResult
    {
        private AwaitableCompletionSource<TResult> _completionSource;

        public Awaitable<TResult> BeginResult()
        {
            _completionSource = new AwaitableCompletionSource<TResult>();

            return _completionSource.Awaitable;
        }

        public void CancelResult()
        {
            _completionSource?.TrySetResult(default);
        }

        protected void SetResult(TResult result)
        {
            _completionSource?.TrySetResult(result);
        }
    }
}
