using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XBL.Core;

public enum MoveType
{
    Normal,//正常移动
    BoundaryUp,//往上是边界
    BoundaryDown//往下是边界
}
/// <summary>
/// 左侧图标移动管理类
/// </summary>
public class SpriteMove{
    public void InitIconData(List<SpriteIcon> SpList, List<RectTransform> SpPosList,SpMeshRefresh refresh)
    {
        this.SpList = SpList;
        this.SpPosList = SpPosList;
        reFresh = refresh;
        BoundaryUpAction += SpriteIconMgr.Instance.BoundaryUpAction;
        BoundaryDownAction += SpriteIconMgr.Instance.BoundaryDownAction;
    }
    public Action BoundaryUpAction;
    public Action BoundaryDownAction;
    List<SpriteIcon> SpList=new List<SpriteIcon>();
    List<RectTransform> SpPosList=new List<RectTransform>();
    SpMeshRefresh reFresh;
    public bool isMoving = false; 
    public MoveType moveType = MoveType.Normal;
    bool AutoBack = false;
    bool isMovingUp = false;

    Vector2 movingTouchpos;
    Vector2 movingPos;
    public void MoveUp()
    {
        isMovingUp = true;
        if (!isMoving)
        {
            MoveDataDeal();
            for (int i = 0; i < SpList.Count; i++)
            {
                if (moveType == MoveType.BoundaryUp)//边界
                {
                    SpList[i].BoundaryUp();
                    AutoBack = true;
                }
                else
                {
                    SpList[i].MoveUp();
                }
            }
        }
    }
    public void MoveDown()
    {
        isMovingUp = false;
        if (!isMoving)
        {
            MoveDataDeal();
            for (int i = 0; i < SpList.Count; i++)
            {
                if (moveType == MoveType.BoundaryDown)
                {
                    AutoBack = true;
                    SpList[i].BoundaryDown();
                }
                else
                {
                    SpList[i].MoveDown();
                }
            }
        }
    }
    public void InitMoveType()
    {
        GetMoveType();
    }
    /// <summary>
    /// 判断是否移动到边界
    /// </summary>
    bool isFutrueIcon = false;
    void GetMoveType()
    {
        for (int i = 0; i < SpList.Count; i++)
        {
            if (SpList[i].Id == 3)
            {
                if (SpList[i].GetSpriteData() != null)
                    if (SpList[i].GetSpriteData().Name.Equals("future"))
                    {
                        isFutrueIcon = true;
                    }
                    else
                    {
                        isFutrueIcon = false;
                        if (SpList[i].GetSpriteData().Name.Equals(WizardItemName.Wizard_BaiYin))
                        {
                            moveType = MoveType.BoundaryDown;
                            if (BoundaryDownAction != null)
                                BoundaryDownAction();
                        }
                         else
                            moveType = MoveType.Normal;
                    }
            }
        }
        if (isFutrueIcon)
        {
            for (int j = 0; j < SpList.Count; j++)
            {
                if (SpList[j].Id == 2)
                {
 
                    if (SpList[j].GetSpriteData() == null | SpList[j].HidingState())
                    {
                        moveType = MoveType.BoundaryUp;
                        if (BoundaryUpAction != null)
                            BoundaryUpAction();
 
                    }
                    else
                    {
                        moveType = MoveType.Normal;
                    }
                }
            }
            if (moveType == MoveType.Normal)
            {
                for (int m = 0; m < SpList.Count; m++)
                    if (SpList[m].Id == 4)
                    {
                        //Debug.LogError("id=4");
                        if (SpList[m].GetSpriteData() == null | SpList[m].HidingState())
                        {
                            moveType = MoveType.BoundaryDown;
                            if (BoundaryDownAction != null)
                                BoundaryDownAction();
                           // Debug.LogError("BoundaryDown");
                        }
                        else
                        {
                            moveType = MoveType.Normal;
                        }
                    }
            }
        }
        // moveType = MoveType.Normal;
    }
    void MoveDataDeal()
    {       
        Spwindow.PlaySmallClipSound("public_xwkyx_090");
        if(reFresh!=null)
        reFresh.RefreshArrow(false);
        GetMoveType();
        SpriteIconMgr.Instance.Invoke("SetMoving", 0.35f);
        AutoBack = false;
        isMoving = true;
    }
    public void ResetAutoBack()
    {
        AutoBack = false;
    }
    public void MoveAutoBack()
    {
        if (!AutoBack)
            return;
 
        for (int i = 0; i < SpList.Count; i++)
        {
            if (moveType != MoveType.Normal)
                SpList[i].MoveAutoBack();
        }
    }
    public void SetMoving()
    {
        isMoving = false;
    }

    public void Update()
    {
        Update_DetectCameraMove();
    }
    void Update_DetectCameraMove()
    {
        if (!DirectorMgr.DirectorAllState)
            return;
        Vector2 deltaPos = Vector2.zero;
        if (TouchManager.Instance.IsTouchValid(0))
        {
            TouchPhaseEnum phase = (TouchPhaseEnum)TouchManager.Instance.GetTouchPhase(0);
            if (phase == TouchPhaseEnum.BEGAN)
            {

            }
            else if (phase == TouchPhaseEnum.MOVED)
            {
                TouchManager.Instance.GetTouchDeltaPos(0, out movingTouchpos);
                TouchManager.Instance.GetTouchPos(0, out movingPos);
                {
                    if (movingPos.x <= 400)
                    {
                        if (movingTouchpos.y > 10)
                        {
                            MoveUp();
                        }
                        if (movingTouchpos.y < -10)
                        {
                            MoveDown();
                        }
                    }
                    else
                    {
                        if (movingTouchpos.x > 2)
                        {
                            SpriteIconMgr.Instance.RotateMesh(false);
                        }
                        if (movingTouchpos.x < -2)
                        {
                            SpriteIconMgr.Instance.RotateMesh(true);
                        }
                    }

                }
            }
            else if (phase == TouchPhaseEnum.ENDED)
            {
                MoveAutoBack();
            }
        }
    }
}
