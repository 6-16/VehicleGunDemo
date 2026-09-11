using System;

namespace ProBase
{
    public class GameplayRequest
    {
        public readonly LevelConfig Level;

        public GameplayRequest(LevelConfig level)
        {
            Level = level != null ? level : throw new ArgumentNullException(nameof(level));
        }
    }
}
