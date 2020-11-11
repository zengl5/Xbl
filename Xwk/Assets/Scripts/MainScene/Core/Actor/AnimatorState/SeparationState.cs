using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XBL.Core;
using XWK.Common.UI_Reward;

namespace YB.XWK.MainScene
{
    public class TouchSeparationState : SeparationState {
        public TouchSeparationState(IActor actorAimMgr) : base(actorAimMgr, "touch_separationstate")
        {
            this.ActorStateName = "touch_separationstate";
        }
    } 
    public class SeparationState : ActorAnimancerState
    {
        private GameObject SPEntranceSign;
        private bool _TouchFlag = false;
        private string _TimeMark = "SeparationState";
        public SeparationState(IActor actorAimMgr ):base(null)
        {
            _AnimAncerAimState = null;
            this.ActorStateName = "separationstate";
            Init(actorAimMgr);
        }
        public SeparationState(IActor actorAimMgr,string stateName) : base(null)
        {
            _AnimAncerAimState = null;
            this.ActorStateName = stateName;
            Init(actorAimMgr);
        }
        protected void Init(IActor actorAimMgr)
        {
            _TouchFlag = true;
            this.HasInteractiveState = true;
            OnInit(actorAimMgr);

            //创建图标
            SPEntranceSign = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/UICover_tishi", true);
            Transform parent = Utility.FindChild(m_ActorMgr.getActor(), "Bone_Root");
            SPEntranceSign.transform.SetParent(parent);
            if (m_ActorMgr.getActor().transform.position.x >= m_ActorMgr.m_Camera.transform.position.x)
            {
                SPEntranceSign.transform.localPosition = new Vector3(-52.9f, 4f, 104f);
                SPEntranceSign.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
                SPEntranceSign.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                SPEntranceSign.transform.localPosition = new Vector3(47f, 4f, 104f);
                SPEntranceSign.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
                SPEntranceSign.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            //分身术
            GameObject GC_Tishi = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/effect_sprite_fss", true);
            GC_Tishi.transform.SetParent(SPEntranceSign.transform);
            if (m_ActorMgr.getActor().transform.position.x >= m_ActorMgr.m_Camera.transform.position.x)
            {
                GC_Tishi.transform.localPosition = new Vector3(22.5f, 37.1f, 22.7f);
                GC_Tishi.transform.localScale = new Vector3(20f, 20f, 20f);
            }
            else
            {
                GC_Tishi.transform.localPosition = new Vector3(25.4f, 40.1f, 22.7f);
                GC_Tishi.transform.localScale = new Vector3(-20f, 20f, 20f);
            }

            BoxCollider boxCollider = SPEntranceSign.GetAddComponent<BoxCollider>();
            boxCollider.center = new Vector3(52.4f, 41.64f, 0f);
            boxCollider.size = new Vector3(116.09f, 100f, 1f);

            SPEntranceSign.gameObject.SetActive(true);
            // Play(this);
            Play(this, _ActorAnimancerResConfig, PlayStand);
        }
        public SeparationState(IActor actorAimMgr, AnimState animState) : base(animState)
        {
            Init(actorAimMgr);
            this.state = animState;
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!_TouchFlag)
            {
                return;
            }
            if (TouchManager.Instance.IsTouchValid(0))
            {
                TouchPhaseEnum phase = (TouchPhaseEnum)TouchManager.Instance.GetTouchPhase(0);
                if (phase == TouchPhaseEnum.BEGAN)
                {
                    Vector2 startTouchpos;

                    TouchManager.Instance.GetTouchPos(0, out startTouchpos);
                    RaycastHit hit;
                    Ray ray;
                    ray = m_ActorMgr.m_Camera.ScreenPointToRay(startTouchpos);
                    if (Physics.Raycast(ray, out hit, 1000) && hit.collider != null)
                    {
                        GameObject obj = hit.collider.gameObject;
                        if (obj != null && obj.name.Equals("UICover_tishi(Clone)"))
                        {
                            _TouchFlag = false;

                            //飞出去的动作
                            // EnterOneState("wukong_ty_out01#anim");
                            PlayAnim("public/anim/wukong/wukong_ty_out01#anim");
                            //进入分身术
                            C_TimerMgr.Instance.RemoveTimer(_TimeMark);
                            //不滑动镜头
                            WindowSliderControl.Instance.DFrozenCamera();
                            //不允许点击切换状态
                            HasInteractiveState = false;

                            Quit();
                            AudioManager.Instance.StopBgMusic();
                            m_ActorMgr.Stop();

                            RewardUIManager.GetInstance().RegisterHomePage(5, SourceType.DailyBonus, 5, (b) => {
                                
                            });
                            if (!DailyBounsData.LeaveBouns(DailyBounsName.DailyBouns_Game_fss))
                            {
                                RewardUIManager.GetInstance().SetFail();
                            }
                            else
                            {
                                DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Game_fss);
                                RewardUIManager.GetInstance().SetSuccess();
                            }


                            AudioManager.Instance.PlayerSound("public/sound/common_81.ogg", false, () => {
                                GameObject.Destroy(m_ActorMgr.m_Camera.gameObject);
                                GameObject.Destroy(m_ActorMgr.getActor().gameObject);
                                GameObject.DestroyObject(AudioManager.Instance);
                                LocalData.m_BackToMain = true;
                                YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("Main", "Separation", () => { Utility.SetMainScene("Separation"); });
                            });
                            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_fenshenshu, "dianji_jinru");
                            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeStart, LocalData.m_game_time, LocalData.m_game_fenshenshu);
                        }
                    }
                }
            }
        }
       
        //public override void Play(IActorState target)
        //{
        //    InitAc("main/animationcontroller/spearation");
        //    EnterOneState("wukong_ty_xingfen01_loop#anim");
        //    //出现点击的图标
        //    SPEntranceSign.gameObject.SetActive(true);
            
        //    //common_80
        //    AudioManager.Instance.PlayerSound("public/sound/common_79.ogg",false,()=> {
        //        PlayStand();
        //        //等待三秒进入下一个待机状态随机
        //        C_TimerMgr.Instance.AddTimer(3, () => {
        //            _TouchFlag = false;
        //            RequestNextState();
        //        }, _TimeMark);
        //    });

        //}
        void PlayStand()
        {
            //等待三秒进入下一个待机状态随机
            //  EnterOneState("wukong_ty_stand01#anim");
            PlayAnim("public/anim/wukong/wukong_ty_stand01#anim");
            C_TimerMgr.Instance.AddTimer(3, () => {
                _TouchFlag = false;
                RequestNextState();
            }, _TimeMark);
        }
        public override void RequestNextState()
        {
            m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_IDLE);
        }

        public override void Stop()
        {
            base.Stop();
            if (_AnimAncerAimState != null)
            {
                _AnimAncerAimState.Stop();
                _AnimAncerAimState = null;
            }
            Quit();
        }
        public void Quit()
        {
            AudioManager.Instance.StopPlayerSound();

            C_TimerMgr.Instance.RemoveTimer(_TimeMark);
            if (SPEntranceSign != null)
            {
                GameObject.Destroy(SPEntranceSign);
                SPEntranceSign = null;
            }
        }
    }
}
