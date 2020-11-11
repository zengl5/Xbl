using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.YM.Game;

namespace YB.XWK.MainScene
{
    public class RpClickState : MonsterBaseState
    {
        /// <summary>
        /// 普通点击
        /// </summary>
        private float touchEventTime = 0;
        private float time = 1;
        private float currentTime = 0;
        public RpClickState(RoleMgrBase roleMgr) : base(roleMgr)
        {
            m_AllowClick = false;
        }
        protected override void OnInit()
        {
            touchEventTime = 0;
            base.OnInit();
            InitAnim();
        }
        void InitAnim()
        {
           // PlayAnim("public/anim/jl_00014/jl_00014_hit01#anim", RequestNextState, true);
        }
        public override void TouchEvent(GameObject obj, Camera camera, Vector3 touchPos)
        {
         
        }
        public void RequestNextState()
        {

        }

        public override void OnStop()
        {
            m_PauseFlag = true;
            base.OnStop();
            ClearAnim();
        }
    }
}

