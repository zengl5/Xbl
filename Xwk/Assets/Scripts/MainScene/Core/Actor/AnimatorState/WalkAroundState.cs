using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class WalkAroundState : ActorAnimancerState
    {
        private Sequence _MySequence;
        private int _MoveTimes = 0;
        private Quaternion _OriginQuaternion =  new Quaternion();
        public WalkAroundState(AnimState state) : base(state)
        {

        }
        private void Init(IActor actorAimMgr)
        {
            this.ActorStateName = "walkaroundstate";
            OnInit(actorAimMgr);
            this.HasInteractiveState = false;
            _OriginQuaternion = Actor.rotation;

            Play(this);
        }
        /// <summary>
        /// 不跟随镜头转向
        /// </summary>
        public override void OnUpdate()
        {
            
        }
        public WalkAroundState(IActor actorAimMgr) : base(null)
        {
            Init(actorAimMgr);
        }

        public override void Play(IActorState target)
        {
            AudioManager.Instance.StopPlayerSound();
            AudioManager.Instance.PlayerSound("public/sound_effect/public_xwkyx_007.ogg", true);
            PlayMoveAnim();
            DoMove();
        }
        void PlayMoveAnim()
        {
            PlayAnim("public/anim/wukong/wukong_xz_walk01#anim");
        }
        void PlayStandAnim()
        {
            PlayAnim("public/anim/wukong/wukong_ty_stand01#anim");
        }
        void DoMove()
        { 
            _MoveTimes++;
            if (_MoveTimes > 1)
            {
                RequestNextState();
                return;
            }
            PlayMoveAnim();
            //判断当前距离左边屏幕和右边屏幕最大的距离，然后取一个最大的的间距移动一半
            Camera camera = m_ActorMgr.m_Camera;
            Vector3 cameraPos = camera.transform.position;
            float deletaLeft = Actor.position.x - cameraPos.x;
            Vector3 targetPos = Vector3.zero;
            float moveX=0f;
            if (deletaLeft <= 0)//走右边
            {
                moveX = getCameraX() / (Random.Range(2, 3));
                targetPos = new Vector3(Actor.position.x +moveX, Actor.position.y, Actor.position.z);
            }
            else if (deletaLeft > 0)//走左边
            {
                moveX = getCameraX() / (Random.Range(2, 4));
                targetPos = new Vector3(Actor.position.x-moveX, Actor.position.y, Actor.position.z);
            }
            float time = moveX*2f;
            //面向移动的方向转身，移动
            Vector3 forwardDir = targetPos - Actor.position;
            Quaternion lookAtRot = Quaternion.LookRotation(targetPos - Actor.position);
            //移动角色到目标点
            Actor.DOKill();
            if (_MySequence != null) _MySequence.Kill();
             _MySequence = DOTween.Sequence();
            _MySequence.Append(Actor.DORotate(Quaternion.LookRotation(targetPos - Actor.position).eulerAngles, 0.5f))
                .Append(Actor.DOMove(targetPos, time))
                .Append(Actor.DOLookAt(new Vector3(m_ActorMgr.m_Camera.transform.position.x, Actor.position.y, m_ActorMgr.m_Camera.transform.position.z), 0.5f))
                .AppendCallback(()=>{
                    PlayStandAnim();
                    AudioManager.Instance.StopPlayerSound();
                })
                .AppendInterval(2)
                .OnComplete(
                () => {
                    DoMove();
                }
                );

        }
        float getCameraX()
        {
            Camera camera = m_ActorMgr.m_Camera;

            float halfFOV = (camera.fieldOfView * 0.5f) * Mathf.Deg2Rad;

            float height = Vector3.Distance(m_ActorMgr.getActor().position, camera.transform.position) * Mathf.Tan(halfFOV);

            return height * camera.aspect;
        }
        public override void RequestNextState()
        {
            AudioManager.Instance.StopPlayerSound();
            Actor.DOKill();
            m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_IDLE);
        }

        public override void Stop()
        {

            if (_MySequence != null) _MySequence.Kill();

            base.Stop();
            AudioManager.Instance.StopPlayerSound();
            Actor.DOKill();
            Actor.LookAt(new Vector3(m_ActorMgr.m_Camera.transform.position.x, Actor.position.y, m_ActorMgr.m_Camera.transform.position.z));
        }
    }
}


