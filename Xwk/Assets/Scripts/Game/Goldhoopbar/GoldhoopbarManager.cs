using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Assets.Scripts.C_Framework;
using YB.XWK.MainScene;
using Xbl;
using XWK.Common.UI_Reward;

public class GoldhoopbarManager : MonoBehaviour, IRubbish
{
    #region#对象
    XwkAnimEvent xwkAnim;
    GoldhoopbarAnimEvent jgbAnim;
    Animator XwkPrefab;
    Animator GoldHoopBarPrefab;
    Animator CameraPrefab;
    GameObject RecognizeEffect;//识别特效
    GameObject Effect_jgb_3_5Obj;
    GameObject effect_jgb_c3a_xh;
    public List<Transform> IdlePosList;
    //金箍棒玩法控制器
    bool haveOperation = false;
    bool GoldStateMoreBigger = false;//越来越大
    C_StateMachine machine;
    GoldBigState big;
    GoldSamllState small;
    GoldGuideState guid;
    Action openAgainEvent = null;
    public static GoldhoopbarManager Instance;
    public static int index = 0;
    int ErrorCount = 0;
    bool isErrorState = false;
    public bool defaultIdle = false;
    public bool moreBigger = true;
    int ScoreCount = 0;
    SceneInfo sceneinfo;
    void Awake()
    {
        Instance = this;
        InitScenes();
        InitMachine();
        if (Application.isEditor)
        {
            GoldStateMoreBigger = true;
            index = 3;
        }
    }
    void Start()
    {
        GotoState("GoldBigState");//默认播放第一阶段
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_jingubang, "EnterGame");
    }
    #endregion   

    #region##初始化   
    void InitScenes()
    {
        RewardUIManager.Instance.SetScoreVisible(false);
        XwkPrefab =GameObjectTool.Instance.InitGameObject(GoldfilePath.Instance.Xwk, GoldfilePath.Instance.XwkControl);
        GoldHoopBarPrefab = GameObjectTool.Instance.InitGameObject(GoldfilePath.Instance.Jgb, GoldfilePath.Instance.JgbControl);
        GoldHoopBarPrefab.transform.position = Vector3.zero;
        jgbAnim = GoldHoopBarPrefab.GetAddComponent<GoldhoopbarAnimEvent>();
        CameraPrefab = GameObjectTool.Instance.InitGameObject(GoldfilePath.Instance.Camera, null, "goldhoopbar",false);
        if (xwkAnim == null)
            xwkAnim = XwkPrefab.GetAddComponent<XwkAnimEvent>();
        xwkAnim.InitIdlePosList(IdlePosList);
        //灯笼特效
        Effect_jgb_3_5Obj = ABResMgr.Instance.LoadResource<GameObject>(GoldfilePath.Instance.Effect_jgb_3_5, "goldhoopbar", true);
        RecognizeEffect = ABResMgr.Instance.LoadResource<GameObject>(GoldfilePath.Instance.RecognizeEffectUrl, "heroeffect", true);
        RecognizeEffect.SetActive(false);
        AudioClip clipef = ABResMgr.Instance.LoadResource<AudioClip>("game/Goldhoopbar/soundeffect/public_xwkbgm_001", "goldhoopbar");
        AudioManager.Instance.PlayBgMusic(clipef);
    }
    void InitMachine()
    {
        sceneinfo = new SceneInfo(XwkPrefab, GoldHoopBarPrefab, CameraPrefab);
        big = new GoldBigState(sceneinfo);
        small = new GoldSamllState(sceneinfo);
        guid = new GoldGuideState(sceneinfo);
        machine = new C_StateMachine();
        machine.RegisterState("GoldBigState", big);
        machine.RegisterState("GoldSamllState", small);
        machine.RegisterState("GoldGuideState", guid);
    }
    #endregion

    #region##状态相关属性
   
    public void GotoState(string name)
    {
        machine.ChangeState(name);
    }
    public bool IsState(string name)
    {
        return machine.TopStateName().Equals(name);
    }
    public Camera getCamera()
    {
        return CameraPrefab.transform.GetChild(0).GetChild(0).GetComponent<Camera>();
    }
    public void RecordError()
    {
        ErrorCount++;
    }
    public void ResetError()
    {
        //按下UI之后重置Error
        ErrorCount = 0;
    }
    public bool IsIdleState()
    {
        if (index >= 1)
        {
            if (ErrorCount >= 2 | isErrorState)
                return true;
        }
        return false;
    }
    public bool IsErrorState()
    {
        return ErrorCount >= 2;
    }
    public void NotiOperation(bool flag)
    {
        haveOperation = flag;
    }
    public bool GetOperation()
    {
        return haveOperation;
    }
    public void PlayCharacterAudio(string name, Action action = null)
    {
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/goldhoopbar/sound/" + name, "goldhoopbar");
        AudioManager.Instance.PlayerSoundByClip(clip, action);
    }
    public void PlaySceneAudio(string name, Action action = null)
    {
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/goldhoopbar/sound/" + name, "goldhoopbar");
        AudioManager.Instance.PlayEffectClipSound(clip, false, action);
    }
    public bool IdleState()
    {
        return ErrorCount >= 2 | defaultIdle;//|machine.TopStateName().Equals("GoldGuideState");
    }
    public void SetIdle()
    {
        defaultIdle = true;
    }
    public void SwitchXwkAnimControl(string path)
    {
        RuntimeAnimatorController anim = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(path, "goldhoopbar", false);
        XwkPrefab.runtimeAnimatorController = anim;
    }
    /// <summary>
    /// 添加棒子特效
    /// </summary>
    public void OpenJgbEffect()
    {
        if (effect_jgb_c3a_xh)
        {
            effect_jgb_c3a_xh.SetActive(true);
        }
        else
        {
            effect_jgb_c3a_xh = ABResMgr.Instance.LoadResource<GameObject>(GoldfilePath.Instance.effect_jgb_c3a_xh, "heroeffect", true);
            effect_jgb_c3a_xh.transform.parent = GoldHoopBarPrefab.transform.GetChild(0).GetChild(0).transform;
            effect_jgb_c3a_xh.transform.localScale = Vector3.one;
            effect_jgb_c3a_xh.transform.localPosition = Vector3.zero;
        }          
    }
    public void CloseJgbEffect()
    {
        if (effect_jgb_c3a_xh)
            effect_jgb_c3a_xh.SetActive(false);
    }
   
    public void JgbPlayIdle()
    {
        jgbAnim.PlayIdle();
    }

    #endregion

    #region##语音识别
    void OpenSpeechRecogNize()
    {
        //if (GoldhoopbarUIControl.Instance != null)
        //    GoldhoopbarUIControl.Instance.ShowSpeechButton(false);
        AudioManager.Instance.StopAllEffect();
        AudioManager.Instance.StopPlayerSound();
        if (xwkAnim != null)
            xwkAnim.PlayListenSingle();
        AudioClip clipef = ABResMgr.Instance.LoadResource<AudioClip>("game/Goldhoopbar/soundeffect/public_xwkyx_014", "goldhoopbar");
        AudioManager.Instance.PlaySound(clipef);
        if (Application.isEditor)
        {
            EditorSpeechCallBack();
        }
        else
        {
            SpeechSystemMgr.Instance.StartRecognizeAudioTecent("大小", SpeechCallBack, RecognizeAudio.ResultType.PickWord);
        }
    }
    /// <summary>
    /// 调用语音识别(通过UI按键）
    /// </summary>
    /// <param name="OpenAgain"></param>
    public void ShowRecognizeByIcon(Action OpenAgain)
    {
        if (IsInvoking("Idle"))
            CancelInvoke("Idle");
        isErrorState = false;
        openAgainEvent = OpenAgain;
        ResetError();
        OpenSpeechRecogNize();
        if (RecognizeEffect)
            RecognizeEffect.SetActive(true);
    }
    /// <summary>
    /// 调用语音识别
    /// </summary>
    /// <param name="flag"></param>
    public void ShowRecognizeEffect(bool flag)
    {
        if (flag)//3S后自动隐藏
        {
            OpenSpeechRecogNize();
        }
        if (RecognizeEffect)
            RecognizeEffect.SetActive(flag);
    }
    bool StillSmall = false;
    void EditorSpeechCallBack()
    {
        string score="大";
        //编辑器随机模拟喊大 喊小
        if (Application.isEditor)
        {
            //if(index==2)
            //{
            //    StillSmall = false;
            //}
            if (index >= 5)
            {
                score = "大";
               // StillSmall = true;
            }
            else
            {
                if (StillSmall)
                {
                    score = "小";
                }
                else
                {
                    float randomKey = UnityEngine.Random.Range(0f, 1f);
                    if (randomKey <= 1f)
                    {
                        score = "大";
                    }
                    else if (randomKey <= 0.45f)
                    {
                        score = "小";
                    }
                    else if (randomKey <= 1f)
                    {
                        score = "无";
                    }
                }
            }            
        }
        SpeechCallBack(score);
    }
    void SpeechCallBack(string score)
    {
        isErrorState = false;
        xwkAnim.PlayListenEnd();
        if (RecognizeEffect)
            RecognizeEffect.SetActive(false);
        switch (score)
        {
            case "大":
                PlayHappy(Bigger);
                break;
            case "小":
                PlayHappy(Smaller);
                break;
            default:
                ErrorDeal();
                break;
        }
    }
    #endregion

    #region#声音播放
    /// <summary>
    /// 变大变小后播放语音,事件在动画中注册(GoldhoopbarAnimEvent.CS)!!!!
    /// </summary>
    public void PlayBiggerOrSmallAudio()
    {
        if (moreBigger)
            PlayBigStateAudio();
        else
            PlaySmallStateAudio();
    }
    void PlayBigStateAudio()
    {
        if (index == 2)
        {
            AudioManager.Instance.PlayerSoundByClip(BabyName.c_BabyNameAudioClip, () =>
            {
                PlayCharacterAudio("common_127");
            });
        }
        else if (index == 3)
        {
            PlayCharacterAudio("common_130");
        }
        else if (index == 4)
        {
            PlayCharacterAudio("common_134");
        }
        else if (index == 5)
        {
            PlayCharacterAudio("common_137");
        }
    }
    void PlaySmallStateAudio()
    {
        int tempIndex = index;
        if (tempIndex == 1)
        {
            AudioManager.Instance.PlayerSoundByClip(BabyName.c_BabyNameAudioClip, () =>
            {
                PlayCharacterAudio("common_145");//嘿，我调下来了
            });
        }
        else if (tempIndex == 2)
        {
            AudioManager.Instance.PlayerSoundByClip(BabyName.c_BabyNameAudioClip, () =>
            {
                PlayCharacterAudio("common_143");

            });
        }
        else if (tempIndex == 3)
        {
            PlayCharacterAudio("common_142");
        }
        else if (tempIndex == 4)
        {
            PlayCharacterAudio("common_143");
        }
        else if (tempIndex == 5)
        {
            PlayCharacterAudio("common_141");
        }
    }
    /// <summary>
    /// 点击小悟空，播放Idle语音
    /// </summary>
    /// <param name="ac"></param>
    public void PlayIdleAudio(Action ac)
    {
        //Debug.Log("NoOperation::" + index);
        if (GoldStateMoreBigger)
            PlayBigNoOperationAudio(ac);
        else
            PlaySmallNoOperationAudio(ac);
    }
    void PlayBigNoOperationAudio(Action ac)
    {
        if (index == 1)
        {
            if (UnityEngine.Random.Range(0, 1f) > 0.5f)
            {
                PlayCharacterAudio("common_125", ac);//好小啊
            }
            else
            {
                PlayCharacterAudio("common_126", ac);
            }
        }
        else if (index == 2)
        {
            //    if (UnityEngine.Random.Range(0, 1f) >= 0.5f)
            //    {
            //        PlayCharacterAudio("common_128",
            //            delegate
            //            {
            //                xwkAnim.EndSpeakDeal(2);//金鸡独立
            //            });
            //}
            //    else
            //    {
            AudioManager.Instance.PlayerSoundByClip(BabyName.c_BabyNameAudioClip, () =>
            {
                PlayCharacterAudio("common_129", ac);
            });
            //}
        }
        else if (index == 3)
        {
            if (UnityEngine.Random.Range(0, 1f) > 0.33f)
            {
                PlayCharacterAudio("common_131", ac);
            }
            else if (UnityEngine.Random.Range(0, 1f) > 0.66f)
            {
                AudioManager.Instance.PlayerSoundByClip(BabyName.c_BabyNameAudioClip, () =>
                {
                    PlayCharacterAudio("common_132", ac);
                });
            }
            else
            {
                PlayCharacterAudio("common_133", ac);
            }
        }
        else if (index == 4)
        {
            if (UnityEngine.Random.Range(0, 1f) > 0.5f)
            {
                PlayCharacterAudio("common_135", ac);//好小啊
            }
            else
            {
                PlayCharacterAudio("common_136", ac);
            }
        }
        else if (index == 5)
        {
            if (UnityEngine.Random.Range(0, 1f) > 0.5f)
            {
                PlayCharacterAudio("common_139", ac);//好小啊
            }
            else
            {
                PlayCharacterAudio("common_140", ac);
            }
        }
    }
    void PlaySmallNoOperationAudio(Action ac)
    {
        if (index == 1)
        {

            if (UnityEngine.Random.Range(0, 1f) >= 0.5f)
            {
                PlayCharacterAudio("common_125", ac);//好小啊
            }
            else
            {
                AudioManager.Instance.PlayerSoundByClip(BabyName.c_BabyNameAudioClip, () =>
                {
                    PlayCharacterAudio("common_126", ac);
                });
            }
        }
        else if (index == 2)
        {
            if (UnityEngine.Random.Range(0, 1f) > 0.33f)
            {
                PlayCharacterAudio("common_128", ac);
            }
            else if (UnityEngine.Random.Range(0, 1f) > 0.66f)
            {
                AudioManager.Instance.PlayerSoundByClip(BabyName.c_BabyNameAudioClip, () =>
                {
                    PlayCharacterAudio("common_129", ac);
                });
            }
            else
            {
                PlayCharacterAudio("common_144", ac);
            }
        }
        else if (index == 3)
        {
            if (UnityEngine.Random.Range(0, 1f) > 0.33f)
            {
                PlayCharacterAudio("common_131", ac);
            }
            else if (UnityEngine.Random.Range(0, 1f) > 0.66f)
            {
                AudioManager.Instance.PlayerSoundByClip(BabyName.c_BabyNameAudioClip, () =>
                {
                    PlayCharacterAudio("common_132", ac);
                });
            }
            else
            {
                PlayCharacterAudio("common_133", ac);
            }
        }
        else if (index == 4)
        {
            if (UnityEngine.Random.Range(0, 1f) > 0.5f)
            {
                PlayCharacterAudio("common_135", ac);//好小啊
            }
            else
            {
                PlayCharacterAudio("common_136", ac);
            }
        }
    }
    #endregion

    void Update()
    {
        if (Application.isEditor)
            C_TimerMgr.Instance.Update();
    }
    void LateUpdate()
    {
        if (big != null)
            big.Update_AddAnimatorEvent();
        if (small != null)
            small.Update_AddAnimatorEvent();
    }

    #region#变大变小处理
    void PlayHappy(Action ac)
    {
        ScoreCount++;
        SwitchXwkAnimControl(GoldfilePath.Instance.XwkControl);
        if (index >= 1 && index <= 5)
        {
            AudioClip clipef = ABResMgr.Instance.LoadResource<AudioClip>("game/Goldhoopbar/soundeffect/public_xwkyx_015", "goldhoopbar");
            AudioManager.Instance.PlayEffectClipSound(clipef);
            xwkAnim.SetIldePosList();
            XwkPrefab.Play("wukong_ty_win03#anim");
            PlayCharacterAudio("common_63", ac);
        }
        else
        {
            XwkPrefab.Play("idle");
            ac();
        }
    }
    void InitState()
    {       
        openAgainEvent = null;
        ResetError();
        defaultIdle = false;
        GoldStateMoreBigger = true;
        ShowRecognizeEffect(false);
        AudioManager.Instance.StopAllEffect();
        isErrorState = false;
    }
    void Bigger()
    {
        InitState();
        moreBigger = true;
        GotoState("GoldBigState");
    }
    void Smaller()
    {
        InitState();
        moreBigger = false;
        GotoState("GoldSamllState");
    }
    public void ErrorDeal()
    {
        RecordError();
        AudioClip clipef = ABResMgr.Instance.LoadResource<AudioClip>("game/Goldhoopbar/soundeffect/public_xwkyx_016", "goldhoopbar");
        AudioManager.Instance.PlaySound(clipef);
        if (ErrorCount >= 2)
        {
            if (openAgainEvent != null)
            {
                openAgainEvent();
                openAgainEvent = null;
            }
            Invoke("Idle", 0.6f);
        }
        else
        {
            if (openAgainEvent != null)
            {
                isErrorState = true;
                openAgainEvent();
                openAgainEvent = null;
                Invoke("Idle", 0.6f);
            }
            else
            {
                guid.Againguide();
            }
        }
    }
    void Idle()
    {
        if (GoldhoopbarUIControl.Instance == null)
            return;
        GoldhoopbarUIControl.Instance.ShowSpeechButton(true);
        if (XwkPrefab.transform.gameObject == null)
            return;
        if (XwkPrefab.transform.gameObject.GetComponent<XwkAnimEvent>() != null)
            XwkPrefab.transform.gameObject.GetComponent<XwkAnimEvent>().PlayIdle();
    }
    #endregion

    #region##垃圾回收与返回主界面
    public void RecycleRubbish()
    {
        CancelInvoke("Idle");
        index = 0;
        AudioManager.Instance.StopPlayerSound();
        machine.UnregisterState("GoldBigState");
        machine.UnregisterState("GoldSamllState");
        machine.UnregisterState("GoldGuideState");
        machine = null;
        if (XwkPrefab)
            Destroy(XwkPrefab.transform.gameObject);
        if (GoldHoopBarPrefab)
            Destroy(GoldHoopBarPrefab.transform.gameObject);
        if (CameraPrefab)
            Destroy(CameraPrefab.transform.gameObject);
        if (RecognizeEffect)
            Destroy(RecognizeEffect.transform.gameObject);
        if (Effect_jgb_3_5Obj)
            Destroy(Effect_jgb_3_5Obj.transform.gameObject);
    }
    /// <summary>
    /// 返回主场景
    /// </summary>
    public void ReturnMainScene()
    {
        if (ScoreCount > 0)
        {
            RewardUIManager.Instance.RegisterStory(5, SourceType.SpriteWindow, 1, ReturnDeal,"jinggubangGame");//30   10   // 31 32  33  34 .....40
            RewardUIManager.Instance.SetSuccess();
        }
        else
        {
            ReturnDeal(true);
        }

    }
    void ReturnDeal(bool flag)
    {
        StopAllCoroutines();
        AnimationEventManager.Instance.UnRegisterAllAnimationFun();
        GameObject.Destroy(AnimationEventManager.Instance);
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, LocalData.m_game_time, LocalData.m_game_jingubang);
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_jingubang, "GameSuccess");
        RecognizeAudio.Instance.StopRecognizeAudio();
        RecognizeAudio.Instance.Clear();//语音识别退出需要调用
        C_UIMgr.Instance.CloseUI("GoldCanvas");
        PlayCharacterAudio("common_119");
        if (big != null)
            big.RecycleRubbish();
        if (small != null)
            small.RecycleRubbish();
        if (guid != null)
            guid.RecycleRubbish();
        RecycleRubbish();
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/Separation/Sound/common_82", "goldhoopbar");
        AudioManager.Instance.PlayerSoundByClip(clip, null);
        Instance = null;
        Resources.UnloadUnusedAssets();
        GC.Collect();

        YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("Goldhoopbar", "Main", () =>
        {
            Destroy(GoldhoopbarManager.Instance);
        });
    }
#endregion
   
}

     

