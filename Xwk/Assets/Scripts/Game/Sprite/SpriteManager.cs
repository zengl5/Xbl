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
using UnityEngine.Networking;
using XWK.Common.UI_Reward;

public class SpriteManager : MonoBehaviour, IRubbish
{
    #region#对象初始化
    public List<Texture2D> TexList;
    public List<AudioClip> EffectSoundName;
    public List<AudioClip> CarSoundName;
    public List<Texture2D> CarRealName;
    GameObject Xwk;
    GameObject Jl;
    public List<GameObject> PaoMadengList;//跑马灯
    GameObject TargetUI;
    List<GameObject> StarList = new List<GameObject>();
    public static SpriteManager Instance;
    public List<SpriteCard> SpriteCardList;
    public SpGame spgame;
    SpReady spready;
    SpFinish spfinish;
    C_StateMachine machine;
    bool hitCard = false;
    int lightId = 0;
    bool userChooseRight = false;
    GameObject StarEffect = null;
    public bool EditorReady;
    public bool EditorGame;
    public bool EditorFinish;
    GameObject spCardCanvas;
    GameObject spUICanvas;
    GameObject RightCardEffect;//跑马灯正确效果
    AudioClip Bgclip;
    //static bool director = false;
    #endregion

    #region##数据初始化
    void Awake()
    {
        Instance = this;
        AddFeedBack();
        //if(!director)
        //{
        //    director = true;
        //    DirectorMgr.Instance.DirectStepFinish();
        //}
        if (Application.isEditor)
            lightId = 0;
    } 
    void Start()
    {
        CreatPrefab();
        InitScene();
        InitMachine();
        CloseCard();
        StartCoroutine(ReadJsonData());
        RewardUIManager.Instance.SetScoreVisible(false);
    }
    void AddFeedBack()
    {
        C_MonoSingleton<C_UIMgr>.GetInstance().MandatoryCloseUIAll();
        if (LocalData.m_SpiritGameMode)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeStart, LocalData.m_game_time, LocalData.m_game_baiyingjingling);
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_baiyingjingling, "EnterGameByUI");
        }
        else
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_baiyingjingling, "EnterGameByStory");
        }
    }
    void InitScene()
    {
        spCardCanvas = GameObject.Find("SpCardCanvas");
        spUICanvas = GameObject.Find("SpUICanvas");
        Bgclip = ABResMgr.Instance.LoadResource<AudioClip>("game/Sprite/sound/scenesound/public_xwkbgm_004", "sprite");
        AudioManager.Instance.PlayBgMusic(Bgclip, 0.2f);
        GameObject star1 = GameObject.Find("SpUICanvas/Bg/star1");
        GameObject star2 = GameObject.Find("SpUICanvas/Bg/star2");
        GameObject star3 = GameObject.Find("SpUICanvas/Bg/star3");
        GameObject star4 = GameObject.Find("SpUICanvas/Bg/star4");
        GameObject star5 = GameObject.Find("SpUICanvas/Bg/star5");
        RightCardEffect = GameObject.Find("SpCardCanvas/ui_public_effect_kpzq");
        RightCardEffect.SetActive(false);
        TargetUI = GameObject.Find("SpUICanvas/Target");
        TargetUI.SetActive(false);
        StarList.Add(star1);
        StarList.Add(star2);
        StarList.Add(star3);
        StarList.Add(star4);
        StarList.Add(star5);       
    }
    void CreatPrefab()
    {
        Xwk =GameObjectTool.Instance.InitPlayer(SpfilePath.Instance.xwk, SpfilePath.Instance.spxwk, new Vector3(-0.88f, 0f, 1), 0.01f*Vector3.one, "sprite");
        Jl = GameObjectTool.Instance.InitPlayer(SpfilePath.Instance.jl00002_2, SpfilePath.Instance.spjl, new Vector3(0.85f, 0f, 0.02f), 0.012f*Vector3.one, "Sprite");
        GameObjectTool.Instance.SetSpMainTexture(Jl, SpfilePath.Instance.jlTex + (LocalData.RoleLevel + 1).ToString());//设置升级贴图

        Jl.transform.eulerAngles = new Vector3(0, -20, 0);
    } 
    void InitMachine()
    {
        Xwk.AddComponent<Spxwk>();
        Jl.AddComponent<Spjl>();
        spgame = new SpGame(SpriteCardList, Xwk, Jl, TargetUI);
        spready = new SpReady(Xwk, Jl);
        spfinish = new SpFinish(Xwk);
        machine = new C_StateMachine();
        machine.RegisterState("SpGame", spgame);
        machine.RegisterState("SpReady", spready);
        machine.RegisterState("SpFinish", spfinish);
    }
    public void GotoState(string name)
    {
        if (machine != null)
            machine.ChangeState(name);
       // else
            //Debug.LogError("SpriteManager GotoStateFun machine is null");
    }
    IEnumerator ReadJsonData()
    {
        //读取内置的配置表文件，异步读取方式是因为IO读取路径出现问题
        string path;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            path = Application.dataPath + "/StreamingAssets/Config/Sprit/Sprit.json";//读取数据，转换成数据流
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            path = Application.streamingAssetsPath + "/Config/Sprit/Sprit.json";//读取数据，转换成数据流
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            path = "file://" + Application.streamingAssetsPath + "/Config/Sprit/Sprit.json";//读取数据，转换成数据流
        }
        else
        {
            path = "file://" + Application.streamingAssetsPath + "/Config/Sprit/Sprit.json";//读取数据，转换成数据流
        }
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();
        if (!request.isHttpError)
        {
            //Debug.LogError(request.downloadHandler.text);
            LitJson.JsonData localdata = LitJson.JsonMapper.ToObject(request.downloadHandler.text);
            spgame.InitJsonData(localdata);
        }
        else
        {
            //Debug.LogError(request.error);
        }
        if (Application.isEditor)
        {
            if (EditorReady)
            {
                GotoState("SpReady");
            }
            else if (EditorGame)
            {
                GotoState("SpGame");
            }
            else if (EditorFinish)
            {
                GotoState("SpFinish");
            }
        }
        else
        {
            GotoState("SpReady");//默认播放第一阶段
        }
    }
    #endregion

    #region##场景状态相关逻辑
    //是否禁用点击按钮
    void InteractbleButton(bool flag)
    {
        for (int i = 0; i < SpriteCardList.Count; i++)
            if (!flag)
                SpriteCardList[i].GetComponent<Button>().transition = Selectable.Transition.None;
            else
                SpriteCardList[i].GetComponent<Button>().transition = Selectable.Transition.ColorTint;
    }
    public void ShowSelectionEffect(int id)
    {
        if (id >= PaoMadengList.Count)
            return;
        else
            PaoMadengList[id].SetActive(true);
    }
    void CloseCard()
    {
        for (int i = 0; i < SpriteCardList.Count; i++)
            SpriteCardList[i].transform.gameObject.SetActive(false);
    }
    public void CancelHit()
    {
        hitCard = false;
        InteractbleButton(false);
    }
    public void NotifyHit()
    {
        hitCard = true;
        InteractbleButton(true);
    }
    public bool HitState()
    {
        return hitCard;
    }
    public bool GotoFinish()
    {
        if (lightId >= 5)
        {
            GotoState("SpFinish");
            return true;
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
    #endregion

    #region #跑马灯效果
    public void ShowPaoMaDeng(int rightId, Action ac = null)
    {
        StartCoroutine(MoveQuikly(rightId, ac));
    }
    public void HidePaoMaDeng()
    {
        for (int i = 0; i < PaoMadengList.Count; i++)
            PaoMadengList[i].SetActive(false);
    }
    void ShowRightEffect(int id)
    {
        Vector3[] posList = new Vector3[] { new Vector3(600, 551, -980), new Vector3(0, 551, -980), new Vector3(-600, 551, -980) };
        RightCardEffect.SetActive(true);
        RightCardEffect.transform.localPosition = posList[id];
    }
    WaitForSeconds swait = new WaitForSeconds(0.15f);
    WaitForSeconds wait = new WaitForSeconds(0.4f);
    WaitForSeconds lwait = new WaitForSeconds(1f);
    IEnumerator MoveQuikly(int rightId, Action ac)
    {
        int moveCount = 0;//先快后慢
        Vector3[] posList = new Vector3[] { new Vector3(600, 551, -980), new Vector3(0, 551, -980), new Vector3(-600, 551, -980) };
        int index = 0;
        while (true)
        {
            moveCount++;
            if (moveCount > 12)
            {
                yield return wait;
            }
            else
            {
                yield return swait;
            }
            switch (index)
            {
                case 0:
                    PaoMadengList[2].SetActive(false);
                    break;
                case 1:
                    PaoMadengList[0].SetActive(false);
                    break;
                case 2:
                    PaoMadengList[1].SetActive(false);
                    break;
            }
            PaoMadengList[index].SetActive(true);
            PlayEffectAudio("public_xwkyx_040");
            index++;
            if (index >= posList.Length)
                index = 0;

            if (moveCount > 17)
            {
                for (int i = 0; i < PaoMadengList.Count; i++)
                    PaoMadengList[i].SetActive(false);
                ShowRightEffect(spgame.rightCard.RightCardId);
                PaoMadengList[rightId].SetActive(true);
                yield return lwait;
                HidePaoMaDeng();
                yield return lwait;
                RightCardEffect.SetActive(false);
                if (ac != null)
                    ac();
                PlayEffectAudio("public_xwkyx_058");
                break;
            }
        }
    }
    #endregion 

    #region #星星点亮效果
    public int GetLightId()
    {
        return lightId;
    }
    public void NotifyGameTime()
    {
        lightId++;
    }
    public void LightStar()
    {
        PlayEffectAudio("public_xwkyx_035");
        //点亮特效
        if (StarEffect == null)
        {
            StarEffect = ABResMgr.Instance.LoadResource<GameObject>(SpfilePath.Instance.ui_public_effect_xxcx, ABCommonConfig.EfBundleType, true);
            StarEffect.transform.parent = StarList[lightId - 1].transform;
            StarEffect.transform.localPosition = Vector3.zero;
            StarEffect.transform.localEulerAngles = Vector3.zero;
            StarEffect.transform.localScale = Vector3.one;
        }
        else
        {
            StarEffect.transform.parent = StarList[lightId - 1].transform;
            StarEffect.transform.localPosition = Vector3.zero;
            StarEffect.SetActive(false);
            StarEffect.SetActive(true);
        }


        if (lightId - 1 <= StarList.Count - 1)
        {
            StarList[lightId - 1].transform.GetChild(0).gameObject.SetActive(true);
            if (LocalData.m_SpiritGameMode)
            {
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_baiyingjingling, "lightstarByUI:" + lightId);
            }
            else
            {
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_baiyingjingling, "lightstarBystory:" + lightId);
            }
        }
    }
    #endregion     


    #region#声音播放
    /// <summary>
    /// 播放不打断特效音
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void PlayEffectAudio(string name, Action action = null)
    {
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/Sprite/soundeffect/gameffect/" + name, "sprite");
        AudioManagerExtern.Instance.PlaySmallClipSound(clip, action);
    }
    public void PlaySceneAudio(string name, Action action = null)
    {
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/Sprite/sound/scenesound/" + name, "sprite");
        AudioManager.Instance.PlayEffectClipSound(clip, false, action);
    }
    public void PlayCharacterAudio(string name, Action action = null)
    {
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/Sprite/sound/scenesound/" + name, "sprite");
        AudioManager.Instance.PlayerSoundByClip(clip, action);
    }

    public void PlayCharacterAudio_AbsolutelyAdress(string name, Action action = null)
    {
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>(name, "sprite");
        AudioManager.Instance.PlayerSoundByClip(clip, action);
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
        if (HitState())
            spgame.RePlayGuideSound();
    }
    public void WarningUserChoose()
    {
        PlayCharacterAudio("byjlyx_7", NotifyHit);
    }
    public void PlayAudio_AllRight(int id, Action ac)
    {
        string[] str = new string[] { "byjlyx_9", "byjlyx_10", "byjlyx_11" };
        PlaySceneAudio(str[id], ac);
    }
    public void PlayAudio_UserRight(Action ac)
    {
        string[] str = new string[] { "byjlyx_16", "byjlyx_17", "byjlyx_18" };
        AudioManager.Instance.PlayerSoundByClip(BabyName.c_BabyNameAudioClip, () =>
        {
            PlaySceneAudio(str[UnityEngine.Random.Range(0, str.Length)], ac);
        });
    }
    public void NotifyUserChooseRight(bool flag)
    {
        userChooseRight = flag;
    }
    public bool GetUserChooseState()
    {
        return userChooseRight;
    }
    public void PlayAudio_UserFail(int rdkey, Action ac)
    {
        switch (rdkey)
        {
            case 0:
                PlaySceneAudio("byjlyx_19", ac);
                break;
            case 1:
                AudioManager.Instance.PlayerSoundByClip(BabyName.c_BabyNameAudioClip, () =>
                {
                    PlaySceneAudio("byjlyx_20", ac);
                }); break;
            case 2:
                PlaySceneAudio("byjlyx_21", ac);
                break;
        }
    }
    public void PlayAudio_AllWrong(int id, Action ac)
    {
        string[] str = new string[] { "byjlyx_12", "byjlyx_13", "byjlyx_14", "byjlyx_15" };
        PlaySceneAudio(str[id], ac);
    }
    #endregion


    #region#场景返回与数据清理
    public void RecycleRubbish()
    {
        if (Bgclip != null)
            Bgclip = null;
        if (spready != null)
            spready.RecycleRubbish();
        if (spgame != null)
            spgame.RecycleRubbish();
        if (spfinish != null)
            spfinish.RecycleRubbish();
        if (spCardCanvas)
            Destroy(spCardCanvas);
        if (spUICanvas)
            Destroy(spUICanvas);
        if (Xwk)
        {
            if (Xwk.GetComponent<Spxwk>() != null)
            {
                Xwk.GetComponent<Spxwk>().RecycleRubbish();
            }
            Destroy(Xwk);
        }
        if (Jl)
            Destroy(Jl);     
    }
    /// <summary>
    /// 返回主场景
    /// </summary>
    public void ReturnMainScene()
    {
        if (LocalData.m_SpiritGameMode)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_baiyingjingling, "ReturnMainSceneByUI");
        }
        else
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_baiyingjingling, "ReturnMainSceneByStory");
        }
        if (!string.IsNullOrEmpty(YB.XWK.MainScene.LocalData.m_story_moudle))
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, "story_time", YB.XWK.MainScene.LocalData.m_story_moudle);
        }
        RecycleRubbish();
        CommonDeal();
