using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YB;

namespace Assets.Scripts.C_Framework
{
    public class C_SceneLoader
    {
        private string _MainActiveSceneName;
        private string _LoadSceneName;
        private bool _LoadOver = false;
        private float m_fProgress = 0;
        public float Progress
        {
            get { return m_fProgress; }
        }

        protected AsyncOperation m_AsyncOperation = null;
        public C_SceneLoader()
        {
        }
        public C_SceneLoader(string sceneName, LoadSceneMode loadSceneMode,string mainscene="")
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                int end = sceneName.LastIndexOf('.');
                sceneName = sceneName.Substring(0, (end == -1 ? sceneName.Length : end));
                _LoadSceneName = sceneName;
                //SceneManager.sceneLoaded -= OnSceneLoaded;
                //SceneManager.sceneLoaded += OnSceneLoaded;
                _LoadOver = false;
                _MainActiveSceneName = mainscene;
                C_MonoSingleton<C_GameFramework>.GetInstance().StartCoroutine(Execute(sceneName, loadSceneMode));
            }
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
#if !C_Framework
            if (string.IsNullOrEmpty(_MainActiveSceneName))
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                _LoadOver = true;
            }

            if (!string.IsNullOrEmpty(_MainActiveSceneName) && scene!=null && scene.name.Equals(_MainActiveSceneName))
            {
                Utility.SetMainScene(_MainActiveSceneName);
                _LoadOver = true;
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }

            C_DebugHelper.Log(" OnSceneLoaded"+ scene.name);
#endif
        }

        private IEnumerator Execute(string sceneName, LoadSceneMode loadSceneMode)
        {
            m_fProgress = 0;
            float toProgress = 0;
            Utility.DisableAnylitics();

            // u3d 5.3之后使用using UnityEngine.SceneManagement;加载场景
            m_AsyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

            // 不允许加载完毕自动切换场景，因为有时候加载太快了就看不到加载进度条UI效果了
            m_AsyncOperation.allowSceneActivation = false;

            // m_AsyncOperation.progress测试只有0和0.9(其实只有固定的0.89...)
            // 所以大概大于0.8就当是加载完成了
            while (m_AsyncOperation.progress < 0.9f)
            {
                toProgress = m_AsyncOperation.progress * 100;
                while (m_fProgress < toProgress)
                {
                    ++m_fProgress;

                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForEndOfFrame();
            }

            m_fProgress = 90;
            toProgress = 100;

            while (m_fProgress < toProgress)
            {
                ++m_fProgress;

                yield return new WaitForEndOfFrame();
            }
        }

        public bool IsDone()
        {
            if (m_fProgress >= 100 )
                return true;

            return false;
        }

        public void ActivationScene()
        {
            if (m_AsyncOperation != null && m_AsyncOperation.allowSceneActivation==false)
                m_AsyncOperation.allowSceneActivation = true;
        }
        public bool LoadOver()
        {
            //if (m_AsyncOperation != null&& _LoadOver)
            if (m_AsyncOperation != null )
                return m_AsyncOperation.isDone;
            return false;
        }
        public void UnloadSceneAddictive(string name, Action OnLoadScene = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            bool isRet = false;
            for(int i = 0;i <SceneManager.sceneCount;i++)
            {
                if (SceneManager.GetSceneAt(i).name.Equals(name))
                {
                    UnloadSceneAddictive(SceneManager.GetSceneAt(i));
                    isRet = true;
                    break;
                }
            }
            if (!isRet)
            {
                Debug.LogError("没有场景："+name);
                return;
            }
        }
        public void UnloadSceneAddictive(Scene scene, Action OnLoadScene = null)
        {
            C_MonoSingleton<C_GameFramework>.GetInstance().StartCoroutine(UnloadSceneAsync(scene, OnLoadScene));
        }

        private IEnumerator UnloadSceneAsync(Scene scene, Action OnLoadScene = null)
        {
            m_fProgress = 0;
            float toProgress = 0;
            Utility.DisableAnylitics();

            m_AsyncOperation = SceneManager.UnloadSceneAsync(scene);

            m_AsyncOperation.allowSceneActivation = false;
            while (m_AsyncOperation.progress < 0.9f)
            {
                toProgress = m_AsyncOperation.progress * 100;
                while (m_fProgress < toProgress)
                {
                    ++m_fProgress;

                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForEndOfFrame();
            }
            m_fProgress = 90;
            toProgress = 100;
            while (m_fProgress < toProgress)
            {
                ++m_fProgress;

                yield return new WaitForEndOfFrame();
            }

            if (OnLoadScene != null)
                OnLoadScene.Invoke();

        }
    }
}
