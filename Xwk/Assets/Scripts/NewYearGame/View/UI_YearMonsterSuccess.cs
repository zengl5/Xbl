using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XWK.Common.UI_Reward;
using YB.YM.Game;
using DG.Tweening;
public class UI_YearMonsterSuccess : C_BaseUI
{
    private Button CoinBox;
    [SerializeField]
    private RectTransform CoinBoxRtf;
    private Image _BgRtf;
    private Sequence _Sequence;
    private bool _Pause;
    private RectTransform _SpIcon;
    protected override void onOpenUI(params object[] uiObjParams)
    {
        UICanvas.worldCamera.depth = 1;
        AudioManager.Instance.PlayerSound("newyeargame/sound/game/common_191");

        _Pause = true;
        CoinBox = CoinBoxRtf.GetComponent<Button>();
        _SpIcon = transform.Find("Canvas/sp").GetComponent<RectTransform>();
        _BgRtf = transform.Find("Canvas/bg").GetComponent<Image>();
        CoinBox.onClick.RemoveAllListeners();
        CoinBox.onClick.AddListener(ClickClose);
        if (_Sequence != null)
        {
            _Sequence.Kill();
        }
        _Sequence = DOTween.Sequence();
        _Sequence.Append(CoinBoxRtf.DOScale(Vector3.zero, 0.00001f))
            .Append(CoinBoxRtf.DOScale(new Vector3(1.12f, 1.12f, 1.12f), 0.21f*0.75f))
            .Append(CoinBoxRtf.DOScale(new Vector3(0.934f, 0.934f, 0.934f), 0.417f * 0.75f))
            .Append(CoinBoxRtf.DOScale(Vector3.one, 0.58f * 0.75f))
            .OnComplete(() => {
                _Pause = false;
                Invoke("CloseBox",5f);
        });
        _BgRtf.DOKill();
        _BgRtf.DOFade(0.7f,0.25f);

        AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_104");

    }
    private void CloseBox()
    {
        PlayReward();
    }
    private void CloseBoxAction()
    {
        if (IsInvoking("CloseBox"))
        {
            CancelInvoke("CloseBox");
        }
    }
    public void ClickClose()
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "newyeargame", "click_coinbox_times");
        PlayReward();
    }
    public void PlayReward()
    {
        if (_Pause)
        {
            return;
        }
        _Pause = true;
        CloseBoxAction();

        if (_Sequence != null)
        {
            _Sequence.Kill();
        }
        CoinBoxRtf.DOScale(new Vector3(0f, 0f, 0f), 0.2f).OnComplete(() =>
        {
            RewardUIManager.Instance.RegisterOfflineBonus(UnityEngine.Random.Range(10,20), new Vector2(Screen.width / 2, Screen.height / 2),ModuleType.SpriteWindow , (b) =>
            {
                int id = AppInfoData.FetchNewYearChip();
                if (id + 1 < 10)
                {
                    //从游戏成功收集碎片
                    AppInfoData.SetComeBackFromNewYearGame();

                    _SpIcon.GetComponent<Image>().sprite = GameResMgr.Instance.LoadResource<Sprite>(string.Concat("newyeargame/texture/bg_ns_pt_", id + 1));
                    _SpIcon.gameObject.SetActive(true);
                    if (_Sequence != null)
                    {
                        _Sequence.Kill();
                    }
                    _Sequence = DOTween.Sequence();

                    _Sequence.Append(_SpIcon.DOScale(Vector3.zero, 0.00001f))
                    .Append(_SpIcon.DOScale(new Vector3(1.12f, 1.12f, 1.12f), 0.2f))
                    .Append(_SpIcon.DOScale(new Vector3(0.95f, 0.95f, 0.95f), 0.2f))
                    .Append(_SpIcon.DOScale(Vector3.one, 0.2f))
                    .AppendInterval(1).OnComplete(() => {
                        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_GAME_SUCCESS_0VER);
                    });
                    AppInfoData.SetNewYearChip(id + 1);
                }
                else
                {
                    C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_GAME_SUCCESS_0VER);
                }
            });
            RewardUIManager.Instance.SetSuccess();
        });
    }
    protected override void onCloseUI()
    {
        CloseBoxAction();
        if (_Sequence != null)
        {
            _Sequence.Kill();
        }
    }
}
