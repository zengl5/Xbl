using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tips : C_BaseUI
{
    [SerializeField]
    private Text m_Text_Content = null;
    [SerializeField]
    private RectTransform m_Bg = null;

    private float m_fDuration = 2.0f;
    protected override void onOpenUI(params object[] uiObjParams)
    {
        if (uiObjParams.Length > 0)
        {
            string content = C_Localization.GetLocalization(uiObjParams[0].ToString());
            if (string.IsNullOrEmpty(content))
            {
                CloseUI();
                return;
            }

            m_Text_Content.text = content;

            if (uiObjParams.Length > 1)
                m_fDuration = (float)uiObjParams[1];
            else
                m_fDuration = 2.0f;

            RectTransform rect = m_Text_Content.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(m_Text_Content.preferredWidth, m_Text_Content.preferredHeight);
            m_Bg.sizeDelta = new Vector2(rect.sizeDelta.x + 100, rect.sizeDelta.y + 100);
        }
        else
        {
            CloseUI();
        }
    }
    
    protected override void onUpdate()
    {
        if (m_fDuration > 0)
            m_fDuration -= Time.deltaTime;
        else
            CloseUI();
    }
}
