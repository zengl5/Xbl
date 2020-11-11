using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;

public class GoldGuideState : C_IState, IRubbish
{
    /// <summary>
    /// 引导状态  1：UI按钮  2：每次变大之后引导 3：每个状态动画播放完
    /// </summary>
    public string Name { get; set; }
    SceneInfo sf;
    string smallSpecialName = "f";
    bool LoopPlay = false;
    public GoldGuideState(SceneInfo sif)
    {
        sf = sif;
    }
    public virtual void OnStateEnter()
    {

        //每次变完之后的指引
        Name = "GoldGuideState";
        //动画播放完成，进入指引，等待操作。无操作则提醒操作 
        RecycleRubbish();
        Initguide();
    }
    public void RecycleRubbish()
    {
        GoldhoopbarManager.Instance.StopAllCoroutines();
        C_TimerMgr.Instance.RemoveTimer("Againguide");
        C_TimerMgr.Instance.RemoveTimer("UserNoOperation");
        C_TimerMgr.Instance.RemoveTimer("QuitPlayGoldhoopbar");
        C_TimerMgr.Instance.RemoveTimer("NoOperation");
    }
    public virtual void OnStateLeave()
    {
        GoldhoopbarManager.Instance.StopAllCoroutines();
        C_TimerMgr.Instance.RemoveTimer("Againguide");
        C_TimerMgr.Instance.RemoveTimer("UserNoOperation");
        C_TimerMgr.Instance.RemoveTimer("QuitPlayGoldhoopbar");
        C_TimerMgr.Instance.RemoveTimer("NoOperation");
        LoopPlay = false;
    }

    public virtual void OnStateOverride()
    {
    }

    public virtual void OnStateResume()
    {
    }

    void Initguide()
    {
        //Debug.Log("引导喊话");//金箍棒可以变大变小喔，我们一起来喊吧！
        sf.XwkPrefab.GetComponent<XwkAnimEvent>().StartSpeak();
        if (GoldhoopbarManager.index == 1)
        {
            GoldhoopbarManager.Instance.PlayCharacterAudio("common_122", delegate
           {
               sf.XwkPrefab.GetComponent<XwkAnimEvent>().EndSpeak();
               GoldhoopbarManager.Instance.StartCoroutine(ShowRecognize());
           }
            );
        }
        else if(GoldhoopbarManager.index == 5)
        {
            GoldhoopbarManager.Instance.PlayCharacterAudio("common_252", delegate
            {
                sf.XwkPrefab.GetComponent<XwkAnimEvent>().EndSpeak();
                GoldhoopbarManager.Instance.StartCoroutine(ShowRecognize());
            }
          );
        }
        else
            {
            //如果3S 以后没有操作或语音识别错误，再次执行
            //C_TimerMgr.Instance.AddTimer(15, Againguide, "Againguide");
            int rdIndex = Random.Range(0, 5);
            if (rdIndex == 0)
                GoldhoopbarManager.Instance.PlayCharacterAudio("common_248", GotoRecognize);
            else if (rdIndex == 1)
                GoldhoopbarManager.Instance.PlayCharacterAudio("common_249", GotoRecognize);
            else if (rdIndex == 2)
                GoldhoopbarManager.Instance.PlayCharacterAudio("common_250", GotoRecognize);
            else if (rdIndex == 3)
                GoldhoopbarManager.Instance.PlayCharacterAudio("common_251", GotoRecognize);
            else if (rdIndex == 4)
                GoldhoopbarManager.Instance.PlayCharacterAudio("common_252", GotoRecognize);          
        }      
        
    }

    void GotoRecognize()
    {
        sf.XwkPrefab.GetComponent<XwkAnimEvent>().EndSpeak();
        GoldhoopbarManager.Instance.StartCoroutine(ShowRecognize());
    }
    WaitForSeconds srwait = new WaitForSeconds(0.6f);
    IEnumerator ShowRecognize()
    {
        yield return srwait;
        GoldhoopbarManager.Instance.ShowRecognizeEffect(true);
    }
    //再次引导 当Idle语音播放完成后
    public void Againguide()
    {
        //RuntimeAnimatorController anim = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(guidxwk_animatorcotrl, "goldhoopbar", false);
        //sf.XwkPrefab.runtimeAnimatorController = anim;
        sf.XwkPrefab.GetComponent<XwkAnimEvent>().StartSpeak();
        GoldhoopbarManager.Instance.PlayCharacterAudio("common_61", delegate
        {
            sf.XwkPrefab.GetComponent<XwkAnimEvent>().EndSpeak();
            GoldhoopbarManager.Instance.StartCoroutine(ShowRecognize());
        }
        );
    }
  
     
    
}
