using Assets.Scripts.C_Framework;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 按钮边界移动
/// </summary>
public class BoundaryMove
{
     List<Vector3> PointList;
     RectTransform target;

    /// <summary>
    /// 边界移动管理类
    /// </summary>
    /// <param name="target"></param>
    /// <param name="List"></param>
    public BoundaryMove(RectTransform target, List<Vector3> List)
    {
        PointList = List;
        this.target = target;
    }

    public void MoveUp(int id)
    {
        if (id == 6)
        {
            Vector3 nextPos = new Vector3(PointList[id].x - 100, PointList[id].y + 100, 0);
            target.DOAnchorPos(nextPos, 0.15f);
            target.anchoredPosition = PointList[0];
        }
        else
        {
            Vector3 centerPos = new Vector3(0.5f * (PointList[id + 1].x + PointList[id].x), 0.5f * (PointList[id + 1].y + PointList[id].y), 0);
            target.DOAnchorPos(centerPos, 0.15f);
        }
    }

    public void MoveDown(int id)
    {
        if (id == 0)
        {
            target.anchoredPosition = PointList[6];
        }
        else
        {
            Vector3 centerPos = new Vector3(0.5f * (PointList[id - 1].x + PointList[id].x), 0.5f * (PointList[id - 1].y + PointList[id].y), 0);
            target.DOAnchorPos(centerPos, 0.15f);
        }
    }

    public void MoveAutoBack(int id)
    {
        target.DOAnchorPos(PointList[id], 0.15f).OnComplete(SpriteIconMgr.Instance.ResetAutoBack);
    }
}

/// <summary>
/// 按钮正常移动
/// </summary>
public class NormalMove
{
     List<Vector3> PointList;
     RectTransform rect;
     SpriteIcon spicon;

    /// <summary>
    /// 普通移动类
    /// </summary>
    /// <param name="spicon"></param>
    /// <param name="target"></param>
    /// <param name="List"></param>
    public NormalMove(SpriteIcon spicon, RectTransform target, List<Vector3> List)
    {
        PointList = List;
        this.rect = target;
        this.spicon = spicon;
    }

    public void MoveUp(int Id)
    {       
        if (spicon.GetSpriteData() != null)
            spicon.GetSpriteData().SetStayCenter(false);
        if (PointList.Count <= 0)
            return;
        if (Id == 6)
        {
            rect.anchoredPosition = PointList[0];
            spicon.ResetID_0();
        }
        else
        {
            if (Id == 2)
            {
                //if (spicon.GetSpriteData() != null)
                //    spicon.GetSpriteData().SetStayCenter(false);
                rect.DOAnchorPos(PointList[Id + 1], 0.3F).OnComplete(spicon.MoveUpFinish);
            }
            else if (Id == 0)
            {
                for (int i = 0; i < SpriteIconMgr.Instance.SpList.Count; i++)
                {
                    if (SpriteIconMgr.Instance.SpList[i].Id == 1)
                    {
                        if(SpriteIconMgr.Instance.SpList[i].GetSpriteData()!=null)
                        {
                            if (SpriteIconMgr.Instance.SpList[i].HidingState())
                            {
                                spicon.HideImage();
                            }
                            else if (SpriteIconMgr.Instance.SpList[i].GetSpriteData().Name.Equals("future"))
                            {
                                spicon.HideImage();
                            }
                        }
                        
                    }
                }
                rect.DOAnchorPos(PointList[Id + 1], 0.3F).OnComplete(spicon.MoveUpFinish);
            }
            else
            {
                rect.DOAnchorPos(PointList[Id + 1], 0.3F).OnComplete(spicon.MoveUpFinish);
            }
        }
    }
    public void MoveDown(int Id)
    {
        if (spicon.GetSpriteData() != null)
            spicon.GetSpriteData().SetStayCenter(false);
        if (PointList.Count <= 0)
            return;
        if (Id == 0)
        {
            for (int i = 0; i < SpriteIconMgr.Instance.SpList.Count; i++)
            {
                if (SpriteIconMgr.Instance.SpList[i].Id == 6)
                {
                    if (SpriteIconMgr.Instance.SpList[i].GetSpriteData() != null)
                    {
                        if (SpriteIconMgr.Instance.SpList[i].HidingState())
                        {
                            spicon.HideImage();
                        }
                        else if (SpriteIconMgr.Instance.SpList[i].GetSpriteData().Name.Equals(WizardItemName.Wizard_BaiYin))
                        {
                            spicon.HideImage();
                        }
                    }
                }
            }
            rect.anchoredPosition = PointList[6];
            spicon.ResetID_6();
        }
        else
        {
            if (Id - 1>=PointList.Count)
                return;
            if (Id == 4)//居中缩放处理
            {
                if (spicon.GetSpriteData() != null)
                {
                    spicon.GetSpriteData().SetStayCenter(true);
                    spicon.SetSpriteIcon(spicon.GetSpriteData());
                }
                rect.DOAnchorPos(PointList[Id - 1], 0.3F).OnComplete(spicon.MoveDownFinish);
            }
            else if (Id == 6)//边界处理
            {               
                rect.DOAnchorPos(PointList[Id - 1], 0.3F).OnComplete(spicon.MoveDownFinish);
            }
            else
            {
                rect.DOAnchorPos(PointList[Id - 1], 0.3F).OnComplete(spicon.MoveDownFinish);
            }
        }
    }
}


