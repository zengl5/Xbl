using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MeshButton : MonoBehaviour {
    public bool Interactable = true;
    Action action;
    Action action2;
    Action<GameObject> action3;
    public void AddMeshEvent(Action action,Action action2=null)
    {
        this.action = action;
        this.action2 = action2;
        if(this.GetComponent<BoxCollider>()==null)
        this.gameObject.AddComponent<BoxCollider>();
    }
    public void  AddMeshEvent(Action<GameObject> ac)
    {
        action3 = ac;
    }
    public void RemoveMeshEvent()
    {
        action = null;
        action2 = null;
        action3 = null;
    }
    public void MeshHit()
    {
        if (!Interactable)
            return;
        if (action != null)
            action();
        if (action2 != null)
            action2();     
    }
    public void MeshHit(GameObject obj)
    {
        if(action3!=null)
        action3(obj);
    }
    public void MeshHit(MonoBehaviour mono)
    {
        if (!Interactable)
            return;   
    }
   

}
