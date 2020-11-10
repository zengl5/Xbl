using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitJointCutsceneModel
{
    
    public class ModelData
    {
        public string PathArtRootNodeName;
        public string CutsceneArtRootNodeName;
        public string PathDesRootNodeName;
        public string CutsceneDesRootNodeName;
        public string ArtSceneName;
        public string DesSceneName;
        public List<string> PathReplaceList;
        public List<string> CutsceneReplaceList;
        public string Tag;
        public ModelData()
        {
            PathArtRootNodeName = "";
            CutsceneArtRootNodeName = "";
            PathDesRootNodeName = "";
            CutsceneDesRootNodeName = "";
            ArtSceneName = "";
            DesSceneName = "";
            PathReplaceList = new List<string>();
            CutsceneReplaceList = new List<string>();
        }
            
    }
    private static string _FilePath =Application.dataPath+ "/Editor/Slate/SplitJointCutscene/Data/";
    private static string _CutSceneRootTag = "CutScene";
    
    #region  播放iuv
    public static void OpenSceneStory(string path,string moudle ="iuv")
    {
        OpenScene(path, _FilePath + moudle+"/");
    }
    #endregion
     
    static void OpenScene(string path, string moudlePath)
    {
        ModelData data = new ModelData();
        string stageJson = C_Save.LoadString(path+".json", moudlePath, "",new C_SaveSimpleEncoder(), new System.Text.UTF8Encoding(false));
        Debug.Log("StageMgr LoadStage stage:" + path + ", stageJson:" + stageJson);
        InitData(data, stageJson);
        SplitJointCutscene.ReplaceArt(data.PathArtRootNodeName, data.PathDesRootNodeName, data.ArtSceneName, data.DesSceneName, data.PathReplaceList, "");
        SplitJointCutscene.ReplaceArt(data.CutsceneArtRootNodeName, data.CutsceneDesRootNodeName, data.ArtSceneName, data.DesSceneName, data.CutsceneReplaceList, _CutSceneRootTag);
    }

    static void InitData(ModelData result, string json)
    {
        if (result == null)
        {
            C_DebugHelper.LogError("result is null");
            return;
        }
        if (string.IsNullOrEmpty(json))
        {
            C_DebugHelper.LogError("json is null");
            return;
        }
        string artScene = C_Json.GetJsonKeyString(json, "ArtScene");
        if (string.IsNullOrEmpty(artScene))
        {
            C_DebugHelper.LogError("ArtScene is null ");
            return;
        }
        result.ArtSceneName = artScene;
        string pathArtName = C_Json.GetJsonKeyString(json, "PathArtName");
        if (string.IsNullOrEmpty(pathArtName))
        {
            C_DebugHelper.LogError("ArtName is null ");
            return;
        }
        result.PathArtRootNodeName = pathArtName;
        string pathDesName = C_Json.GetJsonKeyString(json, "PathDesName");
        if (string.IsNullOrEmpty(pathDesName))
        {
            C_DebugHelper.LogError("DesName is null ");
            return;
        }
        result.PathDesRootNodeName = pathDesName;

        string desScene = C_Json.GetJsonKeyString(json, "DesScene");
        if (string.IsNullOrEmpty(desScene))
        {
            C_DebugHelper.LogError("DesScene is null ");
            return;
        }
        result.DesSceneName = desScene;

        string cutsceneArtName = C_Json.GetJsonKeyString(json, "CutsceneArtName");
        if (string.IsNullOrEmpty(cutsceneArtName))
        {
            C_DebugHelper.LogError("cutsceneArtName is null ");
            return;
        }
        result.CutsceneArtRootNodeName = cutsceneArtName;
        string cutsceneDesName = C_Json.GetJsonKeyString(json, "CutsceneDesName");
        if (string.IsNullOrEmpty(cutsceneDesName))
        {
            C_DebugHelper.LogError("cutsceneDesName is null ");
            return;
        }
        result.CutsceneDesRootNodeName = cutsceneDesName;

        JsonData pathReplacItemNameList = C_Json.GetJsonKeyJsonData(json, "PathReplaceItemName");
        if (pathReplacItemNameList == null)
        {
            C_DebugHelper.LogError("ReplaceItemName is null");
            return;
        }
        foreach (JsonData item in pathReplacItemNameList)
        {
            result.PathReplaceList.Add(item.ToString());
        }

        JsonData cutsceneReplacItemNameList = C_Json.GetJsonKeyJsonData(json, "CutsceneReplaceItemName");
        if (cutsceneReplacItemNameList == null)
        {
            C_DebugHelper.LogError("cutsceneReplacItemNameList is null");
            return;
        }
        foreach (JsonData item in cutsceneReplacItemNameList)
        {
            result.CutsceneReplaceList.Add(item.ToString());
        }
    }

}
