using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Assets.Scripts.C_Framework
{
    // 内存检测器，目前只是输出Profiler信息
    public class C_MemoryDetector
    {
        private readonly static string m_TotalAllocMemroyFormation = "Alloc Memory : {0}M";
        private readonly static string m_TotalReservedMemoryFormation = "Reserved Memory : {0}M";
        private readonly static string m_TotalUnusedReservedMemoryFormation = "Unused Reserved: {0}M";
        private readonly static string m_MonoHeapFormation = "Mono Heap : {0}M";
        private readonly static string m_MonoUsedFormation = "Mono Used : {0}M";

        // 字节到兆
        private float m_ByteToM = 0.000001f;

        private Rect m_AllocMemoryRect;
        private Rect m_ReservedMemoryRect;
        private Rect m_UnusedReservedMemoryRect;
        private Rect m_MonoHeapRect;
        private Rect m_MonoUsedRect;

        public C_MemoryDetector(C_Console console)
        {
            int x = 60;
            int y = 100;
            int w = 200;
            int h = 60;

            m_AllocMemoryRect = new Rect(x, y, w, h);
            m_ReservedMemoryRect = new Rect(x, y + h, w, h);
            m_UnusedReservedMemoryRect = new Rect(x, y + 2 * h, w, h);
            m_MonoHeapRect = new Rect(x, y + 3 * h, w, h);
            m_MonoUsedRect = new Rect(x, y + 4 * h, w, h);

            console.onGUICallback += OnGUI;
        }

        void OnGUI()
        {
            GUI.Label(m_AllocMemoryRect, string.Format(m_TotalAllocMemroyFormation, Profiler.GetTotalAllocatedMemoryLong() * m_ByteToM), C_Console.ConsoleFontGUIStyle);
            GUI.Label(m_ReservedMemoryRect, string.Format(m_TotalReservedMemoryFormation, Profiler.GetTotalReservedMemoryLong() * m_ByteToM), C_Console.ConsoleFontGUIStyle);
            GUI.Label(m_UnusedReservedMemoryRect, string.Format(m_TotalUnusedReservedMemoryFormation, Profiler.GetTotalUnusedReservedMemoryLong() * m_ByteToM), C_Console.ConsoleFontGUIStyle);
            GUI.Label(m_MonoHeapRect, string.Format(m_MonoHeapFormation, Profiler.GetMonoHeapSizeLong() * m_ByteToM), C_Console.ConsoleFontGUIStyle);
            GUI.Label(m_MonoUsedRect, string.Format(m_MonoUsedFormation, Profiler.GetMonoUsedSizeLong() * m_ByteToM), C_Console.ConsoleFontGUIStyle);
        }
    }
}
