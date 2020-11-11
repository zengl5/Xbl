using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using DG.Tweening;
using Xbl;
using XWK.Common.UI_Reward;

public class HlFinish : C_IState, IRubbish
{
    public string Name { get; set; }
    GameObject OverEffect;
    GameObject xwk;
    public HlFinish(GameObject xwk)
    {
        this.xwk = xwk;
    }
    public virtual void OnStateEnter()
    {
        Name = "HlFinish";
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
        xwk.GetComponent<Huluxwk>().Over();       
        //结算     
        RewardUIManager.Instance.RegisterStory(9, SourceType.SpriteWindow, 1, Finish,"huluGame");//30   10   // 31 32  33  34 .....40
        RewardUIManager.Instance.SetSuccess();
    }
    void Finish(bool flag)
    {
        AudioManager.Instance.StopPlayerSound();

        //小悟空说话(耶，我们成功了)+动作 
        HuluManager.Instance.PlayCharacterAudio("bbhlyx_16");
        OverEffect = ABResMgr.Instance.LoadResource<GameObject>(SpfilePath.Instance.ui_public_effect_kpjs, ABCommonConfig.EfBundleType, true);
        OverEffect.transform.localScale = 0.01f * Vector3.one;
        HuluManager.Instance.StartCoroutine(GameOver());

      
    }
    
    WaitForSeconds wait = new WaitForSeconds(5.0f);
    IEnumerator GameOver()
    {
        yield return wait;
        HuluManager.Instance.GameOver();
    }


}