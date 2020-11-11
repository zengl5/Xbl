using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 百音精灵路径
/// </summary>
public class SpfilePath:C_Singleton<SpfilePath>{
    #region#特效路径

    public string public_effect_wkrd = "public/Hero_Effect/prefab/public_effect_wkrd";//入地烟雾  

    public string ui_public_effect_kphj = "public/Hero_Effect/prefab/ui_public_effect_kphj";//卡牌游戏大卡消失汇聚     在大卡移动到角色身上延迟0.3s（时长大概0.5S）

    public string ui_public_effect_xxtw = "public/Hero_Effect/prefab/ui_public_effect_xxtw";//卡牌游戏汇聚的点做位移拖尾    在消失汇聚延迟0.5秒出现。出现后等待0.5秒后用0.5秒时间飞到上面   

    public string ui_public_effect_kpbdbj = "public/Hero_Effect/prefab/ui_public_effect_kpbdbj";//卡牌游戏大卡展示背景  
    
    public string ui_public_effect_xxcx = "public/Hero_Effect/prefab/ui_public_effect_xxcx";//卡牌游戏UI星星出现 


    public string ui_public_effect_dj = "public/Hero_Effect/prefab/ui_public_effect_dj";//卡牌游戏小卡点击  

    public string ui_public_effect_kpjs = "public/Hero_Effect/prefab/ui_public_effect_kpjs";//结算特效  
    #endregion





    #region#模型路径
    public string xwk = "public/mesh/wukong/public_model_wukong#mesh";
    public  string jl00002_2 = "public/mesh/jl_00002/jl_00002_1#mesh";
    public string jlTex= "public/mesh/jl_00002/upTex/jl_00002_tx_c_1_l";

    #endregion
    #region#控制器
    public string spxwk = "game/Sprite/animatorcontroller/spxwk";
    public  string spjl = "game/Sprite/animatorcontroller/spjl";
    #endregion
    #region#ui  
    public string xwkicon = "game/Sprite/ui/xwkicon";
    public string usericon= "game/Sprite/ui/usericon";//编辑器测试
    #endregion
}
/// <summary>
/// 卡片数据管理类
/// </summary>
public class Card
{
    public Card(string cardsingName, string efSoundName, string cardSoundName, string cdrName)
    {
        CardsignName = cardsingName;
        EffectSoundName = efSoundName;
        CardSoundName = cardSoundName;
        CardRealName = cdrName;
    }
    public string CardsignName;//标记名字 icon1....
    public string EffectSoundName;//卡片音效路径   
    public string CardSoundName;//卡片声音   
    public string CardRealName;//真实名字 钢琴
    public Texture2D CardTexture;
    public Texture2D CardBgTex;
    public Vector3 FlyTarget;//卡片移动目标点
    public bool RightCard;//当前卡牌是否是正确卡牌
    public int CardId;//卡片顺序ID 0,1,2
    public int RightCardId;//正确卡牌的ID号
}
public class CardInfo
{
    public CardInfo(RawImage myimage, Text mytext, RawImage myxwkphoto, Image myuserphoto)
    {
        image = myimage;
        text = mytext;
        xwkphoto = myxwkphoto;
        userphoto = myuserphoto;
    }
    public void Hide()
    {
        xwkphoto.transform.gameObject.SetActive(false);
        userphoto.transform.parent.gameObject.SetActive(false);
    }
    public RawImage image;//动物图片
    public Text text;//文字介绍
    public RawImage xwkphoto;//小悟空头像
    public Image userphoto;//用户头像
    public Vector3 firstPos = new Vector3(109, 190, 4.4f);
    public Vector3 secondPos = new Vector3(27, 190, 4.4f);
}
/// <summary>
/// 正确大卡片管理类
/// </summary>
public class TargetBigCard
{
    public TargetBigCard(GameObject card)
    {
        this.targetcard = card;
        targetcard.SetActive(true);
        textureobj = card.transform.GetChild(0).gameObject;
        textobj = card.transform.GetChild(1).gameObject;
    }
    public void InitTargetCard(Card cd)
    {
        this.cd = cd;
        targetcard.GetComponent<RawImage>().texture = cd.CardBgTex;
        textureobj.GetComponent<RawImage>().texture = cd.CardTexture;
        textobj.GetComponent<Text>().text = SpriteManager.RemoveNumber(cd.CardRealName);
        targetcard.transform.localScale = 1.2f * Vector3.one;
        targetcard.transform.localPosition = getInitPos(cd);
    }

    public void MoveToCenter(TweenCallback ac1, TweenCallback ac2)
    {
        targetcard.transform.DOLocalMove(Vector3.zero, 1).OnComplete(
            delegate
            {
                ac1();
                OpenBgEffect();
            }
             );
        targetcard.transform.DOScale(1.8f * Vector3.one, 1).OnComplete(ac2);
    }

    public void CenterToRecycle()
    {
        targetcard.transform.DOLocalMove(recycleTargetpos, 1);
        targetcard.transform.DOScale(Vector3.zero, 1);
    }
    public void CloseEffect()
    {
        if (BgEffect)
            BgEffect.SetActive(false);
    }
    public void OpenBgEffect()
    {
        if (BgEffectParent == null)
        {
            BgEffectParent = GameObject.Find("SpUICanvas").transform;
        }
        //点击特效
        if (BgEffect == null)
        {
            BgEffect = ABResMgr.Instance.LoadResource<GameObject>(SpfilePath.Instance.ui_public_effect_kpbdbj, ABCommonConfig.EfBundleType, true);
            BgEffect.transform.parent = BgEffectParent;
            BgEffect.transform.localPosition = Vector3.zero;
            BgEffect.transform.localEulerAngles = Vector3.zero;
            BgEffect.transform.localScale = Vector3.one;
        }
        else
        {
            BgEffect.SetActive(false);
            BgEffect.SetActive(true);
        }
    }
    Vector3 getInitPos(Card cd)
    {
        switch (cd.CardId)
        {
            case 0:
                return new Vector3(-420, 268, 0);
            case 1:
                return new Vector3(0, 268, 0);
            case 2:
                return new Vector3(420, 268, 0);
            default:
                return Vector3.zero;
        }
    }
    Vector3 recycleTargetpos = new Vector3(-270, -270, 0);
    Card cd;
    GameObject targetcard;
    GameObject textureobj;
    GameObject textobj;
    GameObject BgEffect = null;
    Transform BgEffectParent = null;
    GameObject RecycleEffect = null;
    GameObject StarEffect = null;
}