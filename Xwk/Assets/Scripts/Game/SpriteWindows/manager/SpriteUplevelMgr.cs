using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using DG.Tweening;
using XWK.Common.UI_Reward;
using YB.XWK.MainScene;

public struct BgColor
{
    public void InitColor(Color c1, Color c2, Color c3, Color c4)
    {
        level1 = c1;
        level2 = c2;
        level3 = c3;
        level4 = c4;
    }
    public Color level1;
    public Color level2;
    public Color level3;
    public Color level4;
}
/// <summary>
/// 升级管理类
/// </summary>
public class SpriteUplevelMgr : MonoBehaviour {
    
    public static SpriteUplevelMgr Instance;
    public UpgradeBtn upBtn;
    [Header("[中间loading图对象]")]
    public GameObject UpUI_Center;
    [Header("[左侧4个等级图标父节点]")]
    public GameObject UpPanel;//升级Panel;
    [Header("[升级背景图片]")]
    public RawImage BgImage;
    [Header("[升级特效+1父物体]")]
    public Transform ScoreParent;
    UpgradeUI upGradeui;
    [Header("[右侧4个等级图标RoleIcon对象引用]")]
    public List<RoleIcon> roleIconList;
    [Header("升级小箭头")]
    public RawImage Arrow;

    Dictionary<string, UpgradeUI> upGradeDic = new Dictionary<string, UpgradeUI>();
    SpriteData nowSpdata;
    BgColor color;
    GameObject roleEf;
    static bool  AddScore=false;
    List<Image> CtLoadingImageList = new List<Image>();
    Color Normalcolor = 4 * Vector4.one;
    Color Closecolor = new Vector4(1, 1, 1, 0);
    GameObject DirHand=null;
    List<GameObject> Eflist = new List<GameObject>();
    GameObject UpgradeEf;
    bool isPlayingUpEf = false;

    void Awake()
    {
        Instance = this;
        color = new BgColor();
        color.InitColor(SpWindowPath.Instance.Colevel1, SpWindowPath.Instance.Colevel2, SpWindowPath.Instance.Colevel3, SpWindowPath.Instance.Colevel4);
        if (Application.isEditor)//编辑器默认灵气
        {
            if (!AddScore) //测试默认加500
            {
                AddScore = true;
                if (AnimaData.TotalNimbus <= 5000)
                    AnimaData.TotalNimbus += 5000;
            }
        }        
        //ShowUpDirHand(true);
    }
    /// <summary>
    /// 显示新手引导手
    /// </summary>
    public void ShowUpDirHand(bool flag)
    {
        if(flag)
        {
            DirHand = GameObjectTool.Instance.InitLocal3Dhand("SpUI", new Vector3(40.7F, -8.7F, -54), Vector3.one, 300 * Vector3.one, upBtn.transform);
            DirHand.SetActive(true);
            GameObject.Destroy(DirHand, 3);
        }
        else
        {
            if (DirHand)
                GameObject.Destroy(DirHand);
        }
    }
    #region##渐变开关中间LoadingUI
    public void CloseUpUI_Center()
    {
        if (!UpUI_Center.activeInHierarchy)
            return;
        if (CtLoadingImageList.Count < 3)
        {
            for (int i = 0; i < UpUI_Center.transform.childCount; i++)
            {
                Image image = UpUI_Center.transform.GetChild(i).GetComponent<Image>();
                if (!CtLoadingImageList.Contains(image))
                    CtLoadingImageList.Add(image);
            }
        }
        for (int i = 0; i < CtLoadingImageList.Count; i++)
        {
            CtLoadingImageList[i].DOColor(Closecolor, 0.35f).OnComplete(delegate
            {
                UpUI_Center.transform.gameObject.SetActive(false);
            });
        }
     }
    public void OpenUpUI_Center()
    {       
        if (UpUI_Center.activeInHierarchy)
            return;
        if (CtLoadingImageList.Count < 3)
        {
            for (int i = 0; i < UpUI_Center.transform.childCount; i++)
            {
                Image image = UpUI_Center.transform.GetChild(i).GetComponent<Image>();
                if (!CtLoadingImageList.Contains(image))
                    CtLoadingImageList.Add(image);
            }
        }
        for (int i = 0; i < CtLoadingImageList.Count; i++)
        {
            CtLoadingImageList[i].color = Closecolor;
        }
        for (int i = 0; i < CtLoadingImageList.Count; i++)
        {
            CtLoadingImageList[i].DOColor(Normalcolor, 0.25f).OnComplete(delegate
            {
                UpUI_Center.transform.gameObject.SetActive(true);
            });
        }
    }
    #endregion


