using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.C_Framework;

namespace YB.XWK.MainScene
{
    public class ActorFlashState : ActorAnimancerState
    {
        private GameObject flashParticleSystem;
        private GameObject endParticleSystem;
        private Texture _BlackBodyMat;
        private Texture _OriginBodyMat;
        private Texture _OriginHeadMat;
        private Material[] _BodyMats;
        private Material[] _FaceMats;
        private string _TimeMark = "ActorFlashState";
        public ActorFlashState(IActor actorAimMgr) : base(null)
        {
            HasInteractiveState = false;
            ActorStateName = "flashstate";
            OnInit(actorAimMgr);
            flashParticleSystem = GameResMgr.Instance.LoadResource<GameObject>("public/Hero_Effect/prefab/public_effect_sdsj.prefab", true);
            endParticleSystem = GameResMgr.Instance.LoadResource<GameObject>("public/Hero_Effect/prefab/public_effect_sdxsyan.prefab", true);
            endParticleSystem.gameObject.SetActive(false);
            flashParticleSystem.transform.position = m_ActorMgr.getActor().position;
            _BlackBodyMat = GameResMgr.Instance.LoadResource<Texture>("public/mesh/wukong/xwk_b_tx_c");
            _OriginHeadMat = GameResMgr.Instance.LoadResource<Texture>("public/mesh/wukong/xwk_1_tx_c");
            _OriginBodyMat = GameResMgr.Instance.LoadResource<Texture>("public/mesh/wukong/xwk_2_tx_c");

            _BodyMats = Actor.Find("ty_xwk_body").GetComponent<Renderer>().sharedMaterials;
            _FaceMats = Actor.Find("ty_xwk_face").GetComponent<Renderer>().sharedMaterials;
            InitMaterial();

            Play(this,_ActorAnimancerResConfig,RequestNextState);
          
        }
        public void InitMaterial()
        {
            for (int i = 0; i < _BodyMats.Length; i++)
            {
                if (_BodyMats[i].name.Equals("body"))
                {
                    _BodyMats[i].SetTexture("_MainTex", _BlackBodyMat);
                    break;
                }
            }
            for (int i = 0; i < _FaceMats.Length; i++)
            {
                if (_FaceMats[i].name.Equals("head"))
                {
                    _FaceMats[i].SetTexture("_MainTex", _BlackBodyMat);
                    break;
                }
            }
        }
        public void RecoverMaterial()
        {
            for (int i = 0; i < _BodyMats.Length; i++)
            {
                if (_BodyMats[i].name.Equals("body"))
                {
                    _BodyMats[i].SetTexture("_MainTex", _OriginBodyMat);

                    break;
                }
            }
            for (int i = 0; i < _FaceMats.Length; i++)
            {
                if (_FaceMats[i].name.Equals("head"))
                {
                    _FaceMats[i].SetTexture("_MainTex", _OriginHeadMat);
                    break;
                }
            }
        }
      
        public override void Stop()
        {
            AudioManager.Instance.StopAllEffect();
            if (Actor != null)
            {
                Actor.SetRenderVisible(true);
            }
            C_TimerMgr.Instance.RemoveTimer(_TimeMark);
            //显示角色，关闭爆炸
            if (endParticleSystem!=null)
            {
                GameObject.DestroyObject(endParticleSystem);
            }
            base.Stop();
            if (Actor!=null)
            {
                Actor.transform.DOKill();
            }
            if (flashParticleSystem!=null)
            {
                GameObject.DestroyObject(flashParticleSystem);
                flashParticleSystem = null;
            }
            RecoverMaterial();
        }
        public override void RequestNextState()
        {
            AudioManager.Instance.PlayEffectSound("public/sound_effect/public_xwkyx_002.ogg");
           //隐藏角色，显示爆炸
           endParticleSystem.gameObject.SetActive(true);
            Vector3 pos = m_ActorMgr.getActor().position;
            endParticleSystem.transform.position = new Vector3(pos.x-0.75f, 0.2f,0f);
            C_TimerMgr.Instance.AddTimer(1.5f, () => {
                m_ActorMgr.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_SHOW);
            }, _TimeMark);
            if (Actor != null)
            {
                Actor.SetRenderVisible(false);
            }
        }
    }

}
