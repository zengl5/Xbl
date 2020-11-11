using Assets.Scripts.C_Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene {

    public class CloudRainState : ActorState
    {
        enum cloudstate {
            state_null,
            state_move,
            state_black,
            state_black_over,
            state_rain,
            state_over,
        }

        private bool startRain = false;
        private bool stop = false;
        private float currentTime = 0f;
        private cloudstate _cloudstate;
        private ParticleSystem _CloudBlack;
        private ParticleSystem _Rain;
        private GameObject _CloudBlackObj;
        private GameObject _LightingObj;
        private string _TimeMark= "CloudRainState";
        private string _RainTimeMark= "Rain_CloudRainState";
        private Texture _BlackTexture;
        private Texture _OrignalTexture;
        private Material _CloudMat;
        private GameObject _RainEffect;
        private string _ThunderName = "public/sound_effect/public_xwkyx_055.ogg";

        public CloudRainState(IActor actorAimMgr) : base(null)
        {
            HasInteractiveState = false;
            this.ActorStateName = "rainstate";
            OnInit(actorAimMgr, ActorStateName);
            Play(this);
            startRain = false;
            _CloudBlackObj = GameResMgr.Instance.LoadResource<GameObject>("public/Hero_Effect/prefab/public_effect_sdhyun.prefab", true);
            _CloudBlackObj.transform.position = Actor.transform.position;
            _CloudBlackObj.gameObject.SetActive(false);
            _LightingObj = GameResMgr.Instance.LoadResource<GameObject>("public/Hero_Effect/prefab/public_effect_sdcs.prefab", true);
            _LightingObj.transform.position = Actor.transform.position;
            _LightingObj.gameObject.SetActive(false);
            _BlackTexture = GameResMgr.Instance.LoadResource<Texture>("public/mesh/jdy02/jdy02_b_tx_c.png");
            _OrignalTexture = GameResMgr.Instance.LoadResource<Texture>("public/mesh/jdy02/jdy_2_tx_c.png");
            _CloudMat =  Actor.Find("ty_jdy02").GetComponent<Renderer>().sharedMaterial;
            _RainEffect = Actor.Find("Point002/Point001/bone01/bone02/bone03/bone04/public_effect_jdy_tw").gameObject;
            _RainEffect.gameObject.SetActive(false);
            if (Actor.transform.position.y != 1.97f/* * Screen.height / 1080f*/)
            {
                Vector3 pos = Actor.transform.position;
                Vector3 target = new Vector3(pos.x, 1.97f /** Screen.height / 1080f*/, pos.z);
                float time = Vector3.Distance(pos, target) / 5f;
                Actor.transform.DOMove(target, time).OnComplete(DoStart);
                _cloudstate = cloudstate.state_move;
            }
            else
            {
                DoStart();
            }
        }
        public void DoStart()
        {
            AudioManager.Instance.PlayEffectSound(_ThunderName, true);
            _CloudMat.SetTexture("_MainTex",_BlackTexture);
            _LightingObj.transform.position = Actor.transform.position;
            _CloudBlackObj.transform.position = Actor.transform.position;
            _CloudBlackObj.gameObject.SetActive(true);

            //播放黑云效果
            _cloudstate = cloudstate.state_black;
            C_TimerMgr.Instance.AddTimer(1, () => {
                currentTime = 0f;
                //等待点击下雨
                _cloudstate = cloudstate.state_black_over;

                DoTwiceTouch();

            }, _TimeMark);
        }
        public override void OnUpdate()
        {
            if (_cloudstate == cloudstate.state_black_over)
            {
                currentTime += Time.deltaTime;
                if (currentTime > 10f)
                {
                    Stop();
                    m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_IDLE);
                }
            }
            //if (_cloudstate ==cloudstate.state_rain)
            //{
            //     m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_FLASH);
            //}
        }
        public override void Stop()
        {
            _RainEffect.gameObject.SetActive(true);
            AudioManager.Instance.StopAllEffect();
            _CloudMat.SetTexture("_MainTex", _OrignalTexture);

            _cloudstate = cloudstate.state_over;
            C_TimerMgr.Instance.RemoveTimer(_TimeMark);
            C_TimerMgr.Instance.RemoveTimer(_RainTimeMark);
            if (_CloudBlackObj!=null)
            {
                GameObject.DestroyObject(_CloudBlackObj);
                _CloudBlackObj = null;
            }
            if (_LightingObj != null)
            {
                GameObject.DestroyObject(_LightingObj);
                _LightingObj = null;
            }
            currentTime = 0f;
           
        }
        public void DoTwiceTouch()
        {
            currentTime = 0f;
            if (_cloudstate == cloudstate.state_black_over)
            {
                AudioManager.Instance.StopEffectByKey(_ThunderName);

                AudioManager.Instance.PlayEffectSound("public/sound_effect/public_xwkyx_056.ogg");
                HasInteractiveState = false;
                //开始打雷
                _cloudstate = cloudstate.state_rain;
                m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_FLASH);
               
                _LightingObj.gameObject.SetActive(true);
                C_TimerMgr.Instance.AddTimer(5, () =>
                {
                    Stop();
                    m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_IDLE);
                }, _RainTimeMark);
            }
        }
    }
}

