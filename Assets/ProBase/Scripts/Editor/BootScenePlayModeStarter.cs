using UnityEditor;
using UnityEditor.SceneManagement;

namespace ProBase
{
    [InitializeOnLoad]
    public static class BootScenePlayModeStarter
    {
        static BootScenePlayModeStarter()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            Apply();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            if (change != PlayModeStateChange.ExitingEditMode) return;

            Apply();
        }

        private static void Apply()
        {
            SceneCatalog catalog = BuildSceneAssigner.FindCatalog();

            if (catalog == null || catalog.Boot == null || catalog.Boot.Asset == null) return;

            EditorSceneManager.playModeStartScene = catalog.Boot.Asset;
        }
    }
}
