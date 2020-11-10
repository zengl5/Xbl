using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : C_MonoSingleton<GameLogic>
{
    public enum EnumTimeState
    {
        None,
        Play,
        Rest,
        Sleep
    }

    private EnumTimeState m_TimeState = EnumTimeState.None;
    public EnumTimeState TimeState
    {
        get { return m_TimeState; }
        set
        {
            if (C_Singleton<C_GameStateCtrl>.GetInstance().CurrentStateName == "MainCityState")
            {
                if (m_TimeState != value)
                {
                    if (m_TimeState == EnumTimeState.Rest)
                    {
                        C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI("UI_Rest");
                    }
                    else if (m_TimeState == EnumTimeState.Sleep)
                    {
                        C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI("UI_Sleep");
                    }

                    m_TimeState = value;

                    switch (m_TimeState)
                    {
                        case EnumTimeState.Play:
                            m_nPlayTime = 0;

                            break;
                        case EnumTimeState.Rest:
                            //C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_Rest");
                            PlayerPrefs.SetString(PlayerPrefsData.GAME_LOGIC_REST_TIME, DateTime.Now.AddSeconds(PlayerData.RestTime).ToString());
                            PlayerPrefs.Save();

                            break;
                        case EnumTimeState.Sleep:
                            //C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_Sleep");
                            break;
                    }
                }
            }
        }
    }

    private int m_nTimerInterval = 5;

    private int m_nGetUIDTimer = 0;

    //private int m_nPlayerStatusTimer = 0;
    private int m_nPlayTime = 0;

    public int NeedRestTime
    {
        get
        {
            restDate = Convert.ToDateTime(PlayerPrefs.GetString(PlayerPrefsData.GAME_LOGIC_REST_TIME, "2018-6-1"));
            if (DateTime.Compare(DateTime.Now, restDate) < 0)
                return (int)(restDate - DateTime.Now).TotalSeconds;

            return 0;
        }
    }

    private int m_nEscapeTouch = 0;
    private float m_fEscapeTime = 0.0f;
    void Update()
    {
        if (m_fEscapeTime > 0)
        {
            m_fEscapeTime -= Time.deltaTime;

            if (m_fEscapeTime < 0)
                m_nEscapeTouch = 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_nEscapeTouch++;

            if (m_nEscapeTouch == 1)
                m_fEscapeTime = 1.0f;
            else if (m_nEscapeTouch > 1)
                Application.Quit();
        }
    }

    public void StartMainCity()
    {
        //if (string.IsNullOrEmpty(PlayerData.UID) || m_nGetUIDTimer == 0)
        //{
        //    m_nGetUIDTimer = C_Singleton<C_TimerMgr>.GetInstance().AddTimer(m_nTimerInterval, -1, () =>
        //    {
        //        CheckUID();
        //    });
        //}
    }

    public void LeaveMainCity()
    {
    }

    private void CheckUID()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            return;

        if (string.IsNullOrEmpty(PlayerData.UID))
        {
            GameLoginMgr.RequestUDIDLogin();
        }
        else
        {
            if (m_nGetUIDTimer != 0)
            {
                C_Singleton<C_TimerMgr>.GetInstance().RemoveTimer(m_nGetUIDTimer);
                m_nGetUIDTimer = 0;
            }
        }
    }

    private DateTime wakeUpDate = DateTime.ParseExact("00:00", "HH:mm", null);
    private DateTime sleepDate = DateTime.ParseExact("00:00", "HH:mm", null);

    private DateTime restDate = Convert.ToDateTime("2018-6-1");
    private void CheckTime()
    {
        if (C_Singleton<C_GameStateCtrl>.GetInstance().CurrentStateName == "MainCityState")
        {
            //其实可以判断时间改变了，再重新获取，不需要每m_nTimerInterval秒重新赋值，但我懒，这种性能提升对总体的性能影响不大
            wakeUpDate = DateTime.ParseExact(PlayerData.WakeUpTime, "HH:mm", null);
            sleepDate = DateTime.ParseExact(PlayerData.SleepTime, "HH:mm", null);

            //--------------------判断睡觉切换--------------------
            if (DateTime.Compare(DateTime.Now, wakeUpDate) < 0 || DateTime.Compare(DateTime.Now, sleepDate) > 0)
            {
                TimeState = EnumTimeState.Sleep;
                return;
            }
        }

        if (PlayerPrefs.HasKey(PlayerPrefsData.GAME_LOGIC_REST_TIME))
        {
            restDate = Convert.ToDateTime(PlayerPrefs.GetString(PlayerPrefsData.GAME_LOGIC_REST_TIME));
            if (DateTime.Compare(DateTime.Now, restDate) < 0)
                TimeState = EnumTimeState.Rest;
            else
                TimeState = EnumTimeState.Play;
        }
        else
        {
            TimeState = EnumTimeState.Play;
        }

        if (m_TimeState == EnumTimeState.Play && C_Singleton<C_GameStateCtrl>.GetInstance().CurrentStateName != "LoginState")
        {
            m_nPlayTime += m_nTimerInterval;

            if (PlayerData.RestSpan > 0 && m_nPlayTime >= PlayerData.RestSpan)
                TimeState = EnumTimeState.Rest;
        }
    }
}