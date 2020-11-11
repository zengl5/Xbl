using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HeadlineShowJKBState : C_IState
{
    private HeadlineAnimationGuide _GoldenCudgelGuide;

    public string Name
    {
        get;

        set;
    }

    public void OnStateEnter()
    {
        Init();

    }
    void Init()
    {
#if true
        ShowOver();
#elif false
        GameObject _GoldenCudgelGuideOBJ = new GameObject("HeadlineAnimationGuide");
        _GoldenCudgelGuide = _GoldenCudgelGuideOBJ.AddComponent<HeadlineAnimationGuide>();
        _GoldenCudgelGuide.OnFinish = ()=> { ShowOver(); };
        //ShowOver();
#endif
    }
    void ShowOver()
    {
        //处理结束
      //  PlayerPrefs.SetString(PlayerPrefsData.START_VIDEO_VERSION, GameConfig.AppVersion);
      //  PlayerPrefs.Save();

        // GameLaunchMgr.c_FinshCurStep = true;
        //进入主界面状态
          C_GameStateCtrl.Instance.GotoState("MainCityState");
        
    }
    public void OnStateLeave()
    {
        if(_GoldenCudgelGuide!=null)
            GameObject.DestroyImmediate(_GoldenCudgelGuide.gameObject);
    }

    public void OnStateOverride()
    {
    }

    public void OnStateResume()
    {

    }
}
