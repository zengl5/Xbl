using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using Assets.Scripts.C_Framework;
public class AssetManager : C_MonoSingleton<AssetManager>
{
    bool isLoad = false;
    Queue<string> BundleQue = new Queue<string>();
    Dictionary<string, string> bundleDic = new Dictionary<string,string>();//type ,path  
    Dictionary<string, AssetBundleRef> AbcacheRefDic = new Dictionary<string, AssetBundleRef>();//type 
    int totalCount = 0;

    #region ##Bundle逻辑处理
    public void UnLoadAssetBundle(string type)//public scene1 scene2
    {
        totalCount--;
        if (AbcacheRefDic.ContainsKey(type))
        {
            AbcacheRefDic[type].BundleUnload();
            AbcacheRefDic.Remove(type);
        }
        else
        {
            if (Application.isEditor)
                Debug.LogError("卸载Assetbundle失败,不存在这样的Assetbundle");
        }
    }            
    /// <summary>
    /// 初始化bundle  1个场景可能一个bundle,也可能多个。卸载不同阶段bundle
    /// </summary>
    /// <param name="path">bundle路径</param>
    /// <param name="type">类型 public类型，Scene1场景类型，Scene2场景类型</param>
    public void LoadAsyncAssetBundle(string path,string type)//public scene1 scene2
    {
        totalCount++;
        BundleQue.Enqueue(path);
        if(!bundleDic.ContainsKey(type))
        {
            bundleDic.Add(type, path);
        }
        if (!isLoad)
        {
            isLoad = true;
            if(bundleDic.Count>0)
            {
                StartCoroutine(LoadAsyncCoroutineBundle(BundleQue.Dequeue(),type));
            }
        }
    }
    public void LoadAsyncAssetBundle(string[] pathArray, string[] typeArray)//public scene1 scene2
    {
        bundleDic.Clear();
        isLoad = false;
        for (int i=0;i< pathArray.Length;i++)
        {
             if (!bundleDic.ContainsKey(typeArray[i]))
            {
                if (!AbcacheRefDic.ContainsKey(typeArray[i]))
                {
                    totalCount++;
                    BundleQue.Enqueue(pathArray[i]);
                    bundleDic.Add(typeArray[i], pathArray[i]);
                }
            }
        }          
        if (!isLoad)
        {
            isLoad = true;
            if (bundleDic.Count > 0)
            {
                StartCoroutine(LoadAsyncCoroutineBundle(BundleQue.Dequeue(), typeArray[0]));
            }
        }
    }
    IEnumerator LoadAsyncCoroutineBundle(string path,string type)
    {         
        if (path == null)
        {
            totalCount = 0;
            isLoad = false;//当前阶段bundle加载完成
            yield return null;
        }
        AssetBundleCreateRequest acr = AssetBundle.LoadFromFileAsync(path);
        AssetBundleRef bundle = new AssetBundleRef(path);
        bundle.Progress = acr.progress;
        bundle.abcr = acr;
        if(!AbcacheRefDic.ContainsKey(type))
            AbcacheRefDic.Add(type,bundle);
        yield return acr;
        if (acr != null)
        {
            if (Application.isEditor)
                Debug.LogError("bundle读取完成:"+type);
            bundle.bundle = acr.assetBundle;
            if (BundleQue.Count > 0)
            {
                string nextBundle = BundleQue.Dequeue();
                //从字典取出对应的类型
                foreach (string st in bundleDic.Keys)
                {
                    if (nextBundle.Equals(bundleDic[st]))
                    {
                        StartCoroutine(LoadAsyncCoroutineBundle(nextBundle, st));
                    }
                }
            }
        }
    }
    public float GetProgress()
    {
        //遍历字典中所有bundle的进度
        float progress = 0;
        foreach (AssetBundleRef bundle in AbcacheRefDic.Values)
        {
            progress += bundle.abcr.progress;
        }
        return progress/totalCount;
    }
    #endregion