    #region##初始化升级事件
    /// <summary>
    /// 初始化升级事件[每次切换精灵后]
    /// </summary>
    public void RefreshUpGradeUI()
    {
        nowSpdata = SpriteIconMgr.Instance.GetSpriteData();
        if (nowSpdata == null)
            return;
        if (nowSpdata.lockIcon)
        {
            if (!nowSpdata.Name.Equals(WizardItemName.future))
            {
                SpriteIconMgr.Instance.UpgradeMeshTexture(nowSpdata.spLvel);//刷新模型贴图
                nowSpdata.nowRoleState = nowSpdata.spLvel;
            }
        }
        if(nowSpdata.Roledata==null)//?号状态、非精灵
        {
            OpenUpBtn(false);
            OpenUpPanel(false);
        }
        if (nowSpdata.Roledata == null)
            return;
       
        if (!nowSpdata.Roledata.NeedUpRole)//不需要升级对象
        {
            UpUI_Center.SetActive(false);
            OpenUpBtn(false);
            if(upGradeui!=null)
            upGradeui.CloseUI();
            OpenUpPanel(false);
            return;
        }
        else
        {
            if (nowSpdata.Roledata.NeedUpRole)
            {
                OpenUpPanel(true);
                RefreshUpBtn();
                RefreshGradeUI();
                RefreshSpLockState();
                if (!nowSpdata.lockIcon)
                {
                    OpenUpBtn(false);
                }
                RefreshBgColor(nowSpdata.spLvel);
            }
            else
            {
                OpenUpBtn(false);
                upGradeui.CloseUI();
                OpenUpPanel(false);
            }
        }             
    }

    public void RefreshBgColor(int id)
    {
        switch (id)
        {
            case 0:
                BgImage.material.SetColor("_Color", color.level1);
                break;
            case 1:
                BgImage.material.SetColor("_Color", color.level2);
                break;
            case 2:
                BgImage.material.SetColor("_Color", color.level3);
                break;
            case 3:
                BgImage.material.SetColor("_Color", color.level4);
                break;
        }
    }

    public void RefreshUpBtn()
    {
        if(nowSpdata.spLvel>=3)
        {
            CloseUpUI_Center();
            OpenUpBtn(false);
        }
        else
        {
            if(nowSpdata.spLvel==0)
            {
                OpenUpBtn(true);
            }
            else
            {
                //判断升级按键       
                if (nowSpdata.nowRoleState == nowSpdata.spLvel)//   0 1 2 3 
                {
                    OpenUpBtn(true);
                }
                else
                {
                    OpenUpBtn(false);
                }
            }         
        }    
    }

    
    void RefreshGradeUI()
    {
        upBtn.LongpressStayingEvent = null;
        upBtn.PointOnclickEvent = null;
        if (upGradeDic.ContainsKey(nowSpdata.Name))
        {           
            upGradeui = upGradeDic[nowSpdata.Name];
            RemoveEvent();
            RegisetEvent();
        }
        else
        {          
            upGradeui = new UpgradeUI(UpUI_Center, nowSpdata);
            RemoveEvent();
            RegisetEvent();
        }
        upGradeui.CloseUI();
    }
    void RegisetEvent()
    {
        upBtn.LongpressStayingEvent += LongpressStayingEvent;//注册长按
        upBtn.LongpressExitEvent += LongPressExitEvent;//注册点击离开
        upBtn.PointOnclickEvent += PointOnclickEvent;//注册升级点击

        upGradeui.UpgradeSuccess += UpgradeSuccessFun;//注册升级成功
        upGradeui.UpgradeSuccess += upBtn.ResetUpData;
        upGradeui.UpgradeFullAction += UpgradeFullAction;//注册满级
        upGradeui.UpgradeProgress += UpdateRoleIcon_Progress;//注册实时进度
    }
    void RemoveEvent()
    {
        upBtn.LongpressStayingEvent -= LongpressStayingEvent;
        upBtn.LongpressExitEvent -= LongPressExitEvent;
        upBtn.PointOnclickEvent -= PointOnclickEvent;

        upGradeui.UpgradeSuccess -= UpgradeSuccessFun;
        upGradeui.UpgradeSuccess -= upBtn.ResetUpData;
        upGradeui.UpgradeFullAction -= UpgradeFullAction;
        upGradeui.UpgradeProgress -= UpdateRoleIcon_Progress;
    }
    #endregion


