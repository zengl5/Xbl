using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyBounsItem
{
    public string name = "";
    public int date =0;// 20191216
    public int state = 0; 
}
public class DailyBounsName
{
    //每一个需要奖励的模块名字,命名规范：游戏的一定要包含"game_",格式："game_xx",故事格式："story_xx"
    public static string DailyBouns_Story_Byjl= "story_baiyin";
    public static string DailyBouns_Story_Hulu = "story_bbhulu";
    public static string DailyBouns_Story_Ln= "story_ln";
    public static string DailyBouns_Game_Ggb = "game_jgb";
    public static string DailyBouns_Game_fss = "game_fss";
    public static string DailyBouns_Game_byjl = "game_byjl";
    public static string DailyBouns_Game_bbhl = "game_bbhl"; 
}

public class DailyBounsData  {
    public static string Name = "daily_bouns_data";
    public static string uid = "";
    public static List<DailyBounsItem> URList = new List<DailyBounsItem>();
    public static void Load()
    {
        string strData = C_MonoSingleton<C_DataMgr>.GetInstance().LoadData(Name);
        if (strData.Contains("uid"))
        {
            uid = C_Json.GetJsonKeyString(strData, "uid");
        }

        UpdateData(strData,false);
    }
    public static void UpdateData(string strData,bool clearList = true)
    {
        if (!string.IsNullOrEmpty(strData))
        {
            URList.Clear();

            JsonData UserElfinList = C_Json.GetJsonKeyJsonData(strData, "URList");
            if (UserElfinList != null)
            {
                for (int index = 0; index < UserElfinList.Count; index++)
                {
                    JsonData spiritDataItemListJD = UserElfinList[index];
                    if (spiritDataItemListJD != null)
                    {
                        DailyBounsItem item = new DailyBounsItem();
                        item.name = C_Json.GetJsonKeyString(spiritDataItemListJD, "name");
                        item.date = C_Json.GetJsonKeyInt(spiritDataItemListJD, "date");
                        item.state = C_Json.GetJsonKeyInt(spiritDataItemListJD, "mtime");

                        URList.Add(item);
                    }
                }
            }

        }
    }