    #region##读取Bundle资源
    public T LoadBundleResource<T>(string resPath, string bundletype, bool isInstantiate = false, bool UseInternalResource = false) where T : UnityEngine.Object
    {
        T result = null;
        if (Application.isEditor | UseInternalResource | DevelepManager.UseInternalResource)
        {
            //如果想要在编辑器模式下测试Assetbundle，开启True
            if (DevelepManager.UseInternalResource)
            {
                T res = Resources.Load<T>(GetPath(resPath, true));
                if (res != null)
                {
                    if (isInstantiate)
                    {
                        res = GameObject.Instantiate(res);
                        return res;
                    }
                    else
                        return res;
                }
            }
            else
            {
                if (UseInternalResource)
                {
                    T res = Resources.Load<T>(GetPath(resPath, UseInternalResource));
                    if (res != null)
                    {
                        if (isInstantiate)
                        {
                            res = GameObject.Instantiate(res);
                            return res;
                        }
                        else
                            return res;
                    }
                }
                else
                {
                    result = LoadResource<T>(GetPath(resPath, UseInternalResource), bundletype, isInstantiate);
                    return result;
                }

            }
        }
        result=LoadResource<T>(GetPath(resPath, UseInternalResource), bundletype, isInstantiate);
        return result;
    }
            
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resName">资源名字，属于资源类型中的一个</param>
    /// <param name="bundleType">资源类型，分为主场景，分场景</param>
    /// <param name="isInstantiate">是否实例化</param>
    /// <returns></returns>
    T LoadResource<T>(string resName, string bundleType, bool isInstantiate = false) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(resName))
            return null;
        T result = null;
        try
        {
            if(bundleType != null)
            {
                if(AbcacheRefDic.ContainsKey(bundleType))
                {
                    //对象池中加载数据（前提是用户手动回收，而且数据没有被Unload）
                    UnityEngine.Object tempObject = C_MonoSingleton<C_PoolMgr>.GetInstance().Spawn(resName) as T;
                    if (tempObject != null)
                    {
                        if(Application.isEditor)
                        {
                            Debug.LogError("从缓存中获取数据"+resName);
                        }
                        return tempObject as T;
                    }

                    if (AbcacheRefDic[bundleType].bundle.Contains(resName))
                    {
                        //添加缓存机制
                        result = AbcacheRefDic[bundleType].bundle.LoadAsset<T>(resName);
                        if (isInstantiate && result != null)
                        {
                            if (Application.isEditor)
                                Debug.Log(resName);
                             result =Instantiate(result);
                        }
                        if(result!=null&&!isInstantiate)
                        {
                            return result;
                        }
                    }
                    else
                    {                      
                        if (Application.isEditor)
                            Debug.LogError("找不到资源名字:"+ resName);
                    }
                }
                else
                {
                    if (Application.isEditor)
                        Debug.LogError("不存在这种类型:" + bundleType+"请重新检查地址:"+ resName);
                }
            }         
        }
        catch (Exception e)
        {
            throw e;
        }
        return result;
    }
    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resName">资源名字</param>
    /// <param name="type">bundle类型</param>
    /// <param name="callBack">事件回调</param>
    /// <param name="isInstantiate">是否实例化</param>

    public void AsyncLoadResource<T>(string resName, string bundleType, Action<T>callBack=null,bool isInstantiate = false) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(resName))
            return;
        try
        {
            if (bundleType != null)
            {
                if (AbcacheRefDic.ContainsKey(bundleType))
                {
                    UnityEngine.Object tempObject = C_MonoSingleton<C_PoolMgr>.GetInstance().Spawn(resName) as T;
                    if (tempObject != null)
                    {
                        if (Application.isEditor)
                        {
                            Debug.LogError("从缓存中获取数据" + resName);
                        }
                        if(isInstantiate)
                        {
                            Instantiate(tempObject);
                        }
                        if (callBack != null)
                            callBack(tempObject as T);
                        return;
                    }
                    if (AbcacheRefDic[bundleType].bundle.Contains(resName))
                    {
                        new AsyncAssetBundle<T>(AbcacheRefDic[bundleType], resName, callBack,isInstantiate);
                    }
                }
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    //路径拆分[获取末尾文件名]  "a/b/c" 拆分为c
    string GetPath(string path, bool UseInternalResource)
    {
        if (UseInternalResource)
        {
            return "PackagingResources/" + path;
        }
        else
        {
            if (path == null)
                return "";
            if (path.Contains("/"))
            {
                string[] str = path.Split('/');
                if (str.Length > 0)
                    return (str[str.Length - 1]);
            }
            return path;
        }
    }
    #endregion

    protected override void OnDestroy()
    {
        foreach (AssetBundleRef abrf in AbcacheRefDic.Values)
            abrf.BundleUnload();
    }
    public void DebugString(string bundleType)
    {
        if (AbcacheRefDic.ContainsKey(bundleType))
        {
            string[] strlist = AbcacheRefDic[bundleType].bundle.GetAllAssetNames();
            for (int i = 0; i < strlist.Length; i++)
                Debug.LogError(strlist[i]);

        }
    }
}
