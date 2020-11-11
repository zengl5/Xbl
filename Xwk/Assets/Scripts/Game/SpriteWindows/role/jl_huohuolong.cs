﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jl_huohuolong : baseRoleInfo
{

    void Awake()
    {
        base.Awake();
        AddBoxCollider();
        animLength = new[] { 2.6f, 2f, 3.3f, 1.7f };
        Invoke("PlayRandomPlayAnim", animLength[0]);
    }
    public override void Refresh()
    {
        anim.enabled = false;
        anim.enabled = true;
        CancelInvoke("PlayRandomPlayAnim");
        CancelAllInvoke();

        Invoke("PlayRandomPlayAnim", animLength[0]);
    }
    void AddBoxCollider()
    {
        BoxCollider box = this.gameObject.GetAddComponent<BoxCollider>();
        box.center = new Vector3(0, 50, 0);
        box.size = new Vector3(100, 100, 100);
        box.tag = "meshButton";
    }
    public override void RefreshRtmc(SpriteData sp)
    {
        base.RefreshRtmc(sp);
    }
    public override void OnClickRoleEvent()
    {
        if (!SpriteIconMgr.Instance.GetSpriteData().lockIcon)//还没有解锁，跳转到指定场景
        {
            base.OnClickRoleEvent();
            YB.XWK.MainScene.LocalData.m_SpiritType = WizardItemName.Wizard_huohuolong;
            YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "Piercingeye", () => { Utility.SetMainScene("Piercingeye"); });
        }
        else
        {
            if (anim.GetCurrentAnimatorClipInfo(0).Length > 0)
                if (!anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("jl_00007_show01#anim"))
                {
                    anim.Play("show01#anim");
                    Refresh();
                }
            Spwindow.PlayCharacterAudio("xwk_jlej_47");
        }
    }
    public override void PlayRandomPlayAnim()
    {
        //Debug.LogError("随机播放声音");
        base.PlayRandomPlayAnim();
        base.RandomPlayAnim();
    }
}