public class SpriteIcon : MonoBehaviour
{
    GameObject Cornermarker;
    public string SpriteDataName;
    string baseIconPath = "game/SpriteWindow/ui/role/";
    public int Id = 0;
    public bool isMoving = false;
    RectTransform rect;
    Button button;
    RawImage image;
    SpriteData spdata;
    BoundaryMove boundary;
    NormalMove normMove;
    List<Vector3> SpInitPosList = new List<Vector3>();
    void Awake()
    {
        rect = this.GetComponent<RectTransform>();
        button = this.GetComponent<Button>();
        image = this.GetComponent<RawImage>();
        button.onClick.AddListener(AddButtonEvent);
        Cornermarker = this.transform.GetChild(0).gameObject;
        OpenCornermarker(false);
    }

    void Update()
    {
        if (spdata != null)
            SpriteDataName = spdata.Name;
        else
            SpriteDataName = "";

        if (spdata != null)
        {
            if (spdata.lockIcon)
            {
                if (!GetNullData() && spdata.lockIcon)
                {
                    if (OpenCorner())
                    {
                        OpenCornermarker(true);
                    }
                    else
                    {
                        OpenCornermarker(false);
                    }
                }
            }
            else
            {
                OpenCornermarker(false);
            }
        }
    }

  
    /// <summary>
    /// 构造数据集合和移动管理类
    /// </summary>
    /// <param name="obj"></param>
    public void InitSpInitPosList(List<Vector3> obj)
    {
        SpInitPosList = obj;
        boundary = new BoundaryMove(rect, SpInitPosList);
        normMove = new NormalMove(this, rect, SpInitPosList);
    }

    #region## icon设置
    /// <summary>
    /// 更新是否解锁或者中间位置
    /// </summary>
    public void RefreshSpLockOrCenterTex()
    {
        if (spdata == null)
            return;
        if (spdata.Name.Equals(WizardItemName.future))
        {
            OpenCornermarker(false);
        }
        //Debug.LogError("FixedSetSpriteIcon:::" + spdata.Name + "stayCenter::" + spdata.stayCenter);
        if (!spdata.lockIcon)
        {
            ///AddLock();
            if (spdata.stayCenter)
            {
                Texture2D tex = ABResMgr.Instance.LoadResource<Texture2D>(baseIconPath + spdata.lockCenterName, "", false, true);
                image.texture = tex;
            }
            else
            {
                Texture2D tex = ABResMgr.Instance.LoadResource<Texture2D>(baseIconPath + spdata.lockName, "", false, true);
                image.texture = tex;
            }
        }
        else
        {
            if (spdata.stayCenter)
            {
                Texture2D tex = ABResMgr.Instance.LoadResource<Texture2D>(baseIconPath + spdata.releaseCenterName, "", false, true);
                image.texture = tex;
            }
            else
            {
                Texture2D tex = ABResMgr.Instance.LoadResource<Texture2D>(baseIconPath + spdata.releaseName, "", false, true);
                image.texture = tex;
            }
        }
    }

