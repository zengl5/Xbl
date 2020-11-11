using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_AssetBundleRef
    {
        public string FilePath = "";
        public int RefCount = 0;
        public int RefCacheCount = 0;
        public AssetBundle Bundle = null;

        public void AutoUnload()
        {
            if (--RefCount <= 0 && RefCacheCount <= 0)
                C_MonoSingleton<C_AssetBundleMgr>.GetInstance().Unload(this);
        }

        public void CacheUnload()
        {
            if (--RefCacheCount <= 0 && RefCount <= 0)
                C_MonoSingleton<C_AssetBundleMgr>.GetInstance().ForceCacheUnload(this);
        }

        public T LoadAsset<T>(string resName) where T : Object
        {
            return Bundle.LoadAsset<T>(resName);
        }
    }

    // 池管理
    public class C_AssetBundleMgr : C_MonoSingleton<C_AssetBundleMgr>
    {
        private List<C_AssetBundleRef> m_AssetBundleRefList = new List<C_AssetBundleRef>();

        private List<C_AssetBundleRef> m_DirtyAssetBundleRefList = new List<C_AssetBundleRef>();

        private List<C_AssetBundleRef> m_ForceDirtyAssetBundleRefList = new List<C_AssetBundleRef>();

        private List<C_AssetBundleRef> m_UnloadAssetBundleRefList = new List<C_AssetBundleRef>();

        //Cache
        private Dictionary<string, List<string>> m_AssetBundleCacheDict = new Dictionary<string, List<string>>();

        private Dictionary<string, List<string>> m_NeedLoadCacheDict = new Dictionary<string, List<string>>();

        private Dictionary<string, List<string>> m_UnLoadCacheDict = new Dictionary<string, List<string>>();

        private Dictionary<string, int> _TotalUnLoadSum = new Dictionary<string, int>();

        void Update()
        {
            try
            {

                //强制卸载
                if (m_ForceDirtyAssetBundleRefList.Count > 0)
                {
                    for (int i = 0; i < m_ForceDirtyAssetBundleRefList.Count; i++)
                    {
                       // Debug.Log("--ab name:" + m_ForceDirtyAssetBundleRefList[i].Bundle.name + "start unload.........");
                        if (m_ForceDirtyAssetBundleRefList[i].Bundle != null)
                            m_ForceDirtyAssetBundleRefList[i].Bundle.Unload(true);
                    }

                    m_ForceDirtyAssetBundleRefList.Clear();


                }

                if (m_UnloadAssetBundleRefList.Count > 0)
                {
                    for (int i = 0; i < m_UnloadAssetBundleRefList.Count; i++)
                    {
                        if (m_UnloadAssetBundleRefList[i].Bundle != null)
                            m_UnloadAssetBundleRefList[i].Bundle.Unload(false);
                    }

                    m_UnloadAssetBundleRefList.Clear();

                    //黄志龙，后续修改到loading界面去释放
                    Resources.UnloadUnusedAssets();
                }

                for (int i = m_DirtyAssetBundleRefList.Count - 1; i >= 0; i--)
                {
                    if (m_DirtyAssetBundleRefList[i] == null || m_DirtyAssetBundleRefList[i].Bundle == null)
                    {
                        m_DirtyAssetBundleRefList.RemoveAt(i);
                    }
                    else
                    {
                        m_UnloadAssetBundleRefList.Add(m_DirtyAssetBundleRefList[i]);

                        m_DirtyAssetBundleRefList.RemoveAt(i);
                    }
                }
                if (m_NeedLoadCacheDict.Count > 0)
                {
                    List<string> keyList = new List<string>(m_NeedLoadCacheDict.Keys);

                    for (int i = m_NeedLoadCacheDict.Count - 1; i >= 0; i--)
                    {

                        List<string> valueList = m_NeedLoadCacheDict[keyList[i]];

                          if (valueList.Count > 4)
                         {
                             LoadFromFile(valueList[valueList.Count - 1], true);
                             LoadFromFile(valueList[valueList.Count - 2], true);
                             LoadFromFile(valueList[valueList.Count - 3], true);
                             LoadFromFile(valueList[valueList.Count - 4], true);
                             m_NeedLoadCacheDict[keyList[i]].RemoveAt(valueList.Count - 1);
                             m_NeedLoadCacheDict[keyList[i]].RemoveAt(valueList.Count - 1);
                             m_NeedLoadCacheDict[keyList[i]].RemoveAt(valueList.Count - 1);
                             m_NeedLoadCacheDict[keyList[i]].RemoveAt(valueList.Count - 1);
                         }else if (valueList.Count > 0)
                        {
                            C_DebugHelper.Log("id:" + valueList.Count + "--ab name:" + valueList[valueList.Count - 1] + "start");

                            LoadFromFile(valueList[valueList.Count - 1], true);
                            C_DebugHelper.Log("id:" + valueList.Count + "--ab name:" + valueList[valueList.Count - 1] + "end");

                            m_NeedLoadCacheDict[keyList[i]].RemoveAt(valueList.Count - 1);
                        }
                        else
                        {
                            m_NeedLoadCacheDict.Remove(keyList[i]);
                        }

                    }
                }



                if (m_UnLoadCacheDict.Count > 0)
                {
                    List<string> keyList = new List<string>(m_UnLoadCacheDict.Keys);

                    for (int i = m_UnLoadCacheDict.Count - 1; i >= 0; i--)
                    {
                        List<string> valueList = m_UnLoadCacheDict[keyList[i]];
                        /* if (valueList.Count > 4)
                        {
                            C_AssetBundleRef abr = GetAssetBundleRefList(valueList[valueList.Count - 1]);
                            if (abr != null)
                                abr.CacheUnload();

                            abr = GetAssetBundleRefList(valueList[valueList.Count - 2]);
                            if (abr != null)
                                abr.CacheUnload();
                            abr = GetAssetBundleRefList(valueList[valueList.Count - 3]);
                            if (abr != null)
                                abr.CacheUnload();
                            abr = GetAssetBundleRefList(valueList[valueList.Count - 4]);
                            if (abr != null)
                                abr.CacheUnload();

                            m_UnLoadCacheDict[keyList[i]].RemoveAt(valueList.Count - 1);
                            m_UnLoadCacheDict[keyList[i]].RemoveAt(valueList.Count - 2);
                            m_UnLoadCacheDict[keyList[i]].RemoveAt(valueList.Count - 3);
                            m_UnLoadCacheDict[keyList[i]].RemoveAt(valueList.Count - 4);

                        }  else*/ if (valueList.Count > 0)
                        {
                            C_AssetBundleRef abr = GetAssetBundleRefList(valueList[valueList.Count - 1]);
                            if (abr != null)
                            {
                                abr.CacheUnload();
                                C_DebugHelper.Log("id:" + valueList.Count + "--ab name:" + abr.Bundle.name);
                            }

                            m_UnLoadCacheDict[keyList[i]].RemoveAt(valueList.Count - 1);
                        }
                        else
                        {
                            m_UnLoadCacheDict.Remove(keyList[i]);
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                C_DebugHelper.LogError("NeedLoadCache error is :" + e);
            }

        }

        //加载资源，所有加载之后的bundle都添加到m_AssetBundleRefList
        public C_AssetBundleRef LoadFromFile(string filePath, bool isCache)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            //使用缓存AB 数据
            C_AssetBundleRef listAssetBundleRef = GetAssetBundleRefList(filePath);
            if (listAssetBundleRef != null)
            {
                if (isCache)
                    listAssetBundleRef.RefCacheCount++;
                else
                    listAssetBundleRef.RefCount++;

                return listAssetBundleRef;
            }

            //Debug.LogError("filePath "+ filePath+" is loading local...");
            //通过 LoadFromFile类获取数据

            AssetBundle assetBundle = AssetBundle.LoadFromFile(filePath);
            if (assetBundle != null)
            {
                C_AssetBundleRef assetBundleRef = new C_AssetBundleRef();
                assetBundleRef.FilePath = filePath;

                if (isCache)
                    assetBundleRef.RefCacheCount = 1;
                else
                    assetBundleRef.RefCount = 1;

                assetBundleRef.Bundle = assetBundle;

                m_AssetBundleRefList.Add(assetBundleRef);

                return assetBundleRef;
            }

            return null;
        }

        public C_AssetBundleRef AddAssetBundle(string filePath, AssetBundle bundle, bool isCache)
        {
            C_AssetBundleRef listAssetBundleRef = GetAssetBundleRefList(filePath);
            if (listAssetBundleRef != null)
            {
                if (isCache)
                    listAssetBundleRef.RefCacheCount++;
                else
                    listAssetBundleRef.RefCount++;

                return listAssetBundleRef;
            }

            C_AssetBundleRef assetBundleRef = new C_AssetBundleRef();
            assetBundleRef.FilePath = filePath;

            if (isCache)
                assetBundleRef.RefCacheCount = 1;
            else
                assetBundleRef.RefCount = 1;

            assetBundleRef.Bundle = bundle;

            m_AssetBundleRefList.Add(assetBundleRef);

            return assetBundleRef;
        }

        //根据filepath查找对应的资源
        public C_AssetBundleRef GetAssetBundleRefList(string filePath)
        {
            ClearInvalidAssetBundle();

            for (int i = 0; i < m_AssetBundleRefList.Count; i++)
            {
                if (m_AssetBundleRefList[i].FilePath == filePath)
                    return m_AssetBundleRefList[i];
            }

            return null;
        }

        public void Unload(C_AssetBundleRef assetBundleRef)
        {
            ClearInvalidAssetBundle();

            for (int i = 0; i < m_DirtyAssetBundleRefList.Count; i++)
            {
                if (m_DirtyAssetBundleRefList[i] != null && m_AssetBundleRefList[i].FilePath == assetBundleRef.FilePath)
                    return;
            }

            m_DirtyAssetBundleRefList.Add(assetBundleRef);
        }
        public void ForceCacheUnload(C_AssetBundleRef assetBundleRef)
        {
            ClearInvalidAssetBundle();

            for (int i = 0; i < m_ForceDirtyAssetBundleRefList.Count; i++)
            {
                if (m_ForceDirtyAssetBundleRefList[i] != null && m_AssetBundleRefList[i].FilePath == assetBundleRef.FilePath)
                    return;
            }

            m_ForceDirtyAssetBundleRefList.Add(assetBundleRef);
        }


        private void ClearInvalidAssetBundle()
        {
            for (int i = m_AssetBundleRefList.Count - 1; i >= 0; i--)
            {
                if (m_AssetBundleRefList[i] == null || m_AssetBundleRefList[i].Bundle == null)
                    m_AssetBundleRefList.RemoveAt(i);
            }
        }

        //----------------------------------------------缓存----------------------------------------------
        //添加到预加载队列，进行加载
        public void LoadCacheList(string type, List<string> filePathList)
        {
            m_NeedLoadCacheDict.Add(type, filePathList);
            
            m_AssetBundleCacheDict.Add(type, C_CommonAlgorithm.Clone<string>(filePathList));
        }

        public void UnloadCacheList(string type)
        {
            if (m_NeedLoadCacheDict.ContainsKey(type))
                return;

            if (m_AssetBundleCacheDict.ContainsKey(type))
            {
                m_UnLoadCacheDict.Add(type, m_AssetBundleCacheDict[type]);
                m_AssetBundleCacheDict.Remove(type);
            }
        }
        /// <summary>
        /// 强制卸载所有的资源
        /// </summary>
        /// <param name="type"></param>
        public void ForceUnloadCahceList(string type)
        {
            if (m_NeedLoadCacheDict.ContainsKey(type))
            {
                m_UnLoadCacheDict.Add(type, m_NeedLoadCacheDict[type]);
                m_NeedLoadCacheDict.Remove(type);
            }

            if (m_AssetBundleCacheDict.ContainsKey(type))
            {
                m_UnLoadCacheDict.Add(type, m_AssetBundleCacheDict[type]);
                m_AssetBundleCacheDict.Remove(type);
            }
            if (_TotalUnLoadSum.ContainsKey(type))
            {
                _TotalUnLoadSum[type] = m_UnLoadCacheDict[type].Count;
            }
            else
            {
                _TotalUnLoadSum.Add(type, m_UnLoadCacheDict[type].Count);

            }
        }
        /// <summary>
        /// 资源卸载的进度
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public float GetUnLoadProgress(string type)
        {
            if (m_UnLoadCacheDict.ContainsKey(type) && _TotalUnLoadSum.ContainsKey(type))
            {
                return (_TotalUnLoadSum[type]-m_UnLoadCacheDict[type].Count ) / (float)_TotalUnLoadSum[type];
            }

            return 0;
        }
        public float GetCacheProgress(string type)
        {
            if (m_AssetBundleCacheDict.ContainsKey(type))
            {
                if (m_NeedLoadCacheDict.ContainsKey(type))
                    return (m_AssetBundleCacheDict[type].Count - m_NeedLoadCacheDict[type].Count) / (float)m_AssetBundleCacheDict[type].Count;

                return 100;
            }

            return 0;
        }
    }
}