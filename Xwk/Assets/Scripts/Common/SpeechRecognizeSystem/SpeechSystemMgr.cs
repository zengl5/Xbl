using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.C_Framework;
 

public class SpeechSystemMgr:C_MonoSingleton<SpeechSystemMgr>
{
    private  string _AudioEffectName = "public/sound/public_xwkyx_014.ogg";
    private  string _UICoverName = "PackagingResources/public/Hero_Effect/prefab/UICover_shengyin";
    private  Canvas _UICover;
    private  Action<bool> _VolumeCallback;
    private  Action<bool> _RecordOverCallback;
    private  Action<string> _RecongizeScoreOverCallback;
    private Action _PlayOverCallback;
    private float _AudioVoumle = 1.0f;
    private GameObject _EffectCamera = null;
    private Action _StopCallBack = null;
    /// <summary>
    /// 故事语音识别接口
    /// </summary>
    /// <param name="word"></param>
    /// <param name="recordOver"></param>
    public void StartRecognizeAudioTecentSlate(string word,Action<string> recordOver)
    {
        _RecongizeScoreOverCallback = recordOver;
        //CreataUICover();
        AudioManager.Instance.PlayEffectSound(_AudioEffectName);
        RecognizeAudio.ResultType type;
        if (string.IsNullOrEmpty(word))
        {
            type = RecognizeAudio.ResultType.PickScore;
        }
        else
        {
            type = RecognizeAudio.ResultType.PickWord;
        }

        RecognizeAudio.Instance.StartRecognizeAudioTecent(word, (score)=> {
            RecongizeScoreOver(score);
        }, type,1,1,3,true);
    } 
    /// </summary>
    /// <param name="word">识别的文字</param>
    /// <param name="recordOver">识别成功或者识别失败的回调</param>
    /// <param name="stopCallback">表示中途退出的回调函数</param>
    public void StartRecognizeAudioTecent(string word, Action<string> recordOver, RecognizeAudio.ResultType type = RecognizeAudio.ResultType.PickScore)
    {
        _RecongizeScoreOverCallback = recordOver;
        CreataUICover();
        AudioManager.Instance.PlayEffectSound(_AudioEffectName);
        RecognizeAudio.Instance.StartRecognizeAudioTecent(word, (score)=> {
            RecongizeScoreOver(score);
        }, type);
       
    }
    private void RecongizeScoreOver(string  score)
    { 
        if (_RecongizeScoreOverCallback!=null)
        {
            _RecongizeScoreOverCallback(score);
        }
        StopRecognize();
    }

    public void StopRecognize()
    {
        _RecongizeScoreOverCallback = null;
       
        StopAllCoroutines();

        ClearUI();
        RecognizeAudio.Instance.Clear();
        GameObject.Destroy(RecognizeAudio.Instance.gameObject);
        GameObject.Destroy(SpeechSystemMgr.Instance.gameObject);
    }
    public void StartVolumeRecognition(Action<bool> recordOver,bool showUI = false,bool reward = false)
    {
        _VolumeCallback = recordOver;
        if (showUI)
            CreataUICover();
        MicPhoneSinglton.Instance.StartVolumeRecognition(VolumeRecognitionOver,null,true);
    }
    public void VolumeRecognitionOver(bool reslut)
    {
        ClearUI();
        if (_VolumeCallback != null)
        {
            _VolumeCallback(reslut);
            _VolumeCallback = null;
        }

    }
    public void StopVolumeRecongition()
    {
        ClearUI();
        _VolumeCallback = null;
        MicPhoneSinglton.Instance.StopVolumeRecongition();
    }
    public void EnterSpeechModefiedToneSlate(float pitch,Action<bool> recordOver,Action playOver )
    {
        _PlayOverCallback = playOver;
        _RecordOverCallback = recordOver;
      //  CreataUICover();
        AudioManager.Instance.PlayEffectSound(_AudioEffectName);
        //DG.Tweening.DOVirtual.DelayedCall(0.8f, () => 
        //{
        //});
        _AudioVoumle = AudioManager.Instance.GetPlayerSoundVolume();
        AudioManager.Instance.SetPlayerSoundVolume(2.0f);
        MicPhoneSinglton.Instance.StartRunTimeToModefiedTone(pitch, DoneScore, _PlayOverCallback);
    }
   
    private  void DoneScore(bool pass,float rate)
    {
        AudioManager.Instance.SetPlayerSoundVolume(_AudioVoumle);
        AudioManager.Instance.StopEffectByKey(_AudioEffectName);
        ClearUI();
        if(_RecordOverCallback !=null)
        {
            _RecordOverCallback(pass);
            _RecordOverCallback = null;
        }
#if UNITY_IOS || UNITY_ANDROID
        C_MonoSingleton<GameHelper>.GetInstance().SetMuteModePlay();
#endif
    }


    private  IEnumerator StandaloneRuntimeCheck()
    {
        yield return new WaitForSeconds(2f);
        //DoneScore(false, 0);
        DoneScore(true, 0);
    }
    public  bool CreataUICover()
    {
        if (_UICover != null)
        {
            _UICover.gameObject.SetActive(true);
            return true;
        }
        GameObject go =GameObject.Instantiate( Resources.Load(_UICoverName) as GameObject);
        if (go == null)
        {
            C_DebugHelper.LogError("the Canvas u want to Load does'nt Exist in the path:" + _UICoverName);
            return false;
        }
        go.SetActive(true);
        go.layer= LayerMask.NameToLayer("UI");
        _UICover = go.GetComponent<Canvas>();
        _UICover.renderMode = RenderMode.ScreenSpaceCamera;
        _UICover.worldCamera = Assets.Scripts.C_Framework.C_UIMgr.c_UICameraHigh;
        //_Mic = _UICover.transform.Find(_MicName);
        //if (_Mic == null)
        //{
        //    C_DebugHelper.LogError("can't find the mic with the parent" + _UICover.name + ".check the child named by: " + _MicName);
        //    return false;
        //}
        //if(_Mic.gameObject.activeSelf == false)
        //{
        //    _Mic.gameObject.SetActive(true);
        //}
        return true;
    }
    private  void ShowMicTips()
    {
        //if (_Mic != null)
        //{
        //    _Mic.gameObject.SetActive(true);
        //}
        if (_UICover!=null)
        {
            _UICover.gameObject.SetActive(true);
        }
    }
    private  void CloseTips()
    {
        //if (_Mic != null)
        //{
        //    _Mic.gameObject.SetActive(false);
        //}
        if (_UICover != null)
        {
            _UICover.gameObject.SetActive(true);
        }
    }
    private  void ClearUI()
    {
        if(_UICover != null)
        {
            GameObject.Destroy(_UICover.gameObject);
            _UICover = null;
        }
    }

    public void Stop()
    {
        AudioManager.Instance.SetPlayerSoundVolume(_AudioVoumle);

        _PlayOverCallback = null;
        _RecordOverCallback = null;
        StopAllCoroutines();

        ClearUI();
        MicPhoneSinglton.Instance.InterruptRecord();
        GameObject.Destroy(MicPhoneSinglton.Instance.gameObject);
        GameObject.Destroy(SpeechSystemMgr.Instance.gameObject);
    }
}
