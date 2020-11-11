using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using XBL.Core;
using Assets.Scripts.C_Framework;
using XWK.Common.UI_Reward;
using YB.XWK.MainScene;
using System;
/// <summary>
/// 列表管理类
/// </summary>
public class SpriteIconMgr :  C_BaseUI
{  
    public static SpriteIconMgr Instance;
    public Action spriteSwithAction;
    [Header("[7个图标SpriteIcon对象引用]")]
    public List<SpriteIcon> SpList;
    [Header("[7个图标RectTransform对象引用]")]
    public List<RectTransform> SpPosList;
    [Space(30)]
    [Header("[升级提示手按钮]")]
    public GameObject HandDir;
    GameObject SpDir=null;
    bool readDataFinish = false;
    [Header("[灵气渲染相机]")]
    public Camera LingqiCam;
    [Header("[灵气物体]")]
    public Transform LingqiParent;
    [Space]
    [Header("[预先设置好的6个灵气固定点]")]
    public List<Transform> LqPosList;
    [Header("[精灵名称UI]")]
    public RawImage TitleImage;

    SpMeshRefresh meshReFresh = new SpMeshRefresh();//模型刷新
    SpriteMove spMove = new SpriteMove();//移动
    SpDataMgr spData;
    SpIconMgr spIconMgr;
    public MoveType move;
    int index = 0;
    public bool IsMovingState = false;
    public bool IsHidingState = false;
    public static int SpDatabottomPos = 3;
    public static int SpDataHeadPos = 3;

    protected override void onOpenUI(params object[] uiObjParams)
    {
        Instance = this;
        if(spIconMgr==null)
        {
            spIconMgr = new SpIconMgr(SpList, SpPosList);
        }
        spData = SpDataMgr.Instance;
        SpDataMgr.Instance.ReadJsonData("Config/spwindow/sprit.json", ReadJsonDataFinish);
        AudioClip clipef = ABResMgr.Instance.LoadResource<AudioClip>(SpWindowPath.Instance.public_xwkbgm_008,ABCommonConfig.Instance.SpWindowBundleType);
        AudioManager.Instance.PlayBgMusic(clipef);
    }
    protected override void onCloseUI()
    {
        readDataFinish = false;
        SpriteLingqiMgr.Instance.LeaveSpWindow();
        AudioManager.Instance.StopBgMusic();
        spIconMgr.SaveIconData();
        base.onCloseUI();
        meshReFresh.RemoveAllMesh();
        if (SpDir)
            Destroy(SpDir);
    }

    void Update()
    {
        if (readDataFinish)//所有数据读取完成，才可以滑动
            spMove.Update();
    }

    void ReadJsonDataFinish()
    {
        spMove.InitIconData(SpList, SpPosList, meshReFresh);
        spIconMgr.InitSpIcon();
        AddReportData();
        AddCoinDeal();
        SpriteUplevelMgr.Instance.RefreshUpGradeUI();
        SpriteLingqiMgr.Instance.InitOnlineLingq();//初始灵气个数
        readDataFinish = true;
        if (GameObject.Find("SpriteWindowCam"))
            MeshButtonManager.Instance.SetMeshCamera(GameObject.Find("SpriteWindowCam").GetComponent<Camera>());
    }

