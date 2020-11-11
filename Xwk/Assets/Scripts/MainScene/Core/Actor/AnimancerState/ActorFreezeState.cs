using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class ActorFreezeState : ActorAnimancerState
    {

        public ActorFreezeState(IActor actorAimMgr) : base(null)
        {
            this.ActorStateName = "freezestate";
            this.HasInteractiveState = false;
            OnInit(actorAimMgr);
            Play(this, _ActorAnimancerResConfig, RequestNextState);
        }
        public override void Stop()
        {
            //C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_ENTER_RELEASE_ROLE);

            base.Stop();

            AudioManager.Instance.StopPlayerSound();
        }
    }

}

