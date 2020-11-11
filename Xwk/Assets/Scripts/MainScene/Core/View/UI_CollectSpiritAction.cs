using Assets.Scripts.C_Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace YB.XWK.MainScene
{
    public class UI_CollectSpiritAction : C_BaseUI
    {
        private SpiritAdModel _SpiritAdModel;
        private string _CurrentSpiritName;

        private ParticleSystem _BgParticle;
        private ParticleSystem _EndParticle;
        private ShowSpiritState _ShowSpiritState;
        private Sequence _RoleActionSeq;
        private GameObject _SpiritRole;
        private SpiritRoleMgr _SpiritRoleMgr;
        [SerializeField]
        private Transform _RoleParent;//调用方告诉
        [SerializeField]
        private Image _BgImg;
        [SerializeField]
        private Transform _TargetUI;
        private Vector3 _TargetUIPos;
        private C_Event _GameEvent;
        private Sequence _BgActionSeq;
        private System.Action _Callback;
             
        protected override void onOpenUI(params object[] uiObjParams)
        {
            C_UIMgr.c_UICameraHigh.nearClipPlane = -10f;
            _BgImg.gameObject.SetActive(false);
            _CurrentSpiritName = (string)uiObjParams[0];
            _SpiritAdModel = (SpiritAdModel)uiObjParams[1]; 
            _TargetUIPos = _TargetUI.position;
            if (uiObjParams.Length>2)
            {
                _Callback = (System.Action)uiObjParams[2];
            }
            //if (_GameEvent!=null)
            //{
            //    _GameEvent.UnregisterEvent();
            //}
            //_GameEvent = new C_Event();
            //_GameEvent.RegisterEvent(C_EnumEventChannel.Global,"ShowCollectSpirit",(b)=> {
            //    if ((int)b[0] == 0)
            //    {
            //        ShowImgBg();
            //        PlayRole(_CurrentSpiritName);
            //        _GameEvent.UnregisterEvent();
            //    }
            //});

            ShowImgBg();
            InitBgParticle();
            InitEndParticle();
            PlayRole(_CurrentSpiritName);
          //  _GameEvent.UnregisterEvent();
        }
        private void ShowImgBg()
        {
            _BgImg.gameObject.SetActive(true);
            if (_BgActionSeq != null)
            {
                _BgActionSeq.Kill();
                _BgActionSeq = null;
            }
            _BgActionSeq = DOTween.Sequence();
            _BgActionSeq.Append(_BgImg.DOFade(0f, 0.01f))
                .Append(_BgImg.DOFade(0.7f, 0.5f));
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
        private void PlayRole(string spiritName)
        {
            SpiritAdData spiritAdData = _SpiritAdModel.getSpiritData(spiritName);
            Vector3 particlePos; 
            string path = spiritAdData.rolerespath;
            _SpiritRole = GameResMgr.Instance.LoadResource<GameObject>(path, true);
            _SpiritRole.transform.SetParent(_RoleParent);
            particlePos = _SpiritRole.transform.localPosition = spiritAdData.localPosition;
            _SpiritRole.transform.localScale = spiritAdData.localScale;
            _SpiritRole.transform.localRotation = Quaternion.Euler(spiritAdData.localRotation);
            Utility.SetTransformLayer(_SpiritRole.transform, LayerMask.NameToLayer("UI"));

            _BgParticle.transform.SetParent(_RoleParent);
            _BgParticle.gameObject.SetActive(true);
            _BgParticle.transform.localPosition = new Vector3(particlePos.x, particlePos.y + 220f, particlePos.z + 200f);
            _BgParticle.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            _BgParticle.Play();

            _SpiritRoleMgr = _SpiritRole.GetAddComponent<SpiritRoleMgr>();
            _ShowSpiritState = new ShowSpiritState(_SpiritRoleMgr, _SpiritAdModel.getActorAnimancerResConfig(spiritName), () =>
            {
                CollectSpirit();
            });
        }
        private void CollectSpirit()
        {
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
                   
                    _EndParticle.transform.SetParent(_RoleParent);
                    _EndParticle.gameObject.SetActive(true);
                    _EndParticle.transform.localPosition = _SpiritRole.transform.localPosition;
                    _EndParticle.transform.localScale = new Vector3(1f, 1f, 1f);

                    _EndParticle.Play();
                }).AppendInterval(1f).OnComplete(() =>
                {
                  
                    //发消息，进行更新推荐ui
                    UpdateRecommendUI();
                    //关闭界面 
                    C_UIMgr.Instance.CloseUI("UI_CollectSpiritAction");
                    if (_Callback != null)
                    {
                        _Callback();
                    }
                });
        }
        private void UpdateRecommendUI()
        {
            //发消息给maincityup

            //修改为通过获取队列中最后一个精灵的状态，表示他是否已经被查看过的状态来确定是否需要进行提示
            //int enterAppTimes = Mathf.Max(AppInfoData.FetchRecommendID, 1);
            //if (enterAppTimes <= _SpiritAdModel.getSpiritCount())
            //{
            //    _RecommendUI.sprite = GameResMgr.Instance.LoadResource<Sprite>(_SpiritAdModel.getSpiritRecommendUI(enterAppTimes));
            //    _RecommendUI.gameObject.SetActive(true);
            //}
            //else
            //{
            //    _RecommendUI.gameObject.SetActive(false);
            //}
           
        }
        public void OnClose()
        {
            C_UIMgr.c_UICameraHigh.nearClipPlane = 0.03f;
            if (_GameEvent != null)
            {
                _GameEvent.UnregisterEvent();
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
        }
    }
}

