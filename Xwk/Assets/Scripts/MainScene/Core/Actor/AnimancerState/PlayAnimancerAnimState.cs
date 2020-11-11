using Animancer;
using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    
    public class PlayAnimancerAnimState : AnimAncerAimState
    {
        private AnimationClip _AnimationClip;
        public AnimancerState _CurrentAnimancerState;
        public PlayAnimancerAnimState(IActorState actorState,  AnimancerComponent animancerComponent, string infoData)
        {
            LoadAnimtionClip(infoData);
            m_AnimancerComponent = animancerComponent;
            m_ActorState = actorState;
        }
        public void LoadAnimtionClip(string path)
        {
            _AnimationClip = C_Singleton<GameResMgr>.GetInstance().LoadResource<AnimationClip>(string.Concat(path, ".FBX")) as AnimationClip;
            if (_AnimationClip == null)
            {
                C_DebugHelper.LogError(string.Concat("class PlayAnimancerAnimState,line 21:clip not exited : ", path));
            }
        }
        public override void Handle()
        {
            //播放动画，并且等待结束作为返回
            if (_AnimationClip == null)
            {
                Stop();
                return;
            }
            _CurrentAnimancerState = m_AnimancerComponent.CrossFade(_AnimationClip);
            _CurrentAnimancerState.OnEnd = Player;
        }
        void Player()
        {
            ClearAnim();

            OnFinish();
        }
        public override void Stop()
        {
            ClearAnim();
            OnCompelete = null;
        }
        public  string CurrentAnimationName
        {
            get {
                if (_AnimationClip != null)
                {
                    return _AnimationClip.name;
                }
                return string.Empty;
            }
        }
        public void CrossFade(string clipName)
        {
            if (_CurrentAnimancerState != null)
            {
                _CurrentAnimancerState.OnEnd = null;
                _CurrentAnimancerState = null;
            }

            LoadAnimtionClip(clipName);
            Handle();
        }
        private void ClearAnim()
        {
            if (_CurrentAnimancerState != null)
            {
                _CurrentAnimancerState.OnEnd = null;
                _CurrentAnimancerState = null;

            }
            if (m_AnimancerComponent!=null)
            {
                m_AnimancerComponent.DisposeNoPlayingClip();
            }
            //if (_CurrentAnimancerState != null)
            //{
            //    _CurrentAnimancerState.Stop();
            //    _CurrentAnimancerState.Dispose();
            //    _CurrentAnimancerState = null;
            //}
        }
    }

}

