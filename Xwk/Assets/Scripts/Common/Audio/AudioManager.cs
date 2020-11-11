using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Internal;
using System.Linq;
using Assets.Scripts.C_Framework;
/// <summary>
/// 声音管理器 
/// author ：hzl
/// </summary>
public class AudioManager : MonoBehaviour
{
    public enum SoundFadeState
    {
        NONE,
        FADE_IN,
        FADE_OUT,
    }
    // Fields
    private AudioSource audioBg;//背景音乐

    private AudioSource audioMain;//语音

    private GameObject audioBgObject;

    private GameObject audioMainObject;

    private float audioSound_Volumn = 1f;

    private float audioBgMusicVolumn = 1f;

    public float BgMusicVolume
    {
        get { return audioBgMusicVolumn; }
        set { audioBgMusicVolumn = value; }
    }

    private float audioEffectMusicVolumn = 1f;

    private Dictionary<string, GameObject> EffectSoundCache = new Dictionary<string, GameObject>();
    private List<string> DirtyEffectSound = new List<string>();

    private string _PlayerPrefsKey = "ToggleSound";

    private SoundFadeState soundFadeState;

    private float curSoundTime = 0;

    private float audioBgFadeTime = 0;

    private float audioBgMusicFadeVolumn = 1f;

    private bool _aliveBgMusic = false;

    private bool _Alive = false;

