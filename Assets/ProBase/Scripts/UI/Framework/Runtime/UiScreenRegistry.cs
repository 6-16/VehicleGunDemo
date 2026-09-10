using System;
using System.Collections.Generic;

namespace ProBase
{
    public class UiScreenRegistry
    {
        private readonly Dictionary<Type, UiScreenDefinition> _definitions = new Dictionary<Type, UiScreenDefinition>();
        private readonly UiSceneConfig _config;

        public UiSceneConfig Config => _config;

        public UiScreenRegistry(UiSceneConfig config)
        {
            _config = config != null ? config : throw new ArgumentNullException(nameof(config));

            foreach (UiScreenDefinition definition in config.Screens)
            {
                Register(definition);
            }
        }

        public UiScreenDefinition Get<TScreen>() where TScreen : UiScreen
        {
            return Get(typeof(TScreen));
        }

        public UiScreenDefinition Get(Type screenType)
        {
            if (_definitions.TryGetValue(screenType, out UiScreenDefinition definition)) return definition;

            throw new InvalidOperationException(
                $"No screen definition for '{screenType.Name}'. Add it to the scene's {nameof(UiSceneConfig)}.");
        }

        private void Register(UiScreenDefinition definition)
        {
            if (definition == null) throw new InvalidOperationException($"{_config.name} contains an empty screen entry.");

            Type screenType = definition.ScreenType;

            if (screenType == null)
            {
                throw new InvalidOperationException($"Screen definition '{definition.name}' has no prefab assigned.");
            }

            if (_definitions.ContainsKey(screenType))
            {
                throw new InvalidOperationException(
                    $"'{screenType.Name}' is registered twice in {_config.name}. Each screen type maps to one definition.");
            }

            _definitions.Add(screenType, definition);
        }
    }
}
