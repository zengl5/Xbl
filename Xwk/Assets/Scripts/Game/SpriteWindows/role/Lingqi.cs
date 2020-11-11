using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lingqi : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
/// <summary>
/// 精灵对应灵气生成类
/// </summary>
public class SpLingqi
{
    List<GameObject> LqList=new List<GameObject>();
    List<GameObject> LqEfList = new List<GameObject>();
    List<Vector3> LqPosList = new List<Vector3>();
    int[] GetMaxlq = new[] { 2, 4, 6, 6 };
    int[] GetTimeEditorlimit = new[] { 20, 35, 45, 65 };
    int[] GetTimelimit = new[] { 20, 35, 45, 65 }; 

    float timer = 0;
    float limtTimer = 0;
    int limtCount = 2;
    public SpriteData spData;
    GameObject lingqiParent =null;
    public string Name;
    bool isLoadingLq = false;
    public int lingqiTotalCount = 0;
    float timeScale = 10f;
    public SpLingqi(SpriteData data)
    {
        spData = data;
        Name = spData.Name;
    }
    //时刻生成灵气
    public void Update()
    {
        GetLimit();
        UpdateSpawnLq();
    }
    
    /// <summary>
    /// 初始化接口
    /// </summary>
    /// <param name="offlineTime">挂机时间</param>
    /// <param name="saveCount">用户之前数据</param>
    public void InitOnLineLingq(float offlineTime,float saveCount)//初始化上线灵气个数
    {
        ClearLingqi();
        if (spData.lockIcon)
        {
            if (Application.isEditor)
            {
                //int[] GetMaxlq = new[] { 2, 4, 6, 6 };
                //int[] GetTimelimit = new[] { 20, 35, 45, 65 };
                //*****时间/一个灵气需要时间=需要生成个数
                //if (isFull())
                //    return;
                int needAddcount = (int)offlineTime / GetTimeEditorlimit[spData.spLvel]+(int)saveCount;
                if (lingqiTotalCount + needAddcount >= GetMaxlq[spData.spLvel])
                {
                    ReCreateLingqi(GetMaxlq[spData.spLvel] - lingqiTotalCount);
                    lingqiTotalCount = GetMaxlq[spData.spLvel];
                }
                else
                {
                    //Debug.LogError(needAddcount);
                    ReCreateLingqi(needAddcount);
                    lingqiTotalCount += needAddcount;
                }
            }
            else
            {    
                int needAddcount = (int)offlineTime / GetTimelimit[spData.spLvel]+(int)saveCount;

                if (lingqiTotalCount + needAddcount >= GetMaxlq[spData.spLvel])
                {
                    ReCreateLingqi(GetMaxlq[spData.spLvel] - lingqiTotalCount);
                    lingqiTotalCount = GetMaxlq[spData.spLvel];
                }
                else
                {
                    ReCreateLingqi(needAddcount);
                    lingqiTotalCount += needAddcount;
                }
            }
        }
        else
        {
            ClearLingqi();
        }
    }


    #region##生成灵气
    //分两种，一种是记录当前已经生成，一种是重新打乱，再次生成
    void UpdateSpawnLq()
    {
        if (spData != null)
        {
            if (spData.lockIcon)
            {
                if (!isFull())
                {
                    spData.fullLingqi = false;
                    timer += Time.deltaTime;
                    if (!isLoadingLq)
                    {
                        if (timer > limtTimer)
                        {
                            timer = 0;
                            isLoadingLq = true;
                            SpriteLingqiMgr.Instance.StartCoroutine(AddLingqi());
                        }
                    }
                }
                else
                {
                    spData.fullLingqi = true;
                }
            }
        }
    }
    void ReCreateLingqi(int count)
    {
        LqPosList.Clear();
        LqList.Clear();
        AddLqPosList();

        for (int i = 0; i < count; i++)
        {
            if(LqPosList.Count>0)
            {
                Vector3 pos = LqPosList[Random.Range(0, LqPosList.Count)];
                GameObject obj = AddCoin(pos);
                AddCoinBind(obj);
                LqPosList.Remove(pos);
            }
            else
            {
                AddLqPosList();
            }
        }
    }     
    Transform getLingqiParent()
    {
        if (lingqiParent == null)
        {
            lingqiParent = new GameObject(spData.Name);
            RectTransform rectTrans = lingqiParent.AddComponent<RectTransform>();
            rectTrans.anchorMin = new Vector2(0, 0.5f);
            rectTrans.anchorMax = new Vector2(1, 0.5f);
            lingqiParent.transform.SetParent(SpriteIconMgr.Instance.LingqiParent);
            lingqiParent.transform.localPosition = Vector3.zero;
            lingqiParent.transform.localScale = Vector3.one;
            return lingqiParent.transform;
        }
        return lingqiParent.transform;
    }   
    IEnumerator AddLingqi()
    {
        //先播放特效
        //再播放灵气
        AddLqPosList();   
        if(LqPosList.Count>0)
        {
            Vector3 pos = LqPosList[Random.Range(0, LqPosList.Count)];
            LqPosList.Remove(pos);
            AddLqEffect(pos);
            yield return new WaitForSeconds(5.0f);
            GameObject obj = AddCoin(pos);
            AddCoinBind(obj);
            isLoadingLq = false;
            lingqiTotalCount++;
        }              
    }
    //当灵气位置点不存在时，动态添加
    void AddLqPosList()
    {
        if (LqPosList.Count == 0)
        {
            for (int i = 0; i < SpriteIconMgr.Instance.LqPosList.Count; i++)
                LqPosList.Add(SpriteIconMgr.Instance.LqPosList[i].localPosition);

            for(int i= LqList.Count-1; i>=0;i--)
            {
                for (int j = LqPosList.Count - 1; j >= 0; j--)
                {
                    if (LqPosList[j].Equals(LqList[i].GetComponent<coinIcon>().GetInitLocalPostion()))
                    {
                        //Debug.LogError("移除个数");
                        LqPosList.RemoveAt(j);
                    }
                }                 
            }
        }

       
    }
    void AddLqEffect(Vector3 pos)//添加灵气漩涡,注意层是spui层
    {
        GameObject obj = ABResMgr.Instance.LoadResource<GameObject>(SpWindowPath.Instance.ui_public_effect_lqqcx, ABCommonConfig.Instance.SpWindowBundleType, true, false);
        if(SpriteIconMgr.Instance.GetSpriteData()!=null)
        {
            if (spData.Name.Equals(SpriteIconMgr.Instance.GetSpriteData().Name))
            {
                obj.SetActive(true);
                //Debug.LogError("添加灵气特效:");
            }
            else
            {
                obj.SetActive(false);
                // Debug.LogError("添加灵气特效:");
            }
        }
       
        obj.transform.parent = getLingqiParent();
        obj.transform.localPosition = pos;
        obj.transform.localScale =Vector3.one;
        foreach (Transform tran in obj.GetComponentsInChildren<Transform>())
        {
            tran.gameObject.layer = LayerMask.NameToLayer("SpUI");
        }
        obj.gameObject.tag = "meshButton";
        BoxCollider box=obj.AddComponent<BoxCollider>();
        box.size = 100 * Vector3.one;
        MeshButton meshButton = obj.gameObject.AddComponent<MeshButton>();

        meshButton.AddMeshEvent(AddLqEfClick);
        GameObject.Destroy(obj, 7);
        LqEfList.Add(obj);
    }
    void AddLqEfClick()
    {
        Spwindow.PlayCharacterAudio("xwk_jlej_20");
    }

