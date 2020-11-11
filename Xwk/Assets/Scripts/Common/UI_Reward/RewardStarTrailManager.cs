using System;
using System.Collections.Generic;
using UnityEngine;

namespace XWK.Common.UI_Reward
{
    public enum TrailType
    {
        Normal,
        OfflineBonus,//离线收益
    }

    internal class RewardStarTrailManager : C_Singleton<RewardStarTrailManager>
    {
        public static RewardStarTrail CreateStarTrail(TrailType trailType, List<RectTransform> starsList, Action playEndAction, int score)
        {
            RewardStarTrail trail = null;
            switch (trailType)
            {
                case TrailType.Normal:
                    trail = new RewardStarTrailNormal(starsList, playEndAction, score);
                    break;

                case TrailType.OfflineBonus:
                    trail = new RewardStarTrailOfflineBonus(starsList, playEndAction, score);
                    break;

                default:
                    break;
            }

            return trail;
        }
    }
}