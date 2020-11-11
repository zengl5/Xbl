using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.YM.Game;
using YB.AnimCallbacks;
using DG.Tweening;
using System;

namespace YB.XWK.MainScene {
    public class MaroonMgr : RoleMgrBase
    {
        private Animator _Animator;
        /*big maroon*/
        private GameObject _MaroonBig;
        private GameObject _MaroonBigFlower;
        private GameObject _MaroonBigFire;
        private BoxCollider _MaroonBigCollider;
        private string _BigMaroonName = "big_public_model_dj00053(Clone)";
        private Animator _BigAniamtor;
        private Sequence _SequenceTime;
        private Sequence _SequenceBig;

        /*small maroon*/
        private GameObject _MaroonSmall; 
        private GameObject _MaroonSmallFire;
        private BoxCollider _MaroonSmallCollider;
        private string _SmallMaroonName = "small_public_model_dj00054(Clone)";
        private Animator _SmallAniamtor;
        private Sequence _SequenceSmallTime;

        private Sequence _Sequence;
        
        public MaroonMgr(Camera gameCamra) : base(gameCamra)
        {
            _PauseGame = false;
            OnInit();
            if (_Sequence!=null)
            {
                _Sequence.Kill();
            }
            _Sequence = DOTween.Sequence();
            InitRole();
            HideMaroon();

        }
        public override void OnUpdate()
        {
            if (_PauseGame)
            {
                return;
            }
            base.OnUpdate();
        }
        public override void OnInit()
        {

        }
        protected override void InitRole()
        {
            if (_MaroonSmall == null)
            {
                _MaroonSmall = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/small_public_model_dj00054", true);
                _MaroonSmall.transform.position = new Vector3(-1, 0f, -2.62f);
                _MaroonSmall.transform.localScale = Vector3.one;
                _MaroonSmallCollider = _MaroonSmall.GetAddComponent<BoxCollider>();
                _MaroonSmallCollider.center = new Vector3(0f, 0.33f, 0f);
                _MaroonSmallCollider.size = new Vector3(0.52f,0.64f,1f);
                _MaroonSmallCollider.isTrigger = true;

                _SmallAniamtor = _MaroonSmall.transform.Find("public_model_dj00054#mesh").GetComponent<Animator>();
                _MaroonSmallFire = _MaroonSmall.transform.Find("public_model_dj00054#mesh/Dummy001/public_effect_xbianpao").gameObject;
                _MaroonSmallFire.gameObject.SetActive(false);
            }
            else
            {
                _MaroonSmall.gameObject.SetActive(true);
                _MaroonSmall.transform.position = new Vector3(-1f, 0f, -2.62f);
            }
            if (_MaroonBig == null)
            {
                _MaroonBig = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/big_public_model_dj00053", true);
                _MaroonBig.transform.position = new Vector3(2f, 0f, -4.3f);
                _MaroonBig.transform.localScale = Vector3.one;
                _MaroonBigCollider = _MaroonBig.GetAddComponent<BoxCollider>();
                _MaroonBigCollider.center = new Vector3(0f, 0.38f, 0f);
                _MaroonBigCollider.size = new Vector3(1.4f,0.72f,1f);
                _MaroonBigCollider.isTrigger = true;

                _BigAniamtor = _MaroonBig.transform.Find("public_model_dj00053#mesh").GetComponent<Animator>();
                _MaroonBigFlower = _MaroonBig.transform.Find("public_model_dj00053#mesh/effect_yanhuabao").gameObject;
                _MaroonBigFlower.gameObject.SetActive(false);
                _MaroonBigFire = _MaroonBig.transform.Find("public_model_dj00053#mesh/Dummy001/fire").gameObject;
                _MaroonBigFire.gameObject.SetActive(false);
            }
            else
            {
                _MaroonBig.gameObject.SetActive(true);
                _MaroonBig.transform.position = new Vector3(2f, 0f, -4.3f);
            }
        }
        public override void TouchEvent(GameObject obj, Camera camera, Vector3 pos)
        {
            if (obj == null)
            {
                return;
            }
            if (obj.name.Equals(_SmallMaroonName))
            {
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "main_event", "smallmaroon");

                AudioManager.Instance.PlayEffectAutoClose("newyeargame/sound_effect/public_xwkyx_097",true);

                _MaroonSmallCollider.enabled = false;
                _MaroonSmallFire.gameObject.SetActive(true);
                _SmallAniamtor.SetTrigger("dj00054#anim");
                _SmallAniamtor.AddClipEndCallback("dj00054#anim", SmallAnimatorCallBack);

                if (_SequenceSmallTime != null)
                {
                    _SequenceSmallTime.Kill();
                }
                _SequenceSmallTime = DOTween.Sequence();
                _SequenceSmallTime.AppendInterval(6.0f).AppendCallback(() => {
                    _SmallAniamtor.SetTrigger("dj00054_stand01#anim");

                    _PauseGame = false;
                    _MaroonSmallCollider.enabled = true;
                    _MaroonSmallFire.gameObject.SetActive(false);
                    _MaroonSmall.gameObject.SetActive(false);
                    GenerateMaroon(0);
                    CloseSmallSound();
                });
            }
            else if (obj.name.Equals(_BigMaroonName))
            {
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "main_event", "bigmaroon");

