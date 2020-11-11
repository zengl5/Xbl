using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AccountBinding : C_BaseUI
{
    [SerializeField]
    private GameObject m_WeChatBindingLayer = null;
    [SerializeField]
    private GameObject m_PhoneBindingLayer = null;

    //-------------------------Phone Binding-------------------------
    [SerializeField]
    private InputField m_InputField_Phone = null;
    [SerializeField]
    private InputField m_InputField_PhoneVerificationCode = null;
    [SerializeField]
    private Text m_Text_Resend = null;
    [SerializeField]
    private GameObject m_Button_GetPhoneVerificationCode = null;
    [SerializeField]
    private GameObject m_Image_PhoneBinding_Disabled = null;
    [SerializeField]
    private Button m_Button_PhoneBinding = null;
    [SerializeField]
    private GameObject m_Button_ClearPhoneText = null;
    
    private float m_fCurPhoneVerificationCodeWaitTime = 0.0f;

    private C_Event m_PhoneBindingEvent = new C_Event();
    private C_Event m_WeChatBindingEvent = new C_Event();

    private bool m_bOpenUI_Recommend = false;
    private bool m_SelectAgree = false;
    [SerializeField]
    private Toggle m_toggleAgreement;
    protected override void onOpenUI(params object[] uiObjParams)
    {
        if (uiObjParams.Length > 0)
        {
            if ((int)uiObjParams[0] == 0)
            {
                m_WeChatBindingLayer.SetActive(true);
                m_PhoneBindingLayer.SetActive(false);
            }
            else
            {
                m_WeChatBindingLayer.SetActive(false);
                m_PhoneBindingLayer.SetActive(true);
            }
        }

        if (uiObjParams.Length > 1)
            m_bOpenUI_Recommend = true;

        m_InputField_Phone.text = "";
        m_InputField_PhoneVerificationCode.text = "";

        m_PhoneBindingEvent.RegisterEvent(C_EnumEventChannel.Global, "PhoneBinding", (object[] result) =>
        {
            C_DebugHelper.Log("RegisterEvent PhoneBindingEvent result.Length = " + result.Length);

            if (result.Length > 0)
            {
                if ((bool)result[0])
                    CloseUI();
            }
        });

        m_WeChatBindingEvent.RegisterEvent(C_EnumEventChannel.Global, "WeChatBinding", (object[] result) =>
        {
            C_DebugHelper.Log("RegisterEvent WeChatBindingEvent result.Length = " + result.Length);

            if (result.Length > 0)
            {
                if ((bool)result[0])
                    CloseUI();
            }
        });
    }

    protected override void onRealtimeUpdate(float deltaTime)
    {
        //-------------------------Phone Binding-------------------------

        if (m_fCurPhoneVerificationCodeWaitTime > 0)
        {
            m_fCurPhoneVerificationCodeWaitTime -= deltaTime;
            m_Text_Resend.text = string.Format(C_Localization.GetLocalization("LOACAL_LOGIN_RESEND"), (int)m_fCurPhoneVerificationCodeWaitTime);

            if (m_Button_GetPhoneVerificationCode.activeSelf)
                m_Button_GetPhoneVerificationCode.SetActive(false);
        }
        else
        {
            m_Button_GetPhoneVerificationCode.SetActive(true);
        }

        if (m_PhoneBindingLayer.activeSelf)
        {
            if (m_InputField_Phone.text.Length > 0)
                m_Button_ClearPhoneText.SetActive(true);
            else
                m_Button_ClearPhoneText.SetActive(false);

            //效率很低，但是不影响游戏
            if (m_InputField_Phone.text.Length > 0 && m_InputField_PhoneVerificationCode.text.Length > 0)
            {
                m_Button_PhoneBinding.interactable = true;
                m_Image_PhoneBinding_Disabled.SetActive(false);
            }
            else
            {
                m_Button_PhoneBinding.interactable = false;
                m_Image_PhoneBinding_Disabled.SetActive(true);
            }
        }
    }

    protected override void onCloseUI()
    {
        m_PhoneBindingEvent.UnregisterEvent();
        m_WeChatBindingEvent.UnregisterEvent();

        if (m_bOpenUI_Recommend)
        {
            m_bOpenUI_Recommend = false;

#if UNITY_IOS && !UNITY_EDITOR
            C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_Recommend");
#endif
        }
    }

    public void GoPhoneBindingLayer()
    {
        m_PhoneBindingLayer.SetActive(true);
        m_WeChatBindingLayer.SetActive(false);

        m_InputField_PhoneVerificationCode.text = "";
    }

    public void GoWeChatBindingLayer()
    {
        m_PhoneBindingLayer.SetActive(false);
        m_WeChatBindingLayer.SetActive(true);
    }
    public void DoAgreeSelect(bool select)
    {
        m_SelectAgree = m_toggleAgreement.isOn;

    }
    //-------------------------Phone Binding-------------------------
    public void GetPhoneVerificationCode()
    {
        if (!GameLoginMgr.CheckoutPhone(m_InputField_Phone.text))
            return;

        GameLoginMgr.RequestPhoneVerificationCode(m_InputField_Phone.text);

        m_fCurPhoneVerificationCodeWaitTime = APP_CONST.VerificationCodeWaitTime;
    }
    public void PhoneBinding()
    {
        if (!m_SelectAgree)
        {
            Tips.Create("登录前请阅读并同意隐私政策等协议");
            return;
        }
        GameLoginMgr.RequestPhoneBinding(m_InputField_Phone.text, m_InputField_PhoneVerificationCode.text);
    }

    public void ClearPhoneText()
    {
        m_InputField_Phone.text = "";
    }

    //-------------------------WeChat Binding-------------------------
    public void GoWeChatBinding()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendWeChatBinding();
    }

    public void GoWeChatQRCodeBinding()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendWeChatQRCodeBinding();
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


}
