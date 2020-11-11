using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : C_BaseUI
{
    [SerializeField]
    private Image m_Image_Bg = null;
    [SerializeField]
    private GameObject m_Button_Trigger = null;
    [SerializeField]
    private OnSlidePressed m_OnSlidePressed = null;
    [SerializeField]
    private GameObject m_Button_Close = null;

    private int m_nOpenType = 0;

    protected override void onInit()
    {
        m_OnSlidePressed.Init(() =>
        {
            Trigger();
        });
    }

    protected override void onOpenUI(params object[] uiObjParams)
    {
        C_MonoSingleton<PlayerController>.GetInstance().AutoPlayEnabled = false;

        if (Application.internetReachability != NetworkReachability.NotReachable && string.IsNullOrEmpty(PlayerData.UID))
            GameLoginMgr.RequestUDIDLogin();

        m_nOpenType = (int)uiObjParams[0];

        m_Button_Close.SetActive(true);

        UpdateUI();

        switch (m_nOpenType)
        {
            case 0:
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "pay_openshop_tubiao");
                break;
            case 1:
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "pay_openshop_guanqia");
                break;
            case 2:
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "pay_openshop_tongguan");
                break;
            case 3:
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "pay_openshop_lianxi");
                break;
        }
    }

    protected override void onCloseUI()
    {
        C_MonoSingleton<PlayerController>.GetInstance().AutoPlayEnabled = true;
    }

    public void Close()
    {
        AudioMgr.PlaySoundEffect("public_sd_042");

        m_Button_Close.SetActive(false);

        CloseUI();
    }

    private void UpdateUI()
    {
        StoreConfigItem item = StoreConfig.GetStoreConfig(1);
        if (item != null)
        {
            if (!string.IsNullOrEmpty(item.Voice))
                AudioMgr.PlayAudio_URL_MP3(item.Voice);
            else
                C_MonoSingleton<PlayerController>.GetInstance().PlayAudio("common_546");

            if (C_Singleton<GameDataMgr>.GetInstance().c_StoreSprite != null)
                m_Image_Bg.sprite = C_Singleton<GameDataMgr>.GetInstance().c_StoreSprite;

            if (item.Trigger == 1)
            {
                m_Button_Trigger.SetActive(true);
                m_OnSlidePressed.gameObject.SetActive(false);
            }
            else
            {
                m_Button_Trigger.SetActive(false);
                m_OnSlidePressed.gameObject.SetActive(true);
            }
        }
        else
        {
            m_Button_Trigger.SetActive(false);
            m_OnSlidePressed.gameObject.SetActive(true);
            C_MonoSingleton<PlayerController>.GetInstance().PlayAudio("common_546");
        }

        m_Image_Bg.gameObject.SetActive(true);
    }

    public void Trigger()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Tips.Create("LOACAL_NO_NETWORK");
            return;
        }

        if (string.IsNullOrEmpty(PlayerData.UID))
        {
            GameLoginMgr.RequestUDIDLogin();

            Tips.Create("LOACAL_NO_NETWORK");
            return;
        }

        CloseUI();

        this.gameObject.SetActive(false);

        switch (m_nOpenType)
        {
            case 0:
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "pay_opensdk_tubiao");
                break;
            case 1:
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "pay_opensdk_guanqia");
                break;
            case 2:
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "pay_opensdk_tongguan");
                break;
            case 3:
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "pay_opensdk_lianxi");
                break;
        }

        C_MonoSingleton<GameHelper>.GetInstance().SendOpenShop();
    }
}
