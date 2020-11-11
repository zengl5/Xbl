using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.AnimCallbacks;

namespace YB.XWK.MainScene
{
    public class CloudIdleState : ActorState
    {
        private Animator _Animator;
        private float currentTime = 0f;
        private string currentAnimName;
        public CloudIdleState(IActor actorAimMgr) : base(null)
        {
            currentTime = 0f;
            HasInteractiveState = true;
            this.ActorStateName = "idlestate";
            OnInit(actorAimMgr, ActorStateName);
            _Animator = m_ActorMgr.getAnimator();
            Play(this);
        }
        public override void Play(IActorState target)
        {
            InfoData infoData = null;

            ActorStateInfo info = m_ActorMgr.getInfo(this.ActorStateName);

            if (info == null)
            {
                C_DebugHelper.LogWarning(this.ActorStateName + ": info is null");
                return;
            }
            infoData = info.infodata[Random.Range(0, info.infodata.Count)];
            InitAc(infoData.acname);
            if (!string.IsNullOrEmpty(currentAnimName))
            {
                _Animator.RemoveClipEndCallback(currentAnimName, PlayOver);
            }
            currentAnimName = infoData.anim;
            EnterOneState(currentAnimName);
            _Animator.AddClipEndCallback(currentAnimName, PlayOver);
        }
        public void PlayOver()
        {
            _Animator.RemoveClipEndCallback(currentAnimName, PlayOver);
            int probility = Random.Range(0, 10);
            if (probility <= 8)
            {
                Play(this);
            }
            else
            {
                m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_WALKAROUND);
            }
        }
        public override void Stop()
        {
            
        }
    }


}
