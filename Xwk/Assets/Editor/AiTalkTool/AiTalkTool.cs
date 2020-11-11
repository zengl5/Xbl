using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class AiTalkTool :EditorWindow{

    [MenuItem("工具/智能对话配置")]
    static void ShowMyWindow()
    {
        AiTalkTool myWindow = EditorWindow.GetWindow<AiTalkTool>();
        myWindow.Show();
    }

    string name = "";
    void OnGUI()
    {
        GUILayout.Label("窗口");
        name = GUILayout.TextField(name);
        if (GUILayout.Button("创建一个空物体"))
        {
            GameObject go = new GameObject(name);
            Undo.RegisterCreatedObjectUndo(go, "Create GameObject");
        }                 
    }
}
