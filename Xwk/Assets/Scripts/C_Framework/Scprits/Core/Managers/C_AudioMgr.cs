using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.C_Framework
{
    public class C_AudioMgr : C_MonoSingleton<C_AudioMgr>
    {
        //音乐播放器
        private AudioSource m_MusicAudioSource;

        // 音效播放器
        public List<AudioSource> m_SoundAudioSourceList = new List<AudioSource>();

        private bool m_bMusicAudioEnabled = true;
        public bool MusicAudioEnabled
        {
            get { return m_bMusicAudioEnabled; }
            set
            {
                m_bMusicAudioEnabled = value;

                m_MusicAudioSource.enabled = value;
            }
        }

        private float m_fMusicAudioVolume = 1;
        public float MusicAudioVolume
        {
            get { return m_fMusicAudioVolume; }
            set
            {
                m_fMusicAudioVolume = value;

                SetMusicAudioVolume(m_fMusicAudioVolume);
            }
        }

        private bool m_bSoundAudioEnabled = true;
        public bool SoundAudioEnabled
        {
            get { return m_bSoundAudioEnabled; }
            set
            {
                m_bSoundAudioEnabled = value;

                foreach (AudioSource audioSource in m_SoundAudioSourceList)
                    audioSource.enabled = value;
            }
        }

        private float m_fSoundAudioVolume = 1;
        public float SoundAudioVolume
        {
            get { return m_fSoundAudioVolume; }
            set
            {
                m_fSoundAudioVolume = value;

                SetSoundAudioVolume(m_fSoundAudioVolume);
            }
        }

        // 创建音效播放器和音乐播放器
        protected override void Init()
        {
            m_MusicAudioSource = gameObject.AddComponent<AudioSource>();
            AddSoundAudioSource(gameObject.AddComponent<AudioSource>());


            if (PlayerPrefs.GetInt(C_PlayerPrefsData.GAME_MUSIC_EANBLED, 1) == 0)
                MusicAudioEnabled = false;
            else
                MusicAudioEnabled = true;

            MusicAudioVolume = PlayerPrefs.GetFloat(C_PlayerPrefsData.GAME_MUSIC_VOLUME, 1.0f);


            if (PlayerPrefs.GetInt(C_PlayerPrefsData.GAME_SOUND_EANBLED, 1) == 0)
                SoundAudioEnabled = false;
            else
                SoundAudioEnabled = true;

            SoundAudioVolume = PlayerPrefs.GetFloat(C_PlayerPrefsData.GAME_SOUND_VOLUME, 1.0f);
        }

        public void PlayBGM(AudioClip audioClip)
        {
            if (audioClip != null)
                PlayClip(m_MusicAudioSource, audioClip, true);
        }                                                                       

        public void StopBGM()
        {
            StopClip(m_MusicAudioSource);
        }

        public void PlayClipOneShot(AudioClip audioClip)
        {
            if (audioClip != null)
                m_SoundAudioSourceList[0].PlayOneShot(audioClip);
        }

        public void PlayClipOneShot_MP3(string filePath)
        {
            StartCoroutine(LoadMP3(filePath));
        }

        private IEnumerator LoadMP3(string filePath)
        {
            if (!filePath.Contains("www."))
                filePath = "file://" + filePath;

            using (var uwr = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG))
            {
                yield return uwr.SendWebRequest();

                if (!uwr.isNetworkError)
                    PlayClipOneShot(DownloadHandlerAudioClip.GetContent(uwr));
            }
        }

        public void PlayClip(AudioSource audioSource, AudioClip audioClip, bool loop = false)
        {
            if (audioSource == null || audioClip == null)
                return;

            audioSource.clip = audioClip;
            audioSource.loop = loop;

            if (!audioSource.isPlaying)
                audioSource.Play();
        }

        public void StopClip(AudioSource audioSource)
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = null;
            } 
        }

        public void SetAudioSourceVolume(AudioSource audioSource, float volume)
        {
            if (audioSource == null)
                return;

            audioSource.volume = volume;
        }

        public void SetMusicAudioVolume(float volume)
        {
            SetAudioSourceVolume(m_MusicAudioSource, volume);
        }

        public void SetSoundAudioVolume(float volume)
        {
            foreach (AudioSource audioSource in m_SoundAudioSourceList)
                SetAudioSourceVolume(audioSource, volume);
        }

        public void AddSoundAudioSource(AudioSource audioSource)
        {
            if (audioSource == null)
                return;

            for (int i = m_SoundAudioSourceList.Count - 1; i >= 0; i--)
            {
                if (m_SoundAudioSourceList[i] == audioSource)
                    return;
            }

            m_SoundAudioSourceList.Add(audioSource);
        }

        public void RemoveSoundAudioSource(AudioSource audioSource)
        {
            if (audioSource == null)
                return;

            for (int i = m_SoundAudioSourceList.Count - 1; i >= 0; i--)
            {
                if (m_SoundAudioSourceList[i] == audioSource)
                {
                    m_SoundAudioSourceList.RemoveAt(i);
                    return;
                }
            }
        }

        public void SaveData()
        {
            if (MusicAudioEnabled)
                PlayerPrefs.SetInt(C_PlayerPrefsData.GAME_MUSIC_EANBLED, 1);
            else
                PlayerPrefs.SetInt(C_PlayerPrefsData.GAME_MUSIC_EANBLED, 0);

            PlayerPrefs.SetFloat(C_PlayerPrefsData.GAME_MUSIC_VOLUME, MusicAudioVolume);


            if (SoundAudioEnabled)
                PlayerPrefs.SetInt(C_PlayerPrefsData.GAME_SOUND_EANBLED, 1);
            else
                PlayerPrefs.SetInt(C_PlayerPrefsData.GAME_SOUND_EANBLED, 0);

            PlayerPrefs.SetFloat(C_PlayerPrefsData.GAME_SOUND_VOLUME, SoundAudioVolume);

            PlayerPrefs.Save();
        }
    }
}