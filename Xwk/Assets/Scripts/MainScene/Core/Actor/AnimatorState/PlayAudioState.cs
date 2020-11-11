using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    /// <summary>
    /// 播放声音状态
    /// </summary>
    public  class PlayAudioState : AnimState
    {
      //  public IActorState m_ActorState;

        public PlayAudioState(IActorState actorState,Animator animator, InfoData infoData)
        {
            m_InfoData = infoData;
            m_Animator = animator;
            m_ActorState = actorState;
        }

        public override void Handle()
        {
            //播放动画，并且等待结束作为返回
            if (m_InfoData == null
                || (m_InfoData != null && string.IsNullOrEmpty(m_InfoData.audio)))
            {
                OnFinish();
                return;
            }
            //string audio = m_InfoData.audio;
            //if (!string.IsNullOrEmpty(m_InfoData.audio) && m_InfoData.audio.Contains(","))
            //{
            //    string[] data = m_InfoData.audio.Split(',');
            //    audio = data[Random.Range(0, data.Length)];
            //}
            //AudioManager.Instance.PlayerSound(audio, false, () => {
            //    OnFinish();
            //});

            m_ActorState.PlayAudio(m_InfoData.audiotype, m_InfoData.audio, () => {
                OnFinish();
            }, false);

        }

        public override void Stop()
        {
            OnCompelete = null;
            AudioManager.Instance.StopPlayerSound();
        }
    }
}
