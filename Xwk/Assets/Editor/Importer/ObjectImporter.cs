//using UnityEngine;
//using UnityEditor;
//using System.Collections.Generic;

//public class ObjectImporter : AssetPostprocessor
//{

//    private const ModelImporterAnimationCompression Compression = ModelImporterAnimationCompression.Optimal;
//    private const float AnimationPositionError = 0.2f;
//    private const float AnimationRotationError = 0.1f;


//    void OnPostprocessModel(GameObject g)
//    {
//        //Debug.Log("OnPostprocessModel+");
//        ModelImporter modelImporter = assetImporter as ModelImporter;
//        string modelName = modelImporter.assetPath.Substring(modelImporter.assetPath.LastIndexOf("/") + 1);
//        string extensionName = modelName.Substring(modelName.LastIndexOf("#") + 1);
//        if (string.IsNullOrEmpty(extensionName)) return;
//        if (extensionName.Contains("mesh"))
//        {
//            //设置大小
//            modelImporter.globalScale = 0.01f;
//            modelImporter.useFileScale = false;
//        }
//        if (extensionName.Contains("anim")|| extensionName.Contains("cam"))
//        {
//            modelImporter.globalScale = 0.01f;
//            modelImporter.useFileScale = false;
//            CompressionKeyframesImporter();
//            CompressionAccuracy(g);
//        }
//        //if (extensionName.Contains("cam"))
//        //{
//        //    NotCompressionKeyframesImporter();
//        //    CompressionAccuracy(g);
//        //}
//    }
//    private void NotCompressionKeyframesImporter()
//    {
//        var modelImporter = assetImporter as ModelImporter;
//        if (modelImporter == null)
//            return;

//        modelImporter.animationCompression = ModelImporterAnimationCompression.KeyframeReduction;
//        modelImporter.animationPositionError = 0.5f;
//        modelImporter.animationRotationError = 0.5f;
//        modelImporter.resampleCurves = true;
//    }


//    private void CompressionKeyframesImporter()
//    {
//        var modelImporter = assetImporter as ModelImporter;
//        if (modelImporter != null)
//        {
//            if (Compression != modelImporter.animationCompression)
//            {
//                modelImporter.animationCompression = Compression;
//                modelImporter.animationPositionError = AnimationPositionError;
//                modelImporter.animationRotationError = AnimationRotationError;
//                modelImporter.resampleCurves = false;
//            }
//            else
//            {
//                if (!Mathf.Approximately(modelImporter.animationPositionError, AnimationPositionError))
//                {
//                    modelImporter.animationPositionError = AnimationPositionError;
//                }
//                if (!Mathf.Approximately(modelImporter.animationRotationError, AnimationRotationError))
//                {
//                    modelImporter.animationRotationError = AnimationRotationError;
//                }
//                if (modelImporter.resampleCurves)
//                {
//                    modelImporter.resampleCurves = false;
//                }
//            }

//        }
//    }

//    private static void CompressionAccuracy(GameObject g)
//    {
//        if (g == null) return;
//        List<AnimationClip> animationClipList = new List<AnimationClip>(AnimationUtility.GetAnimationClips(g));
//        if (animationClipList.Count == 0)
//        {
//            AnimationClip[] objectList = UnityEngine.Object.FindObjectsOfType(typeof(AnimationClip)) as AnimationClip[];
//            animationClipList.AddRange(objectList);
//        }

//        foreach (AnimationClip theAnimation in animationClipList)
//        {
//            try
//            {
//                //去除scale曲线
//                //foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(theAnimation))
//                //{
//                //    string name = theCurveBinding.propertyName.ToLower();
//                //    if (name.Contains("scale"))
//                //    {
//                //        AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
//                //    }
//                //}
//                //浮点数精度压缩到f3
//                AnimationClipCurveData[] curves = null;
//                curves = AnimationUtility.GetAllCurves(theAnimation);
//                Keyframe key;
//                Keyframe[] keyFrames;
//                for (int ii = 0; ii < curves.Length; ++ii)
//                {
//                    AnimationClipCurveData curveDate = curves[ii];
//                    if (curveDate.curve == null || curveDate.curve.keys == null)
//                    {
//                        continue;
//                    }
//                    keyFrames = curveDate.curve.keys;
//                    for (int i = 0; i < keyFrames.Length; i++)
//                    {
//                        key = keyFrames[i];
//                        key.value = float.Parse(key.value.ToString("f3"));
//                        key.inTangent = float.Parse(key.inTangent.ToString("f3"));
//                        key.outTangent = float.Parse(key.outTangent.ToString("f3"));
//                        keyFrames[i] = key;
//                    }
//                    curveDate.curve.keys = keyFrames;
//                    theAnimation.SetCurve(curveDate.path, curveDate.type, curveDate.propertyName, curveDate.curve);
//                }
//            }
//            catch (System.Exception e)
//            {
//                Debug.LogError(string.Format("CompressAnimationClip Failed !!! animationPath : {0} error: {1}", g.gameObject.name, e));
//            }
//        }

//    }

//}
