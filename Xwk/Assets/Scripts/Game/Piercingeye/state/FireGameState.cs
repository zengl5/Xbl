using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using DG.Tweening;
using Xbl;
using YB.XWK.MainScene;
using UnityEngine.UI;

public class FireGameState : C_IState, IRubbish
{
    GameObject startcam;
    GameObject gyrocam;
    GameObject jljiaticam;
    GameObject jlcam;
    GameObject xwk;
    GameObject jl_jiati;
    GameObject ui_public_effect_suoding;
    bool Hithulu = false;
    bool init = false;
    GameObject EffectUICam;
    GameObject EffectUui_public_effect_mengbanICam;
    SpritConfig spConfig;
    DialogInfo dialogInfo;
    FazhenUI fzui;
    GameObject public_effect_shoudianji;
    GameObject SkyBoxCam;
    bool oPenFazhen = false;
    WaitForSeconds longwait = new WaitForSeconds(8.0f);
    WaitForSeconds longaswait = new WaitForSeconds(3.0f);
    List<GameObject> RubbishList = new List<GameObject>();
    public string Name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="camobj">初始相机，带位移动画</param>
    /// <param name="gyrocamobj">陀螺仪相机</param>
    /// <param name="jlcamobj">精灵相机（假体）</param>
    /// <param name="xwkobj">精灵相机（真身）</param>
    public FireGameState(GameObject xwkobj)
    {
        xwk = xwkobj;
    }
    public void InitCamera(GameObject initCam, GameObject gyroCam, GameObject jljiatiCam, GameObject jlCam)
    {
        startcam = initCam;
        gyrocam = gyroCam;
        jljiaticam = jljiatiCam;
        jlcam = jlCam;
    }
    public virtual void OnStateEnter()
    {
        Name = "FireGameState";
        Start();
    }    
    
    public void RecycleRubbish()
    {
        init = false;
        if (public_effect_shoudianji)
            GameObject.Destroy(public_effect_shoudianji);

        for (int i = 0; i < RubbishList.Count; i++)
            if (RubbishList[i] != null)
                GameObject.Destroy(RubbishList[i]);
    }
    public virtual void OnStateLeave()
    {
        init = false;
    }
    public virtual void OnStateResume()
    {

    }
    public virtual void OnStateOverride()
    {
    }


    public void AddRubbish(GameObject rb)
    {
        RubbishList.Add(rb);
    }
    void Start()
    {
        if(Application.isEditor)
        {
            JsonManager.Instance.ReadJson("Config/Piercingeye/fireSprit.json", ReadJsonData);
            //ReadJsonData(JsonManager.Instance.GetNowJsonData());
        }
        else
        {
            JsonManager.Instance.ReadJson("Config/Piercingeye/fireSprit.json", ReadJsonData);
        }
    }

