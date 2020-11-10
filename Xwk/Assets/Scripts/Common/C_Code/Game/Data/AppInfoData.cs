using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.XWK.MainScene;
public class AppInfoItem
{
    public int mtime;
    public int state;
    public int type;
    public int uid;
}
public class AppInfoData  {
    public static string Uid = "";
    public static string Name = "app_info_data";
    public static string NewyearclipName = "game_Newyearclip";
    public static string NewyearGameTime = "game_NewYearTime";
    public static string NewyearGameOver = "game_NewyearGameOver";
    public static List<AppInfoItem> UIList = new List<AppInfoItem>();
    public static bool EnterGameFlag = false;
    public static void Load()
    {
        EnterGameFlag = false;
        string strData = C_DataMgr.Instance.LoadData(Name);
        if (strData.Contains("Uid"))
        {
            Uid = C_Json.GetJsonKeyString(strData, "Uid");
        }
        UpdateData(strData,false);
    }
    public static void UpdateData(string strData,bool clearList = true)
    {
        if (string.IsNullOrEmpty(Uid)
            || (!string.IsNullOrEmpty(Uid) && !Uid.Equals(PlayerData.UID)))
        {
            ClearPlayerPrefs();
        }
        if (!string.IsNullOrEmpty(strData))
        {
            UIList.Clear();
            JsonData jsonData = C_Json.GetJsonKeyJsonData(strData, "UIList");
            if (jsonData != null)
            {
                for (int i = 0; i < jsonData.Count; i++)
                {
                    AppInfoItem appInfoItem = new AppInfoItem();
                    appInfoItem.state = C_Json.GetJsonKeyInt(jsonData[i], "state");
                    appInfoItem.type = C_Json.GetJsonKeyInt(jsonData[i], "type");
                    UIList.Add(appInfoItem);
                }
            }
        }
    }
    public static void UpdateData(string strData, int type)
    {
        if (string.IsNullOrEmpty(Uid)
       || (!string.IsNullOrEmpty(Uid) && !Uid.Equals(PlayerData.UID)))
        {
            ClearPlayerPrefs();
        }
        bool flag = false;

        if (!string.IsNullOrEmpty(strData))
        {
            JsonData jsonData = C_Json.GetJsonKeyJsonData(strData, "UInitial");
            if (jsonData != null)
            {
                for (int i = 0; i < UIList.Count; i++)
                {
                    if (UIList[i].type == type)
                    {
                        UIList[i].state = C_Json.GetJsonKeyInt(jsonData[i], "state");
                        flag = true;
                        break;
                    }
                }
            }
            if (!flag)
            {
                AppInfoItem appInfoItem = new AppInfoItem();
                appInfoItem.state = C_Json.GetJsonKeyInt(jsonData[jsonData.Count], "state");
                appInfoItem.type = type; 
                UIList.Add(appInfoItem);
            }
        }
    }
    public static void Save()
    {
       
        Uid = PlayerData.UID;
        C_DataMgr.Instance.SaveData(Name, JsonMapper.ToJson(new AppInfoData()));
    }
    static void setData(int type,int value)
    {
        bool flag = false;
        for (int i = 0; i < UIList.Count; i++)
        {
            if (UIList[i].type == type)
            {
                UIList[i].state = value;
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            AppInfoItem appInfoItem = new AppInfoItem();
            appInfoItem.state = value;
            appInfoItem.type = type;
            UIList.Add(appInfoItem);
        }

        Save();
    }
    static void setData(int type)
    {
        bool flag = false;
        for (int i = 0; i < UIList.Count; i++)
        {
            if (UIList[i].type == type)
            {
                UIList[i].state = 1;
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            AppInfoItem appInfoItem = new AppInfoItem();
            appInfoItem.state = 1;
            appInfoItem.type = type;
            UIList.Add(appInfoItem);
        }

        Save();
    }
    static int getData(int type)
    {
        int result = 0;
        for (int i = 0; i < UIList.Count; i++)
        {
            if (UIList[i].type == type)
            {
                result = UIList[i].state;
                break;
            }
        }
        return result;
    }
    /// <summary>
    /// 精灵故事引导
    /// </summary>
    public static int FetchStoryGuideType
    {
        set
        {
            setData(4);
        }
        get
        {
            return getData(4);
        }
    }
    /// <summary>
    /// 精灵引导
    /// </summary>
    public static int FetchWizardGuideType
    {
        set
        {
            setData(3);
        }
        get
        {
            return getData(3);
        }
    }
    public static int GuideStateData {
        set
        {
            setData(1);
        }
        get
        {
            return getData(1);
        }
    }
    /// <summary>
    /// 获取精灵上新的编号
    /// </summary>
    public static int FetchRecommendID {
        set
        {
            setData(5,value);
        }
        get
        {
            return getData(5);
        }
    }


    public static int GetJingubangStateData
    {
        set
        {
            setData(2);
        }
        get
        {
            return getData(2);
        }
    }
    public static void FetchAllStateData(System.Action callback = null)
    {
        Dictionary<string, int> data = new Dictionary<string, int>();
        C_Singleton<NetworkMgr>.GetInstance().PokeRequestHttp(GameDataMgr.c_DataHost + HttpRequestConfig.GetUinitial, null, (string result) =>
        {
            C_DebugHelper.Log("Appinfo line 114 result:" + result);
            UpdateData(result);
            Save();
            //发送消息用来刷新上新内容
         //   C_EventHandler.SendEvent(C_EnumEventChannel.Global, "PlayerData", 2);
            if (callback != null)
            {
                callback();
            }
        });
    }
    public static void FetchStateData(int type)
    {
        WWWForm form = new WWWForm();
        form.AddField("type", type);
        C_Singleton<NetworkMgr>.GetInstance().PokeRequestHttp(form,GameDataMgr.c_DataHost + HttpRequestConfig.GetUinitial, (string result) =>
        {
            C_DebugHelper.Log("Appinfo line 140 result:" + result);
            UpdateData(result, type);
            Save();
        });
    }

    public static void SynchroData()
    {
        if (string.IsNullOrEmpty(Uid))
        {
            return;
        }
        if (!string.IsNullOrEmpty(Uid) && !Uid.Equals(PlayerData.UID))
        {
            ClearPlayerPrefs();
            return;
        }
        string itemData = JsonMapper.ToJson(UIList);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("initiallist", itemData);
        C_DebugHelper.Log("SysncData.. Appinfodata line 141 itemData:" + itemData);
        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_DataHost + HttpRequestConfig.SetUinitial, data, (string result) =>
        {
            C_DebugHelper.Log("SysncData.. Appinfodata line 144 result:" + result);
        });
    }
    public static int TotalNewYearClipSum()
    {
        return 9;
    }
    public static bool AutoCollectNewYearClip()
    {
        int spId = PlayerPrefs.GetInt(NewyearclipName, 0);
        return ((spId < TotalNewYearClipSum())
            && IsNewYearGameLastDay());
    }
    public static bool IsCollectUI()
    {
        int id = FetchNewYearChip();
        return id < (TotalNewYearClipSum()+1) && id > 0;
    }
    public static int FetchNewYearChip()
    { 
        int spId = PlayerPrefs.GetInt(NewyearclipName, 0);
        //if (AutoCollectNewYearClip())
        //{
        //    spId = TotalNewYearClipSum();
        //    SetNewYearChip(spId);
        //}
        return spId;
    }
    public static void SetNewYearChip(int id)
    {
        PlayerPrefs.SetInt(NewyearclipName,id);
    }
    public static void ResumehNewYearChip()
    {
        SetNewYearChip(0);
    }
    public static bool IsNewYearGameLastDay()
    {
        System.DateTime time = DateTime.Now;
        string data = String.Format("{0:D4}{1:D2}{2:D2}", time.Year, time.Month, time.Day);
        return ( int.Parse(data.ToString()) >= 20200201);
    }
    public static double FetchNewYearGameLeaveTime()
    {
        System.DateTime time = DateTime.Now;
        System.DateTime.TryParse(FetchNewYearGameTime(), out time);
        System.TimeSpan timeSpan = time.Subtract(System.DateTime.Now);
        if (timeSpan.TotalSeconds > LocalData.monsterGameleaveTime)
        {
            ResumehNewYearGameTime();
            timeSpan = time.Subtract(System.DateTime.Now);
        }
        return timeSpan.TotalSeconds;
    }
    public static string FetchNewYearGameTime()
    {
       return PlayerPrefs.GetString(NewyearGameTime, System.DateTime.Now.ToString());
    }
    public static void ResumehNewYearGameTime()
    {
        EnterGameFlag = false;
#if UNITY_EDITOR
        SetNewYearGameTime(DateTime.Now.AddSeconds(LocalData.monsterGameleaveTime).ToString());
#else
       
         SetNewYearGameTime(DateTime.Now.AddMinutes(5).ToString());
#endif
    }
    public static void CleanNewYearGameTime()
    {
        EnterGameFlag = false;
        SetNewYearGameTime(System.DateTime.Now.ToString());
    }

    public static void SetNewYearGameTime(string time)
    {
        PlayerPrefs.SetString(NewyearGameTime, time);
    }
    public static bool CanEntergame()
    {
        return EnterGameFlag;
    }
    public static bool IsComeBackFromNewYearGame()
    {
        return PlayerPrefs.GetInt(NewyearGameOver, 0)==1;
    }
    public static void SetComeBackFromNewYearGame()
    {
        PlayerPrefs.SetInt(NewyearGameOver, 1);
    }
    public static void ResetComeBackFromNewYearGame()
    {
        PlayerPrefs.SetInt(NewyearGameOver, 0);
    }
    public static void ClearPlayerPrefs()
    {
        ResetComeBackFromNewYearGame();
        ResumehNewYearChip();
        CleanNewYearGameTime();
    }
}
