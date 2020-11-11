using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AvatarSetting : C_BaseUI
{
    [SerializeField]
    private Image m_Image_Avatar = null;

    private C_Event m_PlayerDataChangeEvent = new C_Event();

    protected override void onOpenUI(params object[] uiObjParams)
    {
        m_Image_Avatar.sprite = C_Singleton<GameDataMgr>.GetInstance().AvatarSprite;

        m_PlayerDataChangeEvent.RegisterEvent(C_EnumEventChannel.Global, "PlayerDataChange", (object[] result) =>
        {
            m_Image_Avatar.sprite = C_Singleton<GameDataMgr>.GetInstance().AvatarSprite;
        });
    }

    protected override void onCloseUI()
    {
        m_PlayerDataChangeEvent.UnregisterEvent();
    }

    public void Photograph()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendPhotograph();
    }

    public void UsePhotoAlbum()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendUsePhotoAlbum();
    }
}
