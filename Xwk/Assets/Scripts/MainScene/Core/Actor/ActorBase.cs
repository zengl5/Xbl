using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class ActorBase : MonoBehaviour, IActor
    {
        private Camera _Camera;
        public Camera m_Camera { get {
                return getCamera();
            }
            set {
                setCamera(value);
            }
        }
        protected virtual Camera getCamera()
        {
            return _Camera;
        }
        protected virtual void setCamera(Camera camera)
        {
            _Camera = camera;
        }
        public void EnterNextState(ActorStateEnum actorStateEnum)
        {
             
        }

        public virtual Transform getActor()
        {
            return null;
        }

        public virtual List<QuestionConfigData> getActorAnimancerInfo()
        {
            return null;
        }

        public virtual ActorAnimancerResConfig getActorResConfig(string stateName)
        {
            return null;
        }

        public virtual Animator getAnimator()
        {
            return null;
        }

        public virtual ActorStateInfo getInfo(string stateName)
        {
            //throw new NotImplementedException();
            return null; 
        }

        public virtual void PlayAudio(string audioType, string audio, Action callback, bool loop)
        {
            //throw new NotImplementedException();
        }

        public virtual void Stop()
        {
          //  throw new NotImplementedException();
        }
    }

}

