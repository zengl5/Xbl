using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.YM.Game
{
    public class MonsterAttackState:MonsterBaseState
    {
        private GameObject effect;
        private YMGameMonsterLevel CurrentLevel;
        public MonsterAttackState(RoleMgrBase roleBaseMgr, YMGameMonsterLevel level) : base(roleBaseMgr)
        {
            m_AllowClick = false;
            CurrentLevel = level;
        }
        protected override void OnInit()
        {
            base.OnInit();
        }
        public override void OnEnter()
        {
            base.OnEnter();
            PlayAnim("public/anim/jl_00014/jl_00014_skill01#anim", OnExited,true);
            //丢不同的特效
            if (CurrentLevel == YMGameMonsterLevel.YMG_MONSTER_LEVEL_1)
            {
                if (Random.Range(0, 2) == 1)
                {
                    AttackRXJ();
                }
                else
                {
                    AttackRCD();
                }
            }
            else if (CurrentLevel == YMGameMonsterLevel.YMG_MONSTER_LEVEL_2)
            {
                if (Random.Range(0, 2) == 1)
                {
                    AttackRZD();
                }
                else
                {
                    AttackRSD();
                }
            }
            else if(CurrentLevel == YMGameMonsterLevel.YMG_MONSTER_LEVEL_3)
            {
                if (Random.Range(0, 2) == 1)
                {
                    AttackRCD();
                }
                else
                {
                    AttackRXQ();
                }
            }
            else //if(CurrentLevel == YMGameMonsterLevel.YMG_MONSTER_LEVEL_4)
            {
                if (Random.Range(0, 2) == 1)
                {
                    AttackRZD();
                }
                else
                {
                    AttackRSD();
                }
            }
            //else 
            //{
            //    if (Random.Range(0, 2) == 1)
            //    {
            //        AttackRXQ();
            //    }
            //    else
            //    {
            //        AttackRCD();
            //    }
            //}
#if UNITY_EDITOR
            Utility.SetTransformLayer(effect.transform,LayerMask.NameToLayer("UI"));
#endif
            effect.transform.position = new Vector3(1000f, 0, 0);

        }
        void AttackRZD()
        {
            AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_8_1");

            effect = GameObject.Find("ui_public_effect_ns_rzd(Clone)");
            if (effect == null)
            {
                effect = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/ui_public_effect_ns_rzd", true);
                effect.gameObject.SetActive(true);
            }
            else
            {
                effect.gameObject.SetActive(false);
                effect.gameObject.SetActive(true);
            }
        }
        void AttackRCD()
        {
            AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_9_1");

            effect = GameObject.Find("ui_public_effect_ns_rcd(Clone)");
            if (effect == null)
            {
                effect = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/ui_public_effect_ns_rcd", true);
                effect.gameObject.SetActive(true);
            }
            else
            {
                effect.gameObject.SetActive(false);
                effect.gameObject.SetActive(true);
            }
        }
        void AttackRSD()
        {
            AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_10_1");

            //effect = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/ui_public_effect_ns_rsd", true);
            //  effect.gameObject.SetActive(true);
            effect = GameObject.Find("ui_public_effect_ns_rsd(Clone)");
            if (effect == null)
            {
                effect = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/ui_public_effect_ns_rsd", true);
                effect.gameObject.SetActive(true);
            }
            else
            {
                effect.gameObject.SetActive(false);
                effect.gameObject.SetActive(true);
            }
        }
        void AttackRXJ()
        {
            AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_7_1");

            //effect = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/ui_public_effect_ns_rxj", true);
            //effect.gameObject.SetActive(true);

            effect = GameObject.Find("ui_public_effect_ns_rxj(Clone)");
            if (effect == null)
            {
                effect = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/ui_public_effect_ns_rxj", true);
                effect.gameObject.SetActive(true);
            }
            else
            {
                effect.gameObject.SetActive(false);
                effect.gameObject.SetActive(true);
            }
        }
        void AttackRXQ()
        {
            AudioManager.Instance.PlayerSound("newyeargame/sound/game/xwk_hd_ns_11_1");

            //effect = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/ui_public_effect_ns_rxq", true);
            //effect.gameObject.SetActive(true);

            effect = GameObject.Find("ui_public_effect_ns_rxq(Clone)");
            if (effect == null)
            {
                effect = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/ui_public_effect_ns_rxq", true);
                effect.gameObject.SetActive(true);
            }
            else
            {
                effect.gameObject.SetActive(true);
                effect.gameObject.SetActive(true);
            }
        }
        public override void OnExited()
        {
            if (m_RoleMgr!=null)
            {
                m_RoleMgr.EnterNextState(YMGameEvent.YMG_EVENT_IDLESTATE);
            }
        }
    }
}

