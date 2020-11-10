using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using UnityEngine.EventSystems;
using System;

public class OnButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    // 延迟时间  
    private float m_fDelay = 0.2f;

    // 按钮是否是按下状态  
    private bool m_bIsDown = false;
    public bool IsDown
    {
        get { return m_bIsDown; }

        set
        {
            if (m_bIsDown != value)
            {
                m_bIsDown = value;

                if (m_DownChangeAction != null)
                    m_DownChangeAction(m_bIsDown);
            }
        }
    }

    private Action<bool> m_DownChangeAction = null;

    private Action m_onButtonAction = null;

    // 按钮最后一次是被按住状态时候的时间  
    private float m_fLastIsDownTime = 0;
    
    void Update()
    {
        // 如果按钮是被按下状态  
        if (m_bIsDown)
        {
            // 当前时间 -  按钮最后一次被按下的时间 > 延迟时间0.2秒  
            if (Time.time - m_fLastIsDownTime > m_fDelay)
            {
                // 记录按钮最后一次被按下的时间  
                m_fLastIsDownTime = Time.time;
            }
        }
    }

    public void Init(Action<bool> downChangeAction, Action onButtonAction)
    {
        m_DownChangeAction = downChangeAction;
        m_onButtonAction = onButtonAction;
    }

    // 当按钮被按下后系统自动调用此方法  
    public void OnPointerDown(PointerEventData eventData)
    {
        IsDown = true;

        m_fLastIsDownTime = Time.time;
    }

    // 当按钮抬起的时候自动调用此方法  
    public void OnPointerUp(PointerEventData eventData)
    {
        if (m_bIsDown && m_onButtonAction != null)
            m_onButtonAction();

        IsDown = false;
    }

    // 当鼠标从按钮上离开的时候自动调用此方法  
    public void OnPointerExit(PointerEventData eventData)
    {
        IsDown = false;
    }
}
