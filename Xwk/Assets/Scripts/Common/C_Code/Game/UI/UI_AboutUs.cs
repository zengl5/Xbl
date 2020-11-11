using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AboutUs : C_BaseUI
{
    [SerializeField]
    private Text m_Text_Version = null;

    //调试代码
    [SerializeField]
    private GameObject m_DebugLayer = null;
    private int m_nDebugCount = 0;

    protected override void onInit()
    {
        m_Text_Version.text = string.Format(C_Localization.GetLocalization("LOACAL_ABOUT_US_VERSION"), GameConfig.AppVersion);
    }

    protected override void onAdaption()
    {
        if (C_UIMgr.c_AspectRatio < 1.4f)
        {
            Vector3 pos = m_Text_Version.transform.localPosition;
            pos.y += 150.0f;
            m_Text_Version.transform.localPosition = pos;
        }
    }

    public void OpenPrivacyAgreement()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendOpenPrivacyAgreement();
    }
    /// <summary>
    /// 用户服务协议
    /// </summary>
    public void OpenUserServiceProtocol()
    {
        C_DebugHelper.Log("OpenUserServiceProtocol");
        GameHelper.Instance.OpenUserServiceProtocol();

    }
    /// <summary>
    /// 用户隐私保护政策
    /// </summary>
    public void OpenUserPrivacyProtectionPolic()
    {
        C_DebugHelper.Log("OpenUserPrivacyProtectionPolic");
        GameHelper.Instance.OpenUserPrivacyProtectionPolic();

    }
    /// <summary>
    /// 会员服务协议
    /// </summary>
    public void OpenMemberServiceAgreement()
    {
        C_DebugHelper.Log("OpenMemberServiceAgreement");
        GameHelper.Instance.OpenMemberServiceAgreement();
    }


    //调试代码
    public void Debug()
    {
        m_nDebugCount++;
        if (m_nDebugCount > 20)
            m_DebugLayer.SetActive(true);
    }

    public void StartRecord()
    {
        C_MonoSingleton<GameHelper>.GetInstance().sendTecentStartRecord("大小",1,1);
    }

    public void EndRecord()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendEndRecord();
    }

    public void GetRecord()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendGetRecord();
    }
}
