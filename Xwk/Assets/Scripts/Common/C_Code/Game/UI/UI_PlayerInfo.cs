using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerInfo : C_BaseUI
{
    //-------------------------Player Info-------------------------
    [SerializeField]
    private Image m_Image_Avatar = null;
    [SerializeField]
    private Text m_Text_BabyName = null;
    [SerializeField]
    private Text m_Text_BabyAge = null;
    [SerializeField]
    private Text m_Text_StarCount = null;

    //-------------------------Level Info-------------------------
    [SerializeField]
    private Image m_Image_ShowCurLevel = null;
    [SerializeField]
    private Text m_Text_CurLevel = null;

    [SerializeField]
    private Image m_Image_CurLevel = null;
    [SerializeField]
    private Image m_Image_NextLevel = null;
    [SerializeField]
    private Image m_Image_ProgressBar = null;

    [SerializeField]
    private GameObject m_Image_UnlockStage = null;
    [SerializeField]
    private Text m_Text_UnlockStage = null;

    protected override void onOpenUI(params object[] uiObjParams)
    {
        C_MonoSingleton<PlayerController>.GetInstance().AutoPlayEnabled = false;
        C_MonoSingleton<PlayerController>.GetInstance().PlayAudio("common_544");

        UpdatePlayerInfo();
        UpdateLevelInfo();
    }

    protected override void onCloseUI()
    {
        AudioMgr.PlaySoundEffect("public_sd_042");

        C_MonoSingleton<PlayerController>.GetInstance().AutoPlayEnabled = true;
    }

    private void UpdatePlayerInfo()
    {
        m_Image_Avatar.sprite = C_Singleton<GameDataMgr>.GetInstance().AvatarSprite;

        if (string.IsNullOrEmpty(PlayerData.BabyName))
            m_Text_BabyName.text = C_Localization.GetLocalization("LOACAL_NO_BABY_NAME");
        else
            m_Text_BabyName.text = PlayerData.BabyName;

        m_Text_BabyAge.text = C_CommonAlgorithm.GetAge(PlayerData.BabyBirthday) + C_Localization.GetLocalization("LOACAL_AGE");
        m_Text_StarCount.text = PlayerData.StarCount.ToString();
    }

    private void UpdateLevelInfo()
    {
        int curlevel = C_Singleton<GameDataMgr>.GetInstance().CurLevel;

        Texture2D curTexture = C_ResMgr.LoadResource<Texture2D>("Level" + curlevel, LocalPath.LevelResourcesPath);
        m_Image_CurLevel.sprite = Sprite.Create(curTexture, new Rect(0, 0, curTexture.width, curTexture.height), new Vector2(0.5f, 0.5f));
        m_Image_ShowCurLevel.sprite = m_Image_CurLevel.sprite;

        m_Text_CurLevel.text = C_Localization.GetLocalization(LevelConfig.LevelName[curlevel]);


        int nextLevel = curlevel + 1;
        if (nextLevel >= LevelConfig.LevelMaxStar.Length)
            nextLevel = LevelConfig.LevelMaxStar.Length - 1;

        Texture2D nextTexture = C_ResMgr.LoadResource<Texture2D>("Level" + nextLevel, LocalPath.LevelResourcesPath);
        m_Image_NextLevel.sprite = Sprite.Create(nextTexture, new Rect(0, 0, nextTexture.width, nextTexture.height), new Vector2(0.5f, 0.5f));

        m_Image_ProgressBar.fillAmount = PlayerData.StarCount / (float)LevelConfig.LevelMaxStar[curlevel];

        if (PlayerData.StarCount > LevelConfig.LevelMaxStar[curlevel])
        {
            m_Image_UnlockStage.SetActive(true);

            m_Text_UnlockStage.text = LevelConfig.LevelUnlockStage[curlevel + 1];
            if (m_Text_UnlockStage.text == "shuangpin")
                m_Text_UnlockStage.text = "双拼";

            RectTransform image_rect = m_Image_UnlockStage.GetComponent<RectTransform>();
            image_rect.sizeDelta = new Vector2(m_Text_UnlockStage.preferredWidth + 30, image_rect.sizeDelta.y);
        }
        else
        {
            m_Image_UnlockStage.SetActive(false);
        }
    }
}
