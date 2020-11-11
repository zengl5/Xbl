using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GetVIP : C_BaseUI
{
    [SerializeField]
    private GameObject m_Button_Confirm = null;
    [SerializeField]
    private GameObject m_Button_GoBinding = null;
    [SerializeField]
    private Text m_Text = null;

    protected override void onOpenUI(params object[] uiObjParams)
    {
        if (string.IsNullOrEmpty(PlayerData.Phone) && string.IsNullOrEmpty(PlayerData.WeChatUnionID))
        {
            m_Button_Confirm.SetActive(false);
            m_Button_GoBinding.SetActive(true);

            m_Text.text = C_Localization.GetLocalization("LOACAL_GET_VIP_GO_BINDING");
        }
        else
        {
            m_Button_Confirm.SetActive(true);
            m_Button_GoBinding.SetActive(false);
        }
    }

    public void Close()
    {
#if UNITY_IOS && !UNITY_EDITOR
        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_Recommend");
#endif
        CloseUI();
    }

    public void GoBinding()
    {
        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_AccountBinding", 0, 1);

        CloseUI();
    }
}
