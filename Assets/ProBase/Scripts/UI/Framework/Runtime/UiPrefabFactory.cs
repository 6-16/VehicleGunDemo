using System;
using UnityEngine;
using Zenject;

namespace ProBase
{
    public class UiPrefabFactory
    {
        private readonly DiContainer _container;

        public UiPrefabFactory(DiContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public TComponent Create<TComponent>(TComponent prefab, Transform parent) where TComponent : Component
        {
            if (prefab == null) throw new ArgumentNullException(nameof(prefab));

            return _container.InstantiatePrefabForComponent<TComponent>(prefab, parent);
        }
    }
}
