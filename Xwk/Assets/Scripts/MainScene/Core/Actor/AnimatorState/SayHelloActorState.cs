using Animancer;
using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene {
    public class SayActorHelloState : ActorAnimancerState
    {
        private GameObject _CameraPath;
        private GameObject _Jgb;
        private AnimancerComponent _JgbAnimancerComponent;
        private Camera _CurrentCamera;
        public SayActorHelloState(IActor actorAimMgr, AnimState animState) : base(animState)
        {
            Init(actorAimMgr);
            this.state = animState;
        }
        private void Init(IActor actorAimMgr)
        {
            this.ActorStateName = "normalhello";
            OnInit(actorAimMgr);
            _CurrentCamera = m_ActorMgr.m_Camera;
#if UNITY_EDITOR
           RequestNextState(); return;
#endif

            AudioManager.Instance.PlayEffectAutoClose("main/sound/start/main_yx_01.ogg");

            this.HasInteractiveState = false;
            //临时资源，后续提醒美术修改
            _Jgb = GameResMgr.Instance.LoadResource<GameObject>("main/anim/jgb/main_model_jgb_dm_full@mesh", true);
            _Jgb.transform.SetParent(Utility.FindChild(Actor.transform, "Point_weapon"));
            _Jgb.transform.localScale = new Vector3(1.5f, 1.46f, 1.06f);
            _Jgb.transform.localPosition = Vector3.zero;
           //  _JgbAnimancerComponent = _Jgb.GetAddComponent<AnimancerComponent>();
           //  _JgbAnimancerComponent.Animator = _Jgb.GetAddComponent<Animator>();
           // _JgbAnimancerComponent.CrossFade(GameResMgr.Instance.LoadResource<AnimationClip>("main/anim/jgb/jgb_dazhaohu01#anim"));

            _CameraPath = GameResMgr.Instance.LoadResource<GameObject>("main/anim/camera/main_cam_dazhaohu01#anim", true);
            _CameraPath.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            _CurrentCamera.transform.SetParent(Utility.FindChild(_CameraPath.transform, "cam_point"));
            _CurrentCamera.transform.localScale = Vector3.one;
            _CurrentCamera.transform.localPosition = Vector3.zero;

            Play(this, _ActorAnimancerResConfig, RequestNextState);
        }
        public SayActorHelloState(IActor actorAimMgr) : base(null)
        {
            Init(actorAimMgr);
            return;
            //判断是否首次  根据当前时间是否与记录时间一致
            System.DateTime dateTime = DateTime.Now;
            string timePrefs = "main_actor_state";
            string time = string.Concat(dateTime.Year, "-", dateTime.Month, "-", dateTime.Day);
            if (PlayerPrefs.HasKey(timePrefs) && PlayerPrefs.GetString(timePrefs).Equals(time))
            {
                this.ActorStateName = "normalhello";
                Play(this);
                return;
            }
            PlayerPrefs.SetString(timePrefs, time);

            if (dateTime.Hour >= 6 && dateTime.Hour <= 8)//早上
            {
                this.ActorStateName = "morninghello";
            }
            else if (dateTime.Hour >= 11 && dateTime.Hour <= 13)//中午
            {
                this.ActorStateName = "noonhello";
            }
            else if (dateTime.Hour >= 17 && dateTime.Hour <= 19)//傍晚
            {
                this.ActorStateName = "nightfallhello";
            }
            else if (dateTime.Hour >= 22 || dateTime.Hour < 6)//深夜
            {
                this.ActorStateName = "deepnighthello";
            }

            Play(this);
        }
        public override void Play(IActorState target)
        {
            base.Play(target);
        }
        public override void Stop()
        {
            AudioManager.Instance.StopAllEffect();
            base.Stop();
            if (_CameraPath!=null)
            {
                GameObject.DestroyObject(_CameraPath);
                _CameraPath = null;
            }
            if (_Jgb!=null)
            {
              //  _Jgb.GetAddComponent<Animator>().RemoveAnimancerComponent();
                GameObject.DestroyObject(_Jgb);
                _Jgb = null;
            }
            WindowSliderControl.Instance.ReleaseCamera();


        }

        public override void RequestNextState()
        {
            state = null;
            if(_CurrentCamera==null)
                _CurrentCamera = m_ActorMgr.m_Camera;

            Transform cameraTramsform = _CurrentCamera.transform;
            cameraTramsform.SetParent(null);
            cameraTramsform.localRotation = Quaternion.Euler(new Vector3(-2f,180f,0f));
            cameraTramsform.localPosition =  new Vector3(0f,0.68f,3f);
            //进入到闲置状态
            m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_HELLO_OVER);

            C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_MainCityUp");

        }
    }

}


