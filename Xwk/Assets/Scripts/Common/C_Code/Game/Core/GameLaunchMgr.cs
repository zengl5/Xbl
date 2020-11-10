using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using YB.XWK.MainScene;

public class GameLaunchMgr : C_MonoSingleton<GameLaunchMgr>
{
    public enum EnumLaunchStep
    {
        None,
        RequestStageConfig,
        RequestStoreConfig,
        RequestChannelConfig,
        CheckAppVersion,
        RequestTokenVerify,
        Login,
        RefreshData,
        WaitRefreshData, 
        MainCity,
        End
    }

    private EnumLaunchStep m_CurStep = EnumLaunchStep.None;

    public static bool c_FinshCurStep = false;

    private bool m_IsFirstLogin = true;

    private C_Event m_LoginCompleteEvent = new C_Event();

    void Start()
    {
        //if (Application.internetReachability == NetworkReachability.NotReachable)
        //{
        //    GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, "not_connect_network");
        //}
        //else
        //{
        //    GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, "connect_network");
        //}

        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_start_app_time, LocalData.m_start_app_time);
        C_UIMgr.Instance.OpenUI("UI_Effects");

        Loading.Create("正在请求服务器数据......");

        m_CurStep = EnumLaunchStep.RequestStoreConfig;

        m_LoginCompleteEvent.RegisterEvent(C_EnumEventChannel.Global, "LoginComplete", (object[] result) =>
        {
            int login = 0;
            if (result.Length > 0)
                login = (int)result[0];

            if (m_IsFirstLogin || login == 1)
            {
                if (m_IsFirstLogin)
                {
                    AudioManager.Instance.PlayerSound("public/sound/common_166.ogg", false, () =>
                    {
                        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_login_app_time, LocalData.m_login_app_time);
                        m_CurStep = EnumLaunchStep.Login;
                        c_FinshCurStep = true;
                    });
                }
                else
                {
                    m_CurStep = EnumLaunchStep.Login;
                    c_FinshCurStep = true;
                }

                m_IsFirstLogin = false;
            }
        });

    }

    void Update()
    {
        if (c_FinshCurStep)
        {
            m_CurStep++;
            c_FinshCurStep = false;
            
            NextStep();
        }
        if (m_CurStep == EnumLaunchStep.WaitRefreshData && Loading.c_Rate >= 1)
        {
            m_CurStep++;

            Loading.c_Rate = 0;
            Loading.c_Description = "进入主界面......";
            C_Singleton<GameActionMgr>.GetInstance().GotoMainCity();
        }
    }

    protected override void OnDestroy()
    {
        m_LoginCompleteEvent.UnregisterEvent();
    }

    private void NextStep()
    {

        switch (m_CurStep)
        {
            case EnumLaunchStep.RequestChannelConfig:

                Loading.c_Rate = 1;
                Loading.c_Description = "正在获取渠道配置......";
                GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, "request_channel_config");

                RequestChannelConfig();

                break;

            case EnumLaunchStep.CheckAppVersion:

                Loading.c_Rate = 1;
                Loading.c_Description = "正在检查App版本......";
                GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, "check_app_version");

                //string serverAppVersion = ChannelConfig.GetValue("app_version");
                //if (!string.IsNullOrEmpty(serverAppVersion) && serverAppVersion.CompareTo(GameConfig.AppVersion) > 0)
                //    C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_CheckVersion");
                //else
                    c_FinshCurStep = true;

                break;
            case EnumLaunchStep.RequestTokenVerify:
                {
                    Loading.c_Rate = 0;
                    GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, "reuquesttokenverify");


                    Loading.c_Description = "正在验证登录......";
                    GameLoginMgr.ReuquestTokenVerify((reslut)=> {
                        Loading.c_Rate = 1;
                        c_FinshCurStep = true;
                    });
                }
                break;
            case EnumLaunchStep.Login:
                GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, "synchrodata");

                Loading.c_Rate = 0;
                Loading.c_Description = "正在同步数据......";
                if (!string.IsNullOrEmpty(PlayerData.Token))
                {
                    GameDataMgr.Instance.Synchrodata();
                }
                      
                //正式环境 和 测试环境  进行账号检测，如果当前存储账号不符合规定，清空当前存储
                if (!string.IsNullOrEmpty(PlayerData.UID))
                {
                    if ((GameDataMgr.c_Debug == 1 && int.Parse(PlayerData.UID) < 10000) || (GameDataMgr.c_Debug == 0 && int.Parse(PlayerData.UID) >= 10000))
                        PlayerData.UID = "";
                }

                Loading.c_Description = "正在登陆......";
                if (!string.IsNullOrEmpty(PlayerData.Token))
                    GameLoginMgr.RequestTokenLogin();
                else
                    GameLoginMgr.RequestUDIDLogin();

                break;

            case EnumLaunchStep.RefreshData:
                {
                    float progress = 1 / 5f;
                    Loading.c_Description = "同步数据......";
                    c_FinshCurStep = true;
                    Loading.c_Rate = 0;
                    WizardData.FetchUelfinData(()=> {
                        Loading.c_Rate += progress;
                    });
                    AppInfoData.FetchAllStateData(() => {
                        Loading.c_Rate += progress;
                    });
                    AnimaData.FetchData(() => {
                        Loading.c_Rate += progress;
                    });
                    DailyBounsData.FetchUrewardData(() => {
                        Loading.c_Rate += progress;
                    });
                    RecommendSpiritData.FetchUrewardData(() => {
                        Loading.c_Rate += progress;
                    });
                }
                break;
            case EnumLaunchStep.WaitRefreshData:
                {
                   
                }
                break;
            default:

                break;
        }
    }

    

    private void RequestChannelConfig()
    {
        string url = GameDataMgr.c_PinYinHost + HttpRequestConfig.GetChannelConfig;
        C_DebugHelper.Log("url = " + url);

        WWWForm form = new WWWForm();
        form.AddField("channel", GameDataMgr.c_Channel);

        C_DebugHelper.Log(Encoding.UTF8.GetString(form.data));

        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(url, form.data, (string result) =>
        {
            C_DebugHelper.Log("RequestChannelConfig result = " + result);

            try
            {
                if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
                {
                    JsonData configJD = C_Json.GetJsonKeyJsonData(result, "config");
                    if (configJD != null)
                        ChannelConfig.Save(configJD.ToJson());
                }
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("RequestChannelConfig : " + e);
            }
            
            Loading.c_Rate = 1;
            c_FinshCurStep = true;
        });
    }

    private void RequestStoreConfig()
    {
        string url = GameDataMgr.c_PinYinHost + HttpRequestConfig.GetStoreConfig;
        C_DebugHelper.Log("url = " + url);

        WWWForm form = new WWWForm();
        form.AddField("channel", GameDataMgr.c_Channel);
        form.AddField("version", GameConfig.AppVersion);

        C_DebugHelper.Log(Encoding.UTF8.GetString(form.data));

        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(url, form.data, (string result) =>
        {
            C_DebugHelper.Log("RequestStoreConfig result = " + result);

            try
            {
                if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
                {
                    JsonData configJD = C_Json.GetJsonKeyJsonData(result, "severinfo", "config");
                    if (configJD != null)
                        StoreConfig.Save(configJD.ToJson());
                }
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("RequestStoreConfig : " + e);
            }
        });

        Loading.c_Rate = 1;
        c_FinshCurStep = true;
    }
}
