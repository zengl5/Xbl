using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.AnimCallbacks;
public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) { }

    public AnimationClip this[string name]
    {
        get { return this.Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
            {
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
            else
            {
                AnimationClip clip = new AnimationClip();
                clip.name = name;
                this.Add(new KeyValuePair<AnimationClip, AnimationClip>(clip, value));
            }
        }
    }
}
public sealed class UpdateEvent{

    private string _UpdateClipName;
    private bool _UpdateFlag;
    private Action _UpdateCallback;
    private int _LayerIndex;
    public int LayerIndex
    {
        get
        {
            return _LayerIndex;
        }
        set
        {
            _LayerIndex = value;
        }
    }
    public string UpdateClipName
    {
        get
        {
            return _UpdateClipName;
        }
        set
        {
            _UpdateClipName = value;
        }
    }
    public bool UpdateFlag
    {
        get
        {
            return _UpdateFlag;
        }
        set
        {
            _UpdateFlag = value;
        }
    }
    public Action UpdateCallback
    {
        get
        {
            return _UpdateCallback;
        }
        set
        {
            _UpdateCallback = value;
        }
    }
    public UpdateEvent()
    {
        Init();
    }
    void Init()
    {
        _UpdateClipName = "";
        _UpdateFlag = false;
        _UpdateCallback = null;
        _LayerIndex = 0;
    }
    public void Reset()
    {
        Init();
    }
}
public class AnimatorManager :MonoBehaviour {
    public static AnimatorManager Instance
    {
        get
        {
            if (_Ins == null)
            {
                GameObject obj = new GameObject("AnimatorManager");
                _Ins = obj.AddComponent<AnimatorManager>();
                DontDestroyOnLoad(_Ins.gameObject);
            }
            return _Ins;
        }
         
    }
    private static AnimatorManager _Ins  = null;

    public static UpdateEvent _UpdateEvent = new UpdateEvent();
     
