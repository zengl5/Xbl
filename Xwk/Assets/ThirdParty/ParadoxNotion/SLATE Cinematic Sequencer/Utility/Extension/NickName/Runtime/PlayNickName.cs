using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Slate.ActionClips
{
    [Name("Play NickName Clip")]
    [Description("播放小名语音")]
    [Attachable(typeof(ActorAudioTrack), typeof(DirectorAudioTrack))]
    public class PlayNickName : PlayAudio
    {
        public override void LoadAudioRes()
        {
            if (audioClip == null)
                audioClip = BabyName.c_BabyNameAudioClip;
        }
    }
}

