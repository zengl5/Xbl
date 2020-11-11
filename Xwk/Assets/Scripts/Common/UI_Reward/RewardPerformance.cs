using Assets.Scripts.C_Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace XWK.Common.UI_Reward
{
    internal class RewardPerformance : MonoBehaviour
    {
        #region 成员变量

        protected uint _SerialNumber = 0;

        protected readonly Dictionary<MotionType, int> _ScoreCfg = new Dictionary<MotionType, int>
        {
            {MotionType.Click, 1},
            {MotionType.Slide, 2},
            {MotionType.SR, 5},
        };

        /// <summary>
        /// 操作类型
        /// </summary>
        protected MotionType _MotionType = MotionType.None;

        /// <summary>
        /// 来源类型
        /// </summary>
        protected SourceType _SourceType = SourceType.None;

        /// <summary>
        /// 结束回调
        /// </summary>
        protected Action<bool> _Callback = null;

        /// <summary>
        /// 自定义分数
        /// </summary>
        protected int _RealScore = 0;

        /// <summary>
        /// 实际星星数量
        /// </summary>
        protected int _StarNum = 0;

        protected bool _SuccessTag = false;

        private List<RectTransform> starsList = new List<RectTransform>();

        protected List<RectTransform> StarsList
        {
            get
            {
                return starsList;
            }
            set
            {
                starsList = value;
            }
        }

        public MotionType GetMotionType()
        {
            return _MotionType;
        }

        public void SetSerialNumber(uint serialNumber)
        {
            _SerialNumber = serialNumber;
        }

        #endregion 成员变量

        #region RewardUI调用

        public void Register(MotionType motion, SourceType sourceType, int waitTime, Action<bool> animaUICallback)
        {
            _MotionType = motion;
            SetParam(sourceType, waitTime, animaUICallback);
        }

        public void Register(int star, SourceType sourceType, int waitTime, Action<bool> animaUICallback)
        {
            _RealScore = star;
            SetParam(sourceType, waitTime, animaUICallback);
        }

        public virtual void SetSuccess()
        {
            //奖励礼花特效
            RewardUI.Instance.AwardEffects.SetActive(true);
            RewardUI.Instance.AwardEffects.GetComponent<ParticleSystem>().Play();
        }

        public virtual void SetFail()
        {
            DoCallback(false);
            Destroy(gameObject);
            RewardUI.Instance.RemovePerformanceReference(_SerialNumber);
        }

        public virtual void StartCountDown()
        {
            //子类实现
        }

        public virtual void PauseCountDown()
        {
            //子类实现
        }

        public virtual void ResumeCountDown()
        {
            //子类实现
        }

        public virtual void ResetRewardUI()
        {
            //子类实现
        }

        protected virtual void SetWaitTime(int waitTime)
        {
            //子类实现
        }

        public virtual void SetCountdownVisible(bool visible)
        {
            //子类实现
        }

        //设置星星初始位置
        public virtual void SetStarStartPos(Vector2 pos)
        {
            //子类实现
        }

        #endregion RewardUI调用

        #region 子类实现，父类调用

        protected virtual void Init()
        {
        }

        protected virtual void Clear()
        {
            //子类实现
        }

        protected virtual void ChooseCountdown(int waittime)
        {
            //子类实现
        }

        #endregion 子类实现，父类调用

        private void SetParam(SourceType sourceType, int waittime, Action<bool> animaUICallback)
        {
            _SourceType = sourceType;

            _Callback = animaUICallback;

            ChooseCountdown(waittime);

            if (_MotionType == MotionType.None && _RealScore > 0)
            {
                _StarNum = _RealScore;
            }
            else if (_MotionType != MotionType.None)
            {
                _StarNum = _ScoreCfg[_MotionType];
            }

            _RealScore = _StarNum;
            _StarNum = Math.Min(_StarNum, 5);//限制星星数量

            Init();
        }

        protected void CreateStars()
        {
            if (_StarNum <= 0)
            {
                C_DebugHelper.LogError("分数设置错误");
            }

            GameObject star = null;
            RectTransform rectTransform = null;
            for (int i = 0; i < _StarNum; i++)
            {
                star = Instantiate(RewardUI.Instance.StarPrefab, RewardUI.Instance.Canvas);
                rectTransform = star.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(0, -800);
                star.SetActive(true);

                StarsList.Add(rectTransform);
            }
        }

        protected void MoveEnd()
        {
            DoCallback(true);

            Destroy(gameObject);
            RewardUI.Instance.RemovePerformanceReference(_SerialNumber);
        }

        /// <summary>
        /// 执行外部传入的回调
        /// </summary>
        /// <param name="result"></param>
        protected void DoCallback(bool result)
        {
            if (_Callback != null)
            {
                Action<bool> action = _Callback;
                action(result);
                _Callback = null;
            }
        }

        protected void ClearStars()
        {
            if (StarsList != null && StarsList.Count > 0)
            {
                for (int i = 0; i < StarsList.Count; i++)
                {
                    //TODO  Editor中 StarsList[i].gameObject 有为 null 的情况
                    if (StarsList[i] && StarsList[i].gameObject)
                    {
                        Destroy(StarsList[i].gameObject);
                        StarsList[i] = null;
                    }
                }
                StarsList.Clear();
                StarsList = null;
            }
        }

        protected virtual void OnDestroy()
        {
            Clear();
            ClearStars();
        }
    }
}