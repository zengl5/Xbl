using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using UnityEngine.EventSystems;
using System;

public class OnCheckPressed : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    public string ID = "";

    private Action<string> m_CheckAction = null;
    private Action<string, string> m_CollisionAction = null;

    public void Init(Action<string> checkAction, Action<string, string> collisionAction)
    {
        m_CheckAction = checkAction;
        m_CollisionAction = collisionAction;
    }

    // 当按钮被按下后系统自动调用此方法  
    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_CheckAction != null)
            m_CheckAction(ID);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_CollisionAction != null)
            m_CollisionAction(ID, collider.name);
    }
}
