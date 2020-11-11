using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Slate.ActionClips
{
    [Category("Shadow")]
    public class ShadowSlate : DirectorActionClip
    {
        [Tooltip("阴影位置")]
        public Vector3 ShaowPos;
        public Quaternion ShaowQuaternion;
        protected override void OnEnter()
        {
            if (UnityEngine.Application.isPlaying)
            {
               
            }
        }
    }
}

