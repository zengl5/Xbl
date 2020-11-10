using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEvent : MonoBehaviour {

    public Action<Transform> m_TriggerEnter;
    public Action<Transform> m_TriggerStay;
    public Action<Transform> m_TriggerExit;

    public static ColliderEvent AddColliderEvent(Transform t, bool withRigibody = false,bool trigger =true)
    {
        if (t == null) return null;
        ColliderEvent cBase = t.GetComponent<ColliderEvent>();
        if (!cBase)
        {
            cBase = t.gameObject.AddComponent<ColliderEvent>();
        }
        AddBoxCollider(t, trigger);
        if (withRigibody)
        {
            AddRigibody(t);
        }
        return cBase;
    }
    
    public Transform AddBoxCollider(Vector3 center, Vector3 size,bool trigger = false,Transform parent = null)
    {
        GameObject Collider = new GameObject("BoxCollider");
        Collider.transform.parent = parent;
        Collider.transform.localPosition = Vector3.zero;
        Collider.transform.localRotation = Quaternion.identity;
        Collider.transform.localScale = Vector3.one;

        BoxCollider bx = Collider.AddComponent<BoxCollider>();
        bx.isTrigger = true;
        bx.center = center;
        bx.size = size;

        return Collider.transform;
    }
     public static void AddBoxCollider(Transform parent,bool trigger = true)
    {
        if (parent == null) return;
        if (parent.childCount <= 0)
        {
            if (parent.GetComponent<Collider>() == null)
            {
                parent.gameObject.AddComponent<BoxCollider>().isTrigger = trigger;
            }
            return;
        }
        Vector3 postion = parent.position;
        Quaternion rotation = parent.rotation;
        Vector3 scale = parent.localScale;
        parent.position = Vector3.zero;
        parent.rotation = Quaternion.Euler(Vector3.zero);
        parent.localScale = Vector3.one;

        Collider[] colliders = parent.GetComponentsInChildren<Collider>();
        foreach (Collider child in colliders)
        {
            UnityEngine.Object.Destroy(child);
        }
        Vector3 center = Vector3.zero;
        Renderer[] renders = parent.GetComponentsInChildren<Renderer>();
        foreach (Renderer child in renders)
        {
            center += child.bounds.center;
        }
        center /= renders.Length;
        Bounds bounds = new Bounds(center, Vector3.zero);
        foreach (Renderer child in renders)
        {
            bounds.Encapsulate(child.bounds);
        }
        BoxCollider boxCollider = parent.gameObject.AddComponent<BoxCollider>();
        boxCollider.center = bounds.center - parent.position;
        boxCollider.size = bounds.size;
        boxCollider.isTrigger = trigger;

        parent.position = postion;
        parent.rotation = rotation;
        parent.localScale = scale;
    }
    public static Rigidbody AddRigibody(Transform t, bool gravity = false)
    {
        if (!t) return null;
        Rigidbody rigidbody = t.GetComponent<Rigidbody>();
        if (rigidbody == null)
        {
            rigidbody = t.gameObject.AddComponent<Rigidbody>();
        }
        rigidbody.isKinematic = true;
        rigidbody.useGravity = gravity;
        return rigidbody;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (m_TriggerEnter != null)
        {
            m_TriggerEnter(other.transform);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (m_TriggerStay != null)
        {
            m_TriggerStay(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_TriggerExit != null)
        {
            m_TriggerExit(other.transform);
        }
    }
}
 
