using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using DG.Tweening;
using Xbl;
using UnityEngine.UI;
public class SpGame : C_IState, IRubbish
{
    #region
    List<SpriteCard> SpriteCardList;
    List<Texture2D> SpBgtex = new List<Texture2D>();//背景框图片
    List<Card> cardConfigList = new List<Card>();
    public Card rightCard;
    Card card;
    int rightid;
    Vector3 targetPos = new Vector3(-400, 205, 0);
    string clickCardNameByPlayer = "";
    bool clickRightByXwk = false;
    string baseuiPath = "game/Sprite/ui/icon/";
    string basesdPath = "game/Sprite/soundeffect/";
    string basesdEffectPath = "game/Sprite/soundeffect/";
    string baseCardSoundPath = "game/Sprite/sound/animalsound/";
    bool haveClick = false;
    bool playGuid = false;
    Spxwk xwk;
    Spjl jl;
    GameObject targetUI;
    LitJson.JsonData SpjsonData;
    Texture2D tex = null;
    GameObject public_effect_wkrd = null;
    WaitForSeconds lgwait = new WaitForSeconds(0.8f);
    WaitForSeconds lgwait2 = new WaitForSeconds(0.7f);
    WaitForSeconds lgwait3 = new WaitForSeconds(10f);
    WaitForSeconds xwkwait = new WaitForSeconds(1.5f);
    public string Name { get; set; }
    public SpGame(List<SpriteCard> sl, GameObject xwkobj, GameObject jlobj, GameObject tgUI)
    {
        SpriteCardList = sl;
        xwk = xwkobj.GetComponent<Spxwk>();
        jl = jlobj.GetComponent<Spjl>();
        targetUI = tgUI;
        for (int i = 0; i < sl.Count; i++)
            SpBgtex.Add(sl[i].GetComponent<RawImage>().texture as Texture2D);
    }
    public void InitJsonData(LitJson.JsonData data)
    {
        SpjsonData = data;
    }
    public virtual void OnStateEnter()
    {
        Name = "SpGame";
        InitCardJsonData();
        InitCard();
    }
    public void RecycleRubbish()
    {
        SpriteCardList.Clear();
        cardConfigList.Clear();
        SpBgtex.Clear();
        if (public_effect_wkrd != null)
            GameObject.Destroy(public_effect_wkrd);
        SpriteManager.Instance.StopAllCoroutines();
    }
    public virtual void OnStateLeave()
    {
        RecycleRubbish();
    }

    public virtual void OnStateOverride()
    {

    }
    public virtual void OnStateResume()
    {

    }
    #endregion

