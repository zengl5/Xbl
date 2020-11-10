#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace Slate
{
    [CustomEditor(typeof(ActionClips.PlayNickName))]
    public class PlayNickOnInspector : ActionClipInspector<ActionClips.PlayNickName>
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (action.audioClip != null && GUILayout.Button("Set at Clip Length"))
            {
                action.length = action.audioClip.length;
            }
        }
    }
}

#endif