using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using Xbl;
using YB.XWK.MainScene;
using XWK.Common.UI_Reward;

public class XwkAnimationEvent:MonoBehaviour
{
    bool talkFinsh = false;
    bool wukongOut = false;
    bool wk_ty_intoFun = false;
    bool leaveFun = false;
    string Effect_wkcx = "public/Hero_Effect/prefab/effect_ct_chuxian";
    GameObject effect;
    //落地
    void wukong_ty_intoFun()
    {
        if (wk_ty_intoFun)
            return;
        wk_ty_intoFun = true;
        AudioClip clip2 = ABResMgr.Instance.LoadResource<AudioClip>("game/Separation/Sound/common_191", "separation");//欧耶，耶耶耶
        AudioManagerExtern.Instance.PlaySmallClipSound(clip2);
        AudioClip clipef = ABResMgr.Instance.LoadResource<AudioClip>("game/Separation/SoundEffect/public_xwkyx_011", "separation");
        AudioManager.Instance.PlaySound(clipef);
        Invoke("GroundEffect", 0.25f);
        Invoke("VictorySound", 1f);
    }
    void GroundEffect()
    {
        effect = ABResMgr.Instance.LoadResource<GameObject>(Effect_wkcx,ABCommonConfig.EfBundleType, true);
        Vector3 targetPos = new Vector3(WindowSliderControl.Instance.GetCameraPos().x, 0, 0);
        effect.transform.position = targetPos;
        Destroy(effect, 2);
    }
    void VictorySound()
    {       
        AudioClip clipef2 = ABResMgr.Instance.LoadResource<AudioClip>("game/Separation/SoundEffect/public_xwkyx_012", "separation");
        AudioManager.Instance.PlayerSoundByClip(clipef2, null);
    }
    void TalkFun()
    {
        if (talkFinsh)
            return;
        talkFinsh = true;
        int soundIndex = UnityEngine.Random.Range(91, 95);
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/Separation/Sound/common_" + soundIndex.ToString(), "separation");
        AudioManager.Instance.PlayerSoundByClip(clip, WukongOut);
    }
    

    void WukongOut()
    {
        if (wukongOut)
            return;
        wukongOut = true;
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/Separation/Sound/common_95", "separation");
        AudioManager.Instance.PlayerSoundByClip(clip, SeparationManager.Instance.finishstate.WukongOut);
    }
    void LeaveFun()
    {
        if (leaveFun)
            return;
        leaveFun = true;
        SeparationManager.Instance.GameOver();
    }
}

public class FinishState : C_IState,IRubbish
{
    GameObject FinishXwk = null;//最后实例化小悟空
    Camera MainCamera;
    Animator xwk;
    AnimationClip[] finishClipsArray;
    bool finish = false;
    string Finish_RuntimeAnimatorController = "game/Separation/animatorcontroller/finish";//9个小悟空，入场控制器
    RuntimeAnimatorController anim;
    public string Name { get; set; }   
    public virtual void OnStateEnter()
    {
        Name = "FinishState";
        MainCamera = GameObject.Find("SeparationCamera").GetComponent<Camera>();
        FindSunwukong();
    }
    public void RecycleRubbish()
    {
        SeparationManager.Instance.StopAllCoroutines();
        AudioManager.Instance.StopAllSounds();
        if (anim != null)
            anim = null;
        if (FinishXwk)
            GameObject.Destroy(FinishXwk);
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
    void FindSunwukong()
    {
        if (finish)
            return;
        finish = true;
        WindowSliderControl.Instance.DFrozenCamera();//禁用相机移动  
        //实例化一个小悟空
        FinishXwk = ABResMgr.Instance.LoadResource<GameObject>(SepPath.Instance.Xwk, "separation", true,true);
        FinishXwk.SetActive(true);
        FinishXwk.AddComponent<XwkAnimationEvent>();
        xwk = FinishXwk.GetComponent<Animator>();
        anim = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(Finish_RuntimeAnimatorController, "separation", false);
        finishClipsArray = anim.animationClips;
        xwk.runtimeAnimatorController = anim;
        if (MainCamera != null)
            FinishXwk.transform.position = new Vector3(MainCamera.transform.position.x, 0, 0);
        AddEvent();

        //结算
        RewardUIManager.Instance.RegisterStory(5, SourceType.SpriteWindow,1,null,"fengshenshuGame");//30   10   // 31 32  33  34 .....40
        RewardUIManager.Instance.SetSuccess();
    }
    void AddEvent()
    {
        //播放顺序  跳入-》胜利动作-》说话动作-》跳出(离开)
        for (int i = 0; i < finishClipsArray.Length; i++)
        {
            if (finishClipsArray[i].name.Equals("wukong_ty_talk01_loop#anim"))
            {
                if(!AnimationEventManager.Instance.HaveRegist("wukong_ty_talk01_loop#anim"))
                {
                    AnimationEventManager.Instance.RegisterAnimationFun(finishClipsArray[i], 0.1f, "TalkFun");
                }
            }
            else if (finishClipsArray[i].name.Equals("wukong_ty_out01#anim"))
            {
                   if (finishClipsArray[i].length - 1f>0)//场景跳转时间
                    if (!AnimationEventManager.Instance.HaveRegist("wukong_ty_out01#anim"))
                    {
                        AnimationEventManager.Instance.RegisterAnimationFun(finishClipsArray[i], finishClipsArray[i].length - 1f, "LeaveFun");
                    }
            }
            else if (finishClipsArray[i].name.Equals("wukong_ty_into01#anim"))
            {
                if (!AnimationEventManager.Instance.HaveRegist("wukong_ty_into01#anim"))
                {
                    AnimationEventManager.Instance.RegisterAnimationFun(finishClipsArray[i], 0.5f, "wukong_ty_intoFun");
                }
            }
        }
    }
    public void WukongOut()
    {
        if(xwk)
        xwk.Play("wukong_ty_out01#anim");
    }
   

}
