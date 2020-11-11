using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    /// <summary>
    /// 循环播放动画，语音播放结束，则动作切换
    /// </summary>
    public  class PlayLoopAnimState : AnimState
    {
        

        public PlayLoopAnimState(IActorState actorState,Animator animator, InfoData infoData)
        {
            m_InfoData = infoData;
            m_Animator = animator;
            m_ActorState = actorState;
        }

        public override void Handle()
        {
            //播放动画，并且等待结束作为返回
            if (m_InfoData == null
                || (m_InfoData != null && string.IsNullOrEmpty(m_InfoData.audio)))
            {
                OnFinish();
               // if (m_ActorState != null) { m_ActorState.RequestNextState(); }
                return;
            }
            m_ActorState.PlayAudio(m_InfoData.audiotype, m_InfoData.audio, () => {
                OnFinish();
            },false);
            
            //该动作必须是循环
            if (!string.IsNullOrEmpty(m_InfoData.anim))
            {
                EnterOneState(m_InfoData.anim);
                //  m_Animator.GetCurrentAnimatorClipInfo(0)[0].clip
            }
        }

        public override void Stop()
        {
            OnCompelete = null;
            AudioManager.Instance.StopPlayerSound();
        }
    }
}
