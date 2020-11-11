using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbl;
public class XwkAnimEvent : MonoBehaviour
{
    Animator xwk;
    List<Transform> IdlePosList;
    string guidxwk_animatorcotrl = "game/Goldhoopbar/animatorcontroller/guidXwk";
    void Awake()
    {
        xwk = this.GetComponent<Animator>();
        AddXwkClickEvent();
    }    
    void AddXwkClickEvent()
    {
        BoxCollider box = this.transform.GetChild(0).transform.gameObject.AddComponent<BoxCollider>();
        box.center = new Vector3(-20, 0, 50);
        box.size = 100 * Vector3.one;
        MeshButton meshButton = this.transform.GetChild(0).gameObject.AddComponent<MeshButton>();
        meshButton.AddMeshEvent(OnClickXwk);
        MeshButtonManager.Instance.SetMeshCamera(GoldhoopbarManager.Instance.getCamera());
        this.gameObject.tag = "meshButton";
        this.transform.GetChild(0).tag = "meshButton";
    }
   
    public void InitIdlePosList(List<Transform> list)
    {
        IdlePosList = list;
    }
    public void SetIldePosList()
    {
        if (GoldhoopbarManager.index == 1)
        {
            this.transform.position = Vector3.zero;
        }
        else
        {
            if (GoldhoopbarManager.index - 2 <= IdlePosList.Count - 1)
                this.transform.position = IdlePosList[GoldhoopbarManager.index - 2].position;
        }
    }

    #region#播放每一个阶段自动引导
    /// <summary>
    /// 金箍棒播放动画播放事件
    /// </summary>
    void OnComplete_XwkAnimEvent()
    {
        GoldhoopbarManager.Instance.NotiOperation(false);
        if (GoldhoopbarManager.index == 1)
        {
            GoldhoopbarManager.Instance.OpenJgbEffect();
            if (GoldhoopbarManager.Instance.moreBigger)
            {
                GotoGuide();
            }
            else
            {
                StartSpeak();
                Invoke("EndSpeak", 2);
                GotoGuide();
            }
        }
        else
        {
            XwkPlayIlde();
            GotoGuide();
        }
    }  
    void XwkPlayIlde()
    {
        GoldhoopbarManager.Instance.SetIdle();
        if (GoldhoopbarManager.index > 2)
        {
            SetIldePosList();
            GoldhoopbarManager.Instance.SwitchXwkAnimControl(GoldfilePath.Instance.xwk_animatorcotrl_guid);
            xwk.Play("idle");          
        }
        else
        {
            if (GoldhoopbarManager.index == 2)
            {
                this.transform.position = Vector3.zero;
                xwk.Play("b_01idle");//金鸡独立
                GoldhoopbarManager.Instance.JgbPlayIdle();
            }
            else
            {
                this.transform.position = Vector3.zero;
                xwk.Play("idle");
                GoldhoopbarManager.Instance.JgbPlayIdle();
                GoldhoopbarManager.Instance.OpenJgbEffect();
            }
        }
    }
    void GotoGuide()
    {
        switch(GoldhoopbarManager.index)
        {
            case 1:
                if(GoldhoopbarManager.Instance.moreBigger)
                {
                    if (!IsInvoking("GotoGuideState"))
                        Invoke("GotoGuideState", 1f);
                }
                else
                {
                    if (!IsInvoking("GotoGuideState"))
                        Invoke("GotoGuideState", 5f);
                }             
                break;
            case 2:
                if (!IsInvoking("GotoGuideState"))
                    Invoke("GotoGuideState", 3f);
                break;
            case 5:
                if (!IsInvoking("GotoGuideState"))
                    Invoke("GotoGuideState", 4f);
                break;
            default:
                if (!IsInvoking("GotoGuideState"))
                    Invoke("GotoGuideState", 0.5f);
                break;
        }
    }
    void GotoGuideState()
    {
        GoldhoopbarManager.Instance.GotoState("GoldGuideState");
    }
    #endregion

