using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 灵气管理类
/// </summary>
public class SpriteLingqiMgr : C_MonoSingleton<SpriteLingqiMgr> {

    Dictionary<string, SpLingqi> SplingqiDic = new Dictionary<string, SpLingqi>();
    List<SpLingqi> SplingqiList = new List<SpLingqi>();
    TimeSpan ts;
    public static string BackGroundTime;
    public static string OfflineTime;
    static bool OnlineState =true;
    string SaveLocalName = "Lq";
    bool isStayingSpwindow = false;

    void Update()
    {
        if (isStayingSpwindow)
        {
            UPdateLingqiClick();
            UpdateLingqiShowOrClose();
        }
        OnApplicCationHome();
    }

    #region##处理灵气对外接口  
    //实例化灵气对象
    public void Instantiate_Lingqi(SpriteData data) 
    {
        if (!SplingqiDic.ContainsKey(data.Name))
        {
            SpLingqi lq = new SpLingqi(data);
            SplingqiDic.Add(data.Name, lq);
            SplingqiList.Add(lq);
        }
        Refresh_LingqiData();
    } 
    //进入二级界面
    public void InitOnlineLingq()
    {
        if (OnlineState)//首次上线
        {
            OnlineState = false;
            if (WizardData.IsNewUser())
            {
                RefreshNewUserLingqiData(0);
            }
            else
            {
                RefreshOnlineLingqiData(CalculateLeaveTime());
                //Debug.LogError("挂机时间计算:" + UserOfflineTime);
            }
        }
        else
        {
            //切换账号的操作
            if (WizardData.IsNewUser())
            {
                RefreshNewUserLingqiData(0);
            }
            else
            {
                //RefreshLingqi();//如果切换账号是一个老账号，默认清空数据              
                RefreshOnlineLingqiData(CalculateLeaveTime());
            }
        }
        isStayingSpwindow = true;
    }
    //离开二级界面,清空所有灵气对象
    public void LeaveSpWindow()
    {
        isStayingSpwindow = false;
        string time = DateTime.Now.ToString();
        PlayerPrefs.SetString(PlayerData.UID + OfflineTime, time);
        SaveLingqi();
        SplingqiList.Clear();
        SplingqiDic.Clear();
    }
    #endregion


    #region##灵气逻辑处理
    float CalculateLeaveTime()//离线时间
    {
        string offLineTime = PlayerPrefs.GetString(PlayerData.UID + OfflineTime).ToString();
        if (offLineTime == "")
        {
            return 0;
        }
        else
        {
            DateTime oldTime = Convert.ToDateTime(offLineTime);
            ts = DateTime.Now - oldTime;
            float UserOfflineTime = (float)ts.TotalSeconds;
            return UserOfflineTime;
        }
    }
    
    void RefreshOnlineLingqiData(float timer)//刚上线刷新灵气
    {
        string saveData;
        float fdata;
        foreach (SpLingqi obj in SplingqiDic.Values)
        {
            if(PlayerPrefs.GetString(PlayerData.UID+obj.Name + SaveLocalName)=="")
            {
                fdata = 0;
            }
            else
            {
                saveData = PlayerPrefs.GetString(PlayerData.UID+obj.Name + SaveLocalName);
                if (saveData == "")
                {
                    fdata = 0;
                }
                else
                {
                    fdata = Convert.ToSingle(saveData);
                    if (fdata <= 0)
                        fdata = 0;
                }
            }          
            obj.InitOnLineLingq(Mathf.Floor(timer), fdata);
            //Debug.LogError("挂机时间"+Mathf.Floor(timer)+"账号数据:"+fdata);
        }
    }

    void RefreshNewUserLingqiData(float timer)//清空所有的灵气
    {     
        foreach (SpLingqi obj in SplingqiDic.Values)
        {
            obj.ClearLingqi();
            //Debug.LogError(obj.Name + ":" + PlayerPrefs.GetString(obj.Name + SaveLocalName));
        }
    }
        
    void Refresh_LingqiData()//刷新数据
    {
        for (int i = 0; i < SplingqiList.Count; i++)
            SplingqiList[i].spData.Refresh();
        foreach (var obj in SplingqiDic.Values)
            obj.spData.Refresh();
    }
    void UpdateLingqiShowOrClose()//动态监测精灵灵气
    {
        for (int i = 0; i < SplingqiList.Count; i++)
        {
            SplingqiList[i].Update();
            if (SpriteIconMgr.Instance.GetSpriteData() != null)
            {
                if (SplingqiList[i].Name.Equals(SpriteIconMgr.Instance.GetSpriteData().Name))
                {
                    SplingqiList[i].ShowLingqi();
                }
                else
                {
                    SplingqiList[i].CloseLingqi();
                }
            }
        }
    }
    #endregion


