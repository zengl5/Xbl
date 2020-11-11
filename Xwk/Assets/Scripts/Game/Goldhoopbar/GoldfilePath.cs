using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldfilePath : C_Singleton<GoldfilePath>
{
  

    public string Xwk = "public/mesh/wukong/public_model_wukong#mesh";
    //public string Jgb = "public/mesh/jgb/jgb_gm@mesh";
    public string Jgb = "public/mesh/jgb/jgb_gm@mesh";
    public string Camera = "game/Goldhoopbar/prefabs/cam_big1"; 
    //特效
    public string effect_jgb_c3a_chuxian = "public/Hero_Effect/prefab/effect_jgb_c3a_chuxian";//金箍棒移动特效

    public string RecognizeEffectUrl = "public/Hero_Effect/prefab/UICover_shengyin";
    public string Effect_jgb_3_5 = "game/Goldhoopbar/prefabs/effect_jgb_3_5";
    public string effect_jgb_c3a_xh = "public/Hero_Effect/prefab/effect_jgb_c3a_xh";//金箍棒悬浮待机
    //控制器
    public string xwk_animatorcotrl = "game/Goldhoopbar/animatorcontroller/jgbXwk";//领头悟空所用的控制器
    public string xwk_animatorcotrl_guid = "game/Goldhoopbar/animatorcontroller/guidXwk";//领头悟空所用的控制器

    public string XwkControl = "game/Goldhoopbar/animatorcontroller/jgbXwk";
    public string JgbControl = "game/Goldhoopbar/animatorcontroller/jgb";

}
