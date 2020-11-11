using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class PlayAnimancerWaitAudioState : AnimAncerAimState
    {
        private PlayAnimancerAnimState _PlayAnimState;
        private PlayAnimancerAudioState _PlayAudioState;
 
        public PlayAnimancerWaitAudioState(IActorState actorState, AnimancerComponent animancerComponent, ActorAnimancerData stateInfo) : base()
        {
            _PlayAnimState = new PlayAnimancerAnimState(actorState, animancerComponent, stateInfo.anim);
          
            _PlayAudioState = new PlayAnimancerAudioState(actorState, stateInfo.audio, stateInfo.audiotype);
            _PlayAudioState.OnCompelete -= AudioOver;
            _PlayAudioState.OnCompelete += AudioOver;
        }
        public void AudioOver()
        {
            if (_PlayAnimState != null)
            {
                _PlayAnimState.Stop();
            }
            if ( OnCompelete != null)
            {
                OnCompelete();
                OnCompelete = null;
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

