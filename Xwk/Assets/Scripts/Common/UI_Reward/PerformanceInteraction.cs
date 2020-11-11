using UnityEngine;

namespace XWK.Common.UI_Reward
{
    internal class PerformanceInteraction : RewardPerformance
    {
        private bool _OverTimeTag = false;

        /// <summary>
        /// 倒计时组件
        /// </summary>
        private RewardCountdown _CountdownComponent = null;

        private RewardStarTrail _RewardStarTrail = null;

        #region RewardUI调用

        public override void SetSuccess()
        {
            if (_OverTimeTag)
            {
                return;//预防超时后调用（现阶段是超时就删除this）
            }
            if (_SuccessTag)
            {
                return;
            }
            _SuccessTag = true;

            //设置成功处理倒计时的逻辑
            if (_CountdownComponent != null)
            {
                _CountdownComponent.SetSuccess();
            }
            ClearCountdown();
            base.SetSuccess();

            CreateStars();
            _RewardStarTrail = RewardStarTrailManager.CreateStarTrail(TrailType.Normal, StarsList, MoveEnd, _RealScore);
            if (_RewardStarTrail != null)
            {
                _RewardStarTrail.Play();
            }
        }

        //调用失败删除自身和计时器
        public override void SetFail()
        {
            //停掉倒计时
            PauseCountDown();
            ClearCountdown();
            base.SetFail();
        }

        public override void StartCountDown()
        {
            //重置倒计时状态
            if (_CountdownComponent != null)
            {
                _CountdownComponent.SetVisible(true);
                _CountdownComponent.StartCountDown();
            }
        }

        public override void PauseCountDown()
        {
            if (_CountdownComponent != null)
            {
                _CountdownComponent.PauseCountDown();
            }
        }

        public override void ResumeCountDown()
        {
            if (_CountdownComponent != null)
            {
                _CountdownComponent.ResumeCountDown();
            }
        }

        public override void SetCountdownVisible(bool visible)
        {
            if (_CountdownComponent != null)
            {
                _CountdownComponent.SetVisible(visible);
            }
        }

        #endregion RewardUI调用

        //protected override void Init()
        //{
        //}

        /// <summary>
        /// 选择不同的倒计时类型
        /// </summary>
        protected override void ChooseCountdown(int waittime)
        {
            _CountdownComponent = RewardCountdownManager.CreateCountdown(_MotionType, waittime, Overtime);
        }

        private void ClearCountdown()
        {
            if (_CountdownComponent != null)
            {
                Destroy(_CountdownComponent);
                _CountdownComponent = null;
            }
        }

        //超时
        private void Overtime()
        {
            ClearCountdown();

            _OverTimeTag = true;
            if (!_SuccessTag)
            {
                ResetRewardUI();

                DoCallback(false);
            }

            //删除该组件
            Destroy(gameObject);
            RewardUI.Instance.RemovePerformanceReference(_SerialNumber);
        }

        //删除时调用
        protected override void Clear()
        {
            _RewardStarTrail = null;
            ClearCountdown();
        }

        public override void ResetRewardUI()
        {
            _SuccessTag = false;
            RewardUI.Instance.AwardEffects.GetComponent<ParticleSystem>().Stop();
            RewardUI.Instance.AwardEffects.SetActive(false);
        }
    }
}