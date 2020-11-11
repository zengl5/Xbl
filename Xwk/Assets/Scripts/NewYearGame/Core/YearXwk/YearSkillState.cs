using Assets.Scripts.C_Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.YM.Game
{
    public class YearSkillState : MonsterBaseState
    {
        private GameObject _effect_skill;
        private GameObject _effect_skill_fss_sf;
        private GameObject _effect_skill_fss_yan;
        private GameObject _jgb;
        public YMGameWoundedType yMGameWoundedType;
        private Sequence _SequenceIcon;
        private C_Event GameEvent;
        private bool _RecognizeFlag = false;

        public YearSkillState(RoleMgrBase roleMgr, YMGameWoundedType type) : base(roleMgr)
        {
            _RecognizeFlag = false;
            if (GameEvent != null)
            {
                GameEvent.UnregisterEvent();
            }
            GameEvent = new C_Event();
            GameEvent.RegisterEvent(C_EnumEventChannel.Global, "SkillUIEvent", (object[] reslut) => {

                if ((int)reslut[0] == 2)
                {
                    GameEvent.UnregisterEvent();

                    RequestNextState();
                    m_RoleMgr.EnterNextState(YMGameEvent.YMG_EVENT_IDLESTATE);
                }
            });
            yMGameWoundedType = type;
            m_AllowClick = false;
            switch (yMGameWoundedType)
            { 
                case YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_SNOW:
                    {
                        AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_22_1");

                        PlayAnim("newyeargame/anim/wukong/wukong_ns_skill02#anim", () => {
                             PlayIdle();
                        }, true);
                    }
                    break;
                case YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_JGB:
                    {
                        _effect_skill_fss_sf = GameResMgr.Instance.LoadResource<GameObject>("newyeargame/anim/jgb/ny_model_jgb_dm_full@mesh", true);
                        _effect_skill_fss_sf.transform.SetParent(Actor.transform);
                        _effect_skill_fss_sf.transform.localPosition = Vector3.zero;
                        _effect_skill_fss_sf.transform.localRotation = Quaternion.Euler(Vector3.zero);
                        _effect_skill_fss_sf.gameObject.SetActive(true);

                        AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_20_1");

                        PlayAnim("newyeargame/anim/wukong/wukong_ns_skill01#anim", () => {
                             PlayIdle();
                        }, true);
                    }
                    break;
                case YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_FSS:
                    {
                        _RecognizeFlag = true;
                        string[] word = { "新年快乐",
                                            "恭喜发财",
                                            "大吉大利",
                                            "万事如意",
                                            "心想事成",
                                            "招财进宝",
                                            "学业有成",
                                            "福星高照",
                                            "吉祥如意"
                                            };
                        string[] audio = { "newyeargame/sound/game/xwk_hd_ns_24",
                                        "newyeargame/sound/game/xwk_hd_ns_25",
                                        "newyeargame/sound/game/xwk_hd_ns_26",
                                        "newyeargame/sound/game/xwk_hd_ns_27",
                                        "newyeargame/sound/game/xwk_hd_ns_28",
                                        "newyeargame/sound/game/xwk_hd_ns_29",
                                        "newyeargame/sound/game/xwk_hd_ns_30",
                                        "newyeargame/sound/game/xwk_hd_ns_31",
                                        "newyeargame/sound/game/xwk_hd_ns_32"
                                        };
                        int id = Random.Range(0, audio.Length);
                        PlayAnim("public/anim/wukong/wukong_ty_talk01_start#anim",()=> {
                            PlayAnim("public/anim/wukong/wukong_ty_talk01_loop#anim",null);
                            AudioManager.Instance.PlayerSound("public/sound/common_59", false, () =>
                            {
                                AudioManager.Instance.PlayerSound(audio[id], false, () =>
                                {
                                    //说一句话
                                    PlayIdle();
                                    //开启语音识别，根据识别结果处理
                                    SpeechSystemMgr.Instance.StartRecognizeAudioTecent(word[id],(result) =>
                                    {
                                        SpeechSystemMgr.Instance.Stop();
                                        _RecognizeFlag = false;

                                        if (!word[id].Contains(result))
                                        {
                                            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "Monster_Wounded_Asr", 1);
                                            RequestNextState();
                                        }
                                        else
                                        {
                                            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "Monster_Wounded_Asr", 2);
                                            AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_23_1");
                                            EnterFssState();
                                        }
                                    }, RecognizeAudio.ResultType.PickWord);
                                });
                            });
                        });
                    }
                    break;
                case YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_MARROON:
                    {
                        _effect_skill_fss_sf = GameResMgr.Instance.LoadResource<GameObject>("newyeargame/mesh/dj00056/public_model_dj00056#mesh", true);
                        _effect_skill_fss_sf.transform.position = Actor.transform.position;
                        _effect_skill_fss_sf.transform.localRotation =Actor.transform.rotation ;
                        AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_21_1");
                        PlayAnim("newyeargame/anim/wukong/wukong_ns_skill03#anim", () => {
                            PlayIdle();
                          //  RequestNextState();
                        }, true);
                    }
                    break;
                default: break;
            }

        }
        void PlayIdle()
        {
            PlayAnim("public/anim/wukong/wukong_ty_stand01#anim", () => {

            }, true);
        }
        public void EnterFssState()
        {
            _effect_skill_fss_sf = GameResMgr.Instance.LoadResource<GameObject>("public/Hero_Effect/prefab/Effect_fss_sf", true);
            _effect_skill_fss_sf.transform.SetParent(Actor.transform);
            _effect_skill_fss_sf.transform.localPosition = Vector3.zero;
            _effect_skill_fss_sf.transform.localRotation = Quaternion.Euler(Vector3.zero);
            if (_SequenceIcon != null)
            {
                _SequenceIcon.Kill();
            }
            _SequenceIcon = DOTween.Sequence();
            _SequenceIcon.AppendInterval(2.5f).AppendCallback(() => {
                _effect_skill_fss_yan = GameResMgr.Instance.LoadResource<GameObject>("public/Hero_Effect/prefab/public_effect_yan02", true);
                _effect_skill_fss_yan.transform.SetParent(Actor.transform);
                _effect_skill_fss_yan.transform.localPosition = Vector3.zero;
                _effect_skill_fss_yan.transform.localPosition = new Vector3(0,76f,0);
            });

            PlayAnim("public/anim/wukong/wukong_bianbianbian#anim", () => {
                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "Monster_Wounded_Asr", 3);
                //RequestNextState();
                 PlayIdle();
            }, true);
        }
        public override void OnStop()
        {
            if (_RecognizeFlag)
            {
                _RecognizeFlag = false;
                SpeechSystemMgr.Instance.Stop();
            }
            if (GameEvent!=null)
            {
                GameEvent.UnregisterEvent();
            }
            if (_SequenceIcon != null)
            {
                _SequenceIcon.Kill();
                _SequenceIcon = null;
            }
            if (_effect_skill != null)
            {
                _effect_skill.transform.SetParent(null);
                GameObject.DestroyObject(_effect_skill);
                _effect_skill = null;
            }
            if (_effect_skill_fss_sf != null)
            {
                _effect_skill_fss_sf.transform.SetParent(null);
                GameObject.DestroyObject(_effect_skill_fss_sf);
                _effect_skill_fss_sf = null;
            }
            if (_effect_skill_fss_yan != null)
            {
                _effect_skill_fss_yan.transform.SetParent(null);
                GameObject.DestroyObject(_effect_skill_fss_yan);
                _effect_skill_fss_yan = null;
            } 
            base.OnStop(); 
        }

        public void RequestNextState()
        {
            if (m_RoleMgr == null)
            {
                return;
            }
            m_RoleMgr.EnterNextState(YMGameEvent.YMG_EVENT_IDLESTATE);
        }
    }
}

