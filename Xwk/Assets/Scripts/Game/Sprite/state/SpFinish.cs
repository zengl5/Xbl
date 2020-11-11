using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using DG.Tweening;
using Xbl;
using XWK.Common.UI_Reward;

public class SpFinish : C_IState, IRubbish
{
    public string Name { get; set; }
    Spxwk xwk;
    GameObject OverEffect;
    public SpFinish(GameObject xwkobj)
    {
        xwk = xwkobj.GetComponent<Spxwk>();
    }
    public virtual void OnStateEnter()
    {
        Name = "SpFinish";
        SetScore();
    }
    public void RecycleRubbish()
    {
        if (OverEffect)
            GameObject.Destroy(OverEffect);        
    }
    public virtual void OnStateLeave()
    {

    }

    public virtual void OnStateOverride()
    {

    }
    public virtual void OnStateResume()
    {

    }
    void SetScore()
    {
        AudioManager.Instance.StopPlayerSound();
        //小悟空说话(耶，我们成功了)+动作 
        xwk.Over();
        //结算     
        RewardUIManager.Instance.RegisterStory(15, SourceType.SpriteWindow, 1, Finish);//30   10   // 31 32  33  34 .....40
        RewardUIManager.Instance.SetSuccess();
    }
    void Finish(bool flag)
    {      
         
        SpriteManager.Instance.PlayCharacterAudio("byjlyx_23");
        OverEffect = ABResMgr.Instance.LoadResource<GameObject>(SpfilePath.Instance.ui_public_effect_kpjs, ABCommonConfig.EfBundleType, true);
        OverEffect.transform.localScale = 0.01f * Vector3.one;
        SpriteManager.Instance.StartCoroutine(GameOver());
    }
    WaitForSeconds lgwait= new WaitForSeconds(5.0f);
    IEnumerator GameOver()
    {
        yield return lgwait;
        SpriteManager.Instance.GameOver();
    }
}