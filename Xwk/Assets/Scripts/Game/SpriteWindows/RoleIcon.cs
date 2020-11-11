using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 右侧角色当前等级数据显示类
/// </summary>
public class RoleIcon : MonoBehaviour
{
    public Button SpriteBtn;
    public Image LoadingUI;
    public RawImage IconTex;
    public GameObject BottomUI;
    public int Level = 1;
    bool lockingState = true;
    public List<GameObject> RoleIconUiList = new List<GameObject>();
    SpriteData spData;
    RectTransform rect;
    string baseRolePath= "game/SpriteWindow/ui/upgradui/";
    string lockIconName1 = "btn_jingling_sj_suo_1";
    string lockIconName2 = "btn_jingling_sj_suo_2";
    string lockIconName3 = "btn_jingling_sj_suo_3";
    string lockIconName4 = "btn_jingling_sj_suo_4";
    void Awake()
    {    
        SpriteBtn.onClick.AddListener(AddClickEvent);
        rect = IconTex.transform.GetComponent<RectTransform>();
    }
    /// <summary>
    /// 初始化图标状态
    /// </summary>
    /// <param name="sp"></param>
    public void RefreshSpLockState(SpriteData sp)
    {
        spData = sp;
        if (sp == null)
            return;
        if(sp.lockIcon)//解锁
        {
            if(Level==0)
            {
                lockingState = false;
            }
            else
            {
                if (Level<=sp.spLvel)//0  1  2  3
                {
                    lockingState = false;
                }
                else
                {
                    lockingState = true;
                }
            }
        }
        else
        {
            lockingState = true;
        }
        RefreshIcon(lockingState);//图标刷新      
        RefreshIconSize(sp);//大小切换
        RefreshLoadingUi(sp);
    }

    void RefreshLoadingUi(SpriteData sp)
    {
        //loadingui显示隐藏   
        if(sp.spLvel>=3)
        {
            OpenLodingUI(false);
        }
        else
        {
            if(sp.spLvel==0)
            {
                OpenLodingUI(false);
            }
            else
            {
                if (Level == sp.spLvel)
                {
                    if (sp.spExp > 0)
                    {
                        OpenLodingUI(true);
                        UpdateRoleIcon_Progress(sp.spExp);
                    }
                    else
                    {
                        OpenLodingUI(false);
                    }
                }
                else
                {
                    OpenLodingUI(false);
                }
            }           
        }       
    }

    void AddClickEvent()
    {
        if (SpriteIconMgr.Instance.GetSpriteData() == null)
            return;
         
        if (lockingState)//未升级
        {
            Spwindow.PlayCharacterAudio("xwk_jlej_11");
        }

        if (!SpriteIconMgr.Instance.GetSpriteData().lockIcon)//未解锁
        {
            Spwindow.PlayCharacterAudio("xwk_jlej_7");
        }

        if (!SpriteIconMgr.Instance.GetSpriteData().lockIcon)
            return;
        if (lockingState)
            return;

        switch (Level)
        {
            case 0:
                SpriteIconMgr.Instance.RefreseRoleState(0);//更新状态
                SpriteIconMgr.Instance.UpgradeMeshTexture(0);//模型贴图
                SpriteIconMgr.Instance.RefreshIdleEffect(0);//刷新待机特效
                SpriteUplevelMgr.Instance.RefreshUpBtn();//升级按键
                SpriteUplevelMgr.Instance.RefreshBgColor(0);//背景
                break;

            case 1://换装
                SpriteIconMgr.Instance.UpgradeMeshTexture(1);
                SpriteIconMgr.Instance.RefreseRoleState(1);
                SpriteIconMgr.Instance.RefreshIdleEffect(1); 
                SpriteUplevelMgr.Instance.RefreshUpBtn();
                SpriteUplevelMgr.Instance.RefreshBgColor(1);
                break;

            case 2://换装
                SpriteIconMgr.Instance.UpgradeMeshTexture(2);
                SpriteIconMgr.Instance.RefreseRoleState(2);
                SpriteIconMgr.Instance.RefreshIdleEffect(2);


                SpriteUplevelMgr.Instance.RefreshUpBtn();
                SpriteUplevelMgr.Instance.RefreshBgColor(2);

                break;

            case 3://换装
                SpriteIconMgr.Instance.UpgradeMeshTexture(3);
                SpriteIconMgr.Instance.RefreshIdleEffect(3);
                SpriteIconMgr.Instance.RefreseRoleState(3);
                SpriteUplevelMgr.Instance.RefreshUpBtn();
                SpriteUplevelMgr.Instance.RefreshBgColor(3);
                break;
        }
        Spwindow.PlaySmallClipSound("public_xwkyx_068");
        SpriteUplevelMgr.Instance.RefreshIconSize(SpriteIconMgr.Instance.GetSpriteData());//刷新图标
    }

