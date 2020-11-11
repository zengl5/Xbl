using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_TimerMgr : C_Singleton<C_TimerMgr>
    {
        //注意后期臃肿问题
        private List<C_Timer> m_TimerList = new List<C_Timer>();

        private int m_nTimerSequence = 0;

        private int time = 0;
        public void Update()
        {           
                time = (int)(Time.deltaTime * 1000);
                for (int i = 0; i < m_TimerList.Count; i++)
                {
                if (m_TimerList[i].IsFinished())
                {
                    m_TimerList.RemoveAt(i);
                }
                else
                    m_TimerList[i].Update(time);
                }                   
        }
        /// <summary>
        /// 添加延迟事件方法
        /// </summary>
        /// <param name="time">延迟时间</param>
        /// <param name="timeUpAction">方法</param>
        /// <param name="name">方法名</param>
        /// <param name="loop">是否循环？</param>
        public void AddTimer(float time, Action timeUpAction,string name,int loop = 1)
        {
            if (timeUpAction == null)
            {
                C_DebugHelper.LogError("C_TimerMgr AddTimer timeUpAction is null!");
            }
            m_TimerList.Add(new C_Timer(time, timeUpAction, name, loop));
        }
        /// <summary>
        /// 移除事件方法
        /// </summary>
        /// <param name="name"></param>
        public void RemoveTimer(string name)
        {
            for (int i = 0; i < m_TimerList.Count; i++)
            {
                if (m_TimerList[i].AcitonName == name)
                {
                    //Debug.LogError("移除事件*****"+name);
                    m_TimerList[i].Finish();
                    m_TimerList.RemoveAt(i);
                }
            }
        }
        public bool GetNowTimer(string name)
        {
            for (int i = 0; i < m_TimerList.Count; i++)
            {
                if (m_TimerList[i].AcitonName == name)
                {
                    return true;
                }
            }
            return false;
        }
        public int AddTimer(float time, Action timeUpAction, int loop = 1)
        {
            if (timeUpAction == null)
            {
                C_DebugHelper.LogError("C_TimerMgr AddTimer timeUpAction is null!");
                return -1;
            }

            m_nTimerSequence++;

            m_TimerList.Add(new C_Timer(time, timeUpAction, m_nTimerSequence, loop));

            return m_nTimerSequence;
        }

        public void RemoveTimer(int sequence)
        {
            for (int i = 0; i < m_TimerList.Count; i++)
            {
                if (m_TimerList[i].Sequence == sequence)
                {
                    m_TimerList[i].Finish();
                    return;
                }
            }
        }
       

        public void PauseTimer(int sequence)
        {
            C_Timer timer = GetTimer(sequence);
            if (timer != null)
                timer.Pause();
        }

        public void ResumeTimer(int sequence)
        {
            C_Timer timer = GetTimer(sequence);
            if (timer != null)
                timer.Resume();
        }

        public void ResetTimer(int sequence)
        {
            C_Timer timer = GetTimer(sequence);
            if (timer != null)
                timer.Reset();
        }

        public void ResetTimerTotalTime(int sequence, float totalTime)
        {
            C_Timer timer = GetTimer(sequence);
            if (timer != null)
                timer.ResetTotalTime(totalTime);
        }

        public float GetTimerCurrent(int sequence)
        {
            C_Timer timer = GetTimer(sequence);
            if (timer != null)
                return timer.CurrentTime;

            return -1;
        }

        public float GetLeftTime(int sequence)
        {
            C_Timer timer = GetTimer(sequence);
            if (timer != null)
                return timer.GetLeftTime();

            return -1;
        }

        public C_Timer GetTimer(int sequence)
        {
            if (sequence <= 0)
                return null;

            for (int i = 0; i < m_TimerList.Count; i++)
            {
                if (m_TimerList[i].Sequence == sequence)
                    return m_TimerList[i];
            }

            return null;
        }

        public void RemoveAllTimer()
        {
            m_TimerList.Clear();
        }
    }
}