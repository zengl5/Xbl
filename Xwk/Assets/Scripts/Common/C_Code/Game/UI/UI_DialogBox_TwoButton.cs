using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DialogBox_TwoButton : C_BaseUI
{
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
        if (uiObjParams.Length >= 5)
        {
            m_Text_Content.text = C_Localization.GetLocalization(uiObjParams[0].ToString());

            m_Callback1 = (Action)uiObjParams[1];
            m_Callback2 = (Action)uiObjParams[2];

            if (!string.IsNullOrEmpty(uiObjParams[3].ToString()))
                m_Text_OneButton.text = C_Localization.GetLocalization(uiObjParams[3].ToString());

            if (!string.IsNullOrEmpty(uiObjParams[4].ToString()))
                m_Text_TwoButton.text = C_Localization.GetLocalization(uiObjParams[4].ToString());
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
