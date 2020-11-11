using Slate;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveResPathEditor : EditorWindow
{
    protected Vector2 _ScrollPosition;
    string[] StoryName = { "iuv|Assets/Scene/iuv/iuv_Story.unity" , "aoe|Assets/Scene/aoe/aoe_story.unity" };
    string[] LoadConfig = { "aoe#aoe_Camenter|aoe_Cam001/aoe_Cam019_CamNext"};
    protected Dictionary<string, List<string>> _Dic = new Dictionary<string, List<string>>();
    protected List<string> _DicList = new List<string>();
    protected bool _InitFlag = false;
    protected Scene _ActiveDestionScene;
    // //单集多个分割的资源
    protected string savePath;
    protected Dictionary<string, string> _DicSplitResList = new Dictionary<string, string>();
    protected Dictionary<string, Dictionary<string, List<string>>> _DicSplitRes = new Dictionary<string, Dictionary<string, List<string>>>();

  //  [MenuItem("工具/新剧情加载资源工具", false, 102)]
    static void DoSaveCutSceneResPathEditor()
    {
        EditorWindow.GetWindow<SaveResPathEditor>("新剧情加载资源工具");
    }
    protected virtual void Init()
    {
       if (!_InitFlag)
       {
           _InitFlag = true;
           savePath = Application.dataPath + "/Resources/PackagingResources/Config/Loading/";
           for (int i = 0;i < StoryName.Length;i++ )
           {
               string cutscene = StoryName[i].Split('|')[0];
               string[] storyScene = StoryName[i].Substring(StoryName[i].IndexOf('|')+1).Split(',');
               if (storyScene ==null ||( storyScene!= null  && storyScene.Length <= 0))
               {
                   continue;
               }
               List<string> vs = new List<string>();
               for (int j = 0; j < storyScene.Length; j++)
               {
                   vs.Add(storyScene[j]);
               }
               if (_Dic.ContainsKey(cutscene))
               {
                   _Dic[cutscene] = vs;
               }
               else
               {
                   _Dic.Add(cutscene, vs);
               }
           }

           _DicList = new List<string>(_Dic.Keys);


           //单集分割的资源
           for (int i = 0; i < LoadConfig.Length; i++)
           {
               Dictionary<string, List<string>> slitRes = new Dictionary<string, List<string>>();
               //模块名
               string moudle = LoadConfig[i].Split('#')[0];
               string[] fileMsg = LoadConfig[i].Substring(LoadConfig[i].IndexOf('#') + 1).Split('|');
               //fileMsg[0]--表示文件名， //fileMsg[1]-- 表示文件储存的剧情的开始和结束的剧情编号
               List<string> resList = new List<string>();
               string[] resArr = fileMsg[1].Split('/');
               for (int ii = 0;  ii < resArr.Length;ii++)
               {
                   resList.Add(resArr[ii]);
               }

               if (slitRes.ContainsKey(fileMsg[0]))
               {
                   slitRes[fileMsg[0]] = resList;
               }
               else
               {
                   slitRes.Add(fileMsg[0], resList);
               }

                if (_DicSplitRes.ContainsKey(moudle))
                {
                    _DicSplitRes[moudle] = slitRes;
                }
                else
                {
                    _DicSplitRes.Add(moudle, slitRes);
                }
            }


       }
    }
    protected virtual void AutoSave()
    {
        string Result = "";
        foreach (string key in _Dic.Keys)
        {
            Result += Load(key);
            string path = Application.dataPath + "/Resources/PackagingResources/Config/Loading/";
            FileTools.CreateDir(path);
            //更新当前角色的动画设置
            FileTools.CreateFile(path + key + ".txt", Result);
        }

        MessageBoxEditor.ShowErrorBox("收集结束", "加载文件创建完成", "好的");

    }
    void OnGUI()
    {
        Init();
        _ScrollPosition = GUILayout.BeginScrollView(_ScrollPosition, false, true, GUILayout.MinHeight(200), GUILayout.MaxHeight(1000), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("故事创建加载文本");

        EditorGUILayout.BeginVertical();

        foreach (string key in _Dic.Keys)
        {
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField("故事名字：" + key);
            if (GUILayout.Button("创建全部资源加载文本"))
            {
                SaveLoadingRes(key);
            }
            if (GUILayout.Button("创建一集多个加载文本"))
            {
                // SaveLoadingRes(key);
                SaveSplitLoadingRes(key);
            }
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("创建所有加载文本"))
        {
            AutoSave();
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("游戏创建加载文本");

        EditorGUILayout.EndVertical();

        EditorUtility.SetDirty(this);
        EditorGUILayout.EndScrollView();



    }
    protected virtual string Load(string key)
    {
        bool mark = false;
        string Result = "";
        for (int i = 0; i < _Dic[key].Count; i++)
        {
            if (!string.IsNullOrEmpty(_Dic[key][i]))
            {
                mark = true;
                EditorUtility.DisplayProgressBar("加载资源信息收集： ", _Dic[key][i], (float)i / (float)_Dic[key].Count);

                //打开对应的场景,并做相应的移动
                _ActiveDestionScene = EditorSceneManager.OpenScene(_Dic[key][i], OpenSceneMode.Additive);
                EditorSceneManager.SetActiveScene(_ActiveDestionScene);

                //找到cutsceneplayer ,获取所有对象
                GameObject player = GameObject.Find("CutsceneSequencePlayer");
                if (player != null)
                {
                    CutsceneSequencePlayer cutPlayer = player.GetComponent<CutsceneSequencePlayer>();
                    if (cutPlayer == null)
                    {
                        MessageBoxEditor.ShowErrorBox("加载资源信息收集出错", key + "没有CutsceneSequencePlayer对象,加载文件创建失败,请检查后重试", "好的");
                        return "";
                    }

                    Result += cutPlayer.ReadLoadingMsg(key,Result);
                }
                EditorSceneManager.CloseScene(_ActiveDestionScene, true);
            }
        }
        if (mark)
            EditorUtility.ClearProgressBar();

        return Result;

    }
    protected virtual void SaveLoadingRes(string key, bool msg = true)
    {
        if (!_Dic.ContainsKey(key))
        {
            MessageBoxEditor.ShowErrorBox("加载资源信息收集出错", key + "不在设置中，让黄志龙检查", "好的");
            return;
        }
        string Result = Load(key);
        string path = Application.dataPath + "/Resources/PackagingResources/Config/Loading/";
        //更新当前角色的动画设置
        FileTools.CreateDir(path);
        FileTools.CreateFile(path + key + ".txt", Result);
        if (msg)
            MessageBoxEditor.ShowErrorBox("收集结束", key + "加载文件创建完成", "好的");

    }
    ////按照单集的每个分割点进行收集数据
    protected void SaveSplitLoadingRes(string key, bool msg = true)
    {
        string Result = "";
        int sum = 0;
        foreach (string k in _DicSplitRes[key].Keys)///每一个key对应一个保存信息的文本
        {
            Result = "";
            EditorUtility.DisplayProgressBar("加载资源信息收集： ", k, (float)sum++ / (float)_DicSplitRes[key].Keys.Count);

            //对每个资源进行加载
            //k表示每个资源的文件名字，每个资源_DicSplitRes[key][k][i]
            if (!string.IsNullOrEmpty(_DicSplitRes[key][k][0]) && !string.IsNullOrEmpty(_DicSplitRes[key][k][_DicSplitRes[key][k].Count - 1]))
            {
                //打开对应的场景,并做相应的移动，只支持一个场景
                _ActiveDestionScene = EditorSceneManager.OpenScene(_Dic[key][0], OpenSceneMode.Additive);
                EditorSceneManager.SetActiveScene(_ActiveDestionScene);

                //找到cutsceneplayer ,获取所有对象
                GameObject player = GameObject.Find("CutsceneSequencePlayer");
                if (player != null)
                {
                    CutsceneSequencePlayer cutPlayer = player.GetComponent<CutsceneSequencePlayer>();
                    if (cutPlayer == null)
                    {
                        MessageBoxEditor.ShowErrorBox("加载资源信息收集出错", key + "没有CutsceneSequencePlayer对象,加载文件创建失败,请检查后重试", "好的");
                        return;
                    }

                    Result += cutPlayer.ReadLoadingMsg(_DicSplitRes[key][k][0], _DicSplitRes[key][k][_DicSplitRes[key][k].Count - 1], Result);
                }
                EditorSceneManager.CloseScene(_ActiveDestionScene, true);

                //更新当前角色的动画设置
                FileTools.CreateDir(savePath);
                FileTools.CreateFile(savePath + k + ".txt", Result);

            }
            EditorUtility.ClearProgressBar();
        }

        if (msg)
            MessageBoxEditor.ShowErrorBox("收集结束", key + "加载文件创建完成", "好的");

    }
}
