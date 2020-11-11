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

public class UI_YearMonsterPage : C_BaseUI
{
    private Sequence _Sequence;
    private Button EnterBtn; 
    private bool _Pause;
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

    protected override void onOpenUI(params object[] uiObjParams)
    {
        _SpiritAdModel = (SpiritAdModel)uiObjParams[1];

        _CurrentSpId =  AppInfoData.FetchNewYearChip();
        _Pause = false;
        if (uiObjParams.Length >= 1)
        {
            _Pause = true;
            _CollectUI = (bool)uiObjParams[0];
        }
        if (!AppInfoData.IsCollectUI())
        {
            _CollectUI = false;
        }
//#if UNITY_EDITOR
//        _CurrentSpId = 1;
//        _CollectUI = true;
//#endif
        if (_Sequence != null)
        {
            _Sequence.Kill();
        }
        _Sequence = DOTween.Sequence();
        _Sequence.Append(m_MainLayer.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.17f))
           .Append(m_MainLayer.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.164f))
           .Append(m_MainLayer.DOScale(new Vector3(0.96f, 0.96f, 0.96f), 0.166f))
           .Append(m_MainLayer.DOScale(Vector3.one, 0.166f))
           .OnComplete(ShowCollectUI);
        _BgImg = UICanvas.transform.Find("bg").GetComponent<Image>();
        _BgImg.DOFade(0, 0).OnComplete(() =>
        {
            _BgImg.DOFade(0.7f, 0.25f);
        });
       
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
        _CloseBtn.onClick.RemoveAllListeners();
        _CloseBtn.onClick.AddListener(DoClose);

        _LookForBtn = UICanvas.transform.Find("lookfor").GetComponent<Button>();
        _GoBtn = UICanvas.transform.Find("go").GetComponent<Button>();

        if (AppInfoData.CanEntergame())
        {
            _GoBtn.onClick.RemoveAllListeners();
            _GoBtn.onClick.AddListener(EnterNewYearGame);
            _LookForBtn.gameObject.SetActive(false);
            _GoBtn.gameObject.SetActive(true);
        }
        else
        {
            _LookForBtn.onClick.RemoveAllListeners();
            _LookForBtn.onClick.AddListener(PlayLookFor);
            _LookForBtn.gameObject.SetActive(true);
            _GoBtn.gameObject.SetActive(false);
        }

        if (!_CollectUI && _CurrentSpId < 9)
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
        for (int i = 1; i <= 9; i++)
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
       if (_CollectUI && AppInfoData.IsCollectUI() && AppInfoData.IsComeBackFromNewYearGame())
        {
            if (_CurrentSpId < 9)
            {
                AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_33");
            }
            AppInfoData.ResetComeBackFromNewYearGame();

            _SpIcon.gameObject.SetActive(true);
            int id = _CurrentSpId;
            RectTransform rectTransform = m_MainLayer.Find(id.ToString()).GetComponent<RectTransform>();
            Vector2 targetPos = rectTransform.anchoredPosition;
            Vector2 middlePos = new Vector3(targetPos.x+ rectTransform.sizeDelta.x/2, targetPos.y + rectTransform.sizeDelta.y/2);
            if (_Sequence != null)
            {
                _Sequence.Kill();
            }
            _Sequence = DOTween.Sequence();
            _Sequence.Join(_SpIcon.DOScale(0.5f, 0))
                .AppendInterval(0.001f)
                .Join(_SpIcon.DOScale(1.17f, 0.417f))
                .Join(_SpIcon.DOJumpAnchorPos(middlePos, -300f, 1, 0.5f))
                .AppendInterval(0.5f + 0.334f)
                .Join(_SpIcon.DOScale(1f, 0.2f))
                .Join(_SpIcon.DOAnchorPos(targetPos, 0.2f).OnComplete(() =>
                {
                    m_MainLayer.Find(id.ToString()).GetComponent<Image>().material = normalMat;
                    _SpIcon.gameObject.SetActive(false);
                    _CurrentSpId = 9;
                    if (_CurrentSpId == 9)//出现狮子收集界面
                    {
                        DoClose();

                        //  ShowMonster();
                        //出现收集效果
                        WizardData.AddWizardItem(WizardItemName.Wizard_nianshou);
                        WizardData.CurrentLocationRecommend(WizardItemName.Wizard_nianshou);
                        //打开精灵界面
                      //  C_UIMgr.Instance.OpenUI("UI_CollectSpiritAction", WizardItemName.Wizard_nianshou,_SpiritAdModel);

                    }
                    else
                    {
                        DoClose();
                    }
                }));
        }
       else
        {

        }
    }
    protected void EnterNewYearGame()
    {
        _GoBtn.onClick.RemoveAllListeners();
        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_ENTER_NEWYEAR_GAME);
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
        CloseAllBtn();
        if (_Sequence != null)
        {
            _Sequence.Kill();
        }
        _Sequence = DOTween.Sequence();
        _Sequence.Join(_BgImg.DOFade(0, 0.2f))
          .Join(m_MainLayer.DOScale(Vector3.zero, 0.2f))
          .OnComplete(() => {
              CloseUI();
              C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_ENTER_NEWYEAR_PAGE_BACK);
          });
    }
    protected override void onCloseUI()
    {
        _Pause = false;

        if (_Sequence != null)
        {
            _Sequence.Kill();
        }
        if (_BgImg!=null)
        {
            _BgImg.DOKill();
        }
    }
}
