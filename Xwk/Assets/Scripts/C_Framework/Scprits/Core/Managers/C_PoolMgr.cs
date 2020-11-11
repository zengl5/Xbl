using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public enum C_PoolChannel
    {
        None,
        Global,
        UI,
        Avatar,
        Sound
    }

    // 池管理
    public class C_PoolMgr : C_MonoSingleton<C_PoolMgr>
    {
        // 每个消息名字维护一组消息捕捉器
        private Dictionary<C_PoolChannel, List<Object>> m_PoolHandlerDict = new Dictionary<C_PoolChannel, List<Object>>();

        private const int m_Capacity = 100;

        public Object Spawn(string resName, string resType)
        {
            return Spawn(resName, GetPoolChannel(resType));
        }

        public Object Spawn(string resName, C_PoolChannel channel = C_PoolChannel.Global)
        {
            if (string.IsNullOrEmpty(resName) || channel == C_PoolChannel.None)
                return null;

            resName = StandardResName(resName);

            if (m_PoolHandlerDict.ContainsKey(channel))
            {
                for (int i = 0; i < m_PoolHandlerDict[channel].Count; i++)
                {
                    if (m_PoolHandlerDict[channel][i] == null)
                    {
                        m_PoolHandlerDict[channel].RemoveAt(i);
                        break;
                    }                     

                    if (m_PoolHandlerDict[channel][i].name == resName)
                    {
                        Object go = m_PoolHandlerDict[channel][i];
                        m_PoolHandlerDict[channel].RemoveAt(i);

                        return go;
                    }
                }
            }

            return null;
        }

        // 将用完的GameObject放入m_DormantObjects中
        public void Despawn(Object objectParam, C_PoolChannel channel = C_PoolChannel.Global)
        {
            if (objectParam == null || channel == C_PoolChannel.None)
            {
                C_DebugHelper.LogError("C_PoolMgr Despawn: Object is Null!");
                return;
            }

            if (string.IsNullOrEmpty(objectParam.name))
            {
                C_DebugHelper.LogError("C_PoolMgr Despawn: Object name is Null or Empty!");
                return;
            }

            // 添加通道
            if (!m_PoolHandlerDict.ContainsKey(channel))
                m_PoolHandlerDict[channel] = new List<Object>();

            if (!m_PoolHandlerDict[channel].Contains(objectParam))
            {
                m_PoolHandlerDict[channel].Add(objectParam);
                //Debug.LogError("添加 对象池");
            }

            while (m_PoolHandlerDict[channel].Count > m_Capacity)
            {
                Object dob = m_PoolHandlerDict[channel][0];
                m_PoolHandlerDict[channel].RemoveAt(0);
                Destroy(dob);
            }
        }

        //销毁同样名字的对象
        public void Destory(string resName, C_PoolChannel channel = C_PoolChannel.Global)
        {
            if (string.IsNullOrEmpty(resName) || channel == C_PoolChannel.None)
            {
                C_DebugHelper.LogError("C_PoolMgr Destory: resName is Null or Empty!");
                return;
            }

            resName = StandardResName(resName);

            if (m_PoolHandlerDict.ContainsKey(channel))
            {
                for (int i = m_PoolHandlerDict[channel].Count - 1; i >= 0; i++)
                {
                    if (m_PoolHandlerDict[channel][i] != null && m_PoolHandlerDict[channel][i].name == resName)
                    {
                        Object go = m_PoolHandlerDict[channel][i];
                        m_PoolHandlerDict[channel].RemoveAt(i);
                        Destroy(go);
                    }
                }
            }
        }

        private string StandardResName(string resName)
        {
            if (string.IsNullOrEmpty(resName))
                return "";

            resName = resName.Substring(resName.LastIndexOf("/") + 1).ToLower();

            int end = resName.LastIndexOf('.');
            return resName.Substring(0, (end == -1 ? resName.Length : end));
        }

        public static C_PoolChannel GetPoolChannel(string type)
        {
            switch (type)
            {
                case "sound":
                    return C_PoolChannel.Sound;

                default:
                    return C_PoolChannel.Global;
            }
        }
    }
}