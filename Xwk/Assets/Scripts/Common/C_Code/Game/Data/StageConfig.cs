using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageConfigItem
{
    public int ID = 0;
    public string Name = "";
    public int Type = 0;
    public int Star = 0;
    public int State = 0;
    public int Pay = 1;

    public int KnowStar = 0;
    public int ReadStar = 0;
    public int WriteStar = 0;

    public int KnowScore = 0;
    public int ReadScore = 0;
    public int WriteScore = 0;

    public int TestScore = 0;
}

public class StageConfig
{
    public static string Name = "stage_config";

    public static string URL = "";

    public static List<StageConfigItem> StageConfigItemList = new List<StageConfigItem>();

    public static void Load()
    {
        string strData = C_MonoSingleton<C_DataMgr>.GetInstance().LoadData(Name);
        if (!string.IsNullOrEmpty(strData))
        {
            StageConfigItemList.Clear();

            URL = C_Json.GetJsonKeyString(strData, "URL");

            JsonData stageConfigItemListJD = C_Json.GetJsonKeyJsonData(strData, "StageConfigItemList");
            if (stageConfigItemListJD != null)
            {
                for (int i = 0; i < stageConfigItemListJD.Count; i++)
                {
                    if (stageConfigItemListJD[i] != null)
                    {
                        StageConfigItem item = new StageConfigItem();

                        item.ID = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "ID");
                        item.Name = C_Json.GetJsonKeyString(stageConfigItemListJD[i], "Name");
                        item.Type = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "Type");
                        item.Star = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "Star");
                        item.State = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "State");
                        item.Pay = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "Pay");

                        item.KnowStar = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "KnowStar");
                        item.ReadStar = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "ReadStar");
                        item.WriteStar = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "WriteStar");

                        item.KnowScore = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "KnowScore");
                        item.ReadScore = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "ReadScore");
                        item.WriteScore = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "WriteScore");

                        item.TestScore = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "TestScore");

                        StageConfigItemList.Add(item);
                    }
                }
            }
        }
    }

    public static void Save(string strData)
    {
        if (!string.IsNullOrEmpty(strData))
        {
            StageConfigItemList.Clear();

            JsonData stageConfigItemListJD = JsonMapper.ToObject(strData);
            if (stageConfigItemListJD != null)
            {
                for (int i = 0; i < stageConfigItemListJD.Count; i++)
                {
                    StageConfigItem item = new StageConfigItem();

                    item.ID = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "Fid");
                    item.Name = C_Json.GetJsonKeyString(stageConfigItemListJD[i], "Fname");
                    item.Type = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "Ftype");
                    item.Star = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "Fstar");
                    item.State = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "Fstate");
                    item.Pay = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "Fpay");

                    string strStar = C_Json.GetJsonKeyString(stageConfigItemListJD[i], "Fkrw_star");
                    if (!string.IsNullOrEmpty(strStar))
                    {
                        string[] starStr = strStar.Split('|');

                        item.KnowStar = int.Parse(starStr[0]);
                        item.ReadStar = int.Parse(starStr[1]);
                        item.WriteStar = int.Parse(starStr[2]);
                    }

                    string strPass = C_Json.GetJsonKeyString(stageConfigItemListJD[i], "Fkrw_pass");
                    if (!string.IsNullOrEmpty(strPass))
                    {
                        string[] passStr = strPass.Split('|');

                        item.KnowScore = int.Parse(passStr[0]);
                        item.ReadScore = int.Parse(passStr[1]);
                        item.WriteScore = int.Parse(passStr[2]);
                    }

                    item.TestScore = C_Json.GetJsonKeyInt(stageConfigItemListJD[i], "Fscore");

                    StageConfigItemList.Add(item);
                }
            }

            C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new StageConfig()));
        }
    }

    public static StageConfigItem GetStageConfigItem(int stageID)
    {
        for (int i = 0; i < StageConfigItemList.Count; i++)
        {
            if (StageConfigItemList[i] != null && StageConfigItemList[i].ID == stageID)
                return StageConfigItemList[i];
        }

        return null;
    }

    public static StageConfigItem GetStageConfigItem(string stageName)
    {
        for (int i = 0; i < StageConfigItemList.Count; i++)
        {
            if (StageConfigItemList[i] != null && StageConfigItemList[i].Name == stageName)
                return StageConfigItemList[i];
        }

        return null;
    }
}