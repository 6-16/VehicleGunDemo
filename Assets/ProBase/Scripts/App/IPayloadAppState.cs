using UnityEngine;

namespace ProBase
{
    public interface IPayloadAppState<TPayload> : IAppState
    {
        Awaitable EnterAsync(TPayload payload);
    }
}
