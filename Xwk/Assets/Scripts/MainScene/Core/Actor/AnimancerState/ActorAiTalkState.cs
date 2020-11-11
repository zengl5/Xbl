using Animancer;
using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XBL.Core;
using DG.Tweening;
namespace YB.XWK.MainScene
{
    public class ActorAiTalkState : ActorAnimancerState
    {
        public List<QuestionConfigData> _AllActorAnimancerResConfig;

        private bool _TouchFlag = false;
        private int _SpeechTimes = 0;
        private GameObject _LeftUI;
        private GameObject _RightUI;
        private string _MoudleUIPath = "public/hero_effect/prefab/UICover_ai.prefab";
        private string _LeftUiName = "leftui";
        private string _RightUiName = "rightui";
        private string _SpriteRenderName = "effect_sprite_jkb";
        private string _KSpriteRenderName = "effect_sprite_jkb_k";
        private float _CurrentTime = 0f;
        private int _PlayTimes;
        private SpriteRenderer leftRender;
        private SpriteRenderer rightRender;
        private SpriteRenderer k_leftRender;
        private SpriteRenderer k_rightRender;
        private Sequence _Sequence;
        private Sequence _SequenceLeft;
        private Sequence _SequenceRight;
        private bool _Success = false;

        //所有数据
        public QuestionConfigData _ActorAnimancerResConfigAI;
        public  ActorAiTalkState(IActor actorAimMgr) : base(null)
        {
            _Success = false;
            GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, "aitalk","start");
            GameHelper.Instance.SendDataStatistics(EnumDataStatistics.TimeStart, "aitalk", "time");
            this.HasInteractiveState = true;
            _CurrentTime = 0f;
            OnInit(actorAimMgr);
            _PlayTimes =  Random.Range(6,11);
            _SpeechTimes = 0;
            //播放题目
            Play(this, _ActorAnimancerResConfigAI.questionData.actorAnimancerResConfig, ShowOption);
            C_DebugHelper.Log("当前播放question id:"+ _ActorAnimancerResConfigAI.questionid);

        }
        public override void InitResConfig()
        {
            _AllActorAnimancerResConfig = m_ActorMgr.getActorAnimancerInfo();
#if UNITY_EDITOR
            // _ActorAnimancerResConfigAI = _AllActorAnimancerResConfig[1];
             _ActorAnimancerResConfigAI = _AllActorAnimancerResConfig[LocalData.aiQuesid];

#else
            _ActorAnimancerResConfigAI = _AllActorAnimancerResConfig[Random.Range(0, _AllActorAnimancerResConfig.Count)];
#endif
        }
        public void ShowOption()
        {
            //显示ui，注册点击事件,播放角色的
             Play(this, _ActorAnimancerResConfigAI.actorwaitdata.actorAnimancerResConfig, null);
            _TouchFlag = true;
            _LeftUI = GameResMgr.Instance.LoadResource<GameObject>(_MoudleUIPath, true);
            _LeftUI.gameObject.name = _LeftUiName;
            _RightUI = GameResMgr.Instance.LoadResource<GameObject>(_MoudleUIPath, true);
            _RightUI.gameObject.name = _RightUiName;
            BoxCollider leftboxcollider =  _LeftUI.GetAddComponent<BoxCollider>();
            leftboxcollider.center = new Vector3(25.4f,48.5f,0f);
            leftboxcollider.size = new Vector3(117.7f,85.8f,1f);
            leftboxcollider.isTrigger = true;
            BoxCollider rightboxcollider = _RightUI.GetAddComponent<BoxCollider>();
            rightboxcollider.center = leftboxcollider.center;
            rightboxcollider.size = leftboxcollider.size;
            rightboxcollider.isTrigger = true;

            Sprite leftOptionUI = GameResMgr.Instance.LoadResource<Sprite>(_ActorAnimancerResConfigAI.optionlist[0].uipath,true);
            Sprite rightOptionUI = GameResMgr.Instance.LoadResource<Sprite>(_ActorAnimancerResConfigAI.optionlist[1].uipath,true);
            leftRender = Utility.FindChild(_LeftUI.transform, _SpriteRenderName).GetComponent<SpriteRenderer>();
            k_leftRender = Utility.FindChild(_LeftUI.transform, _KSpriteRenderName).GetComponent<SpriteRenderer>();
            rightRender = Utility.FindChild(_RightUI.transform, _SpriteRenderName).GetComponent<SpriteRenderer>();
            k_rightRender = Utility.FindChild(_RightUI.transform, _KSpriteRenderName).GetComponent<SpriteRenderer>();
            leftRender.color = new Color(1,1,1,1);
            rightRender.color = new Color(1,1,1,1);
            leftRender.sprite = leftOptionUI;
            rightRender.sprite = rightOptionUI;
            Transform parent = Utility.FindChild(m_ActorMgr.getActor(), "Bone_Root");
            _LeftUI.transform.SetParent(parent);
            _RightUI.transform.SetParent(parent);
            //左边的图标
            _LeftUI.transform.localPosition = new Vector3(-52.9f, 4f, 104f);
            _LeftUI.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
            _LeftUI.transform.localScale = new Vector3(-1f, 1f, 1f);
            DoBreathUI(_LeftUI.transform, _SequenceLeft);
            //右边的图标
            _RightUI.transform.localPosition = new Vector3(47f, 4f, 104f);
            _RightUI.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
            _RightUI.transform.localScale = new Vector3(1f, 1f, 1f);
            DoBreathUI(_RightUI.transform, _SequenceRight);

            AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_068.ogg");

        }
        public void DoKillAction()
        {
            if(_SequenceLeft!=null)
                _SequenceLeft.Kill();
            if(_SequenceRight != null)
                _SequenceRight.Kill();
            if(_Sequence != null)
                _Sequence.Kill();
            if(rightRender != null)
                rightRender.DOKill();
            if(k_leftRender != null)
                k_leftRender.DOKill();
            if(leftRender != null)
                leftRender.DOKill();
            if(k_rightRender != null)
                k_rightRender.DOKill();
            if(_LeftUI != null)
                _LeftUI.transform.DOKill();
            if(_RightUI != null)
                _RightUI.transform.DOKill();
        }
        public void DoBreathUI(Transform ui , Sequence sequence)
        {
            ui.DOKill();
            sequence.Kill();
            Vector3 originPos = ui.transform.localPosition;
            Vector3 originScale = ui.transform.localScale;
            Vector3 targetScale = new Vector3(originScale.x / Mathf.Abs(originScale.x) * 18.6f / 18f, 18.6f / 18f, 18.6f / 18f);
            Vector3 targetPos = new Vector3(originPos.x+0.8f, originPos.y+2f, originPos.z);
            sequence = DOTween.Sequence();
            float time = 0.75f;
            sequence.Join(ui.DOLocalMove(targetPos, time))
              .Join(ui.DOScale(targetScale, time))
              .Insert(time, ui.DOLocalMove(originPos, time))
              .Insert(time, ui.DOScale(originScale, time))
              .SetLoops(-1);
             
        }
        public void TouchUIAction(Transform ui)
        { 
            float scale = 18f;
            Vector3 originScale = ui.transform.localScale;
            Vector3 targetScale1 = new Vector3(originScale.x / Mathf.Abs(originScale.x) * 21.8f / scale, 21.8f / scale, 21.8f / scale);
            Vector3 targetScale2 = new Vector3(originScale.x / Mathf.Abs(originScale.x) * 16.81f / scale, 16.81f / scale, 16.81f / scale);
            Vector3 targetScale3 = new Vector3(originScale.x / Mathf.Abs(originScale.x) * 19.62f / scale, 19.62f / scale, 19.62f / scale);
            _Sequence = DOTween.Sequence();
            _Sequence.Append(ui.DOScale(targetScale1, 0.2f))
             .Append(ui.DOScale(targetScale2, 0.2f))
             .Append(ui.DOScale(originScale, 0.2f));
        }
        public void HideUI(Transform ui,SpriteRenderer render,SpriteRenderer krender, System.Action callback)
        {
            Vector3 originPos = ui.transform.localPosition;
            Vector3 targetPos = new Vector3(originPos.x, originPos.y + 2f, originPos.z);
            _Sequence = DOTween.Sequence();
            _Sequence.Join(ui.DOLocalMove(targetPos, 0.5f))
                .Join(render.DOColor(new Color(1,1,1,0),0.5f))
                .Join(krender.DOColor(new Color(1, 1, 1, 0), 0.5f))
                .OnComplete(()=> { if (callback != null) callback(); });
        }
        public override void OnUpdate()
        {
            
            base.OnUpdate();
            if (!_TouchFlag)
            {
                return;
            }
            _CurrentTime += Time.deltaTime;
            if (_CurrentTime > 5.0f)
            {
                _TouchFlag = false;
                RequestNextState();
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
                        if (obj != null )
                        {

                            _CurrentTime = 0f;
                            if (obj.name.Equals(_LeftUiName))
                            {
                                AudioManager.Instance.PlayEffectAutoClose("public/sound/public_sd_042.ogg");

                                DoKillAction();
                                TouchUIAction(_LeftUI.transform);
                                _TouchFlag = false;
                                HideUI(_RightUI.transform, rightRender, k_rightRender, null);

                                Play(this, _ActorAnimancerResConfigAI.optionlist[0].actorAnimancerResConfig, ()=> {
                                    _ActorAnimancerResConfigAI = FetchAnimancerResByKey(FetchNextQuestionId(FetchNextIdByResId(_ActorAnimancerResConfigAI, _ActorAnimancerResConfigAI.optionlist[0])));
                                    PlayNextQuestion(_ActorAnimancerResConfigAI);
                                });

                            }
                            if (obj.name.Equals(_RightUiName))
                            {
                                AudioManager.Instance.PlayEffectAutoClose("public/sound/public_sd_042.ogg");

                                DoKillAction();
                                _TouchFlag = false;
                                TouchUIAction(_RightUI.transform);
                                HideUI(_LeftUI.transform, leftRender, k_leftRender, null);

                                Play(this, _ActorAnimancerResConfigAI.optionlist[1].actorAnimancerResConfig, () => {
                                    _ActorAnimancerResConfigAI = FetchAnimancerResByKey(FetchNextQuestionId(FetchNextIdByResId(_ActorAnimancerResConfigAI, _ActorAnimancerResConfigAI.optionlist[1])));
                                    PlayNextQuestion(_ActorAnimancerResConfigAI);
                                });
                            }
                        }
                    }
                }
            }
        }
        public void PlayNextQuestion(QuestionConfigData data)
        {
            DoKillAction();
            HideUI(_LeftUI.transform, leftRender, k_leftRender, null);
            HideUI(_RightUI.transform,rightRender,k_rightRender,()=> { DestoryUI(); });

            _SpeechTimes++;
            if (_SpeechTimes > _PlayTimes)
            {
                GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, "aitalk", "success");
                _Success = true;
                RequestNextState(); 
            }
            else
            {
                Play(this, data.questionData.actorAnimancerResConfig, ShowOption);
                C_DebugHelper.Log("当前播放question id:" + data.questionid);
            }
        }
        public QuestionConfigData FetchAnimancerResByKey(string id)
        {
            QuestionConfigData data = null;
            if (_AllActorAnimancerResConfig == null) return null;
            for (int i = 0; i < _AllActorAnimancerResConfig.Count; i++)
            {
                if (_AllActorAnimancerResConfig[i].questionid.Equals(id))
                {
                    data = _AllActorAnimancerResConfig[i];
                    break;
                }
            }

            return data;
        }
        public string FetchNextQuestionId(string nextid)
        {
            string data = "";
            if (!string.IsNullOrEmpty(nextid))
            {
                string[] nextdata = nextid.Split(',');
                data = nextdata[Random.Range(0, nextdata.Length)];
                C_DebugHelper.Log("选择的下一套题question_id：" + data);
            }
            return data;
        }
        public string FetchNextIdByResId(QuestionConfigData data, ResConfigData config)
        {
            string result = "";
            for (int i = 0; i < data.optiondata.Count; i++)
            {
                if (data.optiondata[i].id.Equals(config.id))
                {
                    result = data.optiondata[i].nextid;
                    break;
                }
            }
            return result;
        }
        public string FetchCurrentIdByResId(QuestionConfigData data, ResConfigData config)
        {
            string result = "";
            for (int i = 0; i < data.optiondata.Count; i++)
            {
                if (data.optiondata[i].id.Equals(config.id))
                {
                    result = data.optiondata[i].id;
                    break;
                }
            }
            return result;
        }
        public override void RequestNextState()
        {
            m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_IDLE);
        }
        public override void Stop()
        {
            DestoryUI();

            if (_AnimancerComponent!=null)
            {
                _AnimancerComponent.enabled = false;
            }
            _TouchFlag = false;
            base.Stop();
            AudioManager.Instance.StopAllEffect();
            AudioManager.Instance.StopPlayerSound();
            if (!_Success)
            {
                GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, "aitalk", "break");
            }
            GameHelper.Instance.SendDataStatistics(EnumDataStatistics.TimeEnd, "aitalk","time");

        }
        public void DestoryUI()
        {
            DoKillAction();
            if (_RightUI != null)
            {
                GameObject.DestroyObject(_RightUI);
                _RightUI = null;
            }
            if (_LeftUI != null)
            {
                GameObject.DestroyObject(_LeftUI);
                _LeftUI = null;
            }
        }
    }

}
