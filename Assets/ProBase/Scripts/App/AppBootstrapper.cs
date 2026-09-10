using System;
using UnityEngine;
using Zenject;

namespace ProBase
{
    public class AppBootstrapper : IInitializable
    {
        private readonly AppStateMachine _stateMachine;

        public AppBootstrapper(AppStateMachine stateMachine)
        {
            _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
        }

        public async void Initialize()
        {
            try
            {
                await _stateMachine.EnterAsync<MainMenuState>();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}
