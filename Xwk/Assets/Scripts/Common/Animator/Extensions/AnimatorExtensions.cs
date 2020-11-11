using System;
using UnityEngine;

namespace YB.AnimCallbacks
{
    public static class AnimatorExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="clipName"></param>
        /// <param name="callback"></param>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        public static bool AddClipStartCallback(this Animator animator, string clipName, Action callback, int layerIndex=0)
        {
           return animator.AddClipCallback(clipName,0.0f,callback,layerIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="clipName"></param>
        /// <param name="callback"></param>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        public static bool AddClipEndCallback(this Animator animator, string clipName, Action callback, int layerIndex = 0)
        {
            var clip = animator.GetAnimationClip(layerIndex, clipName);
            if (clip == null)
            {
                Debug.LogWarning("Failed to get animation clip for Animator component");
                return false;
            }
            return clip.BindCallback(animator.gameObject, clip.length, callback);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="clipName"></param>
        /// <param name="atPosition"></param>
        /// <param name="callback"></param>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        public static bool AddClipCallback(this Animator animator,  string clipName, float atPosition, Action callback, int layerIndex=0)
        {
            var clip = animator.GetAnimationClip(layerIndex, clipName);
            if (clip == null)
            {
                Debug.LogWarning("Failed to get animation clip for Animator component");
                return false;
            }

           return clip.BindCallback(animator.gameObject, atPosition, callback);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="clipName"></param>
        /// <param name="callback"></param>
        /// <param name="layerIndex"></param>
        public static void RemoveClipStartCallback(this Animator animator, string clipName, Action callback, int layerIndex=0)
        {
            animator.RemoveClipCallback(clipName, 0.0f, callback,layerIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="clipName"></param>
        /// <param name="callback"></param>
        /// <param name="layerIndex"></param>
        public static void RemoveClipEndCallback(this Animator animator, string clipName, Action callback, int layerIndex=0)
        {
            var clip = animator.GetAnimationClip(layerIndex, clipName);
            if (clip == null)
            {
                Debug.LogWarning(string.Concat("Failed to get animation clip for Animator component:", clipName));
                return;
            }

            clip.UnbindCallback(animator.gameObject, clip.length, callback);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="clipName"></param>
        /// <param name="atPosition"></param>
        /// <param name="callback"></param>
        /// <param name="layerIndex"></param>
        public static void RemoveClipCallback(this Animator animator,string clipName, float atPosition, Action callback, int layerIndex=0)
        {
            var clip = animator.GetAnimationClip(layerIndex, clipName);
            if (clip == null)
            {
                Debug.LogWarning(string.Concat("Failed to get animation clip for Animator component:", clipName));
                return;
            }

            clip.UnbindCallback(animator.gameObject, atPosition, callback);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="layerIndex"></param>
        /// <param name="clipName"></param>
        /// <returns></returns>
        public static AnimationClip GetAnimationClip(this Animator animator, int layerIndex, string clipName)
        {
          //  var clipsInfo = animator.GetCurrentAnimatorClipInfo(layerIndex);
            RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
            if (runtimeAnimatorController==null)
            {
                Debug.LogError(string.Concat("Failed to get runtimeAnimatorController :", clipName));
                return null;
            }
            AnimationClip[] animationClip = runtimeAnimatorController.animationClips;
            var index = Array.FindIndex(animationClip, x => x.name.ToLower() == clipName.ToLower());
            if (index == -1)
            {
                Debug.LogWarningFormat("Clip with name {0} not found in layer with index {1}", clipName, layerIndex);
                return null;
            }
            var clipInfo = animationClip[index];
            return clipInfo;
        }
        public static bool IsPlayOverByClipName(this Animator animator, int layerIndex = 0)
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(layerIndex);
            if (info.normalizedTime >= 1f)
            {
                return true;
            }
            return false;
        }

    }
}
