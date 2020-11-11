using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.YM.Game
{
    public class YearXwkFailState : MonsterBaseState
    {
        private C_Event _GameEvent = new C_Event();
            
        public YearXwkFailState(RoleMgrBase yearMonsterMgrBase) : base(yearMonsterMgrBase)
        {
            m_RoleMgr = yearMonsterMgrBase;
            if (_GameEvent!=null){
                _GameEvent.UnregisterEvent();
                _GameEvent = null;
            }
            _GameEvent = new C_Event();
            _GameEvent.RegisterEvent(C_EnumEventChannel.Global, "YMGameEvent", (object[] result) => {
                if((YMGameEvet)result[0] == YMGameEvet.YMG_EVENT_GAME_XWK_FAIL_START)
                {
                 //   AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_40");

                    PlayAnim("public/anim/wukong/wukong_ty_shiluo01_start#anim", ()=> {
                        PlayAnim("public/anim/wukong/wukong_ty_shiluo01_loop#anim", () => {
                            PlayAnim("public/anim/wukong/wukong_ty_shiluo01_end#anim", () => {
                                RequestNextState();
                            });
                        });
                    });
                }
            });
            // PlayAnim("public/anim/wukong/wukong_ty_stand01#anim", PlayOver);
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
        public void RequestNextState()
        {
            if (m_RoleMgr!=null)
            {
                m_RoleMgr.EnterNextState(YMGameEvent.YMG_EVENT_IDLESTATE);
                m_RoleMgr = null;
            }
            if (_GameEvent != null)
            {
                _GameEvent.UnregisterEvent();
                _GameEvent = null;
            }
        }
    }

}

