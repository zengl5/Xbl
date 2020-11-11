using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using XBL.Core;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using Xbl;
using YB.XWK.MainScene;
using System.Runtime.InteropServices;

public class PiercingeyeManager : MonoBehaviour, IRubbish
{
    public static PiercingeyeManager Instance;
    C_StateMachine machine;
    FireReadyState fireReadyState;
    FireGameState fireGameState;
    FireFinishState fireFinishState;
    GameObject Xwk;
    public GameObject StartCam;
    public GameObject GyroCam;
    public GameObject JlCam;
    GameObject nextButton;
    Camera startCam;
    bool introduceFinish = false;
    GameObject public_effect_shoudianji;
    public static bool LockFinish=false;
    public bool EditorReadyState = false;
    public bool EditorGameState = false;
     
    void Awake()
    {
        LockFinish = false;
        Instance = this;
        InitSceneInfo();
        InitMachine();
        AddFeedBack();
     }
    void AddFeedBack()
    {
        if (LocalData.m_SpiritGameMode)
        {
            C_MonoSingleton<C_UIMgr>.GetInstance().MandatoryCloseUIAll();
        }

        if (DirectorMgr.DirectorAllState)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_huoyanjingjing, "EnterGameBy:" + LocalData.m_SpiritType);
        }
        else
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_huoyanjingjing, "EnterPiercingeye_NewUser");
        }
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeStart, LocalData.m_game_time, LocalData.m_game_huoyanjingjing);
    }

    #region#状态机初始化    
    void InitSceneInfo()
    {
        startCam = GameObject.Find("StartCam/cam_lot/Main Camera").transform.GetComponent<Camera>();
        Xwk = GameObjectTool.Instance.InitPlayer(FirePath.Instance.xwk, FirePath.Instance.firexwk,0.01f, "piercingeye");
        StartCam.AddComponent<CameraAnimEvent>();
        Xwk.AddComponent<FireXwk>();
        GyroCam.gameObject.SetActive(false);
        JlCam.SetActive(false);
        nextButton = GameObject.Find("FireUICanvas/next");
        nextButton.SetActive(false);
    }
    void InitMachine()
    {
        fireReadyState = new FireReadyState(Xwk, StartCam);
        fireGameState = new FireGameState(Xwk);
        fireGameState.InitCamera(StartCam, GyroCam, JlCam, JlCam);
        fireFinishState = new FireFinishState();
        machine = new C_StateMachine();
        machine.RegisterState("FireReadyState", fireReadyState);
        machine.RegisterState("FireGameState", fireGameState);
        machine.RegisterState("FireFinishState", fireFinishState);
        if (Application.isEditor)
        {
            MeshButtonManager.Instance.SetMeshCamera(startCam);
            if (EditorReadyState)
                GotoState("FireReadyState");//默认播放第一阶段
            else if (EditorGameState)
                GotoState("FireGameState");//默认播放第一阶段
        }
        else
        {
            GotoState("FireReadyState");//默认播放第一阶段
            MeshButtonManager.Instance.SetMeshCamera(startCam);
        }
    }
    public void GotoState(string name)
    {
        machine.ChangeState(name);
    }
    public bool IsState(string name)
    {
        return machine.TopStateName().Equals(name);
    }

    #endregion
    public Camera getCamera()
    {
        if (StartCam != null)
            return StartCam.transform.GetChild(0).GetChild(0).GetComponent<Camera>();
        return null;
    }
    public Camera getJlCam()
    {
        return JlCam.GetComponent<Camera>();
    }
    //销毁旧的精灵
    public void DestoryOldJL()
    {
        fireGameState.DestoryJl();
    }
    public void ShowNormalColorJL()
    {
        fireGameState.SetSpGrayMat(false);
    }
    public void GotoHeti()
    {
        fireReadyState.OpenHeti();
    }
    
    public void CloseDirectEffect()
    {
        if (public_effect_shoudianji)
            public_effect_shoudianji.SetActive(false);
    }
    public void PlayCharacterAudio(string name, Action action = null)
    {
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/Piercingeye/sound/" + name, "Piercingeye");
        AudioManager.Instance.PlayerSoundByClip(clip, action);
    }

    public void PlaySceneAudio(string name, Action action = null)
    {
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/Piercingeye/soundeffect/" + name, "Piercingeye");
        AudioManager.Instance.PlayEffectClipSound(clip, false, action);
    }
    /// <summary>
    /// 播放不打断特效音
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void PlayEffectAudio(string name, Action action = null)
    {
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/Piercingeye/soundeffect/" + name, "Piercingeye");
        AudioManagerExtern.Instance.PlaySmallClipSound(clip, action);
    }

    public void PlayYaoqingAudio(string name)
    {
        if (BabyName.c_BabyNameAudioClip == null)
            PlayCharacterAudio(name);
        else
            AudioManager.Instance.PlayerSoundByClip(BabyName.c_BabyNameAudioClip, () =>
            {
                PlayCharacterAudio(name);
            });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            C_UIMgr.Instance.OpenUI("UI_effect_CA", new Vector3(935.0f, 9.7f, 0.0f), 3f); //返回记得关闭                     
        }
        if (Application.isEditor)
            C_TimerMgr.Instance.Update();
        if (fireReadyState != null)
            fireReadyState.Update();
         
    }
    public void NotifyGameOver()
    {
        //显示全屏打开开关
        nextButton.SetActive(true);
    }
    public void NotifyintroduceFinish()
    {
        introduceFinish = true;
    }
    #region#场景返回与资源卸载
    public void RecycleRubbish()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopPlayerSound();
        if (machine == null)
            return;
        machine.UnregisterState("FireReadyState");
        machine.UnregisterState("FireGameState");
        machine.UnregisterState("FireFinishState");
        machine = null;
    }
    void Finish()
    {
        AudioManager.Instance.StopBgMusic();
        if (Xwk != null)
            Xwk.GetComponent<FireXwk>().RecycleRubbish();
        AnimationEventManager.Instance.UnRegisterAllAnimationFun();
        GameObject.Destroy(AnimationEventManager.Instance);
        if (fireReadyState != null)
            fireReadyState.RecycleRubbish();
        if (fireGameState != null)
            fireGameState.RecycleRubbish();
        if (fireFinishState != null)
            fireFinishState.RecycleRubbish();
        RecycleRubbish();
        if (FindObjectOfType<FazhenUI>() != null)
            FindObjectOfType<FazhenUI>().RecycleRubbish();
        StopAllCoroutines();
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
    /// <summary>
    /// 返回主场景
    /// </summary>
    public void ReturnMainScene()
    {
        if (introduceFinish&&!PiercingeyeManager.LockFinish)
        {
            PiercingeyeManager.LockFinish = true;
            WizardData.UnlockWizardItem(LocalData.m_SpiritType);
            if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_BaiYin))//百音精灵强制引导
            {
                DirectorMgr.Instance.DirectStepFinish();
            }
        }
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, LocalData.m_game_time, LocalData.m_game_huoyanjingjing);
        if (DirectorMgr.DirectorAllState)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_huoyanjingjing, "ReturnMainScene");
        }
        else
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_huoyanjingjing, "ReturnMainScene_NewUser");
        }

        Finish();
#if UNITY_Test
        UnityEngine.SceneManagement.SceneManager.LoadScene("MySceneStart");
#elif !UNITY_Test
        YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("Piercingeye", "Main");
#endif
    }
    public void GameOver()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, LocalData.m_game_time, LocalData.m_game_huoyanjingjing);
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_huoyanjingjing, "GameSuccess");
        Finish();
#if UNITY_Test
        UnityEngine.SceneManagement.SceneManager.LoadScene("MySceneStart");
#else
        //百音精灵
        if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_BaiYin))
        {
            if(!PiercingeyeManager.LockFinish)
            {
                PiercingeyeManager.LockFinish = true;
                DirectorMgr.Instance.DirectStepFinish();
            }
            YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("Piercingeye", "Main");
            WizardData.UnlockWizardItem(WizardItemName.Wizard_BaiYin);
 
         }//百变葫芦
        else
        {
            WizardData.UnlockWizardItem(LocalData.m_SpiritType);
            YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("Piercingeye", "Main");
        }       
#endif
    }
    #endregion
    

}

