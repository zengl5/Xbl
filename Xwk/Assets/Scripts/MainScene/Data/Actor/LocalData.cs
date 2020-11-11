using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class LocalData 
    {
        #region  -----DataStatistics---- start

        public static string m_first_enter_main_time = "first_enter_main";

        public static string m_start_app_time = "start_app";
		
		public static string m_login_app_time = "m_login_app";
		
        public static string m_start_loading_main_time = "m_start_loading_main";

        public static string m_main_event = "main_event";

        public static string m_main_time = "main_time";

        public static string m_game_fenshenshu = "game_fenshenshu";

        public static string m_game_jingubang = "game_jingubang";

        public static string m_game_huoyanjingjing = "game_huoyangjingjing";

        public static string m_game_baiyingjingling = "game_baiyingjingling";

        public static string m_spriteWindow = "spriteWindow";

        public static string m_spWindowStoryGameUi = "spWindowStoryGameUi";

        public static string byjlgameUIclick = "byjlgameUIclick";
        public static string byjlstoryUIclick = "byjlstoryUIclick";

        public static string hulugameUIclick = "hulugameUIclick";
        public static string hulustoryUIclick = "hulustoryUIclick";

        public static string xlnstoryUIclick = "xlnstoryUIclick";



        public static string m_game_baibianhulu = "game_baibianhulu";

        public static string m_game_time = "time_game";

        public static string m_story_time = "story_time";
        //点击主界面云，出现乌云
        public static string m_jindouyun_click = "m_jindouyun_click";
        //点击主界面进入二级界面
        public static string m_main_enter_wizard = "main_enter_wizard";
        //点击主界面云
        public static string m_click_cloud = "main_click_cloud";

        public static string m_click_xwk= "main_click_xwk";

        public static string offlinerevenue = "offlinerevenue";

        public static string spriteUpgrade = "spriteUpgrade";

        public static string m_dailybouns = "DailyBouns";

        #region 灵气奖励统计项
        //点击倒计时
        public static string m_rewardui_countdown_click = "rewardui_countdown_click";
        //滑动倒计时
        public static string m_rewardui_countdown_slide = "rewardui_countdown_slide";
        //语音识别倒计时
        public static string m_rewardui_countdown_sr = "rewardui_countdown_sr";
        #endregion

        //红包玩法
        public static string m_redbomb_module = "redbomb_module";

        #endregion  -----DataStatistics---- end

        public static bool m_FirstEnterApp = true;

        public static bool m_FirstOpenChestUI = true;

        public static bool m_FirstEnterMain = true;

        public static bool m_SpiritGameMode = true; 
        //精灵的名字
        public static string m_SpiritType= "jl_xln";//baiyin  /   bbhulu  /jl_xln  /jl_lingwa  /jl_xcww  /jl_hxjl

        public static string m_story_moudle;

        public static int ai_time = 0;

        public static bool m_BackToMain = true;

        public static string m_Collect_Spirit_Prefs = "main_collect_spirit_name";

        public static bool GotoStoryOrGameBy_SpWindow = false;//进入游戏或者故事通过二级界面

        public static int RoleLevel = 0;
         
        public static int aiQuesid = 36;
         
        public static float monsterGameleaveTime =  5*60f;
    }
}

