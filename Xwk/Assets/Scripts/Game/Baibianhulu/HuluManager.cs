using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Assets.Scripts.C_Framework;
using YB.XWK.MainScene;
using Xbl;
using LitJson;
using UnityEngine.UI;
using XWK.Common.UI_Reward;

public class HuluManager : MonoBehaviour, IRubbish
{
    #region#对象初始化
    public List<string> DajuList;
    List<GameObject> StarList = new List<GameObject>();
    public static HuluManager Instance;
    public List<HuluCard> SpriteCardList;
    HlGame game;
    HlReady ready;
    HlFinish finish;
    C_StateMachine machine;
    int lightId = 0;
    bool userChooseRight = false;
    GameObject StarEffect = null;
    public bool EditorReady;
    public bool EditorGame;
    public bool EditorFinish;
    public Camera Cam;
    GameObject Xwk;
    GameObject Hulu;
    GameObject HuluRen;
    public List<DOTweenPath> pathTweenList;
    public List<DOTweenPath> WukongpathTweenList;

    GameObject daojuPath;
    GameObject huluUICanvas;
    #endregion


    #region##初始化状态机和场景对象
    void Awake()
    {
        Instance = this;
        AddFeedBack();
        CreatPrefab();
     }
    void Start()
    {
        InitScene();
        InitMachine();
        GotoGameState();
        RewardUIManager.Instance.SetScoreVisible(false);
    }
    void AddFeedBack()
    {
        if (LocalData.m_SpiritGameMode)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeStart, LocalData.m_game_time, LocalData.m_game_baibianhulu);
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_baibianhulu, "EnterGameByUI");
        }
        else
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_baibianhulu, "EnterGameByStory");
        }
    }
    void CreatPrefab()
    {
        Transform wukongParent = GameObject.Find("WukongPath/wukongpath3").transform;
        Xwk = GameObjectTool.Instance.InitPlayer(HlfilePath.Instance.xwk, HlfilePath.Instance.xwk_cc, new Vector3(0, -0.5f, 0), 0.01f*Vector3.one, "baibianhulu",wukongParent);
        Hulu = GameObjectTool.Instance.InitPlayer(HlfilePath.Instance.jl00001_2, HlfilePath.Instance.hulu, new Vector3(0, 0.72f, -3.44f), 0.02f*Vector3.one, "baibianhulu");

        HuluRen = GameObjectTool.Instance.InitPlayer(HlfilePath.Instance.jl00001_1, HlfilePath.Instance.huluren, new Vector3(1.4f, 0f, 0f),0.01f*Vector3.one,"baibianhulu");
        GameObjectTool.Instance.SetSpMainTexture(HuluRen, HlfilePath.Instance.huluTex + (LocalData.RoleLevel + 1).ToString());//设置升级贴图

        Hulu.tag = "meshButton";
        Xwk.transform.localEulerAngles = new Vector3(0, 25, 0);
        HuluRen.transform.eulerAngles = new Vector3(0, -25, 0);
        Xwk.AddComponent<Huluxwk>();
        Xwk.AddComponent<MeshButton>();
        Hulu.AddComponent<Hulu>();
        Hulu.AddComponent<MeshButton>();
        HuluRen.AddComponent<huluren>();
    }
 
    void GotoGameState()
    {
        if (Application.isEditor)
        {
            if (EditorReady)
            {
                GotoState("HlReady");
            }
            else if (EditorGame)
            {
                GotoState("HlGame");
            }
            else if (EditorFinish)
            {
                GotoState("HlFinish");
            }
        }
        else
        {
            GotoState("HlReady");//默认播放第一阶段
        }
    }
    void InitScene()
    {
        daojuPath = GameObject.Find("DaojuPath");
        huluUICanvas = GameObject.Find("HuluUICanvas");
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>(HlfilePath.Instance.public_xwkbgm_005, "baibianhulu");
        AudioManager.Instance.PlayBgMusic(clip, 0.2f);
        if (Hulu)
        {
            if (Hulu.GetComponent<BoxCollider>() == null)
            {
                BoxCollider box = Hulu.AddComponent<BoxCollider>();
                box.center = new Vector3(0, 60, 0);
                box.size = new Vector3(60, 120, 75);
            }
            else
            {
                BoxCollider box = Hulu.GetComponent<BoxCollider>();
                box.center = new Vector3(0, 60, 0);
                box.size = new Vector3(60, 120, 75);
            }
        }
        GameObject star1 = GameObject.Find("HuluUICanvas/Bg/star1");
        GameObject star2 = GameObject.Find("HuluUICanvas/Bg/star2");
        GameObject star3 = GameObject.Find("HuluUICanvas/Bg/star3");
        GameObject star4 = GameObject.Find("HuluUICanvas/Bg/star4");
        GameObject star5 = GameObject.Find("HuluUICanvas/Bg/star5");
        StarList.Add(star1);
        StarList.Add(star2);
        StarList.Add(star3);
        StarList.Add(star4);
        StarList.Add(star5);
        C_MonoSingleton<C_UIMgr>.GetInstance().MandatoryCloseUIAll();
     }
    void InitMachine()
    {
        game = new HlGame(Cam, Xwk, Hulu);
        game.InitPathList(pathTweenList, WukongpathTweenList);
        ready = new HlReady(Xwk, HuluRen, Hulu);
        finish = new HlFinish(Xwk);
        machine = new C_StateMachine();
        machine.RegisterState("HlGame", game);
        machine.RegisterState("HlReady", ready);
        machine.RegisterState("HlFinish", finish);
    }
    public void GotoState(string name)
    {
        if (machine == null)
        {
            machine.RegisterState("HlGame", game);
            machine.RegisterState("HlReady", ready);
            machine.RegisterState("HlFinish", finish);
        }
        machine.ChangeState(name);
    }
    #endregion 


    #region #星星点亮效果
    public int GetLightId()
    {
        return lightId;
    }
    public void LightStar()
    {
        PlayEffectAudio("public_xwkyx_035");
        //点亮特效
        if (StarEffect == null)
        {
            StarEffect = ABResMgr.Instance.LoadResource<GameObject>(SpfilePath.Instance.ui_public_effect_xxcx, ABCommonConfig.EfBundleType, true);
            StarEffect.transform.parent = StarList[lightId].transform;
            StarEffect.transform.localPosition = Vector3.zero;
            StarEffect.transform.localEulerAngles = Vector3.zero;
            StarEffect.transform.localScale = 1f * Vector3.one;
        }
        else
        {
            StarEffect.transform.parent = StarList[lightId].transform;
            StarEffect.transform.localScale = 1f * Vector3.one;
            StarEffect.transform.localPosition = Vector3.zero;
            StarEffect.SetActive(false);
            StarEffect.SetActive(true);
        }

        if (lightId <= StarList.Count - 1)
        {
            StarList[lightId].transform.GetChild(0).gameObject.SetActive(true);
            lightId++;
            if (LocalData.m_SpiritGameMode)
            {
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_baibianhulu, "lightstarByUI:" + lightId);
            }
            else
            {
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_baibianhulu, "lightstarBystory:" + lightId);
            }         
        }
        else
        {
            //Debug.LogError("HuluMnager Start Error lightId");
        }
    }
    #endregion


    public bool GotoFinish()
    {
        if(Application.isEditor)
        {
            if (lightId >= 1)
            {
                GotoState("HlFinish");
                return true;
            }
        }
        else
        {
            if (lightId >= 3)
            {
                GotoState("HlFinish");
                return true;
            }
        }
         
        return false;
    }
    /// <summary>
    /// 去掉字符串中的数字
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string RemoveNumber(string key)
    {
        return System.Text.RegularExpressions.Regex.Replace(key, @"\d", "");
    }


    #region#声音播放
    public void PlayChooseRightSound(Action ac)
    {
        string[] str = new string[] { "bbhlyx_9", "bbhlyx_10", "bbhlyx_11" };
        PlayUserNameAudio(str[UnityEngine.Random.Range(0, str.Length)], ac);
    }
    public void PlayChooseWrongSound(Action ac)
    {
        string[] str = new string[] { "bbhlyx_12", "bbhlyx_13", "bbhlyx_14" };
        PlayUserNameAudio(str[UnityEngine.Random.Range(0, str.Length)], ac);
    }

    
    public void PlayCharacterAudio(string name, Action action = null)
    {
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/BaibianHulu/sound/scenesound/" + name, "baibianhulu");
        AudioManager.Instance.PlayerSoundByClip(clip, action);
    }
    //不打断角色说话声音播放
    public void PlayCharacterAudio_Nointerruption(string name, Action action = null)
    {
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/BaibianHulu/sound/scenesound/" + name, "baibianhulu");
        AudioManagerExtern.Instance.PlaySmallClipSound(clip, action);
    }
    /// <summary>
    /// 播放不打断特效音
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void PlayEffectAudio(string name, Action action = null)
    {
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/BaibianHulu/soundeffect/" + name, "baibianhulu");
        AudioManagerExtern.Instance.PlaySmallClipSound(clip, action);
    }
    public void PlayUserNameAudio(string name, Action action = null)
    {
        if (BabyName.c_BabyNameAudioClip == null)
            PlayCharacterAudio(name, action);
        else
            AudioManager.Instance.PlayerSoundByClip(BabyName.c_BabyNameAudioClip, () =>
            {
                PlayCharacterAudio(name, action);
            });
    }
    Action nowac;
    public void CreateCardAudio(Action ac)
    {
        //我要开始了，你们听好咯，没听清楚的话就点我吧
        PlayCharacterAudio("byjlyx_5", HitmeInfo);
        nowac = ac;
    }
    void HitmeInfo()
    {
        PlayCharacterAudio("byjlyx_6", () =>
        {
            if (nowac != null)
                nowac();
        }
        );
    }
    //再次播放提示音
    public void ReplayNowEffectSound()
    {
        game.RePlayGuideSound();
    }
    public void WarningUserChoose()
    {
        PlayCharacterAudio("byjlyx_7");
    }

    public void NotifyUserChooseRight(bool flag)
    {
        userChooseRight = flag;
    }
    public bool GetUserChooseState()
    {
        return userChooseRight;
    }


    #endregion


    #region#场景返回与数据清理
    public void RecycleRubbish()
    {
        if (huluUICanvas)
            Destroy(huluUICanvas);
        if (daojuPath)
            Destroy(daojuPath);
        if (Xwk)
            Destroy(Xwk);
        if (Hulu)
        {
            if(Hulu.GetComponent<Hulu>()!=null)
            {
                Hulu.GetComponent<Hulu>().RecycleRubbish();
            }
            Destroy(Hulu);
        }
        if (HuluRen)
            Destroy(HuluRen);
        if(ready!=null)
        ready.RecycleRubbish();
        if (game != null)
            game.RecycleRubbish();
        if (finish != null)
            finish.RecycleRubbish();     
        for (int i = 0; i < SpriteCardList.Count; i++)
            if(SpriteCardList[i])
            SpriteCardList[i].RecycleRubbish();
        Destroy(this.gameObject);
    }
    void Finish()
    {     

        ABResMgr.Instance.UnLoadAssetBundle(AssetBundleConfig.Instance.BaibianHuluConfig);        
        if (machine != null)
            machine = null;
        AudioManager.Instance.StopBgMusic();
        AudioManager.Instance.StopPlayerSound();
        AnimationEventManager.Instance.UnRegisterAllAnimationFun();
        GameObject.Destroy(AnimationEventManager.Instance);
        RecognizeAudio.Instance.Clear();//语音识别退出需要调用      
        RecycleRubbish();
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
    /// <summary>
    /// 返回主场景
    /// </summary>
    public void ReturnMainScene()
    {
        Finish();
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_baibianhulu, "ReturnMainScene");
        if (!string.IsNullOrEmpty(YB.XWK.MainScene.LocalData.m_story_moudle))
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, "story_time", YB.XWK.MainScene.LocalData.m_story_moudle);
        }
