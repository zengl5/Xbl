using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XBL.Core;
using YB.AnimCallbacks;
using DG.Tweening;
//using PinYinREAD;
using Assets.Scripts.C_Framework;

namespace YB.XWK.MainScene
{

    public class FindGoldenCudgelState : ActorState
    {
        private enum ActorState {
            wait,
            task,
            walk,
            start,
            volumerecognition,
            recordsuccess,
            touch,
            over,
            returnback,
        }
        private ActorState _Actorstate;
        private GameObject _GoldenCudgeOBJ;
        private Animator _ActorAnimator;
        private Transform _Actor;
        private string _TalkClipName = "XBL_talk1@anim";
        private string _WalkClipName = "XBL_walk@anim";
        private Vector3 _Desition = new Vector3(0.69f, 0f, -2.8f);

        private string _PlayerPreFlag = "FindGoldenCudgelState";

        private float _OriginZ = 0f;

        public FindGoldenCudgelState(IActor actorAimMgr, AnimState animState) : base(animState)
        {
            Init(actorAimMgr);
            this.state = animState;
        }
        private void Init(IActor actorAimMgr)
        {
            this.m_ActorMgr = actorAimMgr;
            this.ActorStateName = "findgoldencudge";
            this.HasInteractiveState = false;
        }
        public FindGoldenCudgelState(IActor actorAimMgr) : base(null)
        {
            

            Init(actorAimMgr);
#if   true
           m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_HELLO);
            
            return;
#endif
            _ActorAnimator = m_ActorMgr.getAnimator();
            _OriginZ = _ActorAnimator.transform.position.z;
            _Actorstate = ActorState.wait;
            //判断当前是否已经完成了找金箍棒的引导，已经完成，则进入到sayhellostate
            string flag = PlayerPrefs.GetString(_PlayerPreFlag, string.Empty);
            if (string.IsNullOrEmpty(flag))
            {
                //开始播放
                _GoldenCudgeOBJ = GameResMgr.Instance.LoadResource<GameObject>("main/prefab/goldencudge.prefab", true);
                _ActorAnimator.runtimeAnimatorController = GameResMgr.Instance.LoadResource<RuntimeAnimatorController>("main/animationcontroller/findgoldencudgel");
                _Actorstate = ActorState.task;
            }
            else
            {
                RequestNextState();
            }
        }
        private void Talk(string clipName, string audioName, System.Action callback=null)
        {
            Stand();
            _ActorAnimator.SetBool(clipName, true);

            //隐藏声音特效
            AudioManager.Instance.PlayerSound(audioName, false, () => {
                _ActorAnimator.SetBool(clipName, false);
                if(callback!=null)
                    callback();
            });
        }

        private void Stand()
        {
            _ActorAnimator.SetBool(_TalkClipName, false);
            _ActorAnimator.SetBool(_WalkClipName, false);

        }
        void WalkOver()
        {
            _ActorAnimator.SetBool(_WalkClipName, false);

        }
        void AddWarringEvent()
        {
            C_TimerMgr.Instance.AddTimer(5, () => {  Talk(_TalkClipName, "public/sound/public_sd_164.ogg"); }, "FindGoldenCudgel_warnning");
        }
        void RemoveWarringEvent()
        {
            C_TimerMgr.Instance.RemoveTimer("FindGoldenCudgel_warnning");
        }


