using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 资源管理器
namespace Assets.Scripts.C_Framework
{
    public class C_ResMgr
    {

        #region Resource加载资源
        
        public static UnityEngine.Object LoadResource(string resName, string resPath)
        {
            if (string.IsNullOrEmpty(resName) || string.IsNullOrEmpty(resPath))
                return null;

            return Resources.Load(C_String.DeleteExpandedName(resPath + resName));
        }

        public static T LoadResource<T>(string resName, string resPath) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(resName))
                return null;

            return Resources.Load<T>(C_String.DeleteExpandedName(resPath + resName));
        }

        public static void AsyncLoadResource(string resName, string resPath, Action<UnityEngine.Object> callback)
        {
            if (callback == null)
                return;

            if (string.IsNullOrEmpty(resName) || string.IsNullOrEmpty(resPath))
            {
                callback(null);
                return;
            }

            C_MonoSingleton<C_GameFramework>.GetInstance().StartCoroutine(ExecuteAsyncLoadResource(C_String.DeleteExpandedName(resPath + resName), callback));
        }

        private static IEnumerator ExecuteAsyncLoadResource(string filePath, Action<UnityEngine.Object> callback)
        {
            AsyncOperation asyncOperation = Resources.LoadAsync(filePath);

            yield return asyncOperation;

            callback(((ResourceRequest)asyncOperation).asset);
        }

        public static void AsyncLoadResource<T>(string resName, string resPath, Action<T> callback) where T : UnityEngine.Object
        {
            if (callback == null)
                return;

            if (string.IsNullOrEmpty(resName) || string.IsNullOrEmpty(resPath))
            {
                callback(null);
                return;
            }

            C_MonoSingleton<C_GameFramework>.GetInstance().StartCoroutine(ExecuteAsyncLoadResource<T>(C_String.DeleteExpandedName(resPath + resName), callback));
        }

        private static IEnumerator ExecuteAsyncLoadResource<T>(string filePath, Action<T> callback) where T : UnityEngine.Object
        {
            AsyncOperation asyncOperation = Resources.LoadAsync<T>(filePath);

            yield return asyncOperation;

            callback(((ResourceRequest)asyncOperation).asset as T);
        }

        #endregion



        #region AssetBundle加载资源，加载相关资源和本身资源，并立刻释放，如果没有引用

        public static T LoadAssetBundle<T>(string resName, string assetBundleFilePath, List<string> dpsList, bool isInstantiate = false, bool isForever = false) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(resName) || string.IsNullOrEmpty(assetBundleFilePath))
                return null;

            T result = null;
            try
            {
                C_AssetBundleRef[] abs = new C_AssetBundleRef[dpsList.Count];
                for (int i = 0; i < abs.Length; i++)
                {
                    C_DebugHelper.LogFormat("LoadResource abs[{0}]: {1}", i, dpsList[i]);

                    abs[i] = C_MonoSingleton<C_AssetBundleMgr>.GetInstance().LoadFromFile(dpsList[i], isForever);
                }

                C_AssetBundleRef bundle = C_MonoSingleton<C_AssetBundleMgr>.GetInstance().LoadFromFile(assetBundleFilePath, isForever);
                if (bundle != null)
                {
                    result = bundle.LoadAsset<T>(resName);

                    if (isInstantiate && result != null)
                        result = GameObject.Instantiate(result);

                    bundle.AutoUnload();
                }

                for (int i = 0; i < abs.Length; i++)
                {
                    if (abs[i] != null)
                        abs[i].AutoUnload();
                }
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("LoadAssetBundle : " + e);
            }

            return result;
        }

        public static void AsyncLoadAssetBundle<T>(string resName, string assetBundleFilePath, List<string> dpsList, Action<T> callback, bool isInstantiate = false, bool isForever = false) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(resName) || string.IsNullOrEmpty(assetBundleFilePath))
                return;

            new C_AsyncAssetBundleLoader<T>(resName, assetBundleFilePath, dpsList, callback, isInstantiate, isForever);
        }

        //----------------------------------------------缓存----------------------------------------------
        public static T LoadAssetBundleCache<T>(string resName, string assetBundleFilePath, bool isInstantiate = false, bool isForever = false) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(resName) || string.IsNullOrEmpty(assetBundleFilePath))
                return null;

            T result = null;
            try
            {
                C_AssetBundleRef bundle = C_MonoSingleton<C_AssetBundleMgr>.GetInstance().LoadFromFile(assetBundleFilePath, isForever);
                if (bundle != null)
                {
                    result = bundle.LoadAsset<T>(resName);

                    if (isInstantiate && result != null)
                        result = GameObject.Instantiate(result);
                }
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("LoadAssetBundleCache : " + e);
            }

            return result;
        }

        public static void LoadAssetBundleCacheList(string type, List<string> assetBundlePathList)
        {
            if (string.IsNullOrEmpty(type) || assetBundlePathList.Count == 0)
                return;

            C_AssetBundleMgr.GetInstance().LoadCacheList(type, assetBundlePathList);
        }

        public static void UnloadAssetBundleCacheList(string type)
        {
            if (string.IsNullOrEmpty(type))
                return;

            C_AssetBundleMgr.GetInstance().UnloadCacheList(type);
        }
        public static void ForceUnloadAssetBundleCacheList(string type)
        {
            if (string.IsNullOrEmpty(type))
                return;

            C_AssetBundleMgr.GetInstance().ForceUnloadCahceList(type);
        }
        public static float GetAssetUnBundleCacheListProgress(string type)
        {
            if (string.IsNullOrEmpty(type))
                return 0;

            return C_AssetBundleMgr.GetInstance().GetUnLoadProgress(type);
        }


        public static float GetAssetBundleCacheListProgress(string type)
        {
            if (string.IsNullOrEmpty(type))
                return 0;

            return C_AssetBundleMgr.GetInstance().GetCacheProgress(type);
        }

        public static T GetAssetBundleFormCache<T>(string resName, string assetBundleFilePath, bool isInstantiate = false, bool isForever = false) where T : UnityEngine.Object
        {
            T result = null;
            try
            {
                C_AssetBundleRef bundle = C_MonoSingleton<C_AssetBundleMgr>.GetInstance().GetAssetBundleRefList(assetBundleFilePath);
                if (bundle != null)
                {
                    result = bundle.LoadAsset<T>(resName);

                    if (isInstantiate && result != null)
                        result = GameObject.Instantiate(result);
                }
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("GetAssetBundleFormCache : " + e);
            }

            return result;
        }

        #endregion
    }
}
