using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class StartupSceneLoader
{
    private const string ScenePath = "Assets/Scenes/BootstrapScene.unity";
    
    static StartupSceneLoader()
    {
        EditorApplication.delayCall += LoadDefaultScene; 
    }

    private static void LoadDefaultScene()
    {
        if (EditorSceneManager.GetActiveScene().path != ScenePath)
        {
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        }
    }
}
