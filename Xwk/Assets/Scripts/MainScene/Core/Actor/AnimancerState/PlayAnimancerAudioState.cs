using Animancer;
using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    
    public class PlayAnimancerAudioState : AnimAncerAimState
    {
        public string audio;
        public string audioType;
        public PlayAnimancerAudioState(IActorState actorState, string data,string type)
        {
            if (data.Contains(","))
            {
                string[] dataTmp = data.Split(',');
                audio = dataTmp[Random.Range(0, dataTmp.Length)];
            }
            else
            {
                audio = data;
            }
            audioType = type;
            m_ActorState = actorState;
        }
      
        public override void Handle()
        {
            //播放动画，并且等待结束作为返回
            if (string.IsNullOrEmpty(audio))
            {
                Stop();
                return;
            }
            m_ActorState.PlayAudio(audioType, audio, () => {
                OnFinish();
            }, false);

        }
        void Player()
        {
            OnFinish();
        }
        public override void Stop()
        {
            AudioManager.Instance.StopPlayerSound();
            OnCompelete = null;
        }
    }

}

