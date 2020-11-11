using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.XWK.MainScene;
 
public class Spriteinfo : baseRoleInfo
{
    void Awake()
    {
        base.Awake();
        AddBoxCollider();
        animLength = new[] {2.6f, 1.33f, 3.65f, 2.5f };
        Invoke("PlayRandomPlayAnim", animLength[0]);
    }
    void AddBoxCollider()
    {
        BoxCollider box = this.gameObject.GetAddComponent<BoxCollider>();
        box.size = new Vector3(100, 140, 100);
        box.center = new Vector3(0, 70, 0);
        box.tag = "meshButton";
    }
    public override void RefreshRtmc(SpriteData sp)
    {
        base.RefreshRtmc(sp);
    }
    public override void Refresh()
    {
        CancelInvoke("PlayRandomPlayAnim");
        CancelAllInvoke();
        Invoke("PlayRandomPlayAnim", animLength[0]);
    }
    public override void OnClickRoleEvent()
    {
        if (SpriteIconMgr.Instance.GetSpriteData() == null)
            return;
        if (!SpriteIconMgr.Instance.GetSpriteData().lockIcon)//还没有解锁，跳转到指定场景
        {
            base.OnClickRoleEvent();
            DirectorMgr.Instance.CloseAllDirecStep();//关闭一阶段引导显示
            LocalData.m_SpiritType = WizardItemName.Wizard_BaiYin;
            YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "Piercingeye", () => { Utility.SetMainScene("Piercingeye"); });
            if (DirectorMgr.DirectorAllState)
            {
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_spriteWindow, "Click_Baiyingjingling");
            }
            else
            {
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_spriteWindow, "Click_BaiyingjinglingNewUser");
            }
        }
        else
        {
            if (DirectorMgr.DirectorAllState)
            {
                if(anim.GetCurrentAnimatorClipInfo(0).Length>0)
                {
                    if (!anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("jl00002_1_show01#anim"))
                    {
                        anim.Play("show01#anim");
                        Refresh();
                    }
                }
                 
                int id = Random.Range(0, 4);
                if (id == 0)
                {
                    Spwindow.PlayCharacterAudio("xwk_jlej_22");
                }
                else if (id == 1)
                {
                    Spwindow.PlayCharacterAudio("xwk_jlej_23");
                }
                else if (id == 2)
                {
                    Spwindow.PlayCharacterAudio("xwk_jlej_24");
                }
                else if (id == 3)
                {
                    Spwindow.PlayCharacterAudio("xwk_jlej_25");
                }
            }           
        }
    }
    public override void PlayRandomPlayAnim()
    {
        base.PlayRandomPlayAnim();
        RandomPlayAnim();
    }
}