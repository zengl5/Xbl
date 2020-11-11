using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 精灵展示模型管理类
/// </summary>
public class SpMeshRefresh  {
    Dictionary<string, GameObject> MeshDic = new Dictionary<string, GameObject>();
    public GameObject NowMesh;
    SpriteData NowSpdata;
    GameObject RoleIdleEf2;
    GameObject RoleIdleEf3;
    GameObject RoleIdleEf4;
  
    public void UpgradeMeshTexture(int level)
    {
        if (level > 3)
            return;
        //Debug.LogError("修改贴图");
        Texture2D roleTex=null;
        SkinnedMeshRenderer render;
        
        if (level==0)
        {
            roleTex = ABResMgr.Instance.LoadResource<Texture2D>(NowSpdata.defaultTexName, ABCommonConfig.Instance.SpWindowBundleType, false, false);
        }
        else
        {
            level += 1;
            roleTex = ABResMgr.Instance.LoadResource<Texture2D>(NowSpdata.upGradeTexName + level.ToString(), ABCommonConfig.Instance.SpWindowBundleType, false, false);            
        }

        if (NowMesh.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>() == null)
        {
            render = NowMesh.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        }
        else
        {
            render = NowMesh.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        }
        if(Application.isEditor)
        {
            if(render.materials.Length>1)
            {
                render.materials[0].mainTexture = roleTex;
                render.materials[1].mainTexture = roleTex;
            }
            else
            {
                render.material.mainTexture = roleTex;
            }
        }
        else
        {
            if (render.materials.Length > 1)
            {
                render.sharedMaterials[0].mainTexture = roleTex;
                render.sharedMaterials[1].mainTexture = roleTex;
            }
            else
            {
                render.sharedMaterial.mainTexture = roleTex;
            }
            //render.sharedMaterial.mainTexture = roleTex;
        }
    }

    public void RefreshIdleEffect(int roleState)//刷新待机特效
    {
         switch(roleState)
        {
            case 0:
                CloseAllEffect();
                break;

            case 1:
                CloseAllEffect();
                if (RoleIdleEf2 == null)
                {
                    RoleIdleEf2 = CreateIdleEffect(SpWindowPath.Instance.public_effect_ejjm_daiji01);
                }
                else
                {
                     RoleIdleEf2.SetActive(true);
                }
                break;
            case 2:
                CloseAllEffect();

                if (RoleIdleEf3 == null)
                {
                    RoleIdleEf3 = CreateIdleEffect(SpWindowPath.Instance.public_effect_ejjm_daiji02);
                }
                else
                {
                     RoleIdleEf3.SetActive(true);
                }
                break;
            case 3:
                CloseAllEffect();

                if (RoleIdleEf4 == null)
                {
                    RoleIdleEf4 = CreateIdleEffect(SpWindowPath.Instance.public_effect_ejjm_daiji03);
                }
                else
                {
                     RoleIdleEf4.SetActive(true);
                }
                break;
        }
    }

    void CloseAllEffect()
    {
        if (RoleIdleEf2)
            RoleIdleEf2.SetActive(false);
        if (RoleIdleEf3)
            RoleIdleEf3.SetActive(false);
        if (RoleIdleEf4)
            RoleIdleEf4.SetActive(false);
    }

    GameObject CreateIdleEffect(string path)
    {
        GameObject roleEf = ABResMgr.Instance.LoadResource<GameObject>(path, ABCommonConfig.Instance.SpWindowBundleType, true, false);
        roleEf.transform.position = new Vector3(0, -2.6f, 0);
        return roleEf;
    }

