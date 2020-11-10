using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DialogBox_OneButton : C_BaseUI
{
    [SerializeField]
    private Text m_Text_Content = null;
    [SerializeField]
    private Text m_Text_OneButton = null;

    private Action m_Callback = null;

    protected override void onOpenUI(params object[] uiObjParams)
    {
        if (uiObjParams.Length >= 3)
        {
            m_Text_Content.text = C_Localization.GetLocalization(uiObjParams[0].ToString());
            m_Callback = (Action)uiObjParams[1];

            if (!string.IsNullOrEmpty(uiObjParams[2].ToString()))
                m_Text_OneButton.text = C_Localization.GetLocalization(uiObjParams[2].ToString());
        }
    }

    public void OneButtonCallback()
    {
        if (m_Callback != null)
            m_Callback();

        CloseUI();
    }
}
