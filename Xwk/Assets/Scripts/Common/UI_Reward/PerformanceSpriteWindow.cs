namespace XWK.Common.UI_Reward
{
    internal class PerformanceSpriteWindow : RewardPerformance
    {
        #region 成员变量

        private RewardStarTrail _RewardStarTrail = null;

        #endregion 成员变量

        public override void SetSuccess()
        {
            if (_SuccessTag)
            {
                return;
            }
            _SuccessTag = true;
            if (_StarNum > 0)
            {
                base.SetSuccess();
                base.CreateStars();

                _RewardStarTrail = RewardStarTrailManager.CreateStarTrail(TrailType.Normal, StarsList, MoveEnd, _RealScore);
                if (_RewardStarTrail != null)
                {
                    _RewardStarTrail.Play();
                }
            }
            else
            {
                DoCallback(false);
                Destroy(gameObject);
                RewardUI.Instance.RemovePerformanceReference(_SerialNumber);
            }
        }

        //销毁时由父类OnDestroy调用
        protected override void Clear()
        {
            _RewardStarTrail = null;
        }
    }
}