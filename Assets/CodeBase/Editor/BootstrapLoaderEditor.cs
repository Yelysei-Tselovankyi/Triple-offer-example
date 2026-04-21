using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
    [InitializeOnLoad]
    public static class BootstrapLoaderEditor
    {
        private const string BootstrapScenePath = "Assets/Scenes/BootstrapScene.unity";

        static BootstrapLoaderEditor()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                SceneAsset bootstrapScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(BootstrapScenePath);

                if (bootstrapScene == null)
                {
                    Debug.LogError($"[Bootstrap Loader] Cannot find scene at path: {BootstrapScenePath}");
                    return;
                }

                string currentSceneName = SceneManager.GetActiveScene().name;

                if (currentSceneName != bootstrapScene.name)
                {
                    Debug.Log($"[Bootstrap Loader] Setting Bootstrap as Play Mode start scene: <color=yellow>{bootstrapScene.name}</color>");

                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

                    EditorSceneManager.playModeStartScene = bootstrapScene;
                }
                else
                {
                    EditorSceneManager.playModeStartScene = null;
                }
            }
        }
    }
}
