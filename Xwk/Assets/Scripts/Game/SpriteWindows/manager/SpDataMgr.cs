using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
/// <summary>
/// 精灵数据管理类
/// </summary>
public class SpDataMgr : MonoSingleton<SpDataMgr> {
    SpData spData = new SpData();//数据获取
    public List<SpriteData> SpDataList = new List<SpriteData>();
    Action ac;   

    public void ReadJsonData(string path,Action ac)
    {
        this.ac = ac;
        JsonManager.Instance.ReadJson(path, NewUserReadJsonData);
    }
    void NewUserReadJsonData(LitJson.JsonData data)
    {
        SpDataList.Clear();
        spData.ReadJsonData(data, ReadJsonDataFinish);
    } 
    void ReadJsonDataFinish()
    {
        SpDataList = spData.SpDataList;     
        if (ac != null)
            ac();
    }
    //是否上新
    public bool IsGetNewSprite()
    {
        return spData.IsGetNewSprite();
    }
    //设置已经上新
    public void SetGetSprite()
    {
        spData.SetGetSprite();
    }

    //获取所有灵气满了或者离线收益满了
    public bool GetLingqiFullOrGainFull()
    {
        if(SpriteLingqiMgr.Instance.IsFullAllLingqi()|AllGainIsFull())
        {
            //Debug.LogError("IsFullAll_lingqi"+SpriteLingqiMgr.Instance.IsFullAllLingqi());
            //Debug.LogError("AllGainIsFull"+AllGainIsFull());
            return true;
        }      
        else
        {
            return false;
        }
    }
    bool AllGainIsFull()
    {
        return DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Game_byjl) && DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Story_Hulu)
            && DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Story_Byjl) && DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Story_Hulu) && DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Story_Ln);
    }
 
}
