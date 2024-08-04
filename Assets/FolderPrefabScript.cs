using UnityEditor;
using UnityEngine;

public class FolderPrefabScript : Editor
{
    [MenuItem("Tools/Setup Project Folders")]
    public static void CreateFolders()
    {
        string[] folders = new string[]
        {
            "Assets/Animations",
            "Assets/Audio",
            "Assets/Editor",
            "Assets/Materials",
            "Assets/Prefabs",
            "Assets/Scenes",
            "Assets/Scripts",
            "Assets/Shaders",
            "Assets/Sprites",
            "Assets/Environment",
            "Assets/Environment/Scripts",
            "Assets/Environment/Prefabs",
            "Assets/Environment/Sprites",
            "Assets/Player",
            "Assets/Player/Scripts",
            "Assets/Player/Animations",
            "Assets/Player/Prefabs",
            "Assets/Player/Sprites",
            "Assets/Enemy",
            "Assets/Enemy/Scripts",
            "Assets/Enemy/Animations",
            "Assets/Enemy/Prefabs",
            "Assets/Enemy/Sprites",
            "Assets/UI",
            "Assets/UI/Scripts",
            "Assets/UI/Prefabs",
            "Assets/UI/Sprites"
        };

        foreach (string folder in folders)
        {
            if (!AssetDatabase.IsValidFolder(folder))
            {
                AssetDatabase.CreateFolder(folder.Substring(0, folder.LastIndexOf('/')), folder.Substring(folder.LastIndexOf('/') + 1));
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Project folders setup complete.");
    }
}
