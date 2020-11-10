using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Animations;
using UnityEngine.Playables;
#else
using UnityEngine.Experimental.Director;
#endif
namespace Slate
{
    [Icon("AlembicIcon")]
    [Description("自定义的点击事件处理轨道")]
    [Attachable(typeof(ActorGroup))]
    [Name("Click Track")]
    public class ClickTrack : ActionTrack 
    {
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

