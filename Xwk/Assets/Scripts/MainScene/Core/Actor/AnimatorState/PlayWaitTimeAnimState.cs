using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YB.XWK.MainScene
{

    /// <summary>
    /// 循环播放动画，等待一段时间，结束
    /// </summary>
    public class PlayWaitTimeAnimState : AnimState
    {
        public PlayWaitTimeAnimState(IActorState actorState, Animator animator, InfoData infoData)
        {
            m_InfoData = infoData;
            m_Animator = animator;
            m_ActorState = actorState;
        }

        public override void Handle()
        {
            //播放动画，并且等待结束作为返回
            if (m_InfoData == null
                || (m_InfoData != null && string.IsNullOrEmpty(m_InfoData.anim)))
            {
                OnFinish();
                return;
            }
            C_TimerMgr.Instance.AddTimer(3,()=> {
                PlayOver();
            },"PlayWaitTimeAnimState");
            //该动作必须是循环
            if (!string.IsNullOrEmpty(m_InfoData.anim))
            {
                EnterOneState(m_InfoData.anim);
            }
        }
        void PlayOver()
        {
            C_TimerMgr.Instance.RemoveTimer("PlayWaitTimeAnimState");
            if (OnCompelete!=null)
            {
                OnCompelete();
            }
        }

        public override void Stop()
        {
            C_TimerMgr.Instance.RemoveTimer("PlayWaitTimeAnimState");
            OnCompelete = null;
        }
    }
}