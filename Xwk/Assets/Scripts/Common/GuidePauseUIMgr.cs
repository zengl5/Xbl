using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slate;
public class GuidePauseUIMgr : C_MonoSingleton<GuidePauseUIMgr>
{
    protected int times = 0;
    protected GuideCutsceneSequencePlayer _GuideCutsceneSequencePlayer;
    void Start()
    {
        OnInit();
    }
    protected void OnInit()
    {
        times = 0;
        UI_StoryPause = "UI_StoryPause";
        UI_StoryTitle = "UI_StoryTitle_Skip";
        _GuideCutsceneSequencePlayer = GameObject.FindObjectOfType<GuideCutsceneSequencePlayer>();
    }
    protected void StoryGoMainCityAction()
    {
        times++;
         
        _GuideCutsceneSequencePlayer.JumpCutscene(times);
    }


    public Action mGameGoMainCityAction = null;
    public Action mGameContinueAction = null;
    public Action mGamePauseAction = null;
    protected string UI_StoryPause= "UI_StoryPause";
    protected string UI_StoryTitle = "UI_StoryTitle_Skip";

    protected void StoryPauseAction()
    {
        Time.timeScale = 0;

        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI(UI_StoryPause, new System.Action(StoryGoMainCityAction), new System.Action(StoryContinueAction));

    }
     
    public void OpenStoryUI()
    {
        C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI(UI_StoryTitle);

        mGameGoMainCityAction = null;
        mGameContinueAction = null;
        mGamePauseAction = null;
        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI(UI_StoryTitle, new System.Action(StoryGoMainCityAction));
    }

    protected void StoryContinueAction()
    {
        Time.timeScale = 1f;
    }
    public void ClosePauseUI()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.StopAllCoroutines();
        AudioManager.Instance.StopAllEffect();
        AudioManager.Instance.StopBgMusic();
        AudioManager.Instance.StopPlayerSound();
        C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI(UI_StoryPause);
        if (mGameGoMainCityAction != null)
        {
            mGameGoMainCityAction();
        }
    }
    public virtual void Stop()
    {
        ClosePauseUI();
        C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI(UI_StoryTitle);
        if (Instance != null && Instance.gameObject != null)
        {
            Destroy(Instance.gameObject);
            Instance = null;
        }
    }


    public void OpenGameUI(System.Action GoMainCityAction = null, System.Action PauseAction = null, System.Action ContinueAction = null)
    {
        C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI(UI_StoryTitle);

        mGameGoMainCityAction = GoMainCityAction;
        mGameContinueAction = ContinueAction;
        mGamePauseAction = PauseAction;
        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI(UI_StoryTitle, new System.Action(GamePauseAction));

    }

    private void GamePauseAction()
    {
        if (mGamePauseAction != null)
            mGamePauseAction();
        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI(UI_StoryPause, new System.Action(StoryGoMainCityAction), mGameContinueAction);
    }
}