    public static void Save(string strData)
    {
        if (string.IsNullOrEmpty(strData))
        {
            return;
        }
        UpdateData(strData);

        uid = PlayerData.UID;

        C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new DailyBounsData()));
    }
    public static void Synchrodata()
    {
        if (string.IsNullOrEmpty(uid))
        {
            return;
        }
        if (!string.IsNullOrEmpty(uid) && !uid.Equals(PlayerData.UID))
        {
            return;
        }
        string itemData = JsonMapper.ToJson(URList);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("rewardlist", itemData);
        C_DebugHelper.Log("SpiritData line 142 itemData:" + itemData);
        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_DataHost + HttpRequestConfig.SetUreward, data, (string result) =>
        {
            C_DebugHelper.Log("SpiritData line 137 result:" + result);
        });
    }

    public static void FetchUrewardData(System.Action callback = null)
    {
        DateTime dateTime = System.DateTime.Now;
        
        WWWForm form = new WWWForm();
        form.AddField("date", int.Parse(string.Concat(dateTime.Year, dateTime.Month, dateTime.Day)));
        C_Singleton<NetworkMgr>.GetInstance().PokeRequestHttp(form,GameDataMgr.c_DataHost + HttpRequestConfig.GetUreward, (string result) =>
        {
            C_DebugHelper.Log("SpiritData line 150 result:" + result);
            Save(result);
            if (callback != null)
            {
                callback();
            }
        });
    }
    public static void FetchUrewardData(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            FetchUrewardData();
            return;
        }
        DateTime dateTime = System.DateTime.Now;

        WWWForm form = new WWWForm();
        form.AddField("date", int.Parse(string.Concat(dateTime.Year, dateTime.Month, dateTime.Day)));
        form.AddField("name", name);
        C_Singleton<NetworkMgr>.GetInstance().PokeRequestHttp(form, GameDataMgr.c_DataHost + HttpRequestConfig.GetUreward, (string result) =>
        {
            C_DebugHelper.Log("SpiritData line 150 result:" + result);
            Save(result);
        });
    }
     
    /// <summary>
    /// 获取对应模块的每日奖励灵气值
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static int FetchDailBounsDataState(string name)
    {
        DateTime dateTime = System.DateTime.Now;
        int time = int.Parse(string.Concat(dateTime.Year, dateTime.Month, dateTime.Day));
        if (!IsContain(name))
        {
           // SetDailBounsDataState(name);
            return 0;
        }
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return UnConnectNetWork(name, time);
        }
        return ConnectNetWork(name, time);
    }
    private static int ConnectNetWork(string name,int time)
    {
        int state = 0;
        for (int i = 0; i < URList.Count; i++)
        {
            if (URList[i].name.Equals(name))
            {
                if (time == URList[i].date)
                {
                    state = URList[i].state;
                }
                else
                {
                    state = 1;
                }
                break;
            }
        }
        return state;
    }
    private static  int UnConnectNetWork(string name, int time)
    {
        int state = 0;
        for (int i = 0; i < URList.Count; i++)
        {
            if (URList[i].name.Equals(name))
            {
                if (time == URList[i].date)
                {
                    state = URList[i].state;
                }
                else if (time < URList[i].date)
                {
                    state = 1;
                }
                else
                {
                    state = 0;
                }
                break;
            }
        }
        return state;
    }
    public static void SetDailBounsDataState(int type)
    {
        if (type == 1)
        {
            DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Game_byjl);
        }
        else if (type == 2)
        {
            DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Game_bbhl);
        }
        else if (type == 3)
        {
            DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Game_Ggb);
        }
        else
        {
            DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Game_fss);
        }
    }
    public static bool IsUnLock(int type)
    {
        int state = 0;
        if (type == 1)
        {
            state =  DailyBounsData.FetchDailBounsDataState(DailyBounsName.DailyBouns_Game_byjl);
        }
        else if (type == 2)
        {
            state =  DailyBounsData.FetchDailBounsDataState(DailyBounsName.DailyBouns_Game_bbhl);
        }
        else if (type == 3)
        {
            state = DailyBounsData.FetchDailBounsDataState(DailyBounsName.DailyBouns_Game_Ggb);
        }
        else
        {
            state =  DailyBounsData.FetchDailBounsDataState(DailyBounsName.DailyBouns_Game_fss);
        }
        return (state==0);
    }
    public static bool IsUnLock(string name)
    {
        return (DailyBounsData.FetchDailBounsDataState(name) == 0);
    }
    /// <summary>
    /// 设置对应模块的每日奖励灵气值
    /// </summary>
    /// <param name="name"></param>
    public static void SetDailBounsDataState(string name)
    {
        DateTime dateTime = System.DateTime.Now;

        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("data", string.Concat(dateTime.Year, dateTime.Month, dateTime.Day));
        DailyBounsItem item = FetchItem(name);
        if (item !=null)
        {
            item.date = int.Parse(string.Concat(dateTime.Year, dateTime.Month, dateTime.Day));
            item.state = 1;
        }
        else
        {
            item = new DailyBounsItem();
            item.name = name;
            item.date =int.Parse( string.Concat(dateTime.Year, dateTime.Month, dateTime.Day));
            item.state = 1;
            URList.Add(item);
        }
        SaveData();
    }
    public static bool LeaveBouns()
    {
        bool ret = false;
        for (int i = 0; i < URList.Count; i++)
        {

            if (URList[i].state==0)
            {
                ret = true;
                break;
            }
        }
        return ret;
    }
    public static bool LeaveBouns(string name)
    {
        return FetchDailBounsDataState(name) == 0;
    }
    private static bool IsContain(string name)
    {
        bool contain = false;
        for (int i = 0; i < URList.Count; i++)
        {
            if (URList[i].name.Equals(name))
            {
                contain = true;
                URList[i].state = 1;
                break;
            }
        }
        return contain;
    }
    private static DailyBounsItem FetchItem(string name)
    {
        DailyBounsItem item = null;
        for (int i = 0; i < URList.Count; i++)
        {
            if (URList[i].name.Equals(name))
            {
                item = URList[i];
                break;
            }
        }
        return item;
    }
    public static bool UnCollectHomePageAllGameBouns()
    {
        bool ret = true;
        for (int i = 0; i < URList.Count; i++)
        {
            if (URList[i].state!=0 &&URList[i].name.ToLower().Contains("game_"))
            {
                ret = false;
                break;
            }
        }
        return ret;
    }
    public static bool UnCollectAllBouns()
    {
        bool ret = true;
        for (int i = 0; i < URList.Count; i++)
        {
            if (URList[i].state != 0)
            {
                ret = false;
                break;
            }
        }
        return ret;
    }

    protected static void SaveData()
    {
        uid = PlayerData.UID;
        C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new DailyBounsData()));
    }

}
