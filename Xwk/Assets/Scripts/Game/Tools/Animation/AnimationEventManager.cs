using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using Xbl;
namespace Xbl
{
    /// <summary>
    /// 动画帧事件自动添加类
    /// </summary>
    public class MyAnimationEvent
    {
        AnimationClip clip;
        AnimationEvent aEvent;
        public MyAnimationEvent(AnimationClip cp, string funName)
        {
            clip = cp;
            aEvent = new AnimationEvent();
            aEvent.functionName = funName;
        }
        public void AddAnimationFun(float timer)
        {
            aEvent.time = timer;
            clip.AddEvent(aEvent);
        }
        public void AddAnimationFun(float timer, UnityEngine.Object obj)
        {
            aEvent.objectReferenceParameter = obj;
            aEvent.time = timer;
            clip.AddEvent(aEvent);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timer">在动画帧时间写入方法</param>
        /// <param name="framRange">Fov变化范围（帧A-帧B）</param>
        /// <param name="targetFov">Fov幅度</param>
        public void AddAnimationFun(float timer, string parm=null)
        {
            aEvent.stringParameter = parm;//帧范围[10X20]                                            //aEvent.floatParameter = targetFov;
            aEvent.time = timer;
            clip.AddEvent(aEvent);
        }
    }

    public class AnimationEventManager :C_MonoSingleton<AnimationEventManager>
    {
        List<AnimationClip> AnimationClipList = new List<AnimationClip>();
        Dictionary<string, AnimationClip> AnimDic = new Dictionary<string, AnimationClip>();
        /// <summary>
        /// 注册动画事件方法
        /// </summary>
        /// <param name="clip">AnimationClip</param>
        /// <param name="clipFunTime">执行时间</param>
        /// <param name="clipFunName">对应的方法名</param>
        /// <param name="parm">参数</param>
        public void RegisterAnimationFun(AnimationClip clip,float clipFunTime,string clipFunName,string parm=null)
        {         
            MyAnimationEvent myEvent = new MyAnimationEvent(clip, clipFunName);
            myEvent.AddAnimationFun(clipFunTime, parm);
            AnimationClipList.Add(clip);
            if(!AnimDic.ContainsKey(clip.name))
            AnimDic.Add(clip.name, clip);
        }
        public bool HaveRegist(string clipName)
        {
            for (int i = 0; i < AnimationClipList.Count; i++)
                if (AnimationClipList[i].name.Contains(clipName))
                    return true;
            return false;
        }
        public void UnRegisterAllAnimationFun()
        {
            for(int i=0;i<AnimationClipList.Count;i++)
                if(AnimationClipList[i]!=null)
                AnimationClipList[i].events = default(AnimationEvent[]);
            foreach(AnimationClip clip in AnimDic.Values)
                if (clip != null)
                    clip.events= default(AnimationEvent[]);
        }
        public void UnRegisterAnimationFun(AnimationClip clip)
        {
            AnimationClipList.Remove(clip);
            clip.events = default(AnimationEvent[]);
        }
        public void UnRegisterAnimationFun(string name)
        {
            if (AnimDic.ContainsKey(name))
            {
                AnimDic[name].events= default(AnimationEvent[]);
                AnimDic.Remove(name);
            }
        }
    }
}