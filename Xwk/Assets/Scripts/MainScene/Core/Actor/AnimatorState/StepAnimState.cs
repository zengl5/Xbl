using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    /// <summary>
    /// 先播放声音，播放声音结束，并且多个动作播放结束，则结束
    /// </summary>
    public class StepAnimState : AnimState
    {
        private bool _AudioOver = false;
        private int _StateIndex = 0;
     //   public IActorState m_ActorState;
        public StepAnimState(IActorState actorState, Animator animator, ActorStateInfo stateInfo)
        {
            m_ActorState = actorState;
            m_ActorStateInfo = stateInfo;
            m_Animator = animator;
            _StateIndex = 0;
        }
        public override void Handle()
        {
            InfoData infoData = m_ActorStateInfo.infodata[_StateIndex];
            _StateIndex++;
            if (infoData.statetype.Equals(AnimStateConstant.k_Anim_Main_Audio))
            {
                m_ActorState.MainState = m_ActorState.PlayMode(m_ActorState, infoData, AudioPlayOver);
               
            }
            else
            {
                m_ActorState.State= m_ActorState.PlayMode(m_ActorState, infoData,  HandldeOver);
                
            }
           
            
        }
        public void AudioPlayOver()
        {
            _AudioOver = true;
            m_ActorState.MainState.OnCompelete -= AudioPlayOver;
            if (_AudioOver && _StateIndex > m_ActorStateInfo.infodata.Count - 1)
            {
                m_ActorState.State.OnCompelete -= HandldeOver;
                m_ActorState.RequestNextState();
            }
        }
        public override void HandldeOver()
        {
            if ( _StateIndex > m_ActorStateInfo.infodata.Count-1)
            {
                if (_AudioOver)
                {
                    m_ActorState.MainState.OnCompelete -= AudioPlayOver;
                    m_ActorState.State.OnCompelete -= HandldeOver;
                    m_ActorState.RequestNextState();
                }
               
            }
            else
            {
                InfoData infoData = m_ActorStateInfo.infodata[_StateIndex];
                _StateIndex++;

                m_ActorState.State = m_ActorState.PlayMode(m_ActorState, infoData, HandldeOver);

            }

        }
        public override void Stop()
        { 
            OnCompelete = null; 
        }
    }
}

