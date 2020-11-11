using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Spxwk : MonoBehaviour, IRubbish
{ 
    Animator anim;
    TweenCallback twcallback;
    GameObject public_effect_wkrd = null;
    int moveId = 0;
    //小悟空移动位置
    Vector3 startPos = new Vector3(-0.88f, 0, 1);//初始位置
    Vector3 startAngle = new Vector3(0, 8.4f, 0);//初始旋转

    //播放摸牌初始位置
    Vector3 targetId0 = new Vector3(0.65f, 0.84F, 1);
    Vector3 targetId1 = new Vector3(-0.76f, 0.84f, 1);
    Vector3 targetId2 = new Vector3(-2.23f, 0.84f, 1);
    void Awake () {
        anim = this.GetComponent<Animator>();
        transform.eulerAngles = startAngle;
    }
    public void RecycleRubbish()
    {
        if (public_effect_wkrd != null)
            Destroy(public_effect_wkrd);
    }
    public void AllRight(int id)
    {
        switch(id)
        {
            case 0:
                anim.Play("wukong_ty_xingfen01_start#anim");//wukong_win02#anim
                break;
            case 1:
                anim.Play("wukong_ty_xingfen01_start#anim");//wukong_jindou01#anim
                break;
            case 2:
                anim.Play("wukong_ty_xingfen01_start#anim");//wukong_win03#anim
                break;
        }      
    }
    public void AllWrong(int id)
    {
        switch (id)
        {
            case 0:
                anim.Play("wukong_ty_shiluo01_start#anim");
                break;
            case 1:
                anim.Play("wukong_ty_shiluo01_start#anim");//wukong_chijing01#anim
                break;
            case 2:
                anim.Play("wukong_ty_shiluo01_start#anim");//wukong_yihan01#anim
                break;
            case 3:
                anim.Play("wukong_ty_shiluo01_start#anim");//wukong_bushuang02#anim
                break;
        }
    }
    public void UserRight()
    {
        anim.Play("wukong_ty_win03#anim");
    }
    public void ResetTalk()
    {
        anim.SetBool("talkfinish", false);
    }
    public void EndTalk()
    {
        anim.SetBool("talkfinish", true);
    }
    public void Idle()
    {
        anim.Play("wukong_ty_stand01#animidle");
    }
    public void UserFail(int id)
    {
        switch (id)
        {
            case 0:
                anim.Play("wukong_ty_xingfen01_start#anim");
                break;
            case 1:
                anim.Play("wukong_ty_win03#anim");//wukong_win03#anim
                break;
            case 2:
                anim.Play("wukong_ty_win03#anim");//wukong_win03#anim
                break;       
        }
    }
    //啊哈，我选这个
    public void Xingfen()
    {
        anim.Play("wukong_ty_xingfen01_start#anim");
    }
    public void Over()
    {
        anim.Play("wukong_ty_win01#anim");
    }
    //成功，随机三次
    public void Win1()
    {
        anim.Play("wukong_ty_xingfen01_start#anim");
    }
    public void Win2()
    {
        anim.Play("wukong_ty_xingfen01_start#anim");
    }
    public void Win3()
    {
        anim.Play("wukong_ty_xingfen01_start#anim");
    }
    
    public void ChooseCard(int rdIndex, TweenCallback callback)
    {
        anim.SetBool("wukonginto", false);
        anim.SetBool("wukongraise", false);
        anim.Play("wukong_touchout01#anim");
        twcallback = callback;
        moveId = rdIndex;
        //播放入地特效
        Invoke("MoveToCard",1f);
    }
    void MoveToCard()
    {
        switch(moveId)
        {
            case 0:
                transform.position = targetId0;
                transform.DORotate(new Vector3(0, 340, 0), 0.2f);
                AddGroundEffect(new Vector3(targetId0.x, 0, 0.88f));
                break;
            case 1:
                transform.position = targetId1;
                transform.DORotate(Vector3.zero, 0.2f);
                AddGroundEffect(new Vector3(targetId1.x, 0, 0.88f));
                break;
            case 2:
                transform.position = targetId2;
                transform.DORotate(new Vector3(0,20,0), 0.2f);
                AddGroundEffect(new Vector3(targetId2.x, 0, 0.88f));
                break;
        }
        anim.SetBool("wukongraise",true);
        //播放出地特效
        Invoke("BackToGround", 1f);
    }
     void AddGroundEffect(Vector3 pos)
    {
        //下落特效
        if (public_effect_wkrd == null)
        {
            public_effect_wkrd = ABResMgr.Instance.LoadResource<GameObject>(SpfilePath.Instance.public_effect_wkrd, ABCommonConfig.EfBundleType, true);
            public_effect_wkrd.transform.localPosition = pos;
        }
        else
        {
            public_effect_wkrd.transform.position = pos;
            public_effect_wkrd.SetActive(false);
            public_effect_wkrd.SetActive(true);
        }
    }
    void BackToGround()
    {
        anim.SetBool("wukonginto", true);//举手拍打
        if (twcallback != null)
            twcallback();
        SpriteManager.Instance.PlayEffectAudio("public_xwkyx_039");
        Invoke("BackToGroundDelay", 1f);       
    }
    void BackToGroundDelay()
    {
        transform.eulerAngles = startAngle;
        transform.position = startPos;
    }
    //用户错误，小悟空正确 wind3+jindou
    public void StartTalk()
    {
        anim.Play("wukong_ty_talk01_start#anim"); 
    }    
}
