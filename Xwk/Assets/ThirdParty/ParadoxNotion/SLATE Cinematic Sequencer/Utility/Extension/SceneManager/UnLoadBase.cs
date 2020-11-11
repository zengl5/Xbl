using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Slate.ActionClips
{
    public class UnLoadBase : DirectorActionClip
    {
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
        protected virtual void OnSceneChanged(Scene scene1, Scene scene2)
        {

        }
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

