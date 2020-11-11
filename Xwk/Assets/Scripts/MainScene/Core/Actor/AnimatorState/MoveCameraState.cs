using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class MoveCameraState : ActorAnimancerState
    {
        private float MoveSpeed = 2f;
        protected bool attach = false;
        private Animator _Animator;

        private string _WalkSound = "public/sound_effect/public_xwkyx_007.ogg";
        private string _RunSound = "public/sound_effect/public_xwkyx_006.ogg";
        private string _WalkAnim = "public/anim/wukong/wukong_ty_walk01#anim";
        private string _RunAnim = "public/anim/wukong/wukong_ty_run01#anim";
        private string _WalkAnimName = "wukong_ty_walk01#anim";
        private string _RunAnimName = "wukong_ty_run01#anim";
        private string _WalkSoundName = "public_xwkyx_007";
        private string _RunSoundName = "public_xwkyx_006";
        public MoveCameraState(IActor actorAimMgr, AnimState animState) : base(animState)
        {
            Init(actorAimMgr);
            this.state = animState;
        }
        protected void Init(IActor actorAimMgr)
        {
            this.ActorStateName = "movecamerastate";
            this.HasInteractiveState = true;
            OnInit(actorAimMgr);

        }
        public MoveCameraState(IActor actorAimMgr) : base(null)
        {
            Init(actorAimMgr);
            Play(this);
        }
        public override void Play(IActorState target)
        {
           // InitAc("main/animationcontroller/movecamera");
            m_ActorMgr.getActor().DOKill();
            SetActorPose();
            attach = true;
        }
        void SetActorPose()
        {
            float deleta = Mathf.Abs(Actor.position.x - m_ActorMgr.m_Camera.transform.position.x);
            if (deleta  >= 4 )
            {
                MoveSpeed = 2f;
             //   EnterOneState(_RunAnim);
                PlayAnim(_RunAnim);
             //   AudioManager.Instance.StopPlayerSound();
                AudioManager.Instance.PlayerSound(_RunSound, true);
            }
            else
            {
                MoveSpeed = 0.5f;
               // EnterOneState(_WalkAnim);
                PlayAnim(_WalkAnim);
              //  AudioManager.Instance.StopPlayerSound();
                AudioManager.Instance.PlayerSound(_WalkSound, true);
            }
        }
        private void FixAnimationClip()
        {
            //AnimatorStateInfo stateinfo = _Animator.GetCurrentAnimatorStateInfo(0);
            if (MoveSpeed >= 2)
            {
                if (!_RunAnimName.Equals(getAnimationClipName()) )
                {
                    AudioManager.Instance.PlayerSound(_RunSound, true);
                    PlayAnim(_RunAnim);
                }
            }
            else if (MoveSpeed <= 0.5f)
            {
                if ( !_WalkAnimName.Equals(getAnimationClipName()))
                {
                    AudioManager.Instance.PlayerSound(_WalkSound, true);
                    PlayAnim(_WalkAnim);
                }
            }
        }
        public override void OnUpdate()
        {
            if (attach)
            {
                FixAnimationClip();

                Vector3 targetPos = new Vector3(m_ActorMgr.m_Camera.transform.position.x, Actor.position.y, Actor.position.z); 
                Vector3 forwardDir = targetPos - Actor.position;
                Actor.rotation = Quaternion.LookRotation(forwardDir); 
                Actor.Translate(Vector3.forward * Time.deltaTime * MoveSpeed); // 角色向目标点移动
                float distance = Mathf.Abs(Actor.position.x - m_ActorMgr.m_Camera.transform.position.x);
                if (distance < 0.02f)
                {
                    attach = false;
                    Actor.DOKill();
                    Actor.DORotate((m_ActorMgr.m_Camera.transform.position - Actor.position), 0.5f)
                        .OnComplete(() => {
                            attach = false;
                            CommandNextState();
                        });
                }
            }
        }

        public override void CommandNextState()
        {
            ManagerRes();
            m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_IDLE);
        }
             
        public override void RequestNextState()
        {
            //m_ActorMgr.EnterIdleState();
        }
        public override void Stop()
        {
            ManagerRes();
            base.Stop();
            //面向正面
           // Actor.rotation = Quaternion.Euler(Vector3.zero);
        }
        void ManagerRes()
        {
            attach = false;

            if (_AnimAncerAimState!=null)
            {
                _AnimAncerAimState.Stop();
                _AnimAncerAimState = null;
            }
            //AudioManager.Instance.StopAllEffect();
            if (AudioManager.Instance.isPlayingMainSound(_WalkSoundName) || AudioManager.Instance.isPlayingMainSound(_RunSoundName))
            {
                AudioManager.Instance.StopPlayerSound();
            }
            Actor.DOKill();
            Actor.LookAt(new Vector3(m_ActorMgr.m_Camera.transform.position.x, Actor.position.y, m_ActorMgr.m_Camera.transform.position.z));

        }
    }
}

