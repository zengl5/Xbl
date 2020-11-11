using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace YB.YM.Game
{
    public class NewYearSuccessState : MonsterBaseState
    {
        private bool showOver = false;

        public NewYearSuccessState(RoleMgrBase yearMonsterMgrBase) : base(yearMonsterMgrBase)
        {
            showOver = false;
            m_AllowClick = false;
            m_RoleMgr = yearMonsterMgrBase;
        }
        protected override void OnInit()
        {
            base.OnInit();
            PlayAnim("newyeargame/anim/camera/ns_cam02#anim", PlayOver);
        }
         
        public override void OnEnter()
        {
            base.OnEnter();
        }
        public void PlayOver()
        {
            if (_CurrentAnimancerState != null)
            {
                _CurrentAnimancerState.OnEnd = null;
            }
        }
    }
}
