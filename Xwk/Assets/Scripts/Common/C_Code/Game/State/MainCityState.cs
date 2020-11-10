using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YB.XWK.MainScene;

public class MainCityState : C_IState
{
    public string Name { get; set; }
    public MainCityState()
    {
        Name = "MainCityState";
    }

    public virtual void OnStateEnter()
    {
        C_MonoSingleton<C_UIMgr>.GetInstance().MandatoryCloseUIAll();
        if (LocalData.m_FirstEnterApp)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_start_loading_main_time, LocalData.m_start_loading_main_time);
        }
        //如果没有玩过新手引导，则进入新手引导界面
        if (LocalData.m_FirstEnterApp)
        {
            if (!ChannelConfig.CompareVersionAndChannel("start_guide"))
            {
                if (AppInfoData.GuideStateData == 0 || AppInfoData.GetJingubangStateData == 0)
                {
                    YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "xsyd_lm", () => {

                    });
                    return;
                }
            }
        }
        if (LocalData.m_FirstEnterApp)
        {
#if TEST_MAIN_MOUDLE
            YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("Main","",()=> {
                C_MonoSingleton<C_UIMgr>.GetInstance().MandatoryCloseUIAll();
            });
#else
            //加载场景
            YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "Main", () => {
                Init();
            });
#endif
        }
        else
        {
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent",1);

            Init();
        }
    }

    private void Init()
    {
        Time.timeScale = 1;
    }


    public virtual void OnStateLeave()
    {
        C_MonoSingleton<GameLogic>.GetInstance().LeaveMainCity();
        C_MonoSingleton<C_UIMgr>.GetInstance().MandatoryCloseUIAll();
    }

    public virtual void OnStateOverride()
    {
    }

    public virtual void OnStateResume()
    {
    }
}
