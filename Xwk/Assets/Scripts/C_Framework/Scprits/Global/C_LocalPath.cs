using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 本地路径
public class C_LocalPath
{
    public static string StreamingAssetsConfigPath = Application.streamingAssetsPath + "/c_framework/config/";
    public static string StreamingAssetsDataPath = Application.streamingAssetsPath + "/c_framework/data/";

#if UNITY_EDITOR
    public static string DataPath { get { return Application.dataPath + "/../DownloadFiles/data/"; } }
#else
    public static string DataPath { get { return Application.persistentDataPath + "/c_framework/data/"; } }
#endif
    
    public const string PackagingResources_ios = "packaging_resources_ios";
    public const string PackagingResources_android = "packaging_resources_android";
    public const string PackagingResources_windows = "packaging_resources_windows";
}