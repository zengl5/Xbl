using Assets.Scripts.C_Framework;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace YB.XWK.MainScene
{
    /// <summary>
    /// 1、显示当前推荐的精灵对象，如果当前没有推荐，则将推荐按钮隐藏
    /// 2、每次点击为收集的精灵，进入推荐精灵界面
    /// 3、每次进入显示当前需要收集的精灵
    /// </summary>
    public class UI_SpiritIconAdMgr : C_BaseUI
    {
        private Image _RecommendUI;
        private Vector3 _TargetUIPos;

        [SerializeField]
        private Button _ConfirmBt;

        [SerializeField]
        private Button _IKnowBt;

        [SerializeField]
        private Image _BgImg;

        private SpiritRoleMgr _SpiritRoleMgr;
        private ShowSpiritState _ShowSpiritState;
        private SpiritAdModel _SpiritAdModel;
        private GameObject _SpiritRole;
        private Camera _RoleCamera;
        private string _CurrentSpiritName;

        [SerializeField]
        private Transform _RoleParent;

        private bool _AutoShowAd;
        private Sequence _SelectBtnActionSeq;
        private Sequence _BgActionSeq;
        private Sequence _RoleActionSeq;
        private ParticleSystem _BgParticle;
        private ParticleSystem _EndParticle;
        [SerializeField]
        private Image _PromptImg_Collect;
        [SerializeField]
        private Image _PromptImg_Next;
        private bool _Showing = false;
        protected override void onInit()
        {
            _Showing = false;
        }
        protected override void onOpenUI(params object[] uiObjParams)
        {

            _RecommendUI = ((Button)uiObjParams[0]).GetComponent<Image>();
            _TargetUIPos = ((Transform)uiObjParams[1]).position;
            _SpiritAdModel = (SpiritAdModel)uiObjParams[2];
            _AutoShowAd = (bool)uiObjParams[3];

            _IKnowBt.onClick.RemoveAllListeners();
            _IKnowBt.onClick.AddListener(IKnowConfirm); 
            _ConfirmBt.onClick.RemoveAllListeners();
            _ConfirmBt.onClick.AddListener(CollectSpirit);

            InitBgParticle();
            InitEndParticle();

            DoShowRecommend();
        }

        private void InitEndParticle()
        {
            _EndParticle = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/ui_public_effect_tbcx", true).GetComponent<ParticleSystem>();
            _EndParticle.gameObject.SetActive(false);
        }

        private void InitBgParticle()
        {
            _BgParticle = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/ui_public_effect_tbzs", true).GetComponent<ParticleSystem>();
            _BgParticle.gameObject.SetActive(false);
        }

        private void DoShowRecommend()
        {
            if (RecommendSpiritData.FetchRecommendState())
            {
                ShowRolePage(true);
            }
            else
            {
                if (!_AutoShowAd)
                {
                    //通过按键打开
                    ShowRolePage(false);
                }
                else
                {
                    UpdateRecommendUI();
                    C_UIMgr.GetInstance().CloseUI("UI_SpiritIconAd");
                    
                }
            }


        }
        private void SendCloseRecommendUIEvent()
        {
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 1);
        }
        private void ShowRolePage(bool autoShow = false)
        {
            //首次进入，判断是否精灵获取编号大于1
            int enterAppTimes = AppInfoData.FetchRecommendID;// PlayerPrefs.GetInt("SpiritAD_Number", 0);
            if (enterAppTimes == 0)
            {
                //前3个精灵不获取展示
                // PlayerPrefs.SetInt("SpiritAD_Number", ++enterAppTimes);
                RecommendSpiritData.SetRecommendState();

                AppInfoData.FetchRecommendID = AppInfoData.FetchRecommendID+1;

                UpdateRecommendUI();

                C_UIMgr.GetInstance().CloseUI("UI_SpiritIconAd");
            }
            else
            {
                //先展示昨天获取的精灵，再将获取的编号+1
                if (enterAppTimes <= _SpiritAdModel.getSpiritCount())
                {
                    C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_ENTER_HIDE_HAND);

                    C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 4);
                    if (_BgActionSeq != null)
                    {
                        _BgActionSeq.Kill();
                        _BgActionSeq = null;
                    }
                    _BgActionSeq = DOTween.Sequence();
                    _BgActionSeq.Append(_BgImg.DOFade(0f, 0.01f))
                        .Append(_BgImg.DOFade(0.7f, 0.5f));
                    _BgImg.gameObject.SetActive(true);

                    PlayRole(enterAppTimes);

                    //如果主动点击打开的精灵界面，则不进行累加
                    if (autoShow)
                    {
                        AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_028");

                        _IKnowBt.gameObject.SetActive(false);
                        _ConfirmBt.gameObject.SetActive(true);
                        SelectBtnAction(_ConfirmBt.GetComponent<RectTransform>(), _ConfirmBt.GetComponent<Image>());

                        _PromptImg_Collect.transform.gameObject.SetActive(true);
                        _PromptImg_Next.transform.gameObject.SetActive(false);
                    }
                    else
                    {
                        _IKnowBt.gameObject.SetActive(true);
                        _ConfirmBt.gameObject.SetActive(false);
                        SelectBtnAction(_IKnowBt.GetComponent<RectTransform>(), _IKnowBt.GetComponent<Image>());

                        _PromptImg_Collect.transform.gameObject.SetActive(false);
                        _PromptImg_Next.transform.gameObject.SetActive(true);
                    }
                    ShowRecommendUI(false); 
                }
                else
                {
                    _IKnowBt.gameObject.SetActive(false);
                    _ConfirmBt.gameObject.SetActive(false);
                    ShowRecommendUI(false);
                    C_UIMgr.GetInstance().CloseUI("UI_SpiritIconAd");
                }
            }
        }

        private void SelectBtnAction(RectTransform rectTransform, Image image)
        {
            if (_SelectBtnActionSeq != null)
            {
                _SelectBtnActionSeq.Kill();
            }
            _SelectBtnActionSeq = DOTween.Sequence();
         //   rectTransform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            _SelectBtnActionSeq//.Append(image.DOFade(0, 0))
                .Join(image.DOFade(1, 1 / 8f))
                .Join(rectTransform.DOScale(0.6f, 1 / 8f))
                .AppendInterval(1 / 8f)
                .Append(rectTransform.DOScale(1f, 1 / 8f));
        }

        private void IKnowConfirm()
        {
            _PromptImg_Collect.transform.gameObject.SetActive(false);
            _PromptImg_Next.transform.gameObject.SetActive(false);
            _IKnowBt.onClick.RemoveAllListeners();
            _IKnowBt.gameObject.SetActive(false);

            _ShowSpiritState.Stop();

            _SpiritRole.transform.DOScale(0f, 0.5f).OnComplete(() =>
            {
                ShowRecommendUI(true);
                //关闭界面
                C_UIMgr.GetInstance().CloseUI("UI_SpiritIconAd");
                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 1);
            });
        }

        private void CollectSpirit()
        {
            _PromptImg_Collect.transform.gameObject.SetActive(false);
            _PromptImg_Next.transform.gameObject.SetActive(false);
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "main_collect_jl", string.Concat("main_collect_", _CurrentSpiritName));
            //收集精灵确认
            AppInfoData.FetchRecommendID = AppInfoData.FetchRecommendID + 1;
            RecommendSpiritData.SetRecommendState();
            WizardData.AddWizardItem(_CurrentSpiritName);
            WizardData.CurrentLocationRecommend(_CurrentSpiritName);

            _ConfirmBt.onClick.RemoveAllListeners();
            _ConfirmBt.gameObject.SetActive(false);
            //角色飞道目标位置
            _ShowSpiritState.Stop();
            Vector3 targetPos = new Vector3(_TargetUIPos.x, _TargetUIPos.y, _SpiritRole.transform.position.z);
            if (_RoleActionSeq != null)
            {
                _RoleActionSeq.Kill();
                _RoleActionSeq = null;
            }
            _SpiritRole.transform.DOKill();
            _RoleActionSeq = DOTween.Sequence();

            _RoleActionSeq.Join(_SpiritRole.transform.DOScale(0, 0.5f))
                .Join(_SpiritRole.transform.DOJump(targetPos, (_SpiritRole.transform.position.y + 50), 1, 0.5f))
                .AppendCallback(() =>
            {
                _BgImg.gameObject.SetActive(false);

                _EndParticle.transform.SetParent(_RoleParent);
                _EndParticle.gameObject.SetActive(true);
                _EndParticle.transform.localPosition = _SpiritRole.transform.localPosition;
                _EndParticle.transform.localScale = new Vector3(1f, 1f, 1f);

                _EndParticle.Play();
            }).AppendInterval(1f).OnComplete(() =>
            {
                //更新推荐ui
                UpdateRecommendUI();
                //关闭界面
                C_UIMgr.GetInstance().CloseUI("UI_SpiritIconAd");
                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 13);
            });
        }

        private void UpdateRecommendUI()
        {
            //修改为通过获取队列中最后一个精灵的状态，表示他是否已经被查看过的状态来确定是否需要进行提示
            int enterAppTimes =Mathf.Max(AppInfoData.FetchRecommendID,1); 
            if (enterAppTimes <= _SpiritAdModel.getSpiritCount())
            {
                _RecommendUI.sprite = GameResMgr.Instance.LoadResource<Sprite>(_SpiritAdModel.getSpiritRecommendUI(enterAppTimes));
                _RecommendUI.gameObject.SetActive(true);
            }
            else
            {
                _RecommendUI.gameObject.SetActive(false);
            }
        }

        private void PlayRole(int time)
        {
            Vector3 particlePos;
            _CurrentSpiritName = _SpiritAdModel.getSpiritName(time);
            string path = _SpiritAdModel.getRolePath(_CurrentSpiritName);
            _SpiritRole = GameResMgr.Instance.LoadResource<GameObject>(path, true);
            _SpiritRole.transform.SetParent(_RoleParent);
            particlePos = _SpiritRole.transform.localPosition = _SpiritAdModel.getLocalPosition(time);
            _SpiritRole.transform.localScale = _SpiritAdModel.getLocalScale(time);
            _SpiritRole.transform.localRotation = Quaternion.Euler(_SpiritAdModel.getLocalRotation(time));
            Utility.SetTransformLayer(_SpiritRole.transform, LayerMask.NameToLayer("UI"));

            _BgParticle.transform.SetParent(_RoleParent);
            _BgParticle.gameObject.SetActive(true);
            _BgParticle.transform.localPosition = new Vector3(particlePos.x, particlePos.y + 220f, particlePos.z+200f);
            _BgParticle.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            _BgParticle.Play();

            _SpiritRoleMgr = _SpiritRole.GetAddComponent<SpiritRoleMgr>();
            _ShowSpiritState = new ShowSpiritState(_SpiritRoleMgr, _SpiritAdModel.getActorAnimancerResConfig(_CurrentSpiritName), () =>
            {
            });

            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "collect_jl_moudle", string.Concat("main_collect_", _CurrentSpiritName));
        }

        private void ShowRecommendUI(bool visible)
        {
            _RecommendUI.gameObject.SetActive(visible);
        }

        protected override void onCloseUI()
        {
            if (_BgParticle != null)
            {
                _BgParticle.Stop();
                DestroyObject(_BgParticle.gameObject);
                _BgParticle = null;
            }
            if (_EndParticle != null)
            {
                _EndParticle.Stop();
                DestroyObject(_EndParticle.gameObject);
                _EndParticle = null;
            }
            if (_RoleActionSeq != null)
            {
                _RoleActionSeq.Kill();
                _RoleActionSeq = null;
            }
            if (_BgActionSeq != null)
            {
                _BgActionSeq.Kill();
                _BgActionSeq = null;
            }
            if (_SelectBtnActionSeq != null)
            {
                _SelectBtnActionSeq.Kill();
                _SelectBtnActionSeq = null;
            }
            if (_SpiritRole != null)
            {
                _SpiritRole.transform.DOKill();
                DestroyObject(_SpiritRole);
                _SpiritRole = null;
            }
            if (_RoleCamera != null)
            {
                DestroyObject(_RoleCamera.gameObject);
                _RoleCamera = null;
            }
            _ShowSpiritState = null;
            _SpiritAdModel = null;
            
        }
    }
}