    public void RefreshModel(SpriteData sp)
    {
        if (sp == null)
            return;
        if (sp.Meshdata == null)
            return;
        NowSpdata = sp;
        if (sp.lockIcon)//解锁
        {
            if (MeshDic.ContainsKey(sp.Meshdata.MeshRealName))
            {
                NowMesh = MeshDic[sp.Meshdata.MeshRealName].gameObject;
                NowMesh.SetActive(true);
                if (sp.Name.Equals(WizardItemName.Wizard_jisuanji))//计算机时双材质
                {
                    SetSpMultNormalMat(NowMesh, sp);
                }
                else
                {
                    SetMat(NowMesh, sp);
                }
            }
            else
            {
                GameObject obj = sp.Meshdata.GetMesh(true);
                if (sp.Name.Equals(WizardItemName.Wizard_jisuanji))
                {
                    SetSpMultNormalMat(obj, sp);
                }
                else
                {
                    SetMat(obj, sp);
                }
                foreach (Transform tran in obj.GetComponentsInChildren<Transform>())
                {
                    tran.gameObject.layer = LayerMask.NameToLayer("Jingling");
                }
                NowMesh = obj;
                if (!MeshDic.ContainsKey(sp.Meshdata.MeshRealName))
                    MeshDic.Add(sp.Meshdata.MeshRealName, obj);
                AddBlindScript(sp, obj);
            }
        }
        else
        {
            if (MeshDic.ContainsKey(sp.Meshdata.MeshJiatiName))
            {
                NowMesh = MeshDic[sp.Meshdata.MeshJiatiName].gameObject;
                NowMesh.SetActive(true);
                if(sp.Name.Equals(WizardItemName.Wizard_jisuanji))
                {
                    SetSpMultGrayMat(NowMesh, sp);
                }
                else
                {
                    SetMat(NowMesh, sp);
                }
            }
            else
            {
                GameObject obj = sp.Meshdata.GetMesh(false);
                if (sp.Name.Equals(WizardItemName.Wizard_jisuanji))
                {
                    SetSpMultGrayMat(obj, sp);
                }
                else
                {
                    SetMat(obj, sp);
                }
                foreach (Transform tran in obj.GetComponentsInChildren<Transform>())
                {
                    tran.gameObject.layer = LayerMask.NameToLayer("Jingling");
                }
                if (!MeshDic.ContainsKey(sp.Meshdata.MeshJiatiName))
                    MeshDic.Add(sp.Meshdata.MeshJiatiName, obj);
                NowMesh = obj;
                AddBlindScript(sp, obj);
                // Debug.LogError(sp.Meshdata.MeshJiatiName);
            }
        }
    }
    

    public void AddBlindScript(SpriteData sp, GameObject target)
    {
        if (sp.Name.Equals(WizardItemName.Wizard_BaiYin))
        {
            target.AddComponent<Spriteinfo>();
        }
        else if (sp.Name.Equals(WizardItemName.Wizard_Hulu))
        {
            target.AddComponent<Huluinfo>();
        }
        else if (sp.Name.Equals(WizardItemName.Wizard_Xln))
        {
            target.AddComponent<Xiaolninfo>();
        }
        else if (sp.Name.Equals(WizardItemName.Wizard_lingwa))//灵娃
        {
            target.AddComponent <Lingwa>();
        }
        else if (sp.Name.Equals(WizardItemName.Wizard_xcww))//
        {
            target.AddComponent<Xcwawa>();
        }
        else if (sp.Name.Equals(WizardItemName.Wizard_jl_hxjl))//
        {
            target.AddComponent<Hxjingling>();
        }/////////////////
        else if (sp.Name.Equals(WizardItemName.Wizard_hongbao))//
        {
            target.AddComponent<jl_hongbao>();
        }
        else if (sp.Name.Equals(WizardItemName.Wizard_nianshou))
        {
            target.AddComponent<jl_nsbaobao>();
        }
        else if (sp.Name.Equals(WizardItemName.Wizard_dazuishou))
        {
            target.AddComponent<jl_dazuishou>();
        }
        else if (sp.Name.Equals(WizardItemName.Wizard_menmenmiao))//
        {
            target.AddComponent<jl_menmenmiao>();
        }
        else if (sp.Name.Equals(WizardItemName.Wizard_jisuanji))//
        {
            target.AddComponent<jl_jisuanji>();
        }
        else if (sp.Name.Equals(WizardItemName.Wizard_huohuolong))//
        {
            target.AddComponent<jl_huohuolong>();
        }
        else if (sp.Name.Equals(WizardItemName.Wizard_xixilang))//
        {
            target.AddComponent<jl_xixilang>();
        }
        else if (sp.Name.Equals(WizardItemName.Wizard_xiuxiuwoniu))//咻咻蜗牛
        {
            target.AddComponent<jl_xiuxiuwoniu>();
        }
        else if (sp.Name.Equals(WizardItemName.Wizard_niuxiaoxian))//牛小仙
        {
            target.AddComponent<jl_niuxiaoxian>();
        }
        else if (sp.Name.Equals("future"))
        {
            target.AddComponent<Futureinfo>();
        }
    }

    public void RefreshRtmc(SpriteData data, GameObject target)
    {
        if (target == null)
            return;
        baseRoleInfo info = target.GetComponent<baseRoleInfo>();
        if (info != null)
            info.RefreshRtmc(data);
    }

