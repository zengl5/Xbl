using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using DG.Tweening;
using Xbl;
public class GoldBigState : C_IState, IRubbish
{
    public string Name { get; set; }
    SceneInfo sf;
    bool addwkAnimEvent = true;
    bool addCameraAnimEvent = true;
    public List<GameObject> RubbishList = new List<GameObject>();
    public GoldBigState(SceneInfo sif)
    {
        sf = sif;
    }
    public virtual void OnStateEnter()
    {
        addCameraAnimEvent = false;
        Name = "GoldBigState";
        BigDeal();         //变大
    }
    public void RecycleRubbish()
    {
        C_TimerMgr.Instance.RemoveTimer("OutAudio");       
        for (int i = 0; i < RubbishList.Count; i++)
            if (RubbishList[i] != null)
                GameObject.Destroy(RubbishList[i]);
    }
    public virtual void OnStateLeave()
    {
        addwkAnimEvent = true;
        addCameraAnimEvent = true;
        C_TimerMgr.Instance.RemoveTimer("OutAudio");
        //if (effect_jgb_c3a)
        //    GameObject.Destroy(effect_jgb_c3a);
    }

    public virtual void OnStateOverride()
    {

    }

    public virtual void OnStateResume()
    {

    }
    //动态注册事件，每次进入（BigState）执行一次
    public void Update_AddAnimatorEvent()
    {
        if (!addwkAnimEvent && sf.XwkPrefab != null)
        {
            if (sf.XwkPrefab.GetComponent<XwkAnimEvent>() == null)
                sf.XwkPrefab.gameObject.AddComponent<XwkAnimEvent>();//添加事件
            if (sf.GoldHoopBarPrefab.GetComponent<GoldhoopbarAnimEvent>() == null)
                sf.GoldHoopBarPrefab.GetAddComponent<GoldhoopbarAnimEvent>();//添加事件
            AnimatorClipInfo[] clips = sf.XwkPrefab.GetCurrentAnimatorClipInfo(0);
            AnimatorClipInfo[] clipsJGB = sf.GoldHoopBarPrefab.GetCurrentAnimatorClipInfo(0);
            if (clips.Length > 0)
            {
                addwkAnimEvent = true;
                AnimationEventManager.Instance.RegisterAnimationFun(clips[0].clip,clips[0].clip.length, "OnComplete_XwkAnimEvent");
            }
            if (clipsJGB.Length > 0)
            {
                addwkAnimEvent = true;//金箍棒事件动画执行一半的时候执行
                AnimationEventManager.Instance.RegisterAnimationFun(clipsJGB[0].clip, 0.5F * clipsJGB[0].clip.length, "OnComplete_GoldhoopbarEvent");
            }
        }
        if (!addCameraAnimEvent && sf.CameraPrefab != null)
        {
            if (sf.CameraPrefab.GetComponent<CameraAnimEvent>() == null)
                sf.CameraPrefab.GetAddComponent<CameraAnimEvent>();
            AnimatorClipInfo[] clips = sf.CameraPrefab.GetCurrentAnimatorClipInfo(0);
            if (clips.Length > 0)
            {
                RegisterCameraAnimationFun(clips[0].clip);
            }
        }
    }
    /// <summary>
    /// 自动添加相机动画帧事件
    /// </summary>
    /// <param name="clip"></param>
    void RegisterCameraAnimationFun(AnimationClip clip)
    {
        if (GoldhoopbarManager.index == 4)
        {
            addCameraAnimEvent = true;
            AnimationEventManager.Instance.RegisterAnimationFun(clip, 0.1f, "FovSetting", "0X40X70");
        }
    }


    void BigDeal()
    {
        RuntimeAnimatorController anim = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(GoldfilePath.Instance.xwk_animatorcotrl, "goldhoopbar", false);
        sf.XwkPrefab.runtimeAnimatorController = anim;
        if (GoldhoopbarManager.index >= 5)
        {
            //当前已经是最大的状态
            GoldhoopbarManager.Instance.PlayCharacterAudio("common_138", GotoGuide);
            return;
        }
        GoldhoopbarManager.index++;
        if (GoldhoopbarManager.index != 1)
        {
            GoldhoopbarManager.Instance.CloseJgbEffect();
                //140 242 185 265
                float[] animLengthArray = new float[] { 0.1f, 6.3f, 2.96f, 7.56f };//阶段时间  【2-3】计算=帧数/30
                AudioClip clipef = ABResMgr.Instance.LoadResource<AudioClip>("game/Goldhoopbar/soundeffect/public_xwkyx_008", "goldhoopbar");
                if (GoldhoopbarManager.index - 1 <= animLengthArray.Length)
                    AudioManagerExtern.Instance.PlayFixedLoopClip(clipef, animLengthArray[GoldhoopbarManager.index - 2]);            
        }
        else
        {
            GoldhoopbarManager.Instance.StartCoroutine(AddJgbEffect());
        }
        sf.XwkPrefab.transform.position = Vector3.zero;
        sf.CameraPrefab.Play("cam_big" + GoldhoopbarManager.index);
        sf.XwkPrefab.Play("xwk_big"+GoldhoopbarManager.index);
        sf.GoldHoopBarPrefab.Play("jgb_big" + GoldhoopbarManager.index);

        addwkAnimEvent = false;
        addCameraAnimEvent = false;

        C_TimerMgr.Instance.AddTimer(2F, OutAudio, "OutAudio");
#if !UNITY_EDITOR
        Handheld.Vibrate();
#endif
    }
    void GotoGuide()
    {
        GoldhoopbarManager.Instance.GotoState("GoldGuideState");
    }
    //出来声音
    void OutAudio()
    {
        if (GoldhoopbarManager.index == 1)
        {
            GoldhoopbarManager.Instance.PlayCharacterAudio("common_116_1");//出来         
        }
    }
    IEnumerator AddJgbEffect()
    {
        yield return new WaitForSeconds(4.0f);
        if (GoldhoopbarManager.index == 1)
        {
            GoldhoopbarManager.Instance.OpenJgbEffect();
        }
    }
}
