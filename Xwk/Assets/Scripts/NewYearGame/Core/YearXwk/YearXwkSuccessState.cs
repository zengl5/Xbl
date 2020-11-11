using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.YM.Game
{
    public class YearXwkSuccessState : MonsterBaseState
    {
        private C_Event _GameEvent = new C_Event();
        private BoxCollider _BoxCollider;
        private GameObject _Hand;
        private string colliderName = "Bone001 R Hand";
        public YearXwkSuccessState(RoleMgrBase yearMonsterMgrBase) : base(yearMonsterMgrBase)
        {
            InitHand();
            Actor.gameObject.SetActive(false);
            //添加碰撞体
            _BoxCollider = Actor.transform.Find("Bone_Root/Bone001 R Hand").GetAddComponent<BoxCollider>();
            _BoxCollider.center = new Vector3(0f, 0f, 21f);
            _BoxCollider.size = new Vector3(25f,20f,104.3f);
            _BoxCollider.enabled = false;

            m_RoleMgr = yearMonsterMgrBase;
            PlayAnim("public/anim/wukong/wukong_ty_stand01#anim", null);
            if (_GameEvent!=null){
                _GameEvent.UnregisterEvent();
                _GameEvent = null;
            }
            _GameEvent = new C_Event();
            _GameEvent.RegisterEvent(C_EnumEventChannel.Global, "YMGameEvent", (object[] result) => {
                if((YMGameEvet)result[0] == YMGameEvet.YMG_EVENT_GAME_XWK_SUCCESS_START)
                {
                    Actor.gameObject.SetActive(true);
                    m_AllowClick = true;

                    AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_37");
                    Actor.transform.rotation = Quaternion.Euler(Vector3.zero);
                    Actor.transform.position = Vector3.zero;
                    PlayAnim("public/anim/wukong/wukong_ty_win01#anim", ()=> {
                        PlayAnim("public/anim/wukong/wukong_yaoqing01_start#anim", () => {
                            PlayAnim("public/anim/wukong/wukong_yaoqing01_end#anim", () => {
                                PlayAnim("public/anim/wukong/wukong_jizhang01_start#anim", () => {
                                    PlayAnim("public/anim/wukong/wukong_jizhang01_loop#anim", () => {
                                        _Hand.gameObject.SetActive(true);
                                        _BoxCollider.enabled = true;
                                    });
                                });
                            });
                        });
                    });
                }
            });
            // PlayAnim("public/anim/wukong/wukong_ty_stand01#anim", PlayOver);
        }
        protected void InitHand()
        {
            if (_Hand == null)
            {
                _Hand = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/public_effect_shoudianji", true);
            }
            _Hand.transform.position = new Vector3(0,0.64f,2.63f);
            _Hand.transform.rotation = Quaternion.Euler(new Vector3(0,180f,0f));
            _Hand.gameObject.SetActive(false);
        }
        public override void TouchEvent(GameObject obj)
        {
            if (obj!=null && obj.name.Equals(colliderName))
            {
                m_AllowClick = false;

                _BoxCollider.enabled = false;
                PlayAnim("public/anim/wukong/wukong_jizhang01_end#anim", () => {
                    RequestNextState();
                });
            }
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
            if (_GameEvent != null)
            {
                _GameEvent.UnregisterEvent();
                _GameEvent = null;
            }
            if (m_RoleMgr!=null)
            {
                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_GAME_SUCCESS_UI);
                Actor.gameObject.SetActive(false);
                m_RoleMgr.CleanState();
                m_RoleMgr = null;
                if (_Hand!=null)
                {
                    _Hand.gameObject.SetActive(false);
                }
            }
           
        }
    }

}

