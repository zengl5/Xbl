using Assets.Scripts.C_Framework;
using UnityEngine;
using XWK.Common.UI_Reward;

namespace Slate
{
    public class GuideCutsceneSequencePlayer : CutsceneSequencePlayer
    {
        [Header("第一次跳过镜头,下一个镜头名字")]
        public string m_NextCutsceneName;

        protected int _GuideEndCutsceneId;
        protected string _EndCutsceneName;

        // Use this for initialization
        private void Start()
        {
            YB.XWK.MainScene.LocalData.m_story_moudle = _CurrentMoudle = "xsyd";
            //埋点
            RewardUIManager.GetInstance().ChangeModule(ModuleType.Guide, string.Concat(_CurrentMoudle, "_moudle_reward"));
            base.DoStart();
            GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, "xsyd_start");

            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeStart, "story_time", _CurrentMoudle);

        }

        protected override void DoPlay()
        {
            if (playOnStart)
            {
                if (!C_DebugHelperMode)
                {
                    cutscenes.Clear();
                    if (StaticPlayQueMode)
                    {
                        InitCutScene();
                    }
                    else
                    {
                        InitCutsceneDynamic();
                    }
                }

                if (isPlaying)
                {
                    C_DebugHelper.LogWarning("Sequence is already playing" + gameObject);
                    return;
                }

                if (cutscenes.Count == 0)
                {
                    C_DebugHelper.LogError("No Cutscenes provided" + gameObject);
                    return;
                }
                _GuideEndCutsceneId = getCutsceneId(m_NextCutsceneName);
                _EndCutsceneName = cutscenes[cutscenes.Count - 1].name;

                isPlaying = true;

                currentIndex = cutscenes.Count - 1;

#if UNITY_EDITOR
                if (AppInfoData.GuideStateData == 0)
                {
                    currentIndex = 0;
                }
                else if (AppInfoData.GetJingubangStateData == 0)
                {
                    currentIndex = _GuideEndCutsceneId;
                }

             //   C_DebugHelperMode = false;
                if (!C_DebugHelperMode)
                {
                    currentIndex = 0;
                }
#else
                if(AppInfoData.GuideStateData == 0)
                {
                    currentIndex = 0;
                }
                else if (AppInfoData.GetJingubangStateData == 0)
                {
                    currentIndex = _GuideEndCutsceneId;
                }
#endif

                MoveNext();
            }
        }

        protected override void OpenStoryUI()
        {
            GuidePauseUIMgr.Instance.OpenStoryUI();
        }
         
        public void JumpCutscene(int time)
        {
            RewardUIManager.GetInstance().ChangeModule(ModuleType.Guide,"");

            RewardUIManager.GetInstance().SetFail();
            if (cutscenes != null && currentIndex >= 0 && cutscenes[currentIndex] != null)
                GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, "xsyd_jump", cutscenes[currentIndex].name);

            if (AppInfoData.GuideStateData == 0)
            {
                UpdateData();

                if (time == 1)
                {
                    GuidePauseUIMgr.Instance.ClosePauseUI();
                    Stop();
                    isPlaying = true;
                    _GuideEndCutsceneId = getCutsceneId(m_NextCutsceneName);
                    currentIndex = _GuideEndCutsceneId;
                    MoveNext();
                }
                else if (time == 2)
                {
                    GuidePauseUIMgr.Instance.Stop();
                    Stop();
                    isPlaying = true;
                    currentIndex = cutscenes.Count - 1;
                    MoveNext();
                }
            }
            else
            {
                RewardUIManager.GetInstance().SetScoreVisible(false);

                UpdateData();

                GuidePauseUIMgr.Instance.Stop();
                Stop();
                isPlaying = true;
                currentIndex = cutscenes.Count - 1;
                MoveNext();
            }
        }

        protected override void DoClick()
        {
            if (!string.IsNullOrEmpty(YB.XWK.MainScene.LocalData.m_story_moudle))
            {
                if ("xsyd_cam01".Equals(cutscenes[currentIndex].name)
                    || "xsyd_cam02".Equals(cutscenes[currentIndex].name)
                    || "xsyd_cam10".Equals(cutscenes[currentIndex].name)
                    || "xsyd_cam15_game".Equals(cutscenes[currentIndex].name))
                {
                    C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, cutscenes[currentIndex].name);
                }
                else
                {
                    C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, YB.XWK.MainScene.LocalData.m_story_moudle, cutscenes[currentIndex].name);
                }
            }
        }

        protected override void MoveNext()
        {

            if (cutscenes.Count - 1 <= 0)
            {
                RewardUIManager.GetInstance().ChangeModule(ModuleType.Guide,"");

                RewardUIManager.GetInstance().SetScoreVisible(false);

                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, "story_time", _CurrentMoudle);
            }
            if (currentIndex >= cutscenes.Count)
            {
                GuidePauseUIMgr.Instance.Stop();
                return;
            }
            base.MoveNext();
            UpdateData();
        }

        protected void UpdateData()
        {
            if (_CurrentCutScene != null)
            {
                if (getCutsceneId(_CurrentCutScene.name) == getCutsceneId(m_NextCutsceneName))
                {
                    AppInfoData.GuideStateData = 1;
                }
                if (_CurrentCutScene.name.Equals(_EndCutsceneName))
                {
                    AppInfoData.GuideStateData = 1;
                    AppInfoData.GetJingubangStateData = 1;
                    GuidePauseUIMgr.Instance.Stop();
                }
            }
        }

        public int getCutsceneId(string name)
        {
            if (cutscenes == null)
            {
                return -1;
            }
            int id = -1;
            for (int i = 0; i < cutscenes.Count; i++)
            {
                if (cutscenes[i].name.Equals(name))
                {
                    id = i;
                    break;
                }
            }
            if (id == -1)
            {
                C_DebugHelper.LogError("m_GuideEndCutsceneName is not exited...");
            }

            return id;
        }
    }
}