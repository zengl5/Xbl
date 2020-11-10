using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slate;
public class TitleMoudleMgr : C_MonoSingleton<TitleMoudleMgr> {
    public Action mGameGoMainCityAction = null;
    public Action mGameContinueAction = null;
    public Action mGamePauseAction = null;
    protected static string UI_StoryPause;
    protected static string UI_StoryTitle;

    protected void StoryPauseAction()
    {
        //CutsceneSequencePlayer cutsceneSequencePlayer = GameObject.FindObjectOfType<CutsceneSequencePlayer>();
        //if (cutsceneSequencePlayer !=null)
        //{
        //    Cutscene cutscene = cutsceneSequencePlayer._CurrentCutScene;
        //    if (cutscene != null)
        //    {
        //        cutscene.Pause();
        //    }
        //}

        Time.timeScale = 0;

        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI(UI_StoryPause, new System.Action(StoryGoMainCityAction), new System.Action(StoryContinueAction));

    }
    protected virtual void OnInit()
    {

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
    protected virtual void Stop()
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

  

