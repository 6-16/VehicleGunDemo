using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelCatalog", menuName = "Game/Level Catalog")]
public class LevelCatalog : ScriptableObject
{
    [SerializeField] private List<LevelConfig> _levels = new List<LevelConfig>();

    public IReadOnlyList<LevelConfig> Levels => _levels;
    public LevelConfig First => _levels.Count > 0 ? _levels[0] : null;

    public int NumberOf(LevelConfig level)
    {
        return _levels.IndexOf(level) + 1;
    }
}
