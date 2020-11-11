using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
 
public class SpriteCard : MonoBehaviour{
    #region#卡片初始化
    public delegate void ClickEvent(string clickName,int id);
    public event ClickEvent clickEvent;
    //WordUI 初始坐标
    Vector3 init3dcarPos = new Vector3(160,150,370);
    Texture2D xwktex = null;
    Sprite usertex = null;
    Button button;
    CardInfo cdinfo;
    Card card;
    Card rightCard;
    string thisCardRealName = "";
    GameObject TargetBigCard;
    int rightId = 0;
    string baseuiPath = "game/Sprite/ui/icon/";
    GameObject clickCardEffect = null;
    //添加大卡片背景
    Transform BgEffectParent = null;
    GameObject RecycleEffect = null;
    GameObject StarEffect = null;
    TargetBigCard targetBigCard = null;
    SpGame spgame;
    void Awake () {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(OnclickCardEvent);
        RawImage image = this.transform.GetChild(0).GetComponent<RawImage>();
        Text infotext = this.transform.GetChild(1).GetComponent<Text>();
        RawImage photo = this.transform.GetChild(2).GetComponent<RawImage>();
        Image userphoto = this.transform.GetChild(3).GetChild(0).GetComponent<Image>();
        cdinfo = new CardInfo(image, infotext, photo, userphoto);
        cdinfo.Hide();
    }  
    public void InitTargetBigCard(GameObject targetUI)
    {
        TargetBigCard = targetUI;
        transform.gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.localPosition = init3dcarPos;
        if (cdinfo.xwkphoto!= null)
            cdinfo.xwkphoto.transform.gameObject.SetActive(false);
        if (cdinfo.userphoto!= null)
            cdinfo.userphoto.transform.parent.gameObject.SetActive(false);
    } 
    public void InitCard(Card card,SpGame sp)
    {
        spgame = sp;
        this.card = card;
        rightId = card.RightCardId ;
        if (card.CardTexture!=null)
        cdinfo.image.texture = card.CardTexture;
        thisCardRealName = card.CardRealName ;
        string rmnb = SpriteManager.RemoveNumber(card.CardRealName);
        cdinfo.text.text = rmnb;
    }
    public void MoveCard(Vector3 target,TweenCallback ac)
    {
        SpriteManager.Instance.PlayEffectAudio("public_xwkyx_037");
        transform.DOLocalMove(target, 1);
        transform.DOScale(1.5F*Vector3.one, 1).OnComplete(ac);
    }
    public void NotifyRightCard(Card card)
    {
        rightCard = card;
    }
    #endregion


