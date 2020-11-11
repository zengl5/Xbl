using System;

namespace XWK.Common.UI_Reward
{
    internal class RewardCountdownManager : C_Singleton<RewardUIManager>
    {
        public static RewardCountdown CreateCountdown(MotionType motionType, int waitTime, Action actionOvertime)
        {
            RewardCountdown rewardCountdown = null;
            switch (motionType)
            {
                case MotionType.None:
                    break;

                case MotionType.Click:
                case MotionType.Slide:
                    {
                        RewardUI.Instance.CountdownASRGO.SetActive(false);
                        RewardUI.Instance.CountdownNormalGO.SetActive(true);
                        rewardCountdown = RewardUI.Instance.CountdownNormalGO.AddComponent<RewardCountdownNormal>();
                    }
                    break;

                case MotionType.SR:
                    {
                        RewardUI.Instance.CountdownNormalGO.SetActive(false);
                        RewardUI.Instance.CountdownASRGO.SetActive(true);
                        rewardCountdown = RewardUI.Instance.CountdownASRGO.AddComponent<RewardCountdownASR>();
                    }
                    break;

                default:
                    break;
            }

            if (rewardCountdown != null)
            {
                rewardCountdown.InitCountdown(motionType, waitTime, actionOvertime);

                //初始隐藏倒计时动画
                rewardCountdown.SetVisible(false);
            }

            return rewardCountdown;
        }

        protected override void Init()
        {
        }
    }
}