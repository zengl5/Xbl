using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Slate.ActionClips {
    [Category("Utility")]
    [Description("播放提醒语音")]
    public class PlayWarrningAudio : DirectorActionClip, ISubClipContainable
    {
        [Tooltip("等待的时间")]
        public float time=3.0f;
        [Tooltip("播放声音的路径")]
        public string audioPath;
        [Tooltip("是否循环播放提示的语音")]
        public bool loop = true;
        private bool stop = false;

        [SerializeField]
        [HideInInspector]
        private float _length = 5;
        public override float length
        {
            get { return _length; }
            set { _length = value; }
        }
        public float timeOffset;
        public float subClipOffset
        {
            get { return timeOffset; }
            set { timeOffset = value; }
        }
        float ISubClipContainable.subClipOffset
        {
            get { return timeOffset; }
            set { timeOffset = value; }
        }

        protected override void OnEnter()
        {
            if (UnityEngine.Application.isPlaying)
            {
                stop = false;
                if (CutsceneSequencePlayer._CurrentCutScene == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(audioPath))
                {
                    return;
                }
                CutsceneSequencePlayer._CurrentCutScene.OnCutsceneResume -= CutsceneResume;
                CutsceneSequencePlayer._CurrentCutScene.OnCutsceneResume += CutsceneResume;
                DoPlay();
            }
        }
        protected void CutsceneResume(Cutscene cutscene)
        {
            if(cutscene !=null && cutscene == CutsceneSequencePlayer._CurrentCutScene){
                if (!stop && CutsceneSequencePlayer._CurrentCutScene != null)
                {
                    stop = true;
                    CutsceneSequencePlayer._CurrentCutScene.OnCutsceneResume -= CutsceneResume;
                    DoStop();
                }
            }
        }
        protected override void OnUpdate(float time)
        {
           
        }
        void DoPlay()
        {
            Invoke("PlaySound", time);
        }
        void DoStop()
        {
   
            stop = true;
            CancelInvoke("PlaySound");
            AudioManager.Instance.StopPlayerSound();
        }

        void PlaySound()
        {
          //  Debug.Log("PlaySound...");
            AudioManager.Instance.PlayerSound(audioPath,false,()=> {
                if (loop)
                {
                    DoPlay();
                }
            });
        }
    }

}


