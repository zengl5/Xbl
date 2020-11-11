using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorFramework : C_GameFramework
{
    protected override void Init()
    {
        Utility.DisableAnylitics();
        //初始化设置
        c_DesignWidth = 1920.0f;
        c_DesignHeight = 1080.0f;

        base.Init();

        //设置屏幕正方向在Home键右边
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    protected override void StartPrepareSystem()
    {
        base.StartPrepareSystem();

        C_Singleton<GameConfigMgr>.CreateInstance();

        C_Singleton<GameDataMgr>.CreateInstance();

        C_MonoSingleton<GameHelper>.GetInstance();

        C_MonoSingleton<GameLogic>.GetInstance();
    }

    void Start()
    {
        //设置屏幕自动旋转， 并置支持的方向
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        //设置游戏帧率
        Application.targetFrameRate = 40;
#if !UNITY_EDITOR
        //设置游戏在后台运行，这一属性在Android和iOS上被忽略了
        Application.runInBackground = false;
#else
        Application.runInBackground = true;
#endif
        //设置游戏运行时，不黑屏
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //多点触控
        Input.multiTouchEnabled = true;

        //C_MonoSingleton<C_AudioMgr>.GetInstance().SetMusicAudioVolume(0.2f);
        
        InitGameStateCtrl();

        C_MonoSingleton<GameLaunchMgr>.GetInstance();
    }

    private void InitGameStateCtrl()
    {
        C_Singleton<C_GameStateCtrl>.GetInstance().RegisterState("PlayState", new PlayState());
        C_Singleton<C_GameStateCtrl>.GetInstance().RegisterState("ShowJKBState", new HeadlineShowJKBState());
        C_Singleton<C_GameStateCtrl>.GetInstance().RegisterState("MainCityState", new MainCityState());
        C_Singleton<C_GameStateCtrl>.GetInstance().RegisterState("LoginState", new LoginState());
    }
}
