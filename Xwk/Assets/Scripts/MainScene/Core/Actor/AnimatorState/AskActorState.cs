using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class AskActorState : ActorState
    {
        public AskActorState(IActor actorAimMgr) : base(null)
        {
            this.m_ActorMgr = actorAimMgr;
            this.HasInteractiveState = true;
            this.m_ActorMgr = actorAimMgr;
            this.ActorStateName = "askstate";
            Actor = m_ActorMgr.getActor();
            Play(this);
        }

        public override void RequestNextState()
        {
            m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_IDLE);
        }
        public override void Stop()
        {
            base.Stop();
            AudioManager.Instance.StopPlayerSound();
        }
    }
}
  
