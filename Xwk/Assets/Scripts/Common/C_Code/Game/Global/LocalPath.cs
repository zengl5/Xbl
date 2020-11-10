using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 本地路径
public class LocalPath
{
    public static string StreamingAssetsPath { get { return Application.streamingAssetsPath + "/c_framework/" + GameHotUpdateMgr.CommonChannel + "/"; } }

#if UNITY_EDITOR
    public static string HotUpdatePath { get { return Application.dataPath + "/../DownloadFiles/" + GameHotUpdateMgr.CommonChannel + "/"; } }
#else
    public static string HotUpdatePath{ get { return Application.persistentDataPath + "/c_framework/" + GameHotUpdateMgr.CommonChannel + "/"; } }
#endif

    public static string ServerHotUpdateConfigPath { get { return GameDataMgr.c_HotUpdate + GameHotUpdateMgr.CommonChannel + "/" + GameHotUpdateMgr.HotUpdateConfig + "/"; } }
    public static string LocalHotUpdateConfigPath { get { return HotUpdatePath + GameHotUpdateMgr.HotUpdateConfig + "/"; } }
    public static string StreamingAssetsHotUpdateConfigPath { get { return StreamingAssetsPath + GameHotUpdateMgr.HotUpdateConfig + "/"; } }
    


    //每个游戏独有的

    public const string StagePath = "stage/";
    

#if UNITY_IPHONE
    public const string PackagingResources = "packaging_resources_ios";
 
#elif UNITY_EDITOR
    public const string PackagingResources = "packaging_resources_windows";

#elif UNITY_ANDROID
    public const string PackagingResources = "packaging_resources_android";

#else
    public const string PackagingResources = "packaging_resources_windows";
        
#endif
    
    public static string LocalPackagingResources { get { return HotUpdatePath + PackagingResources + "/"; } }
    public static string StreamingAssetsPackagingResources { get { return StreamingAssetsPath + PackagingResources + "/"; } }

    public const string AvatarResourcesPath = "Game/Avatar/";
    public const string LevelResourcesPath = "Game/Level/";
    public const string StageResourcesPath = "Game/Stage/";
    public const string WordResourcesPath = "Game/Word/";
    public const string WordDescResourcesPath = "Game/WordDesc/";
}