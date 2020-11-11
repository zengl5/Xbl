using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;

public class SepPath : C_Singleton<SepPath>
{

    #region#特效路径
    public string Effect_fss_sf = "public/Hero_Effect/prefab/Effect_fss_sf";//落地动画（领头悟空）
    public string Effect_fss_chuxian = "public/Hero_Effect/prefab/Effect_fss_chuxian";//分身出现动画
    public string Effect_fss_xiaoshi = "public/Hero_Effect/prefab/Effect_fss_xiaoshi";//分身消失动画
    public string Effect_wkcx = "public/Hero_Effect/prefab/Effect_wkcx";
    public string Effect_clickRight = "public/Hero_Effect/prefab/effect_xiaoshi_big";
    public string Effect_click = "public/Hero_Effect/prefab/effect_xiaoshi_small";
    #endregion
    #region#模型路径
    public string Xwk = "public/mesh/wukong/public_model_wukong#mesh";
    public string Jgb = "public/mesh/jgb/jgb_gm@mesh";
    #endregion
    #region#控制器

    public string CameraAnim = "game/Separation/Model/wukong_bianbianbian@cam";
    public string MainXwk_RuntimeAnimatorController = "game/Separation/animatorcontroller/separtion_create9";//领头悟空所用的控制器
    public string Scene9Sep_RuntimeAnimatorController = "game/Separation/animatorcontroller/separtion_create9";//9个小悟空，入场控制器

    public string xwk_RuntimeAnimatorController = "game/Separation/animatorcontroller/separation_scene";//领头悟空所用的控制器
    public string Jgb_RuntimeAnimatorController = "game/Separation/animatorcontroller/jgbwk";//领头悟空所用的控制器
    #endregion
}