    /// <summary>
    /// 设置Spreite数据
    /// </summary>
    /// <param name="data"></param>
    public void SetSpriteIcon(SpriteData data)
    {
        if (data == null)
        {
            if (image == null) image = this.GetComponent<RawImage>();
            image.enabled = false;
            return;
        }
        spdata = data;
        if (data != null)
        {
            if (image == null) image = this.GetComponent<RawImage>();
            image.enabled = true;
        }
        if (!data.lockIcon)
        {
            if (data.stayCenter)
            {
                Texture2D tex = ABResMgr.Instance.LoadResource<Texture2D>(baseIconPath + data.lockCenterName, "", false, true);
                image.texture = tex;
            }
            else
            {
                Texture2D tex = ABResMgr.Instance.LoadResource<Texture2D>(baseIconPath + data.lockName, "", false, true);
                image.texture = tex;
            }
        }
        else
        {
            if (data.stayCenter)
            {
                Texture2D tex = ABResMgr.Instance.LoadResource<Texture2D>(baseIconPath + data.releaseCenterName, "", false, true);
                image.texture = tex;
            }
            else
            {
                Texture2D tex = ABResMgr.Instance.LoadResource<Texture2D>(baseIconPath + data.releaseName, "", false, true);
                image.texture = tex;
            }
        }
        if (Id == 3)
        {
            SetCornermarkerSize(true);
        }
        else
        {
            SetCornermarkerSize(false);
        }
    }

    public void OpenCornermarker(bool flag)//红色角标
    {
        Cornermarker.SetActive(flag);
    }
    public void HideImage()
    {
        this.spdata = null;
        image.enabled = false;
        OpenCornermarker(false);
    }
    void SetCornermarkerSize(bool center)
    {
        if (center)
            Cornermarker.transform.localPosition = new Vector3(90, 70, 0);
        else
            Cornermarker.transform.localPosition = new Vector3(50, 50, 0);
    }
    #endregion

    #region##状态管理
    public bool HidingState()
    {
        return !image.enabled;
    }

    public SpriteData GetSpriteData()
    {
        return spdata;
    }

