using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UI_FirstRecharge : C_BaseUI
{
    [SerializeField]
    private Image m_Image_Bg = null;
    [SerializeField]
    private GameObject m_Button_Trigger = null;
    [SerializeField]
    private OnSlidePressed m_OnSlidePressed = null;
    [SerializeField]
    private GameObject m_Button_Close = null;

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

        StartCoroutine(UpdateUI());

        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "pay_openshop_firstrecharge");
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

    private IEnumerator UpdateUI()
    {
        StoreConfigItem item = StoreConfig.GetStoreConfig(2);
        if (item != null)
        {
            if (!string.IsNullOrEmpty(item.Voice))
                AudioMgr.PlayAudio_URL_MP3(item.Voice);
            else
                C_MonoSingleton<PlayerController>.GetInstance().PlayAudio("common_546");

            if (!string.IsNullOrEmpty(item.Img))
            {
                string filePath = C_LocalPath.DataPath + C_String.GetFileName(item.Img);
                if (!File.Exists(filePath))
                    C_UnityWebRequestDownloader.SyncDownloadFile(item.Img, C_LocalPath.DataPath);

                if (File.Exists(filePath))
                {
                    byte[] bytes = File.ReadAllBytes(filePath);

                    Texture2D texture = new Texture2D(1820, 1024);
                    texture.LoadImage(bytes);

                    m_Image_Bg.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }

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

        yield return null;
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

        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "pay_opensdk_firstrecharge");

        C_MonoSingleton<GameHelper>.GetInstance().SendOpenShop();
    }
}
