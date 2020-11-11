using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.C_Framework;
public class GoldhoopbarUIControl : C_BaseUI {

    public static GoldhoopbarUIControl Instance;
    public Button SpeechButton;
    void Awake()
    {
        Instance = this;
        SpeechButton.onClick.AddListener(SpeechOnclickEvent);
        ShowSpeechButton(false);
    }	     
    public void ReturnMainScene()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, YB.XWK.MainScene.LocalData.m_game_jingubang, "close_scene");
        if(GoldhoopbarManager.Instance)
        GoldhoopbarManager.Instance.ReturnMainScene();
    }
    public void ShowSpeechButton(bool flag)
    {
        SpeechButton.transform.gameObject.SetActive(flag);
    } 
    
    //语音识别按钮事件
    void SpeechOnclickEvent()
    {
        AudioManager.Instance.StopAllSounds();
        AudioManager.Instance.StopAllEffect();
        ShowSpeechButton(false);
        GoldhoopbarManager.Instance.ResetError();
        GoldhoopbarManager.Instance.ShowRecognizeByIcon(AgainShowSpeechButton);
        //开启语音侦听命令
        //通知开启语音识别的监听
    }
    void AgainShowSpeechButton()
    {
        SpeechButton.transform.gameObject.SetActive(true);
    }



}
