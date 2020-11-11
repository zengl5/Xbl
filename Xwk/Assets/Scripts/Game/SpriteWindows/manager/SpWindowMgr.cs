using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using XBL.Core;
using Assets.Scripts.C_Framework;
using XWK.Common.UI_Reward;
using YB.XWK.MainScene;
/// <summary>
/// 用户精灵列表刷新数据管理类
/// </summary>
class SpIconMgr
{
    List<SpriteIcon> SpList;
    List<RectTransform> SpPosList;
    public SpriteData NowSpdata;//当前精灵数据类
    public SpriteData ForwardSpdata;
    public SpriteIcon NowspIcon;
    bool readDataFinish = false;
    
    List<Vector3> SpInitPosList = new List<Vector3>();
    static Dictionary<int, SpriteData> SaveSpDic = new Dictionary<int, SpriteData>();
    static Dictionary<int, Vector3> SavePosDic = new Dictionary<int, Vector3>();
    //SpDataMgr spData;
    int index = 0;

    #region##外部接口
    public SpIconMgr(List<SpriteIcon> spList, List<RectTransform> spPosList)
    {
        SpList = spList;
        SpPosList = spPosList;
    }
    /// <summary>
    /// 刷新精灵选择列表【一进入精灵界面】
    /// </summary>
    /// <param name="userSpList"></param>
    /// <param name="spdata"></param>
    public void InitSpIcon()
    {
        //spData = spdata;
        NowspIcon = SpList[3];
        //要先更新解锁数据
        if (WizardData.IsNewUser())//新用户[切换账号]
        {
            if (SpDataMgr.Instance.IsGetNewSprite())
            {
                SpDataMgr.Instance.SetGetSprite();
            }
            InitBundaryData_Normal();
            StartRefreshNewUserData(SpDataMgr.Instance.SpDataList[index]);
        }
        else
        {
            if (SpDataMgr.Instance.IsGetNewSprite())//是否上新
            {
                InitBundaryData_NewSp();
                SpDataMgr.Instance.SetGetSprite();
                StartRefresh_NewSpData();
                //Debug.LogError("上新");
            }
            else
            {
                if (SaveSpDic.Count > 0)
                {
                    StartRefreshSaveData();
                }
                else
                {
                    //Debug.LogError("新用户");   
                    InitBundaryData_Normal();
                    StartRefreshNewUserData(SpDataMgr.Instance.SpDataList[index]);
                }
            }
        }
        InitIconPosData();
    }

    /// <summary>
    /// 记忆功能，保存精灵数据
    /// </summary>
    public void SaveIconData()
    {
        SaveSpDic.Clear();
        SavePosDic.Clear();
        for (int i = 0; i < SpList.Count; i++)
        {
            if (!SaveSpDic.ContainsKey(SpList[i].Id))
                SaveSpDic.Add(SpList[i].Id, SpList[i].GetSpriteData());
        }
    }

    #endregion

    #region ##上新数据初始化

    public void BoundaryUpAction()
    {
        SpriteIconMgr.SpDataHeadPos = SpDataMgr.Instance.SpDataList.Count - 4;
    }
    public void BoundaryDownAction()
    {
        SpriteIconMgr.SpDatabottomPos = 3;
    }
    /// <summary>
    /// 新用户数据边界值
    /// </summary>
    void InitBundaryData_NewUser()
    {
        SpriteIconMgr.SpDatabottomPos = 3;
        SpriteIconMgr.SpDataHeadPos = SpDataMgr.Instance.SpDataList.Count - 4;
    }
    /// <summary>
    /// 上新数据边界值
    /// </summary>
    void InitBundaryData_NewSp()
    {
        ////重新计算数组边界值
        SpriteIconMgr.SpDatabottomPos = SpDataMgr.Instance.SpDataList.Count + 1;//到底了
        SpriteIconMgr.SpDataHeadPos = SpDataMgr.Instance.SpDataList.Count - 5;
    }

    void InitBundaryData_Normal()
    {
        SpriteIconMgr.SpDatabottomPos = 3;
        SpriteIconMgr.SpDataHeadPos = -3;
    }
    #endregion

    #region##处理逻辑
    void freshNewSpData()
    {
        //重新排序最新精灵位置
        int NewSpIconIndex = SpDataMgr.Instance.SpDataList.Count - 1;//最新数据索引
        for (int i = 0; i < SpList.Count; i++)
        {
            if (i >= 2)
            {
                SpList[i].SetSpriteIcon(SpDataMgr.Instance.SpDataList[NewSpIconIndex]);
                NewSpIconIndex--;
            }
            else
            {
                SpList[i].SetSpriteIcon(null);
            }
        }
    }

