using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class BoneAnimOptimizeTool:AssetPostprocessor
{
    void OnPostprocessModel(GameObject g)
    {
       // Apply(g);
    }

    void Apply(GameObject g)
    {
        List<AnimationClip> animationClipList = new List<AnimationClip>(AnimationUtility.GetAnimationClips(g));
        if (animationClipList.Count == 0)
        {
            AnimationClip[] objectList = UnityEngine.Object.FindObjectsOfType(typeof(AnimationClip)) as AnimationClip[];
            animationClipList.AddRange(objectList);
        }

        foreach (AnimationClip theAnimation in animationClipList)
        {
            foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(theAnimation))
            {
                string name = theCurveBinding.propertyName.ToLower();
                if (name.Contains("scale"))
                {
                    AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
                }
            }
        }
    }
    public static bool CompressAnimationClip(UnityEngine.Object o)
    {
        string animationPath = AssetDatabase.GetAssetPath(o);
        try
        {
            //AnimationClip clip = GameObject.Instantiate(o) as AnimationClip;
            AnimationClip clip = o as AnimationClip;
            AnimationClipCurveData[] curves = null;
            curves = AnimationUtility.GetAllCurves(clip);
            Keyframe key;
            Keyframe[] keyFrames;
            for (int ii = 0; ii < curves.Length; ++ii)
            {
                AnimationClipCurveData curveDate = curves[ii];
                if (curveDate.curve == null || curveDate.curve.keys == null)
                {
                    //Debug.LogWarning(string.Format("AnimationClipCurveData {0} don't have curve; Animation name {1} ", curveDate, animationPath));
                    continue;
                }
                keyFrames = curveDate.curve.keys;
                for (int i = 0; i < keyFrames.Length; i++)
                {
                    key = keyFrames[i];
                    key.value = float.Parse(key.value.ToString("f3"));
                    key.inTangent = float.Parse(key.inTangent.ToString("f3"));
                    key.outTangent = float.Parse(key.outTangent.ToString("f3"));
                    keyFrames[i] = key;
                }
                curveDate.curve.keys = keyFrames;
                clip.SetCurve(curveDate.path, curveDate.type, curveDate.propertyName, curveDate.curve);
            }
            //AssetDatabase.CreateAsset(clip, animationPath);
            Debug.Log(string.Format("  CompressAnimationClip {0} Success !!!", animationPath));
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(string.Format("CompressAnimationClip Failed !!! animationPath : {0} error: {1}", animationPath, e));
            return false;
        }
    }
    [MenuItem("Window/AnimationTool/Optimize BoneAnim")]
    public static void OptimizeAnim()
    {
        var tObjArr = Selection.gameObjects;
        foreach (var obj in tObjArr)
        {
            RemoveAnimationCurve(obj);
        }
    }

    //移除scale
    public static void RemoveAnimationCurve(GameObject _obj)
    {
        List<AnimationClip> tAnimationClipList = new List<AnimationClip>(AnimationUtility.GetAnimationClips(_obj));
        if (tAnimationClipList.Count == 0)
        {
            AnimationClip[] tObjectList = UnityEngine.Object.FindObjectsOfType(typeof(AnimationClip)) as AnimationClip[];
            tAnimationClipList.AddRange(tObjectList);
        }

        foreach (AnimationClip animClip in tAnimationClipList)
        {
            foreach (EditorCurveBinding curveBinding in AnimationUtility.GetCurveBindings(animClip))
            {
                string tName = curveBinding.propertyName.ToLower();
                if (tName.Contains("scale"))
                {
                    AnimationUtility.SetEditorCurve(animClip, curveBinding, null);
                }
            }
            CompressAnimationClip(animClip);
        }
    }

    //压缩精度
    public static void CompressAnimationClip(AnimationClip _clip)
    {
        AnimationClipCurveData[] tCurveArr = AnimationUtility.GetAllCurves(_clip);
        Keyframe tKey;
        Keyframe[] tKeyFrameArr;
        for (int i = 0; i < tCurveArr.Length; ++i)
        {
            AnimationClipCurveData tCurveData = tCurveArr[i];
            if (tCurveData.curve == null || tCurveData.curve.keys == null)
            {
                continue;
            }
            tKeyFrameArr = tCurveData.curve.keys;
            for (int j = 0; j < tKeyFrameArr.Length; j++)
            {
                tKey = tKeyFrameArr[j];
                tKey.value = float.Parse(tKey.value.ToString("f3"));    //#.###
                tKey.inTangent = float.Parse(tKey.inTangent.ToString("f3"));
                tKey.outTangent = float.Parse(tKey.outTangent.ToString("f3"));
                tKeyFrameArr[j] = tKey;
            }
            tCurveData.curve.keys = tKeyFrameArr;
            _clip.SetCurve(tCurveData.path, tCurveData.type, tCurveData.propertyName, tCurveData.curve);
        }
    }
}