using Assets.Scripts.C_Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;
using XWK.Common.UI_Reward;
using YB.XWK.MainScene;

public class UI_MainCityUp : C_BaseUI
{
  
    public static Vector3 StarPos = Vector3.zero;

    private C_Event m_PlayerDataChangeEvent;
    private C_Event m_GameEvent ;

    [SerializeField]
    private Button _WizardEnterIcon;
    private GameObject _Hand; 
    [SerializeField]
    private Button _RecommendUI;
    private SpiritAdModel _SpiritAdModel;
    [SerializeField]
    private Transform _TargetPos;
    [SerializeField]
    private Button _FaShuBtn;
    private Transform _FaShuState;
    private Transform _GameBar;
    private RectTransform _BoardUIRtf;
    private Sequence _SequenceIcon;
    private float timeout;
    private bool _ShowIconFlag = false;
    private bool _Pause = false;
    [SerializeField]
    private WizardIconStateMgr _WizardIconStateMgr;
    [SerializeField]
    private UI_NewYearIcon ui_NewYearIcon;

    public Button _EnterGame;


    protected override void onInit()
    {
        _SpiritAdModel = new SpiritAdModel();
        _SpiritAdModel.Load();

        _WizardEnterIcon.onClick.RemoveAllListeners();
        _WizardEnterIcon.onClick.AddListener(EnterWizard);
        _RecommendUI.onClick.RemoveAllListeners();
        _RecommendUI.onClick.AddListener(ShowSpiritRecommend);
        _FaShuBtn.onClick.RemoveAllListeners();
        _FaShuBtn.onClick.AddListener(ShowGameIcon);

        if (GameDataMgr.c_Debug == 0)
        {
            _EnterGame.gameObject.SetActive(true);
            _EnterGame.onClick.RemoveAllListeners();
            _EnterGame.onClick.AddListener(() =>
            {
                if (_Pause)
                {
                    return;
                }
                _Pause = true;
                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_PAUSE_GAME_FALSE);
                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_ENTER_NEWYEAR_GAME);
            });
        }
        else
        {
            _EnterGame.gameObject.SetActive(false);
        }

        _GameBar = _FaShuBtn.transform.parent;
        _BoardUIRtf = _GameBar.transform.Find("board").GetComponent<RectTransform>();

        _ShowIconFlag = false;

        _Pause = false;
    }
    private void CloseBtnListener()
    {
        return;
        _WizardEnterIcon.onClick.RemoveAllListeners();
        _RecommendUI.onClick.RemoveAllListeners();
        _FaShuBtn.onClick.RemoveAllListeners();
    }
    
    public void EnterGame(int type)
    {
        if (_Pause)
        {
            return;
        }

        if (!DailyBounsData.IsUnLock(type))
        {
            CloseBtnListener();

            _Pause = true;

            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 4);

            RegisterRewardEvent(type);
            RewardUIManager.GetInstance().SetFail();
        }
        else
        {
            GameIcon gameIcon;
            if (type == 1)
            {
                if (WizardData.IsUpdateGradate(WizardItemName.Wizard_BaiYin))
                {
                    gameIcon = _GameBar.Find(DailyBounsName.DailyBouns_Game_byjl).GetComponent<GameIcon>();
                    C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "main_mrjl_story_byjl");
                }
                else
                {
                    gameIcon = null;
                }
            }
            else if (type == 2)
            {
                if (WizardData.IsUpdateGradate(WizardItemName.Wizard_Hulu))
                {
                    gameIcon = _GameBar.Find(DailyBounsName.DailyBouns_Game_bbhl).GetComponent<GameIcon>();
                    C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "main_mrjl_story_bbhl");
                }
                else
                {
                    gameIcon = null;
                    //播放提示语音
                }
            }
            else if (type == 3)
            {
                gameIcon = _GameBar.Find(DailyBounsName.DailyBouns_Game_Ggb).GetComponent<GameIcon>();
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "main_mrjl_game_jgb");
            }
            else
            {
                gameIcon = _GameBar.Find(DailyBounsName.DailyBouns_Game_fss).GetComponent<GameIcon>();
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "main_mrjl_game_fss");
            }
            if (gameIcon!=null)
            {
                _Pause = true;
                CloseBtnListener();

                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_ENTER_FREEZE_ROLE);

                RegisterRewardEvent(type);
                gameIcon.HideCoin();
                RewardUIManager.GetInstance().SetSuccess();
            }
            else
            {
                if (!AudioManager.Instance.isPlayingMainSound("xwk_jlej_11"))
                {
                    C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_ENTER_FREEZE_ROLE);
                    //播放提示语音
                    AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_11", false, () => {
                        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_ENTER_RELEASE_ROLE);
                    });
                }
            }
        }

    }
    private void RegisterRewardEvent(int type)
    {
        RewardUIManager.GetInstance().RegisterHomePage(5, SourceType.DailyBonus, 5, (b) => {
            //   if (b)
            {
                if (type == 1)
                {
                    C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_dailybouns, DailyBounsName.DailyBouns_Game_byjl);

                    DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Game_byjl);
                    C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 8);
                }
                else if (type == 2)
                {
                    C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick,  LocalData.m_dailybouns, DailyBounsName.DailyBouns_Game_bbhl);

                    DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Game_bbhl);
                    C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 9);
                }
                else if (type == 3)
                {
                    C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_dailybouns, DailyBounsName.DailyBouns_Game_Ggb);

                    DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Game_Ggb);
                    C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 10);
                }
                else
                {
                    C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_dailybouns, DailyBounsName.DailyBouns_Game_fss);
                    DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Game_fss);
                    C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 11);
                }
            }
        });
    }
    private void DoShowForwardIcon()
    {
        if (_SequenceIcon != null)
        {
            _SequenceIcon.Kill();
        }
        _SequenceIcon = DOTween.Sequence();
        _SequenceIcon.Join(
            _BoardUIRtf.DOSizeDelta(new Vector2(720, 160), 0.25f)
        )
         .Join(DOTween.To(() => timeout, a => timeout = a, 1, 0.25f / 2 - 0.167f/2).OnComplete(() =>
         {
             GameIcon gameIcon = _GameBar.Find(DailyBounsName.DailyBouns_Game_byjl).GetComponent<GameIcon>();
             gameIcon.gameObject.SetActive(true);
             gameIcon.Show(!WizardData.IsUpdateGradate(WizardItemName.Wizard_BaiYin));
         }))
         .Join(DOTween.To(() => timeout, a => timeout = a, 1, 0.25f / 2 +0).OnComplete(() =>
           {
               GameIcon gameIcon =  _GameBar.Find(DailyBounsName.DailyBouns_Game_bbhl).GetComponent<GameIcon>();
               gameIcon.gameObject.SetActive(true);
               gameIcon.Show(!WizardData.IsUpdateGradate(WizardItemName.Wizard_Hulu));
           }))
        .Join(DOTween.To(() => timeout, a => timeout = a, 1, 0.25f / 2 + 0.167f / 2).OnComplete(() =>
          {
              GameIcon gameIcon = _GameBar.Find(DailyBounsName.DailyBouns_Game_Ggb).GetComponent<GameIcon>();
              gameIcon.gameObject.SetActive(true);
              gameIcon.Show();
          }))
        .Join(DOTween.To(() => timeout, a => timeout = a, 1, 0.25f / 2 + 0.167f).OnComplete(() =>
        {
            GameIcon gameIcon = _GameBar.Find(DailyBounsName.DailyBouns_Game_fss).GetComponent<GameIcon>();
            gameIcon.gameObject.SetActive(true);
            gameIcon.Show();
        }))
        .Insert(0.25f,
            _BoardUIRtf.DOSizeDelta(new Vector2(650, 160), 0.125f)
            )
        .Insert(0.25f + 0.125f,
            _BoardUIRtf.DOSizeDelta(new Vector2(670, 160), 0.125f)
        )
       ;
    }
    private void DoShowBackIcon()
    {
        if (_SequenceIcon != null)
        {
            _SequenceIcon.Kill();
        }
        _SequenceIcon = DOTween.Sequence();
        _SequenceIcon
         .Join(DOTween.To(() => timeout, a => timeout = a, 1, 0.001f).OnComplete(() =>
         {
             GameIcon gameIcon = _GameBar.Find(DailyBounsName.DailyBouns_Game_fss).GetComponent<GameIcon>();
             gameIcon.gameObject.SetActive(true);
             gameIcon.Hide();
         }))
         .Join(DOTween.To(() => timeout, a => timeout = a, 1, 0.167f / 2 * 1).OnComplete(() =>
         {
             GameIcon gameIcon = _GameBar.Find(DailyBounsName.DailyBouns_Game_Ggb).GetComponent<GameIcon>();
             gameIcon.gameObject.SetActive(true);
             gameIcon.Hide();
         }))
        .Join(DOTween.To(() => timeout, a => timeout = a, 1,  0.167f / 2 * 2).OnComplete(() =>
        {

            GameIcon gameIcon = _GameBar.Find(DailyBounsName.DailyBouns_Game_bbhl).GetComponent<GameIcon>();
            gameIcon.gameObject.SetActive(true);
            gameIcon.Hide();
        }))
        .Join(DOTween.To(() => timeout, a => timeout = a, 1, 0.167f / 2 * 3).OnComplete(() =>
        {
            GameIcon gameIcon = _GameBar.Find(DailyBounsName.DailyBouns_Game_byjl).GetComponent<GameIcon>();
            gameIcon.gameObject.SetActive(true);
            gameIcon.Hide();
        }))
        .Insert(0, _BoardUIRtf.DOSizeDelta(new Vector2(650, 160), 0.125f))
        .Insert(0.125f, _BoardUIRtf.DOSizeDelta(new Vector2(720, 160), 0.125f))
        .Insert(0.125f * 2, _BoardUIRtf.DOSizeDelta(new Vector2(0, 160), 0.25f))
        ;
    }
    private void ShowGameIcon()
    {
        AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_068");
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "main_fashu_icon");

        _ShowIconFlag = !_ShowIconFlag;
        if (_ShowIconFlag)
        {
            DoShowForwardIcon();
        }
        else
        {
            DoShowBackIcon();
        }
    }
    private void CloseGameIcon()
    {
        if (_ShowIconFlag)
        {
            _ShowIconFlag = false;
            DoShowBackIcon();
        }
    }
    private void OpenSpiritRecommend(bool autoShow)
    {
        if (C_UIMgr.GetInstance().IsOpenedUI("UI_ClockIn"))
        {
            return;
        }
        if (_Pause)
        {
            return;
        }

        if (autoShow)
        {
            if (LocalData.m_FirstOpenChestUI )
            {
                LocalData.m_FirstOpenChestUI = false; 
                CloseGameIcon();
                C_UIMgr.GetInstance().OpenUI("UI_ClockIn", autoShow);
            }
        }
        else
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "UI_ClockIn_Open");

            CloseGameIcon();
            C_UIMgr.GetInstance().OpenUI("UI_ClockIn", autoShow);
        }

    }
    protected void ShowCollectSpirit()
    {
        if (_WizardIconStateMgr!=null)
            _WizardIconStateMgr.ShowStateIcon(_SpiritAdModel);

    }
    protected void EnterWizard()
    {
        if (_Pause)
        {
            return;
        }
        _Pause = true;

        CloseBtnListener();

        _WizardIconStateMgr.HideStateIcon();
        //_WizardEnterIcon.onClick.RemoveListener(EnterWizard);
        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 2);

        AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_068");

    }
    protected void ShowSpiritRecommend()
    {
        if (_Pause)
        {
            return;
        }

        OpenSpiritRecommend(false);
        //ShowHand(false);
        AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_068");
    }

    protected override void onAdaption()
    {
       // StarPos = m_Transform_Star.position;
    }

    protected override void onOpenUI(params object[] uiObjParams)
    {
        RewardUIManager.GetInstance().ChangeModule(ModuleType.HomePage,"main_moudle_reward");
        InitHand();
        _WizardIconStateMgr.InitStateIcon(_SpiritAdModel);
        ui_NewYearIcon.InitNewYearIcon();
        //先收集年兽碎片，在开始收集精灵
        ui_NewYearIcon.AddClickListener(()=> {
            OpenNewYearGame(false);
        });
        if (AppInfoData.IsComeBackFromNewYearGame())
        {
            OpenNewYearGame(true);
        }
        else
        {
            OpenSpiritRecommend(true);
        }
    }
    private void InitHand()
    {
        if (WizardData.IsNewUser())//显示提示收
        {
            if (_Hand==null)
            {
                _Hand = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/public_effect_shoudianji", true);
            }
            Utility.SetTransformLayer(_Hand.transform, LayerMask.NameToLayer("UI"));
            _Hand.transform.SetParent(_WizardEnterIcon.transform);
            _Hand.transform.localPosition = new Vector3(-11.1f, -39.1f, 29.1f);
            _Hand.transform.localScale = new Vector3(500f, 500f, 500f);
            _Hand.gameObject.SetActive(true);
        }
    }
    private void ShowHand(bool visible)
    {
        if (WizardData.IsNewUser())//显示提示收
        {
            if (_Hand!=null)
            {
                _Hand.gameObject.SetActive(visible);
            }
        }
        else
        {
            if (_Hand != null)
                _Hand.gameObject.SetActive(false);
        }
    }

    protected override void onCloseUI()
    {
        if (m_PlayerDataChangeEvent != null)
        {
            m_PlayerDataChangeEvent.UnregisterEvent();
        }
        if (m_GameEvent!=null)
        {
            m_GameEvent.UnregisterEvent();
        }
        DestoryHand();
    }
    protected void DestoryHand()
    {
        if (_Hand != null)
        {
            GameObject.DestroyObject(_Hand);
            _Hand = null;
        }
    }
    protected override void onShowUI()
    {
        if (m_GameEvent!=null)
        {
            m_GameEvent.UnregisterEvent();
        }
        m_GameEvent = new C_Event();
        m_GameEvent.RegisterEvent(C_EnumEventChannel.Global, "MainGameEvent", (object[] result) => {
            if ((int)result[0] == 13)
            {
                _Pause = false;
                ShowHand(true);
                ShowCollectSpirit();
                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_RESUME_GAME);
            }
            else if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_ENTER_HIDE_HAND)
            {
                ShowHand(false);
            }
            //else if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_ENTER_NEWYEAR_PAGE_BACK)
            //{
            //    _Pause = false;
            //    C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_RESUME_GAME_RESUME_FALSE);
            //}
        });
    }
    protected override void onUpdate()
    {
    }


    public void GoParentsCenterVerification()
    {
        if (_Pause)
        {
            return;
        }
        CloseGameIcon();

        DestoryHand();

        if (GameConfig.AutoTest == 0)
            C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_ParentsCenterVerification");
        else
            C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_ParentsCenter");

        //广播进入家长中心消息
        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_PAUSE_GAME_FALSE);
         
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "dianji_fumuzhongxin");
        AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_068");
    }

    public void GoMessage()
    {
        if (_Pause)
        {
            return;
        }
        DestoryHand();
        CloseGameIcon();

        //广播进入家长中心消息
        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_PAUSE_GAME_FALSE);
        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_ParentsCenterVerification", 1);

        AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_068");

    }
    public void OpenNewYearGame(bool autoShow)
    {
        if (_Pause)
        {
            return;
        }
        _Pause = true;
        CloseGameIcon();

        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_PAUSE_GAME_FALSE);

        C_UIMgr.Instance.OpenUI("UI_YearMonsterHomePage", autoShow, _SpiritAdModel,new System.Action(()=> {
            _Pause = false;
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_RESUME_GAME_RESUME_FALSE);
        }));

        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "newyeargame_click_icon");

    }
}
