using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.C_Framework
{
    public class C_AsyncAssetBundleLoader<T> where T : UnityEngine.Object
    {
        private string m_strResName = "";
        private string m_strAssetBundleFilePath = "";
        private List<string> m_DependenciesFilePathList = new List<string>();
        private Action<T> m_Callback = null;
        private bool m_IsInstantiate = false;
        private bool m_IsForever = false;

        private List<C_AssetBundleRef> m_DependenciesAssetBundleList = new List<C_AssetBundleRef>();

        public C_AsyncAssetBundleLoader(string resName, string assetBundleFilePath, List<string> dependenciesFilePathList, Action<T> callback, bool isInstantiate, bool isForever)
        {
            m_strResName = resName;
            m_strAssetBundleFilePath = assetBundleFilePath;
            m_DependenciesFilePathList = dependenciesFilePathList;
            m_Callback = callback;
            m_IsInstantiate = isInstantiate;
            m_IsForever = isForever;

            C_MonoSingleton<C_GameFramework>.GetInstance().StartCoroutine(ExecuteLoader());
        }

        private int loadedCount = 0;
        
        private IEnumerator ExecuteLoader()
        {
            if (m_DependenciesFilePathList.Count > loadedCount)
            {
                string strDependenciesFilePath = m_DependenciesFilePathList[loadedCount];

                C_AssetBundleRef aref = C_MonoSingleton<C_AssetBundleMgr>.GetInstance().GetAssetBundleRefList(strDependenciesFilePath);
                if (aref == null)
                {
                    AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(strDependenciesFilePath);

                    yield return request;

                    aref = C_MonoSingleton<C_AssetBundleMgr>.GetInstance().AddAssetBundle(strDependenciesFilePath, request.assetBundle, m_IsForever);
                }
                else
                {
                    aref.RefCount++;
                }

                m_DependenciesAssetBundleList.Add(aref);

                loadedCount++;

                ExecuteLoader();
            }
            else
            {
                C_AssetBundleRef aref = C_MonoSingleton<C_AssetBundleMgr>.GetInstance().GetAssetBundleRefList(m_strAssetBundleFilePath);
                if (aref == null)
                {
                    AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(m_strAssetBundleFilePath);

                    yield return request;

                    aref = C_MonoSingleton<C_AssetBundleMgr>.GetInstance().AddAssetBundle(m_strAssetBundleFilePath, request.assetBundle, m_IsForever);
                }
                else
                {
                    aref.RefCount++;
                }

                T tempObject = null;

                if (!string.IsNullOrEmpty(m_strResName))
                {
                    AssetBundleRequest assetBundleRequest = aref.Bundle.LoadAssetAsync<T>(m_strResName);

                    yield return assetBundleRequest;

                    tempObject = assetBundleRequest.asset as T;

                    if (m_IsInstantiate && tempObject != null)
                        tempObject = GameObject.Instantiate(tempObject);
                }

                if (m_Callback != null)
                    m_Callback(tempObject);

                aref.AutoUnload();

                for (int i = 0; i < m_DependenciesAssetBundleList.Count; i++)
                {
                    if (m_DependenciesAssetBundleList[i] != null)
                        m_DependenciesAssetBundleList[i].AutoUnload();
                }
            }
        }
    }
}
