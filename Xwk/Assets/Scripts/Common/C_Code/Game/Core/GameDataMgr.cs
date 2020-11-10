using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GameDataMgr : C_Singleton<GameDataMgr>
{
    public static String img_Test = "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";
    public static string c_DeviceUID = "";
    public static string c_UDID = "";
    public static int c_WeChatEnvironment = 1;
    public static string c_Channel = "xblpy_test";
    public static int c_DeviceType = 1;
    public static int c_FreeSpace = 1000;
    public static int c_Debug = 1;

    public static string c_HotUpdate = "";
    public static string c_Host = "";
    public static string c_DataHost = "";
    public static string c_PinYinHost = "";
    public static string c_PayHost = "";
    public static string c_CommonHost = "";

    private string m_strHeadImg = "";
    private Sprite m_AvatarSprite = null;
    public Sprite AvatarSprite
    {
        get
        {
            if (string.IsNullOrEmpty(PlayerData.HeadImg))
            {
                Texture2D texture = null;

                if (PlayerData.BabyGender == "-1" || PlayerData.BabyGender == "0")
                    texture = C_ResMgr.LoadResource<Texture2D>("boy", LocalPath.AvatarResourcesPath);
                else
                    texture = C_ResMgr.LoadResource<Texture2D>("girl", LocalPath.AvatarResourcesPath);

                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                if (m_strHeadImg == PlayerData.HeadImg)
                    return m_AvatarSprite;

                string filePath = C_LocalPath.DataPath + C_String.GetFileName(PlayerData.HeadImg);
                if (!File.Exists(filePath))
                    C_UnityWebRequestDownloader.SyncDownloadFile(PlayerData.HeadImg, C_LocalPath.DataPath);

                if (File.Exists(filePath))
                {
                    byte[] bytes = File.ReadAllBytes(filePath);

                    Texture2D texture = new Texture2D(500, 500);
                    texture.LoadImage(bytes);

                    m_AvatarSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    m_strHeadImg = PlayerData.HeadImg;
                }

                return m_AvatarSprite;
            }
        }
    }

    private int m_nCurLevel = 0;
    public int CurLevel
    {
        get
        {
            m_nCurLevel = 0;

            //for (int i = 0; i < LevelConfig.LevelMaxStar.Length; i++)
            //{
            //    if (PlayerData.StarCount > LevelConfig.LevelMaxStar[i] && StageMgr.IsUnlockStage(LevelConfig.LevelUnlockStage[i + 1]))
            //        m_nCurLevel = i + 1;
            //}

            return m_nCurLevel;
        }
    }

    private int m_nCurGrade = 0;
    public int CurGrade
    {
        get
        {
            //for (int i = 0; i < LevelConfig.GradeMaxStar.Length; i++)
            //{
            //    if (PlayerData.StarCount > LevelConfig.GradeMaxStar[i] && StageMgr.IsUnlockStage(LevelConfig.GradeUnlockStage[i]))
            //        m_nCurGrade = i + 1;
            //}

            return m_nCurGrade;
        }
    }

    public Sprite c_StoreSprite = null;

    protected override void Init()
    {
#if UNITY_ANDROID
        c_DeviceType = 1;
#elif UNITY_IOS
        c_DeviceType = 0;
#endif
    }
    //to do 未来修改为每次同步账号都要先同步数据结束再进行拉去新数据
    public void Synchrodata()
    {
        AppInfoData.SynchroData();
        WizardData.Synchrodata();
        AnimaData.SynchroData();
        DailyBounsData.Synchrodata();
        RecommendSpiritData.Synchrodata();
    }
         
    public void LoadData()
    {
        PlayerData.Load();
        WizardData.Load();
        AppInfoData.Load();
        AnimaData.Load();
        StoreConfig.Load();
        ChannelConfig.Load();
        DailyBounsData.Load();
        RecommendSpiritData.Load();

    }
    public void InitData()
    {
        //需要匹配DeviceID
        LoadData();
        C_MonoSingleton<GameHelper>.GetInstance().SendSDKData();
    }

    public void LoadBabyNameAudioClip()
    {
        BabyName.c_BabyNameAudioClip = null;

        if (string.IsNullOrEmpty(PlayerData.BabyNameMP3))
            return;

        string filePath = C_LocalPath.DataPath + PlayerData.BabyNameMP3;
        if (!File.Exists(filePath))
        {
            //后续修改为，判断是否需要下载
            C_UnityWebRequestDownloader.SyncDownloadFile(HttpRequestConfig.BabyNameMP3Url + PlayerData.BabyNameMP3, C_LocalPath.DataPath);
        }
        else
        {
            C_MonoSingleton<C_GameFramework>.GetInstance().StartCoroutine(LoadLocalMP3(filePath));
        }

//#if UNITY_EDITOR
        if (BabyName.c_BabyNameAudioClip == null)
            BabyName.c_BabyNameAudioClip = C_Singleton<GameResMgr>.GetInstance().LoadResource_Audio_XBL("baby_name.mp3");
//#endif
    }

    private IEnumerator LoadLocalMP3(string filePath)
    {
        filePath = "file://" + filePath;
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG))
        {
            yield return uwr.SendWebRequest();

            if (!uwr.isNetworkError)
                BabyName.c_BabyNameAudioClip = DownloadHandlerAudioClip.GetContent(uwr);
        }
    }

    public void RefreshPlayerData()
    {
      //  Synchrodata();
        //WizardData.FetchUelfinData();
        //AppInfoData.FetchAllStateData();
        //AnimaData.FetchData();
        //DailyBounsData.FetchUrewardData();
        C_MonoSingleton<GameHelper>.GetInstance().SendSDKData();

        LoadBabyNameAudioClip();
    }

    public void RequestPlayerStar()
    {
        string url = c_PinYinHost + HttpRequestConfig.GetUserStar;
        C_DebugHelper.Log("url = " + url);

        WWWForm form = new WWWForm();
        form.AddField("uid", PlayerData.UID);

        C_DebugHelper.Log(Encoding.UTF8.GetString(form.data));

        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(url, form.data, (string result) =>
        {
            C_DebugHelper.Log("RequestPlayerStar result = " + result);

            try
            {
                if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
                {
                    JsonData severinfo = C_Json.GetJsonKeyJsonData(result, "severinfo");
                    if (severinfo != null)
                    {
                        int star = C_Json.GetJsonKeyInt(severinfo, "total_stars");
                        if (PlayerData.StarCount != star)
                        {
                            PlayerData.StarCount = star;
                            PlayerData.Save();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("RequestPlayerStar : " + e);
            }
        });
    }

    public void ReportedBabyInfo()
    {
        string url = c_Host + HttpRequestConfig.SetBabyInfo;
        C_DebugHelper.Log("url = " + url);

        WWWForm form = new WWWForm();
        form.AddField("babygender", PlayerData.BabyGender);
        form.AddField("birthday", C_DateTime.ConvertDateTimeToInt32(PlayerData.BabyBirthday));
        form.AddField("babyname", PlayerData.BabyName);
        form.AddField("namemp3", PlayerData.BabyNameMP3);
        //form.AddField("namepinyin", "");
       // form.AddField("uid", PlayerData.UID);
        form.AddField("app", APP_CONST.PinYin);
        form.AddField("device", c_DeviceType);
        form.AddField("deviceid", c_DeviceUID);
        form.AddField("udid", c_DeviceUID);
        form.AddField("token", PlayerData.Token);
        form.AddField("ver", GameConfig.AppVersion);
        C_DebugHelper.Log(Encoding.UTF8.GetString(form.data));

        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(url, form.data, (string result) =>
        {
            C_DebugHelper.Log("ReportedBabyInfo result = " + result);
        });
    }

    public void ReportedHeadImage(string filePath)
    {
        C_DebugHelper.Log("ReportedHeadImage filePath = " + filePath);



#if UNITY_EDITOR
        string url = c_Host + HttpRequestConfig.SetHeadImg;
        C_DebugHelper.Log("url = " + url);
        string imgstr = img_Test;// System.Convert.ToBase64String(bytes);
#else
         if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            return;

        string url = c_Host + HttpRequestConfig.SetHeadImg;
        C_DebugHelper.Log("url = " + url);
        byte[] bytes = File.ReadAllBytes(filePath);
        string imgstr = System.Convert.ToBase64String(bytes);
#endif
        WWWForm form = new WWWForm();
        form.AddField("imgstr", imgstr);
        form.AddField("subfix", "jpg");
     //   form.AddField("uid", PlayerData.UID);
        form.AddField("app", APP_CONST.PinYin);
        form.AddField("device", c_DeviceType);
        form.AddField("deviceid", c_DeviceUID);
        form.AddField("udid", c_DeviceUID);
        form.AddField("token", PlayerData.Token);
        form.AddField("ver", GameConfig.AppVersion);
        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(url, form.data, (string result) =>
        {
            C_DebugHelper.Log("ReportedHeadImage result = " + result);

            try
            {
                if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
                {
                    JsonData uinfoJD = C_Json.GetJsonKeyJsonData(result, "uinfo");
                    if (uinfoJD != null)
                    {
                        string strHeadimg = C_Json.GetJsonKeyString(uinfoJD, "headimg");
                        if (PlayerData.HeadImg != strHeadimg)
                        {
                            PlayerData.HeadImg = strHeadimg;
                            PlayerData.Save();

                            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "PlayerDataChange");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("ReportedHeadImg : " + e);
            }
        });
    }

    public void ParseHelperClock(string time)
    {
        PlayerData.BabyBirthday = time;
        PlayerData.Save();

        ReportedBabyInfo();

        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "PlayerDataChange");
    }

    //0：取消，-1：失败，1：成功
    public void ParseHelperOpenShop(string result)
    {
        if (result == "1")
        {
            PlayerData.IsVIP = 1;
            PlayerData.Save();

           // C_EventHandler.SendEvent(C_EnumEventChannel.Global, "StageChange");

          //  C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_GetVIP");

          //  C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "pay_chenggong");
        }
    }


#region 关卡数据

    public void RequestStageData()
    {
        string url = c_PinYinHost + HttpRequestConfig.GetStageData;
        C_DebugHelper.Log("url = " + url);

        WWWForm form = new WWWForm();
        form.AddField("uid", PlayerData.UID);

        C_DebugHelper.Log(Encoding.UTF8.GetString(form.data));

        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(url, form.data, (string result) =>
        {
            C_DebugHelper.Log("StageMgr RequestStageData result = " + result);

            try
            {
                if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
                {
                    JsonData bagInfo = C_Json.GetJsonKeyJsonData(result, "severinfo", "UBagInfo");
                    if (bagInfo != null)
                        StageData.Save(bagInfo.ToJson());
                }
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("RequestStageInfo : " + e);
            }


            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "StageChange");
        });
    }

#endregion






#region 下载商城图片

    public void DownloadStoreSprite()
    {
        C_MonoSingleton<C_GameFramework>.GetInstance().StartCoroutine(ExecuteDownloadStoreSprite());
    }

    private IEnumerator ExecuteDownloadStoreSprite()
    {
        StoreConfigItem item = StoreConfig.GetStoreConfig(1);
        if (item != null)
        {
            if (!string.IsNullOrEmpty(item.Img))
            {
                string filePath = C_LocalPath.DataPath + C_String.GetFileName(item.Img);
                if (!File.Exists(filePath))
                    C_UnityWebRequestDownloader.SyncDownloadFile(item.Img, C_LocalPath.DataPath);

                if (File.Exists(filePath))
                {
                    byte[] bytes = File.ReadAllBytes(filePath);

                    Texture2D texture = new Texture2D(1820, 1024);
                    texture.LoadImage(bytes);

                    c_StoreSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }
        }

        yield return null;
    }

#endregion
}
