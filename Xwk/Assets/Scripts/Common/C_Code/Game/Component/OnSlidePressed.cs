using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using UnityEngine.EventSystems;
using System;

public class OnSlidePressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IDragHandler
{
    [SerializeField]
    private float m_X = 0;
    [SerializeField]
    private float m_Y = 0;

    private Action m_onButtonAction = null;

    private Vector2 m_CurPos = Vector2.zero;

    private bool m_bTouch = false;

    public void Init(Action onButtonAction)
    {
        m_onButtonAction = onButtonAction;
    }

    // 当按钮被按下后系统自动调用此方法  
    public void OnPointerDown(PointerEventData eventData)
    {
        m_CurPos = eventData.position;

        m_bTouch = true;
    }

    // 当按钮抬起的时候自动调用此方法  
    public void OnPointerUp(PointerEventData eventData)
    {
        Parse(eventData.position);
    }

    // 当鼠标从按钮上离开的时候自动调用此方法  
    public void OnPointerExit(PointerEventData eventData)
    {
        Parse(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        ParseDrag(eventData.position);
    }

    private void Parse(Vector2 pos)
    {
        if (m_bTouch)
        {
            m_bTouch = false;

            if (Math.Abs(m_CurPos.x - pos.x) >= m_X && Math.Abs(m_CurPos.y - pos.y) >= m_Y)
            {
                if (m_onButtonAction != null)
                    m_onButtonAction();
            }
        }
    }

    private void ParseDrag(Vector2 pos)
    {
        if (m_bTouch)
        {
            if (Math.Abs(m_CurPos.x - pos.x) >= m_X && Math.Abs(m_CurPos.y - pos.y) >= m_Y)
            {
                m_bTouch = false;

                if (m_onButtonAction != null)
                    m_onButtonAction();
            }
        }
    }
}
