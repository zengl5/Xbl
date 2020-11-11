using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    // 全局唯一，继承于MonoBehaviour的单例类，保证其他公共模块都以C_GameFramework的生命周期为准
    public class C_GameFramework : C_MonoSingleton<C_GameFramework>
    {
        public static float c_DesignWidth = 1920.0f;
        public static float c_DesignHeight = 1080.0f;
        public static float c_DesignAspectRatio = c_DesignWidth / c_DesignHeight;

        public C_VoidDelegate.WithFloat onRealtimeUpdate = null;

        private float m_fTimeAtLastFrame = -1;
 

        protected override void Init()
        {
            c_DesignAspectRatio = c_DesignWidth / c_DesignHeight;

            StartPrepareSystem();
        }

        protected virtual void StartPrepareSystem()
        {
            //初始化一些架构
            InitCoreSys();
        }

        protected void InitCoreSys()
        {
            C_Singleton<C_TimerMgr>.CreateInstance();

            C_Singleton<C_GameStateCtrl>.CreateInstance();

            C_MonoSingleton<C_AudioMgr>.GetInstance();

            C_MonoSingleton<C_UIMgr>.GetInstance();
        }
        
        void Update()
        { 
            if (onRealtimeUpdate != null)
                onRealtimeUpdate(Time.realtimeSinceStartup - m_fTimeAtLastFrame);

            m_fTimeAtLastFrame = Time.realtimeSinceStartup;

            UpdateElse();
        }

        private void UpdateElse()
        {
            C_Singleton<C_TimerMgr>.GetInstance().Update();
        }

        protected override void OnDestroy()
        {
            onRealtimeUpdate = null;
        }
    }
}
