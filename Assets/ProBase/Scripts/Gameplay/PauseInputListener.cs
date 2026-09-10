using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ProBase
{
    public class PauseInputListener : IInitializable, IDisposable
    {
        private readonly InputActionReference _cancelAction;
        private readonly IUiService _uiService;

        public PauseInputListener(InputActionReference cancelAction, IUiService uiService)
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
                if (_uiService.IsOpen<PauseScreen>())
                {
                    await _uiService.BackAsync();
                    return;
                }

                await _uiService.ShowAsync<PauseScreen>();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}
