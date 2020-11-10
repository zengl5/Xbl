using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 可废弃
/// </summary>
public class RecommendSpiritItem
{
    public string name = "";
    public int date = 0;// 20191216
    public int state = 0;
    public int type = 1;
}
public class RecommendSpiritData
{
    public static string Name = "recommend_spirit_data";
    public static string uid = "";
    public static RecommendSpiritItem UReward; 
    public static List<RecommendSpiritItem> URList = new List<RecommendSpiritItem>();
    public static string SpiritName = "recommend_spirit";
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
            URList.Clear();
            //UReward.Clear();
            JsonData UserElfinList = C_Json.GetJsonKeyJsonData(strData, "UReward");
            if (UserElfinList != null)
            {
                UReward = new RecommendSpiritItem();
                UReward.name = C_Json.GetJsonKeyString(UserElfinList, "name");
                UReward.date = C_Json.GetJsonKeyInt(UserElfinList, "date");
                UReward.state = C_Json.GetJsonKeyInt(UserElfinList, "state");
                UReward.type = 1;// C_Json.GetJsonKeyString(spiritDataItemListJD, "type");

                URList.Add(UReward);
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

        C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new RecommendSpiritData()));
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
        //注意提交时list才可以
        string itemData = JsonMapper.ToJson(URList);
      
        WWWForm form = new WWWForm();
        form.AddField("rewardlist", itemData);
       // form.AddField("type", 1);
        C_Singleton<NetworkMgr>.GetInstance().PokeRequestHttp(form, GameDataMgr.c_DataHost + HttpRequestConfig.SetUreward, (string result) =>
        {
            C_DebugHelper.Log("result:"+ result);
        });
    }

    public static void FetchUrewardData(System.Action callback = null)
    {
        DateTime dateTime = System.DateTime.Now;

        WWWForm form = new WWWForm();
        form.AddField("date", int.Parse(string.Concat(dateTime.Year, dateTime.Month, dateTime.Day)));
        form.AddField("name", SpiritName);
        form.AddField("type", 1);
        C_Singleton<NetworkMgr>.GetInstance().PokeRequestHttp(form, GameDataMgr.c_DataHost + HttpRequestConfig.GetUreward, (string result) =>
        {
            C_DebugHelper.Log("99result:" + result);

            Save(result);
            if (callback != null)
            {
                callback();
            }
        });
    }

    /// <summary>
    /// 获取推荐精灵的状态
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool FetchRecommendState()
    {
        bool allowflag = false;
    //    RecommendSpiritItem item = FetchItem(SpiritName);
        if (UReward != null && UReward.state <= 0)
        {
            allowflag = true;
        }
        else
        {
            //如果没有获取，则自动设置获取
            SetRecommendState();
        }
        
        return allowflag;
    }
    //private static RecommendSpiritItem FetchItem(string name)
    //{
    //    RecommendSpiritItem item = null;
    //    for (int i = 0; i < UReward.Count; i++)
    //    {
    //        if (UReward[i].name.Equals(name))
    //        {
    //            item = UReward[i];
    //            break;
    //        }
    //    }
    //    return item;
    //}
    /// <summary>
    /// 设置推荐状态
    /// </summary>
    /// <param name="name"></param>
    public static void SetRecommendState()
    {
        DateTime dateTime = System.DateTime.Now;
 
      //  RecommendSpiritItem item = FetchItem(SpiritName);
        if (UReward != null)
        {
            UReward.name = SpiritName;
            UReward.date = int.Parse(string.Concat(dateTime.Year, dateTime.Month, dateTime.Day));
            UReward.state = 1;
            UReward.type = 1;
        }
        else
        {
            UReward = new RecommendSpiritItem();
            UReward.name = SpiritName;
            UReward.date = int.Parse(string.Concat(dateTime.Year, dateTime.Month, dateTime.Day));
            UReward.state = 1;
            UReward.type = 1;
        }
        SaveData();
    }

    protected static void SaveData()
    {
        uid = PlayerData.UID;
        C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new RecommendSpiritData()));
    }
    
}