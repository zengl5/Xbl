using Assets.Scripts.C_Framework;
using UnityEngine;
using XWK.Common.UI_Reward;

namespace Slate.ActionClips
{
    [Category("Utility")]
    [Description("变声处理")]
    public class SpeechModefyToneSlate : DirectorActionClip
    {
        [Tooltip("播放录制声音的剧本名字")]
        public string playSectionName;

        [Tooltip("有录制声音的播放剧本名字")]
        public string rightSectionName;

        [Tooltip("没录制到声音的播放剧本名字")]
        public string errorSectionName;

        [Tooltip("允许没有录制到声音的次数，默认1次")]
        public int m_TotalErrorTime = 1;

        [Tooltip("变声的播放速度")]
        public float m_Pitch = 1.2f;

        private bool m_Stop = false;
        private int _ErrorTime = 0;
        private bool _NeedPlayOver = false;
        public bool _TestPlayMode = false;

        protected override bool OnInitialize()
        {
            return true;
        }

        protected override void OnEnter()
        {
#if !UNITY_EDITOR
            _TestPlayMode = false;
#endif
            _NeedPlayOver = false;
            m_Pitch = 1.2f;
            m_Stop = false;
            //显示倒计时ui
            RewardUIManager.Instance.RegisterStory(MotionType.SR, SourceType.Interaction, 9, (b) =>
            {
                C_DebugHelper.LogErrorFormat("SpeechModefyToneSlate OnEnter line47 ...b：" + b);

                if (!b)
                    DoStopSpeechModefy();
            });

            CutsceneSequencePlayer.PauseCurrentCutscene();
            SpeechSystemMgr.Instance.EnterSpeechModefiedToneSlate(m_Pitch, (b) =>
            {
#if UNITY_EDITOR
                if (_TestPlayMode)
                {
                    b = true;

                    string temp;
                    temp = errorSectionName;
                    errorSectionName = playSectionName;
                    playSectionName = temp;
                }
                b = false;//用来调试变调成功或者失败，true是成功
#endif
                RewardUIManager.Instance.PauseCountDown();

                if (!b)
                {
                    //_ErrorTime++;
                    //if (_ErrorTime > m_TotalErrorTime)
                    //{
                    //    DoStopSpeechModefy();
                    //}
                    //else
                    //{
                    //    CutsceneSequencePlayer._CurrentCutScene.Xbl_JumpToSection(errorSectionName);
                    //}
                    // RewardUIManager.Instance.SetSuccess();
                    // DoStopSpeechModefy();
                    RewardUIManager.Instance.SetFail();
                }
                else
                {
                    AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_054.ogg");
                    //增加播放人声，使用播放声音音效接口，在播放声音的过程中，让美术同事暂停故事
                    //AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_064.ogg");
                    if (!string.IsNullOrEmpty(playSectionName))
                    {
                        CutsceneSequencePlayer._CurrentCutScene.Xbl_JumpToSection(playSectionName);
                    }
                    _NeedPlayOver = true;
                    CutsceneSequencePlayer.ResumeCurrentCutscene();
                }
            }, PlayOver);
        }

        public void PlayOver()
        {
            C_DebugHelper.LogErrorFormat("SpeechModefyToneSlate PlayOver line101..._NeedPlayOver：" + _NeedPlayOver);

            if (_NeedPlayOver)
            {
                RewardUIManager.Instance.ResumeCountDown();
                RewardUIManager.Instance.SetSuccess();
                DoStopSpeechModefy();
            }
        }

        public void DoStopSpeechModefy()
        {
            AudioManager.Instance.StopAllEffect();
            CutsceneSequencePlayer.ResumeCurrentCutscene();
            CutsceneSequencePlayer._CurrentCutScene.Xbl_JumpToSection(rightSectionName);
            SpeechSystemMgr.Instance.Stop();
        }
    }
}