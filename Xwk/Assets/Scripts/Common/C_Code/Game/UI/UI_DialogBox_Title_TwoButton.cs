using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DialogBox_Title_TwoButton : C_BaseUI
{
    [SerializeField]
    private Text m_Text_Title = null;
    [SerializeField]
    private GameObject m_Image_TiShi = null;
    [SerializeField]
    private Text m_Text_Content = null;
    [SerializeField]
    private Text m_Text_OneButton = null;
    [SerializeField]
    private Text m_Text_TwoButton = null;

    private Action m_Callback1 = null;
    private Action m_Callback2 = null;

    protected override void onOpenUI(params object[] uiObjParams)
    {
        if (uiObjParams.Length >= 6)
        {
            if (uiObjParams[0].ToString() == "LOACAL_HINT")
            {
                m_Image_TiShi.SetActive(true);
                m_Text_Title.gameObject.SetActive(false);
            }
            else
            {
                m_Image_TiShi.SetActive(false);
                m_Text_Title.gameObject.SetActive(true);
                m_Text_Title.text = C_Localization.GetLocalization(uiObjParams[0].ToString());
            }

            m_Text_Content.text = C_Localization.GetLocalization(uiObjParams[1].ToString());

            m_Callback1 = (Action)uiObjParams[2];
            m_Callback2 = (Action)uiObjParams[3];

            if (!string.IsNullOrEmpty(uiObjParams[4].ToString()))
                m_Text_OneButton.text = C_Localization.GetLocalization(uiObjParams[4].ToString());

            if (!string.IsNullOrEmpty(uiObjParams[5].ToString()))
                m_Text_TwoButton.text = C_Localization.GetLocalization(uiObjParams[5].ToString());
        }
    }

    public void OneButtonCallback()
    {
        if (m_Callback1 != null)
            m_Callback1();

        CloseUI();
    }

    public void TwoButtonCallback()
    {
        if (m_Callback2 != null)
            m_Callback2();

        CloseUI();
    }
}
