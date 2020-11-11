using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Sleep : C_BaseUI
{
    protected override void onOpenUI(params object[] uiObjParams)
    {
        C_DebugHelper.Log("GameDataMgr.c_GameData.WakeUpTime = " + PlayerData.WakeUpTime + ", GameDataMgr.c_GameData.SleepTime = " + PlayerData.SleepTime);
    }
}
