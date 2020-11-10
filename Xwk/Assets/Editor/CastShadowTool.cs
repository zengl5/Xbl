using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CastShadowTool  {

    // 在菜单来创建 选项 ， 点击该选项执行搜索代码
    [MenuItem("Assets/关闭所有的阴影")]
    static void CheckSceneSetting()
    {
        Object[] assets =    Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        for (int i=0;i< assets.Length;i++)
        {
            string path = AssetDatabase.GetAssetPath(assets[i]);
            if(!string.IsNullOrEmpty(path) && path.Contains("@mesh")){
                GameObject g = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if(g!=null)
                    Utility.SetMeshrenderInfo(g.transform);
            }
            
        }
    }
    [MenuItem("Assets/压缩动画片段")]
    static void ComprssionAnimtion()
    {
        Object[] assets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        for (int i = 0; i < assets.Length; i++)
        {
            string path = AssetDatabase.GetAssetPath(assets[i]);
            if (!string.IsNullOrEmpty(path) && (path.Contains("@anim") || path.Contains("@cam")))
            {
                GameObject g = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (g != null)
                {
                    //设置动画属性，压缩关键帧
                    //CompressionKeyframesImporter();
                    //精度压缩
                    CompressionAccuracy(g);
                }
            }
        }
    }
    private static void CompressionAccuracy(GameObject g)
    {
        if (g == null) return;
        List<AnimationClip> animationClipList = new List<AnimationClip>(AnimationUtility.GetAnimationClips(g));
        if (animationClipList.Count == 0)
        {
            AnimationClip[] objectList = UnityEngine.Object.FindObjectsOfType(typeof(AnimationClip)) as AnimationClip[];
            animationClipList.AddRange(objectList);
        }

        foreach (AnimationClip theAnimation in animationClipList)
        {
            try
            {
                //去除scale曲线
                //foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(theAnimation))
                //{
                //    string name = theCurveBinding.propertyName.ToLower();
                //    if (name.Contains("scale"))
                //    {
                //        AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
                //    }
                //}
                //浮点数精度压缩到f3
                AnimationClipCurveData[] curves = null;
                curves = AnimationUtility.GetAllCurves(theAnimation);
                Keyframe key;
                Keyframe[] keyFrames;
                for (int ii = 0; ii < curves.Length; ++ii)
                {
                    AnimationClipCurveData curveDate = curves[ii];
                    if (curveDate.curve == null || curveDate.curve.keys == null)
                    {
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
                    theAnimation.SetCurve(curveDate.path, curveDate.type, curveDate.propertyName, curveDate.curve);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(string.Format("CompressAnimationClip Failed !!! animationPath : {0} error: {1}", g.gameObject.name, e));
            }
        }

    }
 

}
