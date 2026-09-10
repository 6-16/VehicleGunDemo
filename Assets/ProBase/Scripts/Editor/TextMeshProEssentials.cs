using TMPro;
using UnityEditor;
using UnityEngine;

namespace ProBase
{
    public static class TextMeshProEssentials
    {
        private const string SettingsResourceName = "TMP Settings";

        public static bool IsImported => Resources.Load<TMP_Settings>(SettingsResourceName) != null;

        [MenuItem("ProBase/Import TextMeshPro Essentials", priority = 1)]
        public static void Import()
        {
            if (IsImported)
            {
                Debug.Log("TextMeshPro essential resources are already imported.");
                return;
            }

            TMP_PackageResourceImporter.ImportResources(true, false, false);
        }
    }
}
