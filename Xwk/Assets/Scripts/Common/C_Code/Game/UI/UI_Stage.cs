using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum EnumStageStatus
{
    None = -1,
    Normal,
    Update,
    Download,
    Downloading,
    Pay
}

public class UI_Stage : MonoBehaviour
{
    [SerializeField]
    private Image m_Image_StoryName = null;
    [SerializeField]
    private GameObject m_AvatarLayer = null;
    [SerializeField]
    private GameObject m_EffectLayer = null;
    [SerializeField]
    private Image m_Image_Tag = null;
    [SerializeField]
    private GameObject[] m_StoryStarVector = null;

    private GameObject m_Avatar = null;
    private List<Animator> m_AvatarAnimatorList = new List<Animator>();

    private GameObject m_Effect = null;

    private GameObject[] m_StarBaoVector = new GameObject[3];

    private StageConfigItem m_StageConfigItem = null;
    private StageDataItem m_StageDataItem = null;

    [HideInInspector]
    public string StoryName = "";

    //0：未解锁，1：正常，2：需要更新，3：需要下载，4：正在下载，5：VIP
    private EnumStageStatus m_StageStatus = EnumStageStatus.None;
    private EnumStageStatus StageStatus
    {
        get { return m_StageStatus; }
        set
        {
            if (m_StageStatus != value)
            {
                m_StageStatus = value;

                Texture2D texture = null;

                switch (m_StageStatus)
                {
                    case EnumStageStatus.Update:
                        texture = C_ResMgr.LoadResource<Texture2D>("Update", LocalPath.StageResourcesPath);
                        break;
                    case EnumStageStatus.Download:
                        texture = C_ResMgr.LoadResource<Texture2D>("Download", LocalPath.StageResourcesPath);
                        break;
                    case EnumStageStatus.Downloading:
                        texture = C_ResMgr.LoadResource<Texture2D>("Download", LocalPath.StageResourcesPath);
                        break;
                    case EnumStageStatus.Pay:
                        texture = C_ResMgr.LoadResource<Texture2D>("Pay", LocalPath.StageResourcesPath);
                        break;
                }

                if (texture != null)
                {
                    m_Image_Tag.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    m_Image_Tag.gameObject.SetActive(true);
                }
                else
                {
                    m_Image_Tag.gameObject.SetActive(false);
                }
            }
        }
    }


    public void Init(string name)
    {
        StoryName = name;

        m_StageConfigItem = StageConfig.GetStageConfigItem(name);
        if (m_StageConfigItem == null)
            return;

        m_StageDataItem = StageData.GetStageDataItem(m_StageConfigItem.ID);
        if (m_StageDataItem == null)
            m_StageDataItem = new StageDataItem();

        InitUI();
    }

    private void InitUI()
    {
        Texture2D texture = C_ResMgr.LoadResource<Texture2D>(StoryName, LocalPath.StageResourcesPath);
        if (texture != null)
            m_Image_StoryName.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        m_Avatar = C_Singleton<GameResMgr>.GetInstance().LoadResource_Effect("ui_zjm_" + StoryName);
        if (m_Avatar != null)
        {
            m_Avatar.transform.SetParent(m_AvatarLayer.transform);
            m_Avatar.transform.localPosition = Vector3.one;
            m_Avatar.transform.localScale = Vector3.one;

            Transform[] grandFa = m_Avatar.GetComponentsInChildren<Transform>();
            foreach (Transform child in grandFa)
            {
                Animator animator = child.gameObject.GetComponent<Animator>();
                if (animator != null)
                {
                    m_AvatarAnimatorList.Add(animator);
                    animator.enabled = false;
                }
            }
        }

    //    RefreshStage();
    }

    //public void RefreshStage()
    //{
    //    if (m_StageConfigItem == null)
    //        return;

    //    if (C_Singleton<StageMgr>.GetInstance().MaxCanPlayStage != m_StageConfigItem.Name)
    //    {
    //        if (m_Effect != null)
    //        {
    //            Destroy(m_Effect);
    //            m_Effect = null;
    //        }
    //    }
    //    else
    //    {
    //        if (m_Effect == null)
    //        {
    //            m_Effect = C_Singleton<GameResMgr>.GetInstance().LoadResource_Effect("ui_public_effect_zjm");
    //            m_Effect.transform.SetParent(m_EffectLayer.transform);
    //            m_Effect.transform.localPosition = Vector3.zero;
    //            m_Effect.transform.localScale = Vector3.one;
    //        }
    //    }

    //    //更新数据
    //    m_StageDataItem = StageData.GetStageDataItem(m_StageConfigItem.ID);
    //    if (m_StageDataItem == null)
    //        m_StageDataItem = new StageDataItem();

