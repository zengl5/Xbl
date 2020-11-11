using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using DG.Tweening;
using Xbl;
using UnityEngine.UI;
using System;
public class DjSpawn
{
    public List<HuluCard> HuluList = new List<HuluCard>();
    public List<MeshButton> meshButtonList = new List<MeshButton>();
    public List<int> IdList = new List<int>();
    public DjData RightDjdate;
    public int RightDaojuId = 1;
    public int RightPosId = 0;
    GameObject DaojuParent = null;
    string basemeshName = "dj0000";
    string baseSoundName = "dj0000";
    HlGame hlGame;
    public int SetRightDaojuId()
    {
        IdList.Clear();
        for (int i = 1; i < 41; i++)
            IdList.Add(i);
        RightDaojuId = UnityEngine.Random.Range(1, 41);//随机卡片[1,40]
        IdList.Remove(RightDaojuId);
        return RightDaojuId;
    }
    public int GetRightDaojuId()
    {
        return RightDaojuId;
    }
    public void InitData(HlGame game)
    {      
        hlGame = game;
        meshButtonList.Clear();
        RightPosId = UnityEngine.Random.Range(0, 3);
        HuluManager.Instance.PlayEffectAudio("public_xwkyx_053");
        for (int i = 0; i < 3; i++)
        {
            if (i == RightPosId)
            {
                string signName = "";
                string EffectSoundName = "";
                string SoundName = baseSoundName + RightDaojuId;
                string RealName = basemeshName + RightDaojuId;
                RightDjdate = new DjData(signName, EffectSoundName, SoundName, RealName);
                RightDjdate.Id = i;
                RightDjdate.IsRight = true;
                RightDjdate.RightId = RightPosId;
                for (int j = 0; j < IdList.Count; j++)
                {
                    if (IdList.Contains(RightDaojuId))
                        IdList.RemoveAt(j);
                }
                CreateMesh(RightDjdate);
            }
            else
            {
                int daoju = IdList[UnityEngine.Random.Range(0, IdList.Count)];//随机卡片
                string signName = "";
                string EffectSoundName = "";
                string SoundName = baseSoundName + daoju;
                string RealName = basemeshName + daoju;
                DjData djdata = new DjData(signName, EffectSoundName, SoundName, RealName);
                djdata.Id = i;
                IdList.Remove(daoju);
                djdata.IsRight = false;
                CreateMesh(djdata);
            }
            if (i >= 2)
            {
                hlGame.PlayDjTween();
                MeshButtonManager.Instance.SetMeshButtonInteractable(hlGame.hulu.GetComponent<MeshButton>(), true);
            }
        }
    }
    void CreateMesh(DjData cd)
    {    
        GameObject obj = ABResMgr.Instance.LoadResource<GameObject>("game/BaibianHulu/prefabs/daoju/" + cd.RealName, "baibianhulu", true);
        if (DaojuParent == null)
            DaojuParent = new GameObject("DaojuParent");
        if (obj == null)
        {
            Debug.LogError("不存在这种资源:" + cd.RealName);
            return;
        }
        obj.transform.parent = hlGame.DjpathTweenList[cd.Id].transform;
        obj.tag = "meshButton";
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = obj.transform.localScale;
        AddDjEvent(obj,cd);
    }
    void AddDjEvent(GameObject obj,DjData data)
    {
        HuluCard hulu = null;
        MeshButton button = null;
        if (obj.GetComponent<HuluCard>() == null)
        {
            hulu = obj.AddComponent<HuluCard>();
            HuluList.Add(hulu);
        }
        else
        {
            hulu = obj.GetComponent<HuluCard>();
            HuluList.Add(hulu);
        }
        if (obj.GetComponent<MeshButton>() == null)
        {
            button = obj.AddComponent<MeshButton>();
        }
        else
        {
            button = obj.GetComponent<MeshButton>();
        }
        meshButtonList.Add(button);
        button.AddMeshEvent(hulu.OnclickDaojuEvent, hlGame.OnClickDaojuFinish);
        MeshButtonManager.Instance.SetMeshButtonInteractable(button, false);
        hulu.InitDjdata(data, hlGame);
        hulu.MoveDaoju(delegate { MeshButtonManager.Instance.SetMeshButtonInteractable(button, true); });//等道具完全掉落才可以点击
    }
}
public class HlGame : C_IState, IRubbish
{
    #region#常量
    public List<DOTweenPath> DjpathTweenList;
    public List<DOTweenPath> wukongpathTweenList;
    public List<GameObject> RubbishList = new List<GameObject>();
    public GameObject hulu;
    public string Name { get; set; }
    Camera cam;
    GameObject xwk;
    Huluxwk hlxwk;
    Hulu hl;
    GameObject public_effect_bbhl_xc;
    bool ChooseDJRight = false;
    Vector3[] xwkPosArray = new Vector3[] { new Vector3(0F,-0.5f, 0f),
            new Vector3(-0.1F, -0.5F, 0),new Vector3(0F,-0.5F, 0f)};
    Vector3[] effectPosArray = new Vector3[] { new Vector3(1.5f,0.95f, 1.65f),
            new Vector3(0, 0.95f, 1.65f),new Vector3(-1.5f,0.95f, 1.65f)};
    string baseDaojuSoundPath = "game/BaibianHulu/sound/daojusound";
    int wrongHitCount = 0;
    WaitForSeconds itwait = new WaitForSeconds(2.0f);
    WaitForSeconds longWait = new WaitForSeconds(5f);
    WaitForSeconds ctwait = new WaitForSeconds(1.2f);
    WaitForSeconds wait = new WaitForSeconds(0.5f);
    DjSpawn Spawn = new DjSpawn();
    public HlGame(Camera cam, GameObject xwk, GameObject hulu)
    {
        this.cam = cam;
        this.xwk = xwk;
        this.hulu = hulu;
        hlxwk = xwk.GetComponent<Huluxwk>();
        hl = hulu.GetComponent<Hulu>();
    }
    public void InitPathList(List<DOTweenPath> pathList, List<DOTweenPath> wukongpathTweenList)
    {
        DjpathTweenList = pathList;
        this.wukongpathTweenList = wukongpathTweenList;
    }
    public virtual void OnStateEnter()
    {
        Name = "HlGame";
        InitGame();
    }
    public void RecycleRubbish()
    {
        if (MeshButtonManager.Instance)
        {
            GameObject.Destroy(MeshButtonManager.Instance.transform.gameObject);
            GameObject.Destroy(MeshButtonManager.Instance);
        }
        for (int i = 0; i < RubbishList.Count; i++)
            if (RubbishList[i] != null)
                GameObject.Destroy(RubbishList[i]);
        if (hulu)
            hulu.GetComponent<MeshButton>().RemoveMeshEvent();
        HuluManager.Instance.StopAllCoroutines();
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
    #endregion


    #region#注册点击状态
    void InitGame()
    {     
        MeshButtonManager.Instance.SetMeshCamera(cam);
        HuluManager.Instance.PlayUserNameAudio("bbhlyx_5",//听好了，我要变成
        delegate
        {
            PlayGuideSound(Spawn.SetRightDaojuId(),
                delegate
                {
                    hlxwk.WukongOutEndSpeak();
                    hlxwk.WukongFlytoHulu(wukongpathTweenList[2], AddHlClickEvent);
                });
        }
                );
    }
    void AddHlClickEvent()
    {
        HuluSwing();
        hulu.GetComponent<MeshButton>().AddMeshEvent(
            delegate
            {
                RePlayGuideSound(null);
            }
             );
    }

    //葫芦摇起来
    void HuluSwing()
    {
        hl.Speak();
        RePlayGuideSound();
        hl.PlayShake(
            delegate
            {
                xwk.SetActive(false);
                for (int i = 0; i < Spawn.HuluList.Count; i++)
                    if (Spawn.HuluList[i])
                    {
                        GameObject.Destroy(Spawn.HuluList[i].gameObject);
                    }
                Spawn.HuluList.Clear();
                HuluManager.Instance.StartCoroutine(CreatDaojuPrefab());
            }
            );
    }  
    IEnumerator CreatDaojuPrefab()
    {
        yield return ctwait;
        Spawn.InitData(this);
    }
    #endregion


    /// <summary>
    /// 播放路径动画
    /// </summary>
    public void PlayDjTween()
    {
        for (int i = 0; i < DjpathTweenList.Count; i++)
        {
            DjpathTweenList[i].DORestart();
        }
    }

    #region##结果反馈处理
    /// <summary>
    /// 用户选择道具逻辑处理
    /// </summary>
    /// <param name="cd"></param>
    public void OnClickDaojuFinish()
    {
        if (ChooseDJRight)
        {
            HuluManager.Instance.PlayEffectAudio("public_xwkyx_054");
            MeshButtonManager.Instance.SetMeshButtonListInteractable(Spawn.meshButtonList, false);
            for (int i = 0; i < Spawn.HuluList.Count; i++)
            {
                if (Spawn.HuluList[i].GetDjdata().IsRight)
                {
                    ChooseSuccessEffect(i);
                    Spawn.HuluList[i].transform.gameObject.SetActive(false);
                    ShowClickResult(true);
                }
                else
                {
                    int timer = 2;
                    if (Spawn.HuluList[i].gameObject)
                        GameObject.Destroy(Spawn.HuluList[i].gameObject, timer);
                }
            }
            MeshButtonManager.Instance.SetMeshButtonInteractable(hulu.GetComponent<MeshButton>(), false);
        }//错误
        else
        {
            HuluManager.Instance.PlayEffectAudio("public_xwkyx_052");
            wrongHitCount++;
            if (wrongHitCount == 1)
            {
                for (int i = 0; i < Spawn.HuluList.Count; i++)
                {
                    if (Spawn.HuluList[i].ClickMesh())
                    {
                        Spawn.HuluList[i].gameObject.SetActive(false);
                        ChooseFailEffect(i);
                        HuluManager.Instance.PlayCharacterAudio_Nointerruption("common_90");
                    }
                }
            }
            else
            {
                for (int i = 0; i < Spawn.HuluList.Count; i++)
                {
                    if (Spawn.HuluList[i].GetDjdata().IsRight)
                    {
                        //两次都选错了，对的悟空出来
                        HuluManager.Instance.StartCoroutine(IntroduceRightXwk(Spawn.HuluList[i].gameObject));
                    }
                    else
                    {
                        HuluManager.Instance.PlayCharacterAudio_Nointerruption("common_90");
                        if (Spawn.HuluList[i].ClickMesh())
                        {
                            if (Spawn.HuluList[i].gameObject.activeInHierarchy)
                            {
                                ChooseFailEffect(i);
                                Spawn.HuluList[i].gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            int timer = 2;
                            if (Spawn.HuluList[i].gameObject)
                                GameObject.Destroy(Spawn.HuluList[i].gameObject, timer);
                        }
                    }
                    wrongHitCount = 0;
                }
                MeshButtonManager.Instance.SetMeshButtonListInteractable(Spawn.meshButtonList, false);
                MeshButtonManager.Instance.SetMeshButtonInteractable(hulu.GetComponent<MeshButton>(), false);
            }
        }
    }
    /// <summary>
    /// 两次都选错了，正确小悟空出现
    /// </summary>
    /// <param name="daoju"></param>
    /// <returns></returns>
    IEnumerator IntroduceRightXwk(GameObject daoju)
    {
        yield return itwait;
        daoju.transform.DOScale(0.012f * Vector3.one, 0.5f);
        yield return wait;
        daoju.SetActive(false);
        ShowClickResult(false);
    }
     
    /// <summary>
    /// 是否找到道具，再次游戏
    /// </summary>
    /// <param name="chooseRight"></param>
    void ShowClickResult(bool chooseRight)
    {
        if (chooseRight)
        {
            RePlayGuideSound();
            SetWukongTransform();
            hlxwk.WukongOutAnim(true,//悟空出现
               delegate
               {
                   HuluManager.Instance.PlayChooseRightSound(LoopGame);
               });
        }
        else
        {
            PlayHitEffect(Spawn.RightDjdate);
            SetWukongTransform();
            RePlayGuideSound();
            hlxwk.WukongOutSpeak();
            HuluManager.Instance.PlayChooseWrongSound(LoopGame);
        }
    }
    /// <summary>
    /// 循环游戏
    /// </summary>
    void LoopGame()
    {
        if (HuluManager.Instance.GotoFinish())
        {
            return;
        }
        SetChooseDJResult(false);
      
        HuluManager.Instance.PlayCharacterAudio("bbhlyx_15",//我们再来一次吧
            delegate
            {
                HuluManager.Instance.PlayUserNameAudio("bbhlyx_5",//听好了，我要变成
        delegate
        {
            PlayGuideSound(Spawn.SetRightDaojuId(),delegate
            {
                wrongHitCount = 0;
                hlxwk.WukongFlytoHulu(wukongpathTweenList[Spawn.RightPosId], HuluSwing);
            });
        });
            }
            );
    }
    
     
    #endregion


    #region#用户点击逻辑处理        
    public void SetChooseDJResult(bool flag)
    {
        ChooseDJRight = flag;
    }
    /// <summary>
    /// 播放道具落地特效
    /// </summary>
    /// <param name="cd"></param>
    public void PlayToGroundEffect(DjData cd)
    {
        GameObject effect = ABResMgr.Instance.LoadResource<GameObject>(HlfilePath.Instance.public_effect_yan02, ABCommonConfig.EfBundleType, true);
        effect.transform.position = effectPosArray[cd.Id];
        RubbishList.Add(effect);
        GameObject.Destroy(effect, 2);
    }
    /// <summary>
    /// 播放点击道具特效
    /// </summary>
    /// <param name="cd"></param>
    void PlayHitEffect(DjData cd)
    {
        HuluManager.Instance.PlayEffectAudio("public_xwkyx_020");
        GameObject effect = ABResMgr.Instance.LoadResource<GameObject>(HlfilePath.Instance.effect_xiaoshi_small, ABCommonConfig.EfBundleType, true);
        effect.transform.position = effectPosArray[cd.Id];
        RubbishList.Add(effect);
        GameObject.Destroy(effect, 2);
    }
    /// <summary>
    /// 播放随机道具声音
    /// </summary>
    /// <param name="ac"></param>
    void PlayGuideSound(int djId,Action ac = null)
    {       
        if (djId >= 10)
        {
            AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>(baseDaojuSoundPath + "/dj000" + Spawn.RightDaojuId, "baibianhulu");
            AudioManagerExtern.Instance.PlaySmallClipSound(clip, ac);
        }
        else
        {
            AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>(baseDaojuSoundPath + "/dj0000" + Spawn.RightDaojuId, "baibianhulu");
            AudioManagerExtern.Instance.PlaySmallClipSound(clip, ac);
        }
    }
    /// <summary>
    /// 再次播放随机道具声音
    /// </summary>
    /// <param name="ac"></param>
    public void RePlayGuideSound(Action ac = null)
    {
        if (Spawn.GetRightDaojuId() >= 10)
        {
            AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>(baseDaojuSoundPath + "/dj000" + Spawn.RightDaojuId, "baibianhulu");
            AudioManagerExtern.Instance.PlaySmallClipSound(clip, ac);
        }
        else
        {
            AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>(baseDaojuSoundPath + "/dj0000" + Spawn.RightDaojuId, "baibianhulu");
            AudioManagerExtern.Instance.PlaySmallClipSound(clip, ac);
        }
    }

    void ChooseFailEffect(int id)
    {
        HuluManager.Instance.PlayEffectAudio("public_xwkyx_020");
        public_effect_bbhl_xc = ABResMgr.Instance.LoadResource<GameObject>(HlfilePath.Instance.public_effect_bbhl_xc, ABCommonConfig.EfBundleType, true);
        public_effect_bbhl_xc.transform.position = effectPosArray[id];
        RubbishList.Add(public_effect_bbhl_xc);
    }
    void ChooseSuccessEffect(int id)
    {
        public_effect_bbhl_xc = ABResMgr.Instance.LoadResource<GameObject>(HlfilePath.Instance.public_effect_bbhl_cg, ABCommonConfig.EfBundleType, true);
        public_effect_bbhl_xc.transform.position = effectPosArray[id];
        RubbishList.Add(public_effect_bbhl_xc);
    }
    void SetWukongTransform()
    {
        xwk.SetActive(true);
        xwk.transform.parent = wukongpathTweenList[Spawn.RightPosId].transform;
        xwk.transform.localPosition = xwkPosArray[Spawn.RightDjdate.Id];
        if (Spawn.RightDjdate.Id == 0)
        {
            xwk.transform.rotation = Quaternion.Euler(0, -20, 0);
        }
        else if (Spawn.RightDjdate.Id == 2)
        {
            xwk.transform.rotation = Quaternion.Euler(0, 20, 0);
        }
        else
        {
            xwk.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        xwk.transform.localScale = 0.01f * Vector3.one;
    }
    #endregion
}