    #region#小悟空动作
    /// <summary>
    /// 播放听用户语音动作【独自播放】
    /// </summary>
    public void PlayListenSingle()
    {
        RuntimeAnimatorController anim = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(guidxwk_animatorcotrl, "goldhoopbar", false);
        xwk.runtimeAnimatorController = anim;
        //重新激活animator，重置动画，第一个开始播放        
        ResetTrigger();
        if (GoldhoopbarManager.index >= 2)
        {
            SetIldePosList();
            xwk.Play("wukong_listen_start#anim");
        }
        else
        {
            xwk.transform.position = Vector3.zero;
            xwk.Play("wukong_listen_start#anim");
        }
    }
    public void PlayListenEnd()
    {
        if (GoldhoopbarManager.index >= 2)
        {
            SetIldePosList();
            xwk.SetBool("listenend", true);
        }
        else
        {
            xwk.transform.position = Vector3.zero;
            xwk.SetBool("listenend", true);
        }
    }  
    public void PlayIdle()
    {
        if(GoldhoopbarManager.index==2)
        {
            xwk.SetBool("wkc3_a_02idle", false);
            xwk.SetBool("wkc3_a_02idle", true);//金鸡独立idle
            xwk.transform.position = Vector3.zero;
        }
        else
        {
            xwk.SetBool("idle", false);
            xwk.SetBool("idle", true);
            SetIldePosList();
         }
    }
    void ResetTrigger()
    {
        xwk.SetBool("wkc3_a_02idle", false);
        xwk.SetBool("idle", false);
        xwk.SetBool("listenend", false);
        xwk.SetBool("yaoqingend", false);
        xwk.SetBool("listen", false);
        xwk.SetBool("yaoqing", false);
    }
    public void StartSpeak()
    {
        RuntimeAnimatorController anim = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(guidxwk_animatorcotrl, "goldhoopbar", false);
        xwk.runtimeAnimatorController = anim;
        SetIldePosList();
        ResetTrigger();
        xwk.enabled = false;
        xwk.enabled = true;
        xwk.transform.gameObject.SetActive(false);
        xwk.transform.gameObject.SetActive(true);
        xwk.Play("wukong_yaoqing_start#anim");
    }
    public void EndSpeak()
    {
        ResetTrigger();
        xwk.SetBool("yaoqingend", true);
        xwk.SetBool("idle", true);
    }
    #endregion

    #region##悟空Idle状态下，点击悟空   
    void OnClickXwk()
    {
        if (GoldhoopbarManager.Instance.IsIdleState())
        {
            StartSpeak();
            if (nowClip != null && AnimationEventManager.Instance)
                AnimationEventManager.Instance.UnRegisterAnimationFun(nowClip);
            GoldhoopbarManager.Instance.PlayIdleAudio(EndSpeakDeal);
        }
    }
    void EndSpeakDeal()
    {
        StartCoroutine(IdleEndSpeak());
    }
  
    AnimatorClipInfo[] clips;
    AnimationClip nowClip;
    IEnumerator IdleEndSpeak(int id=0)
    {
        ResetTrigger();             
        xwk.SetBool("yaoqingend", true);          
        yield return null;
        while (true)
        {
            //Debug.LogError("****");
            clips = xwk.GetCurrentAnimatorClipInfo(0);
            yield return null;
            if (clips.Length > 0)
                if (clips[0].clip.name.Equals("wukong_yaoqing01_end#anim"))
                    break;
        }
        if (clips.Length > 0)
        {
            nowClip = clips[0].clip;
            if (!AnimationEventManager.Instance.HaveRegist(clips[0].clip.name))
                AnimationEventManager.Instance.RegisterAnimationFun(clips[0].clip, clips[0].clip.length, "IdleFun");
        }
    }
    void IdleFun()
    {
        if(GoldhoopbarManager.index==1)
        {
            xwk.SetBool("idle",true);
            xwk.transform.position = Vector3.zero;
        }
        else if (GoldhoopbarManager.index == 2)
        {           
                xwk.SetBool("idle", true);
                if (xwk.GetBool("idle"))
                SetIldePosList();                     
        }
        else 
        {
            xwk.SetBool("idle", true);
            if (xwk.GetBool("idle"))
                SetIldePosList();
        }
    }
    #endregion
}
