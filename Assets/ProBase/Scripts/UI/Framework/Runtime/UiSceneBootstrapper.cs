using System;
using UnityEngine;
using Zenject;

namespace ProBase
{
    public class UiSceneBootstrapper : IInitializable
    {
        private readonly IUiService _uiService;

        public UiSceneBootstrapper(IUiService uiService)
        {
            _uiService = uiService ?? throw new ArgumentNullException(nameof(uiService));
        }

        public async void Initialize()
        {
            try
            {
                await _uiService.ShowEntryScreenAsync();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}
