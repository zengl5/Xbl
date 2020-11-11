using Assets.Scripts.C_Framework;

namespace XWK.Common.RedBomb
{
    internal static class RedBombConstStatistic
    {
        /// <summary>
        /// 游戏中 - 用户次均停留时长
        /// </summary>
        public const string redbomb_game_duration = "redbomb_game_duration";

        /// <summary>
        /// 游戏中 - 统计进入红包活动游戏的事件数
        /// </summary>
        public const string redbomb_game_enter = "redbomb_game_enter";

        /// <summary>
        /// 游戏中 - 点击红包的事件数
        /// </summary>
        public const string redbomb_game_click = "redbomb_game_click";

        /// <summary>
        /// 游戏中 - 点击「返回」按钮的事件数
        /// </summary>
        public const string redbomb_game_return = "redbomb_game_return";

        /// <summary>
        /// 游戏结算 - 统计游戏成功事件数
        /// </summary>
        public const string redbomb_win = "redbomb_win";

        /// <summary>
        /// 游戏结算 - 统计游戏失败事件数
        /// </summary>
        public const string redbomb_fail = "redbomb_fail";

        /// <summary>
        /// 祝福红包 - 统计触发祝福红包的事件数
        /// </summary>
        public const string redbomb_card_enter = "redbomb_card_enter";

        /// <summary>
        /// 祝福红包 - 点击「红包」的事件数
        /// </summary>
        public const string redbomb_card_click = "redbomb_card_click";

        /// <summary>
        /// 祝福红包 - 统计触发祝福签的事件数
        /// </summary>
        public const string redbomb_blessing_enter = "redbomb_blessing_enter";

        /// <summary>
        /// 祝福红包 - 点击「保存」按钮的事件数
        /// </summary>
        public const string redbomb_blessing_save = "redbomb_blessing_save";

        /// <summary>
        /// 祝福红包 - 点击「返回」按钮的事件数
        /// </summary>
        public const string redbomb_blessing_return = "redbomb_blessing_return";

        public static void SendDataStatistics(EnumDataStatistics type, string eventParam)
        {
#if UNITY_EDITOR
            C_DebugHelper.Log(string.Concat(type, YB.XWK.MainScene.LocalData.m_redbomb_module, eventParam));
#else
            GameHelper.Instance.SendDataStatistics(type, YB.XWK.MainScene.LocalData.m_redbomb_module, eventParam);
#endif
        }
    }
}