using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace Slate.ActionClips
{
    [Category("BgMusic")]
    [Description("播放背景音")]
    public class PlayBgMusic : DirectorActionClip
    {
        public string musicName = string.Empty;
        public bool loop = true;
        public float volume = 1f;
        protected override void OnEnter()
        {
            if (UnityEngine.Application.isPlaying)
            {
                AudioManager.Instance.BgMusicVolume = volume;
                AudioManager.Instance.PlayBgMusic(musicName, false, loop);
            }
        }
    }
}