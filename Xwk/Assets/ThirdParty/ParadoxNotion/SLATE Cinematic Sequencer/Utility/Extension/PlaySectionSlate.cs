using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Slate.ActionClips
{
    [Category("Utility")]
    [Description("播放剧本")]
    public class PlaySectionSlate : DirectorActionClip
    {
        //public Cutscene cutscene;
        public Slate.Cutscene.WrapMode playMode;
        public string sectionName;
        protected override void OnEnter()
        {
            if (UnityEngine.Application.isPlaying)
            {
                if (CutsceneSequencePlayer._CurrentCutScene == null)
                {
                    return;
                }
                CutsceneSequencePlayer._CurrentCutScene.Xbl_JumpToSection(sectionName);
            }
        }
    }

}
