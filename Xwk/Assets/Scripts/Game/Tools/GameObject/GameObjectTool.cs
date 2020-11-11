using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;

public class GameObjectTool : C_Singleton<GameObjectTool>
{

    public GameObject InitPlayer(string prefab_url, string animator_url,Vector3 pos,Vector3 scale, string bundleType="",Transform parent = null)
    {
        GameObject obj = ABResMgr.Instance.LoadResource<GameObject>(prefab_url, bundleType, true,true);
        if (parent != null)
            obj.transform.parent = parent;
        obj.transform.localScale = scale;
        obj.transform.localPosition = pos;
        obj.transform.gameObject.SetActive(true);
        if(animator_url!=null)
        {
            Animator am = obj.GetAddComponent<Animator>();
            RuntimeAnimatorController ract = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(animator_url, bundleType, false, true);
            am.runtimeAnimatorController = ract;
        }     
        return obj;
    }
    public GameObject InitPlayer(string fileurl, string racturl, float scale = 0.01f,string bundleType="")
    {
        GameObject obj = ABResMgr.Instance.LoadResource<GameObject>(fileurl, bundleType, true,true);
        obj.transform.localScale = scale * Vector3.one;
        if (racturl == null)
            return obj;
        Animator am = obj.GetComponent<Animator>();
        RuntimeAnimatorController ract = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(racturl, bundleType, false);
        am.runtimeAnimatorController = ract;
        obj.SetActive(true);
        return obj;
    }
    public Animator InitGameObject(string fileurl, string racturl,string bundleType="",bool UseInternalRes=true)
    {
        GameObject obj = ABResMgr.Instance.LoadResource<GameObject>(fileurl, bundleType, true, UseInternalRes);
        obj.transform.localScale = 0.01f * Vector3.one;
        obj.transform.gameObject.SetActive(true);
        Animator am = obj.GetComponent<Animator>();
        if (racturl == null)
            return am;
        RuntimeAnimatorController ract = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(racturl, bundleType, false);
        am.runtimeAnimatorController = ract;
        return am;
    }


   
    public void SetSpMainTexture(GameObject obj,string path)//设置角色贴图
    {
        SkinnedMeshRenderer render;
        if (obj.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>() != null)
        {
            render = obj.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        }
        else
        {
            render = obj.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        }
        Texture2D tex = ABResMgr.Instance.LoadResource<Texture2D>(path, "piercingeye", false, false);
        if(Application.isEditor)
        {
            render.material.mainTexture = tex;
        }
        else
        {
            render.sharedMaterial.mainTexture = tex;
        }
    }
  
   
    /// <summary>
    /// 实例化一个3D 的手
    /// </summary>
    /// <param name="layerName"></param>
    /// <param name="pos"></param>
    /// <param name="eulergel"></param>
    /// <param name="sacle"></param>
    /// <returns></returns>
    public GameObject GetWorldGameObject(string path, Vector3 pos, Vector3 eulergel, Vector3 sacle, string type, string layerName = null)
    {
        GameObject obj = ABResMgr.Instance.LoadResource<GameObject>(path, type, true, false);

        obj.transform.position = pos;
        obj.transform.localScale = sacle;
        obj.transform.eulerAngles = eulergel;
        if (layerName != null)
        {
            foreach (Transform tran in obj.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = LayerMask.NameToLayer(layerName);
            }
        }
        return obj; //;
    }
}
 