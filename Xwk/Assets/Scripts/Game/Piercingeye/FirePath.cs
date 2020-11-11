using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;

/// <summary>
/// 火眼金睛路径类
/// </summary>
public class FirePath:C_Singleton<FirePath>
{
    #region#特效路径
    public  string Effect_wkcx_fp = "public/Hero_Effect/prefab/Effect_wkcx";
     
    public  string effect_hyjj_3heizhao = "public/Hero_Effect/prefab/effect_hyjj_3heizhao";
    public  string effect_hyjj_3xl = "public/Hero_Effect/prefab/effect_hyjj_3xl";

    public  string effect_hyjj_3zshou = "public/Hero_Effect/prefab/effect_hyjj_3zshou";
    public  string effect_hyjj_3yshou = "public/Hero_Effect/prefab/effect_hyjj_3yshou";

    public  string effect_hyjj_5sdx = "public/Hero_Effect/prefab/effect_hyjj_5sdx";
    public  string effect_hyjj_5heizhao = "public/Hero_Effect/prefab/effect_hyjj_5heizhao";
    public  string effect_hyjj_5_yanjing = "public/Hero_Effect/prefab/effect_hyjj_5_yanjing";
    public  string effect_hyjj_5shou = "public/Hero_Effect/prefab/effect_hyjj_5shou";
    public  string effect_hyjj_3yanjing = "public/Hero_Effect/prefab/effect_hyjj_3yanjing";

    public  string effect_hyjj_2heti = "public/Hero_Effect/prefab/effect_hyjj_2heti";
    public string UICover_effect_jdt = "public/Hero_Effect/prefab/UICover_effect_jdt";     
    public  string public_effect_dianji_hyjj = "public/Hero_Effect/prefab/public_effect_dianji_hyjj";
    public  string ui_hyjj_beijing = "public/Hero_Effect/prefab/ui_hyjj_beijing";
    public string UICover_jiantouzhiyin = "public/Hero_Effect/prefab/UICover_jiantouzhiyin";
     

    public string ui_public_effect_suoding = "public/Hero_Effect/prefab/ui_public_effect_suoding";//锁定精灵
    public string ui_public_effect_mengban = "public/Hero_Effect/prefab/ui_public_effect_mengban";//镜头变色和蒙版
    public string public_effect_yan02 = "public/Hero_Effect/prefab/public_effect_yan02";//精灵的烟
    public string public_effect_tishi = "public/Hero_Effect/prefab/public_effect_tishi";//精灵的烟

     
    //ui_public_effect_bao
    public string ui_public_effect_bao = "public/Hero_Effect/prefab/ui_public_effect_bao";//法阵圈    
    public string ui_public_effect_fzxs = "public/Hero_Effect/prefab/ui_public_effect_fzxs";//法阵圈     
    public string ui_public_effect_jlcx = "public/Hero_Effect/prefab/ui_public_effect_jlcx";//法阵圈 
    public string ui_public_effect_jljs = "public/Hero_Effect/prefab/ui_public_effect_jljs";//法阵圈 

    public string ui_public_effect_xyb = "public/Hero_Effect/prefab/ui_public_effect_xyb";//退出特效

    public string public_effect_shoudianji = "public/Hero_Effect/prefab/public_effect_shoudianji";//手点击

    public string UI_effect_CA = "game/Piercingeye/prefabs/UI_effect_CA";//手点击

    #endregion
    #region#声音路径
    public string common105_fp = "game/Piercingeye/sound/common_105";
    public  string common106_fp = "game/Piercingeye/sound/common_106";
    public  string common107_fp = "game/Piercingeye/sound/common_107";
    public  string common108_fp = "game/Piercingeye/sound/common_108";
    public  string common109_fp = "game/Piercingeye/sound/common_109";
    public  string common110_fp = "game/Piercingeye/sound/common_110";
    public  string common111_fp = "game/Piercingeye/sound/common_111";
    public  string common112_fp = "game/Piercingeye/sound/common_112";
    public  string common113_fp = "game/Piercingeye/sound/common_113";
    public string common115_1_fp = "game/Piercingeye/sound/common_115_1";

    public string xln_fp = "game/Piercingeye/sound/xzlz_116";

    #endregion

    #region#模型路径
    public string xwk = "public/mesh/wukong/public_model_wukong#mesh";
    //百音精灵
    public  string jl00002_2 = "public/mesh/jl_00002/public_model_jl00002_2#mesh";//假体
    public  string public_model_jl00002_1 = "public/mesh/jl_00002/public_model_jl00002_1#mesh";//真身
    //百变葫芦
    public string hulu_jiati = "public/mesh/jl_00001/public_model_jl00001_2#mesh";//假体
    public string hulu_zhenshen = "public/mesh/jl_00001/public_model_jl00001_1#mesh";//真身
    //小龙女
    public string xln_jiati = "public/mesh/jl00003_02/public_model_jl00003_02#mesh";
    public string xln_zhenshen = "public/mesh/jl00003/public_model_jl00003_1#mesh";

    public string cam = "game/Piercingeye/prefabs/cam06_01#anim";
    public  string ui= "game/Piercingeye/ui/PiercingeyeCanvas";
    #endregion

    #region#控制器
    public  string firexwk = "game/Piercingeye/animatorcontroller/firexwk";
    public  string firecam = "game/Piercingeye/animatorcontroller/firecam";
    public  string finishbyjl = "game/Piercingeye/animatorcontroller/finishbyjl";
    public string byjljiati = "game/Piercingeye/animatorcontroller/byjljiati";

    public string finishhulu = "game/Piercingeye/animatorcontroller/hulu/finishhulu";
    public string hulujiati = "game/Piercingeye/animatorcontroller/hulu/hulujiati";

    //小龙女
    public string xlnjiati = "game/Piercingeye/animatorcontroller/xln/xlnjiati";
    public string finishxln = "game/Piercingeye/animatorcontroller/xln/finishxln";
    #endregion

    #region#ui
    public string PiercingeyeCanvas = "game/Piercingeye/ui/PiercingeyeCanvas";
    public string  iconui_fp = "game/Piercingeye/ui/icon0";//法阵圈 
    public string dialog_fp = "game/Piercingeye/ui/DialogText";//法阵圈 

    public string dialogTitle_fp = "game/Piercingeye/ui/byjl";//百音精灵
    public string dialogTitle_hulu = "game/Piercingeye/ui/bbhl";//百变葫芦

    public string FazhenCanvas = "game/Piercingeye/ui/FazhenCanvas";//法阵圈 
    public string EffectUICam = "game/Piercingeye/prefabs/EffectUICamera";//法阵圈 
    #endregion


}
/// <summary>
/// 精灵状态管理类
/// </summary>
public class FireSpriteData
{  
    public string Name = "";
    public string titlePath;//标题路径
    public string lockCenterName;

    public string releaseName;
    public string releaseCenterName;

    public string upGradeTexName;
    public int starLevel = 0;//星星等级
    public int spLvel = 0;
    public int nowRoleState = 1;//当前角色等级状态
    public float spExp = 0;//经验值

    public bool lockIcon = false;//解锁
    public bool stayCenter = false;
    public bool upgrade = false;
    public bool isNullState = false;
    public bool fullLingqi = false;
}