using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if !C_Framework
using QFramework;
#endif

public class GameConfigMgr : C_Singleton<GameConfigMgr>
{
    protected override void Init()
    {
        C_DebugHelper.Log("GameConfigMgr Init!");

        StageConfig.Load();
        HttpRequestConfig.Load();
        LevelConfig.Load();
        LearningConfig.Load();
        FieldGuideConfig.Load();
        GameConfig.Load();

#if !C_Framework
        if (GameConfig.LogState == 1)
            C_MonoSingleton<QConsole>.GetInstance().SetShowLogin(false);
        else if (GameConfig.LogState == 2)
            C_MonoSingleton<QConsole>.GetInstance().SetShowLogin(true);
#else
            if (GameConfig.LogState > 0)
                C_MonoSingleton<C_Console>.GetInstance();
#endif
    }
}
