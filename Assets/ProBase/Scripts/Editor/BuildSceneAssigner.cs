using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProBase
{
    public static class BuildSceneAssigner
    {
        [MenuItem("ProBase/Assign Scenes", priority = 0)]
        public static void Assign()
        {
            SceneCatalog catalog = FindCatalog();

            if (catalog == null)
            {
                Debug.LogError($"No {nameof(SceneCatalog)} asset was found. Create one and assign its four scenes.");
                return;
            }

            if (!TryBuildList(catalog, out EditorBuildSettingsScene[] scenes)) return;

            EditorBuildSettings.scenes = scenes;

            Debug.Log($"Build settings replaced with {scenes.Length} scenes, starting at {catalog.Boot.SceneName}.");
        }

        public static SceneCatalog FindCatalog()
        {
            foreach (string guid in AssetDatabase.FindAssets($"t:{nameof(SceneCatalog)}"))
            {
                SceneCatalog catalog = AssetDatabase.LoadAssetAtPath<SceneCatalog>(AssetDatabase.GUIDToAssetPath(guid));

                if (catalog != null) return catalog;
            }

            return null;
        }

        private static bool TryBuildList(SceneCatalog catalog, out EditorBuildSettingsScene[] scenes)
        {
            scenes = null;

            SceneReference[] ordered = { catalog.Boot, catalog.Loading, catalog.MainMenu, catalog.Gameplay };
            List<EditorBuildSettingsScene> collected = new List<EditorBuildSettingsScene>();

            foreach (SceneReference reference in ordered)
            {
                if (reference == null || reference.Asset == null)
                {
                    Debug.LogError($"{nameof(SceneCatalog)} has an unassigned scene. Fill all four before assigning.");
                    return false;
                }

                collected.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(reference.Asset), true));
            }

            scenes = collected.ToArray();

            return true;
        }
    }
}
