using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Recommend : C_BaseUI
{
    [SerializeField]
    private GameObject m_ui_public_effect_pixing = null;
    [SerializeField]
    private GameObject m_Parent = null;
    [SerializeField]
    private GameObject m_Button = null;
    [SerializeField]
    private GameObject m_Hand= null;
    private int m_Times = 0;
    private bool m_Dead = false;
    protected override void onOpenUI(params object[] uiObjParams)
    {
        // m_ui_public_effect_pixing.SetActive(false);
        //播放手的动画
      //  C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 3);

        Invoke("ShowStar", 2.0f);

        m_Times = 0;


    }

    protected override void onCloseUI()
    {
       
    }
    public void DoClose()
    {
      //  C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent",1);
        CloseUI();
    }

    private void ShowStar()
    {
        if (m_Times == 0)
        {
            //删除手
            GameObject.DestroyObject(m_Hand);
            m_Hand = null;
        }
        m_Times++;
        
        if(m_Times > 5){
            //显示按钮
            m_Button.gameObject.SetActive(true);
            return;
        }
        //显示星星
        Transform star = m_Parent.transform.GetChild(m_Times-1);
        star.gameObject.SetActive(true);
        Invoke("ShowStar", 0.2f);
    }

    public void GoSDKRecommend()
    {
       // C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 1);

        C_MonoSingleton<GameHelper>.GetInstance().SendOpen5StarRecommend();

        CloseUI();


    }
}
