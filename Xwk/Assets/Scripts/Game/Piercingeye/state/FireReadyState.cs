using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using DG.Tweening;
using Xbl;
using XBL.Core;
using System;
using XWK.Common.UI_Reward;
/// <summary>
/// 引导小悟空合体滑动
/// </summary>
public class FireReadyState : C_IState, IRubbish
{
    Vector2 startTouchpos;
    Vector2 movingTouchpos;
    bool DetectSlider = false;
    bool OpenFireEye = false;
    public string Name { get; set; }
    Animator xwk;
    Animator cam;
    FireXwk firexwk;
    bool openProgress = false;
    string cameraClipName = "cam06_05#anim";
    Camera hdtwCam;//指尖滑动相机
    GameObject ui_public_effect_hdtw;
    GameObject hdtw;
    WaitForSeconds zs = new WaitForSeconds(1f);
    WaitForSeconds lwait = new WaitForSeconds(3f);
    WaitForSeconds lgwait = new WaitForSeconds(3.5f);
    WaitForSeconds rswait = new WaitForSeconds(1f);
    #region
    public FireReadyState(GameObject xwkobj, GameObject camobj)
    {
        Name = "FireReadyState";
        xwk = xwkobj.GetComponent<Animator>();
        cam = camobj.GetComponent<Animator>();
        firexwk = xwk.GetComponent<FireXwk>();
        JsonManager.Instance.ReadJson("Config/Piercingeye/fireSprit.json", null);
    }
    public virtual void OnStateEnter()
    {
        StartZhuanshen();
        AddMoveEffect();
        RewardUIManager.Instance.ChangeModule(ModuleType.SpriteUnlock, "Piercingeye");
    }
    public void RecycleRubbish()
    {
        if (hdtw)
            GameObject.Destroy(hdtw);
        C_UIMgr.Instance.CloseUI("UI_effect_CA");
        AnimationEventManager.Instance.UnRegisterAllAnimationFun();
        PiercingeyeManager.Instance.StopCoroutine(RegisterCameraAnimation());
    }
    public virtual void OnStateLeave()
    {
        RecycleRubbish();
        PiercingeyeManager.Instance.StopCoroutine(RegisterCameraAnimation());
        C_TimerMgr.Instance.RemoveTimer("DetectSlider");
        AnimationEventManager.Instance.UnRegisterAllAnimationFun();
        firexwk.RecycleRubbish();
    }
    public virtual void OnStateOverride()
    {

    }
    public virtual void OnStateResume()
    {

    }

    #endregion    
    void StartZhuanshen()
    {
        DetectSlider = false;
        OpenFireEye = false;
        xwk.SetBool("step2", false);
        xwk.SetBool("step4", false);
        cam.SetBool("step2", false);
        cam.SetBool("step4", false);
        PiercingeyeManager.Instance.PlayCharacterAudio("common_106", firexwk.Init2StepEffect);//合体
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/Piercingeye/sound/public_xwkbgm_002", "Piercingeye");
        AudioManager.Instance.PlayBgMusic(clip);
        PiercingeyeManager.Instance.StartCoroutine(Zhuanshen());
    }
    /// <summary>
    /// 实例化指尖滑动特效
    /// </summary>
    void AddMoveEffect()
    {
        hdtw = ABResMgr.Instance.LoadResource<GameObject>("game/Piercingeye/prefabs/hdtwCamera", "Piercingeye", true);
        hdtwCam = hdtw.GetComponent<Camera>();
        if (ui_public_effect_hdtw == null)
        {
            ui_public_effect_hdtw = hdtwCam.transform.GetChild(0).gameObject;
            ui_public_effect_hdtw.SetActive(false);
        }
    }

    IEnumerator Zhuanshen()
    {
        yield return zs;
        PiercingeyeManager.Instance.PlayEffectAudio("public_xwkyx_031");
    }
   

