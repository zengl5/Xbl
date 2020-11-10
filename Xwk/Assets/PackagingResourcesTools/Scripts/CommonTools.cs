using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(CommonTools))]
public class EditorCommonTools : Editor
{
    private CommonTools m_Component = null;

    public bool m_bCommonToolsFoldout = true;

    public virtual void OnEnable()
    {
        m_Component = (CommonTools)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(20);

        m_bCommonToolsFoldout = EditorGUILayout.Foldout(m_bCommonToolsFoldout, "Common Tools");
        if (m_bCommonToolsFoldout)
        {
            if (GUILayout.Button("Clear PlayerPrefs", GUILayout.Height(30)))
                m_Component.ClearPlayerPrefs();
        }

        GUILayout.Space(50);
    }
}

public class CommonTools : MonoBehaviour
{
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}

#endif