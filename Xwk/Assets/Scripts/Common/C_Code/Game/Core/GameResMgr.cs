using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameResMgr : C_Singleton<GameResMgr>
{
    private AssetBundleManifest m_AssetBundleManifest = null;

    protected override void Init()
    {
        InitMainAssetBundleManifest();
    }

    public void InitMainAssetBundleManifest()
    {
        try
        {
            string filePath = LocalPath.LocalPackagingResources + LocalPath.PackagingResources;
            if (File.Exists(filePath))
            {
                AssetBundle manifestBundle = AssetBundle.LoadFromFile(filePath);
                if (manifestBundle != null)
                {
                    m_AssetBundleManifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");

                    manifestBundle.Unload(false);
                }
            }
        }
        catch (Exception e)
        {
            C_DebugHelper.LogWarning(" GameResMgr InitMainAssetBundleManifest : " + e);
        }
    }

    #region 加载资源

    //<param name="resPath">格式“common/sound/aa.ogg”或者“pubic/sound/bb.ogg”或者“aoe/prefab/cam1/gg.prefab”</param>
    public T LoadResource<T>(string resPath, bool isInstantiate = false, bool isForever = false) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(resPath))
        {
            C_DebugHelper.LogError(" 加载接口的respath is null");
            return null;
        }

        //拆分
        string resMoudle = "";
        System.Text.StringBuilder resName = new System.Text.StringBuilder();
        System.Text.StringBuilder moudleName = new System.Text.StringBuilder();
        if (resPath.Contains("/"))
        {
            resName.Append(resPath.Substring(resPath.LastIndexOf('/') + 1));//资源名字
            moudleName.Append(resPath.Substring(0, resPath.IndexOf('/')));//模块名字
            resMoudle = moudleName.ToString();
        }
        else
        {
            resName.Append(resPath);
        }

        if (moudleName.ToString().ToLower().Equals("common"))
            moudleName.Replace("common", "public");

        string typePath = "";
        if (resPath.Contains("/"))
            typePath = resPath.Substring(resPath.IndexOf('/') + 1);//去掉模块名

        if (!string.IsNullOrEmpty(typePath) && typePath.Contains("/"))
        {
            typePath = typePath.Substring(0, typePath.LastIndexOf('/'));//去掉包含的资源名字
            moudleName.Append("/").Append(typePath).Append("/");
        }

        return LoadResource<T>(resName.ToString(), resMoudle, "", moudleName.ToString(), isInstantiate, isForever);
    }

    public T LoadResource<T>(string resName, string resPath, string resType, bool isInstantiate) where T : UnityEngine.Object
    {
        return LoadResource<T>(resName, resPath, resType, "", isInstantiate, false);
    }

    public T LoadResource<T>(string resName, string resPath, string resType, string exResPath = "", bool isInstantiate = false, bool isForever = false) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(resName))
            return null;

        if (string.IsNullOrEmpty(resPath) && !string.IsNullOrEmpty(exResPath))
        {
            int index = exResPath.IndexOf('/');
            resPath = exResPath.Substring(0, (index == -1 ? exResPath.Length : index));
        }

        UnityEngine.Object tempObject = C_MonoSingleton<C_PoolMgr>.GetInstance().Spawn(resName, resType);
        if (tempObject != null)
            return tempObject as T;

        //关卡a是内置资源包，需要使用内置资源
        if (GameConfig.LocalResources ==0 )
        {
            string assetBundleName = GetAssetBundleName(resName, resPath, resType, exResPath);
            string assetBundleFilePath = GetAssetBundleFilePath(assetBundleName);

            T temp1 = C_ResMgr.GetAssetBundleFormCache<T>(resName, assetBundleFilePath, isInstantiate, isForever);
            if (temp1 != null)
                return temp1;

            if (m_AssetBundleManifest != null && Hash128.Parse("0") != m_AssetBundleManifest.GetAssetBundleHash(assetBundleName))
            {
                List<string> dpsList = new List<string>();

                if (!resName.Contains(".ogg"))
                    dpsList = GetAllDependencies(assetBundleName);
                //Debug.LogError("load bundle"+resName);
                T temp2 = C_ResMgr.LoadAssetBundle<T>(resName, assetBundleFilePath, dpsList, isInstantiate, isForever);
                if (temp2 != null)
                    return temp2;
            }
        }

        string resourcePath = "";
        if (string.IsNullOrEmpty(exResPath))
            resourcePath = "PackagingResources/" + resPath + "/" + resType + "/";
        else
            resourcePath = "PackagingResources/" + exResPath;

        T resObject = C_ResMgr.LoadResource<T>(resName, resourcePath);

        if (isInstantiate && resObject != null)
            resObject = GameObject.Instantiate(resObject);

        if (resObject == null)
            C_DebugHelper.LogError("-----------------------------------resName = " + resName + ", exResPath = " + exResPath + ", is null");

        return resObject;
    }

    public void AsyncLoadResource<T>(string resName, string resPath, string resType, Action<T> callback, string exResPath = "", bool isForever = false) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(resName) || string.IsNullOrEmpty(resPath))
        {
            callback(null);
            return;
        }

        UnityEngine.Object tempObject = C_MonoSingleton<C_PoolMgr>.GetInstance().Spawn(resName, resType);
        if (tempObject != null)
        {
            callback(tempObject as T);
            return;
        }

        if (GameConfig.LocalResources == 0)
        {
            string assetBundleName = GetAssetBundleName(resName, resPath, resType, exResPath);

            if (m_AssetBundleManifest != null && Hash128.Parse("0") != m_AssetBundleManifest.GetAssetBundleHash(assetBundleName))
            {
                List<string> dpsList = GetAllDependencies(assetBundleName);

                C_ResMgr.AsyncLoadAssetBundle(resName, assetBundleName, dpsList, callback, isForever);
                return;
            }
        }

        string resourcePath = "";
        if (string.IsNullOrEmpty(exResPath))
            resourcePath = "PackagingResources/" + resPath + "/" + resType + "/";
        else
            resourcePath = "PackagingResources/" + exResPath;

        C_ResMgr.AsyncLoadResource(resName, resourcePath, callback);
    }

    private string GetAssetBundleName(string resName, string resPath, string resType, string exResPath)
    {
        string assetBundleName = "";

        if (string.IsNullOrEmpty(exResPath))
            assetBundleName = resPath + "/" + resType + "/" + C_String.DeleteExpandedName(resName);
        else
            assetBundleName = exResPath + C_String.DeleteExpandedName(resName);

        assetBundleName = assetBundleName.ToLower();

        if (!assetBundleName.Contains(C_APP_CONST.AssetBundleExtension))
            assetBundleName += C_APP_CONST.AssetBundleExtension;

        return assetBundleName;
    }

    private string GetAssetBundleFilePath(string assetBundleName)
    {
        string downloadFilePath = LocalPath.LocalPackagingResources + assetBundleName;
        if (!File.Exists(downloadFilePath))
            return LocalPath.StreamingAssetsPackagingResources + assetBundleName;

        return downloadFilePath;
    }

    //递归找到所有依赖的AssetBundle
    private List<string> m_DependencieList = new List<string>();
    public List<string> GetAllDependencies(string assetBundleName)
    {
        m_DependencieList.Clear();

        RecursionDependencies(assetBundleName);

        for (int i = 0; i < m_DependencieList.Count; i++)
            m_DependencieList[i] = GetAssetBundleFilePath(m_DependencieList[i]);

        return m_DependencieList;
    }

    private void RecursionDependencies(string assetBundleName)
    {
        string[] names = m_AssetBundleManifest.GetAllDependencies(assetBundleName);

        for (int i = 0; i < names.Length; i++)
        {
            if (!m_DependencieList.Contains(names[i]))
            {
                m_DependencieList.Add(names[i]);

                RecursionDependencies(names[i]);
            }
        }
    }

    public void UnloadResource(string resName, string resPath, string resType, string exResPath = "")
    {
        if (string.IsNullOrEmpty(resName))
            return;

        if (string.IsNullOrEmpty(resPath) && !string.IsNullOrEmpty(exResPath))
        {
            int index = exResPath.IndexOf('/');

            resPath = exResPath.Substring(0, (index == -1 ? exResPath.Length : index));
        }

        //string assetBundleName = GetAssetBundleName(resName, resPath, resType, exResPath);

        //if (m_AssetBundleManifest != null && Hash128.Parse("0") != m_AssetBundleManifest.GetAssetBundleHash(assetBundleName))
        //{
        //    List<string> dpsList = GetAllDependencies(assetBundleName);
        //    dpsList.Add(GetAssetBundleFilePath(assetBundleName));

        //    C_ResMgr.UnloadAssetBundle(dpsList);
        //}
    }

    //--------------------------------------------------UI专用--------------------------------------------------
    public GameObject LoadResource_UI(string resName, string resPath = "")
    {
        if (string.IsNullOrEmpty(resName))
            return null;

        if (string.IsNullOrEmpty(resPath))
            resPath = "PackagingResources/c_framework/ui/package_ui_prefab/";

        return LoadResource_GameObject(resName, resPath);
    }

    public GameObject LoadResource_Effect(string resName, string resPath = "")
    {
        if (string.IsNullOrEmpty(resName))
            return null;

        if (string.IsNullOrEmpty(resPath))
            resPath = "PackagingResources/c_framework/effect/prefab/";

        return LoadResource_GameObject(resName, resPath);
    }

    public AudioClip LoadResource_Audio_Effect(string resName, string resPath = "")
    {
        if (string.IsNullOrEmpty(resName))
            return null;

        if (string.IsNullOrEmpty(resPath))
            resPath = "PackagingResources/c_framework/audio/sound_effect/";

        return C_ResMgr.LoadResource(resName, resPath) as AudioClip;
    }

    public AudioClip LoadResource_Audio_BGM(string resName, string resPath = "")
    {
        if (string.IsNullOrEmpty(resName))
            return null;

        if (string.IsNullOrEmpty(resPath))
            resPath = "PackagingResources/c_framework/audio/bgm/";

        return C_ResMgr.LoadResource(resName, resPath) as AudioClip;
    }

    public void AsyncLoadResource_Audio_BGM(string resName, Action<AudioClip> callback, string resPath = "")
    {
        if (callback == null)
            return;

        if (string.IsNullOrEmpty(resName))
        {
            callback(null);
            return;
        }

        if (string.IsNullOrEmpty(resPath))
            resPath = "PackagingResources/c_framework/audio/bgm/";

        C_ResMgr.AsyncLoadResource(resName, resPath, callback);
    }

    public AudioClip LoadResource_Audio_XBL(string resName, string resPath = "")
    {
        if (string.IsNullOrEmpty(resName))
            return null;

        if (string.IsNullOrEmpty(resPath))
            resPath = "PackagingResources/c_framework/audio/xbl/";

        return C_ResMgr.LoadResource(resName, resPath) as AudioClip;
    }

    //加载UI资源，内置
    public GameObject LoadResource_GameObject(string resName, string resPath)
    {
        if (string.IsNullOrEmpty(resName) || string.IsNullOrEmpty(resPath))
            return null;

        GameObject tempObject = C_ResMgr.LoadResource(resName, resPath) as GameObject;
        if (tempObject != null)
            tempObject = GameObject.Instantiate(tempObject);

        return tempObject;
    }

    //--------------------------------------------------缓存--------------------------------------------------
    /// <param name="resPath">格式“common/sound/aa.ogg”或者“pubic/sound/bb.ogg”或者“aoe/prefab/cam1/gg.prefab”</param>
    public T LoadAssetBundle_Cache<T>(string resPath, bool isInstantiate = false, bool isForever = false) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(resPath))
        {
            C_DebugHelper.LogError(" 加载接口的respath is null");
            return null;
        }

        //拆分
        string resMoudle = "";
        System.Text.StringBuilder resName = new System.Text.StringBuilder();
        System.Text.StringBuilder moudleName = new System.Text.StringBuilder();
        if (resPath.Contains("/"))
        {
            resName.Append(resPath.Substring(resPath.LastIndexOf('/') + 1));//资源名字
            moudleName.Append(resPath.Substring(0, resPath.IndexOf('/')));//模块名字
            resMoudle = moudleName.ToString();
        }
        else
        {
            resName.Append(resPath);
        }

        if (moudleName.ToString().ToLower().Equals("common"))
            moudleName.Replace("common", "public");

        string typePath = "";
        if (resPath.Contains("/"))
        {
            //去掉模块名
            typePath = resPath.Substring(resPath.IndexOf('/') + 1);
        }

        if (!string.IsNullOrEmpty(typePath) && typePath.Contains("/"))
        {
            //去掉包含的资源名字
            typePath = typePath.Substring(0, typePath.LastIndexOf('/'));
            moudleName.Append("/").Append(typePath).Append("/");
        }

        return LoadAssetBundle_Cache<T>(resName.ToString(), resMoudle, "", moudleName.ToString(), isInstantiate, isForever);
    }

    public T LoadAssetBundle_Cache<T>(string resName, string resPath, string resType, string exResPath = "", bool isInstantiate = false, bool isForever = false) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(resName))
            return null;

        if (string.IsNullOrEmpty(resPath) && !string.IsNullOrEmpty(exResPath))
        {
            int index = exResPath.IndexOf('/');

            resPath = exResPath.Substring(0, (index == -1 ? exResPath.Length : index));
        }

        UnityEngine.Object tempObject = C_MonoSingleton<C_PoolMgr>.GetInstance().Spawn(resName, resType);
        if (tempObject != null)
            return tempObject as T;

        string assetBundleName = GetAssetBundleName(resName, resPath, resType, exResPath);

        return C_ResMgr.LoadAssetBundleCache<T>(resName, GetAssetBundleFilePath(assetBundleName), isInstantiate, isForever);
    }

    public void LoadAssetBundle_CacheList(string type, List<string> assetBundleNameList)
    {
        if (string.IsNullOrEmpty(type) || assetBundleNameList.Count == 0)
            return;

        try
        {
            List<string> assetBundlePathList = new List<string>();

            for (int i = 0; i < assetBundleNameList.Count; i++)
                assetBundlePathList.Add(GetAssetBundleFilePath(assetBundleNameList[i]));

            C_ResMgr.LoadAssetBundleCacheList(type, assetBundlePathList);
        }
        catch (Exception e)
        {
            C_DebugHelper.LogError("LoadAssetBundleCacheList : " + e);
        }
    }

    public void UnloadAssetBundle_CacheList(string type)
    {
        C_ResMgr.UnloadAssetBundleCacheList(type);
    }
    public void ForceUnloadAssetBundle_CacheList(string type)
    {
        C_ResMgr.ForceUnloadAssetBundleCacheList(type);
    }
    public float GetProgressLoadAssetBundle_CacheList(string type)
    {
        return C_ResMgr.GetAssetBundleCacheListProgress(type);
    }
    public float GetProgressUnLoadAssetBundle_CacheList(string type)
    {
        return C_ResMgr.GetAssetUnBundleCacheListProgress(type);
    }

    #endregion



    #region 加载文件

    public void Save<T>(string resName, string resPath, T obj, C_EnumSaveSerializer enumSerializer = C_EnumSaveSerializer.SaveBinarySerializer)
    {
        resPath = LocalPath.HotUpdatePath + resPath;

        C_Save.Save<T>(resName, resPath, obj, enumSerializer);
    }

    public T Load<T>(string resName, string resPath)
    {
        return Load<T>(resName, resPath, default(T));
    }

    public T Load<T>(string resName, string resPath, T defaultValue)
    {
        string tempResPath = LocalPath.HotUpdatePath + resPath;
        if (!File.Exists(tempResPath + resName))
            tempResPath = LocalPath.StreamingAssetsPath + resPath;

        return C_Save.Load<T>(resName, tempResPath, defaultValue);
    }

    public string LoadString(string resName, string resPath)
    {
        string realResPath = LocalPath.HotUpdatePath + resPath;
        if (!File.Exists(realResPath + resName))
            realResPath = LocalPath.StreamingAssetsPath + resPath;

        string result = C_Save.LoadString(resName, realResPath);
        if (string.IsNullOrEmpty(result))
        {
            TextAsset textAsset = Resources.Load<TextAsset>(realResPath + C_String.DeleteExpandedName(resName));
            if (textAsset != null)
                result = textAsset.text;
        }

        return result;
    }

    public string LoadString(string resName, string resPath, string resType, string exResPath = "")
    {
        string tempResPath = "";
        if (string.IsNullOrEmpty(exResPath))
            tempResPath = LocalPath.PackagingResources + "/" + resPath + "/" + resType + "/";
        else
            tempResPath = LocalPath.PackagingResources + "/" + exResPath;

        string realResPath = LocalPath.HotUpdatePath + tempResPath;
        if (!File.Exists(tempResPath + resName))
            tempResPath = LocalPath.StreamingAssetsPath + tempResPath;

        string result = C_Save.LoadString(resName, realResPath);
        if (string.IsNullOrEmpty(result))
        {
            if (string.IsNullOrEmpty(exResPath))
                realResPath = "PackagingResources/" + resPath + "/" + resType + "/";
            else
                realResPath = "PackagingResources/" + exResPath;

            TextAsset textAsset = Resources.Load<TextAsset>(realResPath + C_String.DeleteExpandedName(resName));
            if (textAsset != null)
                result = textAsset.text;
        }

        return result;
    }

    public byte[] LoadByte(string fileResPath)
    {
        string realResPath = LocalPath.HotUpdatePath + LocalPath.PackagingResources + "/" + fileResPath;
        if (!File.Exists(realResPath))
            realResPath = LocalPath.StreamingAssetsPath + LocalPath.PackagingResources + "/" + fileResPath;

        return C_Save.LoadByte(C_String.GetFileName(fileResPath), C_String.GetSavePath(realResPath));
    }

    public void DeleteDirs(DirectoryInfo dirs)
    {
        if (dirs == null || (!dirs.Exists))
            return;

        DirectoryInfo[] subDir = dirs.GetDirectories();
        if (subDir != null)
        {
            for (int i = 0; i < subDir.Length; i++)
            {
                if (subDir[i] != null)
                    DeleteDirs(subDir[i]);
            }

            subDir = null;
        }

        FileInfo[] files = dirs.GetFiles();
        if (files != null)
        {
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] != null)
                {
                    Debug.Log("删除文件:" + files[i].FullName + "__over");
                    files[i].Delete();
                    files[i] = null;
                }
            }
            files = null;
        }

        Debug.Log("删除文件夹:" + dirs.FullName + "__over");
        dirs.Delete();
    }

    #endregion

}