    public void RefreshRoleSound(SpriteData sp)
    {
        if (sp == null)
            return;
        if (!sp.lockIcon)
            return;
        Spwindow.PlayCharacterAudio(sp.soundInfo);
    }

    public void RefreshMeshTitle(RawImage TitleImage,SpriteData sp)//更新精灵名字
    {
        if (sp == null)
            return;
        if (sp.Name.Equals("future"))
        {
            TitleImage.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            TitleImage.transform.parent.gameObject.SetActive(true);
            string baseTitlePath = "game/SpriteWindow/ui/role/";
            Texture2D tex = ABResMgr.Instance.LoadResource<Texture2D>(baseTitlePath + sp.Meshdata.meshTitleName, "window");
            TitleImage.texture = tex;
        }
    }

    public void RemoveAllMesh()
    {
        CloseAllEffect();
        foreach (GameObject obj in MeshDic.Values)
            GameObject.Destroy(obj);
        MeshDic.Clear();
    }
    
    public void RefreshAnim()
    {
        NowMesh.GetComponent<baseRoleInfo>().Refresh();
    }

    public void RotateMesh(bool right)//旋转模型
    {
        if (NowMesh)
        {
            if (right)
                NowMesh.transform.eulerAngles = new Vector3(NowMesh.transform.eulerAngles.x, NowMesh.transform.eulerAngles.y + 8, 0);
            else
                NowMesh.transform.eulerAngles = new Vector3(NowMesh.transform.eulerAngles.x, NowMesh.transform.eulerAngles.y - 8, 0);
        }
    }

    public void ResetEulerAngle(SpriteData sp)
    {
        if (sp == null)
            return;
        if (sp.lockIcon)
        {
            if (NowMesh)
                NowMesh.transform.rotation = Quaternion.Euler(sp.Meshdata.MeshReal.erlerAngles);
        }
        else
        {
            if (NowMesh)
                NowMesh.transform.rotation = Quaternion.Euler(sp.Meshdata.MeshJiati.erlerAngles);
        }


    }

    public void CloseBeforeMesh(SpriteData sp)//关闭之前模型
    {
        if (NowMesh)
            NowMesh.SetActive(false);
    }

    public void RefreshArrow(bool flag)//中间箭头
    {
        SpritBtn.Instance.OpenArrowImage(flag);
    }
  
    void SetMat(GameObject jl_jiati,SpriteData sp)
    {
        SkinnedMeshRenderer render;
        Material roleMat;
        if (jl_jiati.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>() != null)
        {
            render = jl_jiati.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        }
        else
        {
            render = jl_jiati.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        }
        if(sp.lockIcon)
        {
            roleMat = ABResMgr.Instance.LoadResource<Material>(sp.Meshdata.NormalRoleMat, ABCommonConfig.Instance.SpWindowBundleType, false, false);
        }
        else
        {
            roleMat = ABResMgr.Instance.LoadResource<Material>(sp.Meshdata.lockingRoleMat, ABCommonConfig.Instance.SpWindowBundleType, false, false);
        }
        if (roleMat != null)
            render.material = roleMat;
    }
    void SetSpMultGrayMat(GameObject jl_jiati, SpriteData sp)//多个材质球 计算鸡
    {
        //Debug.LogError("计算鸡");
        SkinnedMeshRenderer render;
        if (jl_jiati.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>() != null)
        {
            render = jl_jiati.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        }
        else
        {
            render = jl_jiati.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        }
        Material roleMat = ABResMgr.Instance.LoadResource<Material>(sp.Meshdata.lockingRoleMat, ABCommonConfig.Instance.SpWindowBundleType, false, false);
        Material[] matArray = new Material[] { roleMat, roleMat };
        render.materials = matArray;                 
    }
    void SetSpMultNormalMat(GameObject jl_jiati, SpriteData sp)//多个材质球 计算鸡
    {
        SkinnedMeshRenderer render;
        if (jl_jiati.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>() != null)
        {
            render = jl_jiati.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        }
        else
        {
            render = jl_jiati.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        }
        Material roleMat1 = ABResMgr.Instance.LoadResource<Material>(sp.Meshdata.NormalRoleMat, ABCommonConfig.Instance.SpWindowBundleType, false, false);
        Material roleMat2 = ABResMgr.Instance.LoadResource<Material>(sp.Meshdata.NormalRoleMat+" 1", ABCommonConfig.Instance.SpWindowBundleType, false, false);

        Material[] matArray = new Material[] { roleMat1, roleMat2 };
        render.materials = matArray;
    }

}
