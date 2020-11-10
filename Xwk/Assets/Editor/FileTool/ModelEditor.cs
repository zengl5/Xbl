using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ModelEditor : Editor {
    [MenuItem("小伴龙Tool/设置选中所有动画循环Loop=true")]
    public static void SetAnimationLoop()
    {
        Object[] obj = Selection.objects;
        for (int i = 0; i < obj.Length; i++)
        {
            AnimationClip cp = obj[i] as AnimationClip;
            if (cp != null)
            {
                //cp.wrapMode = WrapMode.Loop;
                AnimationClipSettings clipSetting = AnimationUtility.GetAnimationClipSettings(cp);
                clipSetting.loopTime = true;
                AnimationUtility.SetAnimationClipSettings(cp, clipSetting);
                Debug.LogError(cp.wrapMode);
            }
            else
            {
                //Debug.LogError("当前不是动画");
            }

        }
    }
         [MenuItem("小伴龙Tool/设置选中所有动画Loop=false")]
    public static void SetAnimationLoopFalse()
    {
        Object[] obj = Selection.objects;
        for (int i = 0; i < obj.Length; i++)
        {
            AnimationClip cp = obj[i] as AnimationClip;
            if (cp != null)
            {
                AnimationClipSettings clipSetting = AnimationUtility.GetAnimationClipSettings(cp);
                clipSetting.loopTime = false;
                AnimationUtility.SetAnimationClipSettings(cp, clipSetting);
            }
            else
            {
                //Debug.LogError("当前不是动画");
            }
        }





        // AnimationClip[] cp = AnimationUtility.GetAnimationClips(obj);
        //  Debug.LogError(cp.Length);
        ////cp.wrapMode = WrapMode.Loop;
        //Debug.LogError(cp.name);
        //AnimationClipSettings clipSetting = AnimationUtility.GetAnimationClipSettings(cp);
        //clipSetting.loopTime = true;
        //Debug.LogError("设置动画循环");
        //AnimationUtility.SetAnimationClipSettings(cp, clipSetting);
        //SerializedObject serializedClip = new SerializedObject(clip);

        //clipSettings.loopTime = !clipSettings.loopTime;

        // serializedClip.ApplyModifiedProperties();

        //AnimationClipSettings acs = new AnimationClipSettings();
        //acs.additiveReferencePoseClip.wrapMode = WrapMode.Loop;
        //clipList = AnimationUtility.GetAnimationClips(obj);
        // Debug.LogError(clipList.Length);
        //clipList = AnimationUtility.SetAnimationClipSettings(obj, acs);
        //Debug.LogError(clipList.Length);
        //for(int i=0;i<obj.transform.childCount;i++)
        //{
        //    clipList = AnimationUtility.GetAnimationClips(obj.transform.GetChild(i).gameObject);
        //    AnimList.Add(clipList);
        //}
        //Debug.LogError(AnimList.Count);
        //for (int i = 0; i < AnimList.Count; i++)
        //{
        //    for(int j=0;j<AnimList[i].Length;j++)
        //    {
        //        AnimationClip clip = AnimList[i][j];
        //        clip.wrapMode = WrapMode.Loop;
        //        Debug.LogError(clip.name);
        //    }
        //}         
        //GameObject[] seletList = Selection.gameObjects;
        //for (int i = 0; i < seletList.Length; i++)
        //    Debug.LogError(seletList[i].gameObject.name);
    }
}
