using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.C_Framework
{
    // 场景管理器
    public class C_SceneMgr : C_MonoSingleton<C_SceneMgr>
    {
        public string CurSceneName = "";

        public bool IsLoading = false;

        public string LoadingSceneName = "";
        private Action m_Callback = null;
        private bool m_bAutoActivationScene = true;
        private LoadSceneMode m_LoadSceneMode = LoadSceneMode.Single;

        private C_SceneLoader m_SceneLoader = null;
        private C_SceneLoader m_LoadingLoader = null;
        private string _MainSceneName = string.Empty;
        
        public void LoadScene(string sceneName)
        {
            LoadScene(sceneName, "", null, true, LoadSceneMode.Single);
        }

        public void LoadScene(string sceneName, Action callback)
        {
            LoadScene(sceneName, "", callback, true, LoadSceneMode.Single);
        }

        public void LoadScene(string sceneName, string loadingName, Action callback)
        {
            LoadScene(sceneName, loadingName, callback, true, LoadSceneMode.Single);
        }

        public void LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            LoadScene(sceneName, "", null, true, loadSceneMode);
        }

        public void LoadScene(string sceneName, string loadingName, Action callback, bool autoActivationScene, LoadSceneMode loadSceneMode,string mainSceneName="")
        {
            if (string.IsNullOrEmpty(sceneName))
                return;

            if (IsLoading)
                return;

            m_SceneLoader = null;
            m_LoadingLoader = null;

            IsLoading = true;

            LoadingSceneName = sceneName;
            m_Callback = callback;
            m_bAutoActivationScene = autoActivationScene;
            m_LoadSceneMode = loadSceneMode;
            _MainSceneName = mainSceneName;
            if (string.IsNullOrEmpty(loadingName))
            {
                //如果能下载,我们在此添加加载
                m_SceneLoader = new C_SceneLoader(sceneName, m_LoadSceneMode, mainSceneName);
            }
            else
            {
                m_LoadingLoader = new C_SceneLoader(loadingName, LoadSceneMode.Single);
            }
        }

        void Update()
        {
            if (IsLoading)
            {
                if (m_LoadingLoader != null && m_LoadingLoader.IsDone())
                {
                    m_LoadingLoader.ActivationScene();

                    CurSceneName = "Loading";
                    m_SceneLoader = new C_SceneLoader(LoadingSceneName, m_LoadSceneMode, _MainSceneName);

                    m_LoadingLoader = null;
                }

                if (m_SceneLoader != null && m_SceneLoader.IsDone())
                {
                    if (m_bAutoActivationScene )
                    {
                        m_SceneLoader.ActivationScene();
                        if (m_SceneLoader.LoadOver() )
                        {
                            IsLoading = false;
                            CurSceneName = LoadingSceneName;
                            LoadingSceneName = "";
                            m_SceneLoader = null;
                            m_LoadSceneMode = LoadSceneMode.Single;

                            if (m_Callback != null)
                                m_Callback();

                        }
                    }
                    else
                    {
                        if (m_Callback != null )
                            m_Callback();
                    }                  
                }
            }
        }

        public float Progress
        {
            get
            {
                if (m_SceneLoader != null)
                    return m_SceneLoader.Progress;

                return 0;
            }
        }

        public void ActivationScene()
        {
            if (m_SceneLoader != null && m_SceneLoader.IsDone())
            {
                m_SceneLoader.ActivationScene();
                if( m_SceneLoader.LoadOver())
                {
                    IsLoading = false;
                    CurSceneName = LoadingSceneName;
                    LoadingSceneName = "";
                    m_SceneLoader = null;
                    m_LoadSceneMode = LoadSceneMode.Single;
                }
            }
        }


        public void UnloadSceneAsync(string sceneName, string loadingName, Action callback=null, bool autoActivationScene =true)
        {
            if (string.IsNullOrEmpty(sceneName))
                return;

            if (IsLoading)
                return;

            m_SceneLoader = null;
            m_LoadingLoader = null;

            IsLoading = true;

            LoadingSceneName = sceneName;
            m_Callback = callback;
            m_bAutoActivationScene = autoActivationScene;

            m_SceneLoader = new C_SceneLoader();
            m_SceneLoader.UnloadSceneAddictive(LoadingSceneName);
            _MainSceneName = "";
        }

    }
}