using Assets.Scripts.C_Framework;
using System;
using UnityEngine;

#if !UNITY_EDITOR_WIN
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#endif

public enum EnumDataStatistics
{
    Chick,
    TimeStart,
    TimeEnd
}

public class GameHelper : C_MonoSingleton<GameHelper>
{
    private const string BuglyAppIDForiOS = "af2ad4f799";
    private const string BuglyAppIDForAndroid = "3affa88ea0";

#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaObject m_AndroidJavaObject;

#elif UNITY_IOS
    [DllImport("__Internal")]
    private static extern void sendAppInfo();
    [DllImport("__Internal")]
    private static extern void sendUID(string uid);
    [DllImport("__Internal")]
    private static extern void sendGender(string gender);
    [DllImport("__Internal")]
    private static extern void sendNick(string weChatUnionID);
    [DllImport("__Internal")]
    private static extern void sendBabyName(string babyName);

    [DllImport("__Internal")]
    private static extern void sendOpenPrivacyAgreement();

    [DllImport("__Internal")]
    private static extern void sendPhotograph();
    [DllImport("__Internal")]
    private static extern void sendUsePhotoAlbum();

    [DllImport("__Internal")]
    private static extern void sendOpenClock(string birthday);

    [DllImport("__Internal")]
    private static extern void sendWeChatLogin();
    [DllImport("__Internal")]
    private static extern void sendWeChatQRCodeLogin();
    [DllImport("__Internal")]
    private static extern void sendWeChatBinding();
    [DllImport("__Internal")]
    private static extern void sendWeChatQRCodeBinding();

    [DllImport("__Internal")]
    private static extern void sendOpenShop();

    [DllImport("__Internal")]
    private static extern void sendOpenRecordPermission();
    [DllImport("__Internal")]
    private static extern void sendStartRecord();
    [DllImport("__Internal")]
    private static extern void sendStartRecord(String inputWords,int langType,int speechType );
    [DllImport("__Internal")]
    private static extern void sendRecordStatus();
    [DllImport("__Internal")]
    private static extern void sendEndRecord();
    [DllImport("__Internal")]
    private static extern void sendGetRecord();

    [DllImport("__Internal")]
    private static extern void sendGameStart();
    [DllImport("__Internal")]
    private static extern void sendGameOver();
    [DllImport("__Internal")]
    private static extern void sendClickEvent(string eventname, string stage);
    [DllImport("__Internal")]
    private static extern void sendTimeStartEvent(string  eventname, string stage);
    [DllImport("__Internal")]
    private static extern void sendTimeEndEvent(string eventname, string stage, int status, int ts, int te);
    [DllImport("__Internal")]
    private static extern void sendOpenXBLAppRecommendView();
    [DllImport("__Internal")]
    private static extern void setMuteModePlay();
    [DllImport("__Internal")]
    private static extern void sendOpen5StarRecommend();
    [DllImport("__Internal")]
    private static extern void openvPrivacyPolicy();
    //[DllImport("__Internal")]
    //private static extern void closePrivacyPolicy(string result);
    [DllImport("__Internal")]
    private static extern void openUserServiceProtocol();
    [DllImport("__Internal")]
    private static extern void openUserPrivacyProtectionPolic();
    [DllImport("__Internal")]
    private static extern void openMemberServiceAgreement();
    [DllImport("__Internal")]
    private static extern void openUserFeedback(string uid,string nickname,string headurl);
    [DllImport("__Internal")]
    private static extern void addToDCIM(string base64Code);
    [DllImport("__Internal")]
    private static extern int isWeixinAvailable();
    [DllImport("__Internal")]
    private static extern void shareImgBase64Wechat(string base64Code,int shareType);
    
#endif

    protected override void Init()
    {
#if !UNITY_SDK
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        m_AndroidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
#endif
#endif
        SendAppInfo();

        InitBuglySDK();
        //        string _OutLogFilePath;
        //#if UNITY_EDITOR
        //         _OutLogFilePath = Application.dataPath;

        //#elif UNITY_ANDROID
        //            _OutLogFilePath = Application.persistentDataPath  ;
        //#elif UNITY_IOS
        //            _OutLogFilePath = Application.persistentDataPath ;
        //#endif
    }

    private void OnApplicationQuit()
    {
        SendGameOver();
    }

