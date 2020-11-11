using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.YM.Game;
using DG.Tweening;
namespace YB.XWK.MainScene
{
    public class RpWalkAroundState : MonsterBaseState
    {
        private float _OriginX;
        private int Direction;//1表示向左边，2表示向右边走
        private bool _Flying;
        public RpWalkAroundState(RoleMgrBase roleMgr) : base(roleMgr)
        {
            _OriginX = Actor.transform.position.x;
            m_AllowClick = true;
            _Flying = false;
        }
        protected override void OnInit()
        {
            base.OnInit();
            Direction = 1;//默认从做到右边移动
            DoWalkAround();

        }
        //从左边道右边
        void SetLeftFlyCollider()
        {
            CapsuleCollider capsuleCollider =  Actor.GetComponent<CapsuleCollider>();
            capsuleCollider.center = new Vector3(65.9f,9.4f,0f);
            capsuleCollider.radius = 56.5f;
            capsuleCollider.height = 150f;
            capsuleCollider.direction = 0;
        }
        //从右边道左边
        void SetRightFlyCollider()
        {
            CapsuleCollider capsuleCollider = Actor.GetComponent<CapsuleCollider>();
            capsuleCollider.center = new Vector3(66.2f, 1.48f, 0f);
            capsuleCollider.radius = 45.69f;
            capsuleCollider.height = 124.5f;
            capsuleCollider.direction = 0;
        }
        void SetStandCollider()
        {
            CapsuleCollider capsuleCollider = Actor.GetComponent<CapsuleCollider>();
            capsuleCollider.center = new Vector3(0f, 67.4f, 0f);
            capsuleCollider.radius = 45.69f;
            capsuleCollider.height = 124.5f;
            capsuleCollider.direction = 1;
        }
        void DoWalkAround()
        {
            PlayNextAim();
            Vector3 pos = Actor.transform.position;
                Quaternion targetQuaternion;
            if (_Flying)
            {
                Actor.transform.position = new Vector3(pos.x, 0.98f, pos.z);
                if (Direction == 1)
                {
                    SetRightFlyCollider();
                     targetQuaternion = Quaternion.Euler(0,0,0); ; 
                }
                else
                {
                    SetLeftFlyCollider();
                    targetQuaternion = Quaternion.Euler(0, 0, 180);
                }
            }
            else
            {
                SetStandCollider();
                Actor.transform.position = new Vector3(pos.x, 0, pos.z);
                if (Direction == 1)
                {
                     targetQuaternion = Quaternion.Euler(new Vector3(0, 90, 0f));
                }
                else
                {
                    targetQuaternion = Quaternion.Euler(new Vector3(0, -90f, 0));
                }
            }
            if (Direction == 1)//向左边行走
            {
                Actor.transform.DORotateQuaternion(targetQuaternion, 0.1f).OnComplete(() =>
                {
                    Actor.transform.DOMoveX(14f, 40 / 60f).SetSpeedBased().OnComplete(() =>
                     {
                         Direction = 2;
                         DoWalkAround();
                     });
                });
            }
            else
            {
                Actor.transform.DORotateQuaternion(targetQuaternion, 0.1f).OnComplete(() =>
                {
                    Actor.transform.DOMoveX(-17.05f,40/ 60f).SetSpeedBased().OnComplete(() =>
                    {
                        Direction = 1;
                        DoWalkAround();
                    });
                });
            }
        }
        public void PlayNextAim()
        {
            if (Random.Range(0, 2) == 1)
            {
                _Flying = true;
                PlayAnim("public/anim/jl_00015/jl_00015_fly01#anim", null, true);
            }
            else
            {
                _Flying = false;
                PlayAnim("public/anim/jl_00015/jl_00015_walk01#anim", null, true);
            }
        }
        public override void OnStop()
        { 
            base.OnStop();
            Actor.transform.DOKill();
        }
    }
}
