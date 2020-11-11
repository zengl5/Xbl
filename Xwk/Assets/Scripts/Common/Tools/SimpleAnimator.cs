using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static  class SimpleAnimator
{
    private static Dictionary<string, List<string>> _Dic = new Dictionary<string, List<string>>();
    public static void AddAnimEvent(Animator _Animator, string clipName, string fun, bool share, float timeWeight = -1f)
    {
        AnimationClip[] clips = _Animator.runtimeAnimatorController.animationClips;
        List<AnimationClip> _ListClips = new List<AnimationClip>();
        foreach (var item in clips)
        {
            _ListClips.Add(item);
        }
        AnimationClip clip = _ListClips.Find(p => p.name.StartsWith(clipName));
        if (clip != null)
        {
            if (!_Dic.ContainsKey(clipName))
            {
                List<string> list = new List<string>();
                list.Add(fun);
                _Dic.Add(clipName, list);

            }
            else
            {
                if (_Dic[clipName].Find(p => p.Equals(fun)) != null)
                {
                    Debug.Log("this Event has Added before:"+fun);
                    return;
                }
                else
                {
                    _Dic[clipName].Add(fun);
                }
            }
            AnimationEvent animEnd = new AnimationEvent
            {
                functionName = fun//响应事件
            };
            if (timeWeight < 0)
            {
                animEnd.time = clip.length; //设定对应事件触发时间点为结尾
            }
            else
            {
                animEnd.time = clip.length * timeWeight;
            }
            clip.AddEvent(animEnd);//把事件添加到时间轴上 
            _Animator.Rebind();//重新绑定动画器属性。           
        }
    }
        /// <summary>
        /// 给动画片段添加事件    
        /// </summary>
        /// <param name="clipName">动画片段名称</param>
        public static void AddAnimEvent(Animator _Animator, string clipName, string fun, float timeWeight = -1f)
    {
        AnimationClip[] clips = _Animator.runtimeAnimatorController.animationClips;
        List<AnimationClip> _ListClips = new List<AnimationClip>();
        foreach (var item in clips)
        {
            _ListClips.Add(item);
        }
        AnimationClip clip = _ListClips.Find(p => p.name.StartsWith(clipName));
        if (clip != null)
        {
            AnimationEvent animEnd = new AnimationEvent
            {
                functionName = fun//响应事件
            };
            if (timeWeight < 0)
            {
                animEnd.time = clip.length; //设定对应事件触发时间点为结尾
            }
            else
            {
                animEnd.time = clip.length * timeWeight;
            }
            clip.AddEvent(animEnd);//把事件添加到时间轴上 
            _Animator.Rebind();//重新绑定动画器属性。
        }
    }
    /// <summary>         注销对应事件          /// </summary>  
    public static void RemoveAnimEvent(Animator _Animator, string clipName)
    {
        AnimationClip[] clips = _Animator.runtimeAnimatorController.animationClips;
        List<AnimationClip> _ListClips = new List<AnimationClip>();
        foreach (var item in clips)
        { 
            _ListClips.Add(item);
        }
        AnimationClip clip = _ListClips.Find(p => p.name.StartsWith(clipName));
        if (clip != null)
        {
            clip.events = default(AnimationEvent[]);
            if (_Dic.ContainsKey(clipName))
            {
                _Dic.Remove(clipName);
            }
        }
        _Animator.Rebind();//重新绑定动画器属性。
    }
    public static void AllClear()
    {
        _Dic.Clear();
    }
    public static void PlayAnim(Animator animator ,string triggerName)
    {
        if (string.IsNullOrEmpty(triggerName))
        {
            Debug.LogError("Trigger Name Dons't Exits!");
            return;
        }
        animator.SetTrigger(triggerName);
    }
}
