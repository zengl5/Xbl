using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene {
    public class ActorShowState : IdleActorState
    {
        public ActorShowState(IActor actorAimMgr) : base(actorAimMgr)
        {
            ActorStateName = "showstate";
            OnInit(actorAimMgr);
            Play(this,_ActorAnimancerResConfig,RequestNextState);
        }
        public override void RequestNextState()
        {
            m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_IDLE);
        }
    }
}


