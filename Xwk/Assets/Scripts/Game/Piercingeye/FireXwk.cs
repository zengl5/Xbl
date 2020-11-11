using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.C_Framework;
//小悟空特效生成
public class FireXwk : MonoBehaviour,IRubbish {

    Transform Rhand;
    Transform Lhand;
    Transform Bone_mouth_00;
    List<GameObject> EfList = new List<GameObject>();
    GameObject effect_hyjj_2heti;
    GameObject effect_hyjj_3heizhao;
    GameObject handBox;
    GameObject effect_hyjj_3yanjing;
    GameObject public_effect_dianji_hyjj;
    GameObject UICover_jiantouzhiyin;//指引
    GameObject BackGround_Bg;
    MeshButton meshButton;
    bool OpenPiercingEye = false;
    WaitForSeconds waitrp = new WaitForSeconds(2f);

    // Use this for initialization
    void Start () {        
        Rhand = transform.Find("Bone_Root").Find("Bone001 R Hand");
        Lhand = transform.Find("Bone_Root").Find("Bone001 L Hand");
        Bone_mouth_00 = transform.Find("Bone_Root").Find("Bone001 Head").Find("bone_mouth_00");
        if (PiercingeyeManager.Instance.IsState("FireReadyState"))
        {
            if (handBox == null)
                AddHetiEvent();
            Init1StepEffect();
        }     
    }
    /// <summary>
    /// 小悟空合体事件添加
    /// </summary>
    void AddHetiEvent()
    {
        handBox = new GameObject("HandBox");
        handBox.transform.SetParent(Rhand);
        handBox.AddComponent<BoxCollider>();
        handBox.GetComponent<BoxCollider>().size = 15 * Vector3.one;
        meshButton=handBox.AddComponent<MeshButton>();
        meshButton.AddMeshEvent(PiercingeyeManager.Instance.GotoHeti);
        meshButton.Interactable = false;
        handBox.transform.localPosition = Vector3.zero;
        handBox.transform.localEulerAngles = Vector3.zero;
        handBox.transform.localScale = Vector3.one;
        handBox.tag = "meshButton_Begin";
    }
    /// <summary>
    /// 释放内存
    /// </summary>
    public void RealseMemory()
    {
        for (int i = 0; i < EfList.Count; i++)
            if (EfList[i] != null)
            {
                Resources.UnloadAsset(EfList[i]);
            }
    }
    public void RecycleRubbish()
    {
        if(meshButton != null)
        meshButton.RemoveMeshEvent();
        for (int i = 0; i < EfList.Count; i++)
            if(EfList[i]!=null)
            Destroy(EfList[i]);
        if (handBox)
            Destroy(handBox);
        Destroy(this.gameObject);
    }
    Transform GetRhand()
    {
        return Rhand;
    }

    Transform GetLhand()
    {
        return Lhand;
    }