    #region##状态处理
    /// <summary>
    /// 刷新角标
    /// </summary>
    /// <param name="spdata"></param>
    public void ShowCornerMark(SpriteData spdata)
    {
        for (int i = 0; i < SpList.Count; i++)
        {
            if (SpList[i].GetSpriteData() != null)
                if (SpList[i].GetSpriteData().Name.Equals(spdata.Name))
                {
                    SpList[i].OpenCornermarker(true);
                }
        }
    }
    //是否正在滑动
    public bool IsMoving()
    {
        return spMove.isMoving;
    }
    public bool GetNextHidingState(int id)
    {       
        for(int i=0;i<SpList.Count;i++)
        {
            if (SpList[i].Id == id)
            {
                if (SpList[i].HidingState())
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void SetHide(int id)
    {
        for (int i = 0; i < SpList.Count; i++)
        {
            if (SpList[i].Id == id)
            {
                SpList[i].HideImage();
            }
        }
    }
    public SpriteData GetSpriteData()
    {
        return spIconMgr.NowSpdata;
    }
    public SpriteIcon GetSpIcon()
    {
        return spIconMgr.NowspIcon;
    }
    public void BoundaryUpAction()
    {
        SpDataHeadPos = spData.SpDataList.Count - 4;
    }
    public void BoundaryDownAction()
    {
        SpDatabottomPos = 3;
    }
    #endregion


    #region#精灵数据刷新
    public void RefreshAllDataBegin(SpriteData data)
    {
        if(data==null)
        {
            Debug.LogError("SpriteData is null");
        }
        spIconMgr.ForwardSpdata = data;
        RefreshStayCenter(data);
        RefreshStarLevel(data);

        meshReFresh.RefreshModel(data);
        SpritBtn.Instance.RefreshUI(data);
        meshReFresh.RefreshMeshTitle(TitleImage, data);
        meshReFresh.RefreshRoleSound(data);

        RefreshSpLockOrCenterTex();
        SpriteUplevelMgr.Instance.RefreshUpGradeUI();//刷新升级UI;
    }
    /// <summary>
    /// 在每次滑动选择精灵后
    /// </summary>
    /// <param name="sp"></param>
    /// <param name="icon"></param>
    public void RefreshAllData(SpriteData sp,SpriteIcon icon)
    {
        if (sp == null)
            return;
        if (spriteSwithAction != null)
            spriteSwithAction();
        spIconMgr.NowspIcon = icon;
        spIconMgr.NowSpdata = sp;
        RefreshStayCenter(sp);
        RefreshStarLevel(sp);

        meshReFresh.RefreshArrow(true);
        SpritBtn.Instance.RefreshUI(sp);
        meshReFresh.CloseBeforeMesh(sp);
        meshReFresh.RefreshModel(sp);
        meshReFresh.RefreshMeshTitle(TitleImage,sp);
        meshReFresh.RefreshRtmc(sp, meshReFresh.NowMesh);
        meshReFresh.ResetEulerAngle(sp);
        meshReFresh.RefreshRoleSound(sp);
        meshReFresh.RefreshAnim();

        ReportData(sp);
        SpriteUplevelMgr.Instance.RefreshUpGradeUI();//刷新升级UI;刷新state
        SpriteUplevelMgr.Instance.ShowUpDirHand(false);
        SpriteLingqiMgr.Instance.RefreshLingqi(sp);
        RefreshIdleEffect(sp.nowRoleState);//刷新待机特效
        SpriteUplevelMgr.Instance.RefreshIconSize(sp);//刷新底部坐标
    }


    void RefreshStarLevel(SpriteData sp)
    {
        if (sp != null)
            sp.InitLevel(WizardData.FetchWizardItemStar(sp.Name));
        else
            Debug.LogError("RefreshStarLevel Error");
    }  

    
    void RefreshStayCenter(SpriteData sp)
    {
        for (int i = 0; i < spData.SpDataList.Count; i++)
        {
            if (spData.SpDataList[i] == sp)
            {
                sp.SetStayCenter(true);
            }
            else
            {
                spData.SpDataList[i].SetStayCenter(false);
            }
        }
    }

    void RefreshSpLockOrCenterTex()
    {
        for (int i = 0; i < SpList.Count; i++)
        {
            if (SpList[i].Id == 3)
            {
                if (SpList[i].GetSpriteData() != null)
                    SpList[i].GetSpriteData().SetStayCenter(true);
            }
            else
            {
                if (SpList[i].GetSpriteData() != null)
                    SpList[i].GetSpriteData().SetStayCenter(false);
            }
            SpList[i].RefreshSpLockOrCenterTex();
        }
    }
    #endregion
               
   
    #region##模型换装升级
    public void UpgradeMeshTexture(int level)
    {
        LocalData.RoleLevel = level;
        meshReFresh.UpgradeMeshTexture(level);
    }
    public void RefreshIdleEffect(int roleState)
    {
        meshReFresh.RefreshIdleEffect(roleState);
    }
    public void RefreseRoleState(int roleState)
    {
        spIconMgr.NowSpdata.nowRoleState = roleState;
    }
    public void RotateMesh(bool right)
    {
        meshReFresh.RotateMesh(right);
    }
    #endregion


    #region##精灵列表移动管理
    public void MoveUp()
    {
        spMove.MoveUp();
    }
    public void MoveDown()
    {
        spMove.MoveDown();
    }
    public void ResetAutoBack()
    {
        spMove.ResetAutoBack();
    }
    void SetMoving()
    {
        spMove.SetMoving();
    }
    #endregion


    #region##数据统计埋点与灵气结算
    /// <summary>
    /// 灵气结算
    /// </summary>
    void AddCoinDeal()
    {
        if (LocalData.GotoStoryOrGameBy_SpWindow == true)//进入游戏或者故事通过二级界面
        {
            LocalData.GotoStoryOrGameBy_SpWindow = false;
            RewardUIManager.Instance.ChangeModule(ModuleType.SpriteWindow, "");
            RewardUIManager.Instance.RegisterSpriteWindow(RewardUIManager.Instance.GetSpriteSpriteWindowModuleGainNum(), GotoDirectorUI);//30   10   // 31 32  33  34 .....40
            RewardUIManager.Instance.SetSpriteWindowScore();
            RewardUIManager.Instance.SetSuccess();
            RewardUIManager.Instance.ClearModuleGainNum();
        }
        else
        {
            GotoDirectorUI(true);
            RewardUIManager.Instance.ChangeModule(ModuleType.SpriteWindow, "");
            RewardUIManager.Instance.ClearModuleGainNum();
            return;
        }
    }
    void GotoDirectorUI(bool flag)
    {
        C_UIMgr.Instance.OpenUI("DirectorUI");//如果去掉引导，需要修改全局变量
        SpritBtn.Instance.RefreshButtonInteractable();
    }

    void AddReportData()//数据统计
    {
        if (WizardData.IsNewUser())
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_spriteWindow, "EnterSpWindow_NewUser");
        }
        else
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_spriteWindow, "EnterSpWindow");
        }
        if (spIconMgr.NowSpdata != null)
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeStart, LocalData.m_spriteWindow, spIconMgr.NowSpdata.Name);
    }
    /// <summary>
    /// 数据埋点，保持上一次数据停止计时
    /// </summary>
    /// <param name="sp"></param>
    public void RefreshOldData(SpriteData sp)
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, LocalData.m_spriteWindow, sp.Name);
    }
    void ReportData(SpriteData sp)
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_spriteWindow, "ClickButton:" + sp.Name);
        if (spIconMgr.NowSpdata != null)
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeStart, LocalData.m_spriteWindow, spIconMgr.NowSpdata.Name);
    }
    public void ShowHand()
    {
        if (spIconMgr.NowSpdata == null)
            return;
        if (WizardData.UEList.Count <= 1)
        {
            if (spIconMgr.NowSpdata.Name.Equals(WizardItemName.Wizard_BaiYin))
            {
                if (spIconMgr.NowSpdata.spLvel == 0)
                {
                    SpriteUplevelMgr.Instance.ShowUpDirHand(true);
                    C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_spriteWindow, "UpgradeNewUser");//首次上报新用户升级
                }
            }
        }
    }
    #endregion
}