    GameObject AddCoin(Vector3 pos)//添加金币
    {
        GameObject obj = ABResMgr.Instance.LoadResource<GameObject>(SpWindowPath.Instance.coinPath, ABCommonConfig.Instance.SpWindowBundleType, true, false);
        obj.SetActive(false);
        obj.transform.SetParent(getLingqiParent());
        obj.transform.localPosition = pos;
        obj.transform.localScale = Vector3.one;
        LqList.Add(obj);
        return obj;
    }

    void AddCoinBind(GameObject obj)//添加灵币事件
    {
        coinIcon coin = obj.GetComponent<coinIcon>();
        coin.InitCoinState(this);     
    }
    #endregion

    #region##灵气状态接口
    public void RefreshSpData()
    {
        if (spData != null)
            spData.Refresh();
    }
    public void ClearLingqi()//清空灵气
    {
        timer = 0;
        lingqiTotalCount = 0;
        LqPosList.Clear();
        LqList.Clear();
        LqEfList.Clear();
        lingqiParent = null;
    }
    public void ShowLingqi()//显示灵气
    {
        for (int i = 0; i < LqList.Count; i++)
            if(LqList[i])
            LqList[i].SetActive(true);      
    }
    public void CloseLingqi()//关闭 灵气币和漩涡特效
    {
        for (int i = 0; i < LqList.Count; i++)
            if(LqList[i])
            LqList[i].SetActive(false);
        for (int i = 0; i < LqEfList.Count; i++)
            if (LqEfList[i])
                LqEfList[i].SetActive(false);
    }
    public void RemoveLingqi(GameObject obj)
    {
        lingqiTotalCount--;
        if (lingqiTotalCount <0)
            lingqiTotalCount = 0;
        LqList.Remove(obj);
    }
    //判断当前灵气最大值
    public bool isFull()
    {
        if (spData.spLvel > 3)
        {
            //Debug.LogError("账号等级超标");
        }
        if (lingqiTotalCount >= GetMaxlq[spData.spLvel])
        {
            spData.fullLingqi = true;
            return true;
        }
        else
        {
            spData.fullLingqi = false;
            return false;
        }
    }
    #endregion


    //时间限制
    void GetLimit()
    {
        if (Application.isEditor)
        {
            timeScale = 1;
            if (spData.spLvel >= 3)//3级  20S  30S 45S 60S
            {
                limtTimer = 2 * timeScale;
                limtCount = 6;
            }
            else if (spData.spLvel >= 2)
            {
                limtTimer = 3 * timeScale;
                limtCount = 6;
            }
            else if (spData.spLvel >= 1)
            {
                limtTimer = 4.5f * timeScale;
                limtCount = 4;
            }
            else if (spData.spLvel >= 0)
            {
                limtTimer = 6 * timeScale;
                limtCount = 2;
            }
        }
        else
        {
            if (spData.spLvel >= 3)//3级  20S  30S 45S 60S
            {
                limtTimer = 2 * timeScale;
                limtCount = 6;
            }
            else if (spData.spLvel >= 2)
            {
                limtTimer = 3*timeScale;
                limtCount = 6;
            }
            else if (spData.spLvel >= 1)
            {
                limtTimer = 4.5f * timeScale;
                limtCount = 4;
            }
            else if (spData.spLvel >= 0)
            {
                limtTimer = 6 * timeScale;
                limtCount = 2;
            }
        }
      
    }
}
