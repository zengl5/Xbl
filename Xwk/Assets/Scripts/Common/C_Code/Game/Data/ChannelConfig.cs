using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChannelItem
{
    public string Key = "";
    public string Value = "";
    public string Channel = "";
}

public class ChannelConfig
{
    public static string Name = "channel_config";

    public static string AppVersion = "";
    public static string Channel = "";

    public static List<ChannelItem> ChannelItemList = new List<ChannelItem>();

    public static void Load()
    {
        string strData = C_MonoSingleton<C_DataMgr>.GetInstance().LoadData(Name);
        if (!string.IsNullOrEmpty(strData))
        {
            AppVersion = C_Json.GetJsonKeyString(strData, "AppVersion");
            Channel = C_Json.GetJsonKeyString(strData, "Channel");

            JsonData channelItemListJD = C_Json.GetJsonKeyJsonData(strData, "ChannelItemList");
            if (channelItemListJD != null)
            {
                for (int i = 0; i < channelItemListJD.Count; i++)
                {
                    if (channelItemListJD[i] != null)
                    {
                        ChannelItem item = new ChannelItem();

                        item.Key = C_Json.GetJsonKeyString(channelItemListJD[i], "Key");
                        item.Value = C_Json.GetJsonKeyString(channelItemListJD[i], "Value");
                        item.Channel = C_Json.GetJsonKeyString(channelItemListJD[i], "Channel");

                        ChannelItemList.Add(item);
                    }
                }
            }
        }
    }

    public static void Save(string strData)
    {
        if (!string.IsNullOrEmpty(strData))
        {
            AppVersion = GameConfig.AppVersion;
            Channel = GameDataMgr.c_Channel;

            ChannelItemList.Clear();

            JsonData configJD = JsonMapper.ToObject(strData);
            if (configJD != null)
            {
                int i = 0;
                while (i <= configJD.Count-1)
                {
                    JsonData item = configJD[i]; 
                    if (item == null)
                        break;

                    if (C_Json.GetJsonKeyInt(item, "state") == 1)
                    {
                        ChannelItem channelItem = new ChannelItem();

                        channelItem.Key = C_Json.GetJsonKeyString(item, "key");
                        channelItem.Value = C_Json.GetJsonKeyString(item, "value");
                        channelItem.Channel = C_Json.GetJsonKeyString(item, "channel");

                        ChannelItemList.Add(channelItem);
                    }

                    i++;
                }
            }

            C_MonoSingleton<C_DataMgr>.GetInstance().SaveData(Name, JsonMapper.ToJson(new ChannelConfig()));
        }
    }
    public static string FetchChannel(string key)
    {
        for (int i = 0; i < ChannelItemList.Count; i++)
        {
            if (ChannelItemList[i].Key == key)
                return ChannelItemList[i].Channel;
        }
        return "";

    }
    public static string GetValue(string key)
    {
        for (int i = 0; i < ChannelItemList.Count; i++)
        {
            if (ChannelItemList[i].Key == key)
                return ChannelItemList[i].Value;
        }

        return "";
    }
    public static bool CompareVersionAndChannel(string key)
    {
        string version = GetValue(key);
        string channel = FetchChannel(key);
        if (string.IsNullOrEmpty(AppVersion) || string.IsNullOrEmpty(Channel) || string.IsNullOrEmpty(channel))
        {
            return false;
        }
        if (Channel.Equals(channel) && IsChannel(version)) {
            return true;
        }
        return false;
    }
    private static bool IsChannel(string version)
    {
        if(string.IsNullOrEmpty(version))
        {
            return false;
        }
        string[] data = version.Split(',');
        for (int i =0;i < data.Length;i++)
        {
            if (data[i].Equals(AppVersion))
            {
                return true;
            }
        }
        return false;
    }
}