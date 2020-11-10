//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using Assets.Scripts.C_Framework;

//namespace PinYinREAD
//{
//    public class MicrophoneRecordMgr : C_MonoSingleton<MicrophoneRecordMgr>
//    {
//        AudioClip _Clip;
//        int sFrequency = 12000;
//        int _MaxRecordDur = 5;

//        float _LengthSec = 5;
//        string _AudioClipSavePath = "";
//        Action<bool> _EndRecord = null;
//        Action<float> _OnRecord = null;

//        private C_Event _EventRecordStatus = new C_Event();

//        private Coroutine _WaitPermissionCor = null;

//        private object _PermissionFlag = null;

//        private float _StartRecordWaitTime = 2.0f;

//        private bool _OnPrepareStart = false;
         
//        protected override void Init()
//        {
//            base.Init();
//            //检查mic
//            string[] ms = Microphone.devices;
//            if (ms.Length == 0)
//            {
//                C_DebugHelper.Log("no microphone found");
//            }

//            _EventRecordStatus.RegisterEvent(C_EnumEventChannel.Global, "ResponseRecordStatus", (result) =>
//            {
//                //_HasRecordPermission = "1".Equals(result[0].ToString());
//                _PermissionFlag = result[0];
//            });
//        }

//        protected override void OnDestroy()
//        {
//            base.OnDestroy();
//            ResetData();
//            _OnPrepareStart = false;

//            if (_EventRecordStatus != null)
//                _EventRecordStatus.UnregisterEvent();
//        }

//        public void GoExit()
//        {
//            ResetData();
//            StopAllCoroutines();
//            _OnPrepareStart = false;
//        }

//        private void ResetData()
//        {
//            _Clip = null;
//            Microphone.End(null);
//            _EndRecord = null;
//            _OnRecord = null;
//            _AudioClipSavePath = "";
//            _PermissionFlag = null;
//        }

//        private void FailRecordBack(string message = "")
//        {
//            if (_EndRecord != null)
//            {
//                _EndRecord(false);
//                _EndRecord = null;
//                //C_EventHandler.SendEvent(C_EnumEventChannel.Global, ReadModuleConstData.EventRecordFail, ReadModuleConstData.NoPermissionTipWord);
//            }
//            C_DebugHelper.LogWarning(message);
//            _OnPrepareStart = false;
//        }

//        private IEnumerator WaitForPermissionStart(string audioSavePath)
//        {
//            float time = 0;
//            while (true)
//            {
//                time += Time.deltaTime;
//                if(time>= _StartRecordWaitTime)
//                {
//                    C_DebugHelper.Log("Get Permission overTime");
//                    break;
//                }
//                if(_PermissionFlag != null)
//                {
//                    if(!_PermissionFlag.ToString().Equals("1"))
//                    {
//                        for (int i = 0; i < 2; i++)
//                        {
//                            yield return null;
//                        }
//                    }
//                    C_DebugHelper.Log("Get Permission success! Wait time =" + time);
//                    break;
//                }
//                yield return null;
//            }

//            GetPermissionStartRecord(audioSavePath);
//        }

//        private void GetPermissionStartRecord(string audioSavePath)
//        {
//            if (_PermissionFlag == null ? true : !_PermissionFlag.ToString().Equals("1"))
//            {
//                FailRecordBack("There is no permission to record");
//                return;
//            }
//            try
//            {
//#if UNITY_IOS || UNITY_EDITOR
//                _Clip = Microphone.Start(null, false, (int)_LengthSec + 1, sFrequency);
//#elif UNITY_ANDROID
//                C_MonoSingleton<GameHelper>.GetInstance().Android_MicrophoneEndRecord(audioSavePath);
//#endif
//            }
//            catch (Exception e)
//            {
//                FailRecordBack(e.Message);
//                return;
//            }
//            StartCoroutine("GetRecordVolume", _OnRecord);
//        }

