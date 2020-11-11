using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XBL.Core;
using XWK.Common.UI_Reward;

namespace YB.XWK.MainScene
{
    public class TouchGoldenCudgelState : GoldenCudgelState {

        public TouchGoldenCudgelState(IActor actorAimMgr) : base(actorAimMgr, "touch_goldencudgestate")
        {
            this.ActorStateName = "touch_goldencudgestate";
        }
    }

    public class GoldenCudgelState : ActorAnimancerState
    {
        protected GameObject GC_Tishi;
        private bool _TouchFlag = false;
        private string _TimeMark = "GoldenCudgelState";
        public GoldenCudgelState(IActor actorAimMgr) : base(null)
        {
            this.ActorStateName = "goldencudgestate";
            Init(actorAimMgr);
        }
        public GoldenCudgelState(IActor actorAimMgr,string stateName) : base(null)
        {
            this.ActorStateName = stateName;
            Init(actorAimMgr);
        }
        protected void Init(IActor actorAimMgr)
        {
            _TouchFlag = true;

            OnInit(actorAimMgr);
            this.HasInteractiveState = true;

            //创建图标
            GC_Tishi = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/UICover_tishi", true);
            Transform parent = Utility.FindChild(m_ActorMgr.getActor(), "Bone_Root");
            GC_Tishi.transform.SetParent(parent);
            if (m_ActorMgr.getActor().transform.position.x >= m_ActorMgr.m_Camera.transform.position.x)
            {
                GC_Tishi.transform.localPosition = new Vector3(-52.9f, 4f, 104f);
                GC_Tishi.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
                GC_Tishi.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                GC_Tishi.transform.localPosition = new Vector3(47f, 4f, 104f);
                GC_Tishi.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
                GC_Tishi.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            //金箍棒
            GameObject GCEntranceSign = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/effect_sprite_jkb", true);
            GCEntranceSign.transform.SetParent(GC_Tishi.transform);
            if (m_ActorMgr.getActor().transform.position.x >= m_ActorMgr.m_Camera.transform.position.x)
            {
                GCEntranceSign.transform.localPosition = new Vector3(21.6f, 42.3f, 22.7f);
                GCEntranceSign.transform.localScale = new Vector3(20f, 20f, 20f);
            }
            else
            {
                GCEntranceSign.transform.localPosition = new Vector3(20f, 42.5f, 22.7f);
                GCEntranceSign.transform.localScale = new Vector3(-20f, 20f, 20f);
            }

            BoxCollider boxCollider = GC_Tishi.GetAddComponent<BoxCollider>();
            boxCollider.center = new Vector3(52.4f, 41.64f, 0f);
            boxCollider.size = new Vector3(116.09f, 100f, 1f);
           
            //出现点击的图标
            GC_Tishi.gameObject.SetActive(true);
            //Play(this);
            Play(this,_ActorAnimancerResConfig,PlayStand);
        }
        public GoldenCudgelState(IActor actorAimMgr, AnimState animState) : base(animState)
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
                           // EnterOneState("wukong_ty_out01#anim");
                            PlayAnim("public/anim/wukong/wukong_ty_out01#anim");
                            //不滑动镜头
                            WindowSliderControl.Instance.DFrozenCamera();
                            //不允许点击切换状态
                            HasInteractiveState = false;
                            m_ActorMgr.Stop();

                            Quit();
                            AudioManager.Instance.StopBgMusic();

                            RewardUIManager.GetInstance().RegisterHomePage(5, SourceType.DailyBonus, 5, (b) => {

                            });
                            if (!DailyBounsData.LeaveBouns(DailyBounsName.DailyBouns_Game_Ggb))
                            {
                                RewardUIManager.GetInstance().SetFail();
                            }
                            else
                            {
                                DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Game_Ggb);
                                RewardUIManager.GetInstance().SetSuccess();
                            }

                            //进入金箍棒
                            AudioManager.Instance.PlayerSound("public/sound/common_118.ogg", false, () =>
                            {
                                GameObject.Destroy(m_ActorMgr.getActor().gameObject);
                                GameObject.Destroy(m_ActorMgr.m_Camera.gameObject);
                                C_MonoSingleton<C_UIMgr>.GetInstance().MandatoryCloseUIAll();
                                LocalData.m_BackToMain = true;
                                YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("Main", "Goldhoopbar", () => { Utility.SetMainScene("Goldhoopbar"); });
                            });
                            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_jingubang, "dianji_jinru");
                            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeStart, LocalData.m_game_time, LocalData.m_game_jingubang);

                        }
                    }
                }
            }
        }

        //public override void Play(IActorState target)
        //{
        //    InitAc("main/animationcontroller/maingoldencudgel");
        //    EnterOneState("wukong_ty_xingfen01_loop#anim");
        //    //出现点击的图标
        //    GC_Tishi.gameObject.SetActive(true);
        //    //等待三秒进入下一个待机状态随机
        //    AudioManager.Instance.PlayerSound("public/sound/common_116.ogg", false, () => {
        //        PlayStand();
        //        // _TouchFlag = true;
        //        C_TimerMgr.Instance.AddTimer(3, () => {
        //            _TouchFlag = false;
        //            RequestNextState();
        //        }, _TimeMark);
        //    });
        //}
        void PlayStand()
        {
            //播放站立动作
            // EnterOneState("wukong_ty_stand01#anim");
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
            if (_AnimAncerAimState!=null)
            {
                _AnimAncerAimState.Stop();
                _AnimAncerAimState = null;
            }
            Quit();
        }
        public void Quit()
        {
            C_TimerMgr.Instance.RemoveTimer(_TimeMark);
            if (GC_Tishi != null)
            {
                GameObject.Destroy(GC_Tishi);
                GC_Tishi = null;
            }
            AudioManager.Instance.StopPlayerSound();
        }
    }
}
