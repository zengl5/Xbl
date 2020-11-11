using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace YB.XWK.MainScene {
    public class CloudWalkAroundState : ActorState
    {
        private float speed = 1f;
        private Camera _MainCamera;
        public CloudWalkAroundState(IActor actorAimMgr) : base(null)
        {
            HasInteractiveState = true;
            this.ActorStateName = "walkaroundstate";
            OnInit(actorAimMgr, ActorStateName);
        }
        protected override void OnInit(IActor actorAimMgr, string state)
        {
            base.OnInit(actorAimMgr, state);
            _MainCamera = m_ActorMgr.m_Camera;
            DoMove();
        }

        public override void Play(IActorState target)
        {
            InfoData infoData = null;

            ActorStateInfo info = m_ActorMgr.getInfo(this.ActorStateName);

            if (info == null)
            {
                C_DebugHelper.LogWarning(this.ActorStateName + ": info is null");
                return;
            }
            infoData = info.infodata[Random.Range(0, info.infodata.Count)];
            InitAc(infoData.acname);
            EnterOneState(infoData.anim);
          
        }
        public void DoMove()
        {
            Play(this);
            bool updatePos = false;
            Vector3 target=Vector3.zero;
            float directionX = 2.0f - Random.Range(0, 5);
            float directionY = 2.0f - Random.Range(0, 5);
            float maxY =1.96f * Screen.height/ 1080f;
            float minY = 0.67f * Screen.height / 1080f;
            float currentX = 0f;
            if ( Actor.position.x <_MainCamera.transform.position.x - Screen.width / 2)
            {
                directionX = Mathf.Abs(directionX);
            }
            else if (Actor.position.x> _MainCamera.transform.position.x+ Screen.width / 2)
            {
                directionX = -1f* Mathf.Abs(directionX);
            }
            currentX = Actor.position.x + directionX / 2;
            if (currentX > 15f)
            {
                currentX = 15f;
            }
            if (currentX < -18f)
            {
                currentX = -18f;
            }
            if (Actor.position.y + directionY > maxY)
            {
                target = new Vector3(currentX, maxY, Actor.position.z);
                updatePos = true;
            }
            else if (Actor.position.y + directionY < minY)
            {
                target = new Vector3(currentX, minY, Actor.position.z);
                updatePos = true;
            }
            if (!updatePos)
            {
                target = new Vector3(currentX, Actor.position.y + directionY, Actor.position.z);
            }
            float time = Vector3.Distance(target, Actor.position)/ speed;
            Actor.transform.DOMove(target, time).SetDelay(Random.Range(1,5)).OnComplete(MoveOver);
        }
        public void MoveOver()
        {
            int probility = Random.Range(0, 10);
            if (probility <=8)
            {
                m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_IDLE);
            }
            else
            {
                DoMove();
            }
        }
        public override void Stop()
        {
            Actor.transform.DOKill();
        }
    }

}

