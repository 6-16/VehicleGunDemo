using UnityEngine;

namespace ProBase
{
    public interface ISimpleAppState : IAppState
    {
        Awaitable EnterAsync();
    }
}
