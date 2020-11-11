using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using YB.XWK.MainScene;
/// <summary>
/// 配置表数据解析类
/// </summary>
public class SpData{
    SpriteData nowdata;
    public List<SpriteData> SpDataList = new List<SpriteData>();
    List<SpriteData> AllSpDataList = new List<SpriteData>();

    List<SpriteData> SpRemoveList = new List<SpriteData>();
    List<string> userDataList = new List<string>();
    string newUserData;
    static bool isNewSp = false;
    public void ReadJsonData(LitJson.JsonData data, Action ac)
    {
        AllSpDataList.Clear();
        SpDataList.Clear();
        foreach (LitJson.JsonData temp in data["sprite"])
        {
            string name = temp["name"].ToString();
            string meshjtPath = temp["jiatiMesh"]["meshjtPath"].ToString();
            string meshrealPath = temp["realMesh"]["meshrealPath"].ToString();
            string rtmcjtPath = temp["jiatiMesh"]["rtmcPath"].ToString();
            string rtmcrealPath = temp["realMesh"]["rtmcPath"].ToString();
            string meshTitle = temp["iconName"]["titleName"].ToString();
            string upGradeTexName = temp["realMesh"]["upGradeTex"].ToString();
            string defaultTexName = temp["realMesh"]["defaultTex"].ToString();

            string level1 = temp["roleLevel"]["level1Path"].ToString();
            string level2 = temp["roleLevel"]["level2Path"].ToString();
            string level3 = temp["roleLevel"]["level3Path"].ToString();
            string level4 = temp["roleLevel"]["level4Path"].ToString();
            string roleMat = temp["mat"]["grayMat"].ToString();
            string normalRoleMat = temp["mat"]["normalMat"].ToString();

            RoleLevelData roledata = new RoleLevelData(level1, level2, level3, level4);
            MeshData mdata = new MeshData(meshjtPath, meshrealPath);
            mdata.lockingRoleMat = roleMat;
            SpriteTransform jiati = new SpriteTransform();
            jiati.InitSpTransform(getVector(temp["jiatiMesh"]["position"].ToString()), getVector(temp["jiatiMesh"]["eulerAngles"].ToString()), getVector(temp["jiatiMesh"]["localScale"].ToString()));
            SpriteTransform real = new SpriteTransform();
            real.InitSpTransform(getVector(temp["realMesh"]["position"].ToString()), getVector(temp["realMesh"]["eulerAngles"].ToString()), getVector(temp["realMesh"]["localScale"].ToString()));
            mdata.InitMeshInfo(jiati, real);
            mdata.InitMeshInfo(rtmcjtPath, rtmcrealPath);
            mdata.InitMeshInfo(meshTitle);
            mdata.NormalRoleMat = normalRoleMat;
            string Name = temp["name"].ToString();
            string lockName = temp["iconName"]["lockName"].ToString();
            string lockCenterName = temp["iconName"]["lockCenterName"].ToString();
            string releaseName = temp["iconName"]["releaseName"].ToString();
            string releaseCenterName = temp["iconName"]["releaseCenterName"].ToString();
            string needUpRole = temp["roleLevel"]["needUpRole"].ToString();//是否需要升级
            string haveStory = temp["roleLevel"]["haveStory"].ToString();//是否故事
                                                                         //sound
            string soundInfo = temp["soundInfo"]["roleInfo"].ToString();//自我介绍声音

            if (needUpRole.Equals("0"))
            {
                roledata.NeedUpRole = false;
            }
            else
            {
                roledata.NeedUpRole = true;//需要升级
            }
            nowdata = new SpriteData(name, WizardData.IsLockedWizardItem(name), mdata, roledata);
            nowdata.InitIconName(lockName, lockCenterName, releaseName, releaseCenterName);
            nowdata.InitLevel(WizardData.FetchWizardItemStar(name));
            nowdata.upGradeTexName = upGradeTexName;
            nowdata.defaultTexName = defaultTexName;
            nowdata.spLvel = WizardData.FetchWizardItemLevel(name);
            nowdata.spExp = WizardData.FetchWizardItemLevelProgress(name);
            nowdata.soundInfo = soundInfo;
            if (nowdata.spExp >= 1.0f)
                nowdata.spExp = 0;
            AllSpDataList.Add(nowdata);
            //SpriteLingqiMgr.Instance.Instantiate_Lingqi(nowdata);
            if (haveStory.Equals("0"))
            {
                nowdata.haveStory = false;
            }
            else
            {
                nowdata.haveStory = true;
            }
        }
        filterData();
        //Debug.LogError("账号数据:" + SpDataList.Count);
        if (ac != null)
            ac();
    }