    private bool _PauseEffect = false;
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null) Init();
            return _instance;
        }
    }
    public static void Init()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<AudioManager>();
            if(_instance == null)
                _instance = new GameObject("AudioManager").AddComponent<AudioManager>();
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(_instance.gameObject);
            }
            _instance.InitGameVoice();
            _instance._Alive = true;
        }
    }

    /// <summary>
    /// 初始化音乐和音效
    /// </summary>
    private void InitGameVoice()
    {
        if (audioBg == null)
        {
            audioBgObject = new GameObject("audioBgObject");
            audioBgObject.transform.parent = Instance.transform;
            audioBg = audioBgObject.AddComponent<AudioSource>();
            //  audioBg = (audioBg != null ? audioBg:Instance.gameObject.AddComponent<AudioSource>() );
        }
        if (audioMain == null)
        {
            audioMainObject = new GameObject("audioMainObject");
            audioMainObject.transform.parent = Instance.transform;
            audioMain = audioMainObject.AddComponent<AudioSource>();
            //  audioMain = (audioMain != null ? audioMain : Instance.gameObject.AddComponent<AudioSource>());
        }
        audioEffectMusicVolumn = audioBgMusicVolumn = audioSound_Volumn = ActiveSound;
    }

    #region 背景音乐
    public void PlayFadeInBgMusic(string musicName, bool isLocal = false, bool loop = true, float fadetime = 0f, float voulume = 1f)
    {
        _aliveBgMusic = true;
        audioBg.volume = 0f;
        audioBgFadeTime = fadetime;
        audioBgMusicVolumn = 0;
        audioBgMusicFadeVolumn = voulume;
        PlayBgMusic(musicName, isLocal, loop, SoundFadeState.FADE_IN);
    }
    public void PlayFadeOutBgMusic(string musicName, bool isLocal = false, bool loop = true, float fadetime = 0f, float voulume = 0f)
    {
        _aliveBgMusic = true;
        audioBgFadeTime = fadetime;
        audioBgMusicFadeVolumn = voulume;

        //  PlayBgMusic(musicName, isLocal, loop, SoundFadeState.FADE_OUT);
        this.soundFadeState = SoundFadeState.FADE_OUT;
    }
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="musicName"></param>
    /// <param name="isLocal"></param>
    /// <param name="loop"></param>
    public void PlayBgMusic(string musicName, bool isLocal = false, bool loop = true, SoundFadeState state = SoundFadeState.NONE)
    {
        if ((this.audioBg == null) && this.audioBg.mute)
        {
            return;
        }
        soundFadeState = SoundFadeState.NONE;
        string name = musicName;
        AudioClip clip = C_Singleton<LocalAssetMgr>.Instance.Load_Music(name, isLocal);
        if (clip == null || (this.audioBg == null))
        {
            C_DebugHelper.LogError("musicName :" + musicName + "-Sound Clip Does Not Exists !");
        }
        else
        {
            audioBg.clip = clip;
            audioBg.loop = loop;
            audioBg.Play();

            if (state != SoundFadeState.NONE)
            {
                this.soundFadeState = state;
            }
            else
            {
                audioBg.volume = audioBgMusicVolumn;
            }
        }
    }
    public void PlayBgMusic(AudioClip clip, float voulume = 1, bool loop = true, SoundFadeState state = SoundFadeState.NONE)
    {
        if ((this.audioBg == null) && this.audioBg.mute)
        {
            return;
        }
        soundFadeState = SoundFadeState.NONE;
        if (clip == null || (this.audioBg == null))
        {
            C_DebugHelper.LogError("musicName :" + clip.name + "-Sound Clip Does Not Exists !");
        }
        else
        {
            audioBg.clip = clip;
            audioBg.loop = loop;
            audioBg.Play();
            if (state != SoundFadeState.NONE)
            {
                this.soundFadeState = state;
            }
            else
            {
                audioBg.volume = audioBgMusicVolumn;
            }
            audioBg.volume = voulume;
        }

    }
    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBgMusic()
    {
        if (audioBg != null)
            audioBg.Pause();
        _aliveBgMusic = false;
    }
    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBgMusic()
    {
        if (audioBg != null)
            audioBg.Stop();
        _aliveBgMusic = false;
        ClearFade();
    }
    /// <summary>
    /// 重新开始播放背景音乐
    /// </summary>
    public void ResumeBgMusic()
    {
        if (audioBg != null)
            audioBg.UnPause();
        _aliveBgMusic = true;

    }
    /// <summary>
    /// 背景音乐是否正在被播放
    /// </summary>
    /// <returns></returns>
    public bool IsPlayBgMusic()
    {
        if (audioBg != null && audioBg.isPlaying)
            return true;
        return false;
    }
    #endregion
    #region   人声
    /// <summary>
    /// 使用名字播放人声语音
    /// </summary>
    /// <param name="name"></param>
    /// <param name="loop"></param>
    /// <param name="callback"></param>
    /// <param name="time"></param>
    /// <param name="isFade"></param>
    public void PlayerSound(string name, bool loop = false, Action callback = null, float time = -1, bool local = false)
    {
        AudioClip clip = C_Singleton<LocalAssetMgr>.Instance.Load_Music(name, local);
       

        PlayerSoundByClip(clip, callback, loop, time, local);

    }
    public void PlayerSoundByClip(AudioClip clip, Action callback, bool loop = false, float time = -1, bool local = false)
    {

        if (clip == null)
        {
             Assets.Scripts.C_Framework.C_DebugHelper.LogError("Sound：" + name + "  Clip Does Not Exists !");
            if (callback != null)
            {
                callback();
            }
            return;
        }
        else
        {
            this.audioMain.clip = clip;
            this.audioMain.loop = loop;
            this.audioMain.Play();
            if (((this.audioMain != null) && !this.audioMain.mute) && (this.audioMain.clip != clip))
            {
                this.audioMain.clip = this.audioMain.clip;
                this.audioMain.loop = loop;
            }
            StartCoroutine(playSoundOver(time >= 0 ? time : audioMain.clip.length, callback));
        }
    }
    /// <summary>
    /// 添加声音变调
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="pitch"></param>
    /// <param name="callback"></param>
    /// <param name="loop"></param>
    /// <param name="time"></param>
    /// <param name="local"></param>
    public void PlayerSoundModefyToneByClip(AudioClip clip,float pitch, Action callback, bool loop = false, float time = -1, bool local = false)
    {

        if (clip == null)
        {
            Assets.Scripts.C_Framework.C_DebugHelper.LogError("Sound：" + name + "  Clip Does Not Exists !");
            if (callback != null)
            {
                Debug.LogError("回调");
                callback();
            }
            return;
        }
        else
        {
            this.audioMain.clip = clip;
            this.audioMain.loop = loop;
            this.audioMain.pitch = pitch;
            this.audioMain.Play();
            if (((this.audioMain != null) && !this.audioMain.mute) && (this.audioMain.clip != clip))
            {
                this.audioMain.clip = this.audioMain.clip;
                this.audioMain.loop = loop;
            }
            StartCoroutine(playSoundOver(time >= 0 ? time : audioMain.clip.length, callback));
        }
    }
    /// <summary>
    /// 播放角色语音
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name)
    {
        PlayerSound(name, false, null);
    }
    /// <summary>
    /// 使用名字播放人声语音
    /// </summary>
    /// <param name="name"></param>
    /// <param name="loop"></param>
    /// <param name="callback"></param>
    /// <param name="time"></param>
    /// <param name="isFade"></param>
    public void PlayerSound(string name, Action<int, float> callback, bool loop = false, int index = -1, float time = -1, bool local = false)
    {
        AudioClip clip = C_Singleton<LocalAssetMgr>.Instance.Load_Music(name, local);
        if (clip == null)
        {
            Assets.Scripts.C_Framework.C_DebugHelper.LogError("Sound：" + name + "  Clip Does Not Exists !");
            if(callback != null)
            {
                callback(index, time);
            }
        }
        else
        {
            this.audioMain.clip = clip;
            this.audioMain.loop = loop;
            this.audioMain.Play();
            if (((this.audioMain != null) && !this.audioMain.mute) && (this.audioMain.clip != clip))
            {
                this.audioMain.clip = this.audioMain.clip;
                this.audioMain.loop = loop;
            }
            StartCoroutine(playSoundOver(time >= 0 ? time : audioMain.clip.length, callback, index));
        }
    }


    /// <summary>
    /// 设置播放语音的回调事件，按照语音的播放时间节点
    /// </summary>
    /// <param name="name"></param>
    /// <param name="loop"></param>
    /// <param name="callback"></param>
    /// <param name="index"></param>
    /// <param name="time"></param>
    public void SetPlayerSoundCallBack(string name, Action<int, float> callback = null, int index = -1, float time = -1, bool local = false)
    {
        AudioClip clip = C_Singleton<LocalAssetMgr>.Instance.Load_Music(name, local);
        if (clip == null)
        {
            Assets.Scripts.C_Framework.C_DebugHelper.LogError("Sound：" + name + "  Clip Does Not Exists !");
            if (callback != null)
            {
                callback(index, time);
            }
        }
        else
        {
            StartCoroutine(playSoundOver(time >= 0 ? time : audioMain.clip.length, callback, index));
        }
    }
    /// <summary>
    /// 设置循环播放
    /// </summary>
    /// <param name="name"></param>
    /// <param name="loop"></param>
    public void SetPlayerLoop(string name, bool loop = false, bool local = false)
    {
        AudioClip clip = C_Singleton<LocalAssetMgr>.Instance.Load_Music(name, local);
        if (clip == null)
        {
            Assets.Scripts.C_Framework.C_DebugHelper.LogError("Sound：" + name + "  Clip Does Not Exists !");
        }
        else
        {
            if (this.audioMain.clip == clip)
            {
                this.audioMain.loop = loop;
            }
            else
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogError("Sound：" + name + "  Clip is not playing!");
            }
        }
    }
    /// <summary>
    /// 是否人声正在循环中
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerLoop(bool local = false)
    {
        if (this.audioMain == null)
        {
            Assets.Scripts.C_Framework.C_DebugHelper.LogError(" audioMain is null!");
            return false;
        }
        AudioClip clip = C_Singleton<LocalAssetMgr>.Instance.Load_Music(name, local);
        if (clip == null)
        {
            Assets.Scripts.C_Framework.C_DebugHelper.LogError("Sound：" + name + "  Clip Does Not Exists !");
        }
        else
        {
            return audioMain.loop;
        }
        return false;
    }
    /// <summary>
    /// 播放结束后的回调函数
    /// </summary>
    /// <param name="time"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    IEnumerator playSoundOver(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        if (callback != null)
        {
            callback();
        }
    }
    /// <summary>
    /// 播放结束后的回调函数
    /// </summary>
    /// <param name="time"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    IEnumerator playSoundOver(float time, Action<int, float> callback, int index)
    {
        yield return new WaitForSeconds(time);
        if (callback != null)
        {
            callback(index, time);
        }
    }
    /// <summary>
    /// 语音是否正在播放
    /// </summary>
    /// <returns></returns>
    public bool isPlayerSoundPlay()
    {
        if (audioMain != null)
            return audioMain.isPlaying;
        return false;
    }
    public bool isPlayingMainSound(string name)
    {
        if (audioMain!=null)
        {
            return (audioMain.isPlaying && audioMain.clip.name.Equals(name));
        }
        return false;
    }
    /// <summary>
    /// 停止播放语音
    /// </summary>
    /// <param name="_isFade"></param>
    public void StopPlayerSound(bool _isFade = false)
    {
        if (audioMain != null)
        {
            StopAllCoroutines();

            audioMain.loop = false;
            audioMain.Stop();
        }
    }
    /// <summary>
    /// 恢复播放语音
    /// </summary>
    public void ResnumPlayerSound()
    {
        if (audioMain != null)
            audioMain.UnPause();
    }
    /// <summary>
    /// 暂停 
    /// to do ：暂停了声音，未暂停协程
    /// </summary>
    public void PausePlayerSound()
    {
        if (audioMain != null)
            audioMain.Pause();
    }

    public void SetPlayerSoundPitch(float pitch)
    {
        if (audioMain != null)
            audioMain.pitch = pitch;

    }
    /// <summary>
    /// 播放声音片段
    /// </summary>
    /// <param name="_clip"></param>
    public void PlaySound(AudioClip _clip)
    {
        AudioClip clip = _clip;
        if (clip == null)
        {
            Assets.Scripts.C_Framework.C_DebugHelper.LogError("Sound Clip Does Not Exists !");
        }
        else
        {
            PlayClipAtPoint(clip, this.audioSound_Volumn);
        }
    }

    public void PlaySoundWithName(string fileName, bool local = false)
    {
        AudioClip clip = C_Singleton<LocalAssetMgr>.Instance.Load_Music(fileName, local);
        this.PlaySound(clip);
    }
    /// <summary>
    /// 某一个位置播放，并且支持停止
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="position"></param>
    public static AudioSource PlayClipAtPoint(AudioClip clip, [DefaultValue("1.0F")] float volume, bool loop = false)
    {
        return PlayClipAtPoint(clip, Vector3.zero, volume, loop);
    }
    /// <summary>
    /// 将语音挂载在物体上播放，播放结束后将对象删除，从而停止语音
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    /// <returns></returns>
    public static AudioSource PlayClipAtPoint(AudioClip clip, Vector3 position, [DefaultValue("1.0F")] float volume, bool loop)
    {
        GameObject obj2 = new GameObject("One shot audio")
        {
            transform = { position = position }
        };
        AudioSource source = (AudioSource)obj2.AddComponent(typeof(AudioSource));
        source.clip = clip;
        source.volume = volume;
        source.Play();
        if (!loop)
            UnityEngine.Object.Destroy(obj2, clip.length * ((Time.timeScale >= 0.01f) ? Time.timeScale : 0.01f));
        return source;
    }
    /// <summary>
    /// 判断AudioSource声音是否正在播放
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public bool IsClipAtPointPlaying(AudioSource source)
    {
        if (source != null) {
            return source.isPlaying;
        }
        return false;
    }
    /// <summary>
    /// 停止AudioSource声音 
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public void StopClipAtPoint(AudioSource source)
    {
        if (source != null)
        {
            source.enabled = false;
        }
    }
    #endregion
    #region   音效
    [Serializable]
    public class SoundEffect : MonoBehaviour
    {
        private AudioSource _AudioSource;
        private float _OriginalVolume;
        private float _Duration;
        private float _Time;
        private Action _Callback;
        private bool _Singleton;
        private bool _Loop;
        private bool _Forever;
        private string _EffectName; 
        /// <summary>
        /// 获取和设置声音的名字
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            //get { return _AudioSource.clip.name; }
            get { return _EffectName; }
            set
            {
                _EffectName = value;
            }
        }
        /// <summary>
        /// 获取和设置声音的播放总时间
        /// </summary>
        /// <value>The length.</value>
        public float Length
        {
            get { return _AudioSource.clip.length; }
        }

        /// <summary>
        /// 获取和设置声音的指定时间的回调函数
        /// </summary>
        /// <value>The playback position.</value>
        public float PlaybackPosition
        {
            get { return _AudioSource.time; }
        }

        /// <summary>
        /// 获取和设置声音
        /// </summary>
        /// <value>The source.</value>
        public AudioSource Source
        {
            get { return _AudioSource; }
            set { _AudioSource = value; }
        }

        /// <summary>
        ///获取和设置声音的播放音量
        /// </summary>
        /// <value>The original volume.</value>
        public float OriginalVolume
        {
            get { return _OriginalVolume; }
            set { _OriginalVolume = value; }
        }

        /// <summary>
        /// 获取和设置声音的当前播放时间
        /// </summary>
        /// <value>The duration.</value>
        public float Duration
        {
            get { return _Duration; }
            set { _Duration = value; }
        }

        /// <summary>
        /// 获取和设置声音的剩余的播放时间
        /// </summary>
        /// <value>The duration.</value>
        public float Time
        {
            get { return _Time; }
            set { _Time = value; }
        }

        /// <summary>
        /// 获取剩余的播放时间
        /// </summary>
        /// <value>The normalised time.</value>
        public float NormalisedTime
        {
            get { return Time / Duration; }
        }

        /// <summary>
        ///  播放结束后的回调函数
        /// </summary>
        /// <value>The callback.</value>
        public Action Callback
        {
            get { return _Callback; }
            set { _Callback = value; }
        }

        /// <summary>
        ///  是否只允许一个声音播放
        /// </summary>
        /// <value><c>true</c> if repeat; otherwise, <c>false</c>.</value>
        public bool Singleton
        {
            get { return _Singleton; }
            set { _Singleton = value; }
        }
        public bool Loop
        {
            get { return _Loop; }
            set { _Loop = value; }
        }
        public bool Forever
        {
            get { return _Forever; }
            set { _Forever = value; }
        }
    }

    public void PlayEffectAutoClose(string effect_name, bool loop = false, Action callback = null, bool autoStop = true, bool local = false)
    {
        _PauseEffect = false;
        AudioSource source = null;
        int count = EffectSoundCache.Count;
        if (EffectSoundCache.ContainsKey(effect_name))
        {
            GameObject item; 
            EffectSoundCache.TryGetValue(effect_name, out item);
            if (item == null)
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogError("找不到AudioSource 对象object");
                if (callback!=null)
                {
                    callback();
                }
                return;
            }
            SoundEffect SFX = item.GetComponent<SoundEffect>();
            if (SFX != null)
            {
                SFX.Loop = loop;
                SFX.Forever = autoStop;
            }
            else
            {
                if (callback != null)
                {
                    callback();
                }
            }
            AudioSource component = item.GetComponent<AudioSource>();
            if (component == null)
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogError("找不到AudioSource");
                if (callback != null)
                {
                    callback();
                }
            }
            else if (!component.isPlaying)
            {
                //  component.volume = audioSound_Volumn;
                component.Play();
                component.loop = loop;
                component.volume = audioEffectMusicVolumn;
            }
            else if (component.isPlaying && effect_name.Equals(component.clip.name))
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogWarning("已经有该音效资源在播放");
               
                return;
            }
        }
        else
        {
          AudioClip audioClip =   C_Singleton<LocalAssetMgr>.Instance.Load_Music(effect_name, local, autoStop);
            if (audioClip == null)
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogError("effect_name is not find res:" + effect_name);
                if (callback != null)
                {
                    callback();
                }
                return;
            }
            GameObject item = new GameObject
            {
                name = effect_name
            };
            source = item.AddComponent<AudioSource>();
            source.clip = audioClip;
            source.volume = audioSound_Volumn;
            source.Play();
            source.loop = loop;
            SoundEffect SFX = item.AddComponent<SoundEffect>();
            SFX.Time = SFX.Duration = source.clip.length;
            SFX.Source = source;
            SFX.Callback = callback;
            SFX.Loop = loop;
            SFX.Source.volume = audioEffectMusicVolumn;
            SFX.Forever = autoStop;
            SFX.Name = effect_name;
            EffectSoundCache.Add(SFX.Name, item);
        }
    }


    /// <summary>
    /// 同时播放多个音效,并且播放结束会自动删除
    /// </summary>
    /// <param name="key"></param>
    /// <param name="effect_name"></param>
    /// <param name="loop"></param>
    public void PlayEffectSound(string effect_name, bool loop = false, Action callback = null, bool local = false)
    {
        PlayEffectAutoClose(effect_name, loop, callback, true, local);
    }


    /// <summary>
    /// 播放clip
    /// </summary>
    /// <param name="effect_clip"></param>
    /// <param name="loop"></param>
    /// <param name="callback"></param>
    /// <param name="autoStop"></param>
    /// <param name="local"></param>
    public void PlayEffectClipSound(AudioClip effect_clip, bool loop = false, Action callback = null,bool autoStop=true, bool local = false)
    {
        if (effect_clip==null)
        {
            Assets.Scripts.C_Framework.C_DebugHelper.LogError("effect_clip == null");
            return;
        }
        string effect_name = effect_clip.name;
        _PauseEffect = false;
        AudioSource source = null;
        int count = EffectSoundCache.Count;
        if (EffectSoundCache.ContainsKey(effect_name))
        {
            GameObject item;
            EffectSoundCache.TryGetValue(effect_name, out item);
            if (item == null)
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogError("找不到AudioSource 对象object");
                if (callback != null)
                {
                    callback();
                }
                return;
            }
            SoundEffect SFX = item.GetComponent<SoundEffect>();
            if (SFX != null)
            {
                SFX.Loop = loop;
                SFX.Forever = autoStop;
            }
            else
            {
                if (callback != null)
                {
                    callback();
                }
            }
            AudioSource component = item.GetComponent<AudioSource>();
            if (component == null)
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogError("找不到AudioSource");
                if (callback != null)
                {
                    callback();
                }
            }
            else if (!component.isPlaying)
            {
                //  component.volume = audioSound_Volumn;
                component.Play();
                component.loop = loop;
                component.volume = audioEffectMusicVolumn;
            }
            else if (component.isPlaying && effect_name.Equals(component.clip.name))
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogWarning("已经有该音效资源在播放");

                return;
            }
        }
        else
        {
            
            GameObject item = new GameObject
            {
                name = effect_name
            };
            source = item.AddComponent<AudioSource>();
            source.clip = effect_clip;
            source.volume = audioSound_Volumn;
            source.Play();
            source.loop = loop;
            SoundEffect SFX = item.AddComponent<SoundEffect>();
            SFX.Time = SFX.Duration = source.clip.length;
            SFX.Source = source;
            SFX.Callback = callback;
            SFX.Loop = loop;
            SFX.Source.volume = audioEffectMusicVolumn;
            SFX.Forever = autoStop;
            SFX.Name = effect_name;
            EffectSoundCache.Add(SFX.Name, item);
        }
    }

    /// <summary>
    /// 同事
    /// </summary>
    /// <param name="effect_name"></param>
    /// <param name="loop"></param>
    /// <param name="callback"></param>
    /// <param name="local"></param>
    public void PlayEffectManualClose(string effect_name, bool loop = false, Action callback = null, bool local = false)
    {
        PlayEffectAutoClose(effect_name, loop, callback, false, local);
    }
    //删除不能重复播放，使用PlayEffectClipSound替换
    //public void PlayEffectManualClose(AudioClip audioClip, bool loop = false, Action callback = null, bool autoStop = false)
    //{
    //    GameObject item = new GameObject
    //    {
    //        name = audioClip.name
    //    };
    //    AudioSource source = null;
    //    source = item.AddComponent<AudioSource>();
    //    source.clip = audioClip;
    //    source.volume = audioSound_Volumn;
    //    source.Play();
    //    source.loop = loop;

    //    SoundEffect SFX = item.AddComponent<SoundEffect>();
    //    SFX.Time = SFX.Duration = source.clip.length;
    //    SFX.Source = source;
    //    SFX.Callback = callback;
    //    SFX.Loop = loop;
    //    SFX.Source.volume = audioEffectMusicVolumn;
    //    SFX.Forever = autoStop;
    //    SFX.Name = audioClip.name;
    //    EffectSoundCache.Add(audioClip.name, item);
    //}

    /// <summary>
    /// 控制音效的队列，如果播放结束则删除
    /// </summary>
    void ManageSoundEffects()
    {
        if (_PauseEffect || EffectSoundCache == null)
        {
            return;
        }
        for (int count = 0; count < EffectSoundCache.Count; count++)
        {
            var element = EffectSoundCache.ElementAt(count);
            string Key = element.Key;
            GameObject item = element.Value;
            if (item!=null)
            {
                SoundEffect SFX = item.GetComponent<SoundEffect>();
                if (SFX.Source != null/*&& SFX.Time > 0f*/)
                {
                    if (SFX.Source.isPlaying)
                    {
                        SFX.Time -= Time.deltaTime;
                      //  SFX.Source.volume = audioEffectMusicVolumn;
                    }
                    else if ((SFX.Time <= 0f || !SFX.Source.isPlaying) && !SFX.Loop)
                    {
                        SFX.Source.Stop();
                        if (!SFX.Forever)
                        {
                            //卸载资源
                            C_Singleton<LocalAssetMgr>.GetInstance().UnLoad_Music(SFX.Name);
                        }
                        if (SFX.Callback != null)
                        {
                            SFX.Callback.Invoke();
                            SFX.Callback = null;
                        }
                        //   EffectSoundCache.Remove(key);
                        //  Destroy(SFX.gameObject);

                        DirtyEffectSound.Add(Key);
                    }
                }
                else
                {
                    Assets.Scripts.C_Framework.C_DebugHelper.LogError("item :"+ item.name+ "没有SoundEffect");
                }

            }
            else
            {
                //Assets.Scripts.C_Framework.C_DebugHelper.LogError("Key :" + Key + "没有item");
            }

        }
         
        if (DirtyEffectSound.Count > 0)
        {
            for (int i=0;i < DirtyEffectSound.Count;i++)
            {
                if (EffectSoundCache.ContainsKey(DirtyEffectSound[i]))
                {
                    if (EffectSoundCache[DirtyEffectSound[i]] != null)
                    {
                        Destroy(EffectSoundCache[DirtyEffectSound[i]]);
                    }
                    EffectSoundCache.Remove(DirtyEffectSound[i]);
                }
            }
            DirtyEffectSound.Clear();
        }
        

    }
    /// <summary>
    /// 每一帧刷新音效的播放时间
    /// </summary>
    void Update()
    {
        ManageSoundEffects();
        switch (this.soundFadeState)
        {
            case SoundFadeState.NONE:
                break;
            case SoundFadeState.FADE_IN:
                this.UpdateMusicFadeIn();
                break;

            case SoundFadeState.FADE_OUT:
                this.UpdateMusicFadeOut();
                break;
        }
    }
    void UpdateMusicFadeIn()
    {
        if (this.audioBg != null)
        {
            audioBgMusicVolumn = this.audioBg.volume = audioBgMusicFadeVolumn * (curSoundTime / audioBgFadeTime);
        }
        //按照一定的时间减少
        if (this.curSoundTime >= audioBgFadeTime)
        {
            curSoundTime = audioBgFadeTime;
            this.soundFadeState = SoundFadeState.NONE;
            this.ClearFade();
        }
        else
        {
            this.curSoundTime += Time.deltaTime;
        }
    }
    void UpdateMusicFadeOut()
    {
        if (this.audioBg != null)
        {
            this.audioBg.volume = this.audioBgMusicVolumn * (1 - curSoundTime / audioBgFadeTime);
        }
        if (this.curSoundTime >= audioBgFadeTime)
        {
            this.soundFadeState = SoundFadeState.NONE;
            this.ClearFade();
        }
        else
        {
            this.curSoundTime += Time.deltaTime;
        }
    }

    void ClearFade()
    {

        curSoundTime = 0;
        soundFadeState = SoundFadeState.NONE;
        audioBgFadeTime = 0f;
    }
    public void SetEffectVolumeViaName(string effectName,float volume=1.0f)
    {
        if (EffectSoundCache == null)
        {
            return;
        }
        GameObject effect;
        EffectSoundCache.TryGetValue(effectName,out effect);
        if (effect != null)
        {

            AudioSource audio = effect.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.volume = volume;
            }
            else
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogWarning(effectName+"没有AudioSource");
            }
        }
        else
        {
            Assets.Scripts.C_Framework.C_DebugHelper.LogWarning("没有这个音效");
        }
    }
    /// <summary>
    /// 停止所有音效
    /// </summary>
    public void StopAllEffect()
    {

        if (EffectSoundCache == null)
        {
            return;
        }
        for (int count = 0; count < EffectSoundCache.Count; count++)
        {
            var element = EffectSoundCache.ElementAt(count);
            string Key = element.Key;
            GameObject SoundGo = element.Value;
            if (SoundGo == null)
            {
                if (EffectSoundCache.ContainsKey(Key))
                    EffectSoundCache.Remove(Key);
                continue;
            }
            StopEffect(SoundGo);

        }
        
    }
   
    private void StopEffect(GameObject SoundGo, bool releaseRes = false)
    {
        
        SoundEffect SFX = SoundGo.GetComponent<SoundEffect>();
        SFX.Source.Stop();
        SFX.Loop = false;
        if (!SFX.Forever || releaseRes)
        {
            //卸载资源
            C_Singleton<LocalAssetMgr>.GetInstance().UnLoad_Music(SFX.Name);
        }
        //EffectSoundCache.Remove(SFX.Name);
        //Destroy(SoundGo);

        DirtyEffectSound.Add(SFX.Name);

    }
    public void StopEffectAndReleaseRes(string name)
    {
        if (EffectSoundCache != null)
        {
            for (int count = 0; count < EffectSoundCache.Count; count++)
            {
                var element = EffectSoundCache.ElementAt(count);
                string Key = element.Key;
                if (Key.Equals(name))
                {
                    GameObject SoundGo = element.Value;
                    if (SoundGo == null)
                    {
                        if (EffectSoundCache.ContainsKey(Key))
                            EffectSoundCache.Remove(Key);
                        continue;
                    }
                    StopEffect(SoundGo, true);
                }
            }
        }
    }
    /// <summary>
    /// 停止对应名字的音效
    /// </summary>
    /// <param name="name"></param>
    public void StopEffectByKey(string name)
    {
        if (EffectSoundCache != null)
        {
            for (int count = 0; count < EffectSoundCache.Count; count++)
            {
                var element = EffectSoundCache.ElementAt(count);
                string Key = element.Key;
                if (Key.Equals(name))
                {
                    GameObject SoundGo = element.Value;
                    if (SoundGo == null)
                    {
                        if (EffectSoundCache.ContainsKey(Key))
                            EffectSoundCache.Remove(Key);
                        continue;
                    }
                    StopEffect(SoundGo);
                }
            }
        }
    }
    /// <summary>
    /// 暂停某个名字的音效
    /// </summary>
    /// <param name="name"></param>
    public void PauseEffectByKey(string name)
    {
        if (EffectSoundCache != null)
        {
            _PauseEffect = true;
             
            GameObject SoundGo;
            EffectSoundCache.TryGetValue(name, out SoundGo);
            if (SoundGo == null)
            {
                if (EffectSoundCache.ContainsKey(name))
                    EffectSoundCache.Remove(name);
                Assets.Scripts.C_Framework.C_DebugHelper.LogError("停止音效失败：" + name);
            }
            else
            {
                AudioSource audio = SoundGo.GetComponent<AudioSource>();
                if (audio != null)
                {
                    audio.Pause();
                }
                else
                {
                    Assets.Scripts.C_Framework.C_DebugHelper.LogError(name + "找不到AudioSource");
                }
            }
        }

    }
    /// <summary>
    /// 恢复某个名字的音效
    /// </summary>
    /// <param name="name"></param>
    public void ResumeEffectByKey(string name)
    {
        _PauseEffect = false;
        if (EffectSoundCache != null)
        {
            GameObject SoundGo;
            EffectSoundCache.TryGetValue(name, out SoundGo);
            if (SoundGo == null)
            {
                if(EffectSoundCache.ContainsKey(name))
                    EffectSoundCache.Remove(name);

                Assets.Scripts.C_Framework.C_DebugHelper.LogError("停止音效失败：" + name);
            }
            else
            {
                AudioSource audio = SoundGo.GetComponent<AudioSource>();
                if (audio != null)
                {
                    audio.UnPause();
                }
                else
                {
                    Assets.Scripts.C_Framework.C_DebugHelper.LogError(name + "找不到AudioSource");
                }
            }
        }

    }
    /// <summary>
    /// 暂停所有的音效
    /// </summary>
    public void PauseAllEffectByKey()
    {
        _PauseEffect = true;
        if (EffectSoundCache != null)
        {
            for (int count = 0; count < EffectSoundCache.Count; count++)
            {
                var element = EffectSoundCache.ElementAt(count);
                string Key = element.Key;
                GameObject SoundGo = element.Value;
                if (SoundGo != null)
                {
                    AudioSource audio = this.EffectSoundCache[Key].GetComponent<AudioSource>();
                    if (audio != null)
                    {
                        audio.Pause();
                    }
                }
            }
        }
    }
    /// <summary>
    /// 恢复所有的音效
    /// </summary>
    public void ResumeAllEffect()
    {
        _PauseEffect = false;
        if (EffectSoundCache != null)
        {
            for (int count = 0; count < EffectSoundCache.Count; count++)
            {
                var element = EffectSoundCache.ElementAt(count);
                string Key = element.Key;
                GameObject SoundGo = element.Value;
                if (SoundGo != null)
                {
                    AudioSource audio = this.EffectSoundCache[Key].GetComponent<AudioSource>();
                    if (audio != null)
                    {
                        audio.UnPause();
                    }
                }
            }
        }

    }
    #endregion

    // Properties
    /// <summary>
    /// 设置音量
    /// </summary>
    public float ActiveSound
    {
        get
        {
            if (!LocalSave.HasKey(_PlayerPrefsKey))
            {
                this.ActiveSound = 1;
            }
            return LocalSave.GetFloat(_PlayerPrefsKey);
        }
        set
        {
            LocalSave.SetFloat(_PlayerPrefsKey, value);
            AudioListener.volume = (value == 0) ? 0f : 1f;
            audioSound_Volumn = value;
            audioBgMusicVolumn = value;
            bool flag = value == 0;
            //设置人声音量
            this.audioMain.mute = flag;
            //设置背景音乐音量
            this.audioBg.mute = flag;
        }
    }
    public void SetBackgroundMusicVolumn(float volume)
    {
        audioBgMusicVolumn = volume;
        audioBg.volume = audioBgMusicVolumn;
    }
    public void SetPlayerSoundVolume(float volume)
    {
        audioSound_Volumn = volume;
        audioMain.volume = volume;
    }
    public float GetPlayerSoundVolume()
    {
        float volume = 1.0f;
        if (audioMain!=null)
        {
            volume = audioMain.volume;
        }
        return volume;
    }
    public void SetAllEffectVolumn(float volume)
    {
        audioEffectMusicVolumn = volume;
        if (EffectSoundCache != null)
        {
            for (int count = 0; count < EffectSoundCache.Count; count++)
            {
                var element = EffectSoundCache.ElementAt(count);
                string Key = element.Key;
                GameObject SoundGo = element.Value;
                if (SoundGo != null)
                {
                    AudioSource audio = this.EffectSoundCache[Key].GetComponent<AudioSource>();
                    audio.volume = volume;
                }
            }
        }
    }

    public void StopAllSounds()
    {
        StopAllEffect();
        StopBgMusic();
        StopPlayerSound();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
        SaveAllPreferences();
        GameObject audio = AudioManager.Instance.gameObject;
        GameObject.DestroyObject(audio);
        audio = null;
    }
    public void SaveAllPreferences()
    {
        PlayerPrefs.SetFloat(_PlayerPrefsKey, audioSound_Volumn);
        PlayerPrefs.Save();
    }
    void OnApplicationExit()
    {
        _Alive = false;
    }
    public delegate void AudioCallBack(string soundname);
    public static AudioCallBack  OnMusicVolumeChange;
    public static AudioCallBack  OnSoundVolumeChange;

}//end of class
 
 