    #region##注册升级按钮点击事件
    void PointOnclickEvent()
    {
        if (upGradeui.IsWaitingState())
            return;
        if (AnimaData.TotalNimbus <= 0)//分数小于0
        {
            NoLingqiAudio();
            if(nowSpdata!=null)
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.spriteUpgrade, "UpgradeLqZero:" + nowSpdata.Name);
        }
        else
        {
            upGradeui.Fresh_OnclickUI();
            AddArrowEffect();
            AddClickSound();
            FixedEffect();
            CommonUIMgr.Instance.ClickFunctionButton(upBtn.transform.gameObject);
            //CommonUIMgr.Instance.ClickFunctionButton(Arrow);
        }
    }
   
    void LongpressStayingEvent(float timer)//长按事件
    {
        if (upGradeui.IsWaitingState())
            return;
        LongPress();
        if (AnimaData.TotalNimbus <= 0)//分数小于0
        {
            NoLingqiAudio();
            if (nowSpdata != null)
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.spriteUpgrade, "UpgradeLqZero:" + nowSpdata.Name);
        }
        if (AnimaData.TotalNimbus > 0)//分数>0才会执行升级按键
        {
            upGradeui.Fresh_OnPointDownUI(timer);
            AddArrowEffect();
            AddClickSound();
            FixedEffect();
        }
    }
    void LongPressExitEvent()
    {
        LongPressExit();
    }

    void LongPress()
    {
        upBtn.transform.DOScale(0.8f * Vector3.one, 0.17f);
    }
    void LongPressExit()
    {
        if (upBtn.transform.localScale == 0.8f * Vector3.one)
            upBtn.transform.DOScale(Vector3.one, 0.17f);
    }


    void NoLingqiAudio()
    {
        float rdIndex = UnityEngine.Random.Range(0f, 1f);
        if (rdIndex >= 0.5f)
        {
            Spwindow.PlayCharacterAudio("xwk_jlej_15");
        }
        else
        {
            Spwindow.PlayCharacterAudio("xwk_jlej_16");
        }
    }
    Sequence seq;
    void AddArrowEffect()
    {
        //Arrow.transform.localPosition = new Vector3(Arrow.transform.localPosition.x, 29, 0);
        //Arrow.transform.DOLocalMove(new Vector3(Arrow.transform.localPosition.x, Arrow.transform.localPosition.y + 10, 0), 0.1f);
        if (seq != null)
        {
            seq.Pause();
            seq.Kill();
        }

        seq = DOTween.Sequence();
        Arrow.color = new Vector4(1, 1, 1, 1f);
        Arrow.transform.localScale = Vector3.one;
        Arrow.transform.localPosition = new Vector3(Arrow.transform.localPosition.x, -375f, Arrow.transform.localPosition.z);
        if (Arrow != null)
        {
            seq.Append(Arrow.transform.DOScale(1.11f, 0.167f));
            seq.Insert(0, Arrow.transform.DOLocalMoveY(-325, 0.167f));

            seq.Append(Arrow.transform.DOScale(0.732f, 0.135f));
            seq.Insert(0.167f, Arrow.transform.DOLocalMoveY(-375, 0.135f));

            seq.Append(Arrow.transform.DOScale(1f, 0.075f));
        }
    }
    void AddClickSound()
    {
        Spwindow.PlaySmallClipSound("public_xwkyx_035");
    }
    void FixedEffect()//-1-1特效
    {
        GameObject obj = GetPoolObj();
        if (isPlayingUpEf)
            return;
        if (UpgradeEf == null)
        {
            isPlayingUpEf = true;
            UpgradeEf = GameObjectTool.Instance.GetWorldGameObject(SpWindowPath.Instance.public_effect_ejjm_sj, new Vector3(0, -2.1F, 2), Vector3.zero, Vector3.one, ABCommonConfig.Instance.SpWindowBundleType);
        }
        else
        {
            UpgradeEf.gameObject.SetActive(false);
            UpgradeEf.gameObject.SetActive(true);
            isPlayingUpEf = true;
        }
        Invoke("SetPauseUpEf", 0.5F);
    }
    GameObject GetPoolObj()
    {
        GameObject ef = ABResMgr.Instance.LoadResource<GameObject>(SpWindowPath.Instance.ui_public_effect_jsz_1, ABCommonConfig.Instance.SpWindowBundleType, true, false);
        ef.transform.SetParent(ScoreParent.transform);
        ef.transform.localPosition = new Vector3(70, 80, 0);
        ef.transform.localScale = Vector3.one;
        Destroy(ef, 0.5F);
        return ef;
    }
    void SetPauseUpEf()
    {
        isPlayingUpEf = false;
    }
    #endregion


    #region##升级成功
    void UpgradeFullAction()
    {
        upBtn.StopUpgrade();
    }
    void OpenUpBtn(bool flag)
    {
        upBtn.transform.gameObject.SetActive(flag);//打开关闭升级按键
        Arrow.gameObject.SetActive(flag);      
        upBtn.transform.localScale = Vector3.one;
        Arrow.transform.localScale = Vector3.one;
    }
    void OpenUpPanel(bool flag)
    {
        UpPanel.transform.gameObject.SetActive(flag);//打开升级图标面板
    }    /// <summary>
         /// 升级成功事件
         /// </summary>
    void UpgradeSuccessFun()
    {
        if (nowSpdata.spLvel <= roleIconList.Count - 1)
        {
            if (nowSpdata.spLvel - 1 >= 0)
                roleIconList[nowSpdata.spLvel - 1].OpenLodingUI(false);
        }

        nowSpdata.nowRoleState++;
        if (nowSpdata.spLvel <= 3)
            WizardData.SetWizardItemLevel(nowSpdata.Name, nowSpdata.spLvel);//设置精灵等级，上报服务器

        SpriteLingqiMgr.Instance.RefreshAllLingqiData();
        SpriteIconMgr.Instance.UpgradeMeshTexture(nowSpdata.spLvel);
        SpriteIconMgr.Instance.RefreshIdleEffect(nowSpdata.nowRoleState);
        SpritBtn.Instance.RefreshGameUI(nowSpdata);//刷新游戏UI
        Spwindow.PlaySmallClipSound("public_xwkyx_028");
        if (nowSpdata.spLvel >= 3)
        {
            OpenUpBtn(false);
        }
        RefreshBgColor(nowSpdata.spLvel);
        RefreshSpLockState();
        RefreshIconSize(nowSpdata);
        AddMeshUpGradeEffect();
        PlaySucessAudio();
    }
    void AddMeshUpGradeEffect()//添加模型切换特效
    {
        if (roleEf == null)
        {
            roleEf = ABResMgr.Instance.LoadResource<GameObject>(SpWindowPath.Instance.public_effect_ejjm_jj01, ABCommonConfig.Instance.SpWindowBundleType, true, false);
            roleEf.transform.position = new Vector3(0, -2.92f, 0);
        }
        else
        {
            roleEf.SetActive(false);
            roleEf.SetActive(true);
        }
    }
    void PlaySucessAudio()
    {
        float id = UnityEngine.Random.Range(0f, 1f);
        if(id>=0.66f)
        {
            Spwindow.PlayCharacterAudio("xwk_jlej_12");
        }
        else if (id >= 0.33f)
        {
            Spwindow.PlayCharacterAudio("xwk_jlej_13");
        }
        else
        {
            Spwindow.PlayCharacterAudio("xwk_jlej_14");
        }
    }
    #endregion


    #region##右侧4个不同等级进度条管理
    /// <summary>
    /// 更新右侧等级条Progress
    /// </summary>
    /// <param name="progress"></param>
    void UpdateRoleIcon_Progress(float progress)
    {
        if(nowSpdata.spLvel<=roleIconList.Count-1)
        roleIconList[nowSpdata.spLvel].UpdateRoleIcon_Progress(progress);
    }
    void RefreshSpLockState()//刷新下载图标[切换精灵|升级精灵]
    {
        for (int i = 0; i < roleIconList.Count; i++)
            roleIconList[i].RefreshSpLockState(nowSpdata);
    }
    public void RefreshIconSize(SpriteData sp)//刷新图标大小
    {
        for (int i = 0; i < roleIconList.Count; i++)
            roleIconList[i].RefreshIconSize(sp);
    }
    #endregion    
}

