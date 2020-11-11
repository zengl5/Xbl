using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace YB.XWK.MainScene
{
    public class CloudTouchState : ActorState
    {
        private bool stop = false;
        private float currentTime = 0f;
        private Camera _Camera;
        public CloudTouchState(IActor actorAimMgr) : base(null)
        {
            currentTime = 0f;
            HasInteractiveState = true;
            this.ActorStateName = "touchstate";
            OnInit(actorAimMgr, ActorStateName);
             _Camera = m_ActorMgr.m_Camera;
            AudioManager.Instance.PlayEffectSound("public/sound_effect/public_xwkyx_041.ogg",false);
            Play(this);
            DoMove();
        }
        public void DoMove()
        {
            Vector3 target = Vector3.zero;
            float directionX = Random.Range(-5, -2);
            float MaxX = 15f;
            float MinX = -18f;
            float currentX = 0f;
            float currentY = 0f;
            if (Random.Range(0, 2) == 1)
            {
                directionX = Random.Range(2, 5);
            }
            float directionY = Random.Range(-5, -2);
            if (Random.Range(0, 2) == 1)
            {
                directionY = Random.Range(2, 5);
            }
            currentX = Actor.position.x + directionX / 2;
            float maxY = 1.97f/* * Screen.height / 1080f*/;
            float minY = 0.67f/* * Screen.height / 1080f*/;
            if (currentX < _Camera.transform.position.x - Screen.width / 2)
            {
                directionX = Mathf.Abs(directionX);
                currentX = Actor.position.x + directionX / 2;
            }
            else if (Actor.position.x > _Camera.transform.position.x + Screen.width / 2)
            {
                directionX = -1f * Mathf.Abs(directionX);
                currentX = Actor.position.x + directionX / 2;
            }
            if (currentX < MinX)
            {
                currentX = MinX;
            }
            if (currentX > MaxX)
            {
                currentX = MaxX;
            }
            currentY = Actor.position.y + directionY;
            if (currentY > maxY)
            {
                currentY = maxY;
                
            }
            else if (currentY < minY)
            {
                currentY = minY;
            }
            //if (!updatePos)
            //{
            //    target = new Vector3(Actor.position.x + directionX / 2, Actor.position.y + directionY, Actor.position.z);
            //}
            target = new Vector3(currentX, currentY, Actor.position.z);
            float time = Vector3.Distance(target, Actor.position) / 8f;
            Actor.transform.DOMove(target, time).OnComplete(MoveOver);

        }
       
        public  void MoveOver()
        {
            //if (stop)
            //{
            //    return;
            //}
            //DoMove();
        }
        public override void OnUpdate()
        {
            if (stop)
            {
                return;
            }
            currentTime += Time.deltaTime;
            if (currentTime > 3f)
            {
                Stop();
               // m_ActorMgr.EnterIdleState();
                m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_WALKAROUND);
            }
        }
        public override void RequestNextState()
        {
             //动画都播放结束

        }
        public override void Stop()
        {
            AudioManager.Instance.StopAllEffect();
            Actor.transform.DOKill();
            base.Stop();
            currentTime = 0f;
            stop = true;
        }
    }


}
