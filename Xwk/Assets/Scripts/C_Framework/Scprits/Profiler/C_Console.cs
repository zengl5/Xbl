using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Scripts.C_Framework
{
    // 控制台GUI输出类
    // 包括FPS，内存使用情况，日志GUI输出
    public class C_Console : C_MonoSingleton<C_Console>
    {
        public class C_ConsoleMessage
        {
            public readonly string Message;
            public readonly string StackTrace;
            public readonly LogType Type;

            public C_ConsoleMessage(string message, string stackTrace, LogType type)
            {
                Message = message;
                StackTrace = stackTrace;
                Type = type;
            }
        }

        // Update回调
        public C_VoidDelegate.WithVoid onUpdateCallback = null;

        // OnGUI回调
        public C_VoidDelegate.WithVoid onGUICallback = null;

#if UNITY_IOS && !UNITY_EDITOR
        bool m_Touching = false;
#endif
        //显示调试GUI
        private bool m_ShowGUI = true;

        private List<C_ConsoleMessage> m_EntriesList = new List<C_ConsoleMessage>();

        private const int m_Margin = 20;
        private Rect m_WindowRect = new Rect(m_Margin + Screen.width * 0.5f, m_Margin, Screen.width * 0.5f - (2 * m_Margin), Screen.height - (2 * m_Margin));
        private bool m_ScrollToBottom = true;
        private Vector2 m_ScrollPos = Vector2.zero;
        private bool m_Collapse = false;
        private GUIContent m_ClearLabel = new GUIContent("Clear", "Clear the contents of the console.");
        private GUIContent m_CollapseLabel = new GUIContent("Collapse", "Hide repeated messages.");
        private GUIContent m_ScrollToBottomLabel = new GUIContent("ScrollToBottom", "Scroll bar always at bottom");

        public static GUIStyle ConsoleFontGUIStyle = new GUIStyle();

        private GUIStyle ConsoleFontGUIStyle2 = new GUIStyle();

        protected override void Init()
        {
            ConsoleFontGUIStyle.normal.textColor = Color.white;
            ConsoleFontGUIStyle.fontSize = 30;

            ConsoleFontGUIStyle2.normal.textColor = Color.white;
            ConsoleFontGUIStyle2.fontSize = 30;

            new C_FPSCounter(this);
            new C_MemoryDetector(this);

            Application.logMessageReceived += HandleLog; 
           
        }

        void OnGUI()
        {
            if (!m_ShowGUI)
                return;

            if (onGUICallback != null)
                onGUICallback();

            if (GUI.Button(new Rect(100, 450, 200, 100), "清空数据", ConsoleFontGUIStyle))
            {
                PlayerPrefs.DeleteAll();

#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
				Application.Quit ();
#endif
            }

            m_WindowRect = GUILayout.Window(123456, m_WindowRect, ConsoleWindow, "Console");
        }

        void Destroy()
        {
            Application.logMessageReceived -= HandleLog;
        }

        // 窗口显示记录的消息。
        private void ConsoleWindow(int windowID)
        {
            if (m_ScrollToBottom)
                GUILayout.BeginScrollView(Vector2.up * m_EntriesList.Count * 100.0f);
            else
                m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos);

            // 检查每一个记录
            for (int i = 0; i < m_EntriesList.Count; i++)
            {
                C_ConsoleMessage entry = m_EntriesList[i];

                // 跳过，和最后一个的记录一样的记录
                if (m_Collapse && i > 0 && entry.Message == m_EntriesList[i - 1].Message)
                    continue;

                // 根据日志类型改变文本颜色
                switch (entry.Type)
                {
                    case LogType.Error:
                    case LogType.Exception:
                        GUI.contentColor = Color.red;
                        break;

                    case LogType.Warning:
                        GUI.contentColor = Color.yellow;
                        break;

                    default:
                        GUI.contentColor = Color.white;
                        break;
                }

                if (entry.Type == LogType.Exception)
                    GUILayout.Label(entry.Message + " || " + entry.StackTrace, ConsoleFontGUIStyle2);
                else
                    GUILayout.Label(entry.Message, ConsoleFontGUIStyle2);
            }

            GUI.contentColor = Color.white;
            GUILayout.EndScrollView();
            GUILayout.BeginHorizontal();

            // 清除按钮
            if (GUILayout.Button(m_ClearLabel))
                m_EntriesList.Clear();

            // 崩溃切换
            m_Collapse = GUILayout.Toggle(m_Collapse, m_CollapseLabel, GUILayout.ExpandWidth(false));
            m_ScrollToBottom = GUILayout.Toggle(m_ScrollToBottom, m_ScrollToBottomLabel, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        private void HandleLog(string message, string stackTrace, LogType type)
        {
            m_EntriesList.Add(new C_ConsoleMessage(message, stackTrace, type));
        }

        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyUp(KeyCode.F1))
                m_ShowGUI = !m_ShowGUI;

#elif UNITY_ANDROID
			if ( Input.GetKeyUp (KeyCode.Escape) )
				m_ShowGUI = !m_ShowGUI;

#elif UNITY_IOS
			if (!m_Touching && Input.touchCount == 4)
			{
				m_Touching = true;
				m_ShowGUI = !m_ShowGUI;
			}
			else
			{
				m_Touching = false;
			}
#endif

            if (onUpdateCallback != null)
                onUpdateCallback();
        }
    }
}