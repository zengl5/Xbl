using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class SpiritRoleMgr : ActorBase
    {
        //public override Animator getAnimator()
        //{
        //    return transform.GetAddComponent<Animator>();
        //}
        public override Transform getActor()
        {
            return transform;
        }
        public override Animator getAnimator()
        {
            return transform.GetAddComponent<Animator>();
        }

    }
}
