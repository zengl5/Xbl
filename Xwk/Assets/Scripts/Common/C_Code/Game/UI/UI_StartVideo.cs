using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class UI_StartVideo : C_BaseUI
{
    [SerializeField]
    private VideoPlayer m_VideoPlayer = null;

    private bool m_bRunning = false;

    protected override void onOpenUI(params object[] uiObjParams)
    {
        m_VideoPlayer.url = Application.streamingAssetsPath + "/video/video_start.mp4";
        m_VideoPlayer.targetCamera = C_UIMgr.c_UICameraHigh;

        StartCoroutine(PlayVideo());
    }

    protected override void onUpdate()
    {
        if (m_bRunning && !m_VideoPlayer.isPlaying)
            CloseUI();
    }

    protected override void onCloseUI()
    {
        m_VideoPlayer.Stop();
        
        GameLaunchMgr.c_FinshCurStep = true;
    }

    private IEnumerator PlayVideo()
    {
        m_VideoPlayer.Prepare();

        while (!m_VideoPlayer.isPrepared)
        {
            yield return null;
        }
        
        m_VideoPlayer.Play();

        m_bRunning = true;
    }
}
