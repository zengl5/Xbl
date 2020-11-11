using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.XWK.MainScene;

/*
0 相当于精灵上新，未获得
1 相当于已收集，未解锁，未阅
2 相当于已收集，未解锁，已阅
3 相当于已收集，已解锁，已阅
*/
public class WizardItem
{
    public string elfin = "";//精灵名字
    public int mtime = 0;
    public int state = 0;
    public int uid = 0;
    public int level=0;
    public int star = 0;
    public int progress = 0;
}
public class WizardItemName
{
    //有新增精灵，按照一下格式添加，作为设置精灵相关参数的名字参数
    public static string Wizard_BaiYin          = "baiyin";//百音精灵
    public static string Wizard_Hulu            = "bbhulu";//百变葫芦
    public static string Wizard_Xln             = "jl_xln";//小龙女
    public static string Wizard_hongbao         = "Not_Recommend_jl_00015";//红包
    public static string Wizard_nianshou        = "Not_Recommend_jl_00014";//年兽宝宝
    public static string Wizard_lingwa          = "jl_lingwa";//灵娃
    public static string Wizard_xcww            = "jl_xcww";//仙草娃娃
    public static string Wizard_jl_hxjl         = "jl_hxjl";//海宝
    public static string Wizard_xixilang        = "jl_xixilang";//西西狼jl_00010
    public static string Wizard_huohuolong      = "jl_huohuolong";//火火龙jl_00007
    public static string Wizard_menmenmiao      = "jl_menmenmiao";//萌萌喵jl_00005
    public static string Wizard_niuxiaoxian     = "jl_niuxiaoxian";//牛小仙 jl_00012
    public static string Wizard_xiuxiuwoniu     = "jl_xiuxiuwoniu";//咻咻蜗牛jl_00011
    public static string Wizard_jisuanji        = "jl_jisuanji";//计算鸡jl_00006
    public static string Wizard_dazuishou       = "jl_dazuishou";//大嘴兽jl_00004
    public static string future                 = "future";
}

