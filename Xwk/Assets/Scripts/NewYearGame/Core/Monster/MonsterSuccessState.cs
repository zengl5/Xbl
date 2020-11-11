using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.AnimCallbacks;

namespace YB.YM.Game
{
    public class MonsterSuccessState : MonsterBaseState
    {
        private bool showOver = false;
        private GameObject effectbody;

        public MonsterSuccessState(RoleMgrBase yearMonsterMgrBase) : base(yearMonsterMgrBase)
        {
            showOver = false;
            m_AllowClick = false;
            m_RoleMgr = yearMonsterMgrBase;
        }
        protected override void OnInit()
        {
            base.OnInit();
            effectbody = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/public_effect_nssb", true);
            effectbody.transform.SetParent(Actor.transform);
            effectbody.transform.localPosition = Vector3.zero;

            PlayAnim("public/anim/jl_00014/jl_00014_cam02#anim", PlayOver);
            AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_3_1");
        }
        //void StartShowXWK()
        //{
        //    if (showOver)
        //    {
        //        return;
        //    }
        //    showOver = true;
        //    _AnimationClip.UnbindCallback(m_RoleMgr.getActor(), 454f, StartShowXWK);
        //}
        public override void OnEnter()
        {
            base.OnEnter();
        }
        public void PlayOver()
        {
            if (showOver)
            {
                return;
            }
            showOver = true; 
            if (_CurrentAnimancerState != null)
            {
                _CurrentAnimancerState.OnEnd = null;
                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_GAME_XWK_SUCCESS_START);
            }
            if (effectbody != null)
            {
                GameObject.DestroyObject(effectbody.gameObject);
                effectbody = null;
            }
            if (m_RoleMgr != null)
            {
                Actor.gameObject.SetActive(false);
                m_RoleMgr.CleanState();
                m_RoleMgr = null;
            }
        }
        public override void OnStop()
        {
            base.OnStop();
           
        }
    }
}
