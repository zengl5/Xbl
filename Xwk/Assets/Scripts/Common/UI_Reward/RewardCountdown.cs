using Assets.Scripts.C_Framework;
using System;
using UnityEngine;
using UnityEngine.UI;
using YB.XWK.MainScene;

namespace XWK.Common.UI_Reward
{
    /// <summary>
    /// 倒计时类型
    /// </summary>
    internal enum CountdownType
    {
        Normal,
        ASR,
    }

    internal class RewardCountdown : MonoBehaviour
    {
        /// <summary>
        /// 倒计时进度条
        /// </summary>
        protected Image _ProgressBar;

        #region 成员变量

        /// <summary>
        /// 倒计时时长
        /// </summary>
        protected int _WaitTime = 5;

        /// <summary>
        /// 经过的时长
        /// </summary>
        protected int _PassedTime = -1;

        /// <summary>
        /// 超时回调
        /// </summary>
        protected Action _OvertimeCallback = null;

        protected bool _StartTag = false;
        protected bool _SuccessTag = false;
        protected bool _OverTimeTag = false;

        protected string _EventName = "";

        #endregion 成员变量

        #region 外部调用

        public void InitCountdown(MotionType motionType, int waitTime, Action actionOvertime)
        {
            RecordMotionType(motionType);
            SetWaitTime(waitTime);
            SetCallback(actionOvertime);
        }

        public virtual void StartCountDown()
        {
            if (_SuccessTag)
            {
                return;
            }
            _StartTag = true;
        }

        public virtual void PauseCountDown()
        {
            _StartTag = false;
        }

        public virtual void ResumeCountDown()
        {
            if (_SuccessTag)
            {
                return;
            }
            _StartTag = true;
        }

        public virtual void ResetRewardUI()
        {
            //子类实现
        }

        public virtual void SetVisible(bool vislble)
        {
            gameObject.SetActive(vislble);
        }

        public virtual void SetSuccess()
        {
            if (_SuccessTag)
            {
                return;
            }
            _StartTag = false;
            _SuccessTag = true;
            if (!string.IsNullOrEmpty(_EventName))
            {
                int time = Math.Max((int)((1.0f - _ProgressBar.fillAmount) * (float)_WaitTime), 0);
#if UNITY_EDITOR
                C_DebugHelper.Log("EventName " + _EventName + " time " + time);
#else
                GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, _EventName, time.ToString());
#endif
            }
            SetVisible(false);
        }

        #endregion 外部调用

        #region 子类调用

        protected virtual void OverTime()
        {
            if (_OverTimeTag || _SuccessTag)
            {
                return;
            }
            _OverTimeTag = true;

#if UNITY_EDITOR
            C_DebugHelper.Log("EventName " + _EventName + " time -1");
#else
            GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, _EventName, "-1");
#endif
            SetVisible(false);
        }

        #endregion 子类调用

        protected void DoCallback()
        {
            if (_OvertimeCallback != null)
            {
                Action action = _OvertimeCallback;
                action();
                _OvertimeCallback = null;
            }
        }

        protected virtual void Awake()
        {
            _ProgressBar = transform.Find("progress/progressbar").GetComponent<Image>();
            _ProgressBar.fillAmount = 1.0f;
        }

        protected virtual void Update()
        {
            if (!_StartTag)
            {
                return;
            }
            if (_ProgressBar != null && _WaitTime > 0)
            {
                _ProgressBar.fillAmount -= Time.deltaTime / _WaitTime;
                if (_ProgressBar.fillAmount <= 0.0f)
                {
                    _StartTag = false;

                    OverTime();

                    DoCallback();
                }
            }
        }

        private void RecordMotionType(MotionType motionType)
        {
            switch (motionType)
            {
                case MotionType.Click:
                    _EventName = LocalData.m_rewardui_countdown_click;
                    break;

                case MotionType.Slide:
                    _EventName = LocalData.m_rewardui_countdown_slide;
                    break;

                case MotionType.SR:
                    _EventName = LocalData.m_rewardui_countdown_sr;
                    break;

                default:
                    C_DebugHelper.LogError("倒计时动作类型错误");
                    break;
            }
        }

        private void SetWaitTime(int waitTime)
        {
            _WaitTime = Math.Min(waitTime, 9);
        }

        private void SetCallback(Action callback)
        {
            _OvertimeCallback = callback;
        }

        protected virtual void OnDestroy()
        {
            SetVisible(false);
        }
    }
}