using UnityEngine;

namespace ProBase
{
    [CreateAssetMenu(fileName = "SceneCatalog", menuName = "App/Scene Catalog")]
    public class SceneCatalog : ScriptableObject
    {
        [SerializeField] private SceneReference _boot;
        [SerializeField] private SceneReference _mainMenu;
        [SerializeField] private SceneReference _loading;
        [SerializeField] private SceneReference _gameplay;

        public SceneReference Boot => _boot;
        public SceneReference MainMenu => _mainMenu;
        public SceneReference Loading => _loading;
        public SceneReference Gameplay => _gameplay;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _boot?.SyncFromAsset();
            _mainMenu?.SyncFromAsset();
            _loading?.SyncFromAsset();
            _gameplay?.SyncFromAsset();
        }
#endif
    }
}