#if UNITY_Test
        UnityEngine.SceneManagement.SceneManager.LoadScene("MySceneStart");
#endif
        if (LocalData.m_SpiritGameMode)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, LocalData.m_game_time, LocalData.m_game_baiyingjingling);
            YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("Spirit", "Main");
        }
        else
        {
            C_MonoSingleton<PauseUIMoudleMgr>.GetInstance().OpenStoryUI();
            YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("Spirit,byjl_story", "Main", () =>
            {
                Utility.SetMainScene("wk_scene_01");
                Slate.CutsceneSequencePlayer.StopCurrentCutscene();
            });
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, LocalData.m_game_time, LocalData.m_game_baiyingjingling);
        }
    }
    void CommonDeal()
    {        
        ABResMgr.Instance.UnLoadAssetBundle(AssetBundleConfig.Instance.SpriteConfig);
        if (machine != null)
            machine = null;
        AudioManager.Instance.StopBgMusic();
        AudioManager.Instance.StopPlayerSound();
        AnimationEventManager.Instance.UnRegisterAllAnimationFun();
        GameObject.Destroy(AnimationEventManager.Instance);
        RecognizeAudio.Instance.Clear();//语音识别退出需要调用               
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
    public void GameOver()
    {      
        if (LocalData.m_SpiritGameMode)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_baiyingjingling, "GameSuccessByUI");
        }
        else
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_baiyingjingling, "GameSuccessByStory");
        }       
        RecycleRubbish();
        CommonDeal();
        
#if UNITY_Test
        UnityEngine.SceneManagement.SceneManager.LoadScene("MySceneStart");
#else
        if (LocalData.m_SpiritGameMode)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, LocalData.m_game_time, LocalData.m_game_baiyingjingling);
            YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("Spirit", "Main");
        }
        else
        {
            C_MonoSingleton<PauseUIMoudleMgr>.GetInstance().OpenStoryUI();
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, LocalData.m_game_time, LocalData.m_game_baiyingjingling);

            YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("Spirit", "", () =>
            {
                Utility.SetMainScene("wk_scene_01");
                Slate.CutsceneSequencePlayer.StopCurrentCutscene();
            });
        }

#endif
    }
    #endregion
}

