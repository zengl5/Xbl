using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreLoadResMgr : C_MonoSingleton<PreLoadResMgr>
{
    private string _ResFloder = "config/loading";
    private List<string> _ResList = new List<string>();

    private string _CurrentFilePath = "";

    public string getCurrentFilePath {
        get
        {
            return _CurrentFilePath;
        }
    }
    //根据文本，获取需要加载的对象
    public void StartLoadRes(string filepath, string resPath = "")
    {
        if (string.IsNullOrEmpty(filepath))
        {
            C_DebugHelper.LogError("filename is null");
            return;
        }
        if (!string.IsNullOrEmpty(resPath))
        {
            _ResFloder = resPath;
        }
        TextAsset binAsset = Resources.Load("PackagingResources/" + _ResFloder + "/" + filepath ) as TextAsset;
        //解析文本
        if (binAsset == null)
        {
            C_DebugHelper.LogError(filepath + "出场文件路径不存在");
            return;
        }
        //读取每一行的内容  
        string strAsset = binAsset.text.Replace("\r", string.Empty);
        string[] Info = strAsset.Split('\n');
        if (Info.Length <= 0)
        {
            C_DebugHelper.LogError(filepath + "剧情的文本文件有误");
            return;
        }
        try
        {
            _ResList.Clear();
            for (int i = 0; i < Info.Length; i++)
            {
                if (!string.IsNullOrEmpty(Info[i]))
                {
                    Info[i].Trim();
                    _ResList.Add(Info[i]);
                }
            }
        }
        catch (Exception e)
        {
            C_DebugHelper.LogError("文件名" + filepath + "错误信息" + e.Message);
        }
        //调用全部对象加载的接口
       // Debug.Log(filepath);
        _CurrentFilePath = filepath;
        GameResMgr.Instance.LoadAssetBundle_CacheList(filepath, _ResList);
    }
    public List<string> RequestABResList()
    {
        return _ResList; 
    }

    public float getCurrentLoadProgress(float accout)
    {
        return GameResMgr.Instance.GetProgressLoadAssetBundle_CacheList(_CurrentFilePath)* accout;
    }
    public float getCurrentUnLoadProgress(float accout)
    {
        return GameResMgr.Instance.GetProgressUnLoadAssetBundle_CacheList(_CurrentFilePath) * accout;
    }
    public void ForceUnloadABRes(string type)
    {
        GameResMgr.Instance.ForceUnloadAssetBundle_CacheList(type);
    }
}
