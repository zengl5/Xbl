using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.YM.Game
{
    public class MonsterHenShinState : MonsterBaseState
    {
      
        protected Texture _OrignalTexture;
        protected Material _Material;
        private GameObject _effect_skill;
        public MonsterHenShinState(RoleMgrBase roleMgr,int currentLevel) : base(roleMgr)
        {
            m_currentLevel = currentLevel;
            m_AllowClick = false;

            if (roleMgr.UpdateMainTexture(m_currentLevel))
            {
                _effect_skill = GameResMgr.Instance.LoadResource<GameObject>("public/Hero_Effect/prefab/public_effect_skill_bs", true);
                _effect_skill.transform.SetParent(Actor.transform);
                _effect_skill.transform.localPosition = Vector3.zero;

                PlayAnim("public/anim/jl_00014/jl_00014_skill02#anim", RequesetNextState);
                AudioManager.Instance.PlayEffectAutoClose("newyeargame/sound/game/xwk_hd_ns_42.ogg");
            }
            else
            {
                RequesetNextState();
            }
        }
        
        public void RequesetNextState()
        {
            m_RoleMgr.EnterNextState(YMGameEvent.YMG_EVENT_IDLESTATE);
            if (_effect_skill!=null)
            {
                GameObject.DestroyObject(_effect_skill);
                _effect_skill = null;
            }
        }
    }


}
