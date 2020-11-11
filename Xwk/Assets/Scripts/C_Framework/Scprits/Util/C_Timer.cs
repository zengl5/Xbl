using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_Timer
    {
        private int m_nTotalTime = 1000;
        private Action m_TimeUpAction = null;
        public int Sequence = 0;
        public string AcitonName;
        private int m_nLoop = 1;

        private bool m_bIsRunning = true;
        private bool m_bIsFinished = false;

        private int m_nCurrentTime = 0;
        public float CurrentTime { get { return m_nCurrentTime / 1000.0f; } }

        public C_Timer(float time, Action timeUpAction, int sequence, int loop)
        {
            m_nTotalTime = (int)(time * 1000);
            m_TimeUpAction = timeUpAction;
            Sequence = sequence;
            m_nLoop = loop;

            m_nCurrentTime = 0;
            m_bIsRunning = true;
            m_bIsFinished = false;
        }

        public C_Timer(float time, Action timeUpAction,string name, int loop)
        {
            m_nTotalTime = (int)(time * 1000);
            m_TimeUpAction = timeUpAction;
            AcitonName = name;
            m_nLoop = loop;

            m_nCurrentTime = 0;
            m_bIsRunning = true;
            m_bIsFinished = false;
        }

        public void Update(int deltaTime)
        {
            if (m_bIsFinished || !m_bIsRunning)
                return;

            m_nCurrentTime += deltaTime;
            if (m_nCurrentTime >= m_nTotalTime)
            {
                if (m_TimeUpAction != null)
                    m_TimeUpAction();

                m_nCurrentTime = 0;

                m_nLoop--;
                if (m_nLoop == 0)
                    m_bIsFinished = true;
            }
        }

        public float GetLeftTime()
        {
            return (m_nTotalTime - m_nCurrentTime) / 1000.0f;
        }

        public void Finish()
        {
            m_bIsFinished = true;
        }

        public bool IsFinished()
        {
            return m_bIsFinished;
        }

        public void Pause()
        {
            m_bIsRunning = false;
        }

        public void Resume()
        {
            m_bIsRunning = true;
        }

        public void Reset()
        {
            m_nCurrentTime = 0;
        }

        public void ResetTotalTime(float totalTime)
        {
            m_nCurrentTime = 0;
            m_nTotalTime = (int)(totalTime * 1000);
        }
    }
}