    void ContinueGame()
    {
        if (SpriteManager.Instance.GotoFinish())
            return;
        if (SpriteManager.Instance == null)
            return;
        //判断是否结束
        if (xwk)
        {
            xwk.EndTalk();
        }     
        //语音对话，对话完成之后继续游戏
        if(jl)
        {
            jl.Happy();//笑眯眯拍着自己的肚子
        }
        SpriteManager.Instance.NotifyUserChooseRight(false);
        SpriteManager.Instance.PlayCharacterAudio("byjlyx_22",
            delegate
            {
                cardConfigList.Clear();
                InitCardJsonData();
                InitCard();
            });
    }
    void InitCardJsonData()
    {
        foreach (LitJson.JsonData temp in SpjsonData["SpritList"])
        {
            string cardname = temp["CardName"].ToString();
            string efSoundName = temp["EffectSoundName"].ToString();
            string cardSoundName = temp["CardSoundName"].ToString();
            string cardRealName = temp["CardRealName"].ToString();
            Card cd = new Card(cardname, efSoundName, cardSoundName, cardRealName);
            if (Application.isEditor)
            {
                cardConfigList.Add(cd);
            }
            else
            {
                cardConfigList.Add(cd);
            }
        }
    }
    /// <summary>
    /// 重置UI 位置，缩放系数
    /// </summary>
    void InitCard()
    {
        haveClick = false;
        playGuid = false;
        for (int i = 0; i < SpriteCardList.Count; i++)
        {
            SpriteCardList[i].InitTargetBigCard(targetUI);
            SpriteCardList[i].clickEvent -= PlayerOnclickFinish;
            SpriteCardList[i].clickEvent += PlayerOnclickFinish;
        }
        if(jl)
        jl.StartTalk();
        SpriteManager.Instance.CreateCardAudio(DealCard);
        SpriteManager.Instance.CancelHit();
        SpriteManager.Instance.StartCoroutine(UserNoOpration());
    }
    void DealCard()
    {
        jl.EndTalk();
        jl.DealCard();//播放发牌动画
        SpriteManager.Instance.StartCoroutine(CreatCard());
    }
    IEnumerator CreatCard()
    {
        yield return lgwait;
        //先确认rightcard
        rightid = Random.Range(0, 3);
        int rightindex = UnityEngine.Random.Range(0, cardConfigList.Count);//随机卡片     
        rightCard = cardConfigList[rightindex];
        cardConfigList.RemoveAt(rightindex);
        PlayGuideSound();
        yield return lgwait2;
        int randomindex = 0;
        Vector3[] targetArray = new Vector3[] { new Vector3(600, 551, -972),
            new Vector3(0, 551, -972),new Vector3(-600, 551, -972)};
        for (int i = 0; i < 3; i++)
        {
            if (i == rightid)
            {
                card = rightCard;
                tex = ABResMgr.Instance.LoadResource<Texture2D>(baseuiPath + card.CardsignName, "sprite");
                card.CardTexture = tex;
                card.CardId = i;
                card.CardBgTex = SpBgtex[i];
                card.RightCard = true;
                card.RightCardId = rightid;
                card.CardId = i;
                rightCard = card;
                randomindex = rightindex;
            }
            else
            {
                randomindex = UnityEngine.Random.Range(0, cardConfigList.Count);//随机卡片         
                card = cardConfigList[randomindex];
                tex = ABResMgr.Instance.LoadResource<Texture2D>(baseuiPath + card.CardsignName, "sprite");
                card.CardTexture = tex;
                card.CardId = i;
                card.CardBgTex = SpBgtex[i];
                card.RightCard = false;
                card.RightCardId = -1;
                card.CardId = i;
                cardConfigList.RemoveAt(randomindex);
            }
            //实例化这种卡片,飞向屏幕三个位置           
            SpriteCardList[i].InitCard(card, this);
            SpriteCardList[i].MoveCard(targetArray[i], null);
            SpriteCardList[i].NotifyRightCard(rightCard);
        }
    }
    /// <summary>
    /// 用户无操作
    /// </summary>
    /// <returns></returns>
    IEnumerator UserNoOpration()
    {
        yield return lgwait3;
        if (!haveClick)
        {
            float rdIndex = Random.Range(0f, 1f);
            if(rdIndex>=0.5f)
            {
                SpriteManager.Instance.PlaySceneAudio("byjlyx_6");
            }
            else
            {
                SpriteManager.Instance.PlaySceneAudio("byjlyx_7");
            }
            SpriteManager.Instance.StartCoroutine(UserNoOpration());
        }
    }

