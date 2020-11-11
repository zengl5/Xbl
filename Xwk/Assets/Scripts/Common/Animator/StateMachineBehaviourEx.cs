using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StateMachineBehaviourEx : StateMachineBehaviour
{
    public delegate void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
    public StateEnter OnStateEnterHandler;
    public StateEnter OnStateExitHandler;
    public StateEnter OnStateIKHandler;
    public StateEnter OnStateUpdateHandler;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (OnStateEnterHandler != null)
        {
            OnStateEnterHandler(animator, stateInfo, layerIndex);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        if (OnStateExitHandler != null)
        {
            OnStateExitHandler(animator, stateInfo, layerIndex);
        }
    }
    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateIK(animator, stateInfo, layerIndex);
        if (OnStateIKHandler != null)
        {
            OnStateIKHandler(animator, stateInfo, layerIndex);
        }
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {

    }
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {

    }
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (OnStateUpdateHandler != null)
        {
            OnStateUpdateHandler(animator, stateInfo, layerIndex);
        }
    }
}
