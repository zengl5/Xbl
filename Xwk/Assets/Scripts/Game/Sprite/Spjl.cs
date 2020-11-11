using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spjl : MonoBehaviour {

    Animator anim;
    Camera PlayerCam;
	void Awake () {
        anim = this.GetComponent<Animator>();
        PlayerCam = GameObject.Find("PlayerCam").GetComponent<Camera>();
        AddJlClickEvent();
    }
   /// <summary>
   /// 注册精灵点击事件（重新播放声音）
   /// </summary>
    void AddJlClickEvent()
    {
        BoxCollider box=gameObject.AddComponent<BoxCollider>();
        box.size = 100 * Vector3.one;
        box.center = new Vector3(0, 50, 0);
        this.gameObject.tag = "meshButton";
        MeshButton meshButton=this.gameObject.AddComponent<MeshButton>();
        meshButton.AddMeshEvent(SpriteManager.Instance.ReplayNowEffectSound);
        MeshButtonManager.Instance.SetMeshCamera(PlayerCam);
    }
    void Update()
    {

    }
    public void StartTalk()
    {       
        if(anim==null)
        {
            anim = this.GetComponent<Animator>();
            anim.Play("jl00002_1_xingfen01_start#anim");
        }
        else
        {
            anim.Play("jl00002_1_xingfen01_start#anim");
        }
    }
    public void EndTalk()
    {
        anim.SetBool("talkfinish", true);
    }

    //一开始难过动画
    public void Nanguo()
    {
        anim.SetBool("nanguoover", false);
        anim.Play("jl00002_1_nanguo01_start#anim");
    }
    public void NanguoOver()
    {
        anim.SetBool("nanguoover", true);
    }

    //疑惑动画，点击精灵的时候
    public void Yihuo()
    {
        anim.Play("jl00002_1_yihou01#anim");
    }
    //兴奋，开始发牌的时候
    public void Xingfen()
    {
        anim.Play("jl00002_1_xingfen01_#anim");
    }

    //再次发牌的时候 jl00002_1_happy01#anim 肚子还是很鼓
    public void Happy()
    {
        anim.Play("jl00002_1_xingfen01_#anim");
    }
    //发牌动画
    public void DealCard()
    {
        anim.Play("jl00002_1_play01_start#anim");
    }
    
}
