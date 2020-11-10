using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
public class AssetManager3D : C_MonoSingleton<AssetManager3D>
{
    string path;         
    public GameObject prefab;
    public AudioClip sound;
    bool LoadFinish = false;
    bool isLoad = false;
    Queue<string> BundleQue = new Queue<string>();
    Dictionary<string, List<string>> bundleDic = new Dictionary<string, List<string>>();//type ,path  
    int totalCount = 0;
    public List<AssetBundleRef> AbRefList = new List<AssetBundleRef>();

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }
   public void StartLoad() 
    {
        LoadFinish = false;
        if (Application.isEditor)
        {
            string url = Application.dataPath + "/Data/PC/public/publicasset";
            string ur2 = Application.dataPath + "/Data/PC/aeo/aeobundle";
            string ur3 = Application.dataPath + "/Data/PC/mainwindow/mainwindow";
            InitAssetBundle(url);
            InitAssetBundle(ur2);
            InitAssetBundle(ur3);
        }
        else
        {
            string url = Application.streamingAssetsPath + "/Data/Android/publicBundle/publicasset";
            string ur2 = Application.streamingAssetsPath + "/Data/Android/aoeBundle/aoeasset";
            string ur3 = Application.streamingAssetsPath + "/Data/Android/mainwindowBundle/mainwindow";
            InitAssetBundle(url);
            InitAssetBundle(ur2);
            InitAssetBundle(ur3);
        } 
    }
    
    void OnGUI()
    {
        if(!LoadFinish)
        {
            GUI.skin.label.fontSize = 50;
            GUILayout.Label("当前加载进度:" + GetProgress()*100 + "%");
        }    
    }
    
 
    /// <summary>
    /// 兼容3D拼音项目
    /// </summary>
    /// <param name="path"></param>
    public void InitAssetBundle(string path)//public scene1 scene2
    {       
        totalCount++;
        BundleQue.Enqueue(path);
        if (!isLoad)
        {
            isLoad = true;
            StartCoroutine("LoadAsyncCoroutineBundle", BundleQue.Dequeue());
        }
    }
    IEnumerator LoadAsyncCoroutineBundle(string path)
    {
        if (path == null)
        {
            totalCount = 0;
            yield return null;
        }
        AssetBundleCreateRequest acr = AssetBundle.LoadFromFileAsync(path);
        AssetBundleRef bundle = new AssetBundleRef(path);
        bundle.Progress = acr.progress;
        bundle.abcr = acr;
        AbRefList.Add(bundle);
        yield return acr;
        if (acr != null)
        {
            bundle.bundle = acr.assetBundle;
            if (BundleQue.Count > 0)
                StartCoroutine("LoadAsyncCoroutineBundle", BundleQue.Dequeue());
        }
    }
    public float GetProgress()
    {
        float progress = 0;
        for (int i = 0; i < AbRefList.Count; i++)
        {
            progress+=AbRefList[i].abcr.progress;
            if(i==AbRefList.Count-1)
            return progress/totalCount;        
        }
        return 1;
    }
   
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resName"></param>
    /// <param name="isInstantiate"></param>
    /// <returns></returns>
    public  T LoadAssetBundle<T>(string resName, bool isInstantiate = false) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(resName))
            return null;
        T result = null;
        try
        {
            int index = 0;
            for(index = 0; index < AbRefList.Count; index++)
            {
                if(AbRefList[index].bundle.Contains(resName))
                {
                    result = AbRefList[index].bundle.LoadAsset<T>(resName);
                    if (isInstantiate && result != null)
                    {
                        if (Application.isEditor)
                            Debug.LogError(resName);
                        result = GameObject.Instantiate(result);
                    }
                }
                if (index == AbRefList.Count - 1)
                {
                    if (Application.isEditor)
                        if(result==null)
                        Debug.LogError("找不到资源::" + resName);
                }              
            }                   
        }
        catch (Exception e)
        {
            throw e;
        }
        return result;
    }




   
}