    public void Showxwkphoto()
    {
        OpenCardEffect();
        if (xwktex == null)
        {
            xwktex = ABResMgr.Instance.LoadResource<Texture2D>(SpfilePath.Instance.xwkicon, "sprite");
        }
        //选择同一个动物
        if (cdinfo.userphoto!=null&&cdinfo.userphoto.transform.gameObject.activeInHierarchy)
        {
            cdinfo.xwkphoto.transform.gameObject.SetActive(true);
        }
        else
        {
            cdinfo.xwkphoto.transform.gameObject.SetActive(true);
        }
        if(xwktex!=null)
        cdinfo.xwkphoto.texture = xwktex;
        if (cdinfo.userphoto.gameObject.activeInHierarchy)
            cdinfo.xwkphoto.transform.localPosition = cdinfo.secondPos;
        else
            cdinfo.xwkphoto.transform.localPosition = cdinfo.firstPos;
    }
     //玩家点击卡片
    void OnclickCardEvent()
    {
        if (!SpriteManager.Instance.HitState())
            return;
        SpriteManager.Instance.CancelHit();
        //对应图片点亮
        //显示用户头像
        cdinfo.userphoto.transform.parent.gameObject.SetActive(true);
        cdinfo.userphoto.transform.parent.gameObject.transform.localPosition = cdinfo.firstPos;
        usertex = GameDataMgr.Instance.AvatarSprite;
            if (usertex != null)
                cdinfo.userphoto.sprite = usertex;       
        if(clickEvent!= null)
        clickEvent(thisCardRealName,card.CardId);
        OpenCardEffect();
         
    }
    void OpenCardEffect()
    {
        //点击特效
        if (clickCardEffect == null)
        {
            clickCardEffect = ABResMgr.Instance.LoadResource<GameObject>(SpfilePath.Instance.ui_public_effect_dj, ABCommonConfig.EfBundleType, true);
            clickCardEffect.transform.parent = this.transform;
            clickCardEffect.transform.localPosition = Vector3.zero;
            clickCardEffect.transform.localEulerAngles = Vector3.zero;
            clickCardEffect.transform.localScale = Vector3.one;
        }
        else
        {
            clickCardEffect.SetActive(false);
            clickCardEffect.SetActive(true);
        }
    }
    #region#选择卡片结束逻辑处理
    //显示结果,卡片离开
    public void CardLeave()
    {
        if (card.RightCard)
        {
            if (targetBigCard == null)
            {
                targetBigCard = new TargetBigCard(TargetBigCard);
                targetBigCard.InitTargetCard(card);
            }
            else
            {
                targetBigCard.InitTargetCard(card);
            }
            SpriteManager.Instance.PlayEffectAudio("public_xwkyx_037");
            targetBigCard.MoveToCenter(WaitGotoRecycle, spgame.PlayRightAudio);
            transform.gameObject.SetActive(false);
        }
        else
        {
            //慢慢渐变消失
            transform.DOScale(Vector3.zero, 1);
            transform.SetSiblingIndex(1);
        }
    }   
    void WaitGotoRecycle()
    {        
        Invoke("GotoRecycle", 5);//5S 以后自动回收
        if (SpriteManager.Instance.GetUserChooseState())
        {
            Invoke("ShowStar", 1);//用户选对了，星星飞
            Invoke("AddRecycleEffect", 0.5f);
            Invoke("StarFly", 1.5f);
        }
    }
    //回收卡片 
    void GotoRecycle()
    {                  
        SpriteManager.Instance.PlayEffectAudio("public_xwkyx_037");
        targetBigCard.CenterToRecycle();   
    }    
    void AddRecycleEffect()//汇聚特效
    {
        if (BgEffectParent == null)
        {
            BgEffectParent = GameObject.Find("SpUICanvas").transform;
        }
        if (RecycleEffect == null)
        {
            RecycleEffect = ABResMgr.Instance.LoadResource<GameObject>(SpfilePath.Instance.ui_public_effect_kphj, ABCommonConfig.EfBundleType, true);
            RecycleEffect.transform.parent = BgEffectParent;
            RecycleEffect.transform.localPosition = Vector3.zero;
            RecycleEffect.transform.localEulerAngles = Vector3.zero;
            RecycleEffect.transform.localScale = Vector3.one;
        }
        else
        {
            RecycleEffect.SetActive(false);
            RecycleEffect.SetActive(true);
        }
    }
     void ShowStar()//飞向星星
    {
        if (StarEffect == null)
        {
            BgEffectParent = GameObject.Find("SpUICanvas").transform;
        }
        if (StarEffect == null)
        {
            StarEffect = ABResMgr.Instance.LoadResource<GameObject>(SpfilePath.Instance.ui_public_effect_xxtw, ABCommonConfig.EfBundleType, true);
            StarEffect.transform.parent = BgEffectParent;
            StarEffect.transform.localPosition =Vector3.zero;
            StarEffect.transform.localEulerAngles = Vector3.zero;
            StarEffect.transform.localScale = Vector3.one;

        }
        else
        {
            StarEffect.transform.localPosition = Vector3.zero;
            StarEffect.transform.localEulerAngles = Vector3.zero;
            StarEffect.transform.localScale = Vector3.one;
            StarEffect.SetActive(false);
            StarEffect.SetActive(true);
        }
     }
    void StarFly()
    {
        SpriteManager.Instance.PlayEffectAudio("public_xwkyx_036");
        Vector3[] posList = new Vector3[] { new Vector3(850, 416, 0), new Vector3(850, 316, 0), new Vector3(850, 216, 0), new Vector3(850, 116, 0), new Vector3(850, 16, 0)};
        if (SpriteManager.Instance.GetLightId()-1 <= posList.Length-1 && SpriteManager.Instance.GetLightId()-1 >= 0)
            StarEffect.transform.DOLocalMove(posList[SpriteManager.Instance.GetLightId()-1], 0.5F).OnComplete(SpriteManager.Instance.LightStar);
    }
    #endregion
}
 
