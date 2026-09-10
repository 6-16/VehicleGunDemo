using System;
using UnityEngine;

namespace ProBase
{
    [CreateAssetMenu(fileName = "Screen", menuName = "UI/Screen Definition")]
    public class UiScreenDefinition : ScriptableObject
    {
        [SerializeField] private UiScreen _prefab;
        [SerializeField] private UiLayer _layer;
        [SerializeField] private int _sortPriority;
        [SerializeField] private bool _preload;
        [SerializeField] private bool _destroyOnClose;
        [SerializeField] private bool _blocksBack;

        public UiScreen Prefab => _prefab;
        public UiLayer Layer => _layer;
        public int SortPriority => _sortPriority;
        public bool Preload => _preload;
        public bool DestroyOnClose => _destroyOnClose;
        public bool BlocksBack => _blocksBack;

        public Type ScreenType => _prefab != null ? _prefab.GetType() : null;
    }
}
