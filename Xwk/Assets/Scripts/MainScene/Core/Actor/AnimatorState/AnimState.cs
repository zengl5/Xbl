using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YB.XWK.MainScene{
    public class AnimStateConstant
    {
        //当前动作播放，同时播放某一个声音
        public  static string k_Anim_Main_Ainm = "main_anim";
        public  static string k_Anim_Main_Audio = "main_audio";
    }
    public abstract class AnimStateBase {
        public IActorState m_ActorState;
        public abstract void Handle();
        public delegate void Compelete();
        public Compelete OnCompelete;
        public Compelete OnCompeleteNext;
        public void OnFinish()
        {
            if (OnCompelete != null)
            {
                OnCompelete();
                OnCompelete = null;
            }
        }
        public void PlayNext()
        {
            if (OnCompeleteNext != null)
            {
                OnCompeleteNext();
                OnCompeleteNext = null;
            }
        }
        public abstract void Stop();

    }

    /// <summary>
    /// 播放主界面角色的动画和语音
    /// </summary>
    public abstract class AnimState: AnimStateBase
    {
        public ActorStateInfo m_ActorStateInfo;

        public InfoData m_InfoData;

        public Animator m_Animator;

        protected AnimatorControllerParameterType m_AnimatorControllerParameterType;

        public virtual void HandldeOver()
        {

        }

        public void PlayAnim(string vaule="")
        {
            switch (m_AnimatorControllerParameterType)
            {
                case AnimatorControllerParameterType.Bool:
                    {
                        m_Animator.SetBool(m_InfoData.anim,  bool.Parse(vaule));
                    }
                    break;
                case AnimatorControllerParameterType.Trigger:
                    {
                        m_Animator.SetTrigger(m_InfoData.anim);
                    }
                    break;
                case AnimatorControllerParameterType.Float:
                    {
                        m_Animator.SetFloat(m_InfoData.anim, float.Parse(vaule));
                    }
                    break;
                case AnimatorControllerParameterType.Int:
                    {
                        m_Animator.SetInteger(m_InfoData.anim, int.Parse(vaule));
                    }
                    break;
                default:break;
            }
            
        }
        public virtual void EnterOneState(string satename)
        {
            if (m_Animator==null)
            {
                return;
            }
            m_Animator.SetTrigger(satename );
            return;
            RuntimeAnimatorController runtimeAnimatorController = m_Animator.runtimeAnimatorController;
            AnimationClip[] tAnimationClips = runtimeAnimatorController.animationClips;

            for (int i =0;i < tAnimationClips.Length;i++)
            {
                m_Animator.SetBool(tAnimationClips[i].name,false);
            }

            m_Animator.SetBool(satename, true);

        }
       
    }

     
}
