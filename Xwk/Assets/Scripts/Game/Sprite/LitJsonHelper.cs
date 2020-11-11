using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;
using System;
namespace Xbl
{
    public class LitJsonHelper
    {
        //首页收集精灵，数据管理 
        public static void AddSpritData(string CardName, string EffectSoundName, string CardSoundName,string CardRealName,string jsonpath = null)
        {
            string path = Application.dataPath + "/StreamingAssets/Sprit.json";//读取数据，转换成数据流
            JsonData data = GetJsonData(path);
            //首页获取 精灵写入数据
            JsonData writeData = new JsonData();
            writeData["CardName"] = CardName;
            writeData["EffectSoundName"] = EffectSoundName;
            writeData["CardSoundName"] = CardSoundName;
            writeData["CardRealName"] = CardRealName;
            WriteDatoToJson(data["SpritList"], writeData, data, path);
        }

        public static JsonData GetJsonData(string path)
        {
            FileStream fsRead = File.Open(path, FileMode.OpenOrCreate);
            using (StreamReader strRead = new StreamReader(fsRead, Encoding.UTF8))
            {
                JsonData data = JsonMapper.ToObject(strRead);                strRead.Close();
              
                return data;
            }
        }
    
         
        /// <summary>
        /// 写入数据到json文本
        /// </summary>
        /// <param name="Json">json对象</param>
        /// <param name="data">具体数据信息</param>
        /// <param name="path">路径</param>
        public static void WriteDatoToJson(JsonData Json, JsonData data, JsonData TotalJson, string path)
        {
            Json.Add(data);
            string json = TotalJson.ToJson();
            Debug.LogError(json);
            StreamWriter sw = new StreamWriter(path);
            sw.Write(json);
            sw.Close();
            sw.Dispose();
        }
    }
}
 
