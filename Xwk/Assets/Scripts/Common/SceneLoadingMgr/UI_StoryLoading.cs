using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using UnityEngine.UI;

public class UI_StoryLoading : C_BaseUI
{
    [SerializeField]
    private Image m_Image_Loading = null;

    private float m_fCurrentSliderValue = 0.0f;

    protected override void onUpdate()
    {
        m_Image_Loading.fillAmount = m_fCurrentSliderValue;
    }

    public void UpdateProgressVaule(float vaule)
    {
        m_fCurrentSliderValue = vaule;
    }

    public void CloseLoadingUI()
    {
        CloseUI();
    }
}
