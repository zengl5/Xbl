using UnityEngine;

namespace XWK.Common.UI_Reward
{
    internal class PerformanceOfflineBonus : RewardPerformance
    {
        #region 成员变量

        private RewardStarTrail _RewardStarTrail = null;

        #endregion 成员变量

        protected override void Init()
        {
            _RewardStarTrail = RewardStarTrailManager.CreateStarTrail(TrailType.OfflineBonus, StarsList, MoveEnd, _RealScore);
        }

        public override void SetSuccess()
        {
            if (_SuccessTag)
            {
                return;
            }
            _SuccessTag = true;
            if (_StarNum > 0)
            {
                //base.SetSuccess();//不要彩带
                base.CreateStars();

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

        public override void SetStarStartPos(Vector2 pos)
        {
            if (_RewardStarTrail != null)
            {
                _RewardStarTrail.SetStartPos(pos);
            }
        }
    }
}