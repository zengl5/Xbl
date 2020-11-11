using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class PlayAudioAndAnimAllOverState : AnimState
    {
        private PlayAnimState _PlayAnimState;
        private PlayAudioState _PlayAudioState;
        private bool audioOver = false;
        private bool animOver = false;
        public PlayAudioAndAnimAllOverState(IActorState actorState, Animator animator, InfoData infoData)
        {
            m_InfoData = infoData;
            m_Animator = animator;
            _PlayAnimState = new PlayAnimState(actorState, animator, infoData);
            _PlayAnimState.OnCompelete -= AnimOver;
            _PlayAnimState.OnCompelete += AnimOver;
            _PlayAudioState = new PlayAudioState(actorState, animator, infoData);
            _PlayAudioState.OnCompelete -= AudioOver;
            _PlayAudioState.OnCompelete += AudioOver;
        }
        public void AudioOver()
        {
            audioOver = true;
            if (audioOver && animOver && OnCompelete!=null)
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
            if (_PlayAnimState!=null)
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

