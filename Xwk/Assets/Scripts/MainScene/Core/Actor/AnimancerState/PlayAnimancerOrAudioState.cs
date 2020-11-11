using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 声音或者是动作结束了，就回调相对应的回调函数。需要注意需手动调用stop
/// </summary>
namespace YB.XWK.MainScene
{
    public class PlayAnimancerOrAudioState : AnimAncerAimState
    {
        private PlayAnimancerAnimState _PlayAnimState;
        private PlayAnimancerAudioState _PlayAudioState;
 
        public PlayAnimancerOrAudioState(IActorState actorState, AnimancerComponent animancerComponent, ActorAnimancerData stateInfo) : base()
        {
            _PlayAnimState = new PlayAnimancerAnimState(actorState, animancerComponent, stateInfo.anim);
            _PlayAnimState.OnCompelete -= PlayNext;
            _PlayAnimState.OnCompelete += PlayNext;
            _PlayAudioState = new PlayAnimancerAudioState(actorState, stateInfo.audio, stateInfo.audiotype);
            _PlayAudioState.OnCompelete -= AllOver;
            _PlayAudioState.OnCompelete += AllOver;
        }
        public void AllOver()
        {
           // Stop();
            OnFinish();
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
    public class PlayAnimancerAudioAndAnimWaitAnimOverState : AnimAncerAimState
    {
        private PlayAnimancerAnimState _PlayAnimState;
        private PlayAnimancerAudioState _PlayAudioState;
        private bool _IsOver = false;
        public PlayAnimancerAudioAndAnimWaitAnimOverState(IActorState actorState, AnimancerComponent animancerComponent, ActorAnimancerData stateInfo) : base()
        {
            _PlayAnimState = new PlayAnimancerAnimState(actorState, animancerComponent, stateInfo.anim);
            _PlayAnimState.OnCompelete -= AllOver;
            _PlayAnimState.OnCompelete += AllOver;
            _PlayAudioState = new PlayAnimancerAudioState(actorState, stateInfo.audio, stateInfo.audiotype);
            _IsOver = false;
        }
        public void AllOver()
        {
            Stop();
            OnFinish();
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

