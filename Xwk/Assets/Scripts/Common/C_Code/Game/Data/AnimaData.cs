using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class AnimaData {
    public static int uid = 0;
    public static int nimbus = 0;
    public static string Name = "anima_data";
    public static int gainNimbus = 0;
    public static void Load()
    {
        gainNimbus = 0;
        string strData = C_DataMgr.Instance.LoadData(Name);
        if (strData.Contains("uid"))
        {
            uid = C_Json.GetJsonKeyInt(strData, "uid");
        }
        UpdateData(strData);
    }
    public static void UpdateData(string strData)
    {
        if (!string.IsNullOrEmpty(strData))
        {
            nimbus = C_Json.GetJsonKeyInt(strData, "nimbus");
            nimbus = Math.Max(nimbus, 0);
        }
    }
    public static void Save()
    {
        if (string.IsNullOrEmpty(PlayerData.UID))
        {
            return;
        }
        else
        {
            uid = Convert.ToInt32(PlayerData.UID);
        }
        C_DataMgr.Instance.SaveData(Name, JsonMapper.ToJson(new AnimaData()));
    }
    public static void SynchroData()
    {
        if (uid == 0)
        {
            return;
        }
        if (uid!=0 && !uid.ToString().Equals(PlayerData.UID))
        {
            return;
        }
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("nimbus", nimbus.ToString());
        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_DataHost + HttpRequestConfig.SetUnimbus, data, (string result) =>
        {
            C_DebugHelper.Log("SysncData.. Appinfodata line 144 result:" + result);
        });
    }
    public static void FetchData(System.Action callback = null)
    {
        C_Singleton<NetworkMgr>.GetInstance().PokeRequestHttp(GameDataMgr.c_DataHost + HttpRequestConfig.GetUnimbus, null, (string result) =>
        {
            C_DebugHelper.Log("SysncData.. Appinfodata line 144 result:" + result);
            UpdateData(result);
            Save();
         //   C_EventHandler.SendEvent(C_EnumEventChannel.Global, "PlayerData",1);
            if (callback != null)
            {
                callback();
            }
        });
    }
    public static int  TotalNimbus//0
    {
        set
        {
            nimbus = Math.Max(value,0);
            Save();
        }
        get
        {
            return nimbus;
        }
    }
    public static int GainNimbus {
        set
        {
            gainNimbus = value; 
        }
        get
        {
            return gainNimbus;
        }
    }

}
