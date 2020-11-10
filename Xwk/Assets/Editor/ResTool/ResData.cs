using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ResItem
{
   public string name;
   public string path; 
   public string resType;
    public int overriden;
    /// <summary>
    /// texture
    /// </summary>
   public int textureType;
   public int readEnable;
   public int gengrateMipMaps;
   public string maxSize;
   public int format;
    /// <summary>
    /// audioclip
    /// </summary>
    public int loadtype;
    public int compressionFormat;
    public string quality;
    public int sampleRateSet;
}
public class ResData  {
    public static string Name = "collect_res_config";
    public static List<ResItem> _ResList = new List<ResItem>();
	public static void Load()
    {
      //  string data = C_DataMgr.Instance.LoadData(Name);
        string data = C_Save.LoadString(Name, Application.dataPath + "/../Config/");
        if (!string.IsNullOrEmpty(data))
        {
            UpdateData(data);
        }
    }
    public static void UpdateData(string data)
    {
        JsonData jsonData = C_Json.GetJsonKeyJsonData(data, "_ResList");
        if (jsonData != null)
        {
            _ResList.Clear();
            for (int i = 0; i < jsonData.Count; i++)
            {
                ResItem resItem = new ResItem();
                resItem.name = C_Json.GetJsonKeyString(jsonData[i], "name");
                resItem.path = C_Json.GetJsonKeyString(jsonData[i], "path");
                resItem.resType = C_Json.GetJsonKeyString(jsonData[i], "resType");
               // resItem.overriden = C_Json.GetJsonKeyBool(jsonData[i], "overriden");
                if (string.IsNullOrEmpty(resItem.resType))
                {
                    continue;
                }
                else if (resItem.resType.Equals("texture"))
                {
                    resItem.textureType = C_Json.GetJsonKeyInt(jsonData[i], "textureType");
                    resItem.readEnable = C_Json.GetJsonKeyInt(jsonData[i], "readEnable")  ;
                    resItem.gengrateMipMaps = C_Json.GetJsonKeyInt(jsonData[i], "gengrateMipMaps")  ;
                    resItem.maxSize = C_Json.GetJsonKeyString(jsonData[i], "maxSize");
                    resItem.format = C_Json.GetJsonKeyInt(jsonData[i], "format");
                }
                else if (resItem.resType.Equals("audioclip"))
                {
                    resItem.loadtype = C_Json.GetJsonKeyInt(jsonData[i], "loadtype");
                    resItem.compressionFormat = C_Json.GetJsonKeyInt(jsonData[i], "compressionFormat");
                    resItem.quality = C_Json.GetJsonKeyString(jsonData[i], "quality");
                    resItem.sampleRateSet = C_Json.GetJsonKeyInt(jsonData[i], "sampleRateSet");
                }
               
                _ResList.Add(resItem);
            }
        }
    }
    public static void AddRestItem(ResItem item )
    {
        if (item == null)
        {
            return;
        }
        for (int i  = 0; i< _ResList.Count;i++)
        {
            if (_ResList[i].path.Equals(item.path))
            {
                _ResList[i] = item;
                return;
            }
        }
        _ResList.Add(item);
    }
    public static ResItem FetchItem(string path)
    {
        ResItem item = null ;

        for (int i = 0; i < _ResList.Count; i++)
        {
            if (_ResList[i].path.Equals(path))
            {
                item =  _ResList[i] ;
                break;
            }
        }
        return item;
    }
    public static void Save()
    {
       C_Save.SaveString(Name, Application.dataPath + "/../Config/", JsonMapper.ToJson(new ResData()), "");
    }

}
