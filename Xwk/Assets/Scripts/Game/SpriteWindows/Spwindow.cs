using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SpLockData
{
    public static bool LockSprite=false;
    public static bool LockHulu = false;
    public static bool LockXln = false;
}
 
public class MeshData
{
    public MeshData(string meshJiatiName, string meshRealName)
    {
        MeshJiatiName = meshJiatiName;
        MeshRealName = meshRealName;
    }
    public void InitMeshInfo(string jtRtmc,string rlRtmc)
    {
        MeshJiatiRtmc = jtRtmc;
        MeshRealRtmc = rlRtmc;      
    }
    public void InitMeshInfo(SpriteTransform jiati, SpriteTransform real)
    {
        MeshJiati = jiati;
        MeshReal = real;
    }
  
    public void InitMeshInfo(string titleName)
    {
        meshTitleName = titleName;      
    }
    public string MeshJiatiName;
    public string MeshRealName;

    public string MeshJiatiRtmc;//控制器
    public string MeshRealRtmc;//控制器

    public string meshTitleName;//模型标题名字

    public SpriteTransform MeshJiati;
    public SpriteTransform MeshReal; //有时间优化

    public string lockingRoleMat;
    public string NormalRoleMat;
    public GameObject GetMesh(bool isReal)
    {       
        if (isReal)
        {
            GameObject obj = GameObjectTool.Instance.InitPlayer(MeshRealName, MeshRealRtmc, MeshReal.Position, MeshReal.localScale);
            obj.transform.eulerAngles = MeshReal.erlerAngles;

            return obj;
        }
        else
        {
            GameObject obj=GameObjectTool.Instance.InitPlayer(MeshJiatiName, MeshJiatiRtmc, MeshJiati.Position, MeshJiati.localScale);
            obj.transform.rotation =Quaternion.Euler(MeshJiati.erlerAngles);
            return obj;
        }
    }
    

}
/// <summary>
/// 角色升级图片数据管理类
/// </summary>
public class RoleLevelData
{   
    public bool NeedUpRole = false;
    public string level1Path;
    public string level2Path;
    public string level3Path;
    public string level4Path;
    public RoleLevelData(string s1, string s2, string s3, string s4)
    {
        level1Path = s1;
        level2Path = s2;
        level3Path = s3;
        level4Path = s4;
    }
}
/// <summary>
/// 精灵状态管理类
/// </summary>
public class SpriteData
{
    public MeshData Meshdata;
    public RoleLevelData Roledata;
    public SpriteData(string name, bool lockState,MeshData md,RoleLevelData data=null)
    {
        lockIcon = lockState;
        Meshdata = md;
        Name = name;
        Roledata = data;
    }   
    public void InitIconName(string lockName,string lockCenterName, string releaseName,string releaseCenterName)
    {
        this.lockName = lockName;
        this.lockCenterName = lockCenterName;
        this.releaseName = releaseName;
        this.releaseCenterName = releaseCenterName;
    }
    public void Refresh()
    {
        RefreshLockState();//刷新解锁
        RefreshStarLevel();
        RefreshSpLevel();
        if (Name != "")
        {
            this.spExp = WizardData.FetchWizardItemLevelProgress(Name);
            //Debug.LogError("拉取进度:" + Name + ">>>:" + this.spExp);
            if (this.spExp >= 1.0f)
                this.spExp = 0;
        }
    }
    void RefreshLockState()
    {
        if(Name!="")
        lockIcon = WizardData.IsLockedWizardItem(Name);
    }
    void RefreshStarLevel()
    {
        if (Name != "")
            this.starLevel = WizardData.FetchWizardItemStar(Name);
    }
    void RefreshSpLevel()
    {
        if (Name != "")
        {
            this.spLvel = WizardData.FetchWizardItemLevel(Name);
            if (this.spLvel >= 3)
                this.spLvel = 3;
        }
    }
    public void InitLevel(int startLevel)
    {
        this.starLevel = startLevel;
    }
    public void SetStayCenter(bool flag)
    {
        stayCenter = flag;
    }
    public string Name="";
    public string lockName;
    public string lockCenterName;

    public string releaseName;
    public string releaseCenterName;

    public string upGradeTexName;
    public string defaultTexName;
    public string soundInfo;
    public int starLevel=0;//星星等级
    public int spLvel =0;
    public int nowRoleState = 1;//当前角色等级状态
    public float spExp = 0;//经验值

    public bool lockIcon=false;//解锁
    public bool stayCenter = false;
    public bool upgrade = false;
    public bool isNullState = false;
    public bool fullLingqi = false;
    public bool haveStory = false;
}
public class SpWindowPath : Singleton<SpWindowPath>
{
    public Color Colevel1 = new Color(195F/255, 188/255F, 182/255F, 1);
    public Color Colevel2 = new Color(0, 223/255F,255/255F, 1);
    public Color Colevel3 = new Color(1, 153/255F, 0, 1);
    public Color Colevel4 = new Color(148/255F, 26/255F, 212/255F, 1);

    public string coinPath = "game/SpriteWindow/prefab/RawImage";

