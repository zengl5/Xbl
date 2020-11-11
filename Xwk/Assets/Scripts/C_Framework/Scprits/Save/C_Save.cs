using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.Networking;

namespace Assets.Scripts.C_Framework
{
    public enum C_EnumSaveSerializer
    {
        SaveBinarySerializer,
        SaveJsonSerializer,
        SaveXmlSerializer
    }

    public static class C_Save
    {
        private static string m_strDefaultPassword = "";
        private static C_EnumSaveSerializer m_DefaultSerializer = C_EnumSaveSerializer.SaveBinarySerializer;
        private static C_ISaveEncoder m_DefaultEncoder = new C_SaveSimpleEncoder();
        private static Encoding m_DefaultEncoding = new UTF8Encoding(false);

        public static byte[] ConvertByte<T>(T obj)
        {
            return ConvertByte<T>(obj, m_DefaultSerializer);
        }

        public static byte[] ConvertByte<T>(T obj, C_EnumSaveSerializer enumSerializer)
        {
            if (obj == null)
            {
                Debug.LogError("C_Save ConvertByte Param Error!!");
                return null;
            }

            Stream stream = new MemoryStream();

            C_ISaveSerializer serializer = GetSaveSerializer(enumSerializer);

            serializer.Serialize(obj, stream, m_DefaultEncoding);

            byte[] bytes = new byte[stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, bytes.Length);

            stream.Close();

            return bytes;
        }

        public static void Save<T>(string resName, string resPath, T obj)
        {
            Save<T>(resName, resPath, obj, m_DefaultSerializer, m_DefaultEncoder, m_DefaultEncoding);
        }

        public static void Save<T>(string resName, string resPath, T obj, C_EnumSaveSerializer enumSerializer)
        {
            Save<T>(resName, resPath, obj, enumSerializer, m_DefaultEncoder, m_DefaultEncoding);
        }

        public static void Save<T>(string resName, string resPath, T obj, C_EnumSaveSerializer enumSerializer, C_ISaveEncoder encoder, Encoding encoding)
        {
            if (string.IsNullOrEmpty(resName) || obj == null || encoder == null || encoding == null)
            {
                Debug.LogError("C_Save Save Param Error!! resName = " + resName + ", resPath = " + resPath);
                return;
            }

            byte[] bytes = ConvertByte<T>(obj, enumSerializer);

            SaveByte(resName, resPath, bytes, "", encoder);
        }

        public static void SaveString(string resName, string resPath, string saveData, string password)
        {
            SaveString(resName, resPath, saveData, password, m_DefaultEncoder, m_DefaultEncoding);
        }

        public static void SaveString(string resName, string resPath, string saveData, string password, C_ISaveEncoder encoder, Encoding encoding)
        {
            if (string.IsNullOrEmpty(resName) || string.IsNullOrEmpty(saveData))
            {
                Debug.LogError("C_Save SaveString Param Error!! resName = " + resName + ", resPath = " + resPath);
                return;
            }

            SaveByte(resName, resPath, encoding.GetBytes(saveData), password, encoder);
        }

        public static void SaveByte(string resName, string resPath, byte[] saveData)
        {
            SaveByte(resName, resPath, saveData, m_strDefaultPassword, m_DefaultEncoder);
        }

        public static void SaveByte(string resName, string resPath, byte[] saveData, string password)
        {
            SaveByte(resName, resPath, saveData, password, m_DefaultEncoder);
        }

