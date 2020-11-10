using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OfflineStageDataItem
{
    public int bagid = 0;
    public int state = 0;
    public int type = 0;

    public int score = 0;
}

public class OfflineData
{
    public static string Name = "offline_data";

    public static string UID = "";

    public static List<OfflineStageDataItem> OfflineStageDataItemList = new List<OfflineStageDataItem>();

    //public static List<ReadWriteDataItem> OfflineReadWriteDataItemList = new List<ReadWriteDataItem>();

    public static void Load()
    {
        string strData = C_MonoSingleton<C_DataMgr>.GetInstance().LoadData(Name);
        if (!string.IsNullOrEmpty(strData))
        {
            UID = C_Json.GetJsonKeyString(strData, "UID");
            if (!string.IsNullOrEmpty(PlayerData.UID) && UID == PlayerData.UID)
            {
                OfflineStageDataItemList.Clear();
                JsonData offlineStageDataItemListJD = C_Json.GetJsonKeyJsonData(strData, "OfflineStageDataItemList");
                if (offlineStageDataItemListJD != null)
                {
                    for (int i = 0; i < offlineStageDataItemListJD.Count; i++)
                    {
                        if (offlineStageDataItemListJD[i] != null)
                        {
                            OfflineStageDataItem item = new OfflineStageDataItem();

                            item.bagid = C_Json.GetJsonKeyInt(offlineStageDataItemListJD[i], "bagid");
                            item.state = C_Json.GetJsonKeyInt(offlineStageDataItemListJD[i], "state");
                            item.type = C_Json.GetJsonKeyInt(offlineStageDataItemListJD[i], "type");
                            item.score = C_Json.GetJsonKeyInt(offlineStageDataItemListJD[i], "score");

                            OfflineStageDataItemList.Add(item);
                        }
                    }
                }

                //OfflineReadWriteDataItemList.Clear();
                //JsonData offlineReadWriteDataItemListJD = C_Json.GetJsonKeyJsonData(strData, "OfflineReadWriteDataItemList");
                //if (offlineReadWriteDataItemListJD != null)
                //{
                //    for (int i = 0; i < offlineReadWriteDataItemListJD.Count; i++)
                //    {
                //        if (offlineReadWriteDataItemListJD[i] != null)
                //        {
                //            ReadWriteDataItem item = new ReadWriteDataItem();

                //            item.name = C_Json.GetJsonKeyString(offlineReadWriteDataItemListJD[i], "name");
                //            item.type = C_Json.GetJsonKeyInt(offlineReadWriteDataItemListJD[i], "type");
                //            item.count = C_Json.GetJsonKeyInt(offlineReadWriteDataItemListJD[i], "count");

                //            OfflineReadWriteDataItemList.Add(item);
                //        }
                //    }
                //}
            }
        }
    }

    public static void AddOfflineStageDataItem(int bagid, int state, int type, int score = 0)
    {
        OfflineStageDataItem item = new OfflineStageDataItem();
        item.bagid = bagid;
        item.state = state;
        item.type = type;
        item.score = score;

        OfflineStageDataItemList.Add(item);

        UID = PlayerData.UID;

        C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new OfflineData()));
    }

    //public static void AddReadWriteDataItem(string name, int type, int count)
    //{
    //    for (int i = 0; i < OfflineReadWriteDataItemList.Count; i++)
    //    {
    //        if (OfflineReadWriteDataItemList[i].name == name && OfflineReadWriteDataItemList[i].type == type)
    //        {
    //            OfflineReadWriteDataItemList[i].count += count;

    //            UID = PlayerData.UID;

    //            C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new OfflineData()));

    //            return;
    //        }
    //    }

    //    ReadWriteDataItem item = new ReadWriteDataItem();
    //    item.name = name;
    //    item.type = type;
    //    item.count = count;

    //    OfflineReadWriteDataItemList.Add(item);

    //    UID = PlayerData.UID;

    //    C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new OfflineData()));
    //}

    public static void Synchrodata()
    {
        if (NetworkMgr.IsConnected && (string.IsNullOrEmpty(UID) || UID == PlayerData.UID))
        {
            string strData = C_MonoSingleton<C_DataMgr>.GetInstance().LoadData(Name);
            if (!string.IsNullOrEmpty(strData))
            {
                OfflineStageDataItemList.Clear();
                JsonData offlineStageDataItemListJD = C_Json.GetJsonKeyJsonData(strData, "OfflineStageDataItemList");
                if (offlineStageDataItemListJD != null)
                {
                    for (int i = 0; i < offlineStageDataItemListJD.Count; i++)
                    {
                        OfflineStageDataItem item = new OfflineStageDataItem();

                        item.bagid = C_Json.GetJsonKeyInt(offlineStageDataItemListJD[i], "bagid");
                        item.state = C_Json.GetJsonKeyInt(offlineStageDataItemListJD[i], "state");
                        item.type = C_Json.GetJsonKeyInt(offlineStageDataItemListJD[i], "type");
                        item.score = C_Json.GetJsonKeyInt(offlineStageDataItemListJD[i], "score");

                        OfflineStageDataItemList.Add(item);
                    }
                }

                //OfflineReadWriteDataItemList.Clear();
                //JsonData offlineReadWriteDataItemListJD = C_Json.GetJsonKeyJsonData(strData, "OfflineReadWriteDataItemList");
                //if (offlineReadWriteDataItemListJD != null)
                //{
                //    for (int i = 0; i < offlineReadWriteDataItemListJD.Count; i++)
                //    {
                //        if (offlineReadWriteDataItemListJD[i] != null)
                //        {
                //            ReadWriteDataItem item = new ReadWriteDataItem();

                //            item.name = C_Json.GetJsonKeyString(offlineReadWriteDataItemListJD[i], "name");
                //            item.type = C_Json.GetJsonKeyInt(offlineReadWriteDataItemListJD[i], "type");
                //            item.count = C_Json.GetJsonKeyInt(offlineReadWriteDataItemListJD[i], "count");

                //            OfflineReadWriteDataItemList.Add(item);
                //        }
                //    }
                //}
            }

            //if (OfflineStageDataItemList.Count > 0)
               // C_Singleton<StageMgr>.GetInstance().SendStageData(JsonMapper.ToJson(OfflineStageDataItemList));

            // if (OfflineReadWriteDataItemList.Count > 0)
              //   C_Singleton<ReadWriteMgr>.GetInstance().SendReadWriteData(JsonMapper.ToJson(OfflineReadWriteDataItemList));

            OfflineStageDataItemList.Clear();
            //OfflineReadWriteDataItemList.Clear();
            C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new OfflineData()));
        }
    }
}