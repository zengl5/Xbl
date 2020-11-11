using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class AskAnimState : StepAnimAndAudioState
    {
        public AskAnimState(IActorState actorState, Animator animator, ActorStateInfo stateInfo) : base(actorState, animator, stateInfo)
        {
            m_ActorState = actorState;
            m_ActorStateInfo = stateInfo;
            m_Animator = animator;
            _StateIndex = 0;
        }
    }
}

