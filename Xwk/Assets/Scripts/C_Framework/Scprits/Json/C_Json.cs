using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_Json
    {
        //判断jsondata里面是否含有key
        public static bool JsonDataContainsKey(JsonData data, string key)
        {
            if (null == data)
                return false;

            if (!data.IsObject)
                return false;

            IDictionary tdictionary = data as IDictionary;
            if (tdictionary == null)
                return false;

            if (tdictionary.Contains(key))
                return true;

            return false;
        }

        //获取json字符串的key值
        public static string GetJsonKeyString(string strData, string key)
        {
            if (string.IsNullOrEmpty(strData))
                return "";

            JsonData jd = JsonMapper.ToObject(strData);
            return GetJsonKeyString(jd, key);
        }

        public static string GetJsonKeyString(JsonData data, string key)
        {
            if (JsonDataContainsKey(data, key))
                return (string)data[key];

            return "";
        }

        public static string GetJsonKeyString(string strData, string key1, string key2)
        {
            if (string.IsNullOrEmpty(strData))
                return "";

            JsonData jd = JsonMapper.ToObject(strData);

            return GetJsonKeyString(jd, key1, key2);
        }

        public static string GetJsonKeyString(JsonData data, string key1, string key2)
        {
            if (JsonDataContainsKey(data, key1))
            {
                JsonData data2 = data[key1];
                if (JsonDataContainsKey(data2, key2))
                    return (string)data2[key2];
            }

            return "";
        }

        public static JsonData GetJsonKeyJsonData(string strData, string key)
        {
            if (string.IsNullOrEmpty(strData))
                return null;

            JsonData jd = JsonMapper.ToObject(strData);
            return GetJsonKeyJsonData(jd, key);
        }

        public static JsonData GetJsonKeyJsonData(JsonData data, string key)
        {
            if (JsonDataContainsKey(data, key))
                return data[key];

            return null;
        }

        public static JsonData GetJsonKeyJsonData(string strData, string key1, string key2)
        {
            if (string.IsNullOrEmpty(strData))
                return null;

            JsonData jd = JsonMapper.ToObject(strData);
            return GetJsonKeyJsonData(jd, key1, key2);
        }

        public static JsonData GetJsonKeyJsonData(JsonData data, string key1, string key2)
        {
            if (JsonDataContainsKey(data, key1))
            {
                JsonData data2 = data[key1];
                if (JsonDataContainsKey(data2, key2))
                    return data2[key2];
            }

            return null;
        }

        public static int GetJsonKeyInt(string strData, string key)
        {
            if (string.IsNullOrEmpty(strData))
                return -1;

            JsonData jd = JsonMapper.ToObject(strData);
            return GetJsonKeyInt(jd, key);
        }

        public static int GetJsonKeyInt(JsonData data, string key)
        {
            if (JsonDataContainsKey(data, key))
                return (int)data[key];

            return -1;
        }

        public static int GetJsonKeyInt(string strData, string key1, string key2)
        {
            if (string.IsNullOrEmpty(strData))
                return -1;

            JsonData jd = JsonMapper.ToObject(strData);
            return GetJsonKeyInt(jd, key1, key2);
        }

        public static int GetJsonKeyInt(JsonData data, string key1, string key2)
        {
            if (JsonDataContainsKey(data, key1))
            {
                JsonData data2 = data[key1];
                if (JsonDataContainsKey(data2, key2))
                    return (int)data2[key2];
            }

            return -1;
        }

        public static float GetJsonKeyFloat(string strData, string key)
        {
            if (string.IsNullOrEmpty(strData))
                return 0;

            JsonData jd = JsonMapper.ToObject(strData);
            return GetJsonKeyFloat(jd, key);
        }

        public static float GetJsonKeyFloat(JsonData data, string key)
        {
            try
            {
                 
                if (JsonDataContainsKey(data, key))
                    return (float)(double)data[key];
            }
            catch(System.Exception e)
            {
                Debug.Log("System.Exception e ："+e);
            }
            finally
            {
                 
            }


            return 0;
        }

        public static double GetJsonKeyDouble(string strData, string key)
        {
            if (string.IsNullOrEmpty(strData))
                return 0;

            JsonData jd = JsonMapper.ToObject(strData);
            return GetJsonKeyDouble(jd, key);
        }

        public static double GetJsonKeyDouble(JsonData data, string key)
        {
            if (JsonDataContainsKey(data, key))
                return (double)data[key];

            return 0;
        }

        public static bool GetJsonKeyBool(string strData, string key)
        {
            if (string.IsNullOrEmpty(strData))
                return false;

            JsonData jd = JsonMapper.ToObject(strData);
            return GetJsonKeyBool(jd, key);
        }

        public static bool GetJsonKeyBool(JsonData data, string key)
        {
            string strData = "";

            if (JsonDataContainsKey(data, key))
                strData = (string)data[key];

            if (strData == "true")
                return true;

            return false;
        }
    }
}