    public void AddUpdateEvent(Animator animator, string clipName, Action Callback = null, int layerIndex = 0)
    {
        if (string.IsNullOrEmpty(clipName))
        {
            Debug.Log("AnimatorBase AddAnimatorEvent  parameter is error... ");
            return;
        }
        _UpdateEvent.UpdateCallback = Callback;
        _UpdateEvent.UpdateClipName = clipName;
        _UpdateEvent.UpdateFlag = true;
        _UpdateEvent.LayerIndex = layerIndex;
        if (!animator.AddClipEndCallback(clipName, () => { AnimatorUpdateOver(); }, layerIndex))
        {
            AnimatorUpdateOver();
        }
    }
    public bool IsPlayClipName(Animator animator, string clipName, int layerIndex = 0)
    {
        return animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(clipName);
    }
    public bool IsPlayOverByClipName(Animator animator, string clipName, int layerIndex = 0)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(layerIndex);
        if ( info.normalizedTime >= 1f)
        {
            return true;
        }
        return false;
    }
   /* public void AddAnimatorEvent(Animator animator, string clipName, string function, float time = -1)
    {
        if (string.IsNullOrEmpty(clipName) || string.IsNullOrEmpty(function))
        {
            Debug.Log("AnimatorBase AddAnimatorEvent  parameter is error... ");
            return;
        }

        RuntimeAnimatorController  runtimeAnimatorController = animator.runtimeAnimatorController;
        AnimationClip[] animationClip = runtimeAnimatorController.animationClips;
        for (int i = 0; i < animationClip.Length; i++)
        {
            if (animationClip[i].name.Equals(clipName))
            {
                AnimationEvent animatorEnd = new AnimationEvent();
                animatorEnd.functionName = function;
                animatorEnd.time = time;
                if (time == -1)
                {
                    animatorEnd.time = animationClip[i].length;
                }
                animationClip[i].AddEvent(animatorEnd);
            }
        }
        animator.Rebind();  
    }*/
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateEvent();
	}
    public void UpdateEvent()
    {
        if (_UpdateEvent.UpdateFlag && _UpdateEvent.UpdateCallback != null)
        {
            _UpdateEvent.UpdateCallback();
        }
    }
    void AnimatorUpdateOver()
    {
        _UpdateEvent.Reset();
    }
   /* private void OnAnimatorMove()
    {
        Debug.Log("OnAnimatorMove...");
    }*/

    public static RuntimeAnimatorController GetRunningAnimatorController(Animator aniamtor)
    {
        return aniamtor.runtimeAnimatorController;
    }
    public void SetAnimtorTrigger(Animator animator, string name)
    {
        if (animator == null)
        {
            Debug.LogError("_Animator is null");
            return;
        }
      //  Debug.Log("clipname = " + name + "-aniamtor" + animator.name);
        animator.SetTrigger(name);
    }
    public void SetAnimtorFloat(Animator animator, string name, float value)
    {
        if (animator == null)
        {
           // Debug.LogError("_Animator is null");
            return;
        }
        animator.SetFloat(name, value);
    }
    public void SetAnimtorBool(Animator animator, string name, bool value)
    {
        if (animator == null)
        {
         //   Debug.LogError("_Animator is null");
            return;
        }
        animator.SetBool(name, value);
    }
    public void SetAnimtorInt(Animator animator, string name, int value)
    {
        if (animator == null)
        {
        //    Debug.LogError("_Animator is null");
            return;
        }
        animator.SetInteger(name, value);
    }
    public void PlayAnimtor(Animator animator, string stateName, float transitionDuration = 0)
    {
        if (animator == null)
        {
         //   Debug.LogError("_Animator is null");
            return;
        }
        if (transitionDuration > 0f)
        {
            animator.CrossFade(stateName, transitionDuration);
        }
        else
        {
            animator.Play(stateName);
        }
    }
    public void PlayAnimtor(Animator animator, string stateName, int layer)
    {
        if (animator == null)
        {
         //   Debug.LogError("_Animator is null");
            return;
        }
        animator.Play(stateName, layer);
    }
    public void PlayAnimtor(Animator animator, string stateName, int layer, float normalizedTime)
    {
        if (animator == null)
        {
          //  Debug.LogError("_Animator is null");
            return;
        }
        animator.Play(stateName, layer, normalizedTime);
    }
    public void UpdateAnimatorClipInRunTimeViaTrriger(Animator animator, string pAnimResourceName, string originClipName, string trigger)
    {
        UpdateAnimatorClipInRunTime(animator, pAnimResourceName, originClipName);
     //   animator.SetTrigger(trigger);
        SetAnimtorTrigger(animator, trigger);
        Resources.UnloadUnusedAssets();
    }
    public void UpdateAnimatorClipInRunTimeViaInt(Animator animator, string pAnimResourceName, string originClipName, string integter, int vaule)
    {
        UpdateAnimatorClipInRunTime(animator, pAnimResourceName, originClipName);
      //  animator.SetInteger(integter, vaule);
        SetAnimtorInt(animator,integter, vaule);
        Resources.UnloadUnusedAssets();
    }
    public void UpdateAnimatorClipInRunTimeViaBool(Animator animator, string pAnimResourceName, string originClipName, string boolName, bool vaule)
    {
        UpdateAnimatorClipInRunTime(animator, pAnimResourceName, originClipName);
      //  animator.SetBool(boolName, vaule);
        SetAnimtorBool(animator, boolName, vaule);
        Resources.UnloadUnusedAssets();
    }
    public void UpdateAnimatorClipInRunTimeViaFloat(Animator animator, string pAnimResourceName, string originClipName, string floatName, float vaule)
    {
        UpdateAnimatorClipInRunTime(animator, pAnimResourceName, originClipName);
        //animator.SetFloat(floatName, vaule);
        SetAnimtorFloat(animator, floatName, vaule);
        Resources.UnloadUnusedAssets();
    }
    public void UpdateAnimatorClipInRunTime(Animator animator, string pAnimResourceName, string originClipName)
    {
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        AnimationClipOverrides clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(clipOverrides);
        var mAnimationClips = Resources.LoadAll<AnimationClip>(pAnimResourceName);
        if (mAnimationClips.Length <= 0) {
          //  Debug.LogError(pAnimResourceName+" is no exited..");
            return;
        }
        clipOverrides[originClipName] = mAnimationClips[0];
        animatorOverrideController.ApplyOverrides(clipOverrides);
        animator.runtimeAnimatorController = null;
        animator.runtimeAnimatorController = animatorOverrideController;
    }
    public void ReplaceClip(Animator animator, string clipName, AnimationClip overrideClip)
    {
        AnimatorOverrideController runtimeAnimatorController = animator.runtimeAnimatorController as AnimatorOverrideController;
        if (runtimeAnimatorController == null)
        {
            runtimeAnimatorController = new AnimatorOverrideController
            {
                runtimeAnimatorController = animator.runtimeAnimatorController
            };
        }
        runtimeAnimatorController[clipName] = overrideClip;
        if (!object.ReferenceEquals(animator.runtimeAnimatorController, runtimeAnimatorController))
        {
            animator.runtimeAnimatorController = runtimeAnimatorController;
        }
    }

    public void ReplaceClip(Animator animator, AnimationClip originalClip, AnimationClip overrideClip)
    {
        AnimatorOverrideController runtimeAnimatorController = animator.runtimeAnimatorController as AnimatorOverrideController;
        if (runtimeAnimatorController == null)
        {
            runtimeAnimatorController = new AnimatorOverrideController
            {
                runtimeAnimatorController = animator.runtimeAnimatorController
            };
        }
        runtimeAnimatorController[originalClip] = overrideClip;
        if (!object.ReferenceEquals(animator.runtimeAnimatorController, runtimeAnimatorController))
        {
            animator.runtimeAnimatorController = runtimeAnimatorController;
        }
    }


}
