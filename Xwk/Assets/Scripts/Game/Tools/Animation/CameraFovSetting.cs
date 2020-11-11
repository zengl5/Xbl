using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Xbl
{
    class FrameFov
    {
        public FrameFov(int sf, int ef, float efv)
        {
            startFrame = sf;
            endFrame = ef;
            targetFov = efv;
        }
        public int startFrame;
        public int endFrame;
        public float targetFov;
        public float frameTotalTime = 0;
    }
    public class CameraFovSetting : C_MonoSingleton<CameraFovSetting>
    {
        public Camera MainCamera;
        bool getTime = false;
        FrameFov fov=null;
        /// <summary>
        /// 初始化Fov变化范围
        /// </summary>
        /// <param name="stFrame"></param>
        /// <param name="eFrame"></param>
        /// <param name="stFov"></param>
        /// <param name="eFov"></param>
        public void SetCameraFov(Camera ca,int stFrame, int eFrame, float tFov)
        {
            MainCamera = ca;
            fov = new FrameFov(stFrame, eFrame, tFov);
            getTime = false;
        }
        void UpdateFov()
        {
            if (fov == null)
                return;
            if (fov.startFrame>fov.endFrame)//开始进入镜头
            {
                if (!getTime)
                {
                    getTime = true;
                    float fram = fov.startFrame - fov.endFrame;
                    float fram2 = fram / 30;
                    decimal total = System.Math.Round((decimal)fram2, 2);
                    fov.frameTotalTime = (float)total;//计算两帧时间 1S  0.5S
                    MainCamera.DOFieldOfView(fov.targetFov, fov.frameTotalTime);
                }
            }
            else
            {
                if (!getTime)
                {
                    getTime = true;
                    float fram = fov.endFrame - fov.startFrame;
                    float fram2 = fram / 30;
                    decimal total = System.Math.Round((decimal)fram2, 2);
                    fov.frameTotalTime = (float)total;//计算两帧时间 1S  0.5S
                    MainCamera.DOFieldOfView(fov.targetFov, fov.frameTotalTime);
                }
            }
        }
        // Update is called once per frame
        void Update()
        {
            UpdateFov();
        }
    }

}
