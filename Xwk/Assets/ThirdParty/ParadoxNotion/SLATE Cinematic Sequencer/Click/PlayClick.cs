using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XWK.Common.UI_Reward;
#if UNITY_EDITOR
using UnityEditor;
#endif
using XBL.Core;

namespace Slate
{
    public enum TouchDir
    {
        eClick,
        eSliderLeft,
        eSliderRight,
        eSliderUp,
        eSliderDown,
        eHitEffect,
    }
     public enum TouchMode
    {
        eOnce,
        eLoop,
    }
    [Name("Play Click")]
    [Attachable(typeof(ClickTrack))]
    [Description("播放点击事件")]
    public class PlayClick : ActionClip
    {
        [SerializeField][Header("是否暂停播放")]
        bool PauseScene = true;
        private GameObject mTarget;
        private Camera _CurrentCamera;
        [Header("被点击对象的tag")]
        public string mTargetTag;
        [HideInInspector]
        public bool mIgnorePause = true;
        [HideInInspector]
        public bool mActiveFlag = false;
        [HideInInspector]
        public Cutscene mRoot;
        [SerializeField][Header("滑动的长度")]
        private float _TouchDeltaLength = 0f;
        private Vector2 _StartTouchpos;
        private Vector2 _MoveTouchpos;
        [Header("操作类型")]
        public TouchDir mTargetTouchDir;
        [SerializeField]
        [Header("创建生成的新对象")]
        private GameObject _Prefab;
        [Header("相机的名字")]
        public string _CameraName;

