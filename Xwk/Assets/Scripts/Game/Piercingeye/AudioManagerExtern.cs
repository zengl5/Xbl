using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Internal;
using System.Linq;
using Assets.Scripts.C_Framework;

public class AudioManagerExtern : Singleton<AudioManagerExtern>
{
    public Dictionary<string, GameObject> EffectSoundCache = new Dictionary<string, GameObject>();
    int refcount = 0;
    public class SoundEffect : MonoBehaviour
    {
        public AudioSource Source;
        public float OriginalVolume;
        public float Duration;
        public float Time;
        public Action Callback;
        public bool Singleton;
        public bool Loop;
        public float LoopTime = 0;
        public bool Forever;
        public bool PlayFinish = false;
        public bool AutoDestory = false;
        public string Name;
        public void StopPlay()
        {
            if (Source != null)
                Source.Stop();
        }
        public void StartPlay()
        {
            StartCoroutine(NormalPlay(Source.clip.length, Callback));
        }
        public void StartFixedLoopPlay(AudioSource source,float loopTotalTime)
        {
            StartCoroutine(FixedLoop(source, loopTotalTime, Callback));
        }
        IEnumerator NormalPlay(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            PlayFinish = true;
            if (callback != null)
                callback();
            if (AutoDestory)
                Destroy(this.gameObject);
        }

        IEnumerator FixedLoop(AudioSource source, float loopTotalTime, Action callback)
        {
            float TotalTime = 0;
            while(true)
            {
                source.Play();
                yield return new WaitForSeconds(source.clip.length);
                TotalTime += source.clip.length;
                //Debug.LogError(TotalTime);
                //播放总时间>给定播放时间| 距离播放完成的时间<0.2倍
                if (TotalTime>=loopTotalTime|(loopTotalTime-TotalTime)<0.2f*source.clip.length)
                {
                    source.Stop();
                    PlayFinish = true;
                    if (callback != null)
                        callback();
                    if (AutoDestory)
                        Destroy(this.gameObject);
                    break;
                }             
            }          
        }
    }

    /// <summary>
    /// 播放一个固定时间循环的声音 循环[X]S,时间结束后自动停止
    /// 播放完整结束
    /// </summary>
    /// <param name="effect_clip"></param>
    /// <param name="callback"></param>
    /// <param name="AutoDestory"></param>
    public void PlayFixedLoopClip(AudioClip effect_clip,float loopTime,Action callback = null)
    {
        if (effect_clip == null)
        {
            Assets.Scripts.C_Framework.C_DebugHelper.LogError("effect_clip == null");
            return;
        }
        GameObject item = new GameObject(effect_clip.name);
        AudioSource source;
        source = item.AddComponent<AudioSource>();
        source.clip = effect_clip;
        source.volume = 1;
        source.Play();
        source.loop = false;
        SoundEffect SFX = item.AddComponent<SoundEffect>();
        SFX.Time = SFX.Duration = source.clip.length;
        SFX.Source = source;
        SFX.Callback = callback;
        SFX.Loop = true;
        SFX.Source.volume = 1;
        SFX.Source.loop = true;
        SFX.LoopTime = loopTime;
        SFX.Forever = false;
        SFX.Name = effect_clip.name;
        SFX.AutoDestory = true;
        SFX.StartFixedLoopPlay(source,loopTime);
        refcount = 0;
        AddRefCache(SFX.name, item);
    }
    /// <summary>
    /// 播放时间短，快速切换类型音效，不打断
    /// </summary>
    /// <param name="effect_clip"></param>
    /// <param name="callback"></param>
    public void PlaySmallClipSound(AudioClip effect_clip, Action callback = null, bool autoDestory = true)
    {
        if (effect_clip == null)
        {
            Assets.Scripts.C_Framework.C_DebugHelper.LogError("effect_clip == null");
            return;
        }
        GameObject item = new GameObject(effect_clip.name);
        AudioSource source;
        source = item.AddComponent<AudioSource>();
        source.clip = effect_clip;
        source.volume = 1;
        source.Play();
        source.loop = false;
        SoundEffect SFX = item.AddComponent<SoundEffect>();
        SFX.Time = SFX.Duration = source.clip.length;
        SFX.Source = source;
        SFX.Callback = callback;
        SFX.Loop = false;
        SFX.Source.volume = 1;
        SFX.Forever = false;
        SFX.Name = effect_clip.name;
        SFX.AutoDestory = autoDestory;
        SFX.StartPlay();
        refcount = 0;
        AddRefCache(SFX.name,item);
    }
    public void StopClipSound(string clipName)
    {
        if (EffectSoundCache.ContainsKey(clipName))
        {
            EffectSoundCache[clipName].GetComponent<SoundEffect>().StopPlay();
        }
    }
    void AddRefCache(string name,GameObject item)
    {
        if (!EffectSoundCache.ContainsKey(name))
        {
            EffectSoundCache.Add(name, item);
        }
        else
        {
            refcount++;
            AddRefCache(name + refcount.ToString(), item);
        }
    }
}
