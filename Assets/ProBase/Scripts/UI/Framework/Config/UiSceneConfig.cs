using System.Collections.Generic;
using UnityEngine;

namespace ProBase
{
    [CreateAssetMenu(fileName = "UiSceneConfig", menuName = "UI/Scene Config")]
    public class UiSceneConfig : ScriptableObject
    {
        [SerializeField] private List<UiScreenDefinition> _screens = new List<UiScreenDefinition>();
        [SerializeField] private UiScreenDefinition _entryScreen;

        public IReadOnlyList<UiScreenDefinition> Screens => _screens;
        public UiScreenDefinition EntryScreen => _entryScreen;
    }
}