    private void InitBuglySDK()
    {
#if UNITY_IOS && !UNITY_EDITOR
        BuglyAgent.InitWithAppId(BuglyAppIDForiOS);
#elif UNITY_ANDROID && !UNITY_EDITOR
        BuglyAgent.InitWithAppId(BuglyAppIDForAndroid);
#endif

        // TODO Required. If you do not need call 'InitWithAppId(string)' to initialize the sdk(may be you has initialized the sdk it associated Android or iOS project),
        // please call this method to enable c# exception handler only.
        BuglyAgent.EnableExceptionHandler();

        //   BuglyAgent.PrintLog(LogSeverity.LogInfo, "Init the bugly sdk");
    }

    #region 基础信息交互

    private void SendAppInfo()
    {
        C_DebugHelper.Log("SendAppInfo Start!");
#if UNITY_SDK
        ResponseAppInfo("{ \"deviceId\": \"\", \"udid\": \"\", \"debug\": 1, \"wechat\": 1, \"channel\": \"xwk_360\" }");
        return;
#endif
#if UNITY_IOS && !UNITY_EDITOR
        sendAppInfo();
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendAppInfo");
#else
        // ResponseAppInfo("{ \"deviceId\": \"\", \"udid\": \"\", \"debug\": 1, \"wechat\": 1, \"channel\": \"xblpy_test\" }");
        ResponseAppInfo("{ \"deviceId\": \"\", \"udid\": \"\", \"debug\": 0, \"wechat\": 1, \"channel\": \"xblpy_youxuepai\" }");

#endif
    }

    public void ResponseAppInfo(string result)
    {
        C_DebugHelper.Log("ResponseAppInfo result = " + result);

        GameDataMgr.c_UDID = C_Json.GetJsonKeyString(result, "udid");
        if (string.IsNullOrEmpty(GameDataMgr.c_UDID))
            GameDataMgr.c_UDID = SystemInfo.deviceUniqueIdentifier;

        GameDataMgr.c_DeviceUID = GameDataMgr.c_UDID;

        GameDataMgr.c_Channel = C_Json.GetJsonKeyString(result, "channel");
        // GameDataMgr.c_ProductID = C_Json.GetJsonKeyString(result, "productid");

        GameDataMgr.c_WeChatEnvironment = C_Json.GetJsonKeyInt(result, "wechat");

        int freeSpace = C_Json.GetJsonKeyInt(result, "freeSpace");
        if (freeSpace > 0)
            GameDataMgr.c_FreeSpace = freeSpace;

        GameDataMgr.c_Debug = C_Json.GetJsonKeyInt(result, "debug");
#if UNITY_EDITOR
        GameDataMgr.c_Debug = 0;
#endif
        if (GameDataMgr.c_Debug == 1)
        {
            GameDataMgr.c_HotUpdate = HttpRequestConfig.FormalHotUpdate;
            GameDataMgr.c_Host = HttpRequestConfig.FormalHost;
            GameDataMgr.c_DataHost = HttpRequestConfig.FormalDataHost;
            GameDataMgr.c_PinYinHost = HttpRequestConfig.FormalPinYinHost;
            GameDataMgr.c_PayHost = HttpRequestConfig.FormalPayHost;
            GameDataMgr.c_CommonHost = HttpRequestConfig.FormalCommonHost;
        }
        else
        {
            GameDataMgr.c_HotUpdate = HttpRequestConfig.TestHotUpdate;
            GameDataMgr.c_Host = HttpRequestConfig.TestHost;
            GameDataMgr.c_DataHost = HttpRequestConfig.TestDataHost;
            GameDataMgr.c_PinYinHost = HttpRequestConfig.TestPinYinHost;
            GameDataMgr.c_PayHost = HttpRequestConfig.TestPayHost;
            GameDataMgr.c_CommonHost = HttpRequestConfig.TestCommonHost;
        }

        C_Singleton<GameDataMgr>.GetInstance().InitData();

        GameLaunchMgr.c_FinshCurStep = true;
    }

