using System.IO;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    public static class ClearProgressEditor
    {
        private const string MenuPath = "Tools/Clear Player Progress";

        [MenuItem(MenuPath, priority = 100)]
        private static void ClearPlayerProgress()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "playerProgress.json");

            if (File.Exists(filePath))
            {
                bool confirmed = EditorUtility.DisplayDialog(
                    "Clear Player Progress",
                    "Are you sure you want to delete the player progress file?\n\n" +
                    "This action cannot be undone.\n\n" +
                    "File location:\n" + filePath,
                    "Yes, Delete",
                    "Cancel");

                if (confirmed)
                {
                    File.Delete(filePath);
                    Debug.Log($"<color=green>[ProgressService] Player progress file successfully deleted:</color> {filePath}");

                    EditorUtility.DisplayDialog(
                        "Success",
                        "Player progress file has been deleted.\n\n" +
                        "A new progress will be created the next time you run the game.",
                        "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "File Not Found",
                    $"Progress file does not exist:\n{filePath}",
                    "OK");
            }
        }

        [MenuItem(MenuPath, true)]
        private static bool ValidateClearPlayerProgress()
        {
            return true;
        }
    }
}
