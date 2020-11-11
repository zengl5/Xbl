using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hulu : MonoBehaviour,IRubbish {

    System.Action action;
    Animator anim;
    Transform Point003Pos;
    public List<GameObject> RubbishList = new List<GameObject>();
    void Awake () {
        anim = this.GetComponent<Animator>();
        Point003Pos = transform.GetChild(1).GetChild(0).GetChild(1);
    }
    public void RecycleRubbish()
    {
        for (int i = 0; i < RubbishList.Count; i++)
            if (RubbishList[i] != null)
                GameObject.Destroy(RubbishList[i]);
    }
    /// <summary>
    /// 变
    /// </summary>
    public void ReadyShow()
    {
        anim.Play("jl00001_2_bian02#anim");
    }
    public void Idle()
    {
        anim.Play("jl00001_2_stand01#anim");
    }
    public void Speak()
    {
        anim.Play("jl00001_2_talk04#anim");
        Invoke("PlayShakeSound", 1.5f);
    }
    void PlayShakeSound()
    {
        HuluManager.Instance.PlayEffectAudio("public_xwkyx_051");
    }
    public void ShowDaijiEffect()
    {
        GameObject public_effect_bbhl_daiji = ABResMgr.Instance.LoadResource<GameObject>(HlfilePath.Instance.public_effect_bbhl_daiji, ABCommonConfig.EfBundleType, true);
        public_effect_bbhl_daiji.transform.parent = Point003Pos;
        public_effect_bbhl_daiji.transform.localScale =Vector3.one;
        public_effect_bbhl_daiji.transform.localPosition =Vector3.zero;
        public_effect_bbhl_daiji.transform.gameObject.SetActive(true);
        RubbishList.Add(public_effect_bbhl_daiji);
    }
    public void PlayShake(System.Action ac)
    {
        PlayShakeSound();//播放摇晃
        anim.Play("jl00001_2_shifa01#anim");
        action = ac;
        DoAction();
    }
    void DoAction()
    {
        if (action != null)
            action();
    }
}
