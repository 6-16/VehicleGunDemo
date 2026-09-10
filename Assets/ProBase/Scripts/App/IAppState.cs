using UnityEngine;

namespace ProBase
{
    public interface IAppState
    {
        Awaitable ExitAsync();
    }
}
