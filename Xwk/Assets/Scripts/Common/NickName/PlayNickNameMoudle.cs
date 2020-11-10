using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XBL.Core
{
    public class PlayNickNameMoudle:C_MonoSingleton<PlayNickNameMoudle>
    {
       protected AudioClip InitNickName()
        {
            return BabyName.c_BabyNameAudioClip;
        }
        //播放接口
        public void OnPlay(System.Action callback= null,bool loop = false )
        {
            AudioClip audioClip = InitNickName();
            if (audioClip != null)
            {
                AudioManager.Instance.PlayerSoundByClip(audioClip, callback, loop);
            }
           // AudioManager.Instance.PlayerSound(InitNickName(), loop, callback);

        }
        //暂停
        public void OnPause()
        {
            AudioManager.Instance.PausePlayerSound();
        }
        //停止
        public void OnResume()
        {
            AudioManager.Instance.ResnumPlayerSound();
        }
        public void OnStop()
        {
            AudioManager.Instance.StopPlayerSound();
        }
    }
}
