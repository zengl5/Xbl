using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class GameConfig
{
    public static string Name = "game_config";

    public static string AppVersion = "1.4.4";
    public static string ResVersion = "1.0.0";
    public static int LogState = 0;
    public static int LocalResources = 0;
    public static int AutoTest = 0;
    public static int HYJJ_Test = 1;

    public static void Load()
    {
        string strData = C_MonoSingleton<C_DataMgr>.GetInstance().LoadData(Name);
        if (!string.IsNullOrEmpty(strData))
        {
            AppVersion = C_Json.GetJsonKeyString(strData, "AppVersion");
            ResVersion = C_Json.GetJsonKeyString(strData, "ResVersion");
            LogState = C_Json.GetJsonKeyInt(strData, "LogState");
            LocalResources = C_Json.GetJsonKeyInt(strData, "LocalResources");
            AutoTest = C_Json.GetJsonKeyInt(strData, "AutoTest");
            HYJJ_Test = C_Json.GetJsonKeyInt(strData, "HYJJ_Test");
        }
    }

    public static void Save()
    {
        C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new GameConfig()));
    }

    public static void SaveStreamingAssets()
    {
        C_Save.SaveByte(Name, C_LocalPath.StreamingAssetsDataPath, Encoding.UTF8.GetBytes(JsonMapper.ToJson(new GameConfig())), C_DataMgr.c_Password);
    }
}