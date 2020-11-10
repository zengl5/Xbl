using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ResImporterSettingEditor : EditorWindow
{
    /// <summary>
    /// 创建、显示窗体
    /// </summary>    
    [MenuItem("工具/资源优化工具/收集设置格式配置文件")]
    private static void Init()
    {
        ResData.Load();
        LoopSetTexture();
    }
    [MenuItem("工具/资源优化工具/应用资源的设置格式")]
    private static void Apply()
    {
        ResData.Load();
        ApplyResConfig();
    }
    private static void ApplyResConfig()
    {
        int sum = 1;
        int id = 0;
        EditorApplication.update = delegate ()
        {
            bool isCancle = false;

            isCancle = EditorUtility.DisplayCancelableProgressBar("执行中...", "准备开始应用配置...", (float)id / sum);
            if (isCancle || id >= sum)
            {
                EditorUtility.ClearProgressBar();
                EditorApplication.update = null;
                return;
            }
            //加载json文件
            Object[] textures = GetSelectedTextures();
            sum = textures.Length;
            foreach (Texture2D texture in textures)
            {
                isCancle = EditorUtility.DisplayCancelableProgressBar("执行中...", "应用图片配置...", (float)id / sum);
                if (isCancle || id >= sum)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    return;
                }
                string path = AssetDatabase.GetAssetPath(texture);
                ResItem resItem =  ResData.FetchItem(path);
                if (resItem==null)
                {
                    Debug.LogError(path+"is not config data..");
                    continue;
                }
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

                TextureImporterSettings tis = new TextureImporterSettings();
#if UNITY_EDITOR_WIN
                TextureImporterPlatformSettings textureImporterSettings = textureImporter.GetPlatformTextureSettings("Android");
#elif UNITY_EDITOR_OSX
                TextureImporterPlatformSettings textureImporterSettings = textureImporter.GetPlatformTextureSettings("iOS");
#endif
                textureImporterSettings.overridden = true;
                textureImporter.ReadTextureSettings(tis);
                tis.textureType = (TextureImporterType)resItem.textureType;
                tis.readable = resItem.readEnable == 1 ? true: false;
                tis.mipmapEnabled = resItem.gengrateMipMaps == 1 ? true : false;
                textureImporterSettings.maxTextureSize = int.Parse(resItem.maxSize);
                textureImporterSettings.format = (TextureImporterFormat)resItem.format;
                textureImporter.SetPlatformTextureSettings(textureImporterSettings);
                textureImporter.SetTextureSettings(tis);
                 AssetDatabase.ImportAsset(path);
                  
                id++;
            }
            //添加声音的配置
            Object[] audioclips = GetSelecetedSound();
            sum += audioclips.Length;

            foreach (AudioClip audioclip in audioclips)
            {
                isCancle = EditorUtility.DisplayCancelableProgressBar("执行中...", "应用声音配置...", (float)id / sum);
                if (isCancle || id >= sum)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    return;
                }
                string path = AssetDatabase.GetAssetPath(audioclip);
                AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
                ResItem resItem = ResData.FetchItem(path);
                AudioImporterSampleSettings audioImporterSampleSettings = new AudioImporterSampleSettings();
                audioImporterSampleSettings.loadType = (AudioClipLoadType)resItem.loadtype;
                audioImporterSampleSettings.compressionFormat = (AudioCompressionFormat)resItem.compressionFormat;
                audioImporterSampleSettings.quality  = float.Parse(resItem.quality);
                 audioImporterSampleSettings.sampleRateSetting = (AudioSampleRateSetting)resItem.sampleRateSet;
#if UNITY_EDITOR_WIN
                audioImporter.SetOverrideSampleSettings("Android", audioImporterSampleSettings);

#elif UNITY_EDITOR_OSX
                audioImporter.SetOverrideSampleSettings("iOS", audioImporterSampleSettings);
#endif
                AssetDatabase.ImportAsset(path);
                id++;
            }
            if (isCancle || id >= sum)
            {
                EditorUtility.ClearProgressBar();
                EditorApplication.update = null;
            }
        };
    }

    /// <summary>
    /// 循环设置选择的贴图
    /// </summary>
    private static  void LoopSetTexture()
    {
        int sum = 1;
        int id = 0;
        EditorApplication.update = delegate ()
        {
            bool isCancle = false;

            isCancle =  EditorUtility.DisplayCancelableProgressBar("执行中...", "准备开始收集配置...", (float)id / sum);
            if (isCancle || id >= sum)
            {
                EditorUtility.ClearProgressBar();
                EditorApplication.update = null;
                return;
            }
            //加载json文件
            Object[] textures = GetSelectedTextures();
            sum = textures.Length;
            foreach (Texture2D texture in textures)
            {
                isCancle = EditorUtility.DisplayCancelableProgressBar("执行中...", "搜索图片配置...", (float)id / sum);
                if (isCancle || id >= sum)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    return;
                }
                string path = AssetDatabase.GetAssetPath(texture);
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
#if UNITY_EDITOR_WIN 
                TextureImporterPlatformSettings textureImporterSettings = textureImporter.GetPlatformTextureSettings("Android");
#elif UNITY_EDITOR_OSX
                TextureImporterPlatformSettings textureImporterSettings = textureImporter.GetPlatformTextureSettings("iOS");
#endif

                //TextureImporterSettings tis = new TextureImporterSettings();
                // texImporter.ReadTextureSettings(tis);
                // texImporter.SetTextureSettings(tis);
                //  AssetDatabase.ImportAsset(path);
                ResItem resItem = new ResItem();
                resItem.name = textureImporter.name;
                resItem.path = path;
                resItem.resType = "texture";
                resItem.textureType = (int)textureImporter.textureType;
                resItem.readEnable = textureImporter.isReadable==true?1:2;
                resItem.gengrateMipMaps = textureImporter.mipmapEnabled == true ? 1 : 2;
                if (textureImporterSettings.overridden)
                {
                    resItem.maxSize = textureImporterSettings.maxTextureSize.ToString();
                    resItem.format = (int)textureImporterSettings.format;
                }
                else
                {
                    resItem.maxSize = textureImporter.maxTextureSize.ToString();
                    resItem.format = (int)textureImporter.textureFormat;
                }
              
                ResData.AddRestItem(resItem);

                id++;
            }
            //添加声音的配置
            Object[] audioclips = GetSelecetedSound();
            sum += audioclips.Length;

            foreach (AudioClip audioclip in audioclips)
            {
                isCancle = EditorUtility.DisplayCancelableProgressBar("执行中...", "搜索声音配置...", (float)id / sum);
                if (isCancle || id >= sum)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    return;
                }
                string path = AssetDatabase.GetAssetPath(audioclip);
                AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
#if UNITY_EDITOR_WIN
                AudioImporterSampleSettings audioImporterSampleSettings = audioImporter.GetOverrideSampleSettings("Android");
#elif UNITY_EDITOR_OSX
            AudioImporterSampleSettings audioImporterSampleSettings = audioImporter.GetOverrideSampleSettings("iOS");
#endif
                ResItem resItem = new ResItem();
                resItem.name = audioImporter.name;
                resItem.path = path;
                resItem.resType = "audioclip";
                resItem.loadtype = (int)audioImporterSampleSettings.loadType;
                resItem.compressionFormat = (int)audioImporterSampleSettings.compressionFormat;
                resItem.quality = audioImporterSampleSettings.quality.ToString();
                resItem.sampleRateSet = (int)audioImporterSampleSettings.sampleRateSetting;
                ResData.AddRestItem(resItem);
                id++; 
            }
            if (isCancle || id >= sum)
            {
                EditorUtility.ClearProgressBar();
                EditorApplication.update = null;
            }
            ResData.Save();
        };
    }

    /// <summary>
    /// 获取选择的贴图
    /// </summary>
    /// <returns></returns>
    private static Object[] GetSelectedTextures()
    {
        return Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
    }
    //声音过滤
    private static Object[] GetSelecetedSound()
    {
        return Selection.GetFiltered(typeof(AudioClip), SelectionMode.DeepAssets);
    }
}
