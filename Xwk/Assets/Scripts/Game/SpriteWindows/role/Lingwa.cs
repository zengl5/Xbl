using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lingwa : baseRoleInfo {

    void Awake()
    {
        base.Awake();
        AddBoxCollider();
        animLength = new[] {6.62f, 2f, 2f, 3.33f };
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
        box.size = new Vector3(100, 120, 100);
        box.center = new Vector3(0, 70, 0);
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
            YB.XWK.MainScene.LocalData.m_SpiritType = WizardItemName.Wizard_lingwa;
            YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "Piercingeye", () => { Utility.SetMainScene("Piercingeye"); });
        }
        else
        {
            if (anim.GetCurrentAnimatorClipInfo(0).Length > 0)
                if (!anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("jl_00008_show01#anim"))
            {
                anim.Play("show01#anim");
                Refresh();
            }
            Spwindow.PlayCharacterAudio("xwk_jlej_43");
        }
    }
    public override void PlayRandomPlayAnim()
    {
        //Debug.LogError("随机播放声音");
        base.PlayRandomPlayAnim();
        base.RandomPlayAnim();
    }
}
