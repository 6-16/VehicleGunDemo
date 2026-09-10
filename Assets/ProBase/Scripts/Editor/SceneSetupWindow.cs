using UnityEditor;
using UnityEngine;

namespace ProBase
{
    [InitializeOnLoad]
    public class SceneSetupWindow : EditorWindow
    {
        private const string PromptShownKeyPrefix = "ProBase.SetupPromptShown.";
        private const float WindowWidth = 460f;
        private const float WindowHeight = 290f;
        private const float ButtonHeight = 28f;

        static SceneSetupWindow()
        {
            EditorApplication.delayCall += ShowOnFirstImport;
        }

        private static string PromptShownKey => PromptShownKeyPrefix + Application.dataPath;

        private static void ShowOnFirstImport()
        {
            if (Application.isBatchMode) return;
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (EditorPrefs.GetBool(PromptShownKey, false)) return;
            if (BuildSceneAssigner.FindCatalog() == null) return;

            EditorPrefs.SetBool(PromptShownKey, true);

            Open();
        }

        private static void Open()
        {
            SceneSetupWindow window = CreateInstance<SceneSetupWindow>();

            window.titleContent = new GUIContent("Project Setup");
            window.minSize = new Vector2(WindowWidth, WindowHeight);
            window.maxSize = new Vector2(WindowWidth, WindowHeight);

            window.ShowUtility();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Project setup", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            DrawScenesSection();
            EditorGUILayout.Space();
            DrawTextSection();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Close", GUILayout.Height(ButtonHeight)))
            {
                Close();
            }

            EditorGUILayout.Space();
        }

        private void DrawScenesSection()
        {
            EditorGUILayout.HelpBox(
                "Assign scenes to build settings.\n\n"
                + "This clears the current list and replaces it with Boot, Loading, MainMenu and Gameplay, "
                + "in that order.",
                MessageType.Info);

            if (GUILayout.Button("Assign Scenes", GUILayout.Height(ButtonHeight)))
            {
                BuildSceneAssigner.Assign();
            }
        }

        private void DrawTextSection()
        {
            if (TextMeshProEssentials.IsImported)
            {
                EditorGUILayout.HelpBox("TextMeshPro essential resources are imported.", MessageType.None);
                return;
            }

            EditorGUILayout.HelpBox(
                "TextMeshPro essential resources are missing. Screens that use text will render nothing until "
                + "they are imported.",
                MessageType.Warning);

            if (GUILayout.Button("Import TextMeshPro Essentials", GUILayout.Height(ButtonHeight)))
            {
                TextMeshProEssentials.Import();
            }
        }
    }
}
