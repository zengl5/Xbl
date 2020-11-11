using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class PlayAnimancerAllOverState : AnimAncerAimState
    {
        private PlayAnimancerAnimState _PlayAnimState;
        private PlayAnimancerAudioState _PlayAudioState;
        private bool audioOver = false;
        private bool animOver = false;
        public PlayAnimancerAllOverState(IActorState actorState, AnimancerComponent animancerComponent, ActorAnimancerData stateInfo) : base()
        {
            _PlayAnimState = new PlayAnimancerAnimState(actorState, animancerComponent, stateInfo.anim);
            _PlayAnimState.OnCompelete -= AnimOver;
            _PlayAnimState.OnCompelete += AnimOver;
            _PlayAudioState = new PlayAnimancerAudioState(actorState, stateInfo.audio, stateInfo.audiotype);
            _PlayAudioState.OnCompelete -= AudioOver;
            _PlayAudioState.OnCompelete += AudioOver;
        }
        public void AudioOver()
        {
            audioOver = true;
            if (audioOver && animOver && OnCompelete != null)
            {
                OnCompelete();
            }
        }
        public void AnimOver()
        {
            animOver = true;
            if (audioOver && animOver && OnCompelete != null)
            {
                OnCompelete();
            }
        }
        public override void Handle()
        {
            if (_PlayAnimState != null)
            {
                _PlayAnimState.Handle();
            }
            if (_PlayAudioState != null)
            {
                _PlayAudioState.Handle();
            }
        }

        public override void Stop()
        {
            if (_PlayAnimState != null)
            {
                _PlayAnimState.Stop();
            }
            if (_PlayAudioState != null)
            {
                _PlayAudioState.Stop();
            }
        }
    }
}

