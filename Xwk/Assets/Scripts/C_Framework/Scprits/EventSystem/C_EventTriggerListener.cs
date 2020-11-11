using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class C_EventTriggerListener : EventTrigger
{
    public delegate void VoidDelegate(GameObject go);

    public VoidDelegate onClick;

    public VoidDelegate onSelect;

    public VoidDelegate onDeSelect;

    public VoidDelegate onPointDown;

    public VoidDelegate onPointUp;

    public VoidDelegate onDrag;

    public static C_EventTriggerListener Get(GameObject go)
    {
        C_EventTriggerListener listener = go.GetComponent<C_EventTriggerListener>();
        if (listener == null)
            listener = go.AddComponent<C_EventTriggerListener>();

        return listener;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (onClick != null)
            onClick(gameObject);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        if (onSelect != null)
            onSelect(gameObject);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);

        if (onDeSelect != null)
            onDeSelect(gameObject);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (onPointDown != null)
            onPointDown(gameObject);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if (onPointUp != null)
            onPointUp(gameObject);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        if (onDrag != null)
            onDrag(gameObject);
    }
}