    public string future = "game/SpriteWindow/mesh/public_model_jl_00000#mesh";
    public string future_jiatiRtmc = "game/SpriteWindow/animatorcontroller/future/future";
    public string public_effect_ejjm_jj01= "public/Hero_Effect/prefab/public_effect_ejjm_jj01";//变身特效


    public string ui_effect_xln_zha = "public/Hero_Effect/prefab/ui_effect_xln_zha";//点击灵气球消失     
    public string ui_public_effect_lqqcx = "public/Hero_Effect/prefab/ui_public_effect_lqqcx";// 二级界面灵气球产生出现  
    
    public string public_effect_ejjm_sj = "public/Hero_Effect/prefab/public_effect_ejjm_sj";//每次消耗灵气币是升级     

    //待机特效
    public string public_effect_ejjm_daiji01 = "public/Hero_Effect/prefab/public_effect_ejjm_daiji01";//2阶段待机特效     
    public string public_effect_ejjm_daiji02 = "public/Hero_Effect/prefab/public_effect_ejjm_daiji02";//2阶段待机特效     
    public string public_effect_ejjm_daiji03 = "public/Hero_Effect/prefab/public_effect_ejjm_daiji03";//2阶段待机特效     

    //升级特效
    public string ui_public_effect_jsz_1 = "public/Hero_Effect/prefab/ui_public_effect_jsz_1";

    //背景声音
    public string public_xwkbgm_008 = "game/SpriteWindow/sound/public_xwkbgm_008";

}

public class Spwindow : MonoBehaviour {
    public static void PlayCharacterAudio(string name, Action action = null)
    { 
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/SpriteWindow/sound/" + name,ABCommonConfig.Instance.SpWindowBundleType);
        AudioManager.Instance.PlayerSoundByClip(clip, action);
    }
    public static void PlaySmallClipSound(string name, Action action = null)
    {
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/SpriteWindow/sound/" + name, ABCommonConfig.Instance.SpWindowBundleType);
        AudioManagerExtern.Instance.PlaySmallClipSound(clip, action);
    }
    public static void PlayUserNameAudio(string name, Action action = null)
    {
        if (BabyName.c_BabyNameAudioClip == null)
            PlayCharacterAudio(name, action);
        else
            AudioManager.Instance.PlayerSoundByClip(BabyName.c_BabyNameAudioClip, () =>
            {
                PlayCharacterAudio(name, action);
            });
    }
}
public class baseDirector
{
    public string DirectorName;
    public bool Crossstep = false;
    public baseDirector(string name)
    {
        DirectorName = name;
    }
    public virtual void ShowStep()
    {
        Crossstep = false;
    }
    public virtual void CloseStep()
    {
        Crossstep = true;
    }
    public virtual void ClickDirector()//点击了指引
    {
        Crossstep = true;
    }
    public virtual void Update()
    {

    }
    public virtual bool GetCross()
    {
        return Crossstep;
    }
}
/// <summary>
/// 一阶段指引（指引精灵）--百音精灵，火眼金睛【点击五个法阵以后算完成】
/// </summary>
public class DirectorStep1 : baseDirector
{
    GameObject step1;
    float timer = 0;
    public DirectorStep1(string name, GameObject obj) : base(name)
    {
        step1 = obj;
    }
    public override void ShowStep()
    {
        base.ShowStep();
        if (step1)
            step1.SetActive(true);
        Spwindow.PlayCharacterAudio("xwk_jlej_1");//哇！这里好多精灵啊，我们快来和他们玩吧
    }

    public override void CloseStep()
    {
        base.CloseStep();        
        if(step1)
        step1.SetActive(false);      
    }

    public override void ClickDirector()
    {
        base.ClickDirector();
        Crossstep = true;
    }
    public override void Update()
    {
        base.Update();
        timer += Time.deltaTime;
        if(!Crossstep)
        {
            if (timer >= 10)
            {
                timer = 0;
                Spwindow.PlayUserNameAudio("xwk_jlej_2");
            }
        }
         
    }
    public override bool GetCross()
    {
        return base.GetCross();
    }   
}
/// <summary>
/// 2阶段指引  故事指引，点击故事按钮算通过
/// </summary>
public class DirectorStep2 : baseDirector
{
    GameObject step1;
    float timer=0;
    public DirectorStep2(string name, GameObject obj) : base(name)
    {
        step1 = obj;
    }
    public override void ShowStep()
    {
        base.ShowStep();
        DirectorMgr.Instance.StartCoroutine(waitShowStep());
    }
    IEnumerator waitShowStep()
    {
        yield return new WaitForSeconds(2.5f);
        if (step1)
        {
            step1.SetActive(true);
        }
        Spwindow.PlayCharacterAudio("xwk_jlej_3");//耶！我们成功的找到了第一个精灵啦！快跟她一起玩吧！
    }
    public override void CloseStep()
    {
        base.CloseStep();
        if (step1)
        {
            step1.SetActive(false);
        }
    }
    public override void ClickDirector()
    {
        base.ClickDirector();     
    }
    public override bool GetCross()
    {
        return base.GetCross();
    }
    public override void Update()
    {
        base.Update();
        timer += Time.deltaTime;
        if (!Crossstep)
        {
            if (timer >= 10)
            {
                timer = 0;
                Spwindow.PlayCharacterAudio("xwk_jlej_4");
            }
        }
    }
}
