using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Slate.ActionClips
{
    [Category("BgMusic")]
    [Description("继续播放背景音")]
    public class ResumeBg : DirectorActionClip
    {
        protected override void OnEnter()
        {
            if (UnityEngine.Application.isPlaying)
            {
                AudioManager.Instance.ResumeBgMusic();
            }
        }
    }
}