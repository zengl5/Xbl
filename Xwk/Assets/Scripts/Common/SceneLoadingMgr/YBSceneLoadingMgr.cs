using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YBSceneLoadingMgr : C_MonoSingleton<YBSceneLoadingMgr>
{
    //当前加载的loading界面
    private UI_StoryLoading _UI_StoryLoading;
    //当前需要加载和卸载的总共场景个数
    private int m_nLoadingSceneCount = -1;
    //当前需要加载的场景
    private Queue<string> m_SceneQueue = new Queue<string>();
    //当前需要卸载的场景
    private Queue<string> m_UnloadSceneQueue = new Queue<string>();
    //当前的滑动条进度
    private float _CurSliderValue = 0f;
    //所有操作完成
    public System.Action OnCompelete;
    //预加载的配置文件
    public string _PreLoadFile = string.Empty;
    //需要卸载的ab资源配置文件
    public string _UnloadResFile = string.Empty;
    //进行加载场景的回调函数
    private System.Action _Action = null;
    //当前加载的进度
    private float _CurrentPrecent = 0.5f;
    //当前以哪个场景为主场景
    private string _MainSceneName = string.Empty;
    //是否进行loading界面自动删除
    private bool _AutoCloseUI = true;

    private enum LoadingState
    {
        load_null,
        //加载场景
        load_scene,
        //加载ab资源之后加载场景
        load_ab_then_scene,
        //加载ab资源
        load_ab,
        //卸载ab资源之后卸载场景
        unload_ab_then_scene,
        //卸载ab资源
        unload_ab,
        //卸载场景
        unload_scene,
        //只卸载资源
        unload_ab_only,
    }

    private LoadingState _LoadingState;
    // Use this for initialization
    void Start()
    {
        _LoadingState = LoadingState.load_null;
    }

    // Update is called once per frame
    void Update()
    {
        if (_UI_StoryLoading != null)
        {
            if (_LoadingState == LoadingState.load_ab)//预加载ab资源，之后加载场景
            {
                _CurSliderValue = C_MonoSingleton<PreLoadResMgr>.Instance.getCurrentLoadProgress(_CurrentPrecent);
                if (_CurSliderValue >= _CurrentPrecent)
                {
                    _LoadingState = LoadingState.load_null;
                    //开始进入场景的加载
                    if (_Action != null)
                    {
                        _Action();
                    }
                }
            }
            else if (_LoadingState == LoadingState.load_scene )//加载场景,不加载ab资源
            {
                float loadProgress = (m_nLoadingSceneCount - m_SceneQueue.Count - m_UnloadSceneQueue.Count - 1 + C_MonoSingleton<C_SceneMgr>.GetInstance().Progress / 100) / (float)m_nLoadingSceneCount;
                _CurSliderValue = loadProgress;

            }
            else if (_LoadingState == LoadingState.load_ab_then_scene)//预加载ab之后加载场景
            {
                float loadProgress = (m_nLoadingSceneCount - m_SceneQueue.Count - 1 + C_MonoSingleton<C_SceneMgr>.GetInstance().Progress / 100) / (float)m_nLoadingSceneCount;
                _CurSliderValue = loadProgress * (1 - _CurrentPrecent) + _CurrentPrecent;

            }
            else if (_LoadingState == LoadingState.unload_ab)//卸载ab，之后卸载场景
            {
                _CurSliderValue = C_MonoSingleton<PreLoadResMgr>.Instance.getCurrentUnLoadProgress(_CurrentPrecent);
                if (_CurSliderValue >= _CurrentPrecent)
                {
                    _LoadingState = LoadingState.unload_ab_then_scene;
                    if (_Action != null)
                    {
                        _Action();
                    }
                }
            }
            else if (_LoadingState == LoadingState.unload_ab_only)//只有卸载ab 
            {
                _CurSliderValue = C_MonoSingleton<PreLoadResMgr>.Instance.getCurrentUnLoadProgress(_CurrentPrecent);
                if (_CurSliderValue >= _CurrentPrecent)
                {
                    _LoadingState = LoadingState.load_null;
                    LoadCompelete();
                }
            }
            else if (_LoadingState == LoadingState.unload_ab_then_scene  )//卸载ab资源结束之后开始进行场景卸载
            {
                float loadProgress = (m_nLoadingSceneCount - m_UnloadSceneQueue.Count - 1 + C_MonoSingleton<C_SceneMgr>.GetInstance().Progress / 100) / (float)m_nLoadingSceneCount;
                _CurSliderValue = loadProgress * (1 - _CurrentPrecent) + _CurrentPrecent;
            }
            else if (_LoadingState == LoadingState.unload_scene)//直接卸载场景
            {
                float loadProgress = (m_nLoadingSceneCount - m_UnloadSceneQueue.Count - m_SceneQueue.Count - 1 + C_MonoSingleton<C_SceneMgr>.GetInstance().Progress / 100) / (float)m_nLoadingSceneCount;
                _CurSliderValue = loadProgress;
            }

            _UI_StoryLoading.UpdateProgressVaule(_CurSliderValue);

        }
    }
    void LoadAbSceneProgress()
    {
        
       
    }

    private void NextStep()
    {
        if (m_SceneQueue.Count > 0)
        {
            C_MonoSingleton<C_SceneMgr>.GetInstance().LoadScene(m_SceneQueue.Dequeue(), "", () =>
            {
                NextStep();
            }, true, LoadSceneMode.Additive, _MainSceneName);
        }
        else
        {
            LoadCompelete();
        }
    }
         
    private void UnLoadNextStep()
    {
        if (m_UnloadSceneQueue.Count > 0)
        {

            C_MonoSingleton<C_SceneMgr>.GetInstance().UnloadSceneAsync(m_UnloadSceneQueue.Dequeue(), "", () =>
            {
                UnLoadNextStep();
            });
        }
        else
        {
            NextStep();
        }
    }
    public void LoadCompelete()
    {
        _LoadingState = LoadingState.load_null;

        if (OnCompelete != null)
        {
            OnCompelete();
        }
        if (_UI_StoryLoading != null && _AutoCloseUI)
            _UI_StoryLoading.CloseLoadingUI();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unloadSceneName">表示所有需要卸载的场景，如果有多个场景需要卸载："aoe_story,py_scene_2"</param>
    /// <param name="unloadResConfig">如果需要卸载ab资源，则需要配置一个ab资源配置文件名</param>
    /// <param name="callback"></param>
    /// <param name="uiName"></param>
    public void UnLoadABUnloadAllScene(string unloadResConfig = "", System.Action callback = null, string uiName = "UI_StoryLoading")
    {
        if (_LoadingState != LoadingState.load_null)
        {
            return;
        }
        if (C_MonoSingleton<C_SceneMgr>.GetInstance().IsLoading)
        {
            return;
        }
        if (string.IsNullOrEmpty(uiName))
        {
            return;
        }
        _CurrentPrecent = 0.9f;
        _UnloadResFile = unloadResConfig;
        OnCompelete = callback;
        //打开界面
        OpenUI(uiName);
        //string unloadSceneName = string.Empty;
        //for (int i = 0; i < SceneManager.sceneCount; i++)
        //{
        //    unloadSceneName += SceneManager.GetSceneAt(i).name + ",";
        //}
        AddUnLoadSceneQueue(string.Empty, "Loading");
        StartUnloadWithLoadingScene();

        _AutoCloseUI = false;

         

    }
    public void PreLoadMulitSceneAsync(string mainSceneName, string otherSceneName = "", string resConfig = "", System.Action callback = null, string uiName = "UI_StoryLoading")
    {
        if (_LoadingState != LoadingState.load_null)
        {
            return;
        }
        _AutoCloseUI = true;
        _PreLoadFile = resConfig;
        LoadMulitSceneAsync(mainSceneName, otherSceneName, callback, uiName);
    }
    public void LoadScene(string mainSceneName, System.Action callback = null, string uiName = "UI_StoryLoading")
    {
        LoadMulitSceneAsync(mainSceneName, "", callback, uiName);
    }
    public void LoadMulitSceneAsync(string mainSceneName, string otherSceneName = "", System.Action callback = null, string uiName = "UI_StoryLoading")
    {
        if (_LoadingState != LoadingState.load_null)
        {
            return;
        }
        if (C_MonoSingleton<C_SceneMgr>.GetInstance().IsLoading)
        {
            return;
        }
        if (string.IsNullOrEmpty(mainSceneName) || string.IsNullOrEmpty(uiName))
        {
            return;
        }
        _AutoCloseUI = true;
        OnCompelete = callback;
        //打开界面
        OpenUI(uiName);
        AddLoadSceneQueue(mainSceneName, otherSceneName);
        _MainSceneName = mainSceneName;
        StartLoad();

    }
    public void CloseUI()
    {
        if (_UI_StoryLoading != null)
            _UI_StoryLoading.CloseLoadingUI();
    }

    public void OpenUI(string uiName)
    {
        if (!C_MonoSingleton<C_UIMgr>.GetInstance().IsOpenedUI(uiName))
            C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI(uiName);
        _UI_StoryLoading = FindObjectOfType<UI_StoryLoading>();
    }
    public void AddLoadSceneQueue(string mainSceneName, string otherSceneName)
    {
        m_SceneQueue.Clear();
        m_SceneQueue.Enqueue(mainSceneName);
        if (otherSceneName != null && !string.IsNullOrEmpty(otherSceneName))
        {
            string[] others = otherSceneName.Split(',');
            if (others == null)
            {
                return;
            }
            for (int i = 0; i < others.Length; i++)
            {
                if (!string.IsNullOrEmpty(others[i]))
                {
                    m_SceneQueue.Enqueue(others[i]);
                }
            }
        }
        m_nLoadingSceneCount = m_SceneQueue.Count;
    }
    public void AddUnLoadSceneQueue(string mainSceneName, string otherSceneName)
    {
        m_UnloadSceneQueue.Clear();
        m_SceneQueue.Clear();
        if (!string.IsNullOrEmpty(mainSceneName))
        {
            string[] mains = mainSceneName.Split(',');
            if (mains == null)
            {
                return;
            }
            for (int i = 0; i < mains.Length; i++)
            {
                if (!string.IsNullOrEmpty(mains[i]))
                {
                    m_UnloadSceneQueue.Enqueue(mains[i]);
                }
            }
        }
        if (!string.IsNullOrEmpty(otherSceneName))
        {
            string[] others = otherSceneName.Split(',');
            if (others == null)
            {
                return;
            }
            for (int i = 0; i < others.Length; i++)
            {
                if (!string.IsNullOrEmpty(others[i]))
                {
                    m_SceneQueue.Enqueue(others[i]);
                }
            }
        }
        m_nLoadingSceneCount = m_SceneQueue.Count + m_UnloadSceneQueue.Count;
    }
    public void PreUnloadSceneAndLoadNewSceneAsync_AutoCloseUI(string unloadSceneName, string LoadNewSceneName = "", string resConfig = "", System.Action callback = null, string uiName = "UI_StoryLoading")
    {
        PreUnloadSceneAndLoadNewSceneAsync(unloadSceneName, LoadNewSceneName, resConfig, callback, uiName);
        _AutoCloseUI = true;
    }
    public void PreUnloadSceneAndLoadNewSceneAsync(string unloadSceneName, string LoadNewSceneName = "", string resConfig = "", System.Action callback = null, string uiName = "UI_StoryLoading")
    {
        if (_LoadingState != LoadingState.load_null)
        {
            return;
        }
        _AutoCloseUI = false;
        _PreLoadFile = resConfig;
        UnloadSceneAndLoadNewSceneAsync(unloadSceneName, LoadNewSceneName, callback, uiName);
    }
    public void UnloadSceneAndLoadNewSceneAsync_AutoCloseUI(string unloadSceneName, string LoadNewSceneName, System.Action callback = null, string uiName = "UI_StoryLoading")
    {
        UnloadSceneAndLoadNewSceneAsync(unloadSceneName, LoadNewSceneName, callback, uiName);

    }
    public void UnloadSceneAndLoadNewSceneAsync(string unloadSceneName, string LoadNewSceneName, System.Action callback = null, string uiName = "UI_StoryLoading")
    {
        if (_LoadingState != LoadingState.load_null)
        {
            return;
        }
        if (C_MonoSingleton<C_SceneMgr>.GetInstance().IsLoading)
        {
            return;
        }
        if (string.IsNullOrEmpty(uiName) || string.IsNullOrEmpty(uiName))
        {
            return;
        }
        OnCompelete = callback;
        //打开界面
        OpenUI(uiName);
        AddUnLoadSceneQueue(unloadSceneName, LoadNewSceneName);
        StartUnLoad();

       // m_nLoadingSceneCount = m_SceneQueue.Count;
        _AutoCloseUI = true;

    }

    private void StartLoad()
    {
        _Action = null;

        //_Isloading = true;
        _Action = delegate
        {
            C_MonoSingleton<C_SceneMgr>.GetInstance().LoadScene(m_SceneQueue.Dequeue(), "StageLoading", () =>
            {
                NextStep();
            });
        };
        if (!string.IsNullOrEmpty(_PreLoadFile))
        {
            //  _StartPreloadRes = true;
            _LoadingState = LoadingState.load_ab;
            C_MonoSingleton<PreLoadResMgr>.Instance.StartLoadRes(_PreLoadFile);
        }
        else
        {
            //_StartPreloadRes = false;
            _LoadingState = LoadingState.load_scene;

            _Action();
        }
    }
    private void StartUnLoad()
    {
        // _Isloading = true;
        _Action = null;

        _Action = delegate
        {
            C_MonoSingleton<C_SceneMgr>.GetInstance().UnloadSceneAsync(m_UnloadSceneQueue.Dequeue(), "StageLoading", () =>
            {
                UnLoadNextStep();
            });
        };
        // _Action();
        // _StartPreloadRes = false;
        if (!string.IsNullOrEmpty(_UnloadResFile))
        {
            //  _StartPreloadRes = true;
            C_MonoSingleton<PreLoadResMgr>.Instance.ForceUnloadABRes(_UnloadResFile);
            _LoadingState = LoadingState.unload_ab;

        }
        else
        {
            //_StartPreloadRes = false;
            _LoadingState = LoadingState.unload_scene;

            _Action();
        }
    }
    /// <summary>
    /// 先加载一个loading界面，再开始卸载场景和资源，客户端调用之后，需要对loading界面进行清除
    /// </summary>
    private void StartUnloadWithLoadingScene()
    {
        if (m_SceneQueue.Count > 0)
        {
            C_MonoSingleton<C_SceneMgr>.GetInstance().LoadScene(m_SceneQueue.Dequeue(), "", () =>
            {
                StartUnloadWithLoadingScene();
            }, true, LoadSceneMode.Single, _MainSceneName);
        }
        else
        {

            _LoadingState = LoadingState.unload_ab_only;
            if (!string.IsNullOrEmpty(_UnloadResFile))
            {
                C_MonoSingleton<PreLoadResMgr>.Instance.ForceUnloadABRes(_UnloadResFile);
            }
        }
    }
}
