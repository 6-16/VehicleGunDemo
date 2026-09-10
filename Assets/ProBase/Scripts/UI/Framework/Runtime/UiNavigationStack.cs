using System.Collections.Generic;
using System.Linq;

namespace ProBase
{
    public class UiNavigationStack
    {
        private readonly List<UiScreen> _entries = new List<UiScreen>();

        public int Count => _entries.Count;
        public bool IsEmpty => _entries.Count == 0;
        public UiScreen Top => _entries.Count > 0 ? _entries[_entries.Count - 1] : null;
        public IReadOnlyList<UiScreen> Entries => _entries;

        public void Push(UiScreen screen)
        {
            _entries.Add(screen);
        }

        public void Remove(UiScreen screen)
        {
            _entries.Remove(screen);
        }

        public bool Contains(UiScreen screen)
        {
            return _entries.Contains(screen);
        }

        public UiScreen Find<TScreen>() where TScreen : UiScreen
        {
            return _entries.FirstOrDefault(entry => entry is TScreen);
        }

        public UiScreen TopOf(UiLayer layer)
        {
            for (int index = _entries.Count - 1; index >= 0; index--)
            {
                if (_entries[index].Definition.Layer == layer) return _entries[index];
            }

            return null;
        }

        public IReadOnlyList<UiScreen> TakeAll()
        {
            List<UiScreen> taken = new List<UiScreen>(_entries);
            _entries.Clear();

            return taken;
        }

        public IReadOnlyList<UiScreen> TakeLayer(UiLayer layer)
        {
            List<UiScreen> taken = _entries.Where(entry => entry.Definition.Layer == layer).ToList();
            _entries.RemoveAll(entry => entry.Definition.Layer == layer);

            return taken;
        }
    }
}
