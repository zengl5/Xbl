using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.YM.Game
{
    public class MonsterIdleState : MonsterBaseState
    {
        private float totalTime = 0;
        private float curretnTime = 0;
        public MonsterIdleState(RoleMgrBase yearMonsterMgrBase) : base(yearMonsterMgrBase)
        {
            m_AllowClick = true;
            totalTime = Random.Range(2,5);
            curretnTime = 0;
        }
        protected override void OnInit()
        {
            base.OnInit();
            PlayAnim("public/anim/jl_00014/jl_00014_stand01#anim", PlayOver);
        }
        public override void OnEnter()
        {
            base.OnEnter();
           
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (m_PauseFlag)
            {
                return;
            }
            curretnTime += Time.deltaTime;
            if (curretnTime > totalTime)
            {
                curretnTime = 0;
                //进入到攻击状态
                m_RoleMgr.EnterNextState(YMGameEvent.YMG_EVENT_ATTACKSTATE);
            }
        }
        public void PlayOver()
        {
            //循环动作
        }
        public override void OnStop()
        {
            m_PauseFlag = true;
            curretnTime = 0;
            base.OnStop();
        }
    }

}