/// <summary>
/// 升级对象类
/// </summary>
public class UpgradeUI
{
    public Action UpgradeSuccess;
    public Action UpgradeFail;
    public Action UpgradeFullAction;
    public Action<float> UpgradeProgress;
    SpriteData spData;
    GameObject ui;
    Image fillImage = null;
    Vector3 _pos_new = Vector3.zero;
    Vector3 StartPos = new Vector3(0, 60, 0);
    Image pigImage = null;
    bool OneKeyUpgrade = false;
    bool SuccessWaiting = false;
    public UpgradeUI(GameObject uiobj, SpriteData data)
    {
        ui = uiobj;
        spData = data;
        SuccessWaiting = false;
    }
    public bool IsWaitingState()
    {
        return SuccessWaiting;
    }
    public void CloseUI()
    {
        ui.SetActive(false);
        if (spData == null)
            return;
        if (fillImage == null)
        {
            fillImage = ui.transform.GetChild(1).GetComponent<Image>();
        }
        fillImage.fillAmount = spData.spExp;
    }
    void Success()
    {
        if (fillImage.fillAmount >= 1.0f)
        {
            spData.spLvel++;
            if (spData.spLvel <= 3)
            {
                WizardData.SetWizardItemLevel(spData.Name, spData.spLvel);//上报精灵的等级
            }
            fillImage.fillAmount = 0;
            spData.spExp = 0;
            ui.SetActive(false);
            if (UpgradeSuccess != null)
                UpgradeSuccess();
            if (spData.spLvel >= 3)
            {
                if (UpgradeFullAction != null)
                    UpgradeProgress(1.0F);
            }
        }
    }