        public static void SaveByte(string resName, string resPath, byte[] saveData, string password, C_ISaveEncoder encoder)
        {
            if (saveData == null || saveData.Length <= 0)
            {
                Debug.LogError("C_Save Save saveData is Null or Empty!! resName = " + resName + ", resPath = " + resPath);
                return;
            }

            string filePath = resPath + resName;

            Debug.Log("C_Save Save filePath = " + filePath);

            Stream stream = new MemoryStream();

            if (!string.IsNullOrEmpty(password))
                saveData = encoder.Encode(saveData, password);

            if (!Directory.Exists(resPath))
                Directory.CreateDirectory(resPath);

            FileStream fs = new FileStream(filePath, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(saveData);
            bw.Close();
            fs.Close();
            stream.Close();
        }

        public static T Load<T>(string resName, string resPath, T defaultValue)
        {
            byte[] data = LoadByte(resName, resPath, m_strDefaultPassword, m_DefaultEncoder, m_DefaultEncoding);

            return Load<T>(resName, data, defaultValue);
        }

        public static T Load<T>(string resName, byte[] resData, T defaultValue)
        {
            return Load<T>(resName, resData, defaultValue, m_DefaultSerializer, m_DefaultEncoder, m_DefaultEncoding);
        }

        public static T Load<T>(string resName, byte[] resData, T defaultValue, C_EnumSaveSerializer enumSerializer, C_ISaveEncoder encoder, Encoding encoding)
        {
            if (resData == null)
                return defaultValue;

            Stream stream = new MemoryStream(resData, true);

            C_ISaveSerializer serializer = GetSaveSerializer(enumSerializer);

            T result = serializer.Deserialize<T>(stream, encoding);

            stream.Close();

            return result;
        }

        public static string LoadString(string resName, string resPath)
        {
            return LoadString(resName, resPath, m_strDefaultPassword, m_DefaultEncoder, m_DefaultEncoding);
        }

        public static string LoadString(string resName, string resPath, string password)
        {
            return LoadString(resName, resPath, password, m_DefaultEncoder, m_DefaultEncoding);
        }

        public static string LoadString(string resName, string resPath, string password, C_ISaveEncoder encoder, Encoding encoding)
        {
            byte[] data = LoadByte(resName, resPath, password, encoder, encoding);
            if (data != null)
                return encoding.GetString(data);

            return "";
        }

        public static byte[] LoadByte(string resName, string resPath)
        {
            return LoadByte(resName, resPath, m_strDefaultPassword, m_DefaultEncoder, m_DefaultEncoding);
        }

        public static byte[] LoadByte(string resName, string resPath, string password)
        {
            return LoadByte(resName, resPath, password, m_DefaultEncoder, m_DefaultEncoding);
        }

        public static byte[] LoadByte(string resName, string resPath, string password, C_ISaveEncoder encoder, Encoding encoding)
        {
            if (string.IsNullOrEmpty(resName) || encoder == null || encoding == null)
            {
                if (Application.isPlaying)
                    Debug.LogError("C_Save LoadByte Param Error!! resName = " + resName + ", resPath = " + resPath);
                return null;
            }

            string filePath = resPath + resName;

            //Debug.Log("C_Save Load filePath = " + filePath);

            byte[] data = null;

            if (filePath.Contains("://"))
            {
                using (UnityWebRequest request = UnityWebRequest.Get(filePath))
                {
                    request.SendWebRequest();

                    while (!request.isDone)
                    {
                        if (request.isNetworkError)
                        {
                            Debug.LogWarning("C_Save LoadByte file is null!! filePath = " + filePath + ", request.isError" + request.isNetworkError);
                            return data;
                        }
                    }

                    if (request.isNetworkError)
                        Debug.LogWarning("C_Save LoadByte file is null!! filePath = " + filePath + ", request.isError" + request.isNetworkError);
                    else
                        data = request.downloadHandler.data;
                }
            }
            else
            {
                if (!File.Exists(filePath))
                    Debug.LogWarning("C_Save LoadByte file is null!! filePath = " + filePath);
                else
                    data = File.ReadAllBytes(filePath);
            }

            if (data != null && !string.IsNullOrEmpty(password))
                data = encoder.Decode(data, password);

            return data;
        }

        public static C_ISaveSerializer GetSaveSerializer(C_EnumSaveSerializer enumSerializer)
        {
            switch (enumSerializer)
            {
                case C_EnumSaveSerializer.SaveJsonSerializer:
                    return new C_SaveJsonSerializer();

                case C_EnumSaveSerializer.SaveXmlSerializer:
                    return new C_SaveXmlSerializer();

                default:
                    return new C_SaveBinarySerializer();
            }
        }
    }
}