using UnityEngine;
using XWK.Common.UI_Reward;

namespace Slate.ActionClips
{
    [Category("Utility")]
    [Description("语音识别")]
    public class PlayReconginzeAudioSlate : DirectorActionClip
    {
        [Tooltip("当前需要识别的文字")]
        public string word;

        [Tooltip("当前剧本对象")]
        public Cutscene currentCutscene;

        [Tooltip("有声音的播放剧本名字")]
        public string rightSectionName;

        [Tooltip("没有声音的播放剧本名字")]
        public string errorSectionName;

        [Tooltip("是否暂停剧本播放")]
        public bool mPauseScene = true;

        private int _PlayTime = 0;
        private string rightEffectSound = "public/sound_effect/public_xwkyx_054.ogg";
      //  private string  errorEffectSound= "public/sound/public_bgm_027.ogg";

        protected override void OnEnter()
        {
            if (UnityEngine.Application.isPlaying)
            {
                if (mPauseScene)
                {
                    CutsceneSequencePlayer.PauseCurrentCutscene();
                }
                RewardUIManager.Instance.RegisterStory(MotionType.SR, SourceType.Interaction, 8, (b) =>
                {
                    //失败，正常播放剧情回调，成功不播放奖励ui的回调（动画延时，不等待）
                    if (!b)
                        DoSuccess();
                });
                SpeechSystemMgr.Instance.StartRecognizeAudioTecentSlate(word, (score) =>
                {
                    if (float.Parse(score) < 60.0f)
                    {
                        Back(false);
                    }
                    else
                    {
                        Back(true);
                    }
                });
            }
        }

        private void DoSuccess()
        {
            CutsceneSequencePlayer.ResumeCurrentCutscene();
            if (!string.IsNullOrEmpty(rightSectionName))
            {
                CutsceneSequencePlayer._CurrentCutScene.Xbl_JumpToSection(rightSectionName);
            }
            AudioManager.Instance.PlayEffectSound(rightEffectSound);
        }

        private void Back(bool right)
        {
            //XWK.Common.UI_Reward.RewardUIManager.Instance.SetSuccess();
            //DoSuccess();

            //if (right)
            //{
            //    currentCutscene.Xbl_JumpToSection(rightSectionName);
            //    //  root.Xbl_JumpToSection(rightSectionName);
            //    AudioManager.Instance.PlayEffectSound(errorEffectSound);
            //}
            //else
            //{
            //    _PlayTime++;
            //    if (_PlayTime >= 1)
            //    {
            //        currentCutscene.Xbl_JumpToSection(rightSectionName);
            //        AudioManager.Instance.PlayEffectSound(errorEffectSound);
            //    }
            //    else
            //    {
            //        currentCutscene.Xbl_JumpToSection(errorSectionName);
            //        AudioManager.Instance.PlayEffectSound(rightEffectSound);
            //    }
            //}
            if (right)
            {
                //成功，播放奖励同时播放剧情
                XWK.Common.UI_Reward.RewardUIManager.Instance.SetSuccess();
                DoSuccess();
            }
            else
            {
                //失败，直接奖励结束，播放剧情
                XWK.Common.UI_Reward.RewardUIManager.Instance.SetFail();
            }
        }
    }
}