using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    /// <summary>
    /// 按照配置的顺序播放配置的动画，声音
    /// </summary>
    public class StepAnimAncerState : AnimAncerAimState
    {
        private int _StateIndex = 0;
        public StepAnimAncerState(IActorState actorState, AnimancerComponent animancerComponent, ActorAnimancerResConfig stateInfo )
        {
            m_ActorState = actorState;
            m_ActorAnimancerResConfig = stateInfo;
            m_AnimancerComponent = animancerComponent;
            _StateIndex = 0;
        }
        public override void Handle()
        {
            m_InfoData = m_ActorAnimancerResConfig.aimDatas[_StateIndex];
            _StateIndex++;
            //播放动画
            m_CurentAncerAnimState =  m_ActorState.PlayMode(m_ActorState, m_InfoData, PlayOver);
        }
        public void PlayOver()
        {
            //播放结束，显示ui
            if (_StateIndex >= m_ActorAnimancerResConfig.aimDatas.Count)
            {
                OnFinish();
            }
            else
            {
                Handle();
            }
        }
        
        public override void Stop()
        {
            if(m_CurentAncerAnimState != null)
            {
                m_CurentAncerAnimState.Stop();
                m_CurentAncerAnimState = null;
            }
        }

    }

}

