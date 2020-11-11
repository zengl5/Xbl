using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Assets.Scripts.C_Framework;

public class ReadyState :IRubbish,C_IState
{
    //分身位置
    Camera MainCamera;
    Dictionary<string, Vector3> PosDic = new Dictionary<string, Vector3>();
    Animator Baseanimator;
    List<GameObject> WkChildList = new List<GameObject>();
    List<GameObject> EffectList = new List<GameObject>();//回收特效
    Action Finishaction;
    string WukongbianbianbianAnim= "wukong_bianbianbian@anim";//变变变动画
    string WukongMainIdle = "wukong_fenshen_idle00@anim";//领头悟空Idle
    string WukonghuanchongAnim = "jgb_fenshen_idle_huanchong@anim";//Idle变变变【小悟空9个】
    public string Name { get; set; }
    public virtual void OnStateEnter()
    {
        Name = "ReadyState";
        MainCamera = GameObject.Find("SeparationCamera").GetComponent<Camera>();
        if (MainCamera)
        {
            WindowSliderControl.Instance.InitCharacter(MainCamera);
            WindowSliderControl.Instance.DFrozenCamera();
        }          
        CreatMainXwk(()=> SeparationManager.Instance.GotoState("GameState"));//创建小悟空
    }

    public virtual void OnStateLeave()
    {
        RecycleRubbish();
    }
    public virtual void OnStateOverride()
    {

    }
    public virtual void OnStateResume()
    {

    }
    public void RecycleRubbish()
    {
        C_TimerMgr.Instance.RemoveTimer("CreatChildXwk");
        DestoryLeavingXwk();
        if (Baseanimator)
        {
            if (Baseanimator.transform.gameObject)
                GameObject.Destroy(Baseanimator.transform.gameObject);//销毁领头小悟空
        }
        for (int i = 0; i < EffectList.Count; i++)
        {
            GameObject.Destroy(EffectList[i].gameObject);
        }
        for (int i = 0; i < WkChildList.Count; i++)
        {
            GameObject.Destroy(WkChildList[i].gameObject);
        }
        WkChildList = new List<GameObject>();
        EffectList = new List<GameObject>();
        PosDic = new Dictionary<string, Vector3>();    
        SeparationManager.Instance.StopAllCoroutines();
    }
    /// <summary>
    /// 创建领头的小悟空
    /// </summary>
    /// <param name="action"></param>
    public void CreatMainXwk(Action action)
    {
        Finishaction = action;
        GameObject objgame = ABResMgr.Instance.LoadResource<GameObject>(SepPath.Instance.Xwk, "separation", true,true);
        objgame.SetActive(true);
        objgame.transform.localScale = 0.01F*Vector3.one;
        objgame.transform.localPosition = Vector3.zero;
        objgame.transform.localEulerAngles = Vector3.zero;

        PlayEffect(SepPath.Instance.Effect_fss_sf, objgame.transform);//落地特效
        RuntimeAnimatorController anim = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(SepPath.Instance.MainXwk_RuntimeAnimatorController, "separation", false);
        Baseanimator = objgame.GetComponent<Animator>();
        Baseanimator.runtimeAnimatorController = anim;
        Baseanimator.Play(WukongbianbianbianAnim);
        //播放妈咪妈咪变语音
        AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>("game/Separation/Sound/common_75", "separation");
        AudioManager.Instance.PlaySound(clip);

        AudioClip clipef = ABResMgr.Instance.LoadResource<AudioClip>("game/Separation/SoundEffect/public_xwkyx_001", "separation");
        AudioManager.Instance.PlayerSoundByClip(clipef, null);

        SeparationManager.Instance.StartCoroutine(CameraControl());
        SeparationManager.Instance.StartCoroutine(CreatChildXwk());      

    }
    IEnumerator CameraControl()
    {
        yield return null;
        GameObject cameraobj = ABResMgr.Instance.LoadResource<GameObject>(SepPath.Instance.CameraAnim, "separation", true);
        cameraobj.transform.GetChild(0).GetComponent<Camera>().nearClipPlane = 0.01f;
        RuntimeAnimatorController rtac = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(SepPath.Instance.CameraAnim, "separation", false);
        cameraobj.GetComponent<Animator>().runtimeAnimatorController = rtac;
        cameraobj.transform.localScale = 0.01f * Vector3.one;
        GameObject.Destroy(cameraobj, 3);      
    }
    //创建另外9个小悟空
    WaitForSeconds waitcc = new WaitForSeconds(2.5f);
    IEnumerator CreatChildXwk()
    {
        yield return waitcc;
        Baseanimator.Play(WukongMainIdle);
        List<string> pathList = new List<string>();
        TextAsset tx = Resources.Load("Config/Separation/BeforeSeparation") as TextAsset;
        string[] objInfoArray = tx.text.Split('\n'); // 以\n为分割符将文本分割为一个数组
        for (int i = 0; i < objInfoArray.Length; i++)
        {
            if (objInfoArray[i] != "")
                pathList.Add(objInfoArray[i]);
        }
        PosDic = new Dictionary<string, Vector3>();
        for (int i = 0; i < pathList.Count; i++)
        {
            string[] ss;
            ss = pathList[i].Split(',');//解析wukong @mesh(5),-1.747,0,-2.52
            Vector3 pos = new Vector3(Convert.ToSingle(ss[1]), Convert.ToSingle(ss[2]), Convert.ToSingle(ss[3]));
            PosDic.Add(ss[0], pos);
        }
        SeparationManager.Instance.StartCoroutine(InitModel_Xwk(PosDic));
    }
    WaitForSeconds wait = new WaitForSeconds(0.5f);
    IEnumerator InitModel_Xwk(Dictionary<string, Vector3> PDic)
    {
        yield return wait;
        for (int i = 0; i < PDic.Count; i++)
        {
            GameObject objgame = ABResMgr.Instance.LoadResource<GameObject>(SepPath.Instance.Xwk, "separation", false,true);
            objgame.transform.localScale = 0.01f * Vector3.one;
            string index = "wukong@mesh" + " " + "(" + (i + 1).ToString() + ")";
            Vector3 pos = Vector3.zero;
            foreach (string s in PDic.Keys)
            {
                if (s.Contains(index))
                    pos = PDic[index];//解析wukong@mesh (1)
            }
            GameObject target = GameObject.Instantiate(objgame, pos, Quaternion.identity);
            target.SetActive(true);
            RuntimeAnimatorController anim = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(SepPath.Instance.Scene9Sep_RuntimeAnimatorController, "separation", false);
            Animator animChild = target.GetComponent<Animator>();
            animChild.runtimeAnimatorController = anim;
            animChild.Play(WukonghuanchongAnim);
            WkChildList.Add(target);
            PlayEffect(SepPath.Instance.Effect_fss_chuxian, target.transform);
            //第一排0.1 
            //第二排0.05 （3-5）
            //第三排0.025 （6-8）        
            if (i == 5)
                yield return new WaitForSeconds(0.4f);       
            else if (i == 2)            
                yield return new WaitForSeconds(0.3f);
            else
                yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1.5f);
        AudioClip clipef = ABResMgr.Instance.LoadResource<AudioClip>("game/Separation/Sound/common_95_1", "separation");//散
        AudioManager.Instance.PlaySound(clipef);
        yield return new WaitForSeconds(0.3f);
        SeparationManager.Instance.StartCoroutine(PlayLeavingAnimation());
      
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < EffectList.Count; i++)
        {
            GameObject.Destroy(EffectList[i].gameObject);
        }
        yield return new WaitForSeconds(1f);
        DestoryLeavingXwk();//销毁飞出去的动画            
        if (Finishaction != null)
            Finishaction();
     }
    /// <summary>
    /// 播放离开动画
    /// </summary>
    IEnumerator PlayLeavingAnimation()
    {
        AudioClip clipef = ABResMgr.Instance.LoadResource<AudioClip>("game/Separation/SoundEffect/public_xwkyx_004", "separation");
        AudioManager.Instance.PlaySound(clipef);
        if (Baseanimator)
        {
            Baseanimator.Play("wukong_fenshen_out_up@anim");//领头小悟空播放向上动画
        }
        PlayEffect(SepPath.Instance.Effect_fss_xiaoshi, Baseanimator.transform);//落地特效

        string[] allLeaveAnimList = new string[] { "wukong_fenshen_out_up@anim", "wukong_fenshen_out_rightup@anim",
            "wukong_fenshen_out_right@anim", "wukong_fenshen_out_leftup@anim", "wukong_fenshen_out_left@anim" };
        for (int i = 0; i < WkChildList.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, allLeaveAnimList.Length);
            WkChildList[i].GetComponent<Animator>().Play(allLeaveAnimList[randomIndex]);
            PlayEffect(SepPath.Instance.Effect_fss_xiaoshi, WkChildList[i].transform);//落地特效
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.1f));
        }       
    }
    void DestoryLeavingXwk()
    {
        for (int i = 0; i < WkChildList.Count; i++)
        {
            GameObject.Destroy(WkChildList[i].gameObject);
        }
    }
    void PlayEffect(string path, Transform parent)
    {
        GameObject effect = ABResMgr.Instance.LoadResource<GameObject>(path,ABCommonConfig.EfBundleType, true);
        effect.transform.position = parent.position;
        EffectList.Add(effect);
    }     
}
