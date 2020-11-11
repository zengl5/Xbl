using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Login : C_BaseUI
{
    //-------------------------Login Layer-------------------------
    [SerializeField]
    private GameObject m_PhoneLoginLayer = null;
    [SerializeField]
    private Transform m_ScaleLayer = null;
    [SerializeField]
    private GameObject m_WeChatLoginLayer = null;
    [SerializeField]
    private GameObject m_NoWeChatLoginLayer = null;

    //-------------------------Phone Login-------------------------
    [SerializeField]
    private Transform m_ScaleLayer_PhoneLogin = null;
    [SerializeField]
    private InputField m_InputField_Phone = null;
    [SerializeField]
    private InputField m_InputField_PhoneVerificationCode = null;
    [SerializeField]
    private Text m_Text_Resend = null;
    [SerializeField]
    private GameObject m_Button_GetPhoneVerificationCode = null;
    [SerializeField]
    private GameObject m_Image_PhoneLogin_Disabled = null;
    [SerializeField]
    private Button m_Button_PhoneLogin = null;

    private float m_fCurPhoneVerificationCodeWaitTime = 0.0f;

    private bool m_SelectAgree = false;

    [SerializeField]
    private Toggle m_toggleAgreement;


    protected override void onInit()
    {
        if (C_UIMgr.c_AspectRatio < 1.4f)
        {
            m_ScaleLayer.localScale = new Vector3(0.85f, 0.85f, 0.85f);
            m_ScaleLayer_PhoneLogin.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
    }

    protected override void onOpenUI(params object[] uiObjParams)
    {
        //先将数据同步到服务器
       GameDataMgr.Instance.Synchrodata();

        m_InputField_Phone.text = "";
        m_InputField_PhoneVerificationCode.text = "";

        if (GameDataMgr.c_WeChatEnvironment == 0)
        {
            m_WeChatLoginLayer.SetActive(false);
            m_NoWeChatLoginLayer.SetActive(true);
        }
        else
        {
            m_WeChatLoginLayer.SetActive(true);
            m_NoWeChatLoginLayer.SetActive(false);
        }

        m_PhoneLoginLayer.SetActive(false);
    }

    protected override void onRealtimeUpdate(float deltaTime)
    {
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

        //效率很低，但是不影响游戏
        if (m_InputField_Phone.text.Length > 0 && m_InputField_PhoneVerificationCode.text.Length > 0)
        {
            m_Button_PhoneLogin.interactable = true;
            m_Image_PhoneLogin_Disabled.SetActive(false);
        }
        else
        {
            m_Button_PhoneLogin.interactable = false;
            m_Image_PhoneLogin_Disabled.SetActive(true);
        }
    }

    //-------------------------Login Layer-------------------------
    public void GoPhoneLoginLayer()
    {
        m_toggleAgreement.isOn = false;

        m_PhoneLoginLayer.SetActive(true);

        m_InputField_PhoneVerificationCode.text = "";
    }

    public void SendWechatLogin()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendWeChatLogin();
    }

    public void SendWeChatQRCodeLogin()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendWeChatQRCodeLogin();
    }
    
    public void VisitorLogin()
    {
        GameLoginMgr.RequestUDIDLogin();
    }

    public void GoLoginLayer()
    {
        m_PhoneLoginLayer.SetActive(false);
    }

    //-------------------------Phone Login-------------------------
    public void GetPhoneVerificationCode()
    {
        if (!GameLoginMgr.CheckoutPhone(m_InputField_Phone.text))
            return;

        GameLoginMgr.RequestPhoneVerificationCode(m_InputField_Phone.text);

        m_fCurPhoneVerificationCodeWaitTime = APP_CONST.VerificationCodeWaitTime;
    }

    public void PhoneLogin()
    {
        if (!m_SelectAgree)
        {
            Tips.Create("登录前请阅读并同意隐私政策等协议");
            return;
        }
        GameLoginMgr.RequestPhoneLogin(m_InputField_Phone.text, m_InputField_PhoneVerificationCode.text);
    }

    public void ClearPhoneText()
    {
        m_InputField_Phone.text = "";
    }

    public void DoAgreeSelect(bool select)
    {
        m_SelectAgree = m_toggleAgreement.isOn;

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
