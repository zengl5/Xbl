using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageDownload : C_BaseUI
{
    [SerializeField]
    private Image m_Image_Loading = null;
    [SerializeField]
    private Text m_Text_Percent = null;
    [SerializeField]
    private Text m_Text_Desc = null;
    [SerializeField]
    private GameObject m_NoNetwork = null;
    [SerializeField]
    private GameObject m_Network = null;
    [SerializeField]
    private GameObject[] m_NoNetworkDianVector = null;
    [SerializeField]
    private GameObject[] m_NetworkDianVector = null;

    private bool m_bRunning = false;

    protected override void onOpenUI(params object[] uiObjParams)
    {
        C_MonoSingleton<PlayerController>.GetInstance().AutoPlayEnabled = false;

        if (m_bRunning == false)
        {
            m_bRunning = true;

            Action0();
        }

        AudioMgr.PlaySoundEffect("common_586");
    }

    protected override void onUpdate()
    {
        m_Image_Loading.fillAmount = GameStageHotUpdateMgr.c_FillAmount;
        m_Text_Percent.text = GameStageHotUpdateMgr.c_Percent;
        m_Text_Desc.text = GameStageHotUpdateMgr.c_Desc;

        if (GameStageHotUpdateMgr.c_State == 0)
        {
            m_NoNetwork.SetActive(true);
            m_Network.SetActive(false);
        }
        else
        {
            m_NoNetwork.SetActive(false);
            m_Network.SetActive(true);
        }
    }

    protected override void onCloseUI()
    {
        C_MonoSingleton<PlayerController>.GetInstance().AutoPlayEnabled = true;

        m_bRunning = false;
    }

    public void CloseDownload()
    {
        CloseUI();

        C_MonoSingleton<GameStageHotUpdateMgr>.GetInstance().CloseDownload();
    }

    private void Action0()
    {
        if (m_bRunning)
        {
            m_NoNetworkDianVector[0].SetActive(false);
            m_NoNetworkDianVector[1].SetActive(false);
            m_NoNetworkDianVector[2].SetActive(false);

            m_NetworkDianVector[0].SetActive(false);
            m_NetworkDianVector[1].SetActive(false);
            m_NetworkDianVector[2].SetActive(false);

            Invoke("Action1", 1.0f);
        }
    }

    private void Action1()
    {
        if (m_bRunning)
        {
            m_NoNetworkDianVector[0].SetActive(true);
            m_NetworkDianVector[0].SetActive(true);

            Invoke("Action2", 1.0f);
        }
    }

    private void Action2()
    {
        if (m_bRunning)
        {
            m_NoNetworkDianVector[1].SetActive(true);
            m_NetworkDianVector[1].SetActive(true);

            Invoke("Action3", 1.0f);
        }
    }

    private void Action3()
    {
        if (m_bRunning)
        {
            m_NoNetworkDianVector[2].SetActive(true);
            m_NetworkDianVector[2].SetActive(true);

            Invoke("Action0", 1.0f);
        }
    }
}