    void InitImageData()
    {
        if (fillImage == null)
        {
            fillImage = ui.transform.GetChild(1).GetComponent<Image>();
        }
        if (pigImage == null)
        {
            pigImage = ui.transform.GetChild(2).GetComponent<Image>();
        }
    }
    /// <summary>
    /// 长按升级
    /// </summary>
    /// <param name="timer"></param>
    public void Fresh_OnPointDownUI(float timer)
    {
        if (UpgradeSpFail())
            return;
        SpriteUplevelMgr.Instance.OpenUpUI_Center();
        InitImageData();
        upDataScoreClick();
        SetPigImagePos(pigImage, 180 - spData.spExp * 180, 350);
        if (spData.spExp < 1.0f)
        {
            WizardData.SetWizardItemLevelProgress(spData.Name, spData.spExp);
            fillImage.fillAmount = spData.spExp;
            UpgradeProgress(spData.spExp);
        }
        else
        {
            UpSuccess();
        }
    }
    /// <summary>
    /// 点击升级
    /// </summary>
    public void Fresh_OnclickUI()
    {
        if (UpgradeSpFail())
            return;
        SpriteUplevelMgr.Instance.OpenUpUI_Center();
        InitImageData();
        upDataScoreClick();
        SetPigImagePos(pigImage, 180 - spData.spExp * 180, 350);
        if (spData.spExp < 1.0f)
        {
            WizardData.SetWizardItemLevelProgress(spData.Name, spData.spExp);
            fillImage.fillAmount = spData.spExp;
            UpgradeProgress(spData.spExp);
        }
        else
        {
            UpSuccess();
        }
    }
    bool UpgradeSpFail()
    {
        if (spData.spLvel > 4)
            return true;
        else if (OneKeyUpgrade)
            return true;
        else if (SuccessWaiting)
            return true;
        else
            return false;
    }
    void UpSuccess()
    {
        SuccessWaiting = true;
        SpriteUplevelMgr.Instance.StartCoroutine(ResetSuccessWaiting());
        if (OneKeyUpgrade)
        {
            Tween tw = fillImage.DOFillAmount(1.0f, 1).OnComplete(OneclickupFinish);
            tw.OnUpdate(delegate { SetPigImagePos(pigImage, 180 - fillImage.fillAmount * 180, 350); });
        }
        else
        {
            spData.spExp = 0;
            WizardData.SetWizardItemLevelProgress(spData.Name, 0);
            fillImage.fillAmount = 1.0f;
            UpgradeProgress(1.0f);
            Success();
        }
    }
    IEnumerator ResetSuccessWaiting()//升级成功等待
    {
        yield return new WaitForSeconds(2.0F);
        SuccessWaiting = false;
    }
    //一键升级
    void OneclickupFinish()
    {
        OneKeyUpgrade = false;
        spData.spExp = 0;
        WizardData.SetWizardItemLevelProgress(spData.Name, 0);
        fillImage.fillAmount = 1.0f;
        UpgradeProgress(1.0f);
        Success();
    }

