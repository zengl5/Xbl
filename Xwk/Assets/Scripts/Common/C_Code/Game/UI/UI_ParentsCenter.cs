using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ParentsCenter : C_BaseUI
{
    [SerializeField]
    private Toggle[] m_ToggleVector = null;

    //private string[] m_UINameVector = { "UI_Shop", "UI_LearningReport", "UI_AccountCenter", "UI_LearningSetting", "UI_AboutUs" };
    private string[] m_UINameVector = { "UI_AccountCenter", "UI_AboutUs" };

    protected override void onOpenUI(params object[] uiObjParams)
    {
        //C_EventHandler.SendEvent(C_EnumEventChannel.Global, "ActorExit");
      //  C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 3);


        C_MonoSingleton<PlayerController>.GetInstance().AutoPlayEnabled = false;

        //初始化
        m_ToggleVector[0].isOn = true;
        m_ToggleVector[1].isOn = false;

        ToggleChange(0);

        //C_MonoSingleton<PlayerController>.GetInstance().PlayAudio("common_366");
    }
    public void PressClose()
    {
       C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent",1);
         
        onCloseUI();
    }
    protected override void onCloseUI()
    {
      //  C_EventHandler.SendEvent(C_EnumEventChannel.Global, "ActorResume");

      //  C_MonoSingleton<PlayerController>.GetInstance().AutoPlayEnabled = true;

        m_CurToggleIndex = -1;

        for (int i = 0; i < m_UINameVector.Length; i++)
            C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI(m_UINameVector[i]);

       //AudioMgr.PlaySoundEffect("public_sd_042");
    }

    private int m_CurToggleIndex = -1;
    public void ToggleChange(int i)
    {
        if (m_CurToggleIndex != i)
        {
            if (m_CurToggleIndex >= 0)
                C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI(m_UINameVector[m_CurToggleIndex]);

            m_CurToggleIndex = i;

            C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI(m_UINameVector[i]);

           /// AudioMgr.PlaySoundEffect("public_sd_042");
        }
    }

    public void GoSDKShop()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendOpenShop();

      //  AudioMgr.PlaySoundEffect("public_sd_042");
    }
}