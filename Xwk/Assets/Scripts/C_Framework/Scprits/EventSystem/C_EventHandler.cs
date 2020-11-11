using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public enum C_EnumEventChannel
    {
        Empty,
        Global,
        UI,
        Logic,
        Helper
    }

    public class C_EventHandler
    {
        // 每个消息名字维护一组消息捕捉器
        private static Dictionary<C_EnumEventChannel, Dictionary<string, List<C_Event>>> m_EventHandlerDict = new Dictionary<C_EnumEventChannel, Dictionary<string, List<C_Event>>>();

        // 注册消息, 注意第一个参数,使用了C# this的扩展, 所以只有实现IMsgReceiver的对象才能调用此方法
        public static void RegisterEvent(C_Event cEvent)
        {
            if (cEvent == null)
            {
                Debug.LogError("C_EventHandler RegisterEvent: Event is Null!");
                return;
            }

            if (cEvent.m_EnumEventChannel == C_EnumEventChannel.Empty)
            {
                Debug.LogError("C_EventHandler RegisterEvent: Event channel is Empty!");
                return;
            }

            if (string.IsNullOrEmpty(cEvent.m_strEventName))
            {
                Debug.LogError("C_EventHandler RegisterEvent: Event name is Null or Empty!");
                return;
            }

            // 添加消息通道
            if (!m_EventHandlerDict.ContainsKey(cEvent.m_EnumEventChannel))
            {
                m_EventHandlerDict[cEvent.m_EnumEventChannel] = new Dictionary<string, List<C_Event>>();
            }

            if (!m_EventHandlerDict[cEvent.m_EnumEventChannel].ContainsKey(cEvent.m_strEventName))
            {
                m_EventHandlerDict[cEvent.m_EnumEventChannel][cEvent.m_strEventName] = new List<C_Event>();
            }

            var handlers = m_EventHandlerDict[cEvent.m_EnumEventChannel][cEvent.m_strEventName];

            // 防止重复注册
            for (int i = 0; i < handlers.Count; i++)
            {
                if (handlers[i] == cEvent)
                {
                    Debug.LogWarning("C_EventHandler RegisterEvent: Event already register!");
                    return;
                }
            }

            handlers.Add(cEvent);
        }

        // 注销消息
        public static void UnregisterEvent(C_Event cEvent)
        {
            if (cEvent == null)
            {
                Debug.LogError("C_EventHandler UnregisterEvent: Event is Null!");
                return;
            }

            if (cEvent.m_EnumEventChannel == C_EnumEventChannel.Empty)
            {
                Debug.LogError("C_EventHandler UnregisterEvent: Event channel is Empty!");
                return;
            }

            if (string.IsNullOrEmpty(cEvent.m_strEventName))
            {
                Debug.LogError("C_EventHandler UnregisterEvent: Event name is Null or Empty!");
                return;
            }

            if (!m_EventHandlerDict.ContainsKey(cEvent.m_EnumEventChannel))
            {
                Debug.LogError("C_EventHandler UnregisterEvent Channel:" + cEvent.m_EnumEventChannel.ToString() + " doesn't exist!");
                return;
            }

            if (!m_EventHandlerDict[cEvent.m_EnumEventChannel].ContainsKey(cEvent.m_strEventName))
            {
                Debug.LogError("C_EventHandler UnregisterEvent Name:" + cEvent.m_strEventName + " doesn't exist!");
                return;
            }

            var handlers = m_EventHandlerDict[cEvent.m_EnumEventChannel][cEvent.m_strEventName];

            // 删除List需要从后向前遍历, 从前向后遍历删除后索引值会不断变化
            for (int index = handlers.Count - 1; index >= 0; index--)
            {
                var handler = handlers[index];
                if (handler == cEvent)
                {
                    handlers.Remove(handler);
                    break;
                }
            }
        }


        #region SendEvent

        public static void SendEvent(C_EnumEventChannel enumEventChannel, string eventName, params object[] uiObjParams)
        {
            if (enumEventChannel == C_EnumEventChannel.Empty)
            {
                Debug.LogWarning("C_EventHandler SendEvent: Event channel is Empty!");
                return;
            }

            if (string.IsNullOrEmpty(eventName))
            {
                Debug.LogWarning("C_EventHandler SendEvent:" + eventName + " is Null or Empty!");
                return;
            }

            if (!m_EventHandlerDict.ContainsKey(enumEventChannel))
            {
                Debug.LogWarning("C_EventHandler SendEvent Channel:" + enumEventChannel.ToString() + " doesn't exist!");
                return;
            }

            if (!m_EventHandlerDict[enumEventChannel].ContainsKey(eventName))
            {
                Debug.LogWarning("C_EventHandler SendEvent Name:" + eventName + " doesn't exist!");
                return;
            }

            var handlers = m_EventHandlerDict[enumEventChannel][eventName];
            handlers.Sort(Compare);
            // 删除List需要从后向前遍历, 从前向后遍历删除后索引值会不断变化
            for (int index = handlers.Count - 1; index >= 0; index--)
            {
                var handler = handlers[index];
                if (handler != null)
                {
                    handler.CallBack(uiObjParams);
                }
                else
                {
                    handlers.RemoveAt(index);
                }
            }
        }

        private static int Compare(C_Event cEvent1, C_Event cEvent2)
        {

            if (cEvent1.m_nPriority < cEvent2.m_nPriority)
            {
                return -1;
            }
            else if (cEvent1.m_nPriority > cEvent2.m_nPriority)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        #endregion
    }
}