    Transform GetBone_mouth_00()
    {
        return Bone_mouth_00;
    }
    /// <summary>
    /// 合体按下特效
    /// </summary>
    public void ShowInputEffect(bool flag)
    {
        if (effect_hyjj_2heti == null)
        {
            effect_hyjj_2heti = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.effect_hyjj_2heti, "piercingeye", true);
            AddEffect(effect_hyjj_2heti);
            effect_hyjj_2heti.transform.localScale = 0.01F * Vector3.one;
        }
        else
        {
            effect_hyjj_2heti.SetActive(flag);
            effect_hyjj_2heti.transform.localScale = 0.01F * Vector3.one;
        }
    }
    /// <summary>
    /// 合体背景特效
    /// </summary>
    void Init1StepEffect()
    {
        GameObject ui_hyjj_beijing = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.ui_hyjj_beijing, "piercingeye", true);
        BackGround_Bg = ui_hyjj_beijing.transform.GetChild(1).GetChild(3).transform.gameObject;
        AddEffect(ui_hyjj_beijing);
    }
    void AddBlackBg()
    {
        BackGround_Bg.transform.GetComponent<Image>().color = new Color(0,0,0, 130 / 250f);
    }
    void HideBlackBg()
    {
        BackGround_Bg.transform.GetComponent<Image>().color = Vector4.zero;
    }
    /// <summary>
    /// 2阶段合体引导特效（提示圈）
    /// </summary>
    public void Init2StepEffect()
    {
        public_effect_dianji_hyjj = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.public_effect_dianji_hyjj, "piercingeye", true);
        AddEffect(public_effect_dianji_hyjj, GetRhand());
        public_effect_dianji_hyjj.SetActive(true);
        public_effect_dianji_hyjj.transform.localPosition = new Vector3(-7.5f, 6, 0);
        meshButton.Interactable = true;
        //PiercingeyeManager.Instance.OpenDirectEffect(new Vector3(0.07f,0.37f,4.4f),0.3f*Vector3.one);
    }
    /// <summary>
    /// 关闭手指指引
    /// </summary>
    public void CloseHandDirector()
    {
        if (public_effect_dianji_hyjj)
            public_effect_dianji_hyjj.transform.GetChild(0).gameObject.SetActive(false);
    }
    /// <summary>
    /// 3阶段特效
    /// </summary>
    public void Init3StepEffect()
    {
        PiercingeyeManager.Instance.PlaySceneAudio("public_xwkyx_031");//音效
        if (public_effect_dianji_hyjj)
            public_effect_dianji_hyjj.SetActive(false);
        GameObject effect_hyjj_3xl = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.effect_hyjj_3xl, "piercingeye", true);
        AddEffect(effect_hyjj_3xl, this.transform);

        GameObject effect_hyjj_3yshou = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.effect_hyjj_3yshou, "piercingeye", true);
        AddEffect(effect_hyjj_3yshou,GetRhand());
        GameObject effect_hyjj_3zshou = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.effect_hyjj_3zshou, "piercingeye", true);
        AddEffect(effect_hyjj_3zshou, GetLhand());
        Invoke("Step3Delay", 3.3f);
        Invoke("Step3Delay_yanjing", 2f);
        Invoke("AddBlackBg", 3.8f);

    }
    void Step3Delay()
    {
        effect_hyjj_3heizhao = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.effect_hyjj_3heizhao, "piercingeye", true);
        AddEffect(effect_hyjj_3heizhao, this.transform);
        //指引 3S 循环
        StartCoroutine("Repeatezhiyin");
    }
     IEnumerator Repeatezhiyin()
    {
        yield return waitrp;
        if(UICover_jiantouzhiyin==null)
        {
            UICover_jiantouzhiyin = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.UICover_jiantouzhiyin, "piercingeye", true);
            AddEffect(UICover_jiantouzhiyin);
        }
        else
        {
            UICover_jiantouzhiyin.SetActive(false);
            UICover_jiantouzhiyin.SetActive(true);
        }
        waitrp = new WaitForSeconds(5f);
        if (!OpenPiercingEye)            
        StartCoroutine("Repeatezhiyin");
    }

    void Step3Delay_yanjing()
    {
        effect_hyjj_3yanjing = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.effect_hyjj_3yanjing, "piercingeye", true);
        AddEffect(effect_hyjj_3yanjing, GetBone_mouth_00());
    }
    void AutoHide()
    {
        effect_hyjj_3heizhao.SetActive(false);
    }
    /// <summary>
    /// 5阶段特效,执行右滑操作
    /// </summary>
    public void Init5stepEffect()
    {
        OpenPiercingEye = true;
        StopCoroutine("Repeatezhiyin");
        OpenEye();
        HideBlackBg();
        AutoHide();
        effect_hyjj_3yanjing.SetActive(false);
        GameObject effect_hyjj_5sdx = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.effect_hyjj_5sdx, "piercingeye", true);
        AddEffect(effect_hyjj_5sdx);

        GameObject effect_hyjj_5heizhao = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.effect_hyjj_5heizhao, "piercingeye", true);
        AddEffect(effect_hyjj_5heizhao,this.transform);

        Invoke("YanjingYanchi", 0.2f);

        GameObject effect_hyjj_5shouR = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.effect_hyjj_5shou, "piercingeye", true);
        AddEffect(effect_hyjj_5shouR, GetRhand());

        GameObject effect_hyjj_5shouL = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.effect_hyjj_5shou, "piercingeye", true);
        AddEffect(effect_hyjj_5shouL, GetLhand());      
    }
    void OpenEye()
    {
        PiercingeyeManager.Instance.PlaySceneAudio("public_xwkyx_033");//开眼音效
    }

    //眼睛延迟0.2S 
    void YanjingYanchi()
    {
        GameObject effect_hyjj_5_yanjing = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.effect_hyjj_5_yanjing, "piercingeye", true);
        AddEffect(effect_hyjj_5_yanjing, GetBone_mouth_00());
    }
    void AddEffect(GameObject obj,Transform parent)
    {
        obj.transform.parent = parent;
        obj.transform.localScale= Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = Vector3.zero;
        EfList.Add(obj);
    }
    void AddEffect(GameObject obj)
    {
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = Vector3.zero;
        EfList.Add(obj);
    }
}
