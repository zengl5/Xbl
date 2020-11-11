using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LearningSetting : C_BaseUI
{
    //-------------------------Learning Rhythm-------------------------
    [SerializeField]
    private Toggle[] m_LearningRhythm_ToggleVector = null;
    [SerializeField]
    private Text[] m_LearningRhythm_TextVector = null;

    //-------------------------Learning Time-------------------------
    [SerializeField]
    private Toggle[] m_LearningTime_ToggleVector = null;
    [SerializeField]
    private Text[] m_LearningTime_TextVector = null;

    //-------------------------Rest Time-------------------------
    [SerializeField]
    private Toggle[] m_RestTime_ToggleVector = null;
    [SerializeField]
    private Text[] m_RestTime_TextVector = null;

    private bool m_IsInit = false;

    protected override void onOpenUI(params object[] uiObjParams)
    {
        RequestDownloadCourseInfo();

        UpdateLearningSettingData();

        m_IsInit = true;
    }

    private void RequestDownloadCourseInfo()
    {
    }

    private void UpdateLearningSettingData()
    {
        //-------------------------Learning Rhythm-------------------------
        m_LearningRhythm_ToggleVector[PlayerData.LearningRhythm].isOn = true;
        for (int i = 0; i < m_LearningRhythm_TextVector.Length; i++)
            m_LearningRhythm_TextVector[i].text = C_Localization.GetLocalization(LearningConfig.LearningRhythmName[i]);

        //-------------------------Learning Time-------------------------
        for (int j = 0; j < LearningConfig.LearningTime.Length; j++)
        {
            if (LearningConfig.LearningTime[j] == PlayerData.RestSpan)
                m_LearningTime_ToggleVector[j].isOn = true;
        }
        for (int k = 0; k < m_LearningTime_TextVector.Length; k++)
            m_LearningTime_TextVector[k].text = C_Localization.GetLocalization(LearningConfig.LearningTimeName[k]);

        //-------------------------Rest Time-------------------------
        for (int m = 0; m < LearningConfig.RestTime.Length; m++)
        {
            if (LearningConfig.RestTime[m] == PlayerData.RestTime)
                m_RestTime_ToggleVector[m].isOn = true;
        }
        for (int n = 0; n < m_RestTime_TextVector.Length; n++)
            m_RestTime_TextVector[n].text = C_Localization.GetLocalization(LearningConfig.RestTimeName[n]);
    }

    private int m_nCurLearningRhythmToggleIndex = -1;
    public void LearningRhythmToggleChange(int index)
    {
        if (m_nCurLearningRhythmToggleIndex == index)
            return;

        m_nCurLearningRhythmToggleIndex = index;

        if (m_IsInit)
        {
            //没有接口测试用
            PlayerData.LearningRhythm = index;
            PlayerData.Save();
        }
    }

    private int m_nCurLearningTimeToggleIndex = -1;
    public void LearningTimeToggleChange(int index)
    {
        if (m_nCurLearningTimeToggleIndex == index)
            return;

        m_nCurLearningTimeToggleIndex = index;

        if (m_IsInit)
        {
            //没有接口测试用
            PlayerData.RestSpan = LearningConfig.LearningTime[index];
            PlayerData.Save();
        }
    }

    private int m_nCurRestTimeToggleIndex = -1;
    public void RestTimeToggleChange(int index)
    {
        if (m_nCurRestTimeToggleIndex == index)
            return;

        m_nCurRestTimeToggleIndex = index;

        if (m_IsInit)
        {
            //没有接口测试用
            PlayerData.RestTime = LearningConfig.RestTime[index];
            PlayerData.Save();
        }
    }

    public void SleepTimeChange()
    {
    }

    public void WakeUpTimeChange()
    {
    }
}