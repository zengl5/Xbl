using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class TouchTailState : ActorAnimancerState
    {
        public TouchTailState(IActor actorAimMgr, AnimState animState) : base(animState)
        {
            Init(actorAimMgr);
            this.state = animState;
        }
        private void Init(IActor actorAimMgr)
        {
         //   this.m_ActorMgr = actorAimMgr;
            this.ActorStateName = "touchtailstate";
            OnInit(actorAimMgr);

            this.HasInteractiveState = true;
        }
        public TouchTailState(IActor actorAimMgr) : base(null)
        {
            Init(actorAimMgr);
            Play(this,_ActorAnimancerResConfig, RequestNextState);
        }
        public override void RequestNextState()
        {
            m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_IDLE);
        }
    }
}

