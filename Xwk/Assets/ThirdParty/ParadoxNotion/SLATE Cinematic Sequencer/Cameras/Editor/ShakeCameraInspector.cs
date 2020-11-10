using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Slate
{

	[CustomEditor(typeof(ShakeCamera))]
    public class ShakeCameraInspector : ActionClipInspector<ShakeCamera>
    {
        public override void OnInspectorGUI()
        { 
            
			base.ShowCommonInspector();

            base.OnInspectorGUI();
            EditorGUILayout.HelpBox(".", MessageType.Info);

            GUILayout.Space(10);
            action.shakeamp = EditorGUILayout.FloatField("晃动的幅度", action.shakeamp);
           // EditorGUILayout.HelpBox("表示相机的晃动幅度", MessageType.None);

            GUILayout.Space(10);
            action.shakeTime= EditorGUILayout.FloatField("每次晃动的时间间隔", action.shakeTime); 
            base.ShowAnimatableParameters();
        }
    }
}
