using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.AnimCallbacks;
namespace YB.XWK.MainScene
{
    /// <summary>
    /// 播放动画 ,动画播放结束,则事件结束
    /// </summary>
    public  class PlayAnimState : AnimState
    {
        public IActorState m_ActorState;

        public PlayAnimState(IActorState actorState, Animator animator, InfoData infoData)
        {
            m_InfoData = infoData;
            m_Animator = animator;
        }

        public override void Handle()
        {
            //播放动画，并且等待结束作为返回
            if (m_InfoData == null)
            {
                Stop();
                return;
            }
            if (!string.IsNullOrEmpty(m_InfoData.anim))
            {
                EnterOneState(m_InfoData.anim);
                //点击动画的结束事件，播放结束，将事件去除
                m_Animator.RemoveClipEndCallback(m_InfoData.anim, new System.Action(Player));
                m_Animator.AddClipEndCallback(m_InfoData.anim, new System.Action(Player));
            }
        }
        void Player()
        {
            m_Animator.RemoveClipEndCallback(m_InfoData.anim, new System.Action(Player));
            if (OnCompelete!=null)
            {
                OnCompelete();
            }
        }
        public override void Stop()
        {
            m_Animator.RemoveClipEndCallback(m_InfoData.anim, new System.Action(Player));
            OnCompelete = null;
            //OnFinish();
        }
    }
}
