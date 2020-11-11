using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    /// <summary>
    /// 按照顺序播放，播放结束所有动作之后结束
    /// </summary>
    public class StepAnimAndAudioState : AnimState
    {
        protected int _StateIndex = 0;
      //  public IActorState m_ActorState;
        public StepAnimAndAudioState(IActorState actorState, Animator animator, ActorStateInfo stateInfo)
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
            //固定statetype为k_Anim_AnimWaitAudio
            m_ActorState.State= m_ActorState.PlayMode(m_ActorState,infoData, HandldeOver);
        }

        public override void HandldeOver()
        {
            if (_StateIndex > m_ActorStateInfo.infodata.Count-1)
            {
                if (m_ActorState.State != null)
                {
                    m_ActorState.State.OnCompelete -= HandldeOver;
                    m_ActorState.RequestNextState();

                }
                else
                {
                    C_DebugHelper.Log("m_ActorState.State is null.................................");
                }
            }
            else
            {
                InfoData infoData = m_ActorStateInfo.infodata[_StateIndex];
                _StateIndex++;
                 m_ActorState.State=m_ActorState.PlayMode(m_ActorState, infoData, HandldeOver);
            }
        }
        public override void Stop()
        {
            OnCompelete = null;
            AudioManager.Instance.StopPlayerSound();
        }
    }
}

