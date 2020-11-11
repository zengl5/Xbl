using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using Xbl;
public class GoldSamllState : C_IState, IRubbish
{

    public string Name { get; set; }
    SceneInfo sf;
    string smallSpecialName = "f";
    bool addCameraAnimEvent = true;
    bool addwkAnimEvent = true;
 

    public GoldSamllState(SceneInfo sif)
    {
        sf = sif;
    }
    public virtual void OnStateEnter()
    {
        addCameraAnimEvent = true;
        addwkAnimEvent = true;
        Name = "GoldSamllState";
        SmallDeal();
    }
    public void RecycleRubbish()
    {
       
    }
    public virtual void OnStateLeave()
    {
      
    }

    public virtual void OnStateOverride()
    {

    }

    public virtual void OnStateResume()
    {

    }
    //动态注册事件，每次进入（SmallState）执行一次
    public void Update_AddAnimatorEvent()
    {
        if (!addwkAnimEvent && sf.XwkPrefab != null)
        {
            if (sf.XwkPrefab.GetComponent<XwkAnimEvent>() == null)
                sf.XwkPrefab.GetAddComponent<XwkAnimEvent>();//添加事件
            AnimatorClipInfo[] clips = sf.XwkPrefab.GetCurrentAnimatorClipInfo(0);


            if (clips.Length > 0)
            {
                addwkAnimEvent = true;
                if (!AnimationEventManager.Instance.HaveRegist(clips[0].clip.name))
                    AnimationEventManager.Instance.RegisterAnimationFun(clips[0].clip, clips[0].clip.length, "OnComplete_XwkAnimEvent");
            }
            if (sf.GoldHoopBarPrefab.GetComponent<GoldhoopbarAnimEvent>() == null)
                sf.GoldHoopBarPrefab.GetAddComponent<GoldhoopbarAnimEvent>();//添加事件
            AnimatorClipInfo[] clipsJGB = sf.GoldHoopBarPrefab.GetCurrentAnimatorClipInfo(0);
            if (clipsJGB.Length > 0)
            {
                addwkAnimEvent = true;//金箍棒事件动画执行一半的时候执行
                AnimationEventManager.Instance.RegisterAnimationFun(clipsJGB[0].clip, 0.5F * clipsJGB[0].clip.length, "OnComplete_GoldhoopbarEvent");
            }
        }
        if (!addCameraAnimEvent && sf.CameraPrefab != null)
        {
            AnimatorClipInfo[] clips = sf.CameraPrefab.GetCurrentAnimatorClipInfo(0);
            if (clips.Length >0)
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
        if (GoldhoopbarManager.index == 4|GoldhoopbarManager.index==3)
        {
            addCameraAnimEvent = true;
            AnimationEventManager.Instance.RegisterAnimationFun(clip, 0.5f, "FovSetting", "0X70X40");
        }
    }
    void SmallDeal()
    {
        GoldhoopbarManager.Instance.SwitchXwkAnimControl(GoldfilePath.Instance.xwk_animatorcotrl);
        if (GoldhoopbarManager.index == 1)
        {
            //当前已经是最小的状态
            //播放Idle
            //GoldhoopbarManager.Instance.PlayIlde2();
            sf.XwkPrefab.GetComponent<XwkAnimEvent>().StartSpeak();
            GoldhoopbarManager.Instance.PlayCharacterAudio("common_126", delegate
            {
                sf.XwkPrefab.GetComponent<XwkAnimEvent>().PlayListenEnd();
                GoldhoopbarManager.Instance.ShowRecognizeByIcon(null);
            });
            return;
        }
        // GoldhoopbarManager.Instance.PlaySmallStateAudio();
        float[] animLengthArray = new float[] { 0.83f, 3.56f, 3f, 3f };//阶段时间  【2-3】计算=帧数/30
        AudioClip clipef = ABResMgr.Instance.LoadResource<AudioClip>("game/Goldhoopbar/soundeffect/public_xwkyx_009", "goldhoopbar");

        if(GoldhoopbarManager.index-2>=0&&GoldhoopbarManager.index-2<=3)
        AudioManagerExtern.Instance.PlayFixedLoopClip(clipef, animLengthArray[GoldhoopbarManager.index - 2]);

      
        GoldhoopbarManager.index--;
        sf.XwkPrefab.transform.position = Vector3.zero;
        sf.CameraPrefab.Play("cam_small" + GoldhoopbarManager.index);
        sf.XwkPrefab.Play("xwk_small" + GoldhoopbarManager.index);
        sf.GoldHoopBarPrefab.Play("jgb_small" + GoldhoopbarManager.index);
        //addwkAnimEvent = false;

        GoldhoopbarManager.Instance.StartCoroutine(AddEvent());
#if !UNITY_EDITOR
                Handheld.Vibrate();
#endif
    }
    IEnumerator AddEvent()
    {
        yield return new WaitForSeconds(0.2f);
        addwkAnimEvent = false;
        addCameraAnimEvent = false;
    }
}
