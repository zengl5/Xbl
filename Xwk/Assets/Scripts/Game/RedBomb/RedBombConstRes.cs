namespace XWK.Common.RedBomb
{
    internal static class RedBombConstRes
    {
        /// <summary>
        /// 喷红包特效
        /// </summary>
        public const string RedBombEffectPath = "public/Hero_Effect/prefab/public_effect_phb";

        /// <summary>
        /// 红包模型
        /// </summary>
        public const string RedBombMeshPath = "public/mesh/jl_00015/public_model_jl_00015#mesh";

        /// <summary>
        /// 红包动画控制器路径
        /// </summary>
        public const string StartAnimatorPath = "game/RedBomb/animatorcontroller/hongbao_start";

        /// <summary>
        /// 结束动画
        /// </summary>
        public const string LoopAniClipName = "jl_00015_hongbao01_loop#anim";

        /// <summary>
        /// 红包Prefab
        /// </summary>
        public const string RedBombPrefabPath = "c_framework/ui/package_ui_prefab/ui_redbomb";

        /// <summary>
        /// 红包结算成功特效
        /// </summary>
        public const string WinEffectPath = "public/Hero_Effect/prefab/ui_public_effect_hbcg";

        /// <summary>
        /// 红包结算失败特效
        /// </summary>
        public const string FailEffectPath = "public/Hero_Effect/prefab/ui_public_effect_hbsb";

        /// <summary>
        /// 红包获得加一特效
        /// </summary>
        public const string ClickEffectPath = "public/Hero_Effect/prefab/ui_public_effect_hbhd";

        /// <summary>
        /// 红包获得金币飞翔特效
        /// </summary>
        public const string CoinFlyEffectPath = "public/Hero_Effect/prefab/ui_public_effect_sj";

        /// <summary>
        /// 贺卡图片路径
        /// </summary>
        public const string BlessingImgPath = "c_framework/ui/package_ui_sprite/redbomb/";

        /// <summary>
        /// 贺卡文字语音路径
        /// </summary>
        public const string BlessingAudioPath = "newyeargame/sound/game/";

        /// <summary>
        /// 开场语音
        /// </summary>
        public static readonly string[] OpenAudio = new string[3]
        {
            "xwk_hd_hb_3",//我是红包精灵，发红包是我的最爱！
            "xwk_jlej_41",//快来跟我玩吧，我有好多好多红包哦！
            "xwk_hd_hb_1"//来了来了，下红包雨喽~
        };

        /// <summary>
        /// 成功的结算界面，红包转为灵气
        /// </summary>
        public const string WinAudio = "xwk_hd_hb_4";//红包红包，我爱红包，哈哈哈哈

        /// <summary>
        /// 失败的结算界面，红包裂开
        /// </summary>
        public const string FailAudio = "xwk_hd_hb_5";//唉，我们错过了大红包，呜呜呜呜~

        /// <summary>
        /// 出现 3 个祝福红包
        /// </summary>
        public const string CardAppearAudio = "xwk_hd_hb_6";//这里出现了 3 个大红包，我们点点看吧！

        /// <summary>
        /// 获得贺卡
        /// </summary>
        public const string CardGetAudio = "xwk_hd_hb_7";//哇！我们获得了新年签！

        /// <summary>
        /// 音效路径
        /// </summary>
        public const string SoundEffectPath = "game/RedBomb/SoundEffect/";
        
        /// <summary>
        /// 红包出现音效循环播
        /// </summary>
        public const string AppearSoundEffect = "public_xwkyx_094";

        /// <summary>
        /// 红包结算成功音效
        /// </summary>
        public const string WinSoundEffect = "public_xwkyx_095";

        /// <summary>
        /// 红包结算失败音效
        /// </summary>
        public const string FailSoundEffect = "public_xwkyx_096";
        /// <summary>
        /// 点击音效
        /// </summary>
        public const string ClickSoundEffect = "public_xwkyx_068";
        /// <summary>
        /// 开红包音效
        /// </summary>
        public const string OpenSoundEffect = "public_xwkyx_100";


        /// <summary>
        /// 音乐路径
        /// </summary>
        public const string SoundPath = "game/RedBomb/Sound/";

        /// <summary>
        /// 背景音乐
        /// </summary>
        public const string BGM = "public_xwkbgm_009";
    }
}