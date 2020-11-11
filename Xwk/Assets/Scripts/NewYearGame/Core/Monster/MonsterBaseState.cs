using Animancer;
using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.YM.Game
{
    //1、公共点击管理普通点击
    //2、确定是否能变身
    public class MonsterBaseState
    {
        public GameObject Actor;
        public RoleMgrBase m_RoleMgr;
        public AnimancerComponent m_AnimancerComponent;
        public AnimancerState _CurrentAnimancerState;
        public bool m_AllowClick = true;
        public bool m_PauseFlag = false;
        public AnimationClip _AnimationClip;
        public int m_currentLevel;
        protected virtual void OnInit()
        {
            if (m_RoleMgr == null)
            {
                C_DebugHelper.LogError("m_RoleMgr is nulll");
                return;
            }
            Actor = m_RoleMgr.getActor();
            if (Actor==null)
            {
                return;
            }
            m_AnimancerComponent = Actor.GetAddComponent<AnimancerComponent>();
            Animator animator = Actor.GetAddComponent<Animator>();
            animator.runtimeAnimatorController = null;
            m_AnimancerComponent.Animator = animator;
            m_AnimancerComponent.enabled = true;
        }
        public MonsterBaseState(RoleMgrBase rolemgr)
        {
            m_RoleMgr = rolemgr;
            OnInit();
        }
        public MonsterBaseState()
        {
            OnInit();
        }

        public virtual void OnEnter(int progress)
        {

        }
        public virtual void OnPause()
        {
            m_PauseFlag = true;
        }
        public  virtual void OnEnter()
        {

        }
        public virtual void OnStop()
        {
            ClearAnim();
        }
        public virtual void OnUpdate()
        {

        }
        public virtual void OnExited()
        {

        }
        public virtual void TouchEvent(GameObject obj,Camera camera,Vector3 touchPos)
        {

        }
        public virtual void TouchEvent(GameObject obj)
        {

        }
        public virtual void ClearAnim()
        {
            if (_CurrentAnimancerState != null)
            {
                _CurrentAnimancerState.OnEnd = null;
                _CurrentAnimancerState = null;

            }
            if (m_AnimancerComponent != null)
            {
                m_AnimancerComponent.DisposeNoPlayingClip();
            }
        }
        public virtual void CleanState()
        {
            OnStop();
        }
        public virtual void PlayAnim(string path, System.Action callback, bool complete = false)
        {
           
            _AnimationClip = GameResMgr.Instance.LoadResource<AnimationClip>(path);
            if (m_AnimancerComponent == null || m_AnimancerComponent ==null)
            {
                if (complete && callback != null)
                {
                    callback();
                    callback = null;

                }
                return;
            }
            _CurrentAnimancerState = m_AnimancerComponent.CrossFade(_AnimationClip);
            _CurrentAnimancerState.OnEnd -= callback;
            _CurrentAnimancerState.OnEnd += callback;
        }
       public virtual void  PlayAnim(AnimationClip animationClip, System.Action callback, bool complete = false)
        {
            _AnimationClip = animationClip;
            if (m_AnimancerComponent == null || m_AnimancerComponent == null)
            {
                if (complete && callback != null)
                {
                    callback();
                }
                return;
            }
            _CurrentAnimancerState = m_AnimancerComponent.CrossFadeFromStart(_AnimationClip);
            _CurrentAnimancerState.OnEnd -= callback;
            _CurrentAnimancerState.OnEnd += callback;
        }
    }

}
