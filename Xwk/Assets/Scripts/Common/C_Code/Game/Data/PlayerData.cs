using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public static string Name = "player_data";

    public static string Token = "";
    public static string UID = "";
    public static string BabyGender = "0";
    public static string BabyName = "";
    public static string BabyNameMP3 = "";
    public static string BabyBirthday = "2013-06-01";
    public static string HeadImg = "";
    public static string Phone = "";
    public static string WeChatUnionID = "";

    public static int LearningRhythm = 1;
    public static int RestSpan = 1800;
    public static int RestTime = 180;
    public static string WakeUpTime = "06:30";
    public static string SleepTime = "21:30";

    public static int StarCount = 0;
    public static int IsVIP = 0;

    public static void Load()
    {
        string strData = C_MonoSingleton<C_DataMgr>.GetInstance().LoadData(Name);

        if (string.IsNullOrEmpty(strData))
            strData = C_MonoSingleton<C_DataMgr>.GetInstance().LoadData_Old(Name);

        if (!string.IsNullOrEmpty(strData))
        {
            Token = C_Json.GetJsonKeyString(strData, "Token");
            UID = C_Json.GetJsonKeyString(strData, "UID");
            BabyGender = C_Json.GetJsonKeyString(strData, "BabyGender");
            BabyName = C_Json.GetJsonKeyString(strData, "BabyName");
            InitNickName();
            BabyNameMP3 = C_Json.GetJsonKeyString(strData, "BabyNameMP3");
            BabyBirthday = C_Json.GetJsonKeyString(strData, "BabyBirthday");
            HeadImg = C_Json.GetJsonKeyString(strData, "HeadImg");
            Phone = C_Json.GetJsonKeyString(strData, "Phone");

            RestSpan = C_Json.GetJsonKeyInt(strData, "RestSpan");
            RestTime = C_Json.GetJsonKeyInt(strData, "RestTime");
            WakeUpTime = C_Json.GetJsonKeyString(strData, "WakeUpTime");
            SleepTime = C_Json.GetJsonKeyString(strData, "SleepTime");

            WeChatUnionID = C_Json.GetJsonKeyString(strData, "WeChatUnionID");

            StarCount = C_Json.GetJsonKeyInt(strData, "StarCount");
            IsVIP = C_Json.GetJsonKeyInt(strData, "IsVIP");
        }
        else
        {
            InitNickName();
        }
    }
    public static void InitNickName()
    {
        if (string.IsNullOrEmpty(BabyName))
        {
            BabyName = C_Localization.GetLocalization("LOACAL_DEFAULT_BABY_NAME"); 
        }
    }
    public static void Save()
    {
        C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new PlayerData()));
    }
}