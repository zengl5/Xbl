using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.XWK.MainScene;
public class baseRoleInfo:MonoBehaviour
{
    protected Animator anim;
    protected MeshButton mesh;
    protected float[] animLength;//show01时长 stand01  02   03 时长

    public void Awake()
    {
        anim = this.GetComponent<Animator>();
        if(anim!=null)
        {
            anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;       
        }
        mesh = this.gameObject.AddComponent<MeshButton>();
        mesh.AddMeshEvent(OnClickRoleEvent);
    }
   
    
    public virtual void PlayRandomPlayAnim()
    {
        
    }
    /// <summary>
    /// 刷新角色待机动作
    /// </summary>
    public virtual void Refresh()
    {

    }
    public virtual void RefreshRtmc(SpriteData sp)
    {
        if (sp == null)
            return;
        if (anim == null)
            return;
        if (sp.lockIcon)
        {
            RuntimeAnimatorController rtmc = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(sp.Meshdata.MeshRealRtmc, "spwindow", true);
            anim.runtimeAnimatorController = rtmc;
        }
        else
        {
            RuntimeAnimatorController rtmc = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(sp.Meshdata.MeshJiatiRtmc, "spwindow", true);
            anim.runtimeAnimatorController = rtmc;
        }
    }
    public virtual void OnClickRoleEvent()
    {
        LocalData.GotoStoryOrGameBy_SpWindow = true;
        C_UIMgr.Instance.CloseUI("UI_SpriteWindow");
        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 7);
    }
    protected void RandomPlayAnim()
    {
        if (!this.gameObject.activeInHierarchy)
            return;
        float rd = Random.Range(0f, 1f);
        if (rd <= 0.7f)
        {
            anim.Play("stand01#anim");            
            Invoke("RandomPlayAnim", animLength[1]);
        }
        else if (rd <= 0.85f)
        {
            anim.Play("stand02#anim");
            Invoke("RandomPlayAnim", animLength[2]);
        }
        else
        {
            anim.Play("stand03#anim");
            Invoke("RandomPlayAnim", animLength[3]);
        }
    }

    protected void CancelAllInvoke()
    {
        CancelInvoke("RandomPlayAnim");
    }

}
public class Huluinfo : baseRoleInfo
{
    void Awake()
    {
        base.Awake();
        AddBoxCollider();
        animLength = new[] { 3.3f, 1.65f, 1.65f, 1.9f };
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
        box.size=new  Vector3(60, 120, 100);
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
            LocalData.m_SpiritType = WizardItemName.Wizard_Hulu;
            YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "Piercingeye", () => { Utility.SetMainScene("Piercingeye"); });
            //YBSceneLoadingMgr.Instance.LoadScene("Piercingeye", () => { Utility.SetMainScene("Piercingeye"); });
        }
        else
        {
            if (anim.GetCurrentAnimatorClipInfo(0).Length > 0)
            {
                if (!anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("jl00001_1_show1#anim"))
                {
                    anim.Play("show01#anim");
                    Refresh();
                }
            }                
            int id = Random.Range(0, 4);
            if (id == 0)
            {
                Spwindow.PlayCharacterAudio("xwk_jlej_26");
            }
            else if (id == 1)
            {
                Spwindow.PlayCharacterAudio("xwk_jlej_27");
            }
            else if (id == 2)
            {
                Spwindow.PlayCharacterAudio("xwk_jlej_28");
            }
            else if (id == 3)
            {
                Spwindow.PlayCharacterAudio("xwk_jlej_29");
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
