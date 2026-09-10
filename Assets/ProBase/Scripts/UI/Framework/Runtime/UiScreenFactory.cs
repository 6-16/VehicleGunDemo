using System;
using UnityEngine;
using Zenject;

namespace ProBase
{
    public class UiScreenFactory
    {
        private readonly DiContainer _container;
        private readonly UiRootView _rootView;

        public UiScreenFactory(DiContainer container, UiRootView rootView)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _rootView = rootView != null ? rootView : throw new ArgumentNullException(nameof(rootView));
        }

        public UiScreen Create(UiScreenDefinition definition)
        {
            RectTransform container = _rootView.GetLayer(definition.Layer).Container;

            UiScreen screen = _container.InstantiatePrefabForComponent<UiScreen>(definition.Prefab, container);

            screen.Bind(definition);
            screen.SetVisible(false);
            screen.SetInteractable(false);
            screen.GameObject.SetActive(false);

            return screen;
        }

        public void ApplySortPriority(UiScreen screen)
        {
            RectTransform container = _rootView.GetLayer(screen.Definition.Layer).Container;
            int priority = screen.Definition.SortPriority;
            int index = 0;

            foreach (Transform sibling in container)
            {
                if (sibling == screen.transform) continue;

                UiScreen other = sibling.GetComponent<UiScreen>();

                if (other == null || other.Definition == null) continue;
                if (other.Definition.SortPriority > priority) break;

                index++;
            }

            screen.transform.SetSiblingIndex(index);
        }
    }
}
