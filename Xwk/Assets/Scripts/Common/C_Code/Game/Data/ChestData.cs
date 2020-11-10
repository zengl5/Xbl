using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChestItem {
    public string elfins = "";//精灵名字
    public int mtime = 0;
    public int sign_state = 0;
    public int sign_time = 0;
    public string treasure_name = "";
    public int star = 0;
}

public class ChestData {

    public static string Name = "chest_data";
    public static string uid = "";
    public static string rewards = "";
    public static List<ChestItem> UTList = new List<ChestItem>();

    public static void Load()
    {
        string strData = C_MonoSingleton<C_DataMgr>.GetInstance().LoadData(Name);
        if (strData.Contains("uid"))
        {
            uid = C_Json.GetJsonKeyString(strData, "uid");
        }

        UpdateData(strData, false);
    }
    public static void UpdateData(string strData, bool clearList = true)
    {
        if (!string.IsNullOrEmpty(strData))
        {
            UTList.Clear();
            JsonData UserElfinList = C_Json.GetJsonKeyJsonData(strData, "UTList");
            if (UserElfinList != null)
            {
                for (int index = 0; index < UserElfinList.Count; index++)
                {
                    JsonData spiritDataItemListJD = UserElfinList[index];
                    if (spiritDataItemListJD != null)
                    {
                        ChestItem item = new ChestItem();
                       // item.elfins = C_Json.GetJsonKeyString(spiritDataItemListJD, "elfins");
                        item.mtime = C_Json.GetJsonKeyInt(spiritDataItemListJD, "mtime");
                        item.sign_state = C_Json.GetJsonKeyInt(spiritDataItemListJD, "sign_state");
                        item.sign_time = C_Json.GetJsonKeyInt(spiritDataItemListJD, "sign_time");
                        item.treasure_name = C_Json.GetJsonKeyString(spiritDataItemListJD, "treasure_name");
                        UTList.Add(item);
                    }
                }
            }
        }
    }

    public static void Save(string strData)
    {
        rewards = "";

        UpdateData(strData);

        uid = PlayerData.UID;

        C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new ChestData()));
    }
    public static void SynchroData(System.Action<int, string> callback)
    {
        if (string.IsNullOrEmpty(uid))
        {
            return;
        }
        if (!uid.Equals(PlayerData.UID))
        {
            return;
        }
        if (UTList == null || (UTList != null && UTList.Count <= 0))
        {
            return;
        }

        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("name", UTList[0].treasure_name);
        data.Add("state", UTList[0].sign_state.ToString());
        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_DataHost + HttpRequestConfig.Setureward, data, (string result) =>
        {
            C_DebugHelper.Log("SysncData.. ChestData line 85 result:" + result);
            try
            {
                if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
                {
                    rewards = C_Json.GetJsonKeyString(result, "rewards");
                    int number;
                    //尝试把 input 变为整数(integer), 并储入 number 中
                    if (int.TryParse(rewards, out number))
                    {
                        callback(1, number.ToString());
                    }
                    else
                    {
                        callback(2, rewards);
                    }
                    return;
                }
                else
                {
                    Tips.Create("精灵上新获取数据失败");
                    callback(0, rewards);
                }
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("ResponseLoginResult : " + e);
            }
        });
    }

    public static void FetchChestData(System.Action callback = null)
    {
        C_Singleton<NetworkMgr>.GetInstance().PokeRequestHttp(GameDataMgr.c_DataHost + HttpRequestConfig.Getureward, null, (string result) =>
        {
            C_DebugHelper.Log("ChestData line 94 result:" + result);
            Save(result);
            if (callback != null)
            {
                callback();
            }
        });
    }
    public static bool isChestLock()
    {
        return FetchChestState() == 0 ? true : false;
    }
    public static bool isChestavailable()
    {
        return FetchChestState() == 1 ? true : false;
    }
    public static bool isChestReceive()
    {
        return FetchChestState() == 2 ? true : false;
    }
    //获取状态，进行不同的显示主界面入口
    public static int FetchChestState()
    {
        if (UTList == null || (UTList != null && UTList.Count <= 0))
        {
            return 0;
        }
        return UTList[0].sign_state;
    }
    //获取剩余的时间
    public static TimeSpan FetchLeaveTime()
    {
        if (UTList == null || (UTList != null && UTList.Count <= 0))
        {
            return new TimeSpan(0, 0, 0);
        }
        //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        int lTime = int.Parse(UTList[0].sign_time.ToString());
        //TimeSpan toNow = new TimeSpan(lTime);
        //DateTime dtResult = dtStart.Add(toNow);
        //TimeSpan span = dtResult - DateTime.Now;
        //return span;

        
        int hour = (int)lTime / 3600;
        int minute = ((int)lTime - hour * 3600) / 60;
        int second = (int)lTime - hour * 3600 - minute * 60;

        //只剩下秒，强制设置为1分钟
        if (hour <=0 && minute<=0 && second>0&& second<=60)
        {
            minute = 1;
            second = 0;
        }

        return new TimeSpan(hour, minute, second);
    }
    public static void ReceiveChest(System.Action<int,string> callback)
    {
        if (UTList == null || (UTList != null && UTList.Count <= 0))
        {
            return;
        }
        UTList[0].sign_state = 2;
        uid = PlayerData.UID;
        C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new ChestData()));

        //上报
        SynchroData(callback);
    }
    public static string FetchChestEffect()
    {
        if (UTList == null || (UTList != null && UTList.Count <= 0))
        {
            return "ui_public_effect_jldsj_bxdk";
        }
        if (("Bronze").Equals(UTList[0].treasure_name))
        {
            return "ui_public_effect_jldsj_bxdk";
        }
        else if (("Siver").Equals(UTList[0].treasure_name))
        {
            return "ui_public_effect_jldsj_bxdk01";
        }
        else  
        {
            return "ui_public_effect_jldsj_bxdk02";
        }
    }
    //获取当前的精灵所属宝箱类型
    public static string FetchChestType()
    {
        if (UTList == null || (UTList != null && UTList.Count <= 0))
        {
            return "bg_daka_bx_1";
        }
        if (("Bronze").Equals(UTList[0].treasure_name))
        {
            return "bg_daka_bx_1";
        }
        else if (("Siver").Equals(UTList[0].treasure_name))
        {
            return "bg_daka_bx_3";
        }
        else if (("Gold").Equals(UTList[0].treasure_name))
        {
            return "bg_daka_bx_5";
        }
        else if (("Platinum").Equals(UTList[0].treasure_name))
        {
            return "bg_daka_bx_7";
        }
        else if (("Dimond").Equals(UTList[0].treasure_name))
        {
            return "bg_daka_bx_9";
        }
        return "bg_daka_bx_1";
    }
    public static string FetchChestOpenUI()
    {
        if (UTList == null || (UTList != null && UTList.Count <= 0))
        {
            return "bg_daka_bx_2";
        }
        if (("Bronze").Equals(UTList[0].treasure_name))
        {
            return "bg_daka_bx_2";
        }
        else if (("Siver").Equals(UTList[0].treasure_name))
        {
            return "bg_daka_bx_4";
        }
        else if (("Gold").Equals(UTList[0].treasure_name))
        {
            return "bg_daka_bx_6";
        }
        else if (("Platinum").Equals(UTList[0].treasure_name))
        {
            return "bg_daka_bx_8";
        }
        else if (("Dimond").Equals(UTList[0].treasure_name))
        {
            return "bg_daka_bx_10";
        }
        return "bg_daka_bx_2";
    }
}
