using System;
using UnityEngine;

namespace YB.AnimCallbacks
{
    public static class AnimationExtensions
    {
        public static void AddClipStartCallback(this Animation animation, string clipName, Action callback)
        {
            animation.AddClipCallback(clipName, 0.0f, callback);
        }

        public static void AddClipEndCallback(this Animation animation, string clipName, Action callback)
        {
            var clip = animation.GetClip(clipName);
            if (clip == null)
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogWarning("Failed to get animation clip for Animation component");
                return;
            }

            clip.BindCallback(animation.gameObject, clip.length, callback);
        }

        public static void AddClipCallback(this Animation animation, string clipName, float atPosition, Action callback)
        {
            var clip = animation.GetClip(clipName);
            if (clip == null)
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogWarning("Failed to get animation clip for Animation component");
                return;
            }

            clip.BindCallback(animation.gameObject, atPosition, callback);
        }

        public static void RemoveClipStartCallback(this Animation animation, string clipName, Action callback)
        {
            animation.RemoveClipCallback(clipName, 0.0f, callback);
        }

        public static void RemoveClipEndCallback(this Animation animation, string clipName, Action callback)
        {
            var clip = animation.GetClip(clipName);
            if (clip == null)
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogWarning("Failed to get animation clip for Animation component");
                return;
            }

            clip.UnbindCallback(animation.gameObject, clip.length, callback);
        }

        public static void RemoveClipCallback(this Animation animation, string clipName, float atPosition, Action callback)
        {
            var clip = animation.GetClip(clipName);
            if (clip == null)
            {
                Assets.Scripts.C_Framework.C_DebugHelper.LogWarning("Failed to get animation clip for Animation component");
                return;
            }

            clip.UnbindCallback(animation.gameObject, atPosition, callback);
        }
    }
}
