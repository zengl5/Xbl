using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DialogBox_Title_OneButton : C_BaseUI
{
    [SerializeField]
    private Text m_Text_Title = null;
    [SerializeField]
    private GameObject m_Image_TiShi = null;
    [SerializeField]
    private Text m_Text_Content = null;
    [SerializeField]
    private Text m_Text_OneButton = null;

    private Action m_Callback = null;

    protected override void onOpenUI(params object[] uiObjParams)
    {
        if (uiObjParams.Length >= 4)
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
            m_Callback = (Action)uiObjParams[2];

            if (!string.IsNullOrEmpty(uiObjParams[3].ToString()))
                m_Text_OneButton.text = C_Localization.GetLocalization(uiObjParams[3].ToString());
        }
    }

    public void OneButtonCallback()
    {
        if (m_Callback != null)
            m_Callback();

        CloseUI();
    }
}
