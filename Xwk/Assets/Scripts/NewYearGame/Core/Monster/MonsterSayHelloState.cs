using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.AnimCallbacks;

namespace YB.YM.Game
{
    public class MonsterSayHelloState : MonsterBaseState
    {
        private bool showOver = false;
        private GameObject effectbody;
        private C_Event _GameEvent;
        public MonsterSayHelloState(RoleMgrBase yearMonsterMgrBase) : base(yearMonsterMgrBase)
        {
            showOver = false;
            m_AllowClick = false;
            m_RoleMgr = yearMonsterMgrBase;
            if (_GameEvent!=null)
            {
                _GameEvent.UnregisterEvent();
            }
            _GameEvent = new C_Event();
            _GameEvent.RegisterEvent(C_EnumEventChannel.Global, "YMGameEvent",(object[]b)=> {
                if (YMGameEvet.YMG_EVENT_GAME_START==(YMGameEvet)b[0])
                {
                    RequestNextState();
                    _GameEvent.UnregisterEvent();
                }
            });
        }
        protected override void OnInit()
        {
            base.OnInit();
            effectbody = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/public_effect_nscc_zttx",true);
            effectbody.transform.SetParent(Actor.transform);
            effectbody.transform.localPosition = Vector3.zero;
;
            PlayAnim("public/anim/jl_00014/jl_00014_cam01#anim", PlayOver);
          //  _AnimationClip.BindCallback(m_RoleMgr.getActor(), 454f, StartShowXWK);
            _AnimationClip.BindCallback(m_RoleMgr.getActor(), 15.04f, StartShowXWK);
            AudioManager.Instance.PlayEffectAutoClose("newyeargame/sound/game/xwk_hd_ns_1_1");
        }
        void StartShowXWK()
        {
            if (showOver)
            {
                return;
            }
            showOver = true;

          // _AnimationClip.UnbindCallback(m_RoleMgr.getActor(), 454f, StartShowXWK);
            _AnimationClip.UnbindCallback(m_RoleMgr.getActor(), 15.04f, StartShowXWK);
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_SHOW_XWK_HELLO);
        }
        public override void OnEnter()
        {
            base.OnEnter();
        }
        public void PlayOver()
        {
            PlayAnim("public/anim/jl_00014/jl_00014_stand01#anim",null);
        }
        public void RequestNextState()
        {
            //循环动作
            if (m_RoleMgr != null)
            {
                m_RoleMgr.EnterNextState(YMGameEvent.YMG_EVENT_IDLESTATE);
                m_RoleMgr = null;
            }
            if (_CurrentAnimancerState != null)
            {
                _CurrentAnimancerState.OnEnd = null;
            }
        }
        public override void OnStop()
        {
            AudioManager.Instance.StopEffectByKey("newyeargame/sound/game/xwk_hd_ns_1_1");

            if (_GameEvent!=null)
            {
                _GameEvent.UnregisterEvent();
            }
            base.OnStop();
            if (!showOver)
            {
                StartShowXWK();
                if (effectbody!=null)
                {
                    GameObject.DestroyObject(effectbody.gameObject);
                    effectbody = null;
                }
            }
        }
    }
}
