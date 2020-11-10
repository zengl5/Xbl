using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CheckVersion : C_BaseUI
{
    protected override void onOpenUI(params object[] uiObjParams)
    {
        AudioMgr.PlaySoundEffect("common_583");
    }

    public void Confirm()
    {
        GameLaunchMgr.c_FinshCurStep = true;

        CloseUI();
    }
}
