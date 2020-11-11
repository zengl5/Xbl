using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_DataMgr : C_MonoSingleton<C_DataMgr>
    {
        public const string c_Password = "cdatamima";

        public class C_DataItem
        {
            public string Name = "";
            public byte[] Data;
            public float DealyTime = 2.0f;
            public bool HavePassword = false;

            public C_DataItem(string name, byte[] data, bool havePassword, float dealyTime = 2.0f)
            {
                this.Name = name;
                this.Data = data;
                this.HavePassword = havePassword;
                this.DealyTime = dealyTime;
            }
        }

        private List<C_DataItem> m_DirtyDataList = new List<C_DataItem>();

        void Update()
        {
            for (int i = m_DirtyDataList.Count - 1; i >= 0; i--)
            {
                if (m_DirtyDataList[i].DealyTime <= 0)
                {
                    RealySaveData(m_DirtyDataList[i]);

                    m_DirtyDataList.RemoveAt(i);
                }
                else
                {
                    m_DirtyDataList[i].DealyTime -= Time.deltaTime;
                }
            }
        }

        public void SaveData<T>(string name, T obj)
        {
            if (string.IsNullOrEmpty(name) || obj == null)
                return;

            byte[] data = C_Save.ConvertByte<T>(obj);

            SaveDataByte(name, data, false);
        }

        public void SaveData(string name, string strData)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(strData))
                return;

            SaveDataByte(name, Encoding.UTF8.GetBytes(strData), true);
        }

        public void SaveDataByte(string name, byte[] data, bool havePassword)
        {
            if (string.IsNullOrEmpty(name) || data == null)
                return;

            for (int i = 0; i < m_DirtyDataList.Count; i++)
            {
                if (m_DirtyDataList[i].Name == name)
                {
                    m_DirtyDataList[i].Data = data;
                    m_DirtyDataList[i].HavePassword = havePassword;

                    return;
                }
            }

            m_DirtyDataList.Add(new C_DataItem(name, data, havePassword));
        }

        private void RealySaveData(C_DataItem dataItem)
        {
            if (dataItem.HavePassword)
                C_Save.SaveByte(dataItem.Name, C_LocalPath.DataPath, dataItem.Data, c_Password);
            else
                C_Save.SaveByte(dataItem.Name, C_LocalPath.DataPath, dataItem.Data);
        }
        

        public T LoadData<T>(string name, T defaultValue)
        {
            for (int i = 0; i < m_DirtyDataList.Count; i++)
            {
                if (m_DirtyDataList[i].Name == name)
                    return C_Save.Load<T>(name, m_DirtyDataList[i].Data, defaultValue);
            }

            return C_Save.Load<T>(name, C_LocalPath.DataPath, defaultValue);
        }

        public string LoadData(string name)
        {
            byte[] data = LoadDataByte(name);
            if (data == null)
                data = LoadStreamingAssetsDataByte(name);

            if (data != null)
                return Encoding.UTF8.GetString(data);

            return "";
        }

        public byte[] LoadDataByte(string name)
        {
            for (int i = 0; i < m_DirtyDataList.Count; i++)
            {
                if (m_DirtyDataList[i].Name == name)
                    return m_DirtyDataList[i].Data;
            }

            return C_Save.LoadByte(name, C_LocalPath.DataPath, c_Password);
        }

        public byte[] LoadStreamingAssetsDataByte(string name)
        {
            for (int i = 0; i < m_DirtyDataList.Count; i++)
            {
                if (m_DirtyDataList[i].Name == name)
                    return m_DirtyDataList[i].Data;
            }

            return C_Save.LoadByte(name, C_LocalPath.StreamingAssetsDataPath, c_Password);
        }

        //兼容老数据
        public string LoadData_Old(string name)
        {
            string path = Application.persistentDataPath + "/c_framework/normal/data/";

            byte[] data = C_Save.LoadByte(name, path, c_Password);

            if (data != null)
                return Encoding.UTF8.GetString(data);

            return "";
        }
    }
}