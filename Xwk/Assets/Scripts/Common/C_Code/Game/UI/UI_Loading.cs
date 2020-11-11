using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Loading : C_BaseUI
{
    [SerializeField]
    private Text m_Text_Desc = null;
    [SerializeField]
    private Image m_Image_Loading = null;
    
    protected override void onUpdate()
    {
        m_Image_Loading.fillAmount = Loading.c_Rate;
        m_Text_Desc.text = Loading.c_Description;
    }
}
