using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.XWK.MainScene;

public class Xiaolninfo : baseRoleInfo
{
    void Awake()
    {
        base.Awake();
        AddBoxCollider();
        animLength = new[] { 4.32f, 2f, 3.62f, 4.3f };
        Invoke("PlayRandomPlayAnim", animLength[0]);
    }
    void AddBoxCollider()
    {
        BoxCollider box = this.gameObject.GetAddComponent<BoxCollider>();
         box.center = new Vector3(0F, 120, 40);
        box.size = new Vector3(60, 120, 50);

        box.tag = "meshButton";
    }
    public override void Refresh()
    {
        CancelInvoke("PlayRandomPlayAnim");
        CancelAllInvoke();
        Invoke("PlayRandomPlayAnim", animLength[0]);
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
            LocalData.m_SpiritType = WizardItemName.Wizard_Xln;
            YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "Piercingeye", () => { Utility.SetMainScene("Piercingeye"); });
        }
        else
        {
            if (anim.GetCurrentAnimatorClipInfo(0).Length > 0)
            {
                if (!anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("jl00003_1_show#anim"))
                {
                    anim.Play("show01#anim");
                    Refresh();
                }
            }
            int id = Random.Range(0, 4);
            if (id == 0)
            {
                Spwindow.PlayCharacterAudio("xwk_jlej_30");
            }
            else if (id == 1)
            {
                Spwindow.PlayCharacterAudio("xwk_jlej_31");
            }
            else if (id == 2)
            {
                Spwindow.PlayCharacterAudio("xwk_jlej_32");
            }
            else if (id == 3)
            {
                Spwindow.PlayCharacterAudio("xwk_jlej_33");
            }
        }
    }
    private void Update()
    {

    }
    public override void PlayRandomPlayAnim()
    {
        base.PlayRandomPlayAnim();
        RandomPlayAnim();
    }
}