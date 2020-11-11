using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_Event
    {
        public C_EnumEventChannel m_EnumEventChannel = C_EnumEventChannel.Empty;

        public string m_strEventName = string.Empty;

        private Action<object[]> m_Callback = null;

        public int m_nPriority = 0;

        public void RegisterEvent(C_EnumEventChannel eventChannel, string eventName, Action<object[]> callback, int priority = 0)
        {
            if (eventChannel == C_EnumEventChannel.Empty)
            {
                Debug.LogError("C_Event RegisterEvent: Event channel is Empty!");
                return;
            }

            if (string.IsNullOrEmpty(eventName))
            {
                Debug.LogError("C_Event RegisterEvent: Event name is Null or Empty!");
                return;
            }

            UnregisterEvent();

            m_EnumEventChannel = eventChannel;
            m_strEventName = eventName;
            m_Callback = callback;
            m_nPriority = priority;

            C_EventHandler.RegisterEvent(this);
        }

        public void UnregisterEvent()
        {
            if (m_EnumEventChannel != C_EnumEventChannel.Empty && !string.IsNullOrEmpty(m_strEventName))
            {
                C_EventHandler.UnregisterEvent(this);

                m_EnumEventChannel = C_EnumEventChannel.Empty;
                m_strEventName = string.Empty;
                m_Callback = null;
                m_nPriority = 0;
            }
        }

        public void CallBack(params object[] uiObjParams)
        {
            if (m_Callback != null)
                m_Callback(uiObjParams);
        }
    }
}
