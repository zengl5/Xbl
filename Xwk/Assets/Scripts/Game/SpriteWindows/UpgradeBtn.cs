using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using DG.Tweening;
using XWK.Common.UI_Reward;
using YB.XWK.MainScene;
/// <summary>
/// 升级按钮  点击管理类
/// </summary>
public class UpgradeBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    List<GameObject> Eflist = new List<GameObject>();
    public Action<float> LongpressStayingEvent;
    public Action LongpressExitEvent;
    public Action PointOnclickEvent;
    bool isDown = false;
    public float timer = 0;
    float closeTimer = 0;
    Button button;
    GameObject UpgradeEf;
    bool isPlayingUpEf = false;
    void Awake()
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(OnclickEvent);
    }
    void Start()
    {
        SpriteIconMgr.Instance.spriteSwithAction += AutoExit;
    }    
    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
     }
    public void ResetUpData()
    {
        timer = 0;
    }
    public void StopUpgrade()
    {
        isDown = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isDown = false;
        if (LongpressExitEvent != null)
            LongpressExitEvent();
    }
    /// <summary>
    /// 切换精灵，自动设置离开按钮
    /// </summary>
    void AutoExit()
    {
        isDown = false;
        if (LongpressExitEvent != null)
            LongpressExitEvent();
    }
    void OnclickEvent()
    {
        if (PointOnclickEvent != null)
            PointOnclickEvent();
    }

    void UpdateCloseUpLoadingUI()//自动关闭升级UI  中心进度条
    {
        if(!isDown)
        {
            closeTimer += Time.deltaTime;
            if(closeTimer>2.0f)
            {
                //关闭升级UI进度条
                SpriteUplevelMgr.Instance.CloseUpUI_Center();
            }
        }
        else
        {
            closeTimer = 0;
        }
    }

    void Update()
    {
        if (isDown)//按下效果
        {         
            if (timer >= 0.15F)
            {
                timer = 0;
                if (LongpressStayingEvent != null)
                    LongpressStayingEvent(0);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        UpdateCloseUpLoadingUI();
    }
    
}

 
