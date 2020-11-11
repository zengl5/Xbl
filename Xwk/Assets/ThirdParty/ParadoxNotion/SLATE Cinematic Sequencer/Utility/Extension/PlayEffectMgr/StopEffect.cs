using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
namespace Slate.ActionClips
{
    [Category("Effect")]
    [Description("停止音效")]
    public class StopEffect : DirectorActionClip
    {
        public string effectName;
 
        protected override void OnEnter()
        {
            if (UnityEngine.Application.isPlaying)
            {
                AudioManager.Instance.StopEffectByKey(effectName);
            }
        }
    }
}
