using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace YB.YM.Game
{
    public class NewYearCameraHelloState : MonsterBaseState
    {
        private bool showOver = false;

        public NewYearCameraHelloState(RoleMgrBase yearMonsterMgrBase) : base(yearMonsterMgrBase)
        {
            showOver = false;
            m_AllowClick = false;
            m_RoleMgr = yearMonsterMgrBase;
        }
        protected override void OnInit()
        {
            base.OnInit();
            PlayAnim("newyeargame/anim/camera/ns_cam01#anim", PlayOver);
        }
         
        public override void OnEnter()
        {
            base.OnEnter();
        }
        public void PlayOver()
        {
            //循环动作
            //if (m_RoleMgr != null)
            //{
            //    m_RoleMgr.EnterNextState(YMGameEvent.YMG_EVENT_IDLESTATE);
            //    m_RoleMgr = null;
            //}
            if (_CurrentAnimancerState != null)
            {
                _CurrentAnimancerState.OnEnd = null;
            }
        }
    }
}