//        public void StartRecord(Action<bool> endRecord,string audioSavePath, int lengthSec = 5, Action<float> volumeCallback = null)
//        {
//            if (IsRecording()) return;
//            ResetData();
//            StopCoroutine("GetRecordVolume");
//            _AudioClipSavePath = audioSavePath;
//            _EndRecord = endRecord;
//            _OnRecord = volumeCallback;
//            _LengthSec = Mathf.Clamp(lengthSec, 0, _MaxRecordDur);
//            if (_WaitPermissionCor != null)
//                StopCoroutine(_WaitPermissionCor);

//            //获取录音权限
//            C_MonoSingleton<GameHelper>.GetInstance().SendOpenRecordPermission();
//            _WaitPermissionCor = StartCoroutine(WaitForPermissionStart(audioSavePath));
//            _OnPrepareStart = true;
//#if UNITY_EDITOR
//            _PermissionFlag = "1";
//#endif
//        }

//        public AudioClip StopRecord()
//        {
//            if (!IsRecording())
//            {
//                return null;
//            }
//            Microphone.End(null);
//            if (_Clip == null)
//            {
//                C_DebugHelper.LogError("record fail with null clip");
//            }
//            return _Clip;
//        }

//        public bool StopRecord(string filePath)
//        {
            
//#if UNITY_EDITOR || UNITY_IOS
//            //保存语音文件
//            return WavUtility.SaveAudioClip(StopRecord(), filePath);
//#elif UNITY_ANROID
//             GameHelper.Instance.Android_MicrophoneEndRecord();
//            return true;
//#endif
//        }

//        //public AudioClip GetRecord(string filePath)
//        //{
//        //    return WavUtility.ToAudioClip(filePath);
//        //}

//        public bool IsRecording()
//        {
//#if UNITY_EDITOR || UNITY_IOS
//            //保存语音文件
//            return Microphone.IsRecording(null) || _OnPrepareStart;
//#elif UNITY_ANROID
//            return GameHelper.Instance.Android_ReponseMicrophoneRecordingState() || _OnPrepareStart;
//#endif

//        }

//        private int _SampleWindow = 128;
//        /// <summary>
//        /// 获取录音过程中的实时峰值音量
//        /// </summary>
//        /// <returns> 范围0~1的数值 </returns>
//        private float GetLevelMax()
//        {
//            if (!IsRecording())
//            {
//                return 0;
//            }
//            float levelMax = 0;
//            float[] waveData = new float[_SampleWindow];
//            int micPosition = Microphone.GetPosition(null) - (_SampleWindow + 1);
//            if (micPosition < 0)
//            {
//                return 0;
//            }
//            if(_Clip != null)
//            {
//                _Clip.GetData(waveData, micPosition);
//                for (int i = 0; i < _SampleWindow; i++)
//                {
//                    float wavePeak = Mathf.Abs(waveData[i]);
//                    if (levelMax < wavePeak)
//                    {
//                        levelMax = wavePeak;
//                    }
//                }
//            }          
//            return levelMax;
//        }

//        private IEnumerator GetRecordVolume(System.Action<float> processCallback = null)
//        {
            
//            float time = 0;
//            while (time < _LengthSec)
//            {
//                if (processCallback != null)
//                {
//#if UNITY_IOS || UNITY_EDITOR
//                    processCallback(GetLevelMax());
//#elif UNITY_ANDROID
//                    processCallback(C_MonoSingleton<GameHelper>.GetInstance().Android_GetRecordVolume());
//#endif
//                }
//                time += Time.deltaTime;
//                yield return null;
//            }
//            bool isSave = StopRecord(_AudioClipSavePath);
//            if(!isSave)
//            {
//               // C_EventHandler.SendEvent(C_EnumEventChannel.Global, ReadModuleConstData.EventRecordFail,ReadModuleConstData.AudioSaveFailTip);
//            }
//            if (_EndRecord != null)
//            {
//                _EndRecord(isSave);
//                _EndRecord = null;
//            }
//            _OnPrepareStart = false;
//        }

        


//    }
//}