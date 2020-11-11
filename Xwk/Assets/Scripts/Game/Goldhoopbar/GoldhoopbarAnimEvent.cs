using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldhoopbarAnimEvent : MonoBehaviour {
    Animator anim;
    void Awake()
    {
        anim = this.GetComponent<Animator>();
    }
    /// <summary>
    /// 金箍棒播放动画完成+注册声音事件
    /// </summary>
    void OnComplete_GoldhoopbarEvent()
    {
        GoldhoopbarManager.Instance.PlayBiggerOrSmallAudio();  
    }  
    public void PlayIdle()
    {
        anim.Play("jgb_c3_b_01idle");
    }

}