    public void SendSDKData()
    {
        if (string.IsNullOrEmpty(PlayerData.UID))
            return;

        C_DebugHelper.Log("SendSDKData SendUID UID = " + PlayerData.UID
            + ", SendGender Gender = " + PlayerData.BabyGender
            + ", SendNick WeChatUnionID = " + PlayerData.WeChatUnionID
            + ", SendBabyName BabyName = " + PlayerData.BabyName);
#if UNITY_SDK
        return;
#endif

#if UNITY_IOS && !UNITY_EDITOR
		sendUID(PlayerData.UID);
        sendGender(PlayerData.BabyGender);
        sendNick(PlayerData.WeChatUnionID);
        sendBabyName(PlayerData.BabyName);
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendUID", PlayerData.UID);
        m_AndroidJavaObject.Call("sendGender", PlayerData.BabyGender);
        m_AndroidJavaObject.Call("sendNick", PlayerData.WeChatUnionID);
        m_AndroidJavaObject.Call("sendBabyName", PlayerData.BabyName);
#endif

        SendGameStart();
    }

    private string m_strGameStartUID = "";

    public void SendGameStart()
    {
#if UNITY_SDK
        return;
#endif
        if (string.IsNullOrEmpty(PlayerData.UID) || m_strGameStartUID == PlayerData.UID)
            return;

        if (!string.IsNullOrEmpty(m_strGameStartUID))
            SendGameOver();

        C_DebugHelper.Log("SendGameStart Start!");

        m_strGameStartUID = PlayerData.UID;

#if UNITY_IOS && !UNITY_EDITOR
        sendGameStart();
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendGameStart");
#endif
    }

    public void SendGameOver()
    {
#if UNITY_SDK
        return;
#endif
        if (string.IsNullOrEmpty(PlayerData.UID))
            return;

        C_DebugHelper.Log("SendGameOver Start!");

#if UNITY_IOS && !UNITY_EDITOR
        sendGameOver();
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendGameOver");
#endif
    }

    #endregion 基础信息交互

    #region 打开隐私协议

    public void SendOpenPrivacyAgreement()
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("SendOpenPrivacyAgreement Start!");

#if UNITY_IOS && !UNITY_EDITOR
		sendOpenPrivacyAgreement();
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendOpenPrivacyAgreement");
#endif
    }

    #endregion 打开隐私协议

    #region 拍照

    public void SendPhotograph()
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("SendPhotograph Start!");

#if UNITY_IOS && !UNITY_EDITOR
		sendPhotograph();
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendPhotograph");
#elif UNITY_EDITOR
        C_Singleton<GameDataMgr>.GetInstance().ReportedHeadImage("123");
#endif
    }

    public void ResponsePhotograph(string result)
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("ResponsePhotograph result = " + result);

        C_Singleton<GameDataMgr>.GetInstance().ReportedHeadImage(result);
    }

    public void SendUsePhotoAlbum()
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("SendUsePhotoAlbum Start!");

#if UNITY_IOS && !UNITY_EDITOR
		sendUsePhotoAlbum();
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendUsePhotoAlbum");
#endif
    }

    public void ResponseUsePhotoAlbum(string result)
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("ResponseUsePhotoAlbum result = " + result);

        C_Singleton<GameDataMgr>.GetInstance().ReportedHeadImage(result);
    }

    #endregion 拍照

    #region 打开时钟

    public void SendOpenClock()
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("SendOpenClock Start! " + PlayerData.BabyBirthday);

#if UNITY_IOS && !UNITY_EDITOR
		sendOpenClock(PlayerData.BabyBirthday);
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendOpenClock", PlayerData.BabyBirthday);
#endif
    }

    public void ResponseOpenClock(string result)
    {
        C_DebugHelper.Log("ResponseOpenClock result = " + result);

        C_Singleton<GameDataMgr>.GetInstance().ParseHelperClock(result);
    }

    #endregion 打开时钟

    #region 微信

    public void SendWeChatLogin()
    {
        C_DebugHelper.Log("SendWeChatLogin Start!");

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Tips.Create("LOACAL_NEED_NETWORK");
            return;
        }

#if UNITY_IOS && !UNITY_EDITOR
		sendWeChatLogin();
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendWeChatLogin");
#elif UNITY_EDITOR
        ResponseWeChatLogin("081saM3j1v9gJt0ePv6j1O814j1saM3d");
