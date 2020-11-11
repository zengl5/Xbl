using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StoreConfigItem
{
    public int Type = 0;
    public int Trigger = 0;
    public string Voice = "";
    public string Img = "";
}

public class StoreConfig
{
    public static string Name = "store_config";

    public static List<StoreConfigItem> StoreConfigItemList = new List<StoreConfigItem>();

    public static void Load()
    {
        string strData = C_MonoSingleton<C_DataMgr>.GetInstance().LoadData(Name);
        if (!string.IsNullOrEmpty(strData))
        {
            StoreConfigItemList.Clear();

            JsonData storeConfigItemListJD = C_Json.GetJsonKeyJsonData(strData, "StoreConfigItemList");
            if (storeConfigItemListJD != null)
            {
                for (int i = 0; i < storeConfigItemListJD.Count; i++)
                {
                    if (storeConfigItemListJD[i] != null)
                    {
                        StoreConfigItem item = new StoreConfigItem();

                        item.Type = C_Json.GetJsonKeyInt(storeConfigItemListJD[i], "Type");
                        item.Trigger = C_Json.GetJsonKeyInt(storeConfigItemListJD[i], "Trigger");
                        item.Voice = C_Json.GetJsonKeyString(storeConfigItemListJD[i], "Voice");
                        item.Img = C_Json.GetJsonKeyString(storeConfigItemListJD[i], "Img");

                        StoreConfigItemList.Add(item);
                    }
                }
            }
        }
    }

    public static void Save(string strData)
    {
        if (!string.IsNullOrEmpty(strData))
        {
            StoreConfigItemList.Clear();

            JsonData configJD = JsonMapper.ToObject(strData);
            if (configJD != null)
            {
                int i = 0;
                while (true)
                {
                    JsonData itemJD = C_Json.GetJsonKeyJsonData(configJD, i.ToString());
                    if (itemJD == null)
                        break;
                    if (C_Json.GetJsonKeyInt(itemJD, "state") == 1)
                    {
                        StoreConfigItem item = new StoreConfigItem();

                        item.Type = C_Json.GetJsonKeyInt(itemJD, "type");
                        item.Trigger = C_Json.GetJsonKeyInt(itemJD, "trigger");
                        item.Voice = C_Json.GetJsonKeyString(itemJD, "voice");
                        item.Img = C_Json.GetJsonKeyString(itemJD, "img");

                        StoreConfigItemList.Add(item);
                    }

                    i++;
                }
            }

            C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new StoreConfig()));

            C_Singleton<GameDataMgr>.GetInstance().DownloadStoreSprite();
        }
    }

    public static StoreConfigItem GetStoreConfig(int type)
    {
        for (int i = 0; i < StoreConfigItemList.Count; i++)
        {
            if (StoreConfigItemList[i] != null && StoreConfigItemList[i].Type == type)
                return StoreConfigItemList[i];
        }

        return null;
    }
}