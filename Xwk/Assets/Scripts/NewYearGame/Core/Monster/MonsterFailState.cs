using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.AnimCallbacks;

namespace YB.YM.Game
{
    public class MonsterFailState : MonsterBaseState
    {
        private bool showOver = false;
        private GameObject effectbody;

        public MonsterFailState(RoleMgrBase yearMonsterMgrBase) : base(yearMonsterMgrBase)
        {
            showOver = false;
            m_AllowClick = false;
            m_RoleMgr = yearMonsterMgrBase;
        }
        protected override void OnInit()
        {
            base.OnInit();
            PlayAnim("public/anim/jl_00014/jl_00014_cam03#anim", PlayOver); 
            _AnimationClip.BindCallback(m_RoleMgr.getActor(), 3.25f, DoStartShowXWK);
            AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_4_1");

            effectbody = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/public_effect_nscg", true);
            effectbody.transform.SetParent(Actor.transform);
            effectbody.transform.localPosition = Vector3.zero;
        }
        void DoStartShowXWK()
        {
            if (showOver)
            {
                return;
            }
            showOver = true;

            _AnimationClip.UnbindCallback(m_RoleMgr.getActor(), 3.25f, DoStartShowXWK);
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_GAME_XWK_FAIL_START);

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
                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_GAME_FAIL_QUIT);
            }
        }
        public override void OnStop()
        {
            base.OnStop();
            if(!showOver&&_AnimationClip != null)
                _AnimationClip.UnbindCallback(m_RoleMgr.getActor(), 115f, DoStartShowXWK);
            if (effectbody != null)
            {
                GameObject.DestroyObject(effectbody.gameObject);
                effectbody = null;
            }
        }
    }

}