    public void OpenHeti()
    {
        if (openProgress)
            return;
        RewardUIManager.Instance.RegisterSpriteUnlock(1,null,"Piercingeye");
        RewardUIManager.Instance.SetSuccess();
        openProgress = true;
        PiercingeyeManager.Instance.PlaySceneAudio("public_xwkyx_029");//音效
        firexwk.ShowInputEffect(true);
        firexwk.CloseHandDirector();
        PiercingeyeManager.Instance.StartCoroutine(HetiSuccessEvent());
    }
    /// <summary>
    /// 3s后，合体完成
    /// </summary>
    IEnumerator HetiSuccessEvent()
    {
        yield return lwait;
        if(DirectorMgr.DirectorAllState)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, YB.XWK.MainScene.LocalData.m_game_huoyanjingjing, "successHeti");
        }
        else
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, YB.XWK.MainScene.LocalData.m_game_huoyanjingjing, "successHeti_NewUser");
        }
        PiercingeyeManager.Instance.PlaySceneAudio("public_xwkyx_032");//开眼音效
        // Debug.LogError("合体成功");
        xwk.SetBool("step2", true);
        cam.SetBool("step2", true);
        firexwk.Init3StepEffect();
        firexwk.ShowInputEffect(false);
        C_UIMgr.Instance.CloseUI("UI_effect_CA");
        yield return lgwait;
        PiercingeyeManager.Instance.PlayYaoqingAudio("common_107");//邀请
        DetectSlider = true;
    }         
     

    #region#右滑检测
    public void Update()
    {
        if (DetectSlider)
            Update_DetectTouchLeft();
    }
    /// <summary>
    /// 检测右侧滑动
    /// </summary>
    void Update_DetectTouchLeft()
    {
        if (TouchManager.Instance.IsTouchValid(0))
        {
            TouchPhaseEnum phase = (TouchPhaseEnum)TouchManager.Instance.GetTouchPhase(0);
            if (phase == TouchPhaseEnum.BEGAN)
            {
                TouchManager.Instance.GetTouchPos(0, out startTouchpos);
            }
            else if (phase == TouchPhaseEnum.MOVED)
            {
                TouchManager.Instance.GetTouchPos(0, out movingTouchpos);
                OpenMoveFingerEffect();
            }
            else if (phase == TouchPhaseEnum.ENDED)
            {
                CloseFingerEffect();
                if (movingTouchpos.x - startTouchpos.x > 200)
                {
                    OpenPiercingeye();
                    DetectSlider = false;
                }
            }
        }
    }
    void OpenPiercingeye()
    {
        if (DirectorMgr.DirectorAllState)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, YB.XWK.MainScene.LocalData.m_game_huoyanjingjing, "OpenPiercingeye");
        }
        else
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, YB.XWK.MainScene.LocalData.m_game_huoyanjingjing, "OpenPiercingeye_NewUser");
        }
        PiercingeyeManager.Instance.PlaySceneAudio("public_xwkyx_030");//音效
        if (!DetectSlider)
            return;
        if (OpenFireEye)
            return;
        OpenFireEye = true;

        RewardUIManager.Instance.RegisterSpriteUnlock(2, null, "Piercingeye");
        RewardUIManager.Instance.SetSuccess();

        //1：触发火眼金睛特效2：关闭UI 3:开启第一视角（陀螺仪功能）4：生成一个小葫芦
        xwk.SetBool("step4", true);
        cam.SetBool("step4", true);
        firexwk.Init5stepEffect();
        PiercingeyeManager.Instance.StartCoroutine(RegisterCameraAnimation());
    }
    IEnumerator RegisterCameraAnimation()
    {
        yield return rswait;
        AnimatorClipInfo[] clips = cam.GetCurrentAnimatorClipInfo(0);
        if (clips[0].clip.name.Equals(cameraClipName))
        {
            //Debug.LogError(clips[0].clip.name);
            AnimationEventManager.Instance.RegisterAnimationFun(clips[0].clip, 2f, "FovSetting_Piercingeye", "60X90X40");
            AnimationEventManager.Instance.RegisterAnimationFun(clips[0].clip, 7f, "FovSetting_Piercingeye", "210X220X17");
            AnimationEventManager.Instance.RegisterAnimationFun(clips[0].clip, clips[0].clip.length - 0.2f, "GotoFindHulu_Piercingeye");
        }
        yield return new WaitForSeconds(1f);
        PiercingeyeManager.Instance.PlayCharacterAudio("common_108");//火眼金睛
    }
    /// <summary>
    /// 移动指尖特效
    /// </summary>
    void OpenMoveFingerEffect()
    {
        if (!DetectSlider)
            return;
        Vector3 screenPos = hdtwCam.WorldToScreenPoint(ui_public_effect_hdtw.transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = screenPos.z;//因为鼠标只有X，Y轴，所以要赋予给鼠标Z轴
        Vector3 worldPos = hdtwCam.ScreenToWorldPoint(mousePos);
        ui_public_effect_hdtw.transform.localPosition = worldPos;//控制物体移动
        ui_public_effect_hdtw.transform.parent.gameObject.SetActive(true);
        ui_public_effect_hdtw.SetActive(true);
    }
    void CloseFingerEffect()
    {
        if (ui_public_effect_hdtw)
        {
            ui_public_effect_hdtw.transform.parent.gameObject.SetActive(false);
        }
    }
}
#endregion

