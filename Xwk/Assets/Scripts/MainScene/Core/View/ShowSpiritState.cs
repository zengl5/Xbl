using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene {
    public class ShowSpiritState: ActorAnimancerState
    {
        public ShowSpiritState(IActor actorAimMgr, ActorAnimancerResConfig resConfig,System.Action action = null ) : base(null)
        {
            _ActorAnimancerResConfig = resConfig;
            OnInit(actorAimMgr);
            Play(this, resConfig,()=> {
                if (action!=null)
                {
                    action();
                }
            });
        }
        public override void Stop()
        {
            base.Stop();
        }
        public override void InitResConfig()
        {
            
        }

    }

}