        public override void OnUpdate()
        {
            switch (_Actorstate)
            {
                case ActorState.task:
                    {
                        _Actorstate = ActorState.wait;
                        Talk(_TalkClipName, "public/sound/public_sd_164.ogg", () => { _Actorstate = ActorState.walk; });
                    }
                    break;
                case ActorState.walk:
                    {
                        _Actorstate = ActorState.wait;
                        Stand();
                        _ActorAnimator.SetBool(_WalkClipName, true);
                        //循环播放走动画
                        _ActorAnimator.AddClipEndCallback(_WalkClipName, WalkOver);
                        Vector3 forwardDir = _Desition - _ActorAnimator.transform.position;
                        Quaternion lookAtRot = Quaternion.LookRotation(forwardDir);
                        //移动角色到目标点
                        Sequence mySequence = DOTween.Sequence();
                        mySequence.Append(_ActorAnimator.transform.DORotate(lookAtRot.eulerAngles, 0.5f))
                            .Append(_ActorAnimator.transform.DOMove(_Desition, 2f))
                            .Append(_ActorAnimator.transform.DORotate(Vector3.zero,0.5f))
                            .OnComplete(
                            () => {
                                _ActorAnimator.RemoveClipEndCallback(_WalkClipName, WalkOver);
                                _Actorstate = ActorState.start;
                            }
                            );
                       

                    }
                    break;
                case ActorState.start:
                    {
                        _Actorstate = ActorState.wait;
                        Talk(_TalkClipName, "public/sound/public_sd_164.ogg", () => { _Actorstate = ActorState.volumerecognition; });


                    }
                    break;
                case ActorState.volumerecognition:
                    {
                        _Actorstate = ActorState.wait;


                        //显示语音识别的特效
#if UNITY_EDITOR
                        _Actorstate = ActorState.recordsuccess;
#else
                        _Actorstate = ActorState.recordsuccess;



                    //    //开始音量识别
                    //    MicPhoneSinglton.Instance.StartRunTimeVolumeCheck((isV,Pr)=>{
                    //        if (isV)
                    //        {
                    //            _Actorstate = ActorState.recordsuccess;
                    //        }
                    //        else//没声音
                    //        {

                    //            Talk(_TalkClipName, "public/sound/public_sd_164.ogg", () => { _Actorstate = ActorState.volumerecognition; });
                    //        }
                    //});
#endif



                    }
                    break;
                case ActorState.recordsuccess:
                    {
                        _Actorstate = ActorState.wait;
                        Talk(_TalkClipName, "public/sound/public_sd_164.ogg", () => {

                            _Actorstate = ActorState.touch;
                            AddWarringEvent();
                        });
                        
                    }
                    break;

                case ActorState.touch:
                    {
                        if (TouchManager.Instance.IsTouchValid(0))
                        {
                            TouchPhaseEnum phase = (TouchPhaseEnum)TouchManager.Instance.GetTouchPhase(0);
                            if (phase == TouchPhaseEnum.BEGAN)
                            {
                                RemoveWarringEvent();

                                Vector2 startTouchpos;

                                TouchManager.Instance.GetTouchPos(0, out startTouchpos);
                                RaycastHit hit;
                                Ray ray = m_ActorMgr.m_Camera.ScreenPointToRay(startTouchpos);
                                if (Physics.Raycast(ray, out hit, 1000) && hit.collider != null)
                                {
                                    GameObject obj = hit.collider.gameObject;
                                    if (obj != null && obj.name.Equals("goldencudge(Clone)"))
                                    {
                                        C_TimerMgr.Instance.AddTimer(3,()=> {
                                            _Actorstate = ActorState.over;
                                        },"FindGoldenCudgel");
                                    }
                                }

                            }
                            else if (phase == TouchPhaseEnum.ENDED)
                            {
                                AddWarringEvent();
                                C_TimerMgr.Instance.RemoveTimer("FindGoldenCudgel");
                            }
                        }

                    }
                    break;
                case ActorState.over:
                    {
                        RemoveWarringEvent();

                        _Actorstate = ActorState.wait;
                        //切换金箍棒的手印，添加特效，并出提示语音
                        Talk(_TalkClipName, "public/sound/public_sd_164.ogg", () => {
                            //胜利的动画和声音
                            Talk(_TalkClipName, "public/sound/public_sd_164.ogg", () => { _Actorstate = ActorState.returnback; });
                        });

                    }
                    break;
                case ActorState.returnback:
                    {
                        _Actorstate = ActorState.wait;
                        Stand();
                        _ActorAnimator.SetBool(_WalkClipName,true);
                        _ActorAnimator.AddClipEndCallback(_WalkClipName, WalkOver);
                        //移动到开始点
                        _Desition = new Vector3(_ActorAnimator.transform.position.x, _OriginZ, _ActorAnimator.transform.position.z);
                        Vector3 forwardDir = _Desition - _ActorAnimator.transform.position;
                        Quaternion lookAtRot = Quaternion.LookRotation(forwardDir);
                        //移动角色到目标点
                        Sequence mySequence = DOTween.Sequence();
                        mySequence.Append(_ActorAnimator.transform.DORotate(lookAtRot.eulerAngles, 0.5f))
                            .Append(_ActorAnimator.transform.DOMove(_Desition, 2f))
                            .Append(_ActorAnimator.transform.DORotate(Vector3.zero, 0.5f))
                            .OnComplete(
                            () => {
                                _ActorAnimator.RemoveClipEndCallback(_WalkClipName, WalkOver);
                                RequestNextState();

                                 PlayerPrefs.SetString(_PlayerPreFlag, _PlayerPreFlag);
                            }
                            );

                    }
                    break;
                default: break;
            }
            
        }
        
        public override void RequestNextState()
        {
            //删除金箍棒
            GameObject.Destroy(_GoldenCudgeOBJ);
            _GoldenCudgeOBJ = null;
           m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_HELLO);
        }
    }
}
