using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.C_Framework;
/// <summary>
/// 废弃功能
/// </summary>
namespace YB.XWK.MainScene {
    public class AutoMoveCameraStateBase : MoveCameraState {
        protected bool moveOver = false;
        protected Vector3 targetPos;
        protected float speed = 5.0f;
        public AutoMoveCameraStateBase(IActor actorAimMgr) : base(actorAimMgr)
        {
           
        }
        public void OnStart(IActor actorAimMgr)
        {
            Init(actorAimMgr);
            attach = false;
            moveOver = false;

            this.HasInteractiveState = false;
            float time = Vector3.Distance(m_ActorMgr.m_Camera.transform.position, targetPos);
            //移动到百花精灵对象
            m_ActorMgr.m_Camera.transform.DOKill();

            m_ActorMgr.m_Camera.transform.DOMove(targetPos, time / speed).OnComplete(() => {
                Play(this);
                moveOver = true;
            });
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            PlayOver();
        }
        public void PlayOver()
        {
            if (!attach && moveOver)
            {
                moveOver = false;
                m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_IDLE);
            }
        }
        public override void Stop()
        {
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "AutoMoveCameraOver");

            base.Stop();
            m_ActorMgr.m_Camera.DOKill();
        }
       
    }

    public class AutoMoveCameraState : AutoMoveCameraStateBase
    {
        public AutoMoveCameraState(IActor actorAimMgr) : base(actorAimMgr)
        {
            this.ActorStateName = "AutoMoveCameraState";
            targetPos = new Vector3(3.7f, 0.68f, 3.0f);
            OnStart(actorAimMgr);
        }
    }

    public class AutoMoveCameraStateBBHL: AutoMoveCameraStateBase
    {
        public AutoMoveCameraStateBBHL(IActor actorAimMgr) : base(actorAimMgr)
        {
            this.ActorStateName = "AutoMoveCameraState";
            targetPos = new Vector3(0f, 0.68f, 3.0f);
            OnStart(actorAimMgr);
        }
    }
}