    /// <summary>
    /// 刷新实时进度
    /// </summary>
    public void UpdateRoleIcon_Progress(float progress)
    {
        if (!LoadingUI.gameObject.activeInHierarchy)
        {
            OpenLodingUI(true);
        }
        if(progress==1.0f)
        {
            OpenLodingUI(false);
        }
        else
        {
            LoadingUI.fillAmount = progress;
            if (LoadingUI.fillAmount >= 1)
                OpenLodingUI(false);
        }
    }

    //关闭|隐藏显示进度条
    public void OpenLodingUI(bool flag)
    {
        for (int i = 0; i < RoleIconUiList.Count; i++)
                RoleIconUiList[i].gameObject.SetActive(flag);
    }  
    //等级图标UI切换
    void RefreshIcon(bool lockingstate)
    {
        if(!lockingstate)
        {
            Texture2D obj;
            switch (Level)
            {
                case 0:
                    obj = ABResMgr.Instance.LoadResource<Texture2D>(baseRolePath+spData.Roledata.level1Path, ABCommonConfig.Instance.SpWindowBundleType, false, false);
                    IconTex.texture = obj;
                    break;
                case 1:
                    obj = ABResMgr.Instance.LoadResource<Texture2D>(baseRolePath + spData.Roledata.level2Path, ABCommonConfig.Instance.SpWindowBundleType, false, false);
                    IconTex.texture = obj;
                    break;
                case 2:
                    obj = ABResMgr.Instance.LoadResource<Texture2D>(baseRolePath + spData.Roledata.level3Path, ABCommonConfig.Instance.SpWindowBundleType, false, false);
                    IconTex.texture = obj;
                    break;
                case 3:
                    obj = ABResMgr.Instance.LoadResource<Texture2D>(baseRolePath + spData.Roledata.level4Path, ABCommonConfig.Instance.SpWindowBundleType, false, false);
                    IconTex.texture = obj;
                    break;
            }                
        }
        else
        {
            Texture2D obj;
            switch (Level)
            {
                case 0:
                    obj = ABResMgr.Instance.LoadResource<Texture2D>(baseRolePath + lockIconName1, ABCommonConfig.Instance.SpWindowBundleType, false, false);
                    IconTex.texture = obj;
                    break;
                case 1:
                    obj = ABResMgr.Instance.LoadResource<Texture2D>(baseRolePath + lockIconName2, ABCommonConfig.Instance.SpWindowBundleType, false, false);
                    IconTex.texture = obj;
                    break;
                case 2:
                    obj = ABResMgr.Instance.LoadResource<Texture2D>(baseRolePath + lockIconName3, ABCommonConfig.Instance.SpWindowBundleType, false, false);
                    IconTex.texture = obj;
                    break;
                case 3:
                    obj = ABResMgr.Instance.LoadResource<Texture2D>(baseRolePath + lockIconName4, ABCommonConfig.Instance.SpWindowBundleType, false, false);
                    IconTex.texture = obj;
                    break;
            }
        }
    }

    public void RefreshIconSize(SpriteData sp)//切换图标大小
    {
        if(sp.lockIcon)
        {
            if (sp.nowRoleState == Level)
            {
                BottomUI.SetActive(true);
                rect.sizeDelta = new Vector2(152, 152);
                for (int i = 0; i < RoleIconUiList.Count; i++)
                    RoleIconUiList[i].transform.localScale = Vector3.one;
                for (int i = 0; i < RoleIconUiList.Count; i++)
                    RoleIconUiList[i].transform.localPosition = new Vector3(RoleIconUiList[i].transform.localPosition.x,260, RoleIconUiList[i].transform.localPosition.z);
            }
            else
            {
                BottomUI.SetActive(false);
                rect.sizeDelta = new Vector2(128, 128);
                for (int i = 0; i < RoleIconUiList.Count; i++)
                    RoleIconUiList[i].transform.localScale = 0.84f*Vector3.one;
                for (int i = 0; i < RoleIconUiList.Count; i++)
                    RoleIconUiList[i].transform.localPosition = new Vector3(RoleIconUiList[i].transform.localPosition.x, 255, RoleIconUiList[i].transform.localPosition.z);
            }
        }        
        else
        {
            BottomUI.SetActive(false);
            rect.sizeDelta = new Vector2(128, 128);
        }
        
       
    }
    

    
}