using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Slate.ActionClips
{
    [Name("Stop Audio Clip")]
    [Description("停止该轨道上的语音")]
    [Attachable(typeof(ActorAudioTrack), typeof(DirectorAudioTrack))]

    public class StopAudio : ActionClip
    {
        private AudioTrack track
        {
            get { return (AudioTrack)parent; }
        }
        protected AudioSource source
        {
            get { return track.source; }
        }
        protected override void OnEnter() { Do(); }
        protected override void OnReverseEnter() { Do(); }
        protected override void OnExit() {  }
        protected override void OnReverse() {  }

        protected virtual void Do()
        {
            if (source != null)
            {
                source.Stop();
            }
        }
    }
}

