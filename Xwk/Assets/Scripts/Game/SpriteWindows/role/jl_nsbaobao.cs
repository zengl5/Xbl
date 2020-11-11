using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jl_nsbaobao : baseRoleInfo
{

    void Awake()
    {
        base.Awake();
        AddBoxCollider();
        animLength = new[] { 4.6f, 1.4f, 3.62f, 4.1f };
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
        box.center = new Vector3(0, 500, 0);
        box.size = new Vector3(600, 1000, 600);
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
            YB.XWK.MainScene.LocalData.m_SpiritType = WizardItemName.Wizard_nianshou;
            YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "Piercingeye", () => { Utility.SetMainScene("Piercingeye"); });
        }
        else
        {
            if (anim.GetCurrentAnimatorClipInfo(0).Length > 0)
                if (!anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("jl_00014_show01#anim"))
                {
                    anim.Play("show01#anim");
                    Refresh();
                }
            int id = Random.Range(0, 4);
            if (id == 0)
            {
                Spwindow.PlayCharacterAudio("xwk_jlej_34");
            }
            else if (id == 1)
            {
                Spwindow.PlayCharacterAudio("xwk_jlej_35");
            }
            else if (id == 2)
            {
                Spwindow.PlayCharacterAudio("xwk_jlej_36");
            }
            else if (id == 3)
            {
                Spwindow.PlayCharacterAudio("xwk_jlej_37");
            }
        }
    }
    public override void PlayRandomPlayAnim()
    {
        //Debug.LogError("随机播放声音");
        base.PlayRandomPlayAnim();
        base.RandomPlayAnim();
    }
}

