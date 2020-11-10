using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slate;
using XWK.Common.UI_Reward;

public class PauseUIMoudleMgr : C_MonoSingleton<PauseUIMoudleMgr>
{
    public Action mGameGoMainCityAction = null;
    public Action mGameContinueAction = null;
    public Action mGamePauseAction = null;
    protected  string UI_StoryPause = "UI_StoryPause";
    protected  string UI_StoryTitle = "UI_StoryTitle"; 
    void Start()
    {
        OnInit();
    }
    protected void StoryPauseAction()
    {
        Time.timeScale = 0;

        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI(UI_StoryPause, new System.Action(StoryGoMainCityAction), new System.Action(StoryContinueAction));

    }
    protected virtual void OnInit()
    {
        UI_StoryPause = "UI_StoryPause";
        UI_StoryTitle = "UI_StoryTitle";
    }
    public void OpenStoryUI()
    {
        C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI(UI_StoryTitle);

        mGameGoMainCityAction = null;
        mGameContinueAction = null;
        mGamePauseAction = null;
        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI(UI_StoryTitle, new System.Action(StoryPauseAction));
    }

    protected void StoryContinueAction()
    {
        Time.timeScale = 1f;
    }
    public void QuitStory()
    {
        RewardUIManager.GetInstance().ClearRewardUI();
        mGameGoMainCityAction = null;
        Time.timeScale = 1f;
        AudioManager.Instance.StopAllCoroutines();
        AudioManager.Instance.StopAllEffect();
        AudioManager.Instance.StopBgMusic();
        AudioManager.Instance.StopPlayerSound();
        C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI(UI_StoryTitle);
        C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI(UI_StoryPause);
       
        if (Instance != null && Instance.gameObject != null)
        {
            Destroy(Instance.gameObject);
            Instance = null;
        }
    }
    public virtual void Stop()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.StopAllCoroutines();
        AudioManager.Instance.StopAllEffect();
        AudioManager.Instance.StopBgMusic();
        AudioManager.Instance.StopPlayerSound();
        C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI(UI_StoryTitle);
        C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI(UI_StoryPause);
        if (mGameGoMainCityAction != null)
        {
            mGameGoMainCityAction();
        }
        if (Instance != null && Instance.gameObject != null)
        {
            Destroy(Instance.gameObject);
            Instance = null;
        }
    }
    protected virtual void StoryGoMainCityAction()
    {
        if(!string.IsNullOrEmpty(YB.XWK.MainScene.LocalData.m_story_moudle))
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, YB.XWK.MainScene.LocalData.m_story_moudle, CutsceneSequencePlayer.CurrentPlayerName+"_jump");

        Stop();
        CutsceneSequencePlayer.BackToMainScene();
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


