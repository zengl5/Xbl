using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;
public class AutoSaveEditor : EditorWindow
{
    private int intervalScene;
    private bool autoSaveScene = true;
    private bool showMessage = true;
    private bool isStarted = false;
    private DateTime lastSaveTimeScene = DateTime.Now;
    private string projectPath;
    private string scenePath;

    [MenuItem("工具/设置自动保存场景")]
    static void Init()
    {
        AutoSaveEditor _AutoSaveWindow = (AutoSaveEditor)EditorWindow.GetWindow(typeof(AutoSaveEditor));
        _AutoSaveWindow.Show();
    }
     void OnGUI()
     {
         projectPath = Application.dataPath;

         GUILayout.Label("信息:", EditorStyles.boldLabel);
         EditorGUILayout.LabelField("保存场景路径:", "" + projectPath);
         EditorGUILayout.LabelField("保存场景名:", "" + scenePath);
         GUILayout.Label("选项:", EditorStyles.boldLabel);
         autoSaveScene = EditorGUILayout.BeginToggleGroup("是否自动保存", autoSaveScene);
         intervalScene = EditorGUILayout.IntSlider("时间间隔 (minutes)", intervalScene, 1, 10);
         if (isStarted)
         {
             EditorGUILayout.LabelField("最近一次保存时间:", "" + lastSaveTimeScene);
         }
         EditorGUILayout.EndToggleGroup();
         showMessage = EditorGUILayout.BeginToggleGroup("是否显示log", showMessage);
         EditorGUILayout.EndToggleGroup();

     }
     void Update()
     {
         projectPath = Application.dataPath;
         scenePath =  EditorSceneManager.GetActiveScene().path;//EditorApplication.currentScene;
         if (autoSaveScene)
         {
             if (DateTime.Now.Minute >= (lastSaveTimeScene.Minute + intervalScene) || DateTime.Now.Minute == 59 && DateTime.Now.Second == 59)
             {
                 saveScene();
             }
         }
         else
         {
             isStarted = false;
         }
     }
     void saveScene()
     {
         EditorApplication.SaveScene(scenePath);
         lastSaveTimeScene = DateTime.Now;
         isStarted = true;
         if (showMessage)
         {
             Debug.Log("AutoSave saved: " + scenePath + " on " + lastSaveTimeScene);
         }
         AutoSaveEditor repaintSaveWindow = (AutoSaveEditor)EditorWindow.GetWindow(typeof(AutoSaveEditor));
         repaintSaveWindow.Repaint();
     }
}
