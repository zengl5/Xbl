using Assets.Scripts.C_Framework;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Upgrade : C_BaseUI
{
    [SerializeField]
    private Transform m_MainBg = null;
    [SerializeField]
    private GameObject[] m_UpgradeVector = null;

    [SerializeField]
    private DOTweenAnimation m_DOTweenAnimation0 = null;
    [SerializeField]
    private DOTweenAnimation m_DOTweenAnimation1 = null;
    [SerializeField]
    private DOTweenAnimation m_DOTweenAnimation2 = null;

    [SerializeField]
    private ParticleSystem[] m_ParticleSystemVector = null;

    [SerializeField]
    private AudioSource m_AudioSource = null;

    private GameObject m_CurUpgrade = null;

    private Vector3 m_MainBgEulerAngles = Vector3.zero;

    private int m_nBeforeLevel = 0;
    private int m_nBeforeGrade = 0;
    private Action m_Callback = null;

    private int m_nCurGrade = 0;

    private bool m_bCloseEnabled = false;

    protected override void onUpdate()
    {
        m_MainBgEulerAngles.z = (m_MainBgEulerAngles.z - 3.0f) % 360;

        m_MainBg.localEulerAngles = m_MainBgEulerAngles;
    }

    public void InitUpgrade(int beforeLevel, int beforeGrade, Action callback)
    {
        m_nBeforeLevel = beforeLevel;
        m_nBeforeGrade = beforeGrade;
        m_Callback = callback;

        m_bCloseEnabled = false;

        m_nCurGrade = C_Singleton<GameDataMgr>.GetInstance().CurGrade;
        m_CurUpgrade = m_UpgradeVector[m_nCurGrade];
        m_CurUpgrade.SetActive(true);

        RunLevelAciton();

        Invoke("CloseEnabled", 3.0f);
        Invoke("Close", 8.0f);

        if (m_nBeforeGrade != m_nCurGrade)
        {
            C_MonoSingleton<C_AudioMgr>.GetInstance().PlayClip(m_AudioSource, C_Singleton<GameResMgr>.GetInstance().LoadResource_Audio_Effect("common_340"));
            Invoke("PlayCurUpgradeAudio", 3.0f);
        }
        else
        {
            C_MonoSingleton<C_AudioMgr>.GetInstance().PlayClip(m_AudioSource, C_Singleton<GameResMgr>.GetInstance().LoadResource_Audio_Effect("common_339"));
        }
    }

    private void PlayCurUpgradeAudio()
    {
        string audioName = "";

        switch (m_nCurGrade)
        {
            case 0:
                audioName = "common_342";
                break;
            case 1:
                audioName = "common_343";
                break;
            case 2:
                audioName = "common_344";
                break;
            case 3:
                audioName = "common_345";
                break;
            case 4:
                audioName = "common_346";
                break;
            case 5:
                audioName = "common_347";
                break;
        }

        C_MonoSingleton<C_AudioMgr>.GetInstance().PlayClip(m_AudioSource, C_Singleton<GameResMgr>.GetInstance().LoadResource_Audio_Effect(audioName));

        Invoke("PlayNiceAudio", 2.0f);
    }

    private void PlayNiceAudio()
    {
        C_MonoSingleton<C_AudioMgr>.GetInstance().PlayClip(m_AudioSource, C_Singleton<GameResMgr>.GetInstance().LoadResource_Audio_Effect("common_341"));
    }

    private void CloseEnabled()
    {
        m_bCloseEnabled = true;
    }

    private void RunLevelAciton()
    {
        if (C_Singleton<GameDataMgr>.GetInstance().CurLevel % 3 == 0)
        {
            Invoke("RunDiamond0_Start", 1.0f);
        }
        else if (C_Singleton<GameDataMgr>.GetInstance().CurLevel % 3 == 1)
        {
            m_DOTweenAnimation0.gameObject.SetActive(true);
            Invoke("RunDiamond1_Start", 1.0f);
        }
        else
        {
            m_DOTweenAnimation0.gameObject.SetActive(true);
            m_DOTweenAnimation1.gameObject.SetActive(true);
            Invoke("RunDiamond2_Start", 1.0f);
        }
    }

    private void RunDiamond0_Start()
    {
        m_DOTweenAnimation0.transform.localScale = new Vector3(20, 20, 20);
        m_DOTweenAnimation0.gameObject.SetActive(true);

        m_DOTweenAnimation0.DORestartById("0");
    }

    private void RunDiamond1_Start()
    {
        m_DOTweenAnimation1.transform.localScale = new Vector3(20, 20, 20);
        m_DOTweenAnimation1.gameObject.SetActive(true);

        m_DOTweenAnimation1.DORestartById("0");
    }

    private void RunDiamond2_Start()
    {
        m_DOTweenAnimation2.transform.localScale = new Vector3(20, 20, 20);
        m_DOTweenAnimation2.gameObject.SetActive(true);

        m_DOTweenAnimation2.DORestartById("0");
    }

    public void RunDiamondEnd(int index)
    {
        m_ParticleSystemVector[index].gameObject.SetActive(true);
        m_ParticleSystemVector[index].Play();
    }

    public void Close()
    {
        if (m_bCloseEnabled)
        {
            CloseUI();

            if (m_Callback != null)
            {
                m_Callback();

                m_Callback = null;
            }
        }
    }
}