#endif
    }

    public void ResponseWeChatLogin(string result)
    {
        //测试记得删除
        C_DebugHelper.Log("ResponseWeChatLogin result = " + result);

        GameLoginMgr.RequestWeChatLogin(result);
    }

    public void SendWeChatQRCodeLogin()
    {
        C_DebugHelper.Log("SendWeChatQRCodeLogin Start!");

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Tips.Create("LOACAL_NEED_NETWORK");
            return;
        }

#if UNITY_IOS && !UNITY_EDITOR
        sendWeChatQRCodeLogin();
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendWechatQRCodeLogin");
#endif
    }

    public void ResponseWeChatQRCodeLogin(string result)
    {
        C_DebugHelper.Log("ResponseWeChatQRCodeLogin result = " + result);

        GameLoginMgr.RequestWeChaQRCodetLogin(result);
    }

    public void SendWeChatBinding()
    {
        C_DebugHelper.Log("SendWeChatBinding Start!");

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Tips.Create("LOACAL_NEED_NETWORK");
            return;
        }

#if UNITY_IOS && !UNITY_EDITOR
		sendWeChatBinding();
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendWeChatBinding");
#elif UNITY_EDITOR
        ResponseWeChatBinding("");
#endif
    }

    public void ResponseWeChatBinding(string result)
    {
        C_DebugHelper.Log("ResponseWechatBinding result = " + result);

        GameLoginMgr.RequestWeChatBinding(result);
    }

    public void SendWeChatQRCodeBinding()
    {
        C_DebugHelper.Log("SendWeChatQRCodeBinding Start!");

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Tips.Create("LOACAL_NEED_NETWORK");
            return;
        }

#if UNITY_IOS && !UNITY_EDITOR
		sendWeChatQRCodeBinding();
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendWeChatQRCodeBinding");
#endif
    }

    public void ResponseWeChatQRCodeBinding(string result)
    {
        C_DebugHelper.Log("ResponseWeChatQRCodeBinding result = " + result);

        GameLoginMgr.RequestWeChatQRCodeBinding(result);
    }

    #endregion 微信

    #region 打开商城

    public void SendOpenShop()
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("SendOpenShop Start!");

#if UNITY_IOS && !UNITY_EDITOR
		sendOpenShop();
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendOpenShop");
#endif
    }

    public void ResponseOpenShop(string result)
    {
        C_DebugHelper.Log("ResponseOpenShop result = " + result);

        C_Singleton<GameDataMgr>.GetInstance().ParseHelperOpenShop(result);
    }

    #endregion 打开商城

    #region 录音

    private bool m_RecordStatus = false;

    public void SendOpenRecordPermission()
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("SendOpenRecordPermission Start!");

#if UNITY_IOS && !UNITY_EDITOR
		sendOpenRecordPermission();
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendOpenRecordPermission");
#endif
    }

    public void SendStartRecord()
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("SendStartRecord Start!");

#if UNITY_IOS && !UNITY_EDITOR
		sendStartRecord();
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendStartRecord");
#endif
    }

    #region 腾讯云语音识别

    public void sendTecentStartRecord(String inputWords, int langType, int speechType)
    {
#if UNITY_SDK
        return;
#endif
        if (!m_RecordStatus) { return; }
#if UNITY_IOS && !UNITY_EDITOR
		sendStartRecord(inputWords,langType,speechType);
#elif UNITY_ANDROID && !UNITY_EDITOR
         m_AndroidJavaObject.Call("sendStartRecord", inputWords, langType,speechType);

#endif
    }

    #endregion 腾讯云语音识别

    public void SendRecordStatus()
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("SendRecordStatus Start!");

#if UNITY_IOS && !UNITY_EDITOR
		sendRecordStatus();
#elif UNITY_ANDROID && !UNITY_EDITOR
       // m_AndroidJavaObject.Call("sendRecordStatus");
        m_AndroidJavaObject.Call("sendOpenRecordPermission");
#elif UNITY_EDITOR
        ResponseRecordStatus("1");
#endif
    }

    /// <summary>
    /// 返回是否有权限
    /// </summary>
    /// <param name="result"></param>
    public void ResponseRecordStatus(string result)
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("ResponseRecordStatus result = " + result);

        if (result == "1")
            m_RecordStatus = true;
        else
            m_RecordStatus = false;

        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "ResponseRecordStatus", result);
    }

    public void SendEndRecord()
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("SendEndRecord Start!");

        if (!m_RecordStatus)
            return;

