using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using XWK.Common.UI_Reward;
using YB.XWK.MainScene;
public class FazhenUI : MonoBehaviour,IRubbish
{
    public GameObject Step1Fazhen;
    public List<GameObject> FazhenList;
    GameObject Jl;
    int clickCount = 0;
    List<GameObject> EfList = new List<GameObject>();
    Vector3 moveTarget = new Vector3(-2.85f, -2.3F, 7.24f);
    GameObject DialogPanal;
    GameObject JlCamera;
    DialogInfo dialogInfo;
    SpritConfig spriteConfig;
    bool showDirectEf = false;
    GameObject public_effect_shoudianji;
    WaitForSeconds wait = new WaitForSeconds(5.5f);
    WaitForSeconds wait2 = new WaitForSeconds(1f);
    string baseUIPath = "game/Piercingeye/ui/";
    public void InitFazhenConfig(GameObject jlcam,SpritConfig sprite,DialogInfo dialogInfo)
    {
        RewardUIManager.Instance.ChangeModule(ModuleType.SpriteUnlock, "Piercingeye");
        if (DirectorMgr.DirectorAllState)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, YB.XWK.MainScene.LocalData.m_game_huoyanjingjing, "showfazhen");
        }
        else
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, YB.XWK.MainScene.LocalData.m_game_huoyanjingjing, "showfazhen_NewUser");
        }
        this.dialogInfo = dialogInfo;
        this.spriteConfig = sprite;
        JlCamera = jlcam;    
    }
    /// <summary>
    /// 显示第一颗指引
    /// </summary>
    void ShowDirectEffect(Transform target)
    {
        if (showDirectEf)
            return;
        public_effect_shoudianji = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.public_effect_shoudianji, "piercingeye", true);
        foreach (Transform tran in public_effect_shoudianji.GetComponentsInChildren<Transform>())
        {//遍历当前物体及其所有子物体
            tran.gameObject.layer = LayerMask.NameToLayer("FazhenUI");//更改物体的Layer层            
        }
        public_effect_shoudianji.transform.parent = target.transform.parent;
        public_effect_shoudianji.transform.localPosition = Vector3.zero;
        public_effect_shoudianji.transform.localScale = 500 * Vector3.one;
        showDirectEf = true;
    }
    public void OpenNextClickEffect()
    {
        if (clickCount >= 5)
        {
            PiercingeyeManager.Instance.ShowNormalColorJL();
            Invoke("ShowFazhen", 1);
            Invoke("JlEfSound", 1.2f);
            Invoke("ShwoJl", 3f);
            Invoke("ShowJlEf", 4f);          
            return;
        }
        int rdId = Random.Range(0, FazhenList.Count);
        FazhenList[rdId].SetActive(true);
        FazhenList[rdId].transform.parent.GetComponent<Image>().color = Color.white;
        ShowDirectEffect(FazhenList[rdId].transform);
        FazhenList.RemoveAt(rdId);
    }
    void JlEfSound()
    {
        PiercingeyeManager.Instance.PlaySceneAudio("public_xwkyx_028");//音效
    }
    public void RecycleRubbish()
    {        
        if (public_effect_shoudianji)
            Destroy(public_effect_shoudianji);
        CancelInvoke();
        for (int i = 0; i < EfList.Count; i++)
            if (EfList[i] != null)
                Destroy(EfList[i]);
        Destroy(this.gameObject);
    }
    /// <summary>
    /// 点击法阵处理逻辑
    /// </summary>
    /// <param name="bt"></param>
    public void ClickFazhen(Transform bt)
    {   
        if(bt.GetChild(0).gameObject.activeInHierarchy)
        {
            RewardUIManager.Instance.RegisterSpriteUnlock(1, null, "Piercingeye");
            RewardUIManager.Instance.SetSuccess();

            if (public_effect_shoudianji)
                Destroy(public_effect_shoudianji);
            bt.GetChild(0).gameObject.SetActive(false);//子物体关闭
            //生成点击特效
            clickCount++;
            if (Application.isEditor)
                clickCount = 5;//自动跳过
            OpenNextClickEffect();
            GameObject ui_public_effect_bao = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.ui_public_effect_bao, "piercingeye", true);
            AddRubbish(ui_public_effect_bao,bt);
            ReplaceTexture(bt.gameObject);
            PiercingeyeManager.Instance.PlayEffectAudio("public_xwkyx_027");
            if(clickCount>=5)
            {
                PiercingeyeManager.Instance.NotifyintroduceFinish();//返回跳过就可以算解锁
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_spriteWindow, "LockName:" + LocalData.m_SpiritType);
            }
            if (DirectorMgr.DirectorAllState)
            {
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, YB.XWK.MainScene.LocalData.m_game_huoyanjingjing, "clickFazhen:"+clickCount);
            }
            else
            {
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, YB.XWK.MainScene.LocalData.m_game_huoyanjingjing, "clickFazhen_NewUser:" + clickCount);
            }
        }
    }
    void ReplaceTexture(GameObject obj)
    {
        Sprite tex = ABResMgr.Instance.LoadResource<Sprite>(FirePath.Instance.iconui_fp + clickCount, "piercingeye", true);
        obj.GetComponent<Image>().sprite = tex;
        obj.GetComponent<Image>().color = Color.white;
    }
    /// <summary>
    /// 显示法阵
    /// </summary>
    void ShowFazhen()
    {
        GameObject ui_public_effect_fzxs = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.ui_public_effect_fzxs, "piercingeye", true);
        AddRubbish(ui_public_effect_fzxs,this.transform);
        Step1Fazhen.SetActive(false);
     }
    /// <summary>
    /// 显示精灵
    /// </summary>
    void ShwoJl()
    {       
        //特效
        GameObject ui_public_effect_jlcx = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.ui_public_effect_jlcx, "piercingeye", true);
        AddRubbish(ui_public_effect_jlcx, this.transform);
        ui_public_effect_jlcx.transform.localPosition = Vector3.zero;
        PiercingeyeManager.Instance.DestoryOldJL();
        ReInitJl();
        StartCoroutine(JlInfo());
    }
    /// <summary>
    /// 精灵介绍
    /// </summary>
    void ShowJlEf()
    {
        DialogPanal = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.ui_public_effect_jljs, "piercingeye", true);
        AddRubbish(DialogPanal, this.transform);
    }    
    void AddRubbish(GameObject obj,Transform parent=null)
    {
        if (parent != null)
            obj.transform.SetParent(parent);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = Vector3.zero;         
        EfList.Add(obj);
    }   
    //重新实例化一个精灵（真身），由一个精灵相机单独照射，之前那个精灵销毁
    void ReInitJl()
    {
        if(DirectorMgr.DirectorAllState)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, YB.XWK.MainScene.LocalData.m_game_huoyanjingjing, "showjingling");
        }
        else
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, YB.XWK.MainScene.LocalData.m_game_huoyanjingjing, "showjingling_NewUser");
        }
        JlCamera.SetActive(true);
        JlCamera.transform.position = new Vector3(0, 0, 15);

        //JlCamera.transform.eulerAngles=Vector3.zero;
        JlCamera.transform.eulerAngles = new Vector3(0, 180, 0);

        JlCamera.GetComponent<Camera>().fieldOfView=60;
        Jl = ABResMgr.Instance.LoadResource<GameObject>(spriteConfig.Real_spritePath, "piercingeye", true);
        RuntimeAnimatorController jlrtac = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(spriteConfig.Real_AnimCtrlPath, "piercingeye", false);
        Jl.GetAddComponent<Animator>().runtimeAnimatorController = jlrtac;
        Jl.SetActive(true);
        foreach (Transform tran in Jl.GetComponentsInChildren<Transform>())
        {//遍历当前物体及其所有子物体
            tran.gameObject.layer = LayerMask.NameToLayer("Jingling");//更改物体的Layer层            
        }
        AddRubbish(Jl);
        Jl.transform.localScale =spriteConfig.Real_SpTransfrom.localScale;
        Jl.transform.eulerAngles = spriteConfig.Real_SpTransfrom.erlerAngles;
        Jl.transform.position = spriteConfig.Real_SpTransfrom.Position;
        Jl.GetComponent<Animator>().SetBool("talkfinish", false);
        GetShowjlTime();
        SetSpMainTexture(Jl);
        PiercingeyeManager.Instance.PlayCharacterAudio("common_113");//哇哦，精灵出现了
        Invoke("IntroduceSelf", GetShowjlTime());
    }
    float GetShowjlTime()
    {
        Animator anim = Jl.GetComponent<Animator>();
        AnimatorClipInfo[] info=anim.GetCurrentAnimatorClipInfo(0);
        if (info.Length > 0)
            return info[0].clip.length;
        else
            return 2f;
     }
    public void SetSpMainTexture(GameObject obj)//设置角色贴图
    {
        SkinnedMeshRenderer render;
        if (obj.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>() != null)
        {
            render = obj.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        }
        else
        {
            render = obj.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        }
        Texture2D tex = ABResMgr.Instance.LoadResource<Texture2D>(spriteConfig.normalTex, "piercingeye", false, false);
        if(Application.isEditor)
        {
            render.material.mainTexture = tex;
        }
        else
        {
            render.sharedMaterial.mainTexture = tex;
        }
    }
    IEnumerator JlInfo()
    {
        yield return wait;
        MoveJl();
        AddTextDialog();
        yield return wait2;
        AddTitleTex();
        ShowText();
    }
    //自我介绍
    void IntroduceSelf()
    {
        Jl.GetComponent<Animator>().Play("jl00002_1_xingfen01_start#anim");
        PiercingeyeManager.Instance.PlayCharacterAudio(spriteConfig.SpNameSound,
            delegate
            {
                Jl.GetComponent<Animator>().SetBool("talkfinish", true);
                //通知完成自我介绍后，可以跳过
                PlayTextSound();
                ShowSliderUI();
                InvokeRepeating("RadomJlIdleAnimation", 0.1f, 2);
            });//我是百音精灵
    }
    //添加标题图片
    void AddTitleTex()
    {
        GameObject obj = new GameObject();
        obj.transform.parent = DialogPanal.transform.GetChild(0).transform;
        RawImage rawimage=obj.AddComponent<RawImage>();
        obj.transform.localScale = Vector3.one;
        Texture2D texture = ABResMgr.Instance.LoadResource<Texture2D>(baseUIPath+dialogInfo.TitleTex, "piercingeye");
        rawimage.texture = texture;

        rawimage.SetNativeSize();
        rawimage.transform.localPosition = new Vector3(435, 420, 0);
    }
    //手动添加对话框
    void AddTextDialog()
    {
        for (int i=0;i<3;i++)
        {
            GameObject dialog = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.dialog_fp, "piercingeye", true);
            dialog.transform.SetParent(DialogPanal.transform.GetChild(0).transform);
            dialog.transform.localPosition =dialogInfo.GetTextPos(i);
            dialogInfo.AddTextCompnent(dialog.GetComponent<Text>());
        }
    }
    //精灵移动 
    void MoveJl()
    {
        Jl.transform.DOMove(spriteConfig.MovePostion, 0.75f);
        Jl.transform.DORotate(spriteConfig.MoveEulerAngles, 0.75f);
        Jl.transform.DOScale(spriteConfig.Real_SpTransfrom.localScale, 0.75F);
    }
    void RadomJlIdleAnimation()
    {
        if(Random.Range(0,1f)>=0.5f)
        {
            Jl.GetComponent<Animator>().SetBool("idlerandom", true);
        }
        else
        {
            Jl.GetComponent<Animator>().SetBool("idlerandom", false);
        }    
    }


    void ShowText()
    {
        dialogInfo.GetText(0).text +=dialogInfo.Tpinfo;
        dialogInfo.GetText(0).color = dialogInfo.TpLtcolor;

        dialogInfo.GetText(1).text += dialogInfo.Ctinfo;
        dialogInfo.GetText(1).color = dialogInfo.Ctcolor;

        dialogInfo.GetText(2).text += dialogInfo.Ltinfo;
        dialogInfo.GetText(2).color = dialogInfo.TpLtcolor;
    }

    void PlayTextSound()
    {
        PiercingeyeManager.Instance.PlayCharacterAudio(dialogInfo.SpInfoSound, AutoNext);
    }
    //3S后未点击屏幕，自动进入下一步故事
    void AutoNext()
    {
        if (Application.isEditor)
            return;
        Invoke("GameOver", 3);
    }
    void ShowSliderUI() 
    {            
        GameObject quitUI = ABResMgr.Instance.LoadResource<GameObject>(FirePath.Instance.ui_public_effect_xyb, "piercingeye", true);
        GameObject parent = GameObject.Find("FazhenCanvas(Clone)");
        AddRubbish(quitUI, parent.transform);
        quitUI.transform.localScale = Vector3.one;
        quitUI.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-107f, -473, 0);
        PiercingeyeManager.Instance.NotifyGameOver();
    }
    void GameOver()
    {
        PiercingeyeManager.Instance.GameOver();
    }
}