    void InitIconPosData()
    {
        SpInitPosList.Clear();
        if (SaveSpDic.Count > 0)
        {
            List<SpriteData> SaveSpData = new List<SpriteData>();
            for (int i = 0; i < SaveSpDic.Count; i++)
            {
                if (SaveSpDic.ContainsKey(i))
                    SaveSpData.Add(SaveSpDic[i]);
            }
            for (int i = 0; i < SpList.Count; i++)
            {
                if (!SavePosDic.ContainsKey(SpList[i].Id))
                    SavePosDic.Add(SpList[i].Id, SpList[i].GetComponent<RectTransform>().anchoredPosition);
            }
            for (int i = 0; i < SavePosDic.Count; i++)
            {
                SpInitPosList.Add(SavePosDic[i]);
            }
        }
        else
        {
            for (int i = 0; i < SpPosList.Count; i++)
                SpInitPosList.Add(SpPosList[i].anchoredPosition);
        }

        for (int i = 0; i < SpList.Count; i++)
        {
            SpList[i].InitSpInitPosList(SpInitPosList);
        }

    }

    //新用户数据刷新
    void StartRefreshNewUserData(SpriteData sp)
    {
        SaveSpDic.Clear();
        NowSpdata = SpDataMgr.Instance.SpDataList[index];

        sp.SetStayCenter(true);
        for (int i = 0; i < SpList.Count; i++)
            if (i <= 3)
                if (3 - i <= SpDataMgr.Instance.SpDataList.Count - 1)
                    SpList[i].SetSpriteIcon(SpDataMgr.Instance.SpDataList[3 - i]);
        SpriteIconMgr.Instance.RefreshAllDataBegin(sp);
    }

    //记忆数据
    void StartRefreshSaveData()
    {
        freshSaveSprite();
        NowSpdata = SaveSpDic[3];
        SpriteIconMgr.Instance.RefreshAllDataBegin(NowSpdata);
    }

    //上新数据更新
    void StartRefresh_NewSpData()
    {
        freshNewSpData();
        NowSpdata = SpDataMgr.Instance.SpDataList[SpDataMgr.Instance.SpDataList.Count - 2];
        SpriteIconMgr.Instance.RefreshAllDataBegin(NowSpdata);
    }
    void freshSaveSprite()
    {
        for (int i = 0; i < SaveSpDic.Count; i++)
        {
            if(SaveSpDic.ContainsKey(i))
            {
                if (SaveSpDic[i] != null)
                {
                    SaveSpDic[i].Refresh();//刷新解锁状态                        
                }
            }         
        }
            
        for (int i = 0; i < SpList.Count; i++)
        {
            if (SpList[i] != null)
            {
                switch (SpList[i].Id)
                {
                    case 0:
                        if (SaveSpDic.ContainsKey(0))
                            if (SaveSpDic[0] != null)
                                SpList[i].SetSpriteIcon(SaveSpDic[0]);
                            else
                                SpList[i].SetSpriteIcon(null);
                        break;
                    case 1:
                        if (SaveSpDic.ContainsKey(1))
                            if (SaveSpDic[1] != null)
                                SpList[i].SetSpriteIcon(SaveSpDic[1]);
                            else
                                SpList[i].SetSpriteIcon(null);
                        break;
                    case 2:
                        if (SaveSpDic.ContainsKey(2))
                            if (SaveSpDic[2] != null)
                                SpList[i].SetSpriteIcon(SaveSpDic[2]);
                            else
                                SpList[i].SetSpriteIcon(null);
                        break;
                    case 3:
                        if (SaveSpDic.ContainsKey(3))
                            if (SaveSpDic[3] != null)
                                SpList[i].SetSpriteIcon(SaveSpDic[3]);
                            else
                                SpList[i].SetSpriteIcon(null);
                        break;
                    case 4:
                        if (SaveSpDic.ContainsKey(4))
                            if (SaveSpDic[4] != null)
                                SpList[i].SetSpriteIcon(SaveSpDic[4]);
                            else
                                SpList[i].SetSpriteIcon(null);
                        break;
                    case 5:
                        if (SaveSpDic.ContainsKey(5))
                            if (SaveSpDic[5] != null)
                                SpList[i].SetSpriteIcon(SaveSpDic[5]);
                            else
                                SpList[i].SetSpriteIcon(null);
                        break;
                    case 6:
                        if (SaveSpDic.ContainsKey(6))
                            if (SaveSpDic[6] != null)
                                SpList[i].SetSpriteIcon(SaveSpDic[6]);
                            else
                                SpList[i].SetSpriteIcon(null);
                        break;
                }
            }
        }
    }
    #endregion


}

