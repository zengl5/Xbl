using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class Huluxwk : MonoBehaviour {

    Animator anim;
    Action action;
    Action flyAction;
    DOTweenPath path;
    void Awake () {
        anim = this.GetComponent<Animator>();
    }
    /// <summary>
    /// 悟空出现
    /// </summary>
    public void WukongOutAnim(bool autoSpeak = true,Action action=null)
    {
        anim.Play("wukong_chuxian01#anim");
        Invoke("GoBackHulu", 2.5f);
        if (autoSpeak)
            Invoke("WukongOutSpeak", 4f);
        if (action != null)
            this.action = action;
    }
    void GoBackHulu()
    {
        anim.SetBool("gengdouend", false);
        Vector3 targetPos = transform.localPosition;
        anim.Play("wukong_xz_gengdou_loop#anim2");
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 4, transform.localPosition.z);
        HuluManager.Instance.PlayEffectAudio("public_xwkyx_042");
        Tween tween =transform.DOLocalMove(targetPos, 0.5f).OnComplete(//播放悟空飞回葫芦
            delegate
            {                 
                anim.SetBool("gengdouend", true);
                HuluManager.Instance.PlayEffectAudio("public_xwkyx_018");
            }
                        );
        tween.SetEase(Ease.InCubic);
    }
    /// <summary>
    /// 悟空说话
    /// </summary>
	public void WukongOutSpeak()
    {
        anim.SetBool("talkend", false);
        anim.Play("wukong_ty_xingfen01_start#anim");
        if (action != null)
            action();
    }
  

    public void WukongOutEndSpeak()
    {
        anim.SetBool("talkend", true);
    }
    public void Over()
    {
        anim.Play("wukong_ty_win01#anim");
    }

    /// <summary>
    /// 悟空飞向葫芦
    /// </summary>
    /// <param name="path">悟空飞向曲线</param>  
    public void WukongFlytoHulu(DOTweenPath path, Action ac = null)
    {
        flyAction = ac;
        this.path = path;
        action = null;
        path.DORestart();
        transform.DOScale(Vector3.zero, 1.5f).OnComplete(HideSelf);
        HuluManager.Instance.PlayEffectAudio("public_xwkyx_042");//飞行
        anim.Play("wukong_xz_gengdou_start#anim");
        anim.SetBool("gengdouend", false);       
    } 
    void HideSelf()
    {
        this.path.DOPlayBackwards();
        this.path.duration = 0.1f;
        if (flyAction != null)
            flyAction();
        anim.SetBool("gengdouend", true);
        this.gameObject.SetActive(true);
     }
}