    #region##灵气状态
    public void RefreshAllLingqiData()//升级之后刷新灵气数据
    {
        for (int i = 0; i < SplingqiList.Count; i++)
        {
            SplingqiList[i].RefreshSpData();
        }
    }
    public bool IsFullLingqi(string name)
    {
        if (SplingqiDic.ContainsKey(name))
        {
            return SplingqiDic[name].isFull();
        }
        return false;
    }
    /// <summary>
    /// 所有灵气满了
    /// </summary>
    /// <returns></returns>
    public bool IsFullAllLingqi()
    {
        int lockIconCount = 0;
        int fullCount = 0;
        for (int i=0;i<SplingqiList.Count;i++)
        {
            if (SplingqiList[i].spData.lockIcon)
                lockIconCount++;
        }
        for (int i = 0; i < SplingqiList.Count; i++)
        {
            if (SplingqiList[i].isFull())
                fullCount++;
        }      
        return lockIconCount==fullCount;
    }
    
    public void RefreshLingqi(SpriteData spData)//灵气关闭 打开
    {
        for (int i = 0; i < SplingqiList.Count; i++)
        {
            SplingqiList[i].CloseLingqi();
        }
        if (spData != null)
            if (SplingqiDic.ContainsKey(spData.Name))
                SplingqiDic[spData.Name].ShowLingqi();
    }

    public Camera GetLqCam()
    {
        return SpriteIconMgr.Instance.LingqiCam;
    }

    /// <summary>
    /// 灵气旋涡点击事件
    /// </summary>
    void UPdateLingqiClick()
    {
        if (GetLqCam() == null)
            return;
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = GetLqCam().ScreenPointToRay(Input.mousePosition);
                RaycastHit rayHit;
                if (Physics.Raycast(ray, out rayHit, 2000, ~(1 << 12)))
                {
                    GameObject obj = rayHit.collider.gameObject;
                    if (obj.CompareTag("meshButton") | obj.CompareTag("meshButton_Begin"))
                    {
                        MeshButton button = obj.GetComponent<MeshButton>();
                        if (button != null)
                        {
                            button.MeshHit();
                            button.MeshHit(obj);
                        }
                    }
                }
            }
        }
        if (Input.touchCount >= 1)
        {
            Ray ray = GetLqCam().ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit rayHit;
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (Physics.Raycast(ray, out rayHit))
                {
                    if (Physics.Raycast(ray, out rayHit, 2000, ~(1 << 12)))
                    {
                        GameObject obj = rayHit.collider.gameObject;
                        if (obj.CompareTag("meshButton_Begin"))
                        {
                            MeshButton button = obj.GetComponent<MeshButton>();
                            if (button != null)
                            {
                                button.MeshHit();
                                button.MeshHit(obj);
                            }
                        }
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (Physics.Raycast(ray, out rayHit, 2000))
                {
                    GameObject obj = rayHit.collider.gameObject;
                    if (obj.CompareTag("meshButton"))
                    {
                        MeshButton button = obj.GetComponent<MeshButton>();
                        if (button != null)
                        {
                            button.MeshHit();
                            button.MeshHit(obj);
                        }
                    }
                }
            }
        }
    }
    #endregion


    #region##保存灵气
    void OnApplicCationHome()
    {
        if (Application.platform == RuntimePlatform.Android) // 返回键
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SaveLingqi();
            }
            else if (Input.GetKeyDown(KeyCode.Home))
            {
                SaveLingqi();
            }
        }
    }
    void OnApplicationPause(bool focus)
    {
        if (focus)
        {
            //Debug.LogError("进入后台");
            string time = DateTime.Now.ToString();
            PlayerPrefs.SetString(PlayerData.UID + BackGroundTime, time);
            SaveLingqi();
            PlayerPrefs.SetString(PlayerData.UID + OfflineTime, time);
        }
        else
        {
            string time = PlayerPrefs.GetString(PlayerData.UID + BackGroundTime).ToString();
            if (BackGroundTime == "")
                return;
            DateTime oldTime = Convert.ToDateTime(time);
            ts = DateTime.Now - oldTime;
            float UserOfflineTime = (float)ts.TotalSeconds;
            RefreshOnlineLingqiData(UserOfflineTime);
            // Debug.LogError("后台时间计算:" + UserOfflineTime);
        }
    }
    void OnApplicationQuit()//退出程序
    {
        string time = DateTime.Now.ToString();
        PlayerPrefs.SetString(PlayerData.UID + OfflineTime, time);
        SaveLingqi();
    }

    /// <summary>
    /// 保存  每个精灵灵气个数
    /// </summary>
    void SaveLingqi()
    {
        for(int i=0;i< SplingqiList.Count;i++)
        {
            PlayerPrefs.SetString(PlayerData.UID+SplingqiList[i].Name+SaveLocalName,SplingqiList[i].lingqiTotalCount.ToString());
        }
    }   
    
    #endregion


   

    
 }

