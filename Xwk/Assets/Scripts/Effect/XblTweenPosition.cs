using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class XblTweenPosition : MonoBehaviour {
    public bool IsLocalMode = true;
    public Vector3 TargetPos;
    public float Timer = 1;
    public bool Loop;
    Tween tw;
    // Use this for initialization
    void Start () {
        if (Loop)
        {
            if(IsLocalMode)
            tw = transform.DOLocalMove(TargetPos, Timer);
            else
                tw = transform.DOMove(TargetPos, Timer);
            tw.SetLoops(1000,LoopType.Yoyo);
            tw.SetEase(Ease.Linear);
        }
        else
        {
            if (IsLocalMode)
                tw = transform.DOLocalMove(TargetPos, Timer);
            else
                tw = transform.DOMove(TargetPos, Timer);
        }           
    }
    // Update is called once per frame
    void Update () {
		
	}
}