    void upDataScoreClick()
    {
        AddReport();
        if (Application.isEditor)
        {
            //OneKeyUpgrade = true;
            //Debug.LogError("当前经验:" + spData.spExp);
            if (spData.Name.Equals(WizardItemName.Wizard_BaiYin))//百音精灵（1,30,70）依次升级需要灵气
            {
                if (spData.spLvel >= 1)
                {
                    spData.spExp += 0.01f;
                    RewardUIManager.Instance.UpdateScore(-1);
                }
                else
                {
                    OneKeyUpgrade = true;
                    spData.spExp += 1f;
                    RewardUIManager.Instance.UpdateScore(-1);
                }
            }
            else
            {
                spData.spExp += 0.1f;
                RewardUIManager.Instance.UpdateScore(-1);
            }
            SetPigImagePos(pigImage, 180 - spData.spExp * 180, 350);
        }
        else//可以优化
        {
            if (spData.Name.Equals(WizardItemName.Wizard_BaiYin))//百音精灵（1,30,70）依次升级需要灵气
            {
                if (spData.spLvel == 0)
                {
                    spData.spExp += 1f;
                    OneKeyUpgrade = true;
                    RewardUIManager.Instance.UpdateScore(-1);
                }
                else if (spData.spLvel == 1)
                {
                    spData.spExp += 1 / 30f;
                    RewardUIManager.Instance.UpdateScore(-1);
                }
                else if (spData.spLvel == 2)
                {
                    spData.spExp += 1 / 70f;
                    RewardUIManager.Instance.UpdateScore(-1);
                }
            }
            else
            {
                if (spData.spLvel == 0)
                {
                    spData.spExp += 1 / 100f;
                    RewardUIManager.Instance.UpdateScore(-1);
                }
                else if (spData.spLvel == 1)
                {
                    spData.spExp += 1 / 200f;
                    RewardUIManager.Instance.UpdateScore(-1);
                }
                else if (spData.spLvel == 2)
                {
                    spData.spExp += 1 / 400f;
                    RewardUIManager.Instance.UpdateScore(-1);
                }
            }
        }
    }

    /// <summary>
    /// 设置猪位移
    /// </summary>
    /// <param name="image"></param>
    /// <param name="temp_angle"></param>
    /// <param name="_radius_length"></param>
    void SetPigImagePos(Image image, float temp_angle, float _radius_length)
    {
        image.transform.localEulerAngles = new Vector3(0, 0, temp_angle - 90);//设置猪旋转        
        temp_angle *= Mathf.Deg2Rad;
        _pos_new = new Vector3(StartPos.x + Mathf.Cos(temp_angle) * _radius_length, StartPos.y + Mathf.Sin(temp_angle) * _radius_length, image.transform.localPosition.z);
        image.transform.localPosition = _pos_new;
    }


    void AddReport()//用户点击收集 上报数据?
    {
        if (spData.Name != "")
        {
            if (AnimaData.TotalNimbus > 0)//灵气大于0
            {
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.spriteUpgrade, "Upgrade:" + spData.Name);
            }
            else
            {
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.spriteUpgrade, "UpgradeLqZero:" + spData.Name);
            }
        }
    }

}