        [Header("允许滑动的次数")]
        public TouchMode touchMode= TouchMode.eOnce;
        [Header("点击或者滑动后，需要隐藏的对象名字")]
        public string effectName;
        [Header("奖励倒计时的时间配置")]
        public int countDownTime = 5;
        [Header("是否播放滑动或者是点击的音效")]
        public bool playClickSoundFlag = true;
        // Use this for initialization
        void Start()
        {
           
        }
        void OnStart()
        {
            if (TouchDir.eClick == mTargetTouchDir || TouchDir.eHitEffect == mTargetTouchDir )
            {
                if (mTarget == null)
                {
                   // Debug.LogError("目标对象target没有设置，请先设置");
                    return;
                }
                if ( mTarget != null && mTarget.GetComponentInChildren<Collider>() == null)
                {
                   // Debug.LogError(mTarget.name + "没有碰撞体");
                    return;
                }
            }
            _CurrentCamera = DirectorCamera.CurrentCamera(_CameraName);
            if (_CurrentCamera == null)
            {
                //Debug.LogError("相机出错，请检查是否没有相机或者相机个数太多");
                return;
            }
            //显示倒计时ui
            if (TouchDir.eClick== mTargetTouchDir)
            {
                RewardUIManager.Instance.RegisterStory(MotionType.Click, SourceType.Interaction, countDownTime, (b) => {
                    if(!b)
                        TouchEvent();
                });
            }
            else
            {
                RewardUIManager.Instance.RegisterStory(MotionType.Slide, SourceType.Interaction, countDownTime, (b) => {
                    if (!b)
                        TouchEvent();
                });
            }

        }
        void Update()
        {
            if (_CurrentCamera == null)
            {
                 return ;
            }
            if (!mActiveFlag)
            {
                return;
            }
            if (mRoot == null || (mRoot != null && mRoot.isPaused && !mIgnorePause))
            {
                mActiveFlag = false;
                return;
            }
            if (TouchManager.Instance.IsTouchValid(0))
            {
                TouchPhaseEnum phase = (TouchPhaseEnum)TouchManager.Instance.GetTouchPhase(0);
                if (phase == TouchPhaseEnum.BEGAN)
                {
                    TouchManager.Instance.GetTouchPos(0, out _StartTouchpos);
                    if (mTargetTouchDir == TouchDir.eClick || mTargetTouchDir == TouchDir.eHitEffect)
                    {
                        RaycastHit hit;
                        Ray ray;
                        ray = _CurrentCamera.ScreenPointToRay(_StartTouchpos);
                        if (Physics.Raycast(ray, out hit, 1000) && hit.collider != null)
                        {
                            GameObject obj = hit.collider.gameObject;
                            if (obj != null && obj.tag.Equals(mTargetTag))
                            {
                                if (mTargetTouchDir == TouchDir.eHitEffect)
                                {
                                    Instantiate(_Prefab, transform.position, transform.rotation);
                                    mActiveFlag = false;
                                }
                                else if (mTargetTouchDir == TouchDir.eClick)
                                {
                                    if(playClickSoundFlag)
                                        AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_017.ogg");

                                    mActiveFlag = false;
                                    RewardUIManager.Instance.SetSuccess();
                                    TouchEvent();
                                    HideEffect(); 
                                }
                            }
                        }
                    }

                }
                else if (phase == TouchPhaseEnum.MOVED)
                {
                    if (mTargetTouchDir != TouchDir.eClick)
                    {
                        TouchManager.Instance.GetTouchPos(0, out _MoveTouchpos);
                        float distance = Vector3.Distance(_StartTouchpos, _MoveTouchpos);
                        if (IsRightDir(_MoveTouchpos, _StartTouchpos))
                        {
                            if (touchMode == TouchMode.eOnce)
                            {
                                mActiveFlag = false;
                            }
                            if(playClickSoundFlag)
                                AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_043.ogg");

                            TouchEvent();
                            HideEffect();
                            RewardUIManager.Instance.SetSuccess();
                        }
                    }
                }
            }
             
        }
        private void HideEffect()
        {
            if (string.IsNullOrEmpty(effectName))
            {
                return;
            }
            List<CutsceneGroup> groups = CutsceneSequencePlayer._CurrentCutScene.groups;
            int count = groups.Count;
            string reslutName = string.Concat(effectName, "(Clone)");
            for (int i = 0; i < count; i++)
            {
                GameObject actor = groups[i].actor;
                if (actor != null)
                {
                    string name = actor.name;
                    if (name.Equals(reslutName))
                    {
                        actor.gameObject.SetActive(false);
                    }
                }
            }
        }
        bool IsRightDir(Vector2 currentPos,Vector2 startPos)
        {
            bool isRightDir = false;
            float slideLength = 0f;
            if (mTargetTouchDir != TouchDir.eClick)
            {
                //如果方向是一直，则判断滑动的距离是否一直，是则回调
                Vector2 dir = currentPos - startPos;
                if (dir.x > 0 && mTargetTouchDir == TouchDir.eSliderRight)
                {
                    isRightDir = true;
                    slideLength = dir.x;
                }
                if (dir.x < 0 && mTargetTouchDir == TouchDir.eSliderLeft)
                {
                    isRightDir = true;
                    slideLength = dir.x;
                }
                if (dir.y > 0 && mTargetTouchDir == TouchDir.eSliderUp)
                {
                    isRightDir = true;
                    slideLength = dir.y;
                }
                if (dir.y < 0 && mTargetTouchDir == TouchDir.eSliderDown)
                {
                    isRightDir = true;
                    slideLength = dir.y;
                }
              //  Debug.Log("dir.x:" + dir.x + "--mTargetTouchDir:" + mTargetTouchDir+ "--isRightDir:"+ isRightDir);

            }
            else
            {
              //  Debug.Log(" isRightDir = true;");

                isRightDir = true;
            }
           
            if (!isRightDir)
            {
                return false;
            }
            if (Mathf.Abs(slideLength) < Mathf.Abs(_TouchDeltaLength))
            {
                return false;
            }
            return true;
        }
        protected override void OnExit()
        {
            base.OnExit();
            mActiveFlag = false;
          //  StopCoroutine("UpdateEvent");
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            if (PauseScene)
            {
                if(root == null){
                  //  Debug.LogError("cutscene 是空的，先创建cutscene");
                    return;
                }
                root.Pause();
            }
            if (actor != null)
            {
                PlayClick click = actor.GetComponent<PlayClick>();
                if (click == null)
                {
                    click = actor.AddComponent<PlayClick>();
                }
                click.mActiveFlag = true;
                click.mTarget = actor;
                click.mRoot  = parent.root.context.GetAddComponent<Cutscene>(); 
                click.mTargetTag = mTargetTag;
                click.mTargetTouchDir = mTargetTouchDir;
                click._TouchDeltaLength = _TouchDeltaLength;
                click._Prefab = _Prefab;
                click._CameraName = _CameraName;
                click.effectName = effectName;
                click.countDownTime = countDownTime;
                click.playClickSoundFlag = playClickSoundFlag;
                click.OnStart();
              
            }
        }
        void TouchCoroutineHandle()
        {
            
        }
            
        void TouchEvent()
        {
            //没有点击的时候，隐藏提示的手
            if (mActiveFlag)
            {
                HideEffect();
            }

            //如果是暂停则继续播放
            if (mRoot ==null)
            {
              //  Debug.LogError("mRoot is  null");
                return;
            }
            mRoot.Resume();
        }
       
    }
}
