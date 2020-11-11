using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideMgr : C_MonoSingleton<GuideMgr>
{
    public string c_CurGuide = "";

    public bool c_FirstStageGuideing = false;
    private float m_fFirstStageTime = 5.0f;
    private int m_nCurStageIndex = 0;

    void Update()
    {
        //if (Delegates.c_UI_MainCity != null)
        //{
        //    if (c_CurGuide == "FirstStage")
        //    {
        //        if (Delegates.c_UI_MainCity.CurStageIndex > 0)
        //        {
        //            if (!c_FirstStageGuideing)
        //            {
        //                if (m_nCurStageIndex != Delegates.c_UI_MainCity.CurStageIndex)
        //                {
        //                    m_nCurStageIndex = Delegates.c_UI_MainCity.CurStageIndex;
        //                    m_fFirstStageTime = 5.0f;
        //                }
        //                else
        //                {
        //                    m_fFirstStageTime -= Time.deltaTime;

        //                    if (m_fFirstStageTime <= 0)
        //                        c_FirstStageGuideing = true;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            c_FirstStageGuideing = false;
        //            m_fFirstStageTime = 5.0f;
        //        }
        //    }
        //    else
        //    {
        //        c_FirstStageGuideing = false;
        //    }
        //}
    }

    public void TriggerGuide(string guide)
    {
        c_CurGuide = guide;
        FirstLoginGuide();
        //return;

        //if (guide == "FirstLogin")
        //    FirstLoginGuide();
        //else if (guide == "FirstStage")
        //    FirstStage();
    }

    public bool IsFirstLogin()
    {
      
        if (NetworkMgr.IsConnected 
            && string.IsNullOrEmpty(PlayerData.BabyName)
            && PlayerPrefs.GetString(PlayerPrefsData.GUIDE_FIRST_LOGIN, "") != PlayerData.UID)
        {
            PlayerPrefs.SetString(PlayerPrefsData.GUIDE_FIRST_LOGIN, PlayerData.UID);
            PlayerPrefs.Save();

            return true;
        }

        return false;
    }

    private void FirstLoginGuide()
    {
        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_FirstLoginGuide");
    }

    public bool IsNoPlayStage()
    {
        //StageDataItem item = StageData.GetStageDataItem("a");

        //if ((item == null || (item != null && item.State != 2)) && C_Singleton<StageMgr>.GetInstance().c_CompleteStageData == null)
        //    return true;

        return false;
    }

    private void FirstStage()
    {
        //if (Delegates.c_UI_MainCity != null)
        //{
        //    Delegates.c_UI_MainCity.ShowStageFinger();
        //    Invoke("FirstStage_PlayAudio", 10.0f);
        //}
    }

    private void FirstStage_PlayAudio()
    {
        //if (c_CurGuide == "FirstStage")
        //    C_MonoSingleton<PlayerController>.GetInstance().PlayAudio("common_573");
    }
}
