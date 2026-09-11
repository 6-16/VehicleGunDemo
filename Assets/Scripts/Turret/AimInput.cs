using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class AimInput : IInitializable, IDisposable
{
    private readonly InputActionReference _dragAction;
    private readonly InputActionReference _holdAction;
    private readonly InputActionReference _rotateAction;

    public AimInput(InputActionReference dragAction, InputActionReference holdAction, InputActionReference rotateAction)
    {
        _dragAction = dragAction != null ? dragAction : throw new ArgumentNullException(nameof(dragAction));
        _holdAction = holdAction != null ? holdAction : throw new ArgumentNullException(nameof(holdAction));
        _rotateAction = rotateAction != null ? rotateAction : throw new ArgumentNullException(nameof(rotateAction));
    }

    public float HorizontalDrag => _holdAction.action.IsPressed() ? _dragAction.action.ReadValue<Vector2>().x : 0f;
    public float RotateAxis => _rotateAction.action.ReadValue<float>();

    public void Initialize()
    {
        _dragAction.action.Enable();
        _holdAction.action.Enable();
        _rotateAction.action.Enable();
    }

    public void Dispose()
    {
        _dragAction.action.Disable();
        _holdAction.action.Disable();
        _rotateAction.action.Disable();
    }
}
