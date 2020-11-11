using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageDataItem
{
    public int ID = 0;
    public int State = 0;

    public int MaxStar = 0;
    public int MaxKnowStar = 0;
    public int MaxReadStar = 0;
    public int MaxWriteStar = 0;

    public int MaxKnowScore = 0;
    public int MaxReadScore = 0;
    public int MaxWriteScore = 0;
}

public class StageData
{
    public static string Name = "stage_data";

    public static string UID = "";

    public static List<StageDataItem> StageDataItemList = new List<StageDataItem>();

    public static void Load()
    {
        string strData = C_MonoSingleton<C_DataMgr>.GetInstance().LoadData(Name);
        if (!string.IsNullOrEmpty(strData))
        {
            StageDataItemList.Clear();

            UID = C_Json.GetJsonKeyString(strData, "UID");
            if (!string.IsNullOrEmpty(PlayerData.UID) && UID == PlayerData.UID)
            {
                JsonData stageDataItemListJD = C_Json.GetJsonKeyJsonData(strData, "StageDataItemList");
                if (stageDataItemListJD != null)
                {
                    for (int i = 0; i < stageDataItemListJD.Count; i++)
                    {
                        if (stageDataItemListJD[i] != null)
                        {
                            StageDataItem item = new StageDataItem();

                            item.ID = C_Json.GetJsonKeyInt(stageDataItemListJD[i], "ID");
                            item.State = C_Json.GetJsonKeyInt(stageDataItemListJD[i], "State");

                            item.MaxStar = C_Json.GetJsonKeyInt(stageDataItemListJD[i], "MaxStar");
                            item.MaxKnowStar = C_Json.GetJsonKeyInt(stageDataItemListJD[i], "MaxKnowStar");
                            item.MaxReadStar = C_Json.GetJsonKeyInt(stageDataItemListJD[i], "MaxReadStar");
                            item.MaxWriteStar = C_Json.GetJsonKeyInt(stageDataItemListJD[i], "MaxWriteStar");

                            item.MaxKnowScore = C_Json.GetJsonKeyInt(stageDataItemListJD[i], "MaxKnowScore");
                            item.MaxReadScore = C_Json.GetJsonKeyInt(stageDataItemListJD[i], "MaxReadScore");
                            item.MaxWriteScore = C_Json.GetJsonKeyInt(stageDataItemListJD[i], "MaxWriteScore");

                            StageDataItemList.Add(item);
                        }
                    }
                }
            }
        }

        Unlock_a();

        //C_Singleton<StageMgr>.GetInstance().RefreshMaxCanPlayStage();
    }

    public static void Save(string strData)
    {
        if (!string.IsNullOrEmpty(strData))
        {
            StageDataItemList.Clear();
            
            int index = 0;
            while (true)
            {
                JsonData stageDataJD = C_Json.GetJsonKeyJsonData(strData, index.ToString());
                if (stageDataJD != null)
                {
                    StageDataItem item = new StageDataItem();

                    item.ID = C_Json.GetJsonKeyInt(stageDataJD, "bagid");
                    item.State = C_Json.GetJsonKeyInt(stageDataJD, "state");

                    item.MaxStar = C_Json.GetJsonKeyInt(stageDataJD, "maxstar");
                    item.MaxKnowStar = C_Json.GetJsonKeyInt(stageDataJD, "know_maxstar");
                    item.MaxReadStar = C_Json.GetJsonKeyInt(stageDataJD, "read_maxstar");
                    item.MaxWriteStar = C_Json.GetJsonKeyInt(stageDataJD, "write_maxstar");

                    item.MaxKnowScore = C_Json.GetJsonKeyInt(stageDataJD, "know_maxscore");
                    item.MaxReadScore = C_Json.GetJsonKeyInt(stageDataJD, "read_maxscore");
                    item.MaxWriteScore = C_Json.GetJsonKeyInt(stageDataJD, "write_maxscore");

                    StageDataItemList.Add(item);

                    index++;
                }
                else
                {
                    break;
                }
            }
        }

        Unlock_a();

        UID = PlayerData.UID;

        C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new StageData()));
    }

    private static void Unlock_a()
    {
        StageConfigItem stageConfigItem = StageConfig.GetStageConfigItem("a");
        if (stageConfigItem != null)
        {
            StageDataItem item = GetStageDataItem(stageConfigItem.ID);
            if (item == null || item.State == 0)
                SetStageDataItem(stageConfigItem.ID, 1, 0);
        }
    }

    public static StageDataItem GetStageDataItem(string stage)
    {
        StageConfigItem stageConfigItem = StageConfig.GetStageConfigItem(stage);
        if (stageConfigItem != null)
            return GetStageDataItem(stageConfigItem.ID);

        return null;
    }

    public static StageDataItem GetStageDataItem(int stageID)
    {
        for (int i = 0; i < StageDataItemList.Count; i++)
        {
            if (StageDataItemList[i] != null && StageDataItemList[i].ID == stageID)
                return StageDataItemList[i];
        }

        return null;
    }

    public static void SetStageDataItem(string stage, int state, int star, int knowStar = 0, int readStar = 0, int writeStar = 0)
    {
        StageConfigItem stageConfigItem = StageConfig.GetStageConfigItem(stage);
        if (stageConfigItem != null)
            SetStageDataItem(stageConfigItem.ID, state, star, knowStar, readStar, writeStar);
    }

    public static void SetStageDataItem(int stageID, int state, int star, int knowStar = 0, int readStar = 0, int writeStar = 0)
    {
        for (int i = 0; i < StageDataItemList.Count; i++)
        {
            if (StageDataItemList[i] != null && StageDataItemList[i].ID == stageID)
            {
                if (state > StageDataItemList[i].State)
                    StageDataItemList[i].State = state;

                if (star > StageDataItemList[i].MaxStar)
                    StageDataItemList[i].MaxStar = star;

                if (knowStar > StageDataItemList[i].MaxKnowStar)
                    StageDataItemList[i].MaxKnowStar = knowStar;

                if (readStar > StageDataItemList[i].MaxReadStar)
                    StageDataItemList[i].MaxReadStar = readStar;

                if (writeStar > StageDataItemList[i].MaxWriteStar)
                    StageDataItemList[i].MaxWriteStar = writeStar;

                return;
            }
        }

        StageDataItem item = new StageDataItem();

        item.ID = stageID;
        item.State = state;

        item.MaxStar = star;
        item.MaxKnowStar = knowStar;
        item.MaxReadStar = readStar;
        item.MaxWriteStar = writeStar;

        StageDataItemList.Add(item);

        C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new StageData()));
    }
}