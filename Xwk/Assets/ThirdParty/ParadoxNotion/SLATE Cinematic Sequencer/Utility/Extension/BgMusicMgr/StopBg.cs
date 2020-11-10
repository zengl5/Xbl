using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Slate.ActionClips
{
    [Category("BgMusic")]
    [Description("停止背景音")]
    public class StopBg : DirectorActionClip
    {
        protected override void OnEnter()
        {
            if (UnityEngine.Application.isPlaying)
            {
                AudioManager.Instance.StopBgMusic();
            }
        }
    }
}