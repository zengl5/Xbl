using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StoryPause : C_BaseUI
{
    public Action GoMainCityAction = null;
    public Action ContinueAction = null;

    protected override void onOpenUI(params object[] uiObjParams)
    {
        if (uiObjParams.Length >= 2)
        {
            GoMainCityAction = (Action)uiObjParams[0];
            ContinueAction = (Action)uiObjParams[1];
        }
        UICanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }
    
    public void Continue()
    {
        if (ContinueAction != null)
            ContinueAction();

        CloseUI();
    }

    public void GoMainCity()
    {
        if (GoMainCityAction != null)
            GoMainCityAction();

#if !C_Framework
        PauseUIMoudleMgr.Instance.mGameGoMainCityAction = null;
        PauseUIMoudleMgr.Instance.mGameContinueAction = null;
        PauseUIMoudleMgr.Instance.mGamePauseAction = null;
#endif

        //C_Singleton<StageMgr>.GetInstance().StoryEnd(3);
    }
}
