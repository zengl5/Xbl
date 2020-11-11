using Slate;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveCutSceneResPathEditor : EditorWindow
{
   protected Vector2 _ScrollPosition;

    string[] StoryName = {"aoe|Assets/Scene/aoe/aoe_story.unity"
       ,"aoe_2|Assets/Scene/aoe/aoe_2_story.unity"
       ,"gkh|Assets/Scene/gkh/gkh_story.unity"
       ,"2p|Assets/Scene/2p/2p_story.unity"
       ,"3p|Assets/Scene/3p/3p_story.unity"
       ,"bpmf|Assets/Scene/bpmf/bpmf_story.unity" 
       ,"dtnl|Assets/Scene/dtnl/dtnl_story.unity" 
       ,"iuv|Assets/Scene/iuv/iuv_story.unity" 
       ,"jqx|Assets/Scene/jqx/jqx_story.unity" 
       ,"ssd|Assets/Scene/ssd/ssd_story.unity" 
       ,"zh|Assets/Scene/zh/zh_story.unity" 
       ,"zcs|Assets/Scene/zcs/zcs_story.unity" 
       ,"yw|Assets/Scene/yw/yw_story.unity"
       ,"ai|Assets/Scene/ai/ai_story.unity"
       ,"ao|Assets/Scene/ao/ao_story.unity"
       ,"an|Assets/Scene/an/an_story.unity"
       ,"ang|Assets/Scene/ang/ang_story.unity"
       ,"ie|Assets/Scene/ie/ie_story.unity"
         };
    protected Dictionary<string, List<string>> _Dic = new Dictionary<string, List<string>>();
    protected List<string> _DicList;
    protected bool _InitFlag = false;
    protected Scene _ActiveDestionScene;

    [MenuItem("工具/剧情加载资源工具", false, 102)]
    static void DoSaveCutSceneResPathEditor()
    {
        EditorWindow.GetWindow<SaveCutSceneResPathEditor>("剧情加载资源工具");
    }
   protected virtual void Init()
    {
        if (!_InitFlag)
        {
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
        }
    }
    protected virtual void AutoSave()
    {
        string Result="";
        foreach (string key in _Dic.Keys)
        {
            Result = "";
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
            EditorGUILayout.LabelField("故事名字："+key);
            if (GUILayout.Button("创建加载文本"))
            {
                SaveLoadingRes(key);
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

                    Result += cutPlayer.NewReadLoadingMsg(key,Result);
                }
                EditorSceneManager.CloseScene(_ActiveDestionScene, true);
            }
        }
        if(mark)
            EditorUtility.ClearProgressBar();

        return Result;

    }
    protected virtual void SaveLoadingRes(string key,bool msg = true)
    {
        if (!_Dic.ContainsKey(key))
        {
            MessageBoxEditor.ShowErrorBox("加载资源信息收集出错", key + "不在设置中，让黄志龙检查", "好的");
            return  ;
        }
        string Result = Load(key);
        string path = Application.dataPath + "/Resources/PackagingResources/Config/Loading/";
        //更新当前角色的动画设置
        FileTools.CreateDir(path);
        FileTools.CreateFile(path + key + ".txt", Result);
        if(msg)
           MessageBoxEditor.ShowErrorBox("收集结束", key + "加载文件创建完成", "好的");

    }
   
}
