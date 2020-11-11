using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace YB.YM.Game
{
    public class MonsterWoundedState : MonsterBaseState
    {
        private GameObject _effect_skill;
        private GameObject _wound_effect;
        public YMGameWoundedType yMGameWoundedType;
        private C_Event AsrGameEvent;
        private float timeout;
        private Sequence _SequenceIcon;

        public MonsterWoundedState(RoleMgrBase roleMgr, YMGameWoundedType type) : base(roleMgr)
        {
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "SkillUIEvent", 1);

            yMGameWoundedType = type;
            m_AllowClick = false;
            switch (yMGameWoundedType)
            {
                case YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_SIXCLICK:
                    {
                        AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_41");

                        PlayXuanYunEffect();
                         
                        PlayAnim("public/anim/jl_00014/jl_00014_dizz01_start#anim", () => {
                            PlayAnim("public/anim/jl_00014/jl_00014_dizz01_loop#anim", () => {
                                PlayAnim("public/anim/jl_00014/jl_00014_dizz01_end#anim", () => {
                                    NoticeUI();
                                    RequestNextState();
                                }, true);
                            }, true);
                        }, true); 
                    }
                    break;
                case YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_SNOW:
                    {
                        PlayAnim("public/anim/jl_00014/jl_00014_stand01#anim", null);

                        _effect_skill = GameResMgr.Instance.LoadResource<GameObject>("public/Hero_Effect/prefab/public_effect_skill02_xqsj", true);
                        _effect_skill.transform.SetParent(Actor.transform);
                        _effect_skill.transform.localPosition = Vector3.zero;
                        if (_SequenceIcon != null)
                        {
                            _SequenceIcon.Kill();
                        }
                        _SequenceIcon = DOTween.Sequence();
                        _SequenceIcon.AppendInterval(5f).AppendCallback(()=>{
                            PlayXuanYunEffect();

                            PlayAnim("public/anim/jl_00014/jl_00014_dizz02_start#anim", () => {
                                PlayAnim("public/anim/jl_00014/jl_00014_dizz02_loop#anim", () => {
                                    PlayAnim("public/anim/jl_00014/jl_00014_dizz02_end#anim", () => {
                                        NoticeUI();
                                        RequestNextState();
                                        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameBloodEvent", YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_SNOW, 168);
                                    }, true);
                                }, true);
                            }, true);
                        });
                    }
                    break;
                case YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_JGB:
                    {
                       
                        PlayAnim("public/anim/jl_00014/jl_00014_stand01#anim", null);
                        AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_4_1");

                        if (_SequenceIcon != null)
                        {
                            _SequenceIcon.Kill();
                        }
                        _SequenceIcon = DOTween.Sequence();
                        _SequenceIcon.AppendInterval(2.5f).AppendCallback(() =>
                        {
                            _effect_skill = GameResMgr.Instance.LoadResource<GameObject>("public/Hero_Effect/prefab/public_effect_skill01_jgbsj", true);
                            _effect_skill.transform.SetParent(Actor.transform.Find("root"));
                            _effect_skill.transform.localPosition = Vector3.zero;
                            PlayXuanYunEffect();
                            PlayAnim("public/anim/jl_00014/jl_00014_dizz01_start#anim", () => {
                                PlayAnim("public/anim/jl_00014/jl_00014_dizz01_loop#anim", () => {
                                    PlayAnim("public/anim/jl_00014/jl_00014_dizz01_end#anim", () => {
                                        NoticeUI();
                                        RequestNextState();
                                        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameBloodEvent", YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_JGB, 188);
                                    }, true);
                                }, true);
                            }, true);
                        });
                    }
                    break;
                case YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_FSS:
                    {
                        PlayAnim("public/anim/jl_00014/jl_00014_stand01#anim",null);
                        if (AsrGameEvent !=null)
                        {
                            AsrGameEvent.UnregisterEvent();
                        }
                        AsrGameEvent = new C_Event();
                        AsrGameEvent.RegisterEvent(C_EnumEventChannel.Global,"Monster_Wounded_Asr",(object[] reslut)=> {

                            if ((int)reslut[0]==1)
                            {
                                NoticeUI();

                                m_RoleMgr.EnterNextState(YMGameEvent.YMG_EVENT_IDLESTATE);
                                AsrGameEvent.UnregisterEvent();
                            }
                            if ((int)reslut[0] == 2)
                            {
                                _effect_skill = GameResMgr.Instance.LoadResource<GameObject>("public/Hero_Effect/prefab/public_effect_skill04_fsssj", true);
                                _effect_skill.transform.SetParent(Actor.transform);
                                _effect_skill.transform.localPosition = Vector3.zero;
                            }
                            if ((int)reslut[0] == 3)
                            {
                                EnterAsrResult();
                                AsrGameEvent.UnregisterEvent();
                            }
                        });
                    }
                    break;
                case YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_MARROON:
                    {
                        PlayAnim("public/anim/jl_00014/jl_00014_stand01#anim", null);

                        _effect_skill = GameResMgr.Instance.LoadResource<GameObject>("public/Hero_Effect/prefab/public_effect_skill_bpsj",true);
                        _effect_skill.transform.SetParent(Actor.transform);
                        _effect_skill.transform.localPosition = Vector3.zero;
                        if (_SequenceIcon != null)
                        {
                            _SequenceIcon.Kill();
                        }
                        _SequenceIcon = DOTween.Sequence();
                        _SequenceIcon.AppendInterval(1.9f).AppendCallback(() =>
                        {
                            PlayXuanYunEffect();

                            PlayAnim("public/anim/jl_00014/jl_00014_Panic01_start#anim", () => {
                                PlayAnim("public/anim/jl_00014/jl_00014_Panic01_loop#anim", () => {
                                    
                                }, true);
                            }, true);
                        }).AppendInterval(3.5f+0.13f).AppendCallback(()=> {
                                PlayAnim("public/anim/jl_00014/jl_00014_Panic01_end#anim", () => {
                                    NoticeUI();
                                    RequestNextState();
                                    C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameBloodEvent", YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_MARROON, 368);

                                }, true);
                        });

                    }
                    break;
                default: break;
            }
        }
        void PlayXuanYunEffect()
        {
            _wound_effect = GameResMgr.Instance.LoadResource<GameObject>("public/Hero_Effect/prefab/public_effect_xuanyun", true);
            _wound_effect.transform.SetParent(Actor.transform);
            _wound_effect.transform.localPosition = Vector3.zero;
        }
        void DestoryEffect()
        {
            if (_effect_skill != null)
            {
                _effect_skill.transform.SetParent(null);
                GameObject.DestroyObject(_effect_skill);
                _effect_skill = null;
            }
            if (_wound_effect != null)
            {
                _wound_effect.transform.SetParent(null);
                GameObject.DestroyObject(_wound_effect);
                _wound_effect = null;
            }
        }
        public override void OnStop()
        {
            DestoryEffect();
            if (_SequenceIcon != null)
            {
                _SequenceIcon.Kill();
            }
            base.OnStop();
         
            if (AsrGameEvent != null)
            {
                AsrGameEvent.UnregisterEvent();
            }
        }
        void NoticeUI()
        {
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "SkillUIEvent",2);
        }
        void EnterAsrResult()
        {

            if (_SequenceIcon != null)
            {
                _SequenceIcon.Kill();
            }
            _SequenceIcon = DOTween.Sequence();
            _SequenceIcon.AppendInterval(1f).AppendCallback(() =>
            {
                PlayXuanYunEffect();

                PlayAnim("public/anim/jl_00014/jl_00014_dizz01_start#anim", () => {
                    PlayAnim("public/anim/jl_00014/jl_00014_dizz01_loop#anim", () => {
                        PlayAnim("public/anim/jl_00014/jl_00014_dizz01_loop#anim", () => {
                            PlayAnim("public/anim/jl_00014/jl_00014_dizz01_end#anim", () => {
                                NoticeUI();
                                RequestNextState();
                                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameBloodEvent", YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_FSS, 198);
                            }, true);
                        }, true);
                    }, true);
                    }, true);
            });

        }
        public void RequestNextState()
        {
            if (m_RoleMgr==null)
            {
                return;
            }
            if (m_RoleMgr.EnterHenShiStateFlag)
            {
                m_RoleMgr.EnterHenShiStateFlag = false;
                m_RoleMgr.EnterNextState(YMGameEvent.YMG_EVENT_HENSHINSTATE);
            }
            else
            {
                if (Random.Range(0,2) == 1)
                {
                    m_RoleMgr.EnterNextState(YMGameEvent.YMG_EVENT_ATTACKSTATE);
                }
                else
                {
                    m_RoleMgr.EnterNextState(YMGameEvent.YMG_EVENT_IDLESTATE);
                }
            }
        }
    }
}
