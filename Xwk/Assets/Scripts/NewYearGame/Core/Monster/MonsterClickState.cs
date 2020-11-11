using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.YM.Game
{
    /// <summary>
    /// 普通点击
    /// </summary>
    public class MonsterClickState : MonsterBaseState
    {
        private float time = 1;
        private float currentTime =0;
        private bool _PlayOver = false; 
        public MonsterClickState(RoleMgrBase roleMgr) : base(roleMgr)
        {
            m_AllowClick = true;
            _PlayOver = true; 
        }
        protected override void OnInit()
        {
            base.OnInit();
            PlayAnim("public/anim/jl_00014/jl_00014_hit01#anim", DoPlayOver, false);
        }
        void DoPlayOver()
        {
            if (_PlayOver)
            {
                RequestNextState();
            }
        }
        public void MultipleClick()
        {
            _PlayOver = false;
            ClearAnim();

            if (_AnimationClip!=null)
            {
                PlayAnim(_AnimationClip, RequestNextState, false);
            }
            else
            {
                PlayAnim("public/anim/jl_00014/jl_00014_hit01#anim", RequestNextState, false);
            }
        }
     
        public  void RequestNextState()
        { 
            if (m_RoleMgr != null)
                m_RoleMgr.EnterNextState(YMGameEvent.YMG_EVENT_IDLESTATE);
        }

        public override void OnStop()
        {
            base.OnStop();
            ClearAnim();
        }
    }
}

