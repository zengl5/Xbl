using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Slate.ActionClips
{
    [Category("BgMusic")]
    [Description("淡入背景音")]
    public class BgMusicFadeInSlate : DirectorActionClip
    {
        public string musicName = string.Empty;
        public bool loop = true;
        [Tooltip("淡入的时间")]
        public float fadeTime = 0f;
        public float fadeVulume = 1f;
        protected override void OnEnter()
        {
            if (UnityEngine.Application.isPlaying)
            {
                AudioManager.Instance.PlayFadeInBgMusic(musicName, false, loop, fadeTime, fadeVulume);
            }
        }
#if UNITY_EDITOR
        public override string GetAffectResPath()
        {
            //return base.GetAffectResPath();
            return musicName;
        }
#endif
    }

}