    void PlayGuideSound()
    {
        if (playGuid)
            return;
        playGuid = true;
        SpriteManager.Instance.NotifyHit();
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>(basesdPath + rightCard.EffectSoundName, "sprite");
        AudioManager.Instance.PlayerSoundByClip(clip, null);//播放完提醒用户选--->提醒用户选
    }
    public void RePlayGuideSound()
    {
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>(basesdPath + rightCard.EffectSoundName, "sprite");
        AudioManager.Instance.PlayerSoundByClip(clip, null);//播放完提醒用户选--->提醒用户选
    }
    void PlayerOnclickFinish(string name, int id)
    {
        if (haveClick)
            return;
        clickCardNameByPlayer = name;
        haveClick = true;
        SpriteManager.Instance.PlayEffectAudio("public_xwkyx_039");
        //轮到小悟空选择精灵【系统自动选择】
        SpriteManager.Instance.StartCoroutine(XiaoWukongOnclick());
        SpriteManager.Instance.CancelHit();
        SpriteManager.Instance.ShowSelectionEffect(id);
    }
    IEnumerator XiaoWukongOnclick()
    {       
        yield return xwkwait;
        SpriteManager.Instance.PlayCharacterAudio("byjlyx_8");//啊哈我选这个
        int rdIndex = UnityEngine.Random.Range(0, SpriteCardList.Count);
        if (Application.isEditor)
        {
            if (rdIndex == rightCard.RightCardId)
                clickRightByXwk = true;
            else
                clickRightByXwk = false;
        }
        else
        {
            if (rdIndex == rightCard.RightCardId)
                clickRightByXwk = true;
            else
                clickRightByXwk = false;
        }
        xwk.ChooseCard(rdIndex, SpriteCardList[rdIndex].Showxwkphoto);       
        OpenFallEf();//下落特效    
        yield return new WaitForSeconds(5f); //走马灯效果
        SpriteManager.Instance.ShowPaoMaDeng(rightid, DealCardMove);
    }
    void OpenFallEf()
    {
        if (public_effect_wkrd == null)
        {
            public_effect_wkrd = ABResMgr.Instance.LoadResource<GameObject>(SpfilePath.Instance.public_effect_wkrd, ABCommonConfig.EfBundleType, true);
            public_effect_wkrd.transform.localPosition = new Vector3(-1, 0, 0.88f);
        }
        else
        {
            public_effect_wkrd.SetActive(false);
            public_effect_wkrd.SetActive(true);
        }
    }
    void DealCardMove()
    {
        if (clickCardNameByPlayer.Equals(rightCard.CardRealName) && clickRightByXwk)
        {
            SpriteManager.Instance.NotifyUserChooseRight(true);
            SpriteManager.Instance.NotifyGameTime();
            SpriteManager.Instance.PlayEffectAudio("public_xwkyx_061");
        }
        else if (clickCardNameByPlayer.Equals(rightCard.CardRealName) && !clickRightByXwk)
        {
            SpriteManager.Instance.NotifyUserChooseRight(true);
            SpriteManager.Instance.NotifyGameTime();
            SpriteManager.Instance.PlayEffectAudio("public_xwkyx_061");
        }
        else
        {
            SpriteManager.Instance.NotifyUserChooseRight(false);
            SpriteManager.Instance.PlayEffectAudio("public_xwkyx_062");
        }
        for (int i = 0; i < SpriteCardList.Count; i++)
            SpriteCardList[i].CardLeave();
    }
    public void PlayRightAudio()
    {
        SpriteManager.Instance.PlayCharacterAudio_AbsolutelyAdress(baseCardSoundPath + rightCard.CardSoundName,
               () =>
               SpriteManager.Instance.PlayCharacterAudio_AbsolutelyAdress(basesdEffectPath + rightCard.EffectSoundName, Systemfeedback));
    }
    /// <summary>
    /// 系统判断选择结果
    /// </summary>
    void Systemfeedback()
    {
        if (clickCardNameByPlayer.Equals(rightCard.CardRealName) && clickRightByXwk)
        {
            //都对   //兴奋表情
            xwk.ResetTalk();
            int randomId = Random.Range(0, 3);
            SpriteManager.Instance.PlayAudio_AllRight(randomId, ContinueGame);
            xwk.AllRight(randomId);
        }
        else if (clickCardNameByPlayer.Equals(rightCard.CardRealName) && !clickRightByXwk)
        {
            //用户对 小悟空错
            xwk.ResetTalk();
            SpriteManager.Instance.PlayAudio_UserRight(ContinueGame);
            xwk.UserRight();
        }
        else if (!clickCardNameByPlayer.Equals(rightCard.CardRealName) && clickRightByXwk)
        {
            //用户错 小悟空对
            xwk.ResetTalk();
            int randomId = Random.Range(0, 3);
            SpriteManager.Instance.PlayAudio_UserFail(randomId, ContinueGame);
            xwk.UserFail(randomId);
        }
        else
        {
            //都没对           
            xwk.ResetTalk();
            int randomId = Random.Range(0, 4);
            SpriteManager.Instance.PlayAudio_AllWrong(randomId, ContinueGame);
            xwk.AllWrong(randomId);
        }
    }
}