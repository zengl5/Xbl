using Animancer;
using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class ActorAnimancerState : ActorStateBase
    {
        public ActorAnimancerResConfig _ActorAnimancerResConfig;
        public AnimancerComponent _AnimancerComponent;
        protected virtual void OnInit(IActor actorAimMgr)
        {
            this.m_ActorMgr = actorAimMgr;
            Actor = m_ActorMgr.getActor();
            _AnimancerComponent = Actor.GetAddComponent<AnimancerComponent>();
            Animator animator = m_ActorMgr.getAnimator();
            animator.runtimeAnimatorController = null;
            _AnimancerComponent.Animator = animator;
            _AnimancerComponent.enabled = true;
            InitResConfig();

        }
        public virtual void InitResConfig()
        {
            _ActorAnimancerResConfig = m_ActorMgr.getActorResConfig(ActorStateName);
        }

        public ActorAnimancerState(AnimStateBase state) : base(null)
        {

        }
        public ActorAnimancerState(): base(null)
        {

        }
        public override void Stop()
        {
            base.Stop();
            if (m_ActorAnimancerState!=null)
            {
                m_ActorAnimancerState.Stop();
                m_ActorAnimancerState = null;
            }
            if (_AnimAncerAimState!=null)
            {
                _AnimAncerAimState.Stop();
                _AnimAncerAimState = null;
            }
        }
        public  void Play(IActorState target,ActorAnimancerResConfig actorAnimancerResConfig,AnimStateBase.Compelete callback)
        {
            if (target == null)
            {
                return;
            }
           
            if (actorAnimancerResConfig == null)
            {
                C_DebugHelper.LogWarning(": actorAnimancerResConfig is null");
                return;
            }
            switch (actorAnimancerResConfig.statetype)
            {
                case k_State_StepAnimAndAudio:
                case k_State_One:
                    {
                        m_ActorAnimancerState = new StepAnimAncerState(target, _AnimancerComponent, actorAnimancerResConfig);
                        m_ActorAnimancerState.Handle();
                    }
                    break;
                case k_State_StepAudio_PlayAnim:
                    {
                        m_ActorAnimancerState = new StepAudioPlayAnimAncerState(target, _AnimancerComponent, actorAnimancerResConfig);
                        m_ActorAnimancerState.Handle();
                    }
                    break;
             
                default: break;
            }
            if (m_ActorAnimancerState!=null)
            {
                m_ActorAnimancerState.OnCompelete -= callback;
                m_ActorAnimancerState.OnCompelete += callback;
            }
        }
       
        public override AnimAncerAimState PlayMode(IActorState target, ActorAnimancerData infoData,  AnimStateBase.Compelete callback)
        {
            AnimAncerAimState currentState = null;
            if (target == null)
            {
                return currentState;
            }
           
            switch (infoData.playtype)
            {
                case k_Anim_AnimWaitAudio:
                    {
                        //进入播放声音同时播放动作，声音结束进入下一个状态
                         currentState = new PlayAnimancerWaitAudioState(target, _AnimancerComponent, infoData);
                    }
                    break;
                case k_Anim_Audio:
                    {
                        //播放声音结束，进入下一个状态
                        currentState = new PlayAnimancerAudioState(target, infoData.audio,infoData.audiotype);
                    }
                    break;
                case k_Anim_Anim:
                    {
                        //播放动画结束，进入下一个状态
                        currentState = new PlayAnimancerAnimState(target, _AnimancerComponent, infoData.anim);
                    }
                    break;
              
                case k_Anim_AudioAndAnim:
                    {
                        //动画和语音同时都结束，进入到下一个状态
                        currentState = new PlayAnimancerAllOverState(target, _AnimancerComponent, infoData);
                    }
                    break;
                case k_Anim_AudioOrAnim:
                    {
                        currentState = new PlayAnimancerOrAudioState(target, _AnimancerComponent, infoData);
                    }
                    break;
                case k_Anim_AudioAndAnim_Wait_AnimOver:
                    {
                        currentState = new PlayAnimancerAudioAndAnimWaitAnimOverState(target, _AnimancerComponent, infoData);
                    }
                    break;
                default:
                    {
                        currentState = null;
                    }
                    break;

            }
            if (currentState != null)
            {
                currentState.OnCompelete -= callback;
                currentState.OnCompelete += callback;
                currentState.Handle();
            }
            return currentState;
        }

        protected AnimAncerAimState _AnimAncerAimState;

        protected virtual void PlayAnim(string path)
        {
            if (_AnimAncerAimState != null)
            {
                PlayAnimancerAnimState playAnimancerAnimState = (PlayAnimancerAnimState)_AnimAncerAimState;
                playAnimancerAnimState.CrossFade(path);
            }
            else
            {
                _AnimAncerAimState = new PlayAnimancerAnimState(this, _AnimancerComponent, path);
                _AnimAncerAimState.Handle();
            }
        }
        protected virtual string getAnimationClipName()
        {
            PlayAnimancerAnimState playAnimancerAnimState = (PlayAnimancerAnimState)_AnimAncerAimState;
          return  playAnimancerAnimState.CurrentAnimationName;
        }
    } 
}
