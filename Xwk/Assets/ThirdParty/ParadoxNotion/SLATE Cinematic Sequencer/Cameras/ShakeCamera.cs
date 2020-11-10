using Slate.ActionClips;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Slate
{
    [Description("震动相机工具")]
    [Category("Camera")]
    [RequireComponent(typeof(Camera))]
    public class ShakeCamera : ActorActionClip
    {
        private Camera _cam;
        public Camera cam
        {
            get { return _cam != null ? _cam : _cam = actor.GetComponent<Camera>(); }
        }
        #region 设置相机的晃动参数
        //设置相机的
        [HideInInspector]
        public float shakeTime = 0.0f;
        [HideInInspector]
        public float shakeamp = 0.01f;
        private float frameTime = 0f;
        #endregion

        [SerializeField]
        [HideInInspector]
        private float _length = 1;
        public override float length
        {
            get { return _length; }
            set { _length = value; }
        }

        public override float blendIn
        {
            get { return length; }
        }

        protected override void OnEnter()
        { 
           // shakeTime = 0.1f;
            frameTime = 0f;
        }

        protected override void OnUpdate(float deltaTime)
        {
            frameTime += Time.deltaTime;

            if (frameTime > shakeTime)
            {
                frameTime = 0;
                cam.rect = new Rect(shakeamp * (-1.0f + 2.0f * Random.value), shakeamp * (-1.0f + 2.0f * Random.value), 1.0f, 1.0f);
            }
        }
        protected override void OnExit()
        {
            base.OnExit();
            cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            shakeTime = 0.1f;
            frameTime = 0.01f;
        }
        protected override void OnReverse()
        {
           
        }

    }
}
