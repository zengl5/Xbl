using System;
using System.Collections.Generic;
using UnityEngine;

namespace YB.AnimCallbacks
{
    public class AnimationEventReceiver : MonoBehaviour
    {
        private Dictionary<float, List<Action>> animationTimelineCallbacks = new Dictionary<float, List<Action>>();

        public bool RegisterTimelineCallback(float atPosition, Action callback)
        {
            //Debug.Log("RegisterTimelineCallback------------------------------atPosition："+ atPosition);
            if (callback == null)
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogWarning("Trying to register null animation timeline callback");
                return false;
            }

            if (!animationTimelineCallbacks.ContainsKey(atPosition))
            {
                animationTimelineCallbacks.Add(atPosition, new List<Action>());
            }
            List<Action> callbackList = animationTimelineCallbacks[atPosition];
            //去重复
            //if (callbackList.Exists(p=> p == callback))
            //{
            //    for (int i = callbackList.Count - 1; i >= 0; i--)
            //    {
            //        if (callbackList[i] == callback)
            //        {
            //            callbackList.Remove(callbackList[i]);
            //        }
            //    }
            //}
            //只允许一个时间点一个回调函数--不然会出现回调没有删除的bug
            callbackList.Clear();
            callbackList.Add(callback);
            return true;
        }

        public bool UnregisterTimelineCallback(float atPosition, Action callback)
        {
            if (callback == null)
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogWarning("Trying to unregister null animation timeline callback");
                return false;
            }

            if (!animationTimelineCallbacks.ContainsKey(atPosition))
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogWarningFormat("Trying to unregister animation timeline callback not registered at timeline position {0}", atPosition);
                return false;
            }

            var removed = animationTimelineCallbacks[atPosition].Remove(callback);
            if (!removed)
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogWarning("Failed to unregister animation timeline callback since it was not registered");
                return false;
            }

            var lastCallbackForPositionRemoved = animationTimelineCallbacks[atPosition].Count == 0;
           // Debug.Log("unRegisterTimelineCallback***********atPosition：" + atPosition);

            if (lastCallbackForPositionRemoved)
            {

                animationTimelineCallbacks.Remove(atPosition);
            }

            return lastCallbackForPositionRemoved;
        }

        // Unity binds animation events by method name. This means all components
        // which have method with the name from animation event will be called.
        // Such component must be attached to the object with Animator/Animation.
        private void OnTimelineEventRaised(float atPosition)
        {
            if (animationTimelineCallbacks == null)
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogError("animationTimelineCallbacks  is null");
                return;
            }
            if (!animationTimelineCallbacks.ContainsKey(atPosition))
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogErrorFormat("Callbacks not registered for timeline position {0}", atPosition);
                return;
            }

            var animationPositionCallbacks = animationTimelineCallbacks[atPosition];
            FireCallbacks(animationPositionCallbacks);
           // UnRegisterAllTimeLineCallback(atPosition, animationPositionCallbacks);
        }
        /// <summary>
        /// 销毁某一个时间点的所有回调函数
        /// </summary>
        /// <param name="atPosition"></param>
        /// <param name="callbacks"></param>
        private void UnRegisterAllTimeLineCallback(float atPosition,List<Action> callbacks)
        {
            var count = callbacks.Count;
            for (var i = 0; i < count; i++)
            {
                var callback = callbacks[i];
                UnregisterTimelineCallback(atPosition, callback);
            }
        }

        // Unity cannot call static method from animation event so FireCallbacks
        // cannot be called for AnimationEventReceiver added by user.
        private static void FireCallbacks(List<Action> callbacks)
        {
            // In current implementation registered callbacks cannot be removed.
            // Store current count in local variable for the case if callback
            // adds new callback to the list. Added one will be triggered next
            // time animation event happens.
            var count = callbacks.Count;
            for (var i = callbacks.Count-1; i >= 0; i--)
            {
                var callback = callbacks[i];
                callback();
            }
        }
    }
}
