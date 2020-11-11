using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using LitJson;
/// <summary>
/// Json读取类，后期可以优化，不用每次进入读取,存入缓存
/// </summary>
public class JsonManager : C_MonoSingleton<JsonManager>{

    LitJson.JsonData localdata=null;
    Dictionary<string, JsonData> JsonCacheDic = new Dictionary<string, JsonData>();

    public void ReadJson(string configPath, Action<LitJson.JsonData> ac)
    {
        string name = configPath.Replace('.', '_');
        if (JsonCacheDic.ContainsKey(name))
        {
            if(ac!=null)
            ac(JsonCacheDic[name]);
        }
        else
        {
            StartCoroutine(ReadJsonData(configPath, ac));
        }
    }
    IEnumerator ReadJsonData(string configPath, Action<LitJson.JsonData> ac)// Config/Sprit/Sprit.json
    {
        string path;
        //读取内置的配置表文件，异步读取方式是因为IO读取路径出现问题
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            path = Application.dataPath + "/StreamingAssets/"+ configPath;//读取数据，转换成数据流
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            path = Application.streamingAssetsPath + "/"+configPath;//读取数据，转换成数据流
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            path = "file://" + Application.streamingAssetsPath + "/"+configPath;//读取数据，转换成数据流
        }
        else
        {
            path = "file://" + Application.streamingAssetsPath + "/"+configPath;//读取数据，转换成数据流
        }
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();
        if (!request.isHttpError)
        {
            localdata = LitJson.JsonMapper.ToObject(request.downloadHandler.text);            
            if(ac!=null)
            {
                string name = configPath.Replace('.','_');
                JsonCacheDic.Add(name, localdata);
                if(ac!=null)
                ac(localdata);
            }
        }
        else
        {
            //Debug.LogError("read sprite json error");
        }     
    }
    public LitJson.JsonData GetNowJsonData()
    {
        return localdata;
    }
}
