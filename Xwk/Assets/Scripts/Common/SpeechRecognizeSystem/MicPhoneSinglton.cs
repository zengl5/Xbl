using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicPhoneSinglton : C_MonoSingleton<MicPhoneSinglton> {
    
    AudioSource _audio;
    AudioSource audio
    {
        get
        {
            if (_audio == null)
            {
                _audio = gameObject.AddComponent<AudioSource>();
            }
            return _audio;
        }
    }
    public string _CurrentAudioFilePath;
   protected override void Init() {
        string[] ms = Microphone.devices;
        deviceCount = ms.Length;
        if (deviceCount == 0)
        {
            C_DebugHelper.Log("no microphone found");
        }
    }
     
    int deviceCount;
    string sFrequency = "16000";
    public void StartRecord()
    {
        C_DebugHelper.LogErrorFormat("MicphoneSinglton StartRecord ...");


#if UNITY_EDITOR

#else
        audio.Stop();
        audio.loop = false;
        audio.mute = true;
        Microphone.End(null);
        audio.clip = Microphone.Start(null, false, 5, int.Parse(sFrequency));
        C_DebugHelper.LogErrorFormat("MicphoneSinglton StartRecord ... line46 ");
#endif

    }
    public string StopRecord()
    {
        /* if (!Microphone.IsRecording(null))
         {
             return null;
         }*/
        C_DebugHelper.LogErrorFormat("MicphoneSinglton StopRecord line56 ...");

        Microphone.End(null);
        audio.Stop();
        //保存语音文件
       // WavUtility.FromAudioClip(audio.clip, out _CurrentAudioFilePath);
        return _CurrentAudioFilePath;
    }
    public bool IsRecording()
    {
        return Microphone.IsRecording(null);
    }
    public byte[] GetClipData()
    {
        if (audio.clip == null)
        {
            C_DebugHelper.Log("GetClipData audio.clip is null");
            return null;
        }

        float[] samples = new float[audio.clip.samples];

        audio.clip.GetData(samples, 0);


        byte[] outData = new byte[samples.Length * 2];

        int rescaleFactor = 32767;

        for (int i = 0; i < samples.Length; i++)
        {
            short temshort = (short)(samples[i] * rescaleFactor);

            byte[] temdata = System.BitConverter.GetBytes(temshort);

            outData[i * 2] = temdata[0];
            outData[i * 2 + 1] = temdata[1];
        }
        if (outData == null || outData.Length <= 0)
        {
            C_DebugHelper.Log("GetClipData intData is null");
            return null;
        }
        return outData;
    }
    #region  
    private int _sampleWindow = 128;
    private static float _Duration = 5f;
    private static float _PassLevel = 20f;
    private float _PassRate = 0.05f;   
    private List<float> _wavePeakList = new List<float>();
    /// <summary>
    /// 获取录音过程中的实时峰值音量
    /// </summary>
    /// <returns> 范围0~99的float </returns>
    private float GetLevelMax()
    {
        if(!IsRecording())
        {
            return 0;
        }
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1); // null means the first microphone
        if (micPosition < 0)
        {
            return 0;
        }
        audio.clip.GetData(waveData, micPosition);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = Mathf.Abs(waveData[i]);
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        _wavePeakList.Add(levelMax * 99);
        return levelMax * 99;
    }/// <summary>
     /// 录音片段中捕获的音量评定
     /// </summary>
     /// <param name="rate">峰值合格的时间占总片段时长的比率</param>
     /// <returns> 比率合格时返回true</returns>
    private bool ScoringVolume(out float rate)
    {
        List<float> cacheList = new List<float>();
        cacheList = _wavePeakList.FindAll(p => p >= _PassLevel);
        rate = (float)cacheList.Count / _wavePeakList.Count;
    
        if(rate>_PassRate)
        {
            return true;
        }
        return false;
    }

    public bool IsTalked()
    {
        try
        {
            if (audio.clip == null)
            {
                return false;
            }
            float[] tempSamples; //临时数据存储
            tempSamples = new float[audio.clip.samples * audio.clip.channels];
            audio.clip.GetData(tempSamples, 0);
            float sourceData = 0.0f;
            sourceData = Array.Find(tempSamples, e => Mathf.Abs(e) > 0.05f);
            if (Mathf.Abs(sourceData) > 0.05f)
            {
                return true;
            }
            return false;
        }
        catch(Exception e)
        {
            C_DebugHelper.Log("IsTalked line173:"+e);

            return false;
        }
        finally
        {
           
        }

    }
    /// <summary>
    /// 判断是否有声音接口
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="playOver"></param>
    public void StartVolumeRecognition(System.Action<bool> callback, System.Action playOver = null, bool reward = false)
    {
        if (reward)
        {
        }
        SpeechRecognizeSystemEvent.Instance.AddGetRecordStatusEvent((b) => {
            if (reward)
            {
            }

            if (b)
            {
                SpeechRecognizeSystemEvent.Instance.RemoveGetRecordStatusEvent();
                SpeechRecognizeSystemEvent.Instance.DelayStop = null;
                StartRecord();
                StartCoroutine(GetVoumleResult(callback));
            }
        });

    }
    IEnumerator GetVoumleResult(System.Action<bool> callback)
    {
        yield return new WaitForSeconds(_Duration);
        StopRecord();
        //播放声音结束
        bool result = IsTalked();
        if (callback != null)
        {
            callback(result);
        }
    }
    public void StopVolumeRecongition()
    {
        Microphone.End(null);
        SpeechRecognizeSystemEvent.Instance.RemoveGetRecordStatusEvent();
        SpeechRecognizeSystemEvent.Instance.DelayStop = null;
        StopAllCoroutines();
    }

    /// <summary>
    /// 声音变调入口 
    /// </summary>
    /// <param name="callback">最终回调</param>
    /// <param name="processCallback">过程回调</param>
    public void StartRunTimeToModefiedTone(float pitch,System.Action<bool, float> callback, System.Action playOver =null)
    {
#if UNITY_EDITOR
        _Duration = 2f;
#endif
        //SpeechRecognizeSystemEvent.Instance.StartDelayStop(_Duration, () => {
        //    callback(false, 0.0f);
        //    if (playOver != null)
        //    {
        //        playOver();
        //    }
        //});

        //允许静音下播放声音
        SpeechRecognizeSystemEvent.Instance.AddGetRecordStatusEvent((b)=> {
            if (b)
            {
                SpeechRecognizeSystemEvent.Instance.RemoveGetRecordStatusEvent();
                SpeechRecognizeSystemEvent.Instance.DelayStop = null;
                StartRecord();
                StartCoroutine(EndRecordWithoutSaveData(pitch,callback, playOver));
            }
            else
            {
                callback(false, 0.0f);
                if (playOver != null)
                {
                    playOver();
                }
            }
        });

    }
    //private IEnumerator GetRecordVolume(float pitch,System.Action<bool,float> callback, System.Action playOver =null)
    //{
    //    StartCoroutine(EndRecordWithoutSaveData(pitch, callback, playOver));
    //    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    //    while (IsRecording())
    //    {
    //        yield return endOfFrame;
    //    }
    //}
    private IEnumerator EndRecordWithoutSaveData(float pitch, System.Action<bool, float> callback,Action playOver)
    {
        yield return new WaitForSeconds(_Duration);

        try
        {
            StopRecord();

            //播放声音结束
            bool result = IsTalked();
            C_DebugHelper.LogErrorFormat("MicphoneSinglton EndRecordWithoutSaveData line287 ...result："+ result);

            if (callback != null)
            {
                callback(result, 1.0f);
            }
#if UNITY_EDITOR
            result = false;
#endif
            SpeechRecognizeSystemEvent.Instance.RemoveGetRecordStatusEvent();
            SpeechRecognizeSystemEvent.Instance.DelayStop = null;
            if (!result)
            {
#if UNITY_EDITOR
                //AudioManager.Instance.PlayerSound("public/sound/common_5.ogg",false,()=> {
                //    AudioManager.Instance.SetPlayerSoundPitch(1.0f);
                //    StopAllCoroutines();
                //    if (playOver != null)
                //    {
                //        playOver();
                //    }
                //});
                SpeechRecognizeSystemEvent.Instance.StartDelayStop(_Duration, () =>
                {
                    AudioManager.Instance.SetPlayerSoundPitch(1.0f);
                    StopAllCoroutines();
                    if (playOver != null)
                    {
                        playOver();
                    }
                });
#else
            AudioManager.Instance.SetPlayerSoundPitch(1.0f);
                StopAllCoroutines();
                if (playOver != null)
                {
                    playOver();
                }
#endif
            }
            else
            {

#if UNITY_EDITOR
                AudioManager.Instance.PlayerSound("public/sound/common_5.ogg", false, () => {
                    AudioManager.Instance.SetPlayerSoundPitch(1.0f);
                    StopAllCoroutines();
                    if (playOver != null)
                    {
                        playOver();
                    }
                });
                //SpeechRecognizeSystemEvent.Instance.StartDelayStop(_Duration, () => {
                //    AudioManager.Instance.SetPlayerSoundPitch(1.0f);
                //    StopAllCoroutines();
                //    if (playOver != null)
                //    {
                //        playOver();
                //    }
                //});
#else
         AudioManager.Instance.PlayerSoundModefyToneByClip(audio.clip, pitch,()=> {
            C_DebugHelper.Log("EndRecordWithoutSaveData line 272: PlayerSoundModefyToneByClip over");
            AudioManager.Instance.SetPlayerSoundPitch(1.0f);
            StopAllCoroutines();
            if (playOver!=null)
            {
                playOver();
            }
        });
#endif
            }
        }
        catch(Exception e)
        {
            C_DebugHelper.Log("EndRecordWithoutSaveData line323:" + e);
            AudioManager.Instance.SetPlayerSoundPitch(1.0f);
            StopAllCoroutines();
            if (playOver != null)
            {
                playOver();
            }
        }
        finally
        {
           
        }
        yield return null;
    }
   
    public void InterruptRecord()
    {
        Microphone.End(null);
        SpeechRecognizeSystemEvent.Instance.RemoveGetRecordStatusEvent();
        SpeechRecognizeSystemEvent.Instance.DelayStop = null;
        AudioManager.Instance.SetPlayerSoundPitch(1.0f);
        AudioManager.Instance.StopPlayerSound();
        StopAllCoroutines();

    }
#endregion
}
