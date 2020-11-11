using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ABResMgr : C_Singleton<ABResMgr>
{    
    public void LoadAsyncAssetBundle(string path,string type)
    {
        if (!DevelepManager.UseInternalResource)
            AssetManager.Instance.LoadAsyncAssetBundle(path, type);
    }
    public void LoadAsyncAssetBundle(BundleConfig config)
    {
        if(!DevelepManager.UseInternalResource)
        AssetManager.Instance.LoadAsyncAssetBundle(config.pathArray, config.typeArray);
    }
    public void UnLoadAssetBundle(BundleConfig config)
    {
        if (!DevelepManager.UseInternalResource)
        {
            for (int i = 0; i < config.typeArray.Length; i++)
            {
                AssetManager.Instance.UnLoadAssetBundle(config.typeArray[i]);
                if(Application.isEditor)
                {
                    Debug.LogError("卸载bundle:" + config.typeArray[i]);
                }
            }
        }           
    }
    public void UnLoadAssetBundle(string type)
    {
        AssetManager.Instance.UnLoadAssetBundle(type);
    }
    //读取Bundle进度条
    public float GetLoadProgress()
    {
        return AssetManager.Instance.GetProgress(); 
    }
    //<param name="resPath">资源名字"Separation/Model/public_fbx_XBL@mesh"</param>
    //<param name="type">资源bundle类型“mainBundle”或者“场景1bundle”或者“场景2bundle”</param>
    public T LoadResource<T>(string resPath, string bundletype, bool isInstantiate = false, bool UseInternalResource = false) where T : UnityEngine.Object
    {
        return AssetManager.Instance.LoadBundleResource<T>(resPath,bundletype,isInstantiate,UseInternalResource);
    }
    public void AsyncLoadResource<T>(string resName, string bundletype, Action<T> callback = null,bool isInstantiate=false) where T : UnityEngine.Object
    {
        if (Application.isEditor| DevelepManager.UseInternalResource)
        {
            if(!DevelepManager.UseInternalResource)
            {
                AssetManager.Instance.AsyncLoadResource<T>(resName, bundletype, callback, isInstantiate);
            }
            else
            {
                T res = Resources.Load<T>(GetPath(resName,true));
                if (res != null)
                {
                    if (isInstantiate)
                    {
                        res = GameObject.Instantiate(res);
                    }
                    if (callback != null)
                    {
                        callback(res);
                    }
                }
            }
           
        }
        AssetManager.Instance.AsyncLoadResource<T>(resName, bundletype, callback,isInstantiate);
    }
    string GetPath(string path, bool UseInternalResource)
    {
        if (UseInternalResource)
        {
            return "PackagingResources/" + path;
        }
        else
        {
            if (path.Contains("/"))
            {
                string[] str = path.Split('/');
                if (str.Length > 0)
                    return (str[str.Length - 1]);
            }
            return path;
        }
    }
    public void DebugBundleTypeString(string bundleType)
    {
        AssetManager.Instance.DebugString(bundleType);
    }
}
