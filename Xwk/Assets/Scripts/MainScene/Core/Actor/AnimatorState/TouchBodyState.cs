using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class TouchBodyState : ActorAnimancerState
    {
        public TouchBodyState(IActor actorAimMgr, AnimState animState) : base(animState)
        {
            Init(actorAimMgr);
            this.state = animState;
        }
        private void Init(IActor actorAimMgr)
        {
           // this.m_ActorMgr = actorAimMgr;
            this.ActorStateName = "touchbodystate";
            OnInit(actorAimMgr);
            this.HasInteractiveState = true;
        }
        public TouchBodyState(IActor actorAimMgr) : base(null)
        {
            Init(actorAimMgr);
            Play(this,_ActorAnimancerResConfig,RequestNextState);
        }
        public override void RequestNextState()
        {
            m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_IDLE);
        }
    }
}

