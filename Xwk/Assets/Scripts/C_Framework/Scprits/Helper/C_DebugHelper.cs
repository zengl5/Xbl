using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_DebugHelper : C_Singleton<C_DebugHelper>
    {
        #region 日志

        public static void Log(object message, UnityEngine.Object context)
        {
#if UNITY_EDITOR
            Debug.Log(message, context);
#else
            if (GameConfig.LogState > 0)
                Debug.Log(message, context);
#endif

        }

        public static void Log(object message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#else
           if (GameConfig.LogState > 0)
                Debug.Log(message);
#endif

        }

        public static void LogError(object message, UnityEngine.Object context)
        {
#if UNITY_EDITOR
            Debug.LogError(message, context);
#else
            if (GameConfig.LogState > 0)
                Debug.LogError(message, context);
#endif

        }

        public static void LogError(object message)
        {
#if UNITY_EDITOR
            Debug.LogError(message);
#else
            if (GameConfig.LogState > 0)
                Debug.LogError(message);
#endif
        }
        public static void LogErrorFormat(string format, params object[] args)
        {
            if (GameConfig.LogState > 0)
                Debug.LogErrorFormat(format, args);
        }

        public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
        {
            if (GameConfig.LogState > 0)
                Debug.LogErrorFormat(context, format, args);
        }

        public static void LogException(Exception exception, UnityEngine.Object context)
        {
            if (GameConfig.LogState > 0)
                Debug.LogException(exception, context);
        }

        public static void LogException(Exception exception)
        {
            if (GameConfig.LogState > 0)
                Debug.LogException(exception);
        }

        public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
        {
            if (GameConfig.LogState > 0)
                Debug.LogFormat(context, format, args);
        }

        public static void LogFormat(string format, params object[] args)
        {
            if (GameConfig.LogState > 0)
                Debug.LogFormat(format, args);
        }

        public static void LogWarning(object message)
        {
            if (GameConfig.LogState > 0)
                Debug.LogWarning(message);
        }

        public static void LogWarning(object message, UnityEngine.Object context)
        {
            if (GameConfig.LogState > 0)
                Debug.LogWarning(message, context);
        }

        public static void LogWarningFormat(string format, params object[] args)
        {
            if (GameConfig.LogState > 0)
                Debug.LogWarningFormat(format, args);
        }

        public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
        {
            if (GameConfig.LogState > 0)
                Debug.LogWarningFormat(context, format, args);
        }

#endregion
    }
}