                AudioManager.Instance.PlayEffectAutoClose("newyeargame/sound_effect/public_xwkyx_098");
                _MaroonBigCollider.enabled = false;
                _MaroonBigFlower.gameObject.SetActive(true);
                _MaroonBigFire.gameObject.SetActive(true);
                _BigAniamtor.SetTrigger("dj00053#anim");
                _BigAniamtor.AddClipEndCallback("dj00053#anim", BigAnimatorCallBack);
                 
                if (_SequenceTime!=null)
                {
                    _SequenceTime.Kill();
                }
                _SequenceTime = DOTween.Sequence();
                _SequenceTime.AppendInterval(8.50f).AppendCallback(()=> {
                    _SmallAniamtor.SetTrigger("dj00054_stand01#anim");

                    _PauseGame = false;
                    _MaroonBigCollider.enabled = true;
                    _MaroonBigFlower.gameObject.SetActive(false);
                    _MaroonBigFire.gameObject.SetActive(false);
                    
                    _MaroonBig.gameObject.SetActive(false);
                    GenerateMaroon(1);
                });
            }
        }
       void CloseSmallSound()
        {
            AudioManager.Instance.StopEffectByKey("newyeargame/sound_effect/public_xwkyx_097");
        }
        void BigAnimatorCallBack()
        {
            _PauseGame = false;
            _BigAniamtor.RemoveClipEndCallback("dj00053#anim", BigAnimatorCallBack);
        }
        void SmallAnimatorCallBack()
        {
            CloseSmallSound();
            _PauseGame = false;
            _BigAniamtor.RemoveClipEndCallback("dj00054#anim", SmallAnimatorCallBack);
        }
        void ShowMaroon(int type)
        {
            if (type == 0)
            {
                if (_MaroonSmall != null)
                {
                    _MaroonSmall.gameObject.SetActive(true);
                    _MaroonSmall.transform.position = new Vector3((float)UnityEngine.Random.Range(-8, 3), 0f, -2.62f);

                }
            }
            if (type == 1)
            {
                if (_MaroonBig != null)
                {
                    _MaroonBig.gameObject.SetActive(true);
                    _MaroonBig.transform.position = new Vector3((-1) * (float)UnityEngine.Random.Range(-7, 8), 0f, -4.3f);
                }
            }
            if (type == 2)
            {
                if (_MaroonBig != null)
                {
                    _MaroonBig.gameObject.SetActive(true);
                }
                if (_MaroonSmall != null)
                {
                    _MaroonSmall.gameObject.SetActive(true);
                }
            }

        }
        void HideMaroon()
        {
            if (_MaroonSmall != null)
            {
                _MaroonSmall.gameObject.SetActive(false);
            }
            if (_MaroonBig != null)
            {
                _MaroonBig.gameObject.SetActive(false);
            }
            CloseSmallSound();
        }
        public void Pause()
        {
            _PauseGame = true;
        }
        public void Resume()
        {
            _PauseGame = false;
            if (_MaroonSmall != null)
            {
                _MaroonSmall.gameObject.SetActive(true);
            }
            if (_MaroonBig != null)
            {
                _MaroonBig.gameObject.SetActive(true);
            } 
        }
        public override void Stop()
        {
            CloseSmallSound();
            if (_SequenceSmallTime!=null)
            {
                _SequenceSmallTime.Kill();
            }
            if (_Sequence!=null)
            {
                _Sequence.Kill();
            }
            if (_MaroonSmall != null)
            {
                _MaroonSmall.gameObject.SetActive(true);
                GameObject.DestroyObject(_MaroonSmall);
                _MaroonSmall = null;
            }
            if (_MaroonBig != null)
            {
                _MaroonBig.gameObject.SetActive(true);
                GameObject.DestroyObject(_MaroonBig);
                _MaroonBig = null;
            }
        }
        void GenerateMaroon(int type)
        {
            if (type == 0)
            {
                if (_Sequence != null)
                {
                    _Sequence.Kill();
                }
                _Sequence = DOTween.Sequence();
                _Sequence.AppendInterval(3.0f).AppendCallback(() => {
                    ShowMaroon(type);
                });
            }
            else
            {
                if (_SequenceBig != null)
                {
                    _SequenceBig.Kill();
                }
                _SequenceBig = DOTween.Sequence();
                _SequenceBig.AppendInterval(3.0f).AppendCallback(() => {
                    ShowMaroon(type);
                });
            }
        }
        //开始缠声烟花
        public override void  EnterIdle()
        {
            _PauseGame = false;
            ShowMaroon(2); 
        }
    }
}


