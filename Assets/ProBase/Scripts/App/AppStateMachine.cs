using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProBase
{
    public class AppStateMachine
    {
        private readonly Dictionary<Type, IAppState> _states = new Dictionary<Type, IAppState>();

        private IAppState _current;
        private bool _isTransitioning;

        public IAppState Current => _current;
        public bool IsTransitioning => _isTransitioning;

        public AppStateMachine(List<IAppState> states)
        {
            if (states == null) throw new ArgumentNullException(nameof(states));

            foreach (IAppState state in states)
            {
                _states.Add(state.GetType(), state);
            }
        }

        public async Awaitable EnterAsync<TState>() where TState : ISimpleAppState
        {
            if (_isTransitioning) return;

            TState next = Resolve<TState>();
            _isTransitioning = true;

            try
            {
                await ExitCurrentAsync();

                _current = next;
                await next.EnterAsync();
            }
            finally
            {
                _isTransitioning = false;
            }
        }

        public async Awaitable EnterAsync<TState, TPayload>(TPayload payload) where TState : IPayloadAppState<TPayload>
        {
            if (_isTransitioning) return;

            TState next = Resolve<TState>();
            _isTransitioning = true;

            try
            {
                await ExitCurrentAsync();

                _current = next;
                await next.EnterAsync(payload);
            }
            finally
            {
                _isTransitioning = false;
            }
        }

        public bool IsIn<TState>() where TState : IAppState
        {
            return _current is TState;
        }

        private async Awaitable ExitCurrentAsync()
        {
            if (_current == null) return;

            await _current.ExitAsync();
            _current = null;
        }

        private TState Resolve<TState>() where TState : IAppState
        {
            if (_states.TryGetValue(typeof(TState), out IAppState state)) return (TState)state;

            throw new InvalidOperationException(
                $"'{typeof(TState).Name}' is not bound. Add it to {nameof(ProjectInstaller)} as an {nameof(IAppState)}.");
        }
    }
}