#if UNITY_IOS && !UNITY_EDITOR
		sendEndRecord();
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendEndRecord");
#elif UNITY_EDITOR
        ResponseGetRecord("");
#endif
    }

    public void SendGetRecord()
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("SendGetRecord Start!");

        if (!m_RecordStatus)
        {
            ResponseGetRecord("");
            return;
        }

#if UNITY_IOS && !UNITY_EDITOR
		sendGetRecord();
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendGetRecord");
#endif
    }

    /// <summary>
    /// 返回识别的结果
    /// </summary>
    /// <param name="result"></param>
    public void ResponseGetRecord(string result)
    {
#if UNITY_EDITOR
        //result = "{\"inputWords\":\"大小\",\"resultCode\":1,\"pronAccuracy\":33.64585,\"pronFluency\":0.997088,\"pronCompletion\":1,\"sessionId\":\"5337DA73-B049-4D78-AD34-05FBED44F1DC\",\"audioUrl\":\"\",\"words\":[{\"word\":\"*\",\"endTime\":2660,\"beginTime\":2460,\"pronAccuracy\":-1,\"pronFluency\":0,\"matchTag\":1},{\"word\":\"*\",\"endTime\":2820,\"beginTime\":2660,\"pronAccuracy\":-1,\"pronFluency\":0,\"matchTag\":1},{\"word\":\"*\",\"endTime\":3100,\"beginTime\":2820,\"pronAccuracy\":-1,\"pronFluency\":0,\"matchTag\":1},{\"word\":\"*\",\"endTime\":3280,\"beginTime\":3100,\"pronAccuracy\":-1,\"pronFluency\":0,\"matchTag\":1},{\"word\":\"大\",\"endTime\":3420,\"beginTime\":3280,\"pronAccuracy\":83.63338,\"pronFluency\":0.994176,\"matchTag\":0},{\"word\":\"小\",\"endTime\":3620,\"beginTime\":3460,\"pronAccuracy\":8.65209,\"pronFluency\":1,\"matchTag\":0}]}";
        result = "{\"inputWords\":\"大小\",\"pronFluency\":0.9,\"sessionId\":\"40F01741-BFBB-4E89-BA24-1DB6389E1126\",\"resultCode\":1,\"pronAccuracy\":1.769435,\"words\":[{\"word\":\"*\",\"endTime\":1600,\"beginTime\":1500,\"pronAccuracy\":-1,\"pronFluency\":0,\"matchTag\":1},{\"word\":\"*\",\"endTime\":1740,\"beginTime\":1600,\"pronAccuracy\":-1,\"pronFluency\":0,\"matchTag\":1},{\"word\":\"大\",\"endTime\":1850,\"beginTime\":1740,\"pronAccuracy\":3.260992,\"pronFluency\":1,\"matchTag\":0},{\"word\":\"小\",\"endTime\":2920,\"beginTime\":2790,\"pronAccuracy\":1.023656,\"pronFluency\":1,\"matchTag\":0}],\"pronCompletion\":1,\"audioUrl\":\"\"}";
        // result = "{\"audioUrl\":\"https:/soe-1255701415.cos.ap-beijing.myqcloud.com//1256527981//default//20190530//audio//fbe69e22-3974-4229-a398-dc08928c8b16.mp3\",\"pronAccuracy\":-1,\"pronCompletion\":0.5,\"pronFluency\":1,\"sessionId\":\"e390bd8b-183a-4616-a4d7-389ff4fccd6c\",\"words\":[{\"beginTime\":2170,\"endTime\":2250,\"matchTag\":0,\"pronAccuracy\":-1,\"pronFluency\":-1,\"word\":\"大\"},{\"beginTime\":0,\"endTime\":0,\"matchTag\":2,\"pronAccuracy\":-1,\"pronFluency\":0,\"word\":\"小\"}],\"inputWords\":\"大小\",\"resultCode\":1}";
#endif
        C_DebugHelper.Log("ResponseGetRecord result = " + result);

        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "ResponseRecord", result);
    }

    #endregion 录音

    #region 数据统计

    //status   0 默认值，1 完成，2中断退出,3正常退出  eventName 和eventParam 不能为空
    public void SendDataStatistics(EnumDataStatistics enumDataStatistics, string eventName, string eventParam = "", int status = 0)
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("SendDataStatistics enumDataStatistics = " + enumDataStatistics + ", eventName = " + eventName + ", eventParam = " + eventParam);

        //if (Application.internetReachability == NetworkReachability.NotReachable)
        //    return;

        if (enumDataStatistics == EnumDataStatistics.Chick)
        {
            SendClickEvent(eventName, eventParam);
        }
        else if (enumDataStatistics == EnumDataStatistics.TimeStart)
        {
            SendTimeStartEvent(eventName, eventParam);
        }
        else if (enumDataStatistics == EnumDataStatistics.TimeEnd)
        {
            SendTimeEndEvent(eventName, eventParam, status);
        }
    }

    private void SendClickEvent(string eventName, string eventParam)
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("SendClickEvent eventName = " + eventName + ", eventParam = " + eventParam);

