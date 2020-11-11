using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainCity : C_BaseUI
{
    [SerializeField]
    private RectTransform m_Panel = null;
    [SerializeField]
    private RectTransform m_Content = null;
    [SerializeField]
    private RectTransform m_StagePosList = null;

    private C_Event m_StageChangeEvent = new C_Event();

    private List<GameObject> m_UI_StageList = new List<GameObject>();

    private bool m_bStageDirty = true;

    private static bool s_bFirstDataStatistics = false;

    private GameObject m_UIStage = null;

    private GameObject m_StageFinger = null;

    public static Action c_OpenedAction = null;

    protected override void onInit()
    {
        Delegates.c_UI_MainCity = this;

        //m_UIStage = C_Singleton<GameResMgr>.GetInstance().LoadResource_UI("UI_Stage");
        //m_UIStage.transform.SetParent(m_StagePosList);
        //m_UIStage.SetActive(false);
        
        if (!s_bFirstDataStatistics)
        {
            s_bFirstDataStatistics = true;

            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "launch_game", "main_city");

            //首充界面
            //if (PlayerData.IsVIP != 1)
            //{
            //    string firstRechargeType = ChannelConfig.GetValue("first_recharge_type");

            //    if (firstRechargeType == "1")
            //        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_FirstRecharge");

            //    string date = DateTime.Now.ToString("d");
            //    string firstMainCity = PlayerPrefs.GetString(PlayerPrefsData.FIRST_MAIN_CITY, "");
            //    if (firstMainCity != date)
            //    {
            //        PlayerPrefs.SetString(PlayerPrefsData.FIRST_MAIN_CITY, date);
            //        PlayerPrefs.Save();

            //        if (firstRechargeType == "2")
            //            C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_FirstRecharge");

            //        if (!string.IsNullOrEmpty(firstMainCity))
            //        {
            //            if (firstRechargeType == "3")
            //                C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_FirstRecharge");
            //        }
            //    }
            //}
        }

       // InitUI();
    }

    protected override void onAdaption()
    {
        // 适配高分辨率

    }

    protected override void onOpenUI(params object[] uiObjParams)
    {
      //  C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_MainCityUp");

        //启动引导
      //  LaunchGuide();
        
        if (!NetworkMgr.IsConnected)
            Tips.Create("LOACAL_NO_NETWORK");

        if (c_OpenedAction != null)
        {
            c_OpenedAction();

            c_OpenedAction = null;
        }

    }

    protected override void onCloseUI()
    {
        m_StageChangeEvent.UnregisterEvent();

    }

    protected override void onUpdate()
    {
        
    }

    protected override void onDestroy()
    {
        Delegates.c_UI_MainCity = null;
    }
    private void LaunchGuide()
    {
        C_MonoSingleton<GuideMgr>.GetInstance().c_CurGuide = "";

        if (C_MonoSingleton<GuideMgr>.GetInstance().IsFirstLogin())
            C_MonoSingleton<GuideMgr>.GetInstance().TriggerGuide("FirstLogin");
        //else if (C_MonoSingleton<GuideMgr>.GetInstance().IsNoPlayStage())
        //    C_MonoSingleton<GuideMgr>.GetInstance().TriggerGuide("FirstStage");
    }
 
}
