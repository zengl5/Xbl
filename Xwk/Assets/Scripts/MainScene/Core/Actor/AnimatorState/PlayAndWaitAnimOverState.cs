using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.AnimCallbacks;
namespace YB.XWK.MainScene
{
    /// <summary>
    /// 播放动画和语音，动画播放结束，则事件结束
    /// </summary>
    public  class PlayAndWaitAnimOverState : AnimState
    {
        public IActorState m_ActorState;

        public PlayAndWaitAnimOverState(IActorState actorState, Animator animator, InfoData infoData)
        {
            m_InfoData = infoData;
            m_Animator = animator;
        }

        public override void Handle()
        {
            //播放动画，并且等待结束作为返回
            if (m_InfoData == null
                || (m_InfoData != null && string.IsNullOrEmpty(m_InfoData.audio)))
            {
                if (m_ActorState != null) { m_ActorState.RequestNextState(); }
                return;
            }
            AudioManager.Instance.PlayerSound(m_InfoData.audio);
            if (!string.IsNullOrEmpty(m_InfoData.anim))
            {
              //  m_Animator.Play(m_InfoData.anim, 0, 0);
                m_Animator.CrossFade(m_InfoData.anim, 0.2f);
                m_Animator.AddClipEndCallback(m_InfoData.anim, () => { if (m_ActorState != null) { m_ActorState.RequestNextState(); } });
            }
        }
        public override void Stop()
        {
            OnCompelete = null;
            AudioManager.Instance.StopPlayerSound();
        }
    }
}