#if UNITY_IOS && !UNITY_EDITOR
        if (string.IsNullOrEmpty(eventParam))
            eventParam = eventName;

        sendClickEvent(eventName, eventParam);
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendClickEvent", eventName, eventParam);
#endif
    }

    private Int32 m_StartTime = 0;

    private void SendTimeStartEvent(string eventName, string eventParam)
    {
#if UNITY_SDK
        return;
#endif
        m_StartTime = C_DateTime.ConvertDateTimeToInt32(DateTime.Now);

        C_DebugHelper.Log("sendTimeStartEvent eventName = " + eventName + ", eventParam = " + eventParam + ", m_StartTime = " + m_StartTime);

#if UNITY_IOS && !UNITY_EDITOR
        if (string.IsNullOrEmpty(eventParam))
            eventParam = eventName;

        sendTimeStartEvent(eventName, eventParam);
#elif UNITY_ANDROID && !UNITY_EDITOR
         m_AndroidJavaObject.Call("sendTimeStartEvent", eventName, eventParam);
#endif
    }

    private void SendTimeEndEvent(string eventName, string eventParam, int status)
    {
#if UNITY_SDK
        return;
#endif
        Int32 endTime = C_DateTime.ConvertDateTimeToInt32(DateTime.Now);

        C_DebugHelper.Log("sendTimeStartEvent eventName = " + eventName
            + ", eventParam = " + eventParam
            + ", status = " + status.ToString()
            + ", m_StartTime = " + m_StartTime.ToString()
            + ", endTime = " + endTime.ToString());

#if UNITY_IOS && !UNITY_EDITOR
        if (string.IsNullOrEmpty(eventParam))
            eventParam = eventName;

        sendTimeEndEvent(eventName, eventParam, status, m_StartTime, endTime);
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendTimeEndEvent", eventName, eventParam, status, m_StartTime, endTime);
#endif
    }

    #endregion 数据统计

    #region 打开互推

    public void SendOpenXBLAppRecommendView()
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("SendOpenXBLAppRecommendView Start!");

#if UNITY_IOS && !UNITY_EDITOR
		sendOpenXBLAppRecommendView();
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendOpenXBLAppRecommendView");
#endif
    }

    #endregion 打开互推

    #region 打开静音

    public void SetMuteModePlay()
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("setMuteModePlay Start!");

#if UNITY_IOS && !UNITY_EDITOR
	setMuteModePlay();
#elif UNITY_ANDROID && !UNITY_EDITOR

#endif
    }

    #endregion 打开静音

    #region 打开5星好评

    public void SendOpen5StarRecommend()
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("SendOpen5StarRecommend Start!");

#if UNITY_IOS && !UNITY_EDITOR
        sendOpen5StarRecommend();
#endif
    }

    #endregion 打开5星好评

    #region 用户协议

    #region 主界面请求隐私协议界面

    public void OpenvPrivacyPolicy()
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("OpenvPrivacyPolicy Start!");

#if UNITY_IOS && !UNITY_EDITOR
		 openvPrivacyPolicy();
#elif UNITY_ANDROID && !UNITY_EDITOR
         m_AndroidJavaObject.Call("OpenvPrivacyPolicy");
#elif UNITY_EDITOR
        ClosePrivacyPolicy("1");
