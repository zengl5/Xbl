using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.YM.Game
{
    public class YearXwkHelloState : MonsterBaseState
    {
        private bool playSoundOver = false;
        private C_Event _GameEvent = new C_Event();
            
        public YearXwkHelloState(RoleMgrBase yearMonsterMgrBase) : base(yearMonsterMgrBase)
        {
            playSoundOver = false;
            m_RoleMgr = yearMonsterMgrBase;
            Actor.gameObject.SetActive(false);
            if (_GameEvent!=null){
                _GameEvent.UnregisterEvent();
                _GameEvent = null;
            }
            _GameEvent = new C_Event();
            _GameEvent.RegisterEvent(C_EnumEventChannel.Global, "YMGameEvent", (object[] result) => {
                if((YMGameEvet)result[0] == YMGameEvet.YMG_EVENT_SHOW_XWK_HELLO)
                {
                    if (Actor == null)
                    {
                        RequestNextState();
                        return;
                    }
                    AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_16",false,()=> {
                        playSoundOver = true;
                    });
                    
                    Actor.gameObject.SetActive(true);
                    PlayAnim("public/anim/wukong/wukong_ty_into01#anim", ()=> {

                        PlayAnim("public/anim/wukong/wukong_yaoqing01_start#anim", () => {

                            PlayAnim("public/anim/wukong/wukong_yaoqing01_loop#anim", () => {
                                if (playSoundOver)
                                {
                                    PlayAnim("public/anim/wukong/wukong_yaoqing01_end#anim", () => {
                                        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_GAME_START);
                                        RequestNextState();
                                    });
                                }
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