#if UNITY_Test
        UnityEngine.SceneManagement.SceneManager.LoadScene("MySceneStart");
#endif        
        if (LocalData.m_SpiritGameMode)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, LocalData.m_game_time, LocalData.m_game_baibianhulu);
            YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("BaibianHulu", "Main");
        }
        else
        {
            //YBSceneLoadingMgr.Instance.LoadScene("SpriteWindow");
            //故事模式，玩完游戏还需要跑镜头
            YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("BaibianHulu,bbhl_story", "Main", () =>
            {
                Utility.SetMainScene("wk_scene_01");
                Slate.CutsceneSequencePlayer.StopCurrentCutscene();
            });
        }
    }
    public void GameOver()
    {
        Finish();
        if (LocalData.m_SpiritGameMode)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_baibianhulu, "GameSuccessByUI");
        }
        else
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_baibianhulu, "GameSuccessByStory");
        }
#if UNITY_Test
        UnityEngine.SceneManagement.SceneManager.LoadScene("MySceneStart");
#else
        if (LocalData.m_SpiritGameMode)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, LocalData.m_game_time, LocalData.m_game_baibianhulu);
            YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("BaibianHulu", "Main");
        }
        else
        {
            C_MonoSingleton<PauseUIMoudleMgr>.GetInstance().OpenStoryUI();
            //YBSceneLoadingMgr.Instance.LoadScene("SpriteWindow");
            //故事模式，玩完游戏还需要跑镜头
            YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("BaibianHulu","",() =>
            {
                Utility.SetMainScene("wk_scene_01");
                Slate.CutsceneSequencePlayer.StopCurrentCutscene();
            });
        }

#endif
    }
    #endregion
}