public class WizardData
{
    public static string Name = "wizard_data";
    public static string uid = "";
    public static int UELen = 0;
    public static int currentspiritid = 0;
    public static List<WizardItem> UEList = new List<WizardItem>();

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
            UEList.Clear();
            JsonData UserElfinList = C_Json.GetJsonKeyJsonData(strData, "UEList");
            if (UserElfinList != null)
            {
                for (int index = 0; index < UserElfinList.Count; index++)
                {
                    JsonData spiritDataItemListJD = UserElfinList[index];
                    if (spiritDataItemListJD != null)
                    {
                        WizardItem item = new WizardItem();
                        item.elfin = C_Json.GetJsonKeyString(spiritDataItemListJD, "elfin");
                        item.state = C_Json.GetJsonKeyInt(spiritDataItemListJD, "state");
                        item.mtime = C_Json.GetJsonKeyInt(spiritDataItemListJD, "mtime");
                        item.level = C_Json.GetJsonKeyInt(spiritDataItemListJD, "level");
                        item.star = C_Json.GetJsonKeyInt(spiritDataItemListJD, "star");
                        item.uid = C_Json.GetJsonKeyInt(spiritDataItemListJD, "uid");
                        item.progress = C_Json.GetJsonKeyInt(spiritDataItemListJD, "progress");

                        UEList.Add(item);
                    }
                }
                currentspiritid = C_Json.GetJsonKeyInt(strData, "currentspiritid");

            } 
        }
        if (UEList!=null)
        {
            if (!IsWizardCollected(WizardItemName.Wizard_Xln))
            {
                AddWizardItem(WizardItemName.Wizard_Xln);
            }
            if (!IsWizardCollected(WizardItemName.Wizard_Hulu))
            {
                AddWizardItem(WizardItemName.Wizard_Hulu);
            }
            if (!IsWizardCollected(WizardItemName.Wizard_BaiYin))
            {
                AddWizardItem(WizardItemName.Wizard_BaiYin);
            }
        }
    }

    public static void Save(string strData)
    {
        if (string.IsNullOrEmpty(uid)
          || (!string.IsNullOrEmpty(uid) && !uid.Equals(PlayerData.UID)))
        {
            ClearRecommendPrefs();
        }
        UpdateData(strData);
        
        uid = PlayerData.UID;

        C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new WizardData()));
    }
    public static int CurrentCollectSpiritId
    {
        set{
            currentspiritid = value;
        }
        get
        {
            return currentspiritid;
        }
    }
    public static bool isLevelUp()
    {
        if (IsUpdateGradate(WizardItemName.Wizard_BaiYin)
            && IsUpdateGradate(WizardItemName.Wizard_Hulu))
        {
            return true;
        }
        return false;
    }
    public static List<WizardItem> GetWizardItemList()
    {
        return UEList;
    }
    public static WizardItem GetWizardItem(string name)
    {
        if (UEList==null )
        {
            return null;
        }
        for (int i = 0; i < UEList.Count; i++)
        {
            if (UEList[i] != null && UEList[i].elfin.Equals(name))
            {
                return UEList[i];
            }
        }
        return null;
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
        string itemData = JsonMapper.ToJson(UEList);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("elfinlist", itemData);
        C_DebugHelper.Log("SpiritData line 142 itemData:" + itemData);
        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_DataHost + HttpRequestConfig.SetUelfin, data, (string result) =>
        {
            C_DebugHelper.Log("SpiritData line 137 result:" + result);
        });
    }

    public static void FetchUelfinData(System.Action callback=null)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        C_Singleton<NetworkMgr>.GetInstance().PokeRequestHttp(GameDataMgr.c_DataHost + HttpRequestConfig.GetUelfin, null, (string result) =>
        {
            C_DebugHelper.Log("SpiritData line 150 result:" + result);
            Save(result);
            if (callback != null)
            {
                callback();
            }
        });
    }
    /// <summary>
    /// 获取精灵的等级进度
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static float FetchWizardItemLevelProgress(string name)
    {
        WizardItem item = GetWizardItem(name);
        if (item == null)
        {
            return 0;
        }
        return item.progress/100f;
    }

    /// <summary>
    /// 设置精灵的等级进度
    /// </summary>
    /// <param name="name"></param>
    /// <param name="progress">当前的升级进度，需要是可以随时覆盖，不需要做最大判断</param>
    public static void SetWizardItemLevelProgress(string name, float progress)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }
        int data = Convert.ToInt32(progress * 100);
        WizardItem item = GetWizardItem(name);
        if (item == null)
        {
            item = new WizardItem();
            item.elfin = name;
            item.progress =data;
            UEList.Add(item);
        }
        else
        {
            item.elfin = name;
            item.progress =data;
        }
        SaveData();
    }

    /// <summary>
    /// 获取精灵的等级
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static int FetchWizardItemLevel(string name)
    {
        WizardItem item = GetWizardItem(name);
        if (item ==null)
        {
            return 0; 
        }
        return item.level;
    }
    /// <summary>
    /// 设置精灵的等级
    /// </summary>
    /// <param name="name"></param>
    /// <param name="level"></param>
    public static void SetWizardItemLevel(string name,int level)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }
        WizardItem item = GetWizardItem(name);
        if (item==null)
        {
            item = new WizardItem();
            item.elfin = name;
            item.level = Mathf.Max(item.level, level);
            UEList.Add(item);
        }
        else
        {
            item.elfin = name;
            item.level = Mathf.Max(item.level, level);
            //item.level = level;
        }
        SaveData();
    }
   public static bool IsUpdateGradate(string name)
    {
        return (FetchWizardItemLevel(name) > 0);
    }
    protected static void SaveData()
    {
        uid = PlayerData.UID;

        C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new WizardData()));
    }
    /// <summary>
    /// 没有精灵的数据，新用户
    /// </summary>
    /// <returns></returns>
    public static bool IsNewUser()
    {
        if (UEList == null)
        {
            return true;
        }
        for (int i = 0; i < UEList.Count; i++)
        {
            if (UEList[i] != null && UEList[i].state > 2)
            {
                return false;
            }
        }
        return true;
    }
  
    /// <summary>
    /// 是否已经解锁
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool IsLockedWizardItem(string name)
    {
        WizardItem item = GetWizardItem(name);
        if (item!=null)
        {
            return item.state == 3 ? true : false;
        }
        return false;
    }
    /// <summary>
    /// 解锁精灵状态
    /// </summary>
    /// <param name="name"></param>
    public static void UnlockWizardItem(string name)
    {
        WizardItem item = GetWizardItem(name);
        if (item == null)
        {
            item = new WizardItem();
            item.elfin = name;
            item.state = 3;
            UEList.Add(item);
        }
        else
        {
            item.elfin = name;
            item.state = 3;
        }

        SaveData();
    }
    /// <summary>
    /// 精灵新增
    /// </summary>
    /// <param name="name"></param>
    public static void AddWizardItem(string name)
    {
        WizardItem item = GetWizardItem(name);
        if (item == null)
        {
            item = new WizardItem();
            item.elfin = name;
            item.state = 1;
            item.level = 0;
            item.progress = 0;
            item.star = 0;
            UEList.Add(item);
            SaveData();
        }
    }
    /// <summary>
    /// 获取精灵状态
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static int FetchWizardState(string name)
    {
        WizardItem item = GetWizardItem(name);
        if (item == null)
        {
            return -1;
        }
        return item.state;
    }
    /// <summary>
    /// 获取精灵的星星
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static int FetchWizardItemStar(string name)
    {
        WizardItem item = GetWizardItem(name);
        if (item==null)
        {
            return 0;
        }
        return item.star;
    }
    /// <summary>
    /// 设置精灵的星星
    /// </summary>
    /// <param name="name"></param>
    /// <param name="level"></param>
    public static void SetWizardItemStar(string name, int star)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }
        WizardItem item = GetWizardItem(name);
        if (item == null)
        {
            item = new WizardItem();
            item.elfin = name;
            item.star = Mathf.Max(item.star, star);
            UEList.Add(item);
        }
        else
        {
            item.elfin = name;
            item.star = Mathf.Max(item.star, star);
        }
        SaveData();
    }
    public static bool ShowRecommend()
    {
        bool showFlag = false;
        System.DateTime dateTime = DateTime.Now;
        string timePrefs = "SpiritAD_FirstEnterApp_Time";
        string time = string.Concat(dateTime.Year, "-", dateTime.Month, "-", dateTime.Day);
        if (!time.Equals(PlayerPrefs.GetString(timePrefs)))
        {
            showFlag = true;
            PlayerPrefs.SetString(timePrefs, time);
        }
        return showFlag;
    }
    public static void ClearRecommendPrefs()
    {
        PlayerPrefs.SetString("SpiritAD_FirstEnterApp_Time", "");
        PlayerPrefs.SetString(LocalData.m_Collect_Spirit_Prefs, ""); 
        PlayerPrefs.SetString("current_collect_spirit_name", "");
        ClearRecommendIconState();
    }
    public static void ClearRecommendIconState()
    {
        WizardData.SetRecommendIcon(WizardItemName.Wizard_lingwa, 0);
        WizardData.SetRecommendIcon(WizardItemName.Wizard_xcww, 0);
        WizardData.SetRecommendIcon(WizardItemName.Wizard_jl_hxjl, 0);
    }
    public static bool ShowRecommendIcon(string name)
    {
        int data = PlayerPrefs.GetInt(string.Concat("current_collect_spirit_", name), 0);
        return data == 0;
    }
    public static void SetRecommendIcon(string name,int vaule)
    {
        if (string.IsNullOrEmpty(name)) {
            return;
        }
        //else if (GetWizardItem(name) == null) {
        //    C_DebugHelper.LogError("SetRecommendIcon name is null.. ");
        //    return;
        //} 
        PlayerPrefs.SetInt(string.Concat("current_collect_spirit_", name), vaule);
    }
    public static void CurrentLocationRecommend(string name)
    {
        PlayerPrefs.SetString(LocalData.m_Collect_Spirit_Prefs, name);
    }
    //表示已阅读
    public static void SetRecommendIconState()
    {
        if (UEList == null)
        {
            return ;
        }
        for (int i = 0; i < UEList.Count; i++)
        {
            if (UEList[i] != null && UEList[i].state < 2)
            {
                 UEList[i].state = 2;
            }
        }
        SaveData();

    }
    //是否需要做上新推荐
    public static bool DosShowRecommendIcon(ref string wizardItemName)
    {
        wizardItemName = "";
        if (UEList == null)
        {
            return false;
        }
        if (UEList.Count <1)
        {
            return false;
        }
        WizardItem item = UEList[UEList.Count - 1];
        if (item==null)
        {
            return false;
        }
        wizardItemName = item.elfin;
        if (item!=null && item.state >1)//表示已阅
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// 0表示没有收集
    /// </summary>
    /// <param name="wizardItemName"></param>
    /// <returns></returns>
    public static bool IsWizardCollected(string wizardItemName)
    {
        WizardItem item = GetWizardItem(wizardItemName);
        if (item == null)
        {
            return false;
        }
        return item.state>0;
    }
}
