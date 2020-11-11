using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;

public class HlfilePath : C_Singleton<HlfilePath>
{
    #region#特效路径
    public string effect_xiaoshi_small = "public/Hero_Effect/prefab/effect_xiaoshi_small";
    public string public_effect_yan02 = "public/Hero_Effect/prefab/public_effect_yan02";
    public string public_effect_bbhl_daiji = "public/Hero_Effect/prefab/public_effect_bbhl_daiji";
    public string public_effect_bbhl_xc = "public/Hero_Effect/prefab/public_effect_bbhl_xc";//选错的烟 
    public string public_effect_bbhl_cg = "public/Hero_Effect/prefab/public_effect_bbhl_cg";//选对的烟    
    #endregion
    #region#模型路径
    public string xwk = "public/mesh/wukong/public_model_wukong#mesh";
    public string jl00001_2 = "public/mesh/jl_00001/public_model_jl00001_2#mesh";
    public string jl00001_1 = "public/mesh/jl_00001/public_model_jl00001_1#mesh";
    public string huluTex = "public/mesh/jl_00001/upTex/jl_00001_tx_c_l";
    public string ui = "game/Piercingeye/ui/PiercingeyeCanvas";
    #endregion
    #region#控制器
    public string xwk_cc = "game/BaibianHulu/animatorcontroller/xwk";
    public string hulu = "game/BaibianHulu/animatorcontroller/hulu";
    public string huluren = "game/BaibianHulu/animatorcontroller/huluren";

    #endregion
    public string public_xwkbgm_005 = "game/BaibianHulu/sound/scenesound/public_xwkbgm_005";
}
/// <summary>
/// 卡片数据管理类
/// </summary>
public class DjData
{
    public DjData(string cardsingName, string efSoundName, string cardSoundName, string cdrName)
    {
        signName = cardsingName;
        SoundEfName = efSoundName;
        SoundName = cardSoundName;
        RealName = cdrName;
    }
    public string signName;//标记名字 icon1....
    public string SoundEfName;//卡片音效路径   
    public string SoundName;//卡片声音   
    public string RealName;//真实名字 钢琴
    public Texture2D Texture;
    public Texture2D BgTex;
    public Vector3 FlyTarget;//卡片移动目标点
    public bool IsRight;//当前卡牌是否是正确卡牌
    public int Id;//卡片顺序ID 0,1,2
    public int RightId;//正确卡牌的ID号
}