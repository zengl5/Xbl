using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using DG.Tweening;
using Xbl;

public class HlReady :C_IState, IRubbish
{
    public string Name { get; set; }
    Huluxwk xwk;
    huluren huluren;
    Hulu hulu;
    GameObject public_effect_yan02;
    public HlReady(GameObject xwk,GameObject huluren,GameObject hulu)
    {
        this.xwk = xwk.GetComponent<Huluxwk>();
        this.huluren = huluren.GetComponent<huluren>();
        this.hulu = hulu.GetComponent<Hulu>();
    }
    public virtual void OnStateEnter()
    {
        Name = "HlReady";
        Init();
    }
    public void RecycleRubbish()
    {
        if (public_effect_yan02 != null)
            GameObject.Destroy(public_effect_yan02);
    }
    public virtual void OnStateLeave()
    {
        if (public_effect_yan02 != null)
            GameObject.Destroy(public_effect_yan02);
    }

    public virtual void OnStateOverride()
    {

    }
    public virtual void OnStateResume()
    {

    }
    void Init()
    {
        ShowXwk();
        ShowHuluRen();
     }
    void ShowHuluRen()
    {
        hulu.transform.gameObject.SetActive(false);
        huluren.Talk();
        HuluManager.Instance.PlayCharacterAudio("bbhlyx_3",
            delegate
            {
                huluren.TalkEnd();
                huluren.Bianshen();
                HuluManager.Instance.StartCoroutine(ShowHulu());
            }
             );
    }
    WaitForSeconds swwait = new WaitForSeconds(0.65f);
    IEnumerator ShowHulu()
    {
        yield return swwait;
        public_effect_yan02 = ABResMgr.Instance.LoadResource<GameObject>(HlfilePath.Instance.public_effect_yan02, ABCommonConfig.EfBundleType, true);//白烟
        public_effect_yan02.transform.position = new Vector3(huluren.transform.position.x, 1, huluren.transform.position.z);
        HuluManager.Instance.PlayEffectAudio("public_xwkyx_020");
        huluren.transform.gameObject.SetActive(false);
        HuluShow();
        yield return new WaitForSeconds(1.5f);        
        HuluMove();
    }
    void HuluShow()
    {
        hulu.transform.gameObject.SetActive(true);
        hulu.transform.position = new Vector3(1.4f, -0.3f, 0);
        hulu.transform.localScale = 0.01f * Vector3.one;
        hulu.GetComponent<Hulu>().ReadyShow();
    }
    void HuluMove()
    {
        hulu.transform.position = new Vector3(1.4f, -0.2f, 0);
        hulu.GetComponent<Hulu>().Idle();
        Vector3 targetPos = new Vector3(0, 0.72f, -3.44f);
        HuluManager.Instance.PlayEffectAudio("public_xwkyx_042");//飞行
        hulu.transform.DOMove(targetPos, 1).OnComplete(XwkTalk);
        hulu.transform.DOScale(0.02f * Vector3.one, 1);
    }
    void ShowXwk()
    {
        xwk.transform.gameObject.SetActive(true);
        xwk.transform.localScale = 0.01f * Vector3.one;
    }
    void XwkTalk()
    {
        hulu.GetComponent<Hulu>().ShowDaijiEffect();
        xwk.WukongOutSpeak();
        HuluManager.Instance.PlayUserNameAudio("bbhlyx_4", GotoGame);
    }
    void GotoGame()
    {
        if(HuluManager.Instance!=null)
        HuluManager.Instance.GotoState("HlGame");
    }

}