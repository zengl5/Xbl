using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Slate.ActionClips
{
    [Category("SceneLoad")]
    public class UnloadAndGameLoad : DirectorActionClip, ISubClipContainable
    {
        protected enum UnloadAndGameLoadState
        {
            start,
            sceneload,
            over,
        }

        protected UnloadAndGameLoadState _UnloadAndGameLoadState;
          
        public string mainScenName;
  
        public bool newApi = false;

        [Header("游戏跳出的下一个场景名字")]
        public string NextStorySceneName;

        protected C_Event _MainSceneLoadEvent = new C_Event();

        protected C_Event _SceneLoadEvent = new C_Event();

        [SerializeField]
        [HideInInspector]
        private float _length = 5;
        
        public override float length
        {
            get { return _length; }
            set { _length = value; }
        }

        float ISubClipContainable.subClipOffset
        {
            get { return timeOffset; }
            set { timeOffset = value; }
        }

        protected override bool OnInitialize()
        {
            return true;
        }
        protected override void OnEnter()
        {
            CutsceneSequencePlayer.PauseCurrentCutscene();

            _UnloadAndGameLoadState = UnloadAndGameLoadState.start;

            _SetActive = false;

            mStop = false;

            _SceneLoadEvent.RegisterEvent(C_EnumEventChannel.Global, "UnloadAndSceneLoadEvent", (object[] result) => { UnloadAndSceneLoadEvent(result); });
            _MainSceneLoadEvent.RegisterEvent(C_EnumEventChannel.Global, "MainSceneLoadEvent", (object[] result) => { MainSceneLoadEvent(result); });

            //if (string.IsNullOrEmpty(oldScenName))
            //{
            //    C_DebugHelper.LogError("scenename is null");
            //    return;
            //}
            //  Game_Minor.G_MinorCoreConfigData.SetGameCoreName(newScenName, NextStorySceneName);
            string sceneName = string.Empty;
            if (newApi)
            {
             //   string sceneName = Game_Minor.G_MinorCoreConfigData.GetLoadNextSceneName();
                if (string.IsNullOrEmpty(uiName))
                    SceneLoadingMgr.GetInstance().PreUnloadSceneAndLoadNewSceneAsync(oldScenName, sceneName, loadConfig, () => { LoadOver(); });
                else
                    SceneLoadingMgr.GetInstance().PreUnloadSceneAndLoadNewSceneAsync(oldScenName, sceneName, loadConfig, () => { LoadOver(); }, uiName);

            }
            else
            {
		        if (string.IsNullOrEmpty(uiName))
		            SceneLoadingMgr.GetInstance().PreUnloadSceneAndLoadNewSceneAsync(oldScenName, newScenName, loadConfig, () => { LoadOver(); });
		        else
		            SceneLoadingMgr.GetInstance().PreUnloadSceneAndLoadNewSceneAsync(oldScenName, newScenName, loadConfig, () => { LoadOver(); }, uiName);
                _ActiveSceneName = newScenName;

             }
        }
        //进入游戏
        protected void LoadOver()
        {
         //   _ActiveSceneName = Game_Minor.G_MinorCoreConfigData.GetBaseSceneName(newApi);

            SetMainScene(_ActiveSceneName);
        }
        //切换场景
        protected void MainSceneLoadEvent(object[] result)
        {
            _UnloadAndGameLoadState = UnloadAndGameLoadState.sceneload;

            _ActiveSceneName = result[0].ToString();

            SetMainScene(_ActiveSceneName);
        }
        //离开游戏
        protected void UnloadAndSceneLoadEvent(object[] result)
        {
            _UnloadAndGameLoadState = UnloadAndGameLoadState.over;

            mStop = true;

            _ActiveSceneName = result[0].ToString();

            SetMainScene(_ActiveSceneName);
        }
        protected  void OnSceneChanged(Scene scene1, Scene scene2)
        {
            switch (_UnloadAndGameLoadState)
            {
                case UnloadAndGameLoadState.start:
                case UnloadAndGameLoadState.sceneload:
                    {
                        CloseUI();
                    }
                    break;
                case UnloadAndGameLoadState.over:
                    {
                        if (mStop)
                        {
                            _SceneLoadEvent.UnregisterEvent();
                            _MainSceneLoadEvent.UnregisterEvent();

                            CutsceneSequencePlayer.StopCurrentCutscene();

                            //关闭ui
                            CloseUI();
                        }
                    }
                    break;
                default:
                    break;
            }
           
        }
       

        protected override void OnUpdate(float time)
        {
            DoUpdate();
        }




        //parent
        public float timeOffset;

        public float subClipOffset
        {
            get { return timeOffset; }
            set { timeOffset = value; }
        }

        [Header("需要去掉的场景名字")]
        public string oldScenName;

        [Header("需要添加的场景名字")]
        public string newScenName;

        protected string uiName = "UI_StoryLoading";

        public string loadConfig = "";

        protected bool mStop = false;

        protected bool _SetActive = false;

        protected string _ActiveSceneName;

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnClipGUI(Rect rect)
        {
            GUI.DrawTexture(new Rect(0, 0, rect.height, rect.height), Slate.Styles.cutsceneIcon);
        }

#endif
       
        public void SetMainScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                C_DebugHelper.LogError("mainScenName is null");
                return;
            }
            Scene activescene = SceneManager.GetActiveScene();
            if (activescene != null && activescene.name.Equals(sceneName))
            {
                OnSceneChanged(activescene, activescene);
            }
            else
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    if (sceneName.Equals(SceneManager.GetSceneAt(i).name))
                    {
                        SceneManager.SetActiveScene(SceneManager.GetSceneAt(i));
                        _SetActive = true;
                        break;
                    }
                }
            }

        }

        protected virtual void DoUpdate()
        {
            if (!mStop && !CutsceneSequencePlayer.isPause())
            {
                CutsceneSequencePlayer.PauseCurrentCutscene();
            }
            if (_SetActive)
            {
                //判断当前的场景是否为active 场景
                Scene activeScene = SceneManager.GetActiveScene();
                if (activeScene != null && activeScene.name.Equals(_ActiveSceneName))
                {
                    _SetActive = false;
                    OnSceneChanged(activeScene, activeScene);
                }
            }
        }

        protected void CloseUI()
        {
            SceneLoadingMgr.GetInstance().CloseUI();
        }

    }

}

