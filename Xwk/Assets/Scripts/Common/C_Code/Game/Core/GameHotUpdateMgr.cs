using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotUpdateItem
{
    public string Name;
    public string MD5;
}

public static class GameHotUpdateMgr
{
    public const string HotUpdateConfig = "hot_update_config";
    public const string CommonChannel = "xblpy_common";

    private static string m_ServerHotUpdateConfig = "";
    public static string ServerHotUpdateConfig
    {
        get
        {
            if (string.IsNullOrEmpty(m_ServerHotUpdateConfig))
            {
                if (NetworkMgr.IsConnected)
                {
                    m_ServerHotUpdateConfig = C_DownloadMgr.GetUrlString(LocalPath.ServerHotUpdateConfigPath + HotUpdateConfig + "@" + CommonChannel + "@" + GameConfig.AppVersion);
                    if (!string.IsNullOrEmpty(m_ServerHotUpdateConfig))
                    {
                        PlayerPrefs.SetString(HotUpdateConfig, m_ServerHotUpdateConfig);
                        PlayerPrefs.Save();
                    }
                }
                else
                {
                    m_ServerHotUpdateConfig = PlayerPrefs.GetString(HotUpdateConfig);
                }
            }

            return m_ServerHotUpdateConfig;
        }
    }

    public static int GetStageHotUpdateStatus(string stageName)
    {
        return C_MonoSingleton<GameStageHotUpdateMgr>.GetInstance().GetStageHotUpdateState(stageName);
    }

    public static void DownloadStageHotUpdate(string stageName, Action action)
    {
        C_MonoSingleton<GameStageHotUpdateMgr>.GetInstance().DownloadStageHotUpdate(stageName, action);
    }

    public static List<HotUpdateItem> ParseHotUpdateConfig(string config)
    {
        List<HotUpdateItem> list = new List<HotUpdateItem>();

        //解析
        if (!string.IsNullOrEmpty(config))
        {
            JsonData configJD = C_Json.GetJsonKeyJsonData(config, "config");
            if (configJD != null)
            {
                foreach (JsonData itemJD in configJD)
                {
                    HotUpdateItem item = new HotUpdateItem();
                    item.Name = C_Json.GetJsonKeyString(itemJD, "name");
                    item.MD5 = C_Json.GetJsonKeyString(itemJD, "md5");

                    list.Add(item);
                }
            }
        }

        return list;
    }
}
