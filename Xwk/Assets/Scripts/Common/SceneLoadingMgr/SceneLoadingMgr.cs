using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingMgr : C_MonoSingleton<SceneLoadingMgr> {
    private UI_StoryLoading _UI_StoryLoading;
    private int m_nLoadingSceneCount = -1;
    private Queue<string> m_SceneQueue = new Queue<string>();
    private Queue<string> m_UnloadSceneQueue = new Queue<string>();
    private float _CurSliderValue = 0f;
    public System.Action OnCompelete;
    public string _PreLoadFile= string.Empty;
    public bool _StartPreloadRes = false;
    private System.Action _Action = null;
    private bool _Isloading = false;
    private float _CurrentPrecent = 0.8f;
    private string _MainSceneName=string.Empty;
    private bool _AutoCloseUI = true;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(_UI_StoryLoading != null)
        {
            if (_StartPreloadRes)//预加载
            {
                _CurSliderValue =  C_MonoSingleton<PreLoadResMgr>.Instance.getCurrentLoadProgress(_CurrentPrecent);
                if (_CurSliderValue >= _CurrentPrecent && !string.IsNullOrEmpty(_PreLoadFile))
                {
                    _Isloading = false;

                    _StartPreloadRes = false;
                    if (_Action != null)
                    {
                        _Action();
                    }
                }
            }
            else
            {
                float loadProgress = (m_nLoadingSceneCount - m_SceneQueue.Count-1 + C_MonoSingleton<C_SceneMgr>.GetInstance().Progress/100) / (float)m_nLoadingSceneCount;

                if (string.IsNullOrEmpty(_PreLoadFile))
                {
                    _CurSliderValue = loadProgress;
                }
                else
                {
                    _CurSliderValue = loadProgress*(1- _CurrentPrecent) + _CurrentPrecent;
                }
            }
            _UI_StoryLoading.UpdateProgressVaule(_CurSliderValue);
            
        }
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
             _StartPreloadRes = false;
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
            if (!string.IsNullOrEmpty(_PreLoadFile))
            {
                _StartPreloadRes = true;
                C_MonoSingleton<PreLoadResMgr>.Instance.StartLoadRes(_PreLoadFile);
            }
            NextStep();

        }
    }
    public void LoadCompelete()
    {
        _Isloading = false;
        _StartPreloadRes = false;
       
        if (OnCompelete != null)
        {
            OnCompelete();
        }
        if (_UI_StoryLoading != null && _AutoCloseUI)
            _UI_StoryLoading.CloseLoadingUI();
    }
  
    public void PreLoadMulitSceneAsync(string mainSceneName, string otherSceneName = "",  string resConfig="" , System.Action callback = null, string uiName = "UI_StoryLoading")
    {
        if (_Isloading)
        {
            return;
        }
        _AutoCloseUI = true;
        _PreLoadFile = resConfig;
        LoadMulitSceneAsync(mainSceneName, otherSceneName, callback, uiName);
    }
    public void LoadScene(string mainSceneName, System.Action callback = null, string uiName = "UI_StoryLoading")
    {
        LoadMulitSceneAsync(mainSceneName,"", callback, uiName);
    }
    public void LoadMulitSceneAsync(string mainSceneName,  string otherSceneName  = "", System.Action callback = null, string uiName = "UI_StoryLoading")
    {
        if (_Isloading)
        {
            return;
        }
        if (C_MonoSingleton<C_SceneMgr>.GetInstance().IsLoading )
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
        //_UI_StoryLoading = GameObject.Find(uiName).GetComponent<UI_StoryLoading>();
        _UI_StoryLoading = FindObjectOfType<UI_StoryLoading>();
    }
    public void AddLoadSceneQueue(string mainSceneName,string otherSceneName)
    {
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
        if (  !string.IsNullOrEmpty(mainSceneName))
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
        if ( !string.IsNullOrEmpty(otherSceneName))
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
        m_nLoadingSceneCount = m_SceneQueue.Count+m_UnloadSceneQueue.Count;
    }
    public void PreUnloadSceneAndLoadNewSceneAsync_AutoCloseUI(string unloadSceneName, string LoadNewSceneName = "", string resConfig = "", System.Action callback = null, string uiName = "UI_StoryLoading")
    {
        PreUnloadSceneAndLoadNewSceneAsync(unloadSceneName, LoadNewSceneName, resConfig, callback, uiName);
        _AutoCloseUI = true;
    }
    public void PreUnloadSceneAndLoadNewSceneAsync(string unloadSceneName, string LoadNewSceneName = "", string resConfig = "", System.Action callback = null, string uiName = "UI_StoryLoading")
    {
        if (_Isloading)
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
        if (_Isloading)
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
        if (string.IsNullOrEmpty(unloadSceneName) && string.IsNullOrEmpty(LoadNewSceneName)  )
        {
            return;
        }
        OnCompelete = callback;
        //打开界面
        OpenUI(uiName);
        AddUnLoadSceneQueue(unloadSceneName, LoadNewSceneName);
        if (string.IsNullOrEmpty(unloadSceneName))
        {
            _MainSceneName = LoadNewSceneName.Split(',')[0];
            StartAddScene();
        }
        else
        {
            StartUnLoad();
        }

        m_nLoadingSceneCount = m_SceneQueue.Count;
        _AutoCloseUI = true;

    }
    private void StartAddScene()
    {
        _Isloading = true;
        _Action = delegate
        {
            NextStep();
        };
        if (!string.IsNullOrEmpty(_PreLoadFile))
        {
            _StartPreloadRes = true;
            C_MonoSingleton<PreLoadResMgr>.Instance.StartLoadRes(_PreLoadFile);
        }
        else
        {
            _StartPreloadRes = false;
            _Action();
        }
    }
    private void StartLoad()
    {
        _Isloading = true;
        _Action = delegate
        {
            C_MonoSingleton<C_SceneMgr>.GetInstance().LoadScene(m_SceneQueue.Dequeue(), "StageLoading", () =>
            {
                NextStep();
            });
        };
        if (!string.IsNullOrEmpty(_PreLoadFile))
        {
             _StartPreloadRes = true;
            C_MonoSingleton<PreLoadResMgr>.Instance.StartLoadRes(_PreLoadFile);
        }
        else
        {
            _StartPreloadRes = false;
            _Action();
        }
    }
    private void StartUnLoad()
    {
        _Isloading = true;

        _Action = delegate
        {
            C_MonoSingleton<C_SceneMgr>.GetInstance().UnloadSceneAsync(m_UnloadSceneQueue.Dequeue(), "StageLoading", () =>
            {
                //NextStep();
                 UnLoadNextStep();
            });
        };
        _Action();
        _StartPreloadRes = false;

    }

}
