#if  UNITY_EDITOR
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Slate
{
    [CustomEditor(typeof(PlayClick))]
    public class PlayClickInspector : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.HelpBox("支持选择点击的事件类型，以及对应的cutscene播放控制", MessageType.Info);

            GUILayout.Space(10);

        }
    }
}
#endif