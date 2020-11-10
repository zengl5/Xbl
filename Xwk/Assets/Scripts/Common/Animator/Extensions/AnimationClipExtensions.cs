using Assets.Scripts.C_Framework;
using System;
using System.Linq;
using UnityEngine;

namespace YB.AnimCallbacks
{
    public static class AnimationClipExtensions
    {
        private const string OnTimelineEventRaisedMethodName = "OnTimelineEventRaised";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="animEvtReceiverObject"></param>
        /// <param name="atPosition"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static bool BindCallback(this AnimationClip clip, GameObject animEvtReceiverObject, float atPosition, Action callback)
        {
          return  clip.BindOrUnbindCallback(animEvtReceiverObject, atPosition, callback, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="animEvtReceiverObject"></param>
        /// <param name="atPosition"></param>
        /// <param name="callback"></param>
        public static void UnbindCallback(this AnimationClip clip, GameObject animEvtReceiverObject, float atPosition, Action callback)
        {
            clip.BindOrUnbindCallback(animEvtReceiverObject, atPosition, callback, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="animEvtReceiverObject"></param>
        /// <param name="atPosition">当前动画片段的某一个时间位置</param>
        /// <param name="callback"></param>
        /// <param name="bind">绑定事件或者是解绑</param>
        private static bool BindOrUnbindCallback(this AnimationClip clip, GameObject animEvtReceiverObject, float atPosition, Action callback, bool bind)
        {
            var actionWord = bind ? "register" : "unregister";
            if (animEvtReceiverObject == null)
            {
                C_DebugHelper.LogWarningFormat("Trying to {0} callback for null animation event receiver game object", actionWord);
                return false;
            }

            if (callback == null)
            {
                C_DebugHelper.LogWarningFormat("Trying to {0} null callback for animation clip", actionWord);
                return false;
            }

            if (atPosition < 0.0f || atPosition > clip.length)
            {
                C_DebugHelper.LogWarningFormat("Trying to {0} callback for position outside of clip timeline", actionWord);
                return false; 
            }

            var eventReceiver = animEvtReceiverObject.GetComponent<AnimationEventReceiver>();
            if (bind)
            {
                if (eventReceiver == null)
                {
                    eventReceiver = animEvtReceiverObject.AddComponent<AnimationEventReceiver>();
                }

                if (!eventReceiver.RegisterTimelineCallback(atPosition, callback))
                {
                    return false;
                }
               // Debug.Log("clip.AddEvent ："+ clip.name+ "<<<<<<<<<<<<<<<atPosition" + atPosition);
                clip.AddEvent(OnTimelineEventRaisedMethodName, atPosition, atPosition);
            }
            else
            {
                if (eventReceiver == null)
                {
                    Debug.LogWarning("Trying to unregister callback for game object without AnimationEventReceiver component");
                    return false;
                }
               // Debug.Log("clip.RemoveEvent ：" + clip.name + "++++++++++++++++++atPosition" + atPosition);

                var lastCallbackForPositionRemoved = eventReceiver.UnregisterTimelineCallback(atPosition, callback);
                if (lastCallbackForPositionRemoved)
                {
                     clip.RemoveEvent(OnTimelineEventRaisedMethodName, atPosition, atPosition);
                }
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="methodName"></param>
        /// <param name="floatParameter"></param>
        /// <param name="time"></param>
        private static void AddEvent(this AnimationClip clip, string methodName, float floatParameter, float time)
        {
            var clipAnimationEvents = clip.events;
            var animationEvent = Array.Find(clipAnimationEvents,
                e => e.functionName == methodName && e.floatParameter == floatParameter && e.time == time);

            if (animationEvent == null)
            {
                animationEvent = new AnimationEvent();
                animationEvent.functionName = methodName;
                animationEvent.floatParameter = floatParameter;
                animationEvent.time = time;
                clip.AddEvent(animationEvent);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="methodName"></param>
        /// <param name="floatParameter"></param>
        /// <param name="time"></param>
        private static void RemoveEvent(this AnimationClip clip, string methodName, float floatParameter, float time)
        {
            var clipAnimationEvents = clip.events;
            var animationEventIndex = Array.FindIndex(clipAnimationEvents,
                e => e.functionName == methodName && e.floatParameter == floatParameter && e.time == time);

            if (animationEventIndex != -1)
            {
                clipAnimationEvents = clipAnimationEvents.Where((val, idx) => idx != animationEventIndex).ToArray();
                clip.events = clipAnimationEvents;
            }
            else
            {
                C_DebugHelper.LogWarningFormat("Failed to remove animation event for clip {0}", clip.name);
            }
        }
    }
}
