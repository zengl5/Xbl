using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class TouchHeadState : ActorState
    {
        public TouchHeadState(IActor actorAimMgr, AnimState animState) : base(animState)
        {
            Init(actorAimMgr);
            this.state = animState;
        }
        private void Init(IActor actorAimMgr)
        {
            this.m_ActorMgr = actorAimMgr;
            this.ActorStateName = "touchheadstate";
            this.HasInteractiveState = false;
        }
        public TouchHeadState(IActor actorAimMgr) : base(null)
        {
            Init(actorAimMgr);
            Play(this);
        }
        public override void RequestNextState()
        {
            m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_IDLE);
        }
    }
}

