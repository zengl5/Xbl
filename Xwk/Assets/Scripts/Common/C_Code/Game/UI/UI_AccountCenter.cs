using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AccountCenter : C_BaseUI
{
    //-------------------------Avatar-------------------------
    [SerializeField]
    private Image m_Image_Avatar = null;

    //-------------------------Baby Name-------------------------
    [SerializeField]
    private Text m_Text_BabyName = null;

    //-------------------------Birthday-------------------------
    [SerializeField]
    private Text m_Text_Birthday = null;

    //-------------------------Baby Gender-------------------------
    [SerializeField]
    private Toggle[] m_BabyGenderToggleVector = null;
    [SerializeField]
    private GameObject[] m_BabyGenderToggleLabelVector = null;

    //-------------------------UID-------------------------
    [SerializeField]
    private Text m_Text_UID = null;

    //-------------------------Binding-------------------------
    [SerializeField]
    private GameObject m_Button_WeChatBinding = null;
    [SerializeField]
    private GameObject m_Button_PhoneBinding = null;
    [SerializeField]
    private GameObject m_Button_SwitchAccount0 = null;
    [SerializeField]
    private GameObject m_Button_SwitchAccount1 = null;
    [SerializeField]
    private GameObject m_VIPDesc = null;
    [SerializeField]
    private Text m_Text_WeChat = null;
    [SerializeField]
    private Text m_Text_Phone = null;

    private C_Event m_PlayerDataChangeEvent = new C_Event();
    private C_Event m_WeChatBindingEvent = new C_Event();

    private bool m_bRrefreshPlayerData = false;

    protected override void onOpenUI(params object[] uiObjParams)
    {
        m_bRrefreshPlayerData = false;

        RrefreshUI();

        m_PlayerDataChangeEvent.RegisterEvent(C_EnumEventChannel.Global, "PlayerDataChange", (object[] result) =>
        {
            RrefreshUI();
        });

        m_WeChatBindingEvent.RegisterEvent(C_EnumEventChannel.Global, "WeChatBinding", (object[] result) =>
        {
            UpdateBinding();
        });
    }

    protected override void onUpdate()
    {
        m_Text_UID.text = PlayerData.UID.ToString();
    }

    protected override void onCloseUI()
    {
        m_PlayerDataChangeEvent.UnregisterEvent();
        m_WeChatBindingEvent.UnregisterEvent();

        if (m_bRrefreshPlayerData)
        {
            PlayerData.Save();

            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "PlayerDataChange");
        }
    }

    public void RrefreshUI()
    {
        m_Image_Avatar.sprite = C_Singleton<GameDataMgr>.GetInstance().AvatarSprite;

        if (string.IsNullOrEmpty(PlayerData.BabyName))
            m_Text_BabyName.text = C_Localization.GetLocalization("LOACAL_NO_BABY_NAME");
        else
            m_Text_BabyName.text = PlayerData.BabyName;

        m_Text_Birthday.text = PlayerData.BabyBirthday;

        if (!string.IsNullOrEmpty(PlayerData.BabyGender))
        {
            int index = int.Parse(PlayerData.BabyGender);
            if (index == 0)
            {
                m_BabyGenderToggleVector[0].isOn = true;
                m_BabyGenderToggleVector[1].isOn = false;
            }
            else
            {
                m_BabyGenderToggleVector[0].isOn = false;
                m_BabyGenderToggleVector[1].isOn = true;
            }
        }

        UpdateBinding();
    }

    private void UpdateBinding()
    {
        if(string.IsNullOrEmpty(PlayerData.WeChatUnionID))
        {
            m_Button_WeChatBinding.SetActive(true);
        }
        else
        {
            m_Button_WeChatBinding.SetActive(false);
            m_Text_WeChat.text = PlayerData.WeChatUnionID;
        }

        if (string.IsNullOrEmpty(PlayerData.Phone))
        {
            m_Button_PhoneBinding.SetActive(true);
        }
        else
        {
            m_Button_PhoneBinding.SetActive(false);
            m_Text_Phone.text = PlayerData.Phone;
        }

        if (string.IsNullOrEmpty(PlayerData.Phone) && string.IsNullOrEmpty(PlayerData.WeChatUnionID))
        {
            if (PlayerData.IsVIP == 1)
            {
                m_Button_SwitchAccount0.SetActive(false);
                m_Button_SwitchAccount1.SetActive(false);
                m_VIPDesc.SetActive(true);
            }
            else
            {
                m_Button_SwitchAccount0.SetActive(false);
                m_Button_SwitchAccount1.SetActive(true);
                m_VIPDesc.SetActive(false);
            }
        }
        else
        {
            m_Button_SwitchAccount0.SetActive(true);
            m_Button_SwitchAccount1.SetActive(false);
            m_VIPDesc.SetActive(false);
        }
    }

    public void GoAvatarSettting()
    {
        if (!NetworkMgr.IsConnected)
        {
            Tips.Create("LOACAL_NEED_NETWORK");
            return;
        }

        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_AvatarSetting");
    }

    public void GoBabyNameSettting()
    {
        if (!NetworkMgr.IsConnected)
        {
            Tips.Create("LOACAL_NEED_NETWORK");
            return;
        }

        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_BabyNameSetting");
    }

    public void GoBirthdaySettting()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendOpenClock();
    }

    public void GoLogin()
    {
        if (!NetworkMgr.IsConnected)
        {
            Tips.Create("LOACAL_NEED_NETWORK");
            return;
        }

        C_Singleton<C_GameStateCtrl>.GetInstance().GotoState("LoginState");
    }

    public void GoAccountBinding(int type)
    {
        if (!NetworkMgr.IsConnected)
        {
            Tips.Create("LOACAL_NEED_NETWORK");
            return;
        }

        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_AccountBinding", type);
    }

    private int m_nCurBabGenderToggleIndex = -1;
    public void BabyGenderToggleChange(int index)
    {
        if (m_BabyGenderToggleVector[index].isOn)
            m_BabyGenderToggleLabelVector[index].SetActive(false);
        else
            m_BabyGenderToggleLabelVector[index].SetActive(true);

        if (m_nCurBabGenderToggleIndex == index)
            return;

        if (m_nCurBabGenderToggleIndex != -1)
        {
            if (PlayerData.BabyGender != index.ToString())
            {
                PlayerData.BabyGender = index.ToString();

                m_Image_Avatar.sprite = C_Singleton<GameDataMgr>.GetInstance().AvatarSprite;

                m_bRrefreshPlayerData = true;

                C_Singleton<GameDataMgr>.GetInstance().ReportedBabyInfo();
            }
        }

        m_nCurBabGenderToggleIndex = index;
    }

    public void SwitchAccount()
    {
        if (!NetworkMgr.IsConnected)
        {
            Tips.Create("LOACAL_NEED_NETWORK");
            return;
        }

        DialogBox.Create("LOACAL_HINT", "LOACAL_ACCOUNT_CENTER_SWITCH_ACCOUNT_DESC", null, () =>
        {
            GoLogin();

        }, "LOACAL_CANCLE", "LOACAL_CONFIRM");
    }
}
