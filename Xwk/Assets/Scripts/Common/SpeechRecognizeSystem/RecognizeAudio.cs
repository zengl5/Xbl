using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts.C_Framework;
using DG.Tweening;
/// <summary>
///  author  van
///  time 2019-5-30
/// </summary>

public class RecognizeAudio : C_MonoSingleton<RecognizeAudio>
{
    [System.Serializable]
    class WordItem
    {
        public float beingTime { get; set; }
        public float endTime { get; set; }
        public int matchTag { get; set; }
        public float pronAccuracy { get; set; }
        public float pronFluency { get; set; }
        public string word { get; set; }
    }
    [System.Serializable]
    class WordRespone
    {
       public string inputWords { get; set; }
        public int resultCode { get; set; }
        public float pronAccuracy { get; set; }
        public int pronFluency { get; set; }
        public float pronCompletion { get; set; }
        public List<WordItem> words { get; set; }
    }
    public enum ResultType {
        PickWord,//拿到最高分数的单词
        PickScore,//获取分数
     }

    System.Action<string> _ResultCallback;

    string _AnswerWord;

    ResultType _ResultType;

    /// <summary>
    /// 评分
    /// </summary>
    /// <param name="result">返回腾讯云的识别结果</param>
     public  void GetTecentReslut_old(string result)
    {
        float score = 0;
        string resultWord = "0";
        if (!string.IsNullOrEmpty(result))
        {
            WordRespone itemDate = JsonUtility.FromJson<WordRespone>(result);//特殊处理的jason，一般jason类继续使用框架jason
            string word = itemDate.inputWords;
            C_DebugHelper.Log("class:RecognizeAudio function:GetTecentReslut  line 59..  word =" + word);
            int resultCode = itemDate.resultCode;
            if (resultCode == 1)
            {
                if (_ResultType == ResultType.PickWord)
                {
                    //解析所有的文字，找到分数最高的文字
                    float wordMaxScore = 0f;
                    List<WordItem> wordList = itemDate.words; 
                    if (wordList != null)
                    {
                        for (int i = 0; i<wordList.Count; i++)
                        {
                            if (wordList[i] != null)
                            {
                                string wordName = wordList[i].word;
                                float tmpScore = wordList[i].pronAccuracy * 0.6f + wordList[i].pronFluency*100f*0.4f;
                                if (tmpScore >= wordMaxScore ||(i == 0))
                                {
                                    wordMaxScore = tmpScore;
                                    resultWord = wordName;
                                }
                            }
                        }
                    }
                    DoScoreCallBack(resultWord);
                    return;
                }
                else if (_ResultType == ResultType.PickScore)
                {
                    //直接计算分数
                    score = (itemDate.pronAccuracy * 0.6f + itemDate.pronFluency * 100f * 0.4f) * itemDate.pronCompletion;
                }

            }
            if (_ResultCallback != null)
            {
                C_DebugHelper.Log("_ResultCallback ..GetTecentReslut  result =" + result);
                _ResultCallback(score.ToString());
                _ResultCallback = null;
            }
        }
        else
        {
            DoScoreCallBack(score.ToString());
        }
    }

     void GetTecentReslut(string result)
    {
        float score = 0;
        try
        {
            string resultWord = "0";
            if (!string.IsNullOrEmpty(result))
            {
                //  WordRespone itemDate = JsonUtility.FromJson<WordRespone>(result);//特殊处理的jason，一般jason类继续使用框架jason
                C_DebugHelper.Log("class:RecognizeAudio function:GetTecentReslut  line 59..  result =" + result);

                string word = C_Json.GetJsonKeyString(result, "inputWords");
#if !UNITY_IOS
                int pronCompletion = C_Json.GetJsonKeyInt(result, "pronCompletion");
#else
                 
#endif
                int resultCode = C_Json.GetJsonKeyInt(result, "resultCode");
                float pronAccuracy = C_Json.GetJsonKeyFloat(result, "pronAccuracy");
                float pronFluency = C_Json.GetJsonKeyFloat(result, "pronFluency");
                if (resultCode == 1)
                {
                    if (_ResultType == ResultType.PickWord)
                    {
                        //解析所有的文字，找到分数最高的文字
                        float wordMaxScore = 0f;
                        JsonData wordList = C_Json.GetJsonKeyJsonData(result, "words");
                        if (wordList != null)
                        {
                            for (int i = 0; i < wordList.Count; i++)
                            {
                                if (wordList[i] != null)
                                {
                                    string wordName = C_Json.GetJsonKeyString(wordList[i], "word");
                                    float pronAccuracy_word = C_Json.GetJsonKeyFloat(wordList[i], "pronAccuracy");
                                    float pronFluency_word = C_Json.GetJsonKeyFloat(wordList[i], "pronFluency");
#if !UNITY_IOS
                                    float tmpScore = pronAccuracy_word * 0.6f + pronFluency_word * 100f * 0.4f;
#else
                                    float tmpScore = pronAccuracy_word;
                                    //C_DebugHelper.Log("class:RecognizeAudio function:GetTecentReslut  line 153..  pronFluency_word =" + pronFluency_word);
#endif
                                    if (tmpScore >= wordMaxScore || (i == 0))
                                    {
                                        wordMaxScore = tmpScore;
                                        resultWord = wordName;
                                    }
                                }
                            }
                        }
                        DoScoreCallBack(resultWord);
                        return;
                    }
                    else if (_ResultType == ResultType.PickScore)
                    {
                        //直接计算分数

#if !UNITY_IOS
                    score = (pronAccuracy * 0.6f + pronFluency * 100f * 0.4f) * pronCompletion;
#else
                        score = (pronAccuracy * 0.6f + pronFluency * 100f * 0.4f) ;
#endif
                    }

                }
                if (_ResultCallback != null)
                {
                    C_DebugHelper.Log("_ResultCallback ..GetTecentReslut  result =" + result);
                    _ResultCallback(score.ToString());
                    _ResultCallback = null;
                }
            }
            else
            {
                C_DebugHelper.Log("class:RecognizeAudio function:GetTecentReslut  line 169..  result =" + result);

                DoScoreCallBack(score.ToString());
            }
        }
        catch(Exception e)
        {
            DoScoreCallBack(score.ToString());

        }
    }


    public void DoScoreCallBack(string vaule)
    {
        ClearEvent();
        if (_ResultCallback != null)
        {
            _ResultCallback(vaule);
            _ResultCallback = null;
        }
    }
    /// <summary>
    /// 开始录音，腾讯云
    /// </summary>
    /// <param name="answer"></param>
    /// <param name="scoreCallback"></param>
    public void StartRecognizeAudioTecent(string word, System.Action<string> scoreCallback, ResultType type = ResultType.PickScore, int langType = 1, int speechType = 1, float recordDur = 3f, bool reward = false)
    {
        _ResultCallback = scoreCallback;
        _ResultType = type;
        if (recordDur <= 0)
        {
            DoScoreCallBack("0");
            C_DebugHelper.LogError("您设置的录音时间过短，请重新确认！");
            return;
        }

#if UNITY_EDITOR
        DelayStop = DOVirtual.DelayedCall(recordDur, () =>
        {
            StopWithStartOverTimeCheck();
        });
        return;
#endif
       
       

        if (_GetRecordStatusEvent != null)
        {
            _GetRecordStatusEvent.UnregisterEvent();
        }
        _GetRecordStatusEvent = null;
        _GetRecordStatusEvent = new C_Event();
        _GetRecordStatusEvent.RegisterEvent(C_EnumEventChannel.Global,"ResponseRecordStatus",(status)=> {
            _GetRecordStatusEvent.UnregisterEvent();
            if (status!=null )
            {
                string answer = "1";
                if(answer.Equals(status[0]))
                {

                    _IsRecording = true;
                }
                else
                {
                    Tips.Create("开启语音权限后，才可以继续使用噢！");
                }
            }
#if !UNITY_IOS
    StartRecordAction(recordDur);                   
#endif
        });

        _AnswerWord = word;
#if UNITY_IOS
    StartRecordAction(recordDur);                   
#endif
      
    } 
    void StartRecordAction(float recordDur){
         //等待结果返回
            DelayStop = DOVirtual.DelayedCall(recordDur, () =>
            {
                StopWithStartOverTimeCheck();
            });
    }

    /// <summary>
    /// 停止录音，并评分
    /// </summary>
    /// <param name="answer"></param>
    /// <returns></returns>
    public  void StopRecognizeAudio()
    {
        StopWithStartOverTimeCheck();
    }

    #region ----修改加回超时判断----
    private  bool _IsRecording = false;

    private  Tween _DelayStop;

    public  Tween DelayStop
    {
        get { return _DelayStop; }
        set
        {
            if (_DelayStop != null)
                _DelayStop.Kill();

            _DelayStop = value;
        }
    }

    private  float _DelayTime = 5f;

    private  Tween _DelayCallback;

    public  Tween DelayCallback
    {
        get { return _DelayCallback; }
        set
        {
            if (_DelayCallback != null)
                _DelayCallback.Kill();

            _DelayCallback = value;
        }
    }

    private  C_Event _GetTecentWordEvent;
    private  C_Event _GetRecordStatusEvent;
  
    /// <summary>
    /// 超时 判断 默认 正确
    /// </summary>
     void StopWithStartOverTimeCheck()
    {
        //没网 弹窗tips 不走腾讯云语音识别  等待超时
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            DelayCallback = DOVirtual.DelayedCall(2f, () =>
            {
                ClearEvent();
                DoScoreCallBack("100");
                C_DebugHelper.Log("本轮语音识别无网络,默认正确！");
            });
            Tips.Create("语音识别异常，请切换网络后重试");
            return;
        }

        //等待结果返回，5秒之后如果没有返回结果，则自动返回
        DelayCallback = DOVirtual.DelayedCall(_DelayTime, () =>
        {
            ClearEvent();
            DoScoreCallBack("100");
            C_DebugHelper.Log("当轮语音识别没有拿到结果,默认正确！请确定网络状况是否良好！");
        });
        float time = Time.time;
        //float timeScale = Time.timeScale;
        //DelayCallback.OnUpdate(() =>
        //{
        //    if (Time.timeScale != timeScale)
        //    {
        //        C_DebugHelper.Log("TimeScale:" + Time.timeScale);
        //        timeScale = Time.timeScale;
        //    }
        //    C_DebugHelper.Log("花费时间：" + (Time.time - time));
        //});


        if (_GetTecentWordEvent != null)
        {
            _GetTecentWordEvent.UnregisterEvent();
        }
        _GetTecentWordEvent = null;
        _GetTecentWordEvent = new C_Event();

        _GetTecentWordEvent.RegisterEvent(C_EnumEventChannel.Global, "ResponseRecord", (word) =>
        {
            //停掉超时计算
            //if (DelayCallback != null)
            //{
            //    DelayCallback.Kill();
            //    DelayCallback = null;
            //}
            ClearEvent();
            GetTecentReslut((word == null || word.Length <= 0) ? "" : word[0].ToString());
            
        });

        _IsRecording = false;

    }
     public void ClearEvent()
    {
        DelayStop = null;

        if (DelayCallback != null)
        {
            DelayCallback.Kill();
            DelayCallback = null;
        }
        if (_GetTecentWordEvent != null)
        {
            _GetTecentWordEvent.UnregisterEvent();
        }
        _GetTecentWordEvent = null;

        if (_GetRecordStatusEvent != null)
        {
            _GetRecordStatusEvent.UnregisterEvent();
        }
        _GetRecordStatusEvent = null;
    }
    public  void Clear()
    {
        if (DelayCallback != null) { DelayCallback.Kill(); }

        DelayCallback = null;
        _ResultCallback = null;
        _AnswerWord = null;
        DelayStop = null;

        ClearEvent();
        if (_IsRecording)
        {
        }
        _IsRecording = false;
    }
    #endregion
}
