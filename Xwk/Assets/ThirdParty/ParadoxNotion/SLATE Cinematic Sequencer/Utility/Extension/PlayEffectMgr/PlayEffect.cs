using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
namespace Slate.ActionClips
{
    [Category("Effect")]
    [Description("播放音效")]
    public class PlayEffect : DirectorActionClip
    {
        public string effectName;
        public bool loop = false;
        protected override void OnEnter()
        {
            if (UnityEngine.Application.isPlaying)
            {
                AudioManager.Instance.PlayEffectSound(effectName, loop);
            }
        }
    }
}
