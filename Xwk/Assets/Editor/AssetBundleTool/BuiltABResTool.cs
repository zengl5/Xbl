using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class BuiltABResTool : EditorWindow
{
    protected static string[] _Plamtform = { "packaging_resources_ios/", "packaging_resources_android/" , "packaging_resources_windows/" };
    protected static Dictionary<string, bool> _StoryDic = new Dictionary<string, bool>();
    protected static Dictionary<string, bool> _PlamtformDic = new Dictionary<string, bool>();
    protected static string[] StoryName = {"aoe"
            ,"aoe_2"
            ,"iuv"
            ,"dtnl"
            ,"bpmf"
            ,"jqx"
            ,"gkh"
            ,"ssd"
            ,"zcs"
            ,"zh"
            ,"2p"
            ,"3p"
            ,"yw"
            ,"ai"
            ,"ao"
            ,"an"
            ,"ang"
            ,"ie"
            ,"g_ crane_aoe"
            ,"g_aoe01_cleanupMouth"
            ,"g_card_aoe"
            ,"g_grab_aoe"
            ,"asr_res"
            ,"g_all"
    };
    protected static string _ConfigPath;
    protected static string _DestionResPath;
    protected static string _ResPath;
    protected static bool _Init = false;
    [MenuItem("工具/抽离每一集打包的ab资源",false,103)]
    static void DoBuiltInResTool()
    {
        EditorWindow.GetWindow<BuiltABResTool>("抽离每一集打包的资源");
        _ConfigPath = Application.dataPath + "/Resources/PackagingResources/Config/Loading/";
        _StoryDic.Clear();
        _PlamtformDic.Clear();
        for (int i = 0;i < StoryName.Length;i++)
        {
            _StoryDic.Add(StoryName[i],false);
        }
        for (int j = 0; j < _Plamtform.Length; j++)
        {
            _PlamtformDic.Add(_Plamtform[j], false);
        }
        _Init = true;
    }
    void OnGUI()
    {
      
        MoveAssets();
    }
   protected virtual bool getResPath(string path)
    {
        _ResPath = LocalPath.HotUpdatePath + path;
        if (!Directory.Exists(_ResPath))
        {
            Debug.LogError("打包路径资源：" + _ResPath + "不存在");
            return false;
        }
        return true;
    }
    int  ShowPlamt()
    {
        int plamtId = -1;
        EditorGUILayout.BeginToggleGroup("Plamtform Toggle", true);
        for (int i = 0; i < _PlamtformDic.Keys.Count; i++)
        {
            _PlamtformDic[_Plamtform[i]] = EditorGUILayout.Toggle(_Plamtform[i], _PlamtformDic[_Plamtform[i]]);
            if (!_PlamtformDic[_Plamtform[i]])
            {
                continue;
            }
            if (!getResPath(_Plamtform[i]))
            {
                Debug.LogError("打包路径资源：" + _ResPath + "不存在");
                continue;
            }
            plamtId = i;
        }
        EditorGUILayout.EndToggleGroup();
        return plamtId;
    }
   protected void MoveAssets()
    {
        if (!_Init)
        {
            return;
        }
        EditorGUILayout.BeginVertical();
        int plamtSum = _PlamtformDic.Keys.Count;
        int plamtId = -1;
        EditorGUILayout.LabelField("选择打包的平台");
        
    
        int row = 0;
        int column = 0;

        EditorGUILayout.BeginVertical();
        plamtId =  ShowPlamt();
        GUILayout.Space(5);
        EditorGUILayout.LabelField("勾选需要移动的资源包名字");
        GUILayout.Space(plamtSum * 30 + 10);
        for (int key = 0; key < StoryName.Length; key++)
        {
            if (key % 4 == 0)
            {
                row++;
                column = 0;
            }
            _StoryDic[StoryName[key]] = GUI.Toggle(new Rect(column * 300, (plamtSum + row) * 30, 300, 20), _StoryDic[StoryName[key]], StoryName[key]);
            column++;
        }

        GUILayout.Space((plamtSum + row) * 30 + 10);
        EditorGUILayout.LabelField("选择平台");
        GUILayout.Space(5);

        if (GUILayout.Button("全部开始移动", GUILayout.Width(200)))
        {
            Move(plamtId);
        }
        GUILayout.Space(10);

        if (GUILayout.Button("分开单独移动", GUILayout.Width(200)))
        {
            Move(plamtId, true);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();

    }
   public virtual void Move(int i,bool single = false)
    {
        Debug.Log("开始移动");
        for (int j = 0; j < StoryName.Length; j++)
        {
            if (!_StoryDic[StoryName[j]])
            {
                continue;
            }
            if (single)
            {
                //每一集单独一个资源文件夹
                _DestionResPath = LocalPath.HotUpdatePath + "builtin/" + _Plamtform[i] + StoryName[j]+"/";
            }
            else
            {
                _DestionResPath = LocalPath.HotUpdatePath + "builtin/" + _Plamtform[i] + _Plamtform[i];
            }
            //目标文件夹路径
            FileTools.CreateDir(_DestionResPath);
            //查找资源
            string config = _ConfigPath + StoryName[j] + ".txt";
            if (!FileTools.IsFileExited(config))
            {
                MessageBoxEditor.ShowErrorBox("移动内置资源", config + "不存在，移动内置资源失败", "好的");
                continue;
            }
            string data = getContent(config);
            data.Replace("\r", "");
            string[] result = data.Split('\n');

            for (int k = 0; k < result.Length; k++)
            {

                if (string.IsNullOrEmpty(result[k]))
                {
                    continue;
                }
                result[k] = result[k].Split('\r')[0];

                string res = _ResPath + result[k];
                if (string.IsNullOrEmpty(res))//资源名字
                {
                    continue;
                }
                if (!Directory.Exists(_ResPath))
                {
                    Debug.LogError("不存在资源目录：" + _ResPath);
                    continue;
                }
                
                //移动到目标文件夹，如果文件夹不存在，先创建
                if (!File.Exists(res))
                {
                    Debug.LogError("不存在资源：" + res);
                    continue;
                }
              
                string subFloder = Assets.Scripts.C_Framework.C_String.GetSavePath(result[k]);//= result[k].Substring(0, result[k].LastIndexOf('/') > -1 ? result[k].LastIndexOf('/') : result[k].Length);
                if(FileTools.CreateDir(_DestionResPath + subFloder))
                {
                   // Debug.Log(_DestionResPath + subFloder+"is exited..");
                }
                try
                {
                    File.Copy(_ResPath + result[k], _DestionResPath + result[k], true);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    continue;
                }
                string mainfest = _ResPath + result[k] + ".manifest";
                if (!File.Exists(mainfest))
                {
                    Debug.LogError(res + "不存在资源manifest文件：" + mainfest);
                    continue;
                }
                try
                {
                    Debug.Log("存在资源manifest文件：" + mainfest);
                    if (File.Exists(_DestionResPath + result[k] + ".manifest"))
                    {
                        Debug.Log("存在destion资源manifest文件：" + _DestionResPath + result[k] + ".manifest");
                    }
                    File.Copy(mainfest, _DestionResPath + result[k] + ".manifest", true);

                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    continue;
                }

            }
            //移动主mainfe文件
            if (single)
            {
                MoveMainfest(i);
            }
        }
        //移动主mainfe文件
        if (!single)
        {
            MoveMainfest(i);
        }


        Debug.Log("移动完成");
    }
    void MoveMainfest(int i)
    {
        string mainfestFile = _Plamtform[i].Split('/')[0];
        string resmainfest = _ResPath + mainfestFile;
        if (string.IsNullOrEmpty(resmainfest))
        {
            return;
        }
        if (!File.Exists(resmainfest))
        {
            Debug.LogError("不存在资源：" + resmainfest);
            return;

        }
        File.Copy(resmainfest, _DestionResPath + mainfestFile, true);
        if (!File.Exists(resmainfest + ".manifest"))
        {
            Debug.LogError("不存在资源：" + resmainfest + ".manifest");
            return;
        }
        File.Copy(resmainfest, _DestionResPath + mainfestFile + ".manifest", true);
    }
    public string getContent(string filePath)
    {
        FileStream fs = new FileStream(filePath, FileMode.Open);
        int len = (int)fs.Length;
        byte[] buffer = new byte[len];
        int length =  fs.Read(buffer, 0, len);
        string myStr = System.Text.Encoding.UTF8.GetString(buffer);
        fs.Close();
        fs.Dispose();
        return myStr;
    }

}
