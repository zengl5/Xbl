using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    // 帧率计算器
    public class C_FPSCounter
    {
        private const float m_fCalcRate = 0.5f; // 帧率计算频率
        private int m_iFrameCount = 0;          // 本次计算频率下帧数
        private float m_fRateDuration = 0;      // 频率时长
        private int m_iFPS = 0;                 // 显示帧率

        public C_FPSCounter(C_Console console)
        {
            console.onUpdateCallback += OnUpdate;
            console.onGUICallback += OnGUI;
        }

        private void OnUpdate()
        {
            m_iFrameCount++;
            m_fRateDuration += Time.deltaTime;
            if (m_fRateDuration > m_fCalcRate)
            {
                // 计算帧率
                m_iFPS = (int)(m_iFrameCount / m_fRateDuration);
                m_iFrameCount = 0;
                m_fRateDuration = 0.0f;
            }
        }

        private void OnGUI()
        {
            GUI.color = Color.white;

            
            GUI.Label(new Rect(80, 20, 120, 20), "FPS:" + m_iFPS.ToString(), C_Console.ConsoleFontGUIStyle);
        }
    }
}
