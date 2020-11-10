using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// Add a `Scenes` menu to Unity editor for quick access to all scenes in project.
///
/// Generates/updates a `ScenesMenu.cs` file with the required menu annotations.
public static class SceneDditor
{
    // Path on filesystem (relative to Assets directory) to write menu command scripts.
    // This can be inside any "Editor" folder.
    static readonly string ScenesMenuPath = "Scene/zengll";

    [MenuItem("Scenes/Update This List")]
    public static void UpdateList()
    {
        string scenesMenuPath = Path.Combine(Application.dataPath, ScenesMenuPath);
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("// Generated file");
        stringBuilder.AppendLine("using UnityEditor;");
        stringBuilder.AppendLine("using UnityEditor.SceneManagement;");
        stringBuilder.AppendLine("public static class ScenesMenu");
        stringBuilder.AppendLine("{");

        foreach (string sceneGuid in AssetDatabase.FindAssets("t:Scene", new string[] { "Assets" }))
        {
            string sceneFilename = AssetDatabase.GUIDToAssetPath(sceneGuid);
            string sceneName = Path.GetFileNameWithoutExtension(sceneFilename);
            string methodName = sceneFilename.Replace('/', '_').Replace('\\', '_').Replace('.', '_').Replace('-', '_');
            stringBuilder.AppendLine(string.Format("    [MenuItem(\"Scenes/{0}\", priority = 10)]", sceneName));
            stringBuilder.AppendLine(string.Format("    public static void {0}() {{ ScenesMenuBuild.OpenScene(\"{1}\"); }}", methodName, sceneFilename));
        }
        stringBuilder.AppendLine("}");
        Directory.CreateDirectory(Path.GetDirectoryName(ScenesMenuPath));
        File.WriteAllText(scenesMenuPath, stringBuilder.ToString());
        AssetDatabase.Refresh();
    }

    public static void OpenScene(string filename)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene(filename);
    }
}