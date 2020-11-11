using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    /// <summary>
    ///播放一个声音和多个动作，声音播放结束，则动作动作结束
    /// </summary>
    public class StepAudioPlayAnimAncerState : AnimAncerAimState
    {
        private bool _AnimOverFlag = false;
        private bool _AudioOverFlag = false;
        private int _StateIndex = 0;
        public StepAudioPlayAnimAncerState(IActorState actorState, AnimancerComponent animancerComponent, ActorAnimancerResConfig stateInfo )
        {
            m_ActorState = actorState;
            m_ActorAnimancerResConfig = stateInfo;
            m_AnimancerComponent = animancerComponent;
            _StateIndex = 0;
            _AudioOverFlag = false;
            _AnimOverFlag = false;
        }
        public override void Handle()
        {
            
            m_InfoData = m_ActorAnimancerResConfig.aimDatas[_StateIndex];
            _StateIndex++;
            if (!string.IsNullOrEmpty(m_InfoData.audio))
            {
                m_ActorState.DoAnimancerMainState = m_ActorState.PlayMode(m_ActorState, m_InfoData, PlayOver);
                m_ActorState.DoAnimancerMainState.OnCompeleteNext -= DoNext;
                m_ActorState.DoAnimancerMainState.OnCompeleteNext += DoNext;
            }
            else
            {
                m_ActorState.DoAnimancerOtherState = m_ActorState.PlayMode(m_ActorState, m_InfoData, AnimPlayOver);
            }
        }
        public void DoNext()
        {
            if (_StateIndex >= m_ActorAnimancerResConfig.aimDatas.Count)
            {
                return;
            }
            Handle();
        }
        public void AnimPlayOver()
        {
            if (_StateIndex >= m_ActorAnimancerResConfig.aimDatas.Count)
            {
                _AnimOverFlag = true;
            }
            else
            {
                Handle();
            }
        }
        public void PlayOver()
        {
            if (!_AnimOverFlag&& m_ActorState.DoAnimancerOtherState!=null)
            {
                m_ActorState.DoAnimancerOtherState.Stop();
            }
            if (OnCompelete!=null)
            {
                OnCompelete();
                OnCompelete = null;
            }
        }
        
        public override void Stop()
        {
            if(m_ActorState==null)
            {
                return;
            }
            if (m_ActorState.DoAnimancerMainState != null)
            {
                m_ActorState.DoAnimancerMainState.Stop();
                m_ActorState.DoAnimancerMainState = null;
            }
            if (m_ActorState.DoAnimancerOtherState != null)
            {
                m_ActorState.DoAnimancerOtherState.Stop();
                m_ActorState.DoAnimancerOtherState = null;
            }
        }

    }
     
}

