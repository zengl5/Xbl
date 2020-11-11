using XWK.Common.UI_Reward;

namespace Slate.ActionClips
{
    [Category("Utility")]
    [Description("音量识别")]
    public class PlayVolumeRecognize : DirectorActionClip
    {
        public bool mPauseScene = true;
        private int time = 0;

        protected override void OnEnter()
        {
            CutsceneSequencePlayer.PauseCurrentCutscene();
            SpeechSystemMgr.Instance.StartVolumeRecognition(Back);
            RewardUIManager.Instance.RegisterStory(MotionType.SR, SourceType.Interaction, 7, (b) =>
            {
                if (!b)
                    DoStop();
            });
        }

        private void Back(bool result)
        {
#if UNITY_EDITOR
            result = true;
#endif
            //RewardUIManager.Instance.SetSuccess();
            //DoStop();

            //if (result == true)
            //{
            //    DoStop();
            //}
            //else
            //{
            //    time++;
            //    if (time > 2)
            //    {
            //        DoStop();
            //    }
            //    else
            //    {
            //        SpeechSystemMgr.Instance.StartVolumeRecognition(Back);
            //    }
            //}

            if (result == true)
            {
                AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_054.ogg");
                RewardUIManager.Instance.SetSuccess();
                DoStop();
            }
            else
            {
                RewardUIManager.Instance.SetFail();
            }
        }

        public void DoStop()
        {
            CutsceneSequencePlayer.ResumeCurrentCutscene();
            SpeechSystemMgr.Instance.StopVolumeRecongition();
        }
    }
}