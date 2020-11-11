using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Assets.Scripts.C_Framework;

public class GameState : IRubbish, C_IState
{
    public List<string> AnimatorNameList = new List<string>();//动画名字
    public List<Vector3> PosList = new List<Vector3>();//位置信息
    public List<Vector3> RotationList = new List<Vector3>();//角度信息
    public Transform SwkParent;
    List<Sunwukong> SunwukongList=new List<Sunwukong>();
    bool SetRight = false;
    int SavePointCount = 10;//场景保留随机点个数
    List<GameObject> EffectList = new List<GameObject>();//回收特效
    public string Name { get; set; }
    public virtual void OnStateEnter()
    {
        Name = "GameState";
        ReadData();
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
        for (int i = 0; i < EffectList.Count; i++)
        {
            GameObject.Destroy(EffectList[i].gameObject);
        }
        AnimatorNameList = new List<string>();
        PosList = new List<Vector3>();
        RotationList = new List<Vector3>();
        SunwukongList = new List<Sunwukong>();
        SeparationManager.Instance.StopAllCoroutines();
    }
    void ReadData()
    {
        //读取动画信息
        List<string> pathList = new List<string>();
        TextAsset tx = Resources.Load("Config/Separation/AfterSeparationInfo") as TextAsset;
        //Debug.LogError(tx);
        string[] objInfoArray = tx.text.Split('\n'); // 以\n为分割符将文本分割为一个数组
        for (int i = 0; i < objInfoArray.Length; i++)
        {
            if (objInfoArray[i] != "")
                pathList.Add(objInfoArray[i]);
        }
        IdleAnimationClipList(pathList);
    }
    //解析文本存储的信息
    void IdleAnimationClipList(List<string> obj)
    {
        int totalCount = obj.Count;
        //随机选择10个点的数据 20 5
        for (int i = 0; i < totalCount - SavePointCount; i++)
        {
            int randomKey = UnityEngine.Random.Range(0, obj.Count);
            obj.RemoveAt(randomKey);
        }
        AnimatorNameList = new List<string>();
        PosList = new List<Vector3>();
        RotationList = new List<Vector3>();
        for (int i = 0; i < obj.Count; i++)
        {
            string[] s2 = obj[i].Split(':');
            AnimatorNameList.Add(s2[0]);//存储名字   wukong_fenshen_idle13@anim:-15.274,0.79289,-4.10461:0,2.000005,0
            string[] s3 = s2[1].Split(',');//解析位置信息
            string[] s4 = s2[2].Split(',');//解析旋转信息         
            PosList.Add(new Vector3(Convert.ToSingle(s3[0]), Convert.ToSingle(s3[1]), Convert.ToSingle(s3[2])));
            RotationList.Add(new Vector3(Convert.ToSingle(s4[0]), Convert.ToSingle(s4[1]), Convert.ToSingle(s4[2])));
        }
    
        SeparationManager.Instance.StartCoroutine(InitSunWukong());
    }
    WaitForSeconds wait = new WaitForSeconds(0.1f);
    IEnumerator InitSunWukong()
    {
        SetRight = false;
        int randomIndex = 0;
        SunwukongList = new List<Sunwukong>();
        for (int i = 0; i <PosList.Count; i++)
        {
            GameObject effect = ABResMgr.Instance.LoadResource<GameObject>(SepPath.Instance.Effect_wkcx, ABCommonConfig.EfBundleType, true);
            EffectList.Add(effect);
            effect.transform.position = PosList[i];
            GameObject obj  = ABResMgr.Instance.LoadResource<GameObject>(SepPath.Instance.Xwk, "separation",false,true);
            GameObject swk = GameObject.Instantiate(obj, PosList[i], Quaternion.identity);
            swk.SetActive(true);
            EffectList.Add(swk);//内存管理
            RuntimeAnimatorController anim = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(SepPath.Instance.xwk_RuntimeAnimatorController, "separation", false);
            swk.GetComponent<Animator>().runtimeAnimatorController = anim;
            swk.name = AnimatorNameList[i];
            Sunwukong wukong = swk.AddComponent<Sunwukong>();
            AddJGB(swk.transform.GetChild(0).transform, AnimatorNameList[i]);
            wukong.InitAnimator(AnimatorNameList[i]);
            wukong.InitEvent(SeparationManager.Instance.FindSunWukong, SeparationManager.Instance.FindErrorSunWukong);
            SunwukongList.Add(wukong);
            wukong.InitXwk(RotationList[i]);//[i=0,name=1]
            //21 22 23 号小悟空需要添加金箍棒
             //初始化事件[找到真的孙悟空]
            if (!SetRight)
            {
                randomIndex = UnityEngine.Random.Range(0, PosList.Count - 1);
                SetRight = true;
            }
            if (i == randomIndex)
            {
                wukong.SetRight();
            }
            AudioClip clipef = ABResMgr.Instance.LoadResource<AudioClip>("game/Separation/SoundEffect/public_xwkyx_005", "separation");
            AudioManager.Instance.PlaySound(clipef);
            yield return wait;
        }
        WindowSliderControl.Instance.ReleaseCamera();
    }
    //添加金箍棒 21 22 23号小悟空
    void AddJGB(Transform parent,string name)
    {
        int id = 0;
        if (name.Contains("21"))
        {
            id = 21;
        }
        else if (name.Contains("22"))
        {
            id = 22;
        }
        else if (name.Contains("23"))
        {
            id = 23;
        }
        if(id!=0)
        {
            string animId = "jgb_fenshen_idle";//21@anim";
            GameObject obj = ABResMgr.Instance.LoadResource<GameObject>(SepPath.Instance.Jgb, "separation", true,true);
            if (parent.transform.parent.GetComponent<Sunwukong>() != null)
            {
                parent.transform.parent.GetComponent<Sunwukong>().SetJGB(obj);//把金箍棒依赖到小悟空类中
            }
            obj.transform.parent = parent;
            obj.transform.localPosition =  Vector3.zero;
            obj.transform.localScale = 1f * Vector3.one;
             
            RuntimeAnimatorController anim = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(SepPath.Instance.Jgb_RuntimeAnimatorController, "separation", false);
            obj.GetComponent<Animator>().runtimeAnimatorController = anim;
            obj.GetComponent<Animator>().Play(animId + (id).ToString() + "@anim");
        }                      
    }
    //找到真的孙悟空      
    public void FindSunwukong()
    {
        for (int i = 0; i < SunwukongList.Count; i++)
            if (SunwukongList[i]!= null)
                GameObject.Destroy(SunwukongList[i].transform.gameObject,0.1f*(i+1));
    }

}