    /// <summary>
    /// 过滤数据
    /// </summary>
    void filterData()
    {      
        //if (Application.isEditor)
        //{
        //    Debug.LogError(PlayerPrefs.GetString(LocalData.m_Collect_Spirit_Prefs));           
        //}
        //添加账号数据
        AddNormalData();
        if (PlayerPrefs.GetString(LocalData.m_Collect_Spirit_Prefs) != "")//精灵上新
        {
            isNewSp = true;
            for (int j = 0; j < AllSpDataList.Count; j++)
            {
                //Debug.LogError("精灵上新:"  + PlayerPrefs.GetString(LocalData.m_Collect_Spirit_Prefs));
                //判断是否是今日上新
                if (AllSpDataList[j].Name.Contains(PlayerPrefs.GetString(LocalData.m_Collect_Spirit_Prefs)))
                {
                    if (!SpDataList.Contains(AllSpDataList[j]))
                    {
                        SpDataList.Add(AllSpDataList[j]);
                    }
                }
            }
            PlayerPrefs.SetString(LocalData.m_Collect_Spirit_Prefs, "");
        }
        else
        {
            isNewSp = false;
        }
        AddFuture();
    }

    /// <summary>
    /// 添加默认的3个数据
    /// </summary>
    void AddNormalData()
    {
        //添加默认3个精灵
        for (int j = 0; j < AllSpDataList.Count; j++)
        {
            if (AllSpDataList[j].Name.Equals(WizardItemName.Wizard_BaiYin))
            {
                SpDataList.Add(AllSpDataList[j]);
            }
        }
        for (int j = 0; j < AllSpDataList.Count; j++)
        {
            if (AllSpDataList[j].Name.Equals(WizardItemName.Wizard_Hulu))
            {
                SpDataList.Add(AllSpDataList[j]);
            }
        }
        for (int j = 0; j < AllSpDataList.Count; j++)
        {
            if (AllSpDataList[j].Name.Equals(WizardItemName.Wizard_Xln))
            {
                SpDataList.Add(AllSpDataList[j]);
            }
        }
        //AddEditorData();
        //添加账号数据
        for (int i = 0; i < WizardData.GetWizardItemList().Count; i++)
        {
            for (int j = 0; j < AllSpDataList.Count; j++)
            {
                if (AllSpDataList[j].Name.Equals(WizardData.GetWizardItemList()[i].elfin))
                {
                    if (!SpDataList.Contains(AllSpDataList[j]))
                        SpDataList.Add(AllSpDataList[j]);
                }
            }
        }
    }
    static bool addEditorData = false;
    void AddEditorData()
    {
         if (addEditorData)
            return;
        addEditorData = true;
        if (Application.isEditor)
        {
            for (int j = 0; j < AllSpDataList.Count; j++)
            {
                if (AllSpDataList[j].Name.Equals(WizardItemName.Wizard_hongbao))
                {
                    SpDataList.Add(AllSpDataList[j]);
                }
            }
            for (int j = 0; j < AllSpDataList.Count; j++)
            {
                if (AllSpDataList[j].Name.Equals(WizardItemName.Wizard_nianshou))
                {
                    SpDataList.Add(AllSpDataList[j]);
                }
            }
            for (int j = 0; j < AllSpDataList.Count; j++)
            {
                if (AllSpDataList[j].Name.Equals(WizardItemName.Wizard_jisuanji))
                {
                    SpDataList.Add(AllSpDataList[j]);
                }
            }
        }
    }

    /// <summary>
    /// 获取到最新的精灵， 是否是今日上新
    /// </summary>
    /// <returns></returns>
    public bool IsGetNewSprite()
    {
        return isNewSp;       
    }
    public void SetGetSprite()//设置已经上新
    {
        isNewSp = false;
    }
    public void ResetSpritePos()//注意切换账号要初始化
    {
        isNewSp = false;
    }
  
    Vector3 getVector(string str)
    {
        string[] sArray = Regex.Split(str, ",", RegexOptions.IgnoreCase);
        return new Vector3(Convert.ToSingle(sArray[0]), Convert.ToSingle(sArray[1]), Convert.ToSingle(sArray[2]));
    }
    /// <summary>
    /// 添加？号精灵
    /// </summary>
    void AddFuture()
    {
        MeshData mdata4 = new MeshData(SpWindowPath.Instance.future, null);
        SpriteTransform jiati4 = new SpriteTransform();
        jiati4.InitSpTransform(new Vector3(0.1F, -1.5f, -0.36F), Vector3.zero, 0.04f * Vector3.one);

        SpriteTransform real4 = new SpriteTransform();
        real4.InitSpTransform(new Vector3(0f, -2.2f, 0f), Vector3.one, 0.04f * Vector3.one);
        mdata4.InitMeshInfo(jiati4, real4);

        mdata4.InitMeshInfo(SpWindowPath.Instance.future_jiatiRtmc, null);
        mdata4.InitMeshInfo("xln/xln_titile");
        SpriteData data4 = new SpriteData("future", false, mdata4);
        data4.InitIconName("future/btn_wenhao_2", "future/btn_wenhao_1", null, null);
        if (!SpDataList.Contains(data4))
            SpDataList.Add(data4);
    }
}

 
     
