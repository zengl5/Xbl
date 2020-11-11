using Animancer;
using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
   
    public interface IInteractive
    {
        void EnterInteractiveState();
    }
    public interface IActorState
    {
        AnimState MainState { get; set; }

        AnimState State { get; set; }

        AnimAncerAimState DoAnimancerOtherState { get; set; }

        AnimAncerAimState DoAnimancerMainState { get; set; }

        void RequestNextState();

        AnimState PlayMode(IActorState target, InfoData infoData, AnimState.Compelete callback);

        AnimAncerAimState PlayMode(IActorState target, ActorAnimancerData infoData, AnimStateBase.Compelete callback);

        void Stop();

        void OnUpdate();

        void PlayAudio(string audioType, string audio, System.Action callback, bool loop = false);
    }
    public class ActorStateBase : IActorState {
        protected Transform Actor { set; get; }

        //每个动作等待语音结束 
        internal protected const string k_State_One = "onestate";
        //动作等语音结束，进入下一组
        internal protected const string k_State_StepAnim = "stepanimstate";

        internal protected const string k_State_StepAnimAndAudio = "stepanimaudiostate";
        //等待多个播放语音播放结束
        internal protected const string k_State_StepAudio = "stepaudio";

        //todo清理下这些类型
        //播放一个声音，通过播放多个动作，声音结束，则结束
        internal protected const string k_State_StepAudio_PlayAnim = "stepaudio_palyanim";
          
        internal protected const string k_State_Ask = "askstate";
        //播放声音的同时播放动作，等待语音结束
        internal protected const string k_Anim_AnimWaitAudio = "play_audio_anim";
        //播放动画，结束回调
        internal protected const string k_Anim_Anim = "play_anim";
        //播放声音，结束回调
        internal protected const string k_Anim_Audio = "play_audio";
        //播放声音和动画，动画和声音都结束时，则结束
        internal protected const string k_Anim_AudioAndAnim = "play_audio_and_anim";
        //动画或者声音结束，则告诉调用方，声音结束则结束
        internal protected const string k_Anim_AudioOrAnim = "play_audio_or_anim_over";

        internal protected const string k_Anim_WaitAnswer = "wait_answer";
        //同时播放动作和声音，动作播放结束，则结束。
        internal protected const string k_Anim_AudioAndAnim_Wait_AnimOver= "play_audio_and_anim_wait_animover";
        

        public IActor m_ActorMgr;

        public string ActorStateName { set; get; }

        //使用actor的公共点击事件
        public bool HasInteractiveState { get; set; }

        public AnimState state;
        public AnimState mainstate;
        public AnimState actorstate;
        public AnimAncerAimState otherAccerAinmstate;
        public AnimAncerAimState mainAccerAinmstate;
        public AnimAncerAimState m_ActorAnimancerState;

        public AnimState State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
            }
        }

        public AnimState MainState
        {
            get
            {
                return mainstate;

            }

            set
            {
                mainstate = value;

            }
        }
        public AnimState CurrentActorstate
        {
            get
            {
                return actorstate;
            }
            set
            {
                actorstate = value;
            }
        }
        public AnimAncerAimState DoActorAnimancerState
        {
            get
            {
                return m_ActorAnimancerState;
            } 
            set
            {
                m_ActorAnimancerState = value;
            }
        }

        public AnimAncerAimState DoAnimancerOtherState
        {
            get
            {
                return otherAccerAinmstate;
            }
            set
            {
                otherAccerAinmstate = value;
            }
        }

        public AnimAncerAimState DoAnimancerMainState
        {
            get
            {
                return mainAccerAinmstate;
            }
            set
            {
                mainAccerAinmstate = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="state">默认ActorContext的初始状态</param>
        public ActorStateBase(AnimState state)
        {
            State = state;
        }

        protected virtual void OnInit(IActor actorAimMgr, string state)
        {
            this.m_ActorMgr = actorAimMgr;
            this.ActorStateName = state;
            Actor = m_ActorMgr.getActor();
        }
        public virtual void Play(IActorState target)
        {
             
        }
        public virtual AnimState PlayMode(IActorState target, InfoData infoData, AnimState.Compelete callback)
        {
            return null;
        }
        public virtual AnimAncerAimState PlayMode(IActorState target, ActorAnimancerData infoData, AnimStateBase.Compelete callback)
        {
            return null;
        }
        public Animator InitAc(string acPath)
        {
            if (m_ActorMgr == null)
            {
                return null;
            }

            Animator actorAnimator = m_ActorMgr.getAnimator();
            if (actorAnimator == null)
            {
                return null;
            }
            ////更新动画控制器
            if (actorAnimator.runtimeAnimatorController == null || (
                actorAnimator.runtimeAnimatorController != null &&
                !actorAnimator.runtimeAnimatorController.name.Equals(C_String.GetFileName(acPath))
                )
                )
            {
                actorAnimator.runtimeAnimatorController = GameResMgr.Instance.LoadResource<RuntimeAnimatorController>(acPath);
            }
            return actorAnimator;
        }
       
        public virtual void RequestNextState()
        {

        }
        public virtual void Stop()
        {
            if (mainstate != null)
            {
                mainstate.Stop();
                mainstate = null;
            }
            if (State != null)
            {
                State.Stop();
                State = null;
            }
            if (CurrentActorstate !=null)
            {
                CurrentActorstate.Stop();
            }
        }

        public virtual void Touch()
        {
            Stop();
        }

        public virtual void OnUpdate()
        {
            if (Actor == null || m_ActorMgr == null)
            {
                return;
            }
            Actor.LookAt(new Vector3(m_ActorMgr.m_Camera.transform.position.x, Actor.position.y, m_ActorMgr.m_Camera.transform.position.z));
        }

        public virtual void CommandNextState()
        {

        }

        public virtual void EnterOneState(string satename, bool isRet = true)
        {
            Animator animator = m_ActorMgr.getAnimator();

            animator.SetTrigger(satename);
            return;

        }
      

        public virtual void PlayAudio(string audioType, string audio, System.Action callback, bool loop = false)
        {
            string audioTmp = audio;
            if (!string.IsNullOrEmpty(audio) && audio.Contains(","))
            {
                string[] data = audio.Split(',');
                audioTmp = data[UnityEngine.Random.Range(0, data.Length)];
            }

            if (string.IsNullOrEmpty(audioType))
            {
                AudioManager.Instance.PlayerSound(audioTmp, loop, callback);
                return;
            }
            if (audioType.Equals("1"))//先播放小名，再直接播放声音
            {
                if (BabyName.c_BabyNameAudioClip == null)
                {
                    AudioManager.Instance.PlayerSound(audioTmp, loop, callback);
                }
                else
                {
                    AudioManager.Instance.PlayerSoundByClip(BabyName.c_BabyNameAudioClip, () =>
                    {
                        AudioManager.Instance.PlayerSound(audioTmp, loop, callback);
                    });
                }
            }
        }
       
    }

    public class ActorState: ActorStateBase
    {
       
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="state">默认ActorContext的初始状态</param>
        public ActorState(AnimState state):base(null)
        {
            State = state;
        }
     
        protected override void OnInit(IActor actorAimMgr,string state)
        {
            this.m_ActorMgr = actorAimMgr;
            this.ActorStateName = state;
            Actor = m_ActorMgr.getActor();
             
        }
        public override void Play(IActorState target)
        {
            if (target == null)
            {
                return;
            }
            InfoData infoData = null;

            ActorStateInfo info = m_ActorMgr.getInfo(this.ActorStateName);

            if (info == null)
            {
                C_DebugHelper.LogWarning(this.ActorStateName + ": info is null");
                return;
            }
            //切换动作
            switch (info.statetype)
            {
                case k_State_One:
                    {
                        //同时只有一个状态
                        infoData = info.infodata[UnityEngine.Random.Range(0, info.infodata.Count)];
                        CurrentActorstate =  PlayMode(target, infoData, RequestNextState);
                    }
                    break;
                case k_State_StepAnimAndAudio:
                    {
                        CurrentActorstate = new StepAnimAndAudioState(target, m_ActorMgr.getAnimator(), info);
                        CurrentActorstate.Handle();
                    }
                    break;
                case k_State_StepAnim:
                    {
                        CurrentActorstate = new StepAnimState(target, m_ActorMgr.getAnimator(), info);
                        CurrentActorstate.Handle();

                    }
                    break;
                case k_State_StepAudio:
                    {
                        CurrentActorstate = new StepAudioState(target, m_ActorMgr.getAnimator(), info);
                        CurrentActorstate.Handle();

                    }
                    break;
                case k_State_StepAudio_PlayAnim:
                    {
                        CurrentActorstate = new StepAudioPlayAnimState(target, m_ActorMgr.getAnimator(), info);
                        CurrentActorstate.Handle();
                    }
                    break;
                case k_State_Ask:
                    {
                        CurrentActorstate = new AskAnimState(target, m_ActorMgr.getAnimator(), info);
                        CurrentActorstate.Handle();
                    }
                    break;
                default: break;
            }
        }
       public override AnimState PlayMode(IActorState target, InfoData infoData, AnimState.Compelete callback)
        {
            AnimState currentState = null;
            if (target== null)
            {
                return currentState;
            }
            Animator animator = InitAc(infoData.acname);
            if (animator == null)
            {
 
                return currentState;
            }
            switch (infoData.statetype)
            {
                case k_Anim_AnimWaitAudio:
                    {
                        //进入播放声音同时播放动作，声音结束进入下一个状态
                        currentState = new PlayLoopAnimState(target, animator, infoData);
                    }
                    break;
                case k_Anim_Audio:
                    {
                        //播放声音结束，进入下一个状态
                        currentState = new PlayAudioState(target, animator, infoData);
                    }
                    break;
                case k_Anim_Anim:
                    {
                        //播放动画结束，进入下一个状态
                        currentState = new PlayAnimState(target, animator, infoData);
                    }
                    break;
                case k_Anim_WaitAnswer:
                    {
                        //播放动画结束，进入下一个状态
                        currentState = new PlayWaitTimeAnimState(target, animator, infoData);
                    }
                    break;
                case k_Anim_AudioAndAnim:
                    {

                        currentState = new PlayAudioAndAnimAllOverState(target, animator, infoData);
                    }
                    break;
                default:
                    {
                        currentState = null;
                    }
                    break;
                   
            }
            if (currentState!=null)
            {
                currentState.OnCompelete -= callback;
                currentState.OnCompelete += callback;
                currentState.Handle();
            }
            return currentState;

        }

    }
}

