using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class AssetbundleVersionMgr : SaveResPathEditor
{
    private AssetBundleManifest _MainAssetBundleManifest;

    [MenuItem("工具/Assetbundle资源版本创建", false, 101)]
    static void DoAssetbundleVersionMgr()
    {
        EditorWindow.GetWindow<AssetbundleVersionMgr>("Assetbundle资源版本创建");
    }

    private void OnGUI()
    {
        Init();
        _ScrollPosition = GUILayout.BeginScrollView(_ScrollPosition, false, true, GUILayout.MinHeight(200), GUILayout.MaxHeight(1000), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("故事创建assetbundle资源版本");
        EditorGUILayout.BeginVertical();

        foreach (string key in _Dic.Keys)
        {
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField("故事名字：" + key);
            if (GUILayout.Button("创建资源版本"))
            {
                Save(key);
            }

            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("创建所有资源版本"))
        {
            AutoSave();
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("游戏创建资源版本");

        EditorGUILayout.EndVertical();

        EditorUtility.SetDirty(this);
        EditorGUILayout.EndScrollView();
    }

    protected void  Save(string key)
    {
        //_MainAssetBundleManifest = C_Singleton<GameResMgr>.Instance.GetAssetBundleManifest(key);
        if (!_Dic.ContainsKey(key))
        {
            MessageBoxEditor.ShowErrorBox("加载资源信息收集出错", key + "不在设置中，让黄志龙检查", "好的");
            return;
        }
        SaveLoadingRes(key,false);
        string content = "";
        string result = Load(key);
        string[] data = result.Split('\n');
        //string md5 = "";
        for (int i = 0;i < data.Length;i++)
        {

            //string path = LocalPath.LocalPackagingResources + data[i];
            string path = data[i];
            EditorUtility.DisplayProgressBar("创建md5值： " , path, (float)i / (float)data.Length);

            //if (File.Exists(path))
            //{
            //    md5 = getFileHash(path);
            //    content += data[i] + "|" + md5 + "\n";
            //}
            //else
            //{
            //    Debug.LogError(path+"没有打包，请检查打包资源");
            //    continue;
            //}
            if (string.IsNullOrEmpty(path))
            {
                continue;
            }
            //资源对象
            //if (_MainAssetBundleManifest == null || Hash128.Parse("0") == _MainAssetBundleManifest.GetAssetBundleHash(path))
            //{
            //    Debug.LogError("path :"+path+ "没有打入总表,或者_MainAssetBundleManifest 总表不存在");
            //    continue;
            //}
           // content +=  setFileMd5(content, path);
            //查找依赖项
            List<string> dpsList = C_Singleton<GameResMgr>.Instance.GetAllDependencies(path);
            for (int j = 0; j < dpsList.Count; j++)
            {
                content += setFileMd5(content, dpsList[j]);
            }
        }
        EditorUtility.ClearProgressBar();

        string configPath = LocalPath.LocalHotUpdateConfigPath + key + ".txt";
        FileTools.CreateDir(LocalPath.LocalHotUpdateConfigPath);
        FileTools.CreateFile(configPath, content);
        MessageBoxEditor.ShowErrorBox("assetbundle资源版本创建", "资源版本创建完成", "好的");

    }
   public string  setFileMd5(string content,string fileVaule)
    {
        if (content.Contains(fileVaule.ToLower()))
        {
            Debug.Log(fileVaule + "已经被打包，不需要添加");
            return "";
        }
        string path = LocalPath.LocalPackagingResources + fileVaule;
        if (File.Exists(path))
        {
           string  md5 = getFileHash(path);
           return fileVaule + "|" + md5 + "\n";
        }
        else
        {
            Debug.LogError(fileVaule + "没有打包，请检查打包资源");
        }
        return "";

    }

    public static string getFileHash(string filePath)
    {
        try
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            int len = (int)fs.Length;
            byte[] data = new byte[len];
            fs.Read(data, 0, len);
            fs.Close();
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string fileMD5 = "";
            foreach (byte b in result)
            {
                fileMD5 += Convert.ToString(b, 16);
            }
            return fileMD5;
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e.Message);
            return "";
        }
    }

}
