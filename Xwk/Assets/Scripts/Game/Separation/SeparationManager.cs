using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using UnityEngine.UI;
using System;
using Xbl;
using XWK.Common.UI_Reward;

public interface IRubbish
{
    void RecycleRubbish();
}
public class SeparationManager : MonoBehaviour
{
    public Camera CameraGm;
    bool Haveoperation = false;
    int ErrorCount = 0;//统计每点错3次就播放点错语音
    bool FindXwk = false;
    C_StateMachine machine;
    ReadyState readstate;
    GameState gamestate;
    public FinishState finishstate;
    public static SeparationManager Instance;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        RewardUIManager.Instance.SetScoreVisible(false);
        FindXwk = false;
        readstate = new ReadyState();
        gamestate = new GameState();
        finishstate = new FinishState();
        machine = new C_StateMachine();
        machine.RegisterState("ReadyState", readstate);
        machine.RegisterState("GameState", gamestate);
        machine.RegisterState("FinishState", finishstate);
        WindowSliderControl.Instance.InitCharacter(CameraGm);

        if (Application.isEditor)
            GotoState("GameState");
        else
            GotoState("ReadyState");
        if (CameraGm)
        {
            CameraGm.transform.position = new Vector3(0, 0.68f, 3);
        }
        AudioClip clipef = ABResMgr.Instance.LoadResource<AudioClip>("game/Separation/SoundEffect/public_xwkbgm_001", "separation");
        AudioManager.Instance.PlayBgMusic(clipef);
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, YB.XWK.MainScene.LocalData.m_game_fenshenshu, "EnterGame");
    }

    public void GotoState(string name)
    {
        machine.ChangeState(name);
    }
    //先执行，执行销毁操作
    public void FindSunWukong()
    {
        if (gamestate != null)
            gamestate.FindSunwukong();
        FindXwk = true;
        Invoke("GotoOver", 1.5f);
    }
    void GotoOver()
    {
        GotoState("FinishState");
    }
    //找到真的孙悟空，延迟3S 执行跳转下一个阶段
    public void FindSunWukong(Sunwukong wukong)
    {
        GotoState("FinishState");
    }

    public void FindErrorSunWukong()
    {
        //86-90随机播放
        ErrorCount++;
        if (ErrorCount % 3 == 0)
        {
            int soundIndex = UnityEngine.Random.Range(86, 91);
            AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/Separation/Sound/common_" + soundIndex.ToString(), "separation");
            AudioManager.Instance.PlayerSoundByClip(clip, null);
        }
    }

    void Update()
    {
        if (Application.isEditor)
            C_TimerMgr.Instance.Update();//全局唯一
        if (Input.GetKeyDown(KeyCode.F1))
        {
            InitScene();
        }      
    }
   
    public void InitScene()
    {
        FindXwk = false;
        if (CameraGm)
        {
            CameraGm.transform.position = new Vector3(0, 0.68f, 3);
        }
        WindowSliderControl.Instance.DFrozenCamera();
        readstate.RecycleRubbish();
        gamestate.RecycleRubbish();
        finishstate.RecycleRubbish();
        GotoState("ReadyState");
    }
    public void GameOver()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, YB.XWK.MainScene.LocalData.m_game_time, YB.XWK.MainScene.LocalData.m_game_fenshenshu);
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, YB.XWK.MainScene.LocalData.m_game_fenshenshu, "GameSuccess");
        Finish();
    }
    void Finish()
    {
        GameObject.Destroy(AnimationEventManager.Instance);

        if (readstate != null)
            readstate.RecycleRubbish();
        if (gamestate != null)
            gamestate.RecycleRubbish();
        if (finishstate != null)
            finishstate.RecycleRubbish();
        if (machine != null)
        {
            machine.UnregisterState("ReadyState");
            machine.UnregisterState("GameState");
            machine.UnregisterState("FinishState");
        }
        
        ABResMgr.Instance.UnLoadAssetBundle(AssetBundleConfig.Instance.SeparationConfig);       
        AudioManager.Instance.StopPlayerSound();
        AnimationEventManager.Instance.UnRegisterAllAnimationFun();
        Resources.UnloadUnusedAssets();
        GC.Collect();
#if UNITY_Test
        UnityEngine.SceneManagement.SceneManager.LoadScene("MySceneStart");
#else
        YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("Separation", "Main", () =>
        {
            //DestroyObject(this.gameObject);
        });
#endif
    }
    /// <summary>
    /// 返回主场景
    /// </summary>
    public void ReturnMainScene()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, YB.XWK.MainScene.LocalData.m_game_fenshenshu, "ReturnMainScene");
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, YB.XWK.MainScene.LocalData.m_game_fenshenshu, "close_scene");
        Finish();
    }
    public void OnClickReturnMainScene()
    {
        ReturnMainScene();
    }


}