    void ReadJsonData(LitJson.JsonData data)
    {
        FireData nowdata = new FireData();
        nowdata.ReadJsonData(data, InitSpriteConfig);
    }
    void InitSpriteConfig(SpritConfig sp, DialogInfo info)
    {
        spConfig = sp;
        dialogInfo = info;
        if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_BaiYin))
        {
            dialogInfo.Tpinfo = DialogConfig.Instance.fireTop;
            dialogInfo.Ctinfo = DialogConfig.Instance.fireCenter;
            dialogInfo.Ltinfo = DialogConfig.Instance.fireLast;
        }
        else if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_Hulu))
        {
            dialogInfo.Tpinfo = DialogConfig.Instance.huluTop;
            dialogInfo.Ctinfo = DialogConfig.Instance.huluCenter;
            dialogInfo.Ltinfo = DialogConfig.Instance.huluLast;
        }
        else if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_Xln))
        {
            dialogInfo.Tpinfo = DialogConfig.Instance.xlnTop;
            dialogInfo.Ctinfo = DialogConfig.Instance.xlnCenter;
            dialogInfo.Ltinfo = DialogConfig.Instance.xlnLast;
        }
        else if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_lingwa))
        {
            dialogInfo.Tpinfo = DialogConfig.Instance.lingwaTop;
            dialogInfo.Ctinfo = DialogConfig.Instance.lingwaCenter;
            dialogInfo.Ltinfo = DialogConfig.Instance.lingwaLast;
        }
        else if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_xcww))
        {
            dialogInfo.Tpinfo = DialogConfig.Instance.xcwwTop;
            dialogInfo.Ctinfo = DialogConfig.Instance.xcwwCenter;
            dialogInfo.Ltinfo = DialogConfig.Instance.xcwwLast;
        }
        else if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_jl_hxjl))
        {
            dialogInfo.Tpinfo = DialogConfig.Instance.hxjlTop;
            dialogInfo.Ctinfo = DialogConfig.Instance.hxjlCenter;
            dialogInfo.Ltinfo = DialogConfig.Instance.hxjlLast;
        }//新增
        else if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_dazuishou))
        {
            dialogInfo.Tpinfo = DialogConfig.Instance.Top1;
            dialogInfo.Ctinfo = DialogConfig.Instance.Center1;
            dialogInfo.Ltinfo = DialogConfig.Instance.Last1;
        }
        else if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_menmenmiao))
        {
            dialogInfo.Tpinfo = DialogConfig.Instance.Top2;
            dialogInfo.Ctinfo = DialogConfig.Instance.Center2;
            dialogInfo.Ltinfo = DialogConfig.Instance.Last2;
        }
        else if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_huohuolong))
        {
            dialogInfo.Tpinfo = DialogConfig.Instance.Top3;
            dialogInfo.Ctinfo = DialogConfig.Instance.Center3;
            dialogInfo.Ltinfo = DialogConfig.Instance.Last3;
        }
        else if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_xiuxiuwoniu))
        {
            dialogInfo.Tpinfo = DialogConfig.Instance.Top4;
            dialogInfo.Ctinfo = DialogConfig.Instance.Center4;
            dialogInfo.Ltinfo = DialogConfig.Instance.Last4;
        }
        else if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_niuxiaoxian))
        {
            dialogInfo.Tpinfo = DialogConfig.Instance.Top5;
            dialogInfo.Ctinfo = DialogConfig.Instance.Center5;
            dialogInfo.Ltinfo = DialogConfig.Instance.Last5;
        }
        else if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_xixilang))
        {
            dialogInfo.Tpinfo = DialogConfig.Instance.Top6;
            dialogInfo.Ctinfo = DialogConfig.Instance.Center6;
            dialogInfo.Ltinfo = DialogConfig.Instance.Last6;
        }
        else if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_jisuanji))
        {
            dialogInfo.Tpinfo = DialogConfig.Instance.Top7;
            dialogInfo.Ctinfo = DialogConfig.Instance.Center7;
            dialogInfo.Ltinfo = DialogConfig.Instance.Last7;
        }
        else if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_nianshou))
        {
            dialogInfo.Tpinfo = DialogConfig.Instance.Top8;
            dialogInfo.Ctinfo = DialogConfig.Instance.Center8;
            dialogInfo.Ltinfo = DialogConfig.Instance.Last8;
        }
        else if (LocalData.m_SpiritType.Equals(WizardItemName.Wizard_hongbao))
        {
            dialogInfo.Tpinfo = DialogConfig.Instance.Top9;
            dialogInfo.Ctinfo = DialogConfig.Instance.Center9;
            dialogInfo.Ltinfo = DialogConfig.Instance.Last9;
        }







        InitGameState();
        CreateGroundJingling();
    }
    
    void InitGameState()
    {
        init = true;
        if(xwk)
        xwk.GetComponent<FireXwk>().RecycleRubbish();//销毁火眼金睛特效
        SkyBoxCam = GameObject.Find("SkyBoxCam");
        if (SkyBoxCam)
            SkyBoxCam.SetActive(false);
        startcam.SetActive(false);
        gyrocam.SetActive(true);
        gyrocam.transform.position = spConfig.GyroCamPos;
        jlcam.SetActive(true);
    }
    /// <summary>
    /// 开启手指指引
    /// </summary>
    void OpenDirectEffect()
    {
        public_effect_shoudianji = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.public_effect_shoudianji, "piercingeye", true);
        if (LocalData.m_SpiritType.Equals(0))
        {
            public_effect_shoudianji.transform.localPosition = new Vector3(2.28f, 0.58f, 1);
            public_effect_shoudianji.transform.localEulerAngles = new Vector3(0, -180, 0);
            public_effect_shoudianji.transform.localScale = 1.5f * Vector3.one;
        }
        else
        {
            public_effect_shoudianji.transform.localPosition = new Vector3(-2.37f, 0.8f, 1);
            public_effect_shoudianji.transform.localEulerAngles = new Vector3(0, -180, 0);
            public_effect_shoudianji.transform.localScale = 1.5f * Vector3.one;
        }
        foreach (Transform tran in public_effect_shoudianji.GetComponentsInChildren<Transform>())
        {//遍历当前物体及其所有子物体
            tran.gameObject.layer = LayerMask.NameToLayer("Default");//更改物体的Layer层            
        }
    }
    /// <summary>
    /// 找到地上精灵
    /// </summary>
    /// <param name="obj"></param>
    void DealClickJingling(GameObject obj)
    {
        if (Hithulu)
            return;
        if (public_effect_shoudianji)
            GameObject.Destroy(public_effect_shoudianji);
        //Debug.LogError("找到精灵");
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_huoyanjingjing, "FindJingling");
        WindowSliderBygyro.Instance.CancelGyro();
        OpenFazhen();
        PiercingeyeManager.Instance.PlayCharacterAudio("common_111",//啊哈，看到啦 
            delegate
            {
                fzui.OpenNextClickEffect();
                PiercingeyeManager.Instance.PlayCharacterAudio("common_112");//开启法阵
            });    
        Hithulu = true;
    }
    /// <summary>
    /// 创建精灵(植物)
    /// </summary>
    void CreateGroundJingling()
    {
        PiercingeyeManager.Instance.PlayCharacterAudio("common_109");//合体成功
        jl_jiati = ABResMgr.Instance.LoadResource<GameObject>(spConfig.spritePath_Jiati, "piercingeye", true);

        RuntimeAnimatorController cc = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(spConfig.AnimCtrlPath_Jiati, "piercingeye", false);
        Animator anim = jl_jiati.GetComponent<Animator>();
        anim.runtimeAnimatorController = cc;
        jl_jiati.transform.position = spConfig.SpTransfrom_Jiati.Position;
        jl_jiati.transform.localEulerAngles = spConfig.SpTransfrom_Jiati.erlerAngles;
        jl_jiati.transform.localScale =spConfig.SpTransfrom_Jiati.localScale;
        jl_jiati.SetActive(true);

        SetSpGrayMat(true);
        BoxCollider box = null;
        if (jl_jiati.GetComponent<BoxCollider>() == null)
            box = jl_jiati.GetAddComponent<BoxCollider>();    
        box.size = 100 * Vector3.one;
        box.center = new Vector3(0, 60, 0);
        jl_jiati.tag = "meshButton";
        foreach (Transform tran in jl_jiati.GetComponentsInChildren<Transform>())
        {//遍历当前物体及其所有子物体
            tran.gameObject.layer = LayerMask.NameToLayer("Jingling");//更改物体的Layer层
        }
        AddRubbish(jl_jiati);
        DealClickJingling(jl_jiati);
    }
    public void SetSpGrayMat(bool flag)//设置角色灰色状态
    {
        SkinnedMeshRenderer render;
        if (jl_jiati.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>() != null)
        {
            render = jl_jiati.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        }
        else
        {
            render = jl_jiati.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        }
        if (flag)
        {          
            Material roleMat = ABResMgr.Instance.LoadResource<Material>(spConfig.RolelockingMat, "piercingeye", false, false);
            render.material = roleMat;
        }
        else
        {
            Material roleMat = ABResMgr.Instance.LoadResource<Material>(spConfig.RoleNormalMat, "piercingeye", false, false);
            render.material = roleMat;
        }        
    }
    
    void SetGyroCam()
    {
        jljiaticam.SetActive(true);
        jljiaticam.transform.position = gyrocam.transform.position;
        jljiaticam.transform.rotation = gyrocam.transform.rotation;

        for (int i = 0; i < gyrocam.transform.childCount; i++)
            gyrocam.transform.GetChild(i).gameObject.SetActive(false);

    }
    /// <summary>
    /// 开启法阵
    /// </summary>
    void OpenFazhen()
    {
        //开启法阵
        SetGyroCam();
        jlcam.SetActive(true);
        gyrocam.GetComponent<Camera>().cullingMask &= ~(1 << 27); // 关闭层x     
         oPenFazhen = true;
        if (ui_public_effect_suoding)
        {
            GameObject.Destroy(ui_public_effect_suoding);
        }
        GameObject FazhenCanvas = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.FazhenCanvas, "piercingeye", true);
        fzui = FazhenCanvas.GetComponent<FazhenUI>();
        fzui.InitFazhenConfig(jlcam, spConfig, dialogInfo);
        AddRubbish(FazhenCanvas);
    }
    void AddCameraEffect()
    {
        EffectUICam = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.EffectUICam, "piercingeye", true);
        EffectUICam.transform.parent = gyrocam.transform;
        EffectUui_public_effect_mengbanICam = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.ui_public_effect_mengban, "piercingeye", true);
        EffectUui_public_effect_mengbanICam.transform.parent = EffectUICam.transform;
        EffectUui_public_effect_mengbanICam.transform.localPosition = new Vector3(0, 0, 10);
        EffectUui_public_effect_mengbanICam.transform.localScale = 0.01f * Vector3.one;

        AddRubbish(EffectUui_public_effect_mengbanICam);
        AddRubbish(EffectUICam);
    }  
    public void DestoryJl()
    {
        GameObject.Destroy(jl_jiati);
    }
}

