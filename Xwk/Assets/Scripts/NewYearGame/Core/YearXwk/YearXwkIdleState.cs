using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.YM.Game
{
    public class YearXwkIdleState : MonsterBaseState
    {
        public YearXwkIdleState(RoleMgrBase yearMonsterMgrBase) : base(yearMonsterMgrBase)
        {
            PlayAnim("public/anim/wukong/wukong_ty_stand01#anim", PlayOver);
        }
        protected override void OnInit()
        {
            base.OnInit();
        }
        public override void OnEnter()
        {
            base.OnEnter();

        }
        public void PlayOver()
        {
            //循环动作
        }
        public override void OnStop()
        {
            base.OnStop();
        }
    }

}

