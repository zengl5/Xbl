using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AsyncAssetBundle<T> where T : UnityEngine.Object
{
     string m_strResName = "";
     string m_strAssetBundleFilePath = "";
     List<string> m_DependenciesFilePathList = new List<string>();
     Action<T> m_Callback = null;
     bool m_IsInstantiate = false;


    public AsyncAssetBundle(AssetBundleRef abrf, string resName, Action<T> callback, bool isInstantiate=false)
    {
        m_Callback = callback;
        m_IsInstantiate = isInstantiate;
        AssetManager.Instance.StartCoroutine(ExecuteLoader(abrf, resName));
    }


    IEnumerator ExecuteLoader(AssetBundleRef abrf, string resName)
    {
        AssetBundleRequest abrs = abrf.bundle.LoadAssetAsync<T>(resName);
        yield return abrs;
        T tempObject = null;
        yield return abrs;
        if (abrs.isDone)
        {
            tempObject = abrs.asset as T;
            if (tempObject != null)
                if (m_IsInstantiate)
                {
                    tempObject = GameObject.Instantiate(tempObject);
                    if (m_Callback != null)
                        m_Callback(tempObject);
                }               
        }
    }
}
            