    //    UpdateStar();
    //    UpdateTag();
    //}

    //private void UpdateStar()
    //{
    //    int maxStar = 0;
    //    if (C_Singleton<StageMgr>.GetInstance().c_CompleteStageData != null && StoryName == C_Singleton<StageMgr>.GetInstance().c_CompleteStageData.StageName)
    //        maxStar = C_Singleton<StageMgr>.GetInstance().c_CompleteStageData.BeforeStar;
    //    else
    //        maxStar = m_StageDataItem.MaxStar;

    //    for (int i = 0; i < m_StoryStarVector.Length; i++)
    //    {
    //        if (i < maxStar)
    //            m_StoryStarVector[i].SetActive(true);
    //        else
    //            m_StoryStarVector[i].SetActive(false);
    //    }
    //}

    public void UpdateTag()
    {
        if (PlayerData.IsVIP != 1 && m_StageConfigItem.Pay == 1)
        {
            StageStatus = EnumStageStatus.Pay;
            return;
        }

        //3代表正在下载，2代表需要下载，1代表需要更新，0代表是最新的
        switch (GameHotUpdateMgr.GetStageHotUpdateStatus(StoryName))
        {
            case 3:
                StageStatus = EnumStageStatus.Downloading;
                break;

            case 2:
                StageStatus = EnumStageStatus.Download;
                break;

            case 1:
                StageStatus = EnumStageStatus.Update;
                break;

            default:
                StageStatus = EnumStageStatus.Normal;
                break;
        }
    }

    //public void GoStory()
    //{
    //    //AudioMgr.PlaySoundEffect("public_sd_042");

    //    C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "dianji_gushi", StoryName);

    //    if (m_StageConfigItem.Pay == 1 && PlayerData.IsVIP != 1)
    //    {
    //        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_Shop", 1);
    //        return;
    //    }

    //    switch (StageStatus)
    //    {
    //        case EnumStageStatus.Download:
    //        case EnumStageStatus.Update:

    //            GameHotUpdateMgr.DownloadStageHotUpdate(StoryName, ()=> { StageStatus = EnumStageStatus.Downloading; });

    //            break;

    //        case EnumStageStatus.Normal:

    //            //if (m_StageDataItem.State == 0)
    //            //{
    //            //    C_MonoSingleton<PlayerController>.GetInstance().PlayAudio("common_372");
    //            //    return;
    //            //}

    //            //C_Singleton<StageMgr>.GetInstance().LoadStage(StoryName);

    //            //C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "jinru_gushi", StoryName);

    //            //C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeStart, "time_gushi", StoryName);

    //            break;
    //    }
    //}

    public void RunStarAction(int beforeStar, int affterStar)
    {
        float delayTime = 0.0f;

        if (beforeStar < affterStar)
        {
            if (beforeStar == 0 && affterStar > 0)
            {
                Invoke("StoryShowStar0", delayTime);
                delayTime += 0.5f;
            }

            if (beforeStar < 2 && affterStar > 1)
            {
                Invoke("StoryShowStar1", delayTime);
                delayTime += 0.5f;
            }

            if (beforeStar < 3 && affterStar > 2)
                Invoke("StoryShowStar2", delayTime);
        }

        Invoke("StoryEndAction", delayTime);
    }

    private void StoryShowStar0()
    {
        RunStarBao(0);
    }

    private void StoryShowStar1()
    {
        RunStarBao(1);
    }

    private void StoryShowStar2()
    {
        RunStarBao(2);
    }

    private void RunStarBao(int index)
    {
        m_StoryStarVector[index].SetActive(true);

        m_StarBaoVector[index] = C_Singleton<GameResMgr>.GetInstance().LoadResource_Effect("ui_public_effect_xxsmall");
        m_StarBaoVector[index].transform.SetParent(m_StoryStarVector[index].transform);
        m_StarBaoVector[index].transform.localPosition = Vector3.zero;
        m_StarBaoVector[index].transform.localScale = Vector3.one;

       // AudioMgr.PlaySoundEffect("public_sd_057");
    }

    private void StoryEndAction()
    {
        for (int i = 0; i < m_StarBaoVector.Length; i++)
        {
            if (m_StarBaoVector[i] != null)
            {
                Destroy(m_StarBaoVector[i]);
                m_StarBaoVector[i] = null;
            }
        }

       // C_Singleton<GameActionMgr>.GetInstance().RunMainCity_CompleteStageStep(21);
    }

    public void SetStageActionEnabled(bool value)
    {
        for (int i = 0; i < m_AvatarAnimatorList.Count; i++)
            m_AvatarAnimatorList[i].enabled = value;
    }
}
