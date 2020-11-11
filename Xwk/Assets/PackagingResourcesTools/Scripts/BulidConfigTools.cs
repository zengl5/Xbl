using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(BulidConfigTools))]
public class EditorBulidConfigTools : Editor
{
    private BulidConfigTools m_Component = null;

    public bool m_bBulidGameConfigFoldout = true;

    public bool m_bBulidCommonConfigFoldout = true;

    public virtual void OnEnable()
    {
        m_Component = (BulidConfigTools)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(20);

        m_bBulidGameConfigFoldout = EditorGUILayout.Foldout(m_bBulidGameConfigFoldout, "Build Game Config");
        if (m_bBulidGameConfigFoldout)
        {
            if (GUILayout.Button("Build Game Config", GUILayout.Height(30)))
                m_Component.BulidGameConfig();
        }

        GUILayout.Space(20);

        m_bBulidCommonConfigFoldout = EditorGUILayout.Foldout(m_bBulidCommonConfigFoldout, "Build Common Config");
        if (m_bBulidCommonConfigFoldout)
        {
            if (GUILayout.Button("Bulid Common Config", GUILayout.Height(30)))
                m_Component.BulidCommonConfig();
        }

        GUILayout.Space(20);
    }
}

public class BulidConfigTools : MonoBehaviour
{
    public void BulidGameConfig()
    {

        Debug.Log("BulidConfigTools BulidGameConfig Succeed!");

        AssetDatabase.Refresh();
    }

    public void BulidCommonConfig()
    {
        HttpRequestConfig.Save();
        LevelConfig.Save();
        LearningConfig.Save();
        FieldGuideConfig.Save();


        Debug.Log("BulidConfigTools BulidCommonConfig Succeed!");

        AssetDatabase.Refresh();
    }
}

#endif