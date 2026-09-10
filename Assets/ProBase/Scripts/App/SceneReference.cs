using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProBase
{
    [Serializable]
    public class SceneReference
    {
        [SerializeField] private string _sceneName;

#if UNITY_EDITOR
        [SerializeField] private SceneAsset _sceneAsset;
#endif

        public string SceneName => _sceneName;
        public bool IsAssigned => !string.IsNullOrEmpty(_sceneName);

#if UNITY_EDITOR
        public SceneAsset Asset => _sceneAsset;

        public void SyncFromAsset()
        {
            _sceneName = _sceneAsset != null ? _sceneAsset.name : string.Empty;
        }
#endif
    }
}
