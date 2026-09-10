using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ProBase
{
    public class UiService : IUiService, IInitializable
    {
        private readonly UiScreenRegistry _registry;
        private readonly UiScreenFactory _factory;
        private readonly UiNavigationStack _stack;
        private readonly UiRootView _rootView;
        private readonly SignalBus _signalBus;

        private readonly Dictionary<Type, UiScreen> _instances = new Dictionary<Type, UiScreen>();

        public UiService(
            UiScreenRegistry registry,
            UiScreenFactory factory,
            UiNavigationStack stack,
            UiRootView rootView,
            SignalBus signalBus)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _stack = stack ?? throw new ArgumentNullException(nameof(stack));
            _rootView = rootView != null ? rootView : throw new ArgumentNullException(nameof(rootView));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
        }

        public void Initialize()
        {
            foreach (UiScreenDefinition definition in _registry.Config.Screens)
            {
                if (definition.Preload)
                {
                    Resolve(definition);
                }
            }
        }

        public async Awaitable<TScreen> ShowAsync<TScreen>() where TScreen : UiScreen
        {
            UiScreenDefinition definition = _registry.Get<TScreen>();

            return (TScreen)await OpenAsync(definition);
        }

        public async Awaitable<TScreen> ShowAsync<TScreen, TArgs>(TArgs args) where TScreen : UiScreenWithArgs<TArgs>
        {
            UiScreenDefinition definition = _registry.Get<TScreen>();
            TScreen screen = (TScreen)Resolve(definition);

            screen.SetArgs(args);

            return (TScreen)await OpenAsync(definition);
        }

        public async Awaitable<TResult> ShowForResultAsync<TScreen, TArgs, TResult>(TArgs args)
            where TScreen : UiScreenWithResult<TArgs, TResult>
        {
            UiScreenDefinition definition = _registry.Get<TScreen>();
            TScreen screen = (TScreen)Resolve(definition);

            screen.SetArgs(args);

            Awaitable<TResult> pendingResult = screen.BeginResult();

            await OpenAsync(definition);

            TResult result = await pendingResult;

            if (_stack.Contains(screen))
            {
                await CloseScreenAsync(screen);
            }

            return result;
        }

        public Awaitable CloseAsync<TScreen>() where TScreen : UiScreen
        {
            UiScreen screen = _stack.Find<TScreen>();

            return screen != null ? CloseScreenAsync(screen) : Awaitable.NextFrameAsync();
        }

        public Awaitable CloseTopAsync()
        {
            UiScreen top = _stack.Top;

            return top != null ? CloseScreenAsync(top) : Awaitable.NextFrameAsync();
        }

        public Awaitable BackAsync()
        {
            UiScreen top = _stack.TopOf(UiLayer.Overlay);

            if (top == null) return Awaitable.NextFrameAsync();
            if (top.Definition.BlocksBack) return Awaitable.NextFrameAsync();

            return CloseScreenAsync(top);
        }

        public async Awaitable CloseAllAsync()
        {
            _rootView.InputBlocker.Block();

            foreach (UiScreen screen in _stack.TakeAll())
            {
                await CloseInstanceAsync(screen);
            }

            _rootView.InputBlocker.Unblock();
        }

        public async Awaitable ShowEntryScreenAsync()
        {
            UiScreenDefinition entry = _registry.Config.EntryScreen;

            if (entry == null) return;

            await OpenAsync(entry);
        }

        public bool IsOpen<TScreen>() where TScreen : UiScreen
        {
            return _stack.Find<TScreen>() != null;
        }

        public TScreen Get<TScreen>() where TScreen : UiScreen
        {
            return _instances.TryGetValue(typeof(TScreen), out UiScreen screen) ? (TScreen)screen : null;
        }

        private async Awaitable<UiScreen> OpenAsync(UiScreenDefinition definition)
        {
            _rootView.InputBlocker.Block();

            if (definition.Layer == UiLayer.Screen)
            {
                foreach (UiScreen open in _stack.TakeLayer(UiLayer.Screen))
                {
                    await CloseInstanceAsync(open);
                }
            }

            UiScreen screen = Resolve(definition);

            _stack.Push(screen);
            _factory.ApplySortPriority(screen);

            await screen.OpenAsync();

            _signalBus.Fire(new ScreenOpenedSignal(screen));
            _rootView.InputBlocker.Unblock();

            return screen;
        }

        private async Awaitable CloseScreenAsync(UiScreen screen)
        {
            _rootView.InputBlocker.Block();

            _stack.Remove(screen);
            await CloseInstanceAsync(screen);

            _rootView.InputBlocker.Unblock();
        }

        private async Awaitable CloseInstanceAsync(UiScreen screen)
        {
            await screen.CloseAsync();

            if (screen is IUiScreenResult resultScreen)
            {
                resultScreen.CancelResult();
            }

            _signalBus.Fire(new ScreenClosedSignal(screen));

            if (!screen.Definition.DestroyOnClose) return;

            _instances.Remove(screen.GetType());
            UnityEngine.Object.Destroy(screen.GameObject);
        }

        private UiScreen Resolve(UiScreenDefinition definition)
        {
            Type screenType = definition.ScreenType;

            if (_instances.TryGetValue(screenType, out UiScreen existing)) return existing;

            UiScreen created = _factory.Create(definition);
            _instances.Add(screenType, created);

            return created;
        }
    }
}
