using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Help : C_BaseUI
{
    [SerializeField]
    private RectTransform m_Content1 = null;
    [SerializeField]
    private RectTransform m_Image_Content0 = null;
    [SerializeField]
    private RectTransform m_Image_Content1 = null;
    [SerializeField]
    private GameObject m_Text_Content0 = null;
    [SerializeField]
    private GameObject m_Text_Content1 = null;

    private bool m_Content0Enabled = false;
    private bool m_Content1Enabled = false;

    private Vector2 m_Original = new Vector2(1250, 80);
    private Vector2 m_Ver2Content0 = new Vector2(1250, 470);
    private Vector2 m_Ver2Content1 = new Vector2(1250, 210);

    private Vector3 m_Ver3Original = new Vector3(0, -140, 0);
    private Vector3 m_Ver3Content1 = new Vector3(0, -530, 0);
    
    protected override void onUpdate()
    {
        if (m_Content0Enabled)
        {
            m_Text_Content0.SetActive(true);
            m_Image_Content0.sizeDelta = m_Ver2Content0;

            m_Content1.localPosition = m_Ver3Content1;
        }
        else
        {
            m_Text_Content0.SetActive(false);
            m_Image_Content0.sizeDelta = m_Original;

            m_Content1.localPosition = m_Ver3Original;
        }

        if (m_Content1Enabled)
        {
            m_Text_Content1.SetActive(true);
            m_Image_Content1.sizeDelta = m_Ver2Content1;
        }
        else
        {
            m_Text_Content1.SetActive(false);
            m_Image_Content1.sizeDelta = m_Original;
        }
    }

    protected override void onCloseUI()
    {
        AudioMgr.PlaySoundEffect("public_sd_042");
    }

    public void ChangeContent0()
    {
        m_Content0Enabled = !m_Content0Enabled;
    }

    public void ChangeContent1()
    {
        m_Content1Enabled = !m_Content1Enabled;
    }
}
