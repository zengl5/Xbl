using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Story_TitleBar :  C_BaseUI{

    public Action PauseAction = null;

    protected override void onOpenUI(params object[] uiObjParams)
    {
        if (uiObjParams.Length >= 1)
        {
            PauseAction = (Action)uiObjParams[0];
        }
        UICanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    public void Pause()
    {
        if (PauseAction != null)
            PauseAction();
    }

    public void SerRenderMode(RenderMode renderMode)
    {
        UICanvas.renderMode = renderMode;
    }

    public void ClosePauseUI()
    {
        CloseUI();
    }
    protected override void onCloseUI()
    {

    }

}
