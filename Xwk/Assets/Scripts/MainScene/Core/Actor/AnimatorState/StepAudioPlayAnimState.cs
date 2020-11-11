using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace YB.XWK.MainScene
{
    /// <summary>
    /// 按照顺序播放，播放结束所有动作之后结束
    /// </summary>
    public class StepAudioPlayAnimState : AnimState
    {
        private bool _AnimOverFlag = false;
        private bool _AudioOverFlag = false;
        private int _StateIndex = 0;
        public StepAudioPlayAnimState(IActorState actorState, Animator animator, ActorStateInfo stateInfo)
        {
            m_ActorState = actorState;
            m_ActorStateInfo = stateInfo;
            m_Animator = animator;
            _StateIndex = 0;
            _AudioOverFlag = false;
            _AnimOverFlag = false;
        }
        public override void Handle()
        {
            InfoData infoData = m_ActorStateInfo.infodata[_StateIndex];
            _StateIndex++;
            m_ActorState.State.OnCompelete -= m_ActorState.RequestNextState;
            if (infoData.statetype.Equals(AnimStateConstant.k_Anim_Main_Audio))
            {
                m_ActorState.MainState = m_ActorState.PlayMode(m_ActorState, infoData, HandldeOver);
            }
            else
            {
                m_ActorState.State = m_ActorState.PlayMode(m_ActorState, infoData, AnimPlayOver);
            }
        }
        public void AnimPlayOver()
        {
            if (_StateIndex > m_ActorStateInfo.infodata.Count - 1)
            {
                _AnimOverFlag = true;

            }
            else
            {
                InfoData infoData = m_ActorStateInfo.infodata[_StateIndex];
                _StateIndex++;

                m_ActorState.State = m_ActorState.PlayMode(m_ActorState, infoData, AnimPlayOver);
            }
        }

        public override void HandldeOver()
        {
            if (!_AnimOverFlag)
            {
                m_ActorState.State.Stop();
            }
            m_ActorState.MainState.OnCompelete -= AnimPlayOver;

            m_ActorState.State.OnCompelete -= HandldeOver;
            m_ActorState.RequestNextState();
        }
        public override void Stop()
        {
            OnCompelete = null;
            AudioManager.Instance.StopPlayerSound();
        }
    }
}