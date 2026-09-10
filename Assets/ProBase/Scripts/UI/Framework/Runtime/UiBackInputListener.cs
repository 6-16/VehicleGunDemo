using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ProBase
{
    public class UiBackInputListener : IInitializable, IDisposable
    {
        private readonly InputActionReference _cancelAction;
        private readonly IUiService _uiService;

        public UiBackInputListener(InputActionReference cancelAction, IUiService uiService)
        {
            _cancelAction = cancelAction != null ? cancelAction : throw new ArgumentNullException(nameof(cancelAction));
            _uiService = uiService ?? throw new ArgumentNullException(nameof(uiService));
        }

        public void Initialize()
        {
            _cancelAction.action.performed += OnCancelPerformed;
            _cancelAction.action.Enable();
        }

        public void Dispose()
        {
            _cancelAction.action.performed -= OnCancelPerformed;
            _cancelAction.action.Disable();
        }

        private async void OnCancelPerformed(InputAction.CallbackContext context)
        {
            try
            {
                await _uiService.BackAsync();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}