#endif
    }

    #endregion 主界面请求隐私协议界面

    #region 关闭隐私协议界面

    public void ClosePrivacyPolicy(string result)
    {
#if UNITY_SDK
        return;
#endif
        C_DebugHelper.Log("ClosePrivacyPolicy result = " + result);
        if (!string.IsNullOrEmpty(result) && result.Equals("1"))
        {
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 5, "1");
        }
    }

    #endregion 关闭隐私协议界面

    /// <summary>
    /// 用户服务协议
    /// </summary>
    public void OpenUserServiceProtocol()
    {
#if UNITY_IOS && !UNITY_EDITOR
		 openUserServiceProtocol();
#elif UNITY_ANDROID && !UNITY_EDITOR
         m_AndroidJavaObject.Call("OpenUserServiceProtocol");
#elif UNITY_EDITOR

#endif
    }

    /// <summary>
    /// 用户隐私保护政策
    /// </summary>
    public void OpenUserPrivacyProtectionPolic()
    {
#if UNITY_SDK
        return;
#endif
#if UNITY_IOS && !UNITY_EDITOR
		 openUserPrivacyProtectionPolic();
#elif UNITY_ANDROID && !UNITY_EDITOR
         m_AndroidJavaObject.Call("OpenUserPrivacyProtectionPolic");
#elif UNITY_EDITOR

#endif
    }

    /// <summary>
    /// 会员服务协议
    /// </summary>
    public void OpenMemberServiceAgreement()
    {
#if UNITY_SDK
        return;
#endif
#if UNITY_IOS && !UNITY_EDITOR
		 openMemberServiceAgreement();
#elif UNITY_ANDROID && !UNITY_EDITOR
         m_AndroidJavaObject.Call("OpenMemberServiceAgreement");
#elif UNITY_EDITOR

#endif
    }

    #endregion 用户协议

    #region 反馈信息

    //打开吐槽页面,腾讯要求:三个参数都为必填，如果少了其中任何一个，登录态的构建的都会失败。登录态构建失败不会影响跳转，但会直接以匿名状态登录。
    public void SDK_SendOpenComment(String uid, String nickname, String headurl)
    {
#if UNITY_SDK
        return;
#endif
#if UNITY_IOS && !UNITY_EDITOR
        openUserFeedback(uid, nickname, headurl);
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("sendOpenComment", uid, nickname, headurl);
#endif
    }

    #endregion 反馈信息

//    #region 获取存储权限

//    public int SendSDCard()
//    {
//        int authority = 0;
//#if UNITY_SDK
//        return;
//#endif

//#if UNITY_IOS && !UNITY_EDITOR
//		//sendPhotograph();
//#elif UNITY_ANDROID && !UNITY_EDITOR
//        authority = m_AndroidJavaObject.Call<int>("sendSDCard", 1);
//#elif UNITY_EDITOR

//#endif
//        C_DebugHelper.Log("sendSDCard " + authority);
//        return authority;
//    }

//    #endregion 获取存储权限

    #region 存储到相册

    public void AddToDCIM(ref string base64Code)
    {
        if (string.IsNullOrEmpty(base64Code))
        {
            return;
        }
#if UNITY_SDK
        return;
#endif

#if UNITY_IOS && !UNITY_EDITOR
        addToDCIM(base64Code);
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("addToDCIM", base64Code);
#elif UNITY_EDITOR
#endif
    }

    #endregion 存储到相册
    #region 判断是否有安装微信 
    public bool IsWxAvailable()
    {
#if UNITY_IOS && !UNITY_EDITOR
        if(isWeixinAvailable()==1){
            return true;
        }else{
            return false;
        }
#elif UNITY_ANDROID && !UNITY_EDITOR
        if(m_AndroidJavaObject.Call<int>("isWeixinAvailable")==1){
            return true;
        }else{
            return false;
        }
#elif UNITY_EDITOR
        return true;
#endif
    }

    #endregion

    #region  分享到微信和朋友圈
    public void ShareImgBase64Wechat(string base64Code,int shareType)
    {
        if (string.IsNullOrEmpty(base64Code))
        {
            return;
        }
#if UNITY_SDK
        return;
#endif

#if UNITY_IOS && !UNITY_EDITOR
        shareImageBase64Wechat(base64Code,shareType);
#elif UNITY_ANDROID && !UNITY_EDITOR
        m_AndroidJavaObject.Call("shareImageBase64Wechat", base64Code,shareType);
#elif UNITY_EDITOR
#endif
    }

    #endregion
}