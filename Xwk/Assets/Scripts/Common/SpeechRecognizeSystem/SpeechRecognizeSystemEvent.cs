using Assets.Scripts.C_Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechRecognizeSystemEvent : C_MonoSingleton<SpeechRecognizeSystemEvent> {
    private System.Action<bool> _ActionCallback = null;
    private C_Event _GetRecordStatusEvent;
    public void AddGetRecordStatusEvent(System.Action<bool> actionCallback = null)
    {
        C_DebugHelper.LogErrorFormat("SpeechRecognizeSystemEvent AddGetRecordStatusEvent  start ");

        _ActionCallback = actionCallback;
        if (_GetRecordStatusEvent != null)
        {
            _GetRecordStatusEvent.UnregisterEvent();
        }
        _GetRecordStatusEvent = null;
        _GetRecordStatusEvent = new C_Event();
        _GetRecordStatusEvent.RegisterEvent(C_EnumEventChannel.Global, "ResponseRecordStatus", (status) => {
            _GetRecordStatusEvent.UnregisterEvent();
            if (status != null)
            {
                C_DebugHelper.LogErrorFormat("ResponseRecordStatus is :"+ status);
                string answer = "1";
                if (answer.Equals(status[0]))
                {
                    if (_ActionCallback != null)
                    {
                        _ActionCallback(true);
                    }
                }
                else
                {
                    Tips.Create("开启语音权限后，才可以继续使用噢！");
                    if (_ActionCallback != null)
                    {
                        _ActionCallback(false);
                    }
                }
            }
        });
    }

    public void RemoveGetRecordStatusEvent()
    {
        _ActionCallback = null;
        if (_GetRecordStatusEvent != null)
        {
            _GetRecordStatusEvent.UnregisterEvent();
        }
        _GetRecordStatusEvent = null;
    }

    //-------等待一段时间
    private Tween _DelayStop;

    public Tween DelayStop
    {
        get { return _DelayStop; }
        set
        {
            if (_DelayStop != null)
                _DelayStop.Kill();

            _DelayStop = value;
        }
    }
   
    public void StartDelayStop(float time,System.Action action)
    {
        //等待结果返回
        DelayStop = DOVirtual.DelayedCall(time, () =>
        {
            if (action!=null)
            {
                action();
            }
        });
    }
    public void ClearDelayStop()
    {
        DelayStop = null;
    }

}
