using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class CloudHelloState : ActorState
    {

        public CloudHelloState(IActor actorAimMgr) : base(null)
        {
            HasInteractiveState = false;
            this.ActorStateName = "hellostate";
            OnInit(actorAimMgr, ActorStateName);
            Play(this);
        }
        
        public override void RequestNextState()
        {
            //动画都播放结束 
            //恢复位置
            m_ActorMgr.getActor().transform.localPosition = new Vector3(-1f, 2.30f, -2f);
            m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_IDLE);
        }
        public override void Stop()
        {
            base.Stop();
        }

    }
}