    public bool GetNullData()
    {
        return GetSpriteData() == null ? true : false;
    }
    /// <summary>
    /// 判断是否需要打开角标
    /// </summary>
    /// <returns></returns>
    bool OpenCorner()
    {
        if (spdata.Name.Equals(WizardItemName.Wizard_BaiYin))
        {
            return SpriteLingqiMgr.Instance.IsFullLingqi(spdata.Name) |
                (DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Game_byjl) && DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Story_Byjl));
        }
        else if (spdata.Name.Equals(WizardItemName.Wizard_Hulu))
        {
            return SpriteLingqiMgr.Instance.IsFullLingqi(spdata.Name) |
                (DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Story_Hulu) && DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Game_bbhl));
        }
        else if (spdata.Name.Equals(WizardItemName.Wizard_Xln))
        {
            return SpriteLingqiMgr.Instance.IsFullLingqi(spdata.Name) |
                DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Story_Ln);
        }
        else
        {
            return SpriteLingqiMgr.Instance.IsFullLingqi(spdata.Name);
        }
    }
    #endregion

    #region##Event
    void AddButtonEvent()
    {
        if (!DirectorMgr.DirectorAllState)
            return;
        if (Id == 1 | Id == 2)
        {
            SpriteIconMgr.Instance.MoveUp();
        }
        else if (Id == 4 | Id == 5)
        {
            SpriteIconMgr.Instance.MoveDown();
        }
    }
    #endregion

    #region#移动接口

    public void MoveUp()
    {
        if (normMove != null)
            normMove.MoveUp(Id);
    }

    public void MoveDown()
    {
        if (normMove != null)
            normMove.MoveDown(Id);
    }

    public void BoundaryUp()
    {
        if (boundary != null)
            boundary.MoveUp(Id);
    }

    public void BoundaryDown()
    {
        if (boundary != null)
            boundary.MoveDown(Id);
    }

    public void MoveAutoBack()
    {
        if (boundary != null)
            boundary.MoveAutoBack(Id);
    }
    #endregion

    #region##移动完成Event
    /// <summary>
    /// 往上切换
    /// </summary>
    public void ResetID_0()
    {
        Invoke("DelayResetID_0", 0.25F);
    }
    void DelayResetID_0()
    {
        Id = 0;
        SpriteIconMgr.SpDataHeadPos++;
        SpriteIconMgr.SpDatabottomPos++;

        //Debug.LogError(SpriteIconMgr.SpDatabottomPos);
        if (SpriteIconMgr.SpDatabottomPos <=SpDataMgr.Instance.SpDataList.Count - 1)
        {
            this.image.enabled = true;
            if (SpriteIconMgr.SpDatabottomPos >= 0)
                SetSpriteIcon(SpDataMgr.Instance.SpDataList[SpriteIconMgr.SpDatabottomPos]);
            // Debug.LogError(SpDataList[SpriteIconMgr.SpDatabottomPos].Name+"ID:"+ SpriteIconMgr.SpDatabottomPos);
        }
        else
        {
            HideImage();
        }
    }


    /// <summary>
    /// 往下切换
    /// </summary>
    public void ResetID_6()
    {
        Invoke("DelayResetID_6", 0.3F);
    }
    void DelayResetID_6()
    {
        Id = 6;
        SpriteIconMgr.SpDatabottomPos--;
        SpriteIconMgr.SpDataHeadPos--;
        //Debug.LogError("SpDataHeadPos:"+SpriteIconMgr.SpDataHeadPos);
        if (SpriteIconMgr.SpDataHeadPos >= 0) //[0,1,2]
        {
            //判断下一个值是不是空，如果是，则这个值也是空
            this.image.enabled = true;
            if (SpriteIconMgr.SpDataHeadPos <=SpDataMgr.Instance.SpDataList.Count - 1)
            {
                SetSpriteIcon(SpDataMgr.Instance.SpDataList[SpriteIconMgr.SpDataHeadPos]);
            }
        }
        else
        {
            HideImage();
        }
        //如果下一个点是百音精灵,则隐藏这个点，或者下一个点是Null

        if (SpriteIconMgr.Instance.GetNextHidingState(5))
        {
            HideImage();
        }
        else
        {
            for (int i = 0; i < SpriteIconMgr.Instance.SpList.Count; i++)
            {
                if (SpriteIconMgr.Instance.SpList[i].Id == 5)
                {
                    if (SpriteIconMgr.Instance.SpList[i].GetSpriteData() != null)
                    {
                        if (SpriteIconMgr.Instance.SpList[i].GetSpriteData().Name.Equals(WizardItemName.Wizard_BaiYin))
                        {

                            HideImage();
                            this.spdata = null;
                        }
                    }
                }
            }
        }

    }

    public void MoveUpFinish()
    {
        Id++;
        if (Id == 3)
        {
            if (spdata != null)
            {
                SpriteIconMgr.Instance.RefreshAllData(spdata, this);
                spdata.SetStayCenter(true);
                SetSpriteIcon(spdata);
            }        
        }
        else
        {
            if (spdata != null)
            {
                spdata.SetStayCenter(false);
                SetSpriteIcon(spdata);
            }            
        }
        if (Id == 4)
        {
            if (spdata != null)
                SpriteIconMgr.Instance.RefreshOldData(spdata);
        }
    }




    public void MoveDownFinish()
    {
        Id--;

        if (Id == 3)
        {
            if (spdata != null)
            {
                spdata.SetStayCenter(true);
                SetSpriteIcon(spdata);
                SpriteIconMgr.Instance.RefreshAllData(spdata, this);
            }
            SetCornermarkerSize(true);
        }
        else
        {
            if (spdata != null)
            {
                spdata.SetStayCenter(false);
                SetSpriteIcon(spdata);
            }
            SetCornermarkerSize(false);
        }
        if (Id == 2)
        {
            if (spdata != null)
                SpriteIconMgr.Instance.RefreshOldData(spdata);
        }
    }

    #endregion
}