using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.C_Framework;

public class Sunwukong : MonoBehaviour, IRubbish
{
    public string AnimName;
    public bool IsRight;
    Action FindWukongErrorAction;
    public delegate void FindWukongRightAction(Sunwukong wk);
    public event FindWukongRightAction findWukong;
    float Scaleratio = 0.01F;
    Vector3 BoxColliderCenter = new Vector3(0, 60f, 0);
    Animator anim;
 
    GameObject JGB;//有的小悟空身上有金箍棒
    GameObject Effect;
    void Awake()
    {
        AddMeshButton();
    }
    public void RecycleRubbish()
    {
        StopAllCoroutines();
        if (Effect)
            Destroy(Effect);
    }
    void AddMeshButton()
    {
        this.gameObject.tag = "meshButton";
        MeshButton meshButton = this.gameObject.AddComponent<MeshButton>();
        meshButton.AddMeshEvent(ClickDeal);
        MeshButtonManager.Instance.SetMeshCamera(SeparationManager.Instance.CameraGm);
    }
    public void InitXwk(Vector3 angle)
    {
        transform.localScale =Vector3.zero;
        transform.eulerAngles = angle;
        StartCoroutine(ShowXwk());   
    }
    WaitForSeconds swait = new WaitForSeconds(0.7f);
    IEnumerator ShowXwk()
    {
        yield return swait;
        transform.localScale = Scaleratio * Vector3.one;
    }
    public void InitAnimator(string animName)
    {
        AnimName = animName;
        anim = transform.GetComponent<Animator>();
        anim.enabled = false;
        anim.Play(animName);
        anim.enabled = true;
        if (this.gameObject != null)
        {
            BoxCollider box = transform.gameObject.AddComponent<BoxCollider>();
            //22 160;
            if (AnimName.Contains("22"))
            {
                box.center = new Vector3(0, 160, 0);
            }
            else
            {
                box.center = BoxColliderCenter;
            }
            box.size = 100 * Vector3.one;
        }
    }
    
    public void InitEvent(FindWukongRightAction findRightAc, Action findErrorAc)
    {
        findWukong = findRightAc;
        FindWukongErrorAction = findErrorAc;
    }
    public void SetRight()
    {
        IsRight = true;
    }
    public void SetJGB(GameObject jgb)
    {
        JGB = jgb;
    }
    public void ClickDeal()
    {
        ClickEffect(IsRight);       
        //真悟空处理
        if (IsRight)
        {
            WindowSliderControl.Instance.DFrozenCamera();
             SeparationManager.Instance.FindSunWukong();
            //该动画播放向上动画
            //实例化一个小悟空在前面说话
            if (anim != null)
                anim.Play("wukong_fenshen_out_up@anim");
        }          
        if (!IsRight)
        {
            //选错三次就播放错误语音3,6,9
            if (FindWukongErrorAction != null)
                FindWukongErrorAction();
            this.gameObject.SetActive(false);
        }
    }
   
    void ClickEffect(bool flag)
    {
        AudioClip clipef = ABResMgr.Instance.LoadResource<AudioClip>("game/Separation/SoundEffect/public_xwkyx_002", "separation");
        AudioManager.Instance.PlaySound(clipef);
        if (JGB)
            JGB.SetActive(false);
        
        if (flag)
        {
            Effect = ABResMgr.Instance.LoadResource<GameObject>(SepPath.Instance.Effect_clickRight, ABCommonConfig.EfBundleType, true);
            Effect.transform.position = this.transform.position;
            Effect.transform.localScale = 1f * Vector3.one;
            AudioClip clipef2 = ABResMgr.Instance.LoadResource<AudioClip>("game/Separation/SoundEffect/public_xwkyx_003", "separation");
            AudioManager.Instance.PlaySound(clipef2);
        }
        else
        {
            Effect = ABResMgr.Instance.LoadResource<GameObject>(SepPath.Instance.Effect_click,ABCommonConfig.EfBundleType, true);
            Effect.transform.position = this.transform.position;
            Effect.transform.localScale = 1f * Vector3.one;
        }
    }
}
