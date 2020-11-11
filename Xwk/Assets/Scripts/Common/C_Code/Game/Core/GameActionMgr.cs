using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionMgr : C_Singleton<GameActionMgr>
{
    public void GotoLoginState(Action callback = null)
    {
        C_Singleton<C_GameStateCtrl>.GetInstance().GotoState("MainCityState");

        UI_MainCity.c_OpenedAction = callback;
    }
    public void GotoMainCity(Action callback = null)
    {
        C_Singleton<C_GameStateCtrl>.GetInstance().GotoState("MainCityState");

        UI_MainCity.c_OpenedAction = callback;
    }
    public void GoToShowJKBState()
    {
        C_Singleton<C_GameStateCtrl>.GetInstance().GotoState("ShowJKBState");
    }

    public void GotoMainCity_FieldGuide()
    {
        C_Singleton<C_GameStateCtrl>.GetInstance().GotoState("MainCityState");

        UI_MainCity.c_OpenedAction = () =>
        {
            C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_FieldGuide");
        };
    }

    public void GotoMainCity_Exercise()
    {
        C_Singleton<C_GameStateCtrl>.GetInstance().GotoState("MainCityState");

        UI_MainCity.c_OpenedAction = () =>
        {
            C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_Exercise");

            if (PlayerData.IsVIP != 1)
                C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_Shop", 3);
        };
    }

    public void Show_UI_Star(int starCount, Vector3 starPos, Action callback)
    {
        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_Star");

        UI_Star ui_star = C_MonoSingleton<C_UIMgr>.GetInstance().GetUI<UI_Star>("UI_Star");
        if (ui_star != null)
            ui_star.InitStar(starCount, starPos, callback);
    }

    public void Show_UI_Upgrade(int beforeLevel, int beforeGrade, Action callback)
    {
        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_Upgrade");

        UI_Upgrade ui_upgrade = C_MonoSingleton<C_UIMgr>.GetInstance().GetUI<UI_Upgrade>("UI_Upgrade");
        if (ui_upgrade != null)
            ui_upgrade.InitUpgrade(beforeLevel, beforeGrade, callback);
    }

     
}
