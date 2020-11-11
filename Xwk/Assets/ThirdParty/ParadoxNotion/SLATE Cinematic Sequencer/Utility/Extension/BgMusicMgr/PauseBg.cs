using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace Slate.ActionClips
{
    [Category("BgMusic")]
    [Description("暂停背景音")]
    public class PauseBg : DirectorActionClip
    {
        protected override void OnEnter()
        {
            if (UnityEngine.Application.isPlaying)
            {
                AudioManager.Instance.PauseBgMusic();
            }
        }
    }
}
