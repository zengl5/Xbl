using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.YM.Game
{
    public class NewYearCameraMgr : RoleMgrBase
    {
        private GameObject _CameraObj;
        private Camera _Camera;
        public NewYearCameraMgr(Camera camera) : base(camera)
        {
            _CameraObj = camera.gameObject;
            _Camera = camera;
            OnInit();
        }
        public override void OnInit()
        {

          //  InitRole();
            //#if !UNITY_EDITOR

            //            _RoleState = new YearXwkIdleState();
            //#else
            //            _RoleState = new YearXwkIdleState(this);
            //#endif
            _RoleState = new NewYearCameraHelloState(this);
            _RoleState.OnEnter();
        }
        public override GameObject getActor()
        {
            return _CameraObj;
        }
        public override void EnterSuccessState()
        {
            if (_RoleState != null && typeof(NewYearSuccessState) != _RoleState.GetType())
            {
                CleanState();
                _RoleState = new NewYearSuccessState(this);
                _RoleState.OnEnter();
            }
        }
        public override void EnterFailState()
        {
            if (_RoleState != null && typeof(NewYearFailState) != _RoleState.GetType())
            {
                CleanState();
                _RoleState = new NewYearFailState(this);
                _RoleState.OnEnter();
            }
        }
        public override void EnterIdle()
        {
            if (_CameraObj!=null)
            {
                CleanState();

                _CameraObj.transform.position = new Vector3(0,1.84f,16.77f);
                _CameraObj.transform.rotation =Quaternion.Euler( new Vector3(-7.109f,-180f,0f));
            }
        }
    }
}
