using UnityEngine;

namespace ProBase
{
    public interface IUiScreen
    {
        UiScreenDefinition Definition { get; }
        GameObject GameObject { get; }

        void Bind(UiScreenDefinition definition);
        Awaitable OpenAsync();
        Awaitable CloseAsync();
        void SetVisible(bool isVisible);
        void SetInteractable(bool isInteractable);
    }
}
