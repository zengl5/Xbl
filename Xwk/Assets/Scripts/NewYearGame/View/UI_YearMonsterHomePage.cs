using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XWK.Common.UI_Reward;
using YB.YM.Game;
using DG.Tweening;
using YB.XWK.MainScene;

public class UI_YearMonsterHomePage : C_BaseUI
{
    private Sequence _StartSequence;
    private Sequence _CollectUISequence;
    private Sequence _BackSequence;
    private Button EnterBtn; 
    private bool _Pause;//false 表示不能点击
    private Image _BgImg;
    protected bool _CollectUI = false;
    protected RectTransform _SpIcon;
    protected int _CurrentSpId;
    protected Material grayMat;
    protected Material normalMat;
    protected Button _CloseBtn;
    protected Button _GoBtn;
    protected Button _LookForBtn;
    protected SpiritAdModel _SpiritAdModel;
    protected int _TotalNewYearClip;
    protected Action OnFinishBack;
    protected bool _AutoCollect;
    protected override void onOpenUI(params object[] uiObjParams)
    {
        //  C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "newyeargame","homepage_click_icon");
        AudioManager.Instance.PlayEffectAutoClose("game/redbomb/soundeffect/public_xwkyx_094");

        _Pause = true;
        _TotalNewYearClip = AppInfoData.TotalNewYearClipSum();
        _SpiritAdModel = (SpiritAdModel)uiObjParams[1];
        OnFinishBack = (Action)uiObjParams[2];
        _CurrentSpId =  AppInfoData.FetchNewYearChip();


        if (uiObjParams.Length >= 1)
        {
            _CollectUI = (bool)uiObjParams[0];
        }
        if (!AppInfoData.IsCollectUI())
        {
            _CollectUI = false;
        }
        if (AppInfoData.AutoCollectNewYearClip())
        {
            _CollectUI = true;
            _AutoCollect = true;
            _CurrentSpId = _TotalNewYearClip;
        }
        else
        {
            _AutoCollect = false;
        }

        _SpIcon = UICanvas.transform.Find("MainLayer/sp").GetComponent<RectTransform>();
        _SpIcon.gameObject.SetActive(false);
        ShowSp();
        if (_CurrentSpId > 0 && _CollectUI)
        {
            _SpIcon.anchoredPosition = new Vector2(1045f, 296f);
            _SpIcon.gameObject.SetActive(false);
            _SpIcon.GetComponent<Image>().sprite = GameResMgr.Instance.LoadResource<Sprite>(string.Concat("newyeargame/texture/bg_ns_pt_", _CurrentSpId.ToString()));
        }

        _CloseBtn = m_MainLayer.Find("close").GetComponent<Button>();
        _CloseBtn.onClick.AddListener(DoClose);

        _LookForBtn = UICanvas.transform.Find("lookfor").GetComponent<Button>();
        _GoBtn = UICanvas.transform.Find("go").GetComponent<Button>();

        if (AppInfoData.CanEntergame())
        {
            _GoBtn.onClick.AddListener(EnterNewYearGame);
            _LookForBtn.gameObject.SetActive(false);
            _GoBtn.gameObject.SetActive(true);
        }
        else
        {
            _LookForBtn.onClick.AddListener(PlayLookFor);
            _LookForBtn.gameObject.SetActive(true);
            _GoBtn.gameObject.SetActive(false);
        }

        if (!_CollectUI && _CurrentSpId < _TotalNewYearClip)
        {
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_34");
            }
            else
            {
                AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_35");
            }
        }
        if (_StartSequence != null)
        {
            _StartSequence.Kill();
        }
        _StartSequence = DOTween.Sequence();
        _StartSequence.Append(m_MainLayer.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.17f))
           .Append(m_MainLayer.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.164f))
           .Append(m_MainLayer.DOScale(new Vector3(0.96f, 0.96f, 0.96f), 0.166f))
           .Append(m_MainLayer.DOScale(Vector3.one, 0.166f))
           .OnComplete(ShowCollectUI);

        _BgImg.DOKill();
        _BgImg = UICanvas.transform.Find("bg").GetComponent<Image>();
        _BgImg.DOFade(0, 0).OnComplete(() =>
        {
            _BgImg.DOFade(0.7f, 0.25f);
        });


        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeStart, "newyeargame_home_page_time");

    }
    void PlayLookFor()
    {
        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_18");
        }
        else
        {
            AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_19");
        }
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "newyeargame", "homepage_look_btn");

    }
    protected void ShowSp()
    {
        grayMat = GameResMgr.Instance.LoadResource<Material>("newyeargame/material/grayUI");
        normalMat = GameResMgr.Instance.LoadResource<Material>("newyeargame/material/normalUI");
        int showId = 0;
        if (_CollectUI)
        {
            showId = _CurrentSpId - 1;
        }
        else
        {
            showId = _CurrentSpId;
        }
        for (int i = 1; i <= _TotalNewYearClip; i++)
        {
            if (i <= showId)
            {
                m_MainLayer.Find(i.ToString()).GetComponent<Image>().material = normalMat;
            }
            else
            {
                m_MainLayer.Find(i.ToString()).GetComponent<Image>().material = grayMat;
            }
        }
    }
    protected void ShowCollectUI()
    {
        if ((_CollectUI && AppInfoData.IsComeBackFromNewYearGame())
            || _AutoCollect)
        {
            AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_103");

            if (_CurrentSpId < _TotalNewYearClip)
            {
                AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_33");
            }
            AppInfoData.ResetComeBackFromNewYearGame();

            _SpIcon.gameObject.SetActive(true);
            RectTransform rectTransform = m_MainLayer.Find(_CurrentSpId.ToString()).GetComponent<RectTransform>();
            Vector2 targetPos = rectTransform.anchoredPosition;
            Vector2 middlePos = new Vector3(targetPos.x + rectTransform.sizeDelta.x / 2, targetPos.y + rectTransform.sizeDelta.y / 2);
            if (_CollectUISequence != null)
            {
                _CollectUISequence.Kill();
                _CollectUISequence = null;
            }
            _CollectUISequence = DOTween.Sequence();
            _CollectUISequence.Join(_SpIcon.DOScale(0.5f, 0))
                .AppendInterval(0.001f)
                .Join(_SpIcon.DOScale(1.17f, 0.417f))
                .Join(_SpIcon.DOJumpAnchorPos(middlePos, -300f, 1, 0.5f))
                .AppendInterval(0.5f + 0.334f)
                .Join(_SpIcon.DOScale(1f, 0.2f))
                .Join(_SpIcon.DOAnchorPos(targetPos, 0.2f).OnComplete(() =>
                {
                    m_MainLayer.Find(_CurrentSpId.ToString()).GetComponent<Image>().material = normalMat;
                    _SpIcon.gameObject.SetActive(false);
                    
                    if (_CurrentSpId == _TotalNewYearClip  )//出现狮子收集界面
                    {
                        AppInfoData.SetNewYearChip(_CurrentSpId + 1);
                        //出现收集效果
                        WizardData.AddWizardItem(WizardItemName.Wizard_nianshou);
                        WizardData.CurrentLocationRecommend(WizardItemName.Wizard_nianshou);
                        //打开精灵界面
                        C_UIMgr.Instance.OpenUI("UI_CollectSpiritAction", WizardItemName.Wizard_nianshou, _SpiritAdModel, new System.Action(() =>
                        {
                            _Pause = false;

                           // DoClose();
                        }));

                        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "newyeargame", "homepage_collect_ui");

                    }
                    else
                    {
                        //DoClose();
                        _Pause = false;
                    }
                }));
        }
       else
        {
            _Pause = false;
        }

    }
    protected void EnterNewYearGame()
    {
        _GoBtn.onClick.RemoveAllListeners();
        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_ENTER_NEWYEAR_GAME);
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "newyeargame", "homepage_go_btn");

    }
    protected void ShowMonster()
    {
        _BgImg.gameObject.SetActive(false);
        CloseAllBtn();
        m_MainLayer.gameObject.SetActive(false);
    }
    protected void CloseAllBtn()
    {
        _BgImg = UICanvas.transform.Find("bg").GetComponent<Image>();
        _GoBtn.onClick.RemoveAllListeners();
        _LookForBtn.onClick.RemoveAllListeners();
        _LookForBtn.gameObject.SetActive(false);
        _GoBtn.gameObject.SetActive(false);
        _CloseBtn.onClick.RemoveAllListeners();
    }
    protected void DoClose()
    {
        if (_Pause)
        {
            return;
        }
        CloseAllBtn();
        if (_BackSequence != null)
        {
            _BackSequence.Kill(true);
        }
        _BackSequence = DOTween.Sequence();
        _BackSequence.Join(_BgImg.DOFade(0, 0.25f))
          .Join(m_MainLayer.DOScale(1.08f, 0.125f))
          .Append(m_MainLayer.DOScale(0f, 0.125f))
          .OnComplete(() =>
          {
              CloseUI();
              C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_ENTER_NEWYEAR_PAGE_BACK);
          });
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "newyeargame", "homepage_close_btn");

    }
    protected override void onCloseUI()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, "newyeargame_home_page_time");

        if (_CollectUISequence != null)
        {
            _CollectUISequence.Kill();
            _CollectUISequence = null;
        }
        if (_StartSequence!=null)
        {
            _StartSequence.Kill();
            _StartSequence = null;
        }
        if (_BackSequence != null)
        {
            _BackSequence.Kill(true);
            _BackSequence = null;

        }
        if (m_MainLayer != null)
        {
            m_MainLayer.DOKill();
        }
        if (_BgImg!=null)
        {
            _BgImg.DOKill();
        }
        if (_GoBtn!=null)
        {
            _GoBtn.onClick.RemoveAllListeners();
        }
        if (_LookForBtn != null)
        {
            _LookForBtn.onClick.RemoveAllListeners();
        }
        if (_CloseBtn != null)
        {
            _CloseBtn.onClick.RemoveAllListeners();
        }

        if (OnFinishBack!=null)
        {
            OnFinishBack();
            OnFinishBack = null;
        }
    }
}
