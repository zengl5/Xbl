using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.YM.Game {
    public class YearXwkMgr : RoleMgrBase
    {
        private GameObject _XwkRole;
        public YearXwkMgr(Camera gameCamra) : base(gameCamra)
        {
            _ActorTag = "Actor";
            OnInit();
        }

        public override void OnInit()
        {

            InitRole();
//#if !UNITY_EDITOR
//           _RoleState = new YearXwkIdleState();
//#else
//            _RoleState = new YearXwkIdleState(this);
//#endif
            _RoleState = new YearXwkHelloState(this);
            _RoleState.OnEnter();
        }
        public override GameObject getActor()
        {
            if (_XwkRole == null)
            {
                InitRole();
            }
            return _XwkRole;
        }
        protected override void InitRole()
        {
            if (_XwkRole == null)
            {
                _XwkRole = GameObject.FindGameObjectWithTag(_ActorTag);
                if (_XwkRole==null)
                {
                    _XwkRole = GameResMgr.Instance.LoadResource<GameObject>("public/mesh/wukong/public_model_wukong#mesh.prefab", true);
                    _XwkRole.SetActive(true);
                    _XwkRole.transform.localPosition = new Vector3(-4.61f, 0f, 8.13f);
                    _XwkRole.transform.localRotation = Quaternion.Euler(new Vector3(0f, 160f, 0f));
                    _XwkRole.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                    _XwkRole.gameObject.tag = _ActorTag;

                    CapsuleCollider capsuleCollider = _XwkRole.GetAddComponent<CapsuleCollider>();
                    capsuleCollider.center = new Vector3(0f, 72f, 0f);
                    capsuleCollider.radius = 32.2f;
                    capsuleCollider.height = 144f;
                    capsuleCollider.direction = 1;

                    Rigidbody rigidbody = _XwkRole.GetAddComponent<Rigidbody>();
                    rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                    rigidbody.mass = 100f;
                }
            }
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        /// <summary>
        /// 进入技能攻击
        /// </summary>
        /// <param name="type"></param>
        public override void EnterUnMatchedState(YMGameWoundedType type)
        {
            if (_RoleState != null && typeof(YearSkillState) != _RoleState.GetType())
            {
                CleanState();
                _RoleState = new YearSkillState(this, type);
                _RoleState.OnEnter();
            }
        }
     
        public override void EnterIdle()
        {
            CleanState();
            _RoleState = new YearXwkIdleState(this);
            _RoleState.OnEnter();
        }
        public override void Stop()
        {
            CleanState();
            if (_XwkRole != null)
            {
                GameObject.DestroyObject(_XwkRole);
                _XwkRole = null;
            }
        }

        public override void EnterSuccessState()
        {
            if (_RoleState != null && typeof(YearXwkSuccessState) != _RoleState.GetType())
            {
                CleanState();
                _RoleState = new YearXwkSuccessState(this);
                _RoleState.OnEnter();
            }
        }
        public override void EnterFailState()
        {
            if (_RoleState != null && typeof(YearXwkFailState) != _RoleState.GetType())
            {
                CleanState();
                _RoleState = new YearXwkFailState(this);
                _RoleState.OnEnter();
            }
        }
    }

}

