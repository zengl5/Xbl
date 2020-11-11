using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMultiSceneManager : MonoBehaviour
{
    public static LoadMultiSceneManager _Ins;
    public static LoadMultiSceneManager Instance
    {
        get
        {
            if(_Ins == null){
                GameObject go = new GameObject("LoadMultiSceneManager");
                _Ins = go.AddComponent<LoadMultiSceneManager>();
            }
            return _Ins;
        }

    }
    public string _curSceneName;//当前即将切换的场景名字
    public string _precacheSceneName;///缓存的场景名字
    public bool _fromOuterScene = false;

    public AsyncOperation _curResult;
    public AsyncOperation _precacheResult;
    void Start()
    {
        DontDestroyOnLoad(this);
    }
    //void OnGUI()
    //{
    //    if (GUILayout.Button("EnterScene cache 100002_01", GUILayout.Width(200)))
    //    {
    //        EnterScene("EnterScene", "100002_01");

    //        //DialogSceneMgr.Instance.InitScenes(new List<string>() { "BaseLight", "BaseLight2" });
    //    }

    //    if (GUILayout.Button("100002_01 cache 100002_02", GUILayout.Width(200)))
    //    {
    //        EnterScene("100002_01", "100002_02");
    //    }

    //    if (GUILayout.Button("SwitchOut 100002_01", GUILayout.Width(200)))
    //    {
    //        EnterOuterScene("100002_01");
    //    }
    //    if (GUILayout.Button("100002_02 cache EnterScene", GUILayout.Width(200)))
    //    {
    //        EnterScene("100002_02", "EnterScene");
    //    }
    //    if (_curResult != null)
    //        GUILayout.TextArea("CurProgress:" + _curResult.progress);
    //    if (_precacheResult != null)
    //        GUILayout.TextArea("PrecacheProgress:" + _precacheResult.progress);
    //}

    public void EnterScene(string sceneName, string precacheSceneName)
    {
        _curSceneName = sceneName;
        //没有precache的scene说明是第一次进入
        _fromOuterScene = string.IsNullOrEmpty(_precacheSceneName);

        //缓存的场景和即将进入的场景不一致直接Load场景
        if (_curSceneName != _precacheSceneName && !_fromOuterScene)
        {
            Clear();
            SceneManager.LoadScene(_curSceneName);
            return;
        }
        _precacheSceneName = precacheSceneName;
        StartCoroutine(LoadSceneCor());
    }

    private void Clear()
    {
        _precacheSceneName = null;
    }

    public void EnterOuterScene(string sceneName)
    {
        EnterScene(sceneName, null);
    }

    private IEnumerator LoadSceneCor()
    {
        //from prev to cur
        if (_fromOuterScene)
        {
            LoadAsyncApi(ref _curResult, _curSceneName);
            yield return StartCoroutine(LoadInProgress(_curResult));
            yield return StartCoroutine(LoadAfterProgress(_curResult, _curSceneName));
        }
        else
        {
            _curResult = _precacheResult;
            yield return StartCoroutine(LoadAfterProgress(_curResult, _curSceneName));

        }

     //   UnLoadPrevScene();
        SetActiveScene(_curSceneName);

        //precache next
        //没有缓存场景了，不需要缓存
        if (string.IsNullOrEmpty(_precacheSceneName))
        {
            yield break;
        }

        LoadAsyncApi(ref _precacheResult, _precacheSceneName);
        yield return StartCoroutine(LoadInProgress(_precacheResult));
    }
    
    private void UnLoadPrevScene()
    {
        Scene prevScene = SceneManager.GetActiveScene();
        AsyncOperation resultTmp = SceneManager.UnloadSceneAsync(prevScene.name);
    }

    private void SetActiveScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene != null)
        {
            SceneManager.SetActiveScene(scene);
        }
    }

    //0-0.9 
    private IEnumerator LoadInProgress(AsyncOperation result)
    {
        result.allowSceneActivation = false;
        while (result.progress < 0.9f)
        {
            yield return null;
        }
    }
    private void LoadAsyncApi(ref AsyncOperation result, string sceneName)
    {
        result = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    //0.9-1在之前预加载中已经加载90%，切换到缓存场景加载剩余的10%
    private IEnumerator LoadAfterProgress(AsyncOperation result, string sceneName)
    {
        result.allowSceneActivation = true;
        while (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            yield return null;
        }
    }
}
