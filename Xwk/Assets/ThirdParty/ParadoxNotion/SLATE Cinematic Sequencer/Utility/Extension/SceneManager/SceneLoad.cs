using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Slate.ActionClips
{
    [Category("SceneLoad")]
    public class SceneLoad : DirectorActionClip, ISubClipContainable
    {
        public string mainScenName;
        public string uiName= "UI_StoryLoading";
        public string otherSceneName;
        public string loadConfig = "";
        private bool mStop = false;
        protected override void OnEnter()
        {
            if (string.IsNullOrEmpty(mainScenName))
            {
                Debug.LogError("scenename is null");
                return;
            }
            CutsceneSequencePlayer.PauseCurrentCutscene();
             mStop = false;
            if (string.IsNullOrEmpty(uiName)) SceneLoadingMgr.GetInstance().PreLoadMulitSceneAsync(mainScenName, otherSceneName, loadConfig, () => { Back(); });
            else SceneLoadingMgr.GetInstance().PreLoadMulitSceneAsync(mainScenName, otherSceneName, loadConfig, () => { Back(); }, uiName);
        }

        void Back()
        {
            CutsceneSequencePlayer.StopCurrentCutscene();
        }
        public float timeOffset;
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
        protected override void OnUpdate(float time)
        {
            
            if (!mStop && !CutsceneSequencePlayer.isPause())
            {
                CutsceneSequencePlayer.PauseCurrentCutscene();
            }
        }
        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnClipGUI(Rect rect)
        {
            GUI.DrawTexture(new Rect(0, 0, rect.height, rect.height), Slate.Styles.cutsceneIcon);
        }

#endif

    }

}

