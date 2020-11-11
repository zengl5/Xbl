using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Slate.ActionClips
{
    [Category("BgMusic")]
    [Description("淡出背景音")]
    public class BgMusicFadeOutSlate : DirectorActionClip
    {
        public string musicName = string.Empty;
        public bool loop = true;
        [Tooltip("淡出的时间")]
        public float fadeTime = 0f;
        public float fadeVulume = 0f;
        protected override void OnEnter()
        {
            if (UnityEngine.Application.isPlaying)
            {
                AudioManager.Instance.PlayFadeOutBgMusic(musicName, false, loop, fadeTime, fadeVulume);

            }
        }

    }
}
