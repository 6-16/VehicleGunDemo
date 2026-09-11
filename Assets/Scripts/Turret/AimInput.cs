using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class AimInput : IInitializable, IDisposable
{
    private readonly InputActionReference _dragAction;
    private readonly InputActionReference _holdAction;

    public AimInput(InputActionReference dragAction, InputActionReference holdAction)
    {
        _dragAction = dragAction != null ? dragAction : throw new ArgumentNullException(nameof(dragAction));
        _holdAction = holdAction != null ? holdAction : throw new ArgumentNullException(nameof(holdAction));
    }

    public float HorizontalDrag => _holdAction.action.IsPressed() ? _dragAction.action.ReadValue<Vector2>().x : 0f;

    public void Initialize()
    {
        _dragAction.action.Enable();
        _holdAction.action.Enable();
    }

    public void Dispose()
    {
        _dragAction.action.Disable();
        _holdAction.action.Disable();
    }
}
