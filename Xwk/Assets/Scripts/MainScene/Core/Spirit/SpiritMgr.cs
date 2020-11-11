using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.AnimCallbacks;

namespace YB.XWK.MainScene
{
    public class SpiritItem : MonoBehaviour
    {
        public Camera _MainCamera;
        public bool mVisible = false;
        public bool mStop = false;
        public void Update()
        {
            if (mStop)
            {
                return;
            }
            IsInView(transform.position);
        }
        public void IsInView(Vector3 worldPos)
        {
            if (_MainCamera==null)
            {
                return;
            }
            Transform camTransform = _MainCamera.transform;
            Vector2 viewPos = _MainCamera.WorldToViewportPoint(worldPos);
            Vector3 dir = (worldPos - camTransform.position).normalized;
            float dot = Vector3.Dot(camTransform.forward, dir);     //判断物体是否在相机前面

            if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
            {
             
                mVisible = true;
            }
            else
            {
              
                mVisible = false;
            }
        }

    }

    public class SpiritMgr : MonoBehaviour
    {
        private SpiritItem _SpiritHuLuItem;
        private GameObject _SpiritHuLu;
        private SpiritItem _SpiritItem;
        private GameObject _TouchEffect;
        private GameObject _HuluTouchEffect;
        private GameObject jl00002Spirit = null;
        private Animator _BYAnimator;
        private Animator _HLAnimator;
        private string _StandAnimName = "jl00002_2_stand01#anim";
        private string _DongdongAnimName = "jl00002_2_doudong#anim";
        private string _CurrentHLAnim;
        private int PlayStandTime = 0;
        private int PlayHuluAnimTime = 0;

        public void InitSpirit(Camera camera)
        {
            PlayStandTime = 0;
            PlayHuluAnimTime = 0;
            if (jl00002Spirit != null) return;
          
            jl00002Spirit = GameResMgr.Instance.LoadResource<GameObject>("public/mesh/jl_00002/public_model_jl00002_2#mesh.prefab", true);
          //  jl00002Spirit.transform.position = new Vector3(4.68f, 0.02f, -3.39f);
            jl00002Spirit.transform.position = new Vector3(2.297f, 0.02f, -3.39f);
            jl00002Spirit.transform.localRotation = Quaternion.Euler(new Vector3(0, -26.4f, 0));
            jl00002Spirit.SetActive(true);

            //_TouchEffect = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/public_effect_dianji", true);
            //_TouchEffect.transform.SetParent(jl00002Spirit.transform);
            //_TouchEffect.transform.localPosition = new Vector3(19f, 47f, 58f);
            //_TouchEffect.SetActive(true);


            BoxCollider jl00002Collider = jl00002Spirit.GetAddComponent<BoxCollider>();
            jl00002Collider.center = new Vector3(0, 64.9f, 0);
            jl00002Collider.size = new Vector3(100f, 131f, 100f);
            jl00002Collider.GetAddComponent<MainFireSpirit>();
            _SpiritItem = jl00002Spirit.transform.Find("ji_00002_2").GetAddComponent<SpiritItem>();
            _SpiritItem._MainCamera = camera;

            _BYAnimator = jl00002Spirit.GetAddComponent<Animator>();
            _BYAnimator.runtimeAnimatorController = GameResMgr.Instance.LoadResource<RuntimeAnimatorController>("main/animationcontroller/main_jl00002_ac");
            _BYAnimator.AddClipEndCallback(_StandAnimName, new System.Action(PlayStandOver));


            _SpiritHuLu = GameResMgr.Instance.LoadResource<GameObject>("public/mesh/jl_00001/public_model_jl00001_2#mesh.prefab", true);
            _SpiritHuLu.transform.position = new Vector3(-2.28f, 0.38f, -4.19f);
            _SpiritHuLu.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            _SpiritHuLu.SetActive(true);

            BoxCollider huLuCollider = _SpiritHuLu.GetAddComponent<BoxCollider>();
            huLuCollider.center = new Vector3(0, 64.9f, 0);
            huLuCollider.size = new Vector3(100f, 131f, 100f);

            _HuluTouchEffect = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/public_effect_hlts", true);
            _HuluTouchEffect.transform.SetParent(_SpiritHuLu.transform);
            _HuluTouchEffect.transform.localPosition = new Vector3(32f, -9.2f, 58f);
            _HuluTouchEffect.SetActive(true);

            _SpiritHuLuItem = _SpiritHuLu.transform.Find("jl_00001_2").GetAddComponent<SpiritItem>();
            _SpiritHuLuItem._MainCamera = camera;

            _HLAnimator = _SpiritHuLu.GetAddComponent<Animator>();
            _HLAnimator.runtimeAnimatorController = GameResMgr.Instance.LoadResource<RuntimeAnimatorController>("main/animationcontroller/main_jl00001_ac");
            PlayHuluAnim();

        }
        void PlayHuluAnim()
        {
            if (!string.IsNullOrEmpty(_CurrentHLAnim))
            {
                _HLAnimator.RemoveClipEndCallback(_CurrentHLAnim, new System.Action(PlayHuluAnim));
            }
            string[] stateName = { "jl00001_2_stand01#anim", "jl00001_2_doudong01#anim" };
            PlayHuluAnimTime = ++PlayHuluAnimTime% stateName.Length;
            _CurrentHLAnim = stateName[PlayHuluAnimTime];
            _HLAnimator.AddClipEndCallback(_CurrentHLAnim, new System.Action(PlayHuluAnim));
            _HLAnimator.SetTrigger(_CurrentHLAnim);

        }
        void PlayStandOver()
        {
            if (PlayStandTime > 4)
            {
                _BYAnimator.SetTrigger(_DongdongAnimName);
                _BYAnimator.RemoveClipEndCallback(_StandAnimName, new System.Action(PlayStandOver));
                _BYAnimator.AddClipEndCallback(_DongdongAnimName, new System.Action(PlayerDongDongOver));
                PlayStandTime = 0;
            }
            else
            {
                _BYAnimator.SetTrigger(_StandAnimName);
                PlayStandTime++;
            }
        }
        void PlayerDongDongOver()
        {
            _BYAnimator.SetTrigger(_StandAnimName);
            _BYAnimator.RemoveClipEndCallback(_DongdongAnimName, new System.Action(PlayerDongDongOver));
            _BYAnimator.AddClipEndCallback(_StandAnimName, new System.Action(PlayStandOver));
        }
        public void StopAnim()
        {
            if (_BYAnimator != null)
            {
                _BYAnimator.RemoveClipEndCallback(_DongdongAnimName, new System.Action(PlayerDongDongOver));
                _BYAnimator.RemoveClipEndCallback(_StandAnimName, new System.Action(PlayStandOver));
            }
            if (!string.IsNullOrEmpty(_CurrentHLAnim) && _HLAnimator !=null)
            {
                _HLAnimator.RemoveClipEndCallback(_CurrentHLAnim, new System.Action(PlayHuluAnim));
            }

        }
        public void DestorySpirit()
        {
            if (jl00002Spirit!=null)
            {
                DestroyObject(jl00002Spirit);
                jl00002Spirit = null;
            }
            //if (_TouchEffect!=null)
            //{
            //    DestroyObject(_TouchEffect);
            //    _TouchEffect = null;
            //}
            if (_SpiritHuLu!=null)
            {
                DestroyObject(_SpiritHuLu);
                _SpiritHuLu = null;
            }
            if (_HuluTouchEffect!=null)
            {
                DestroyObject(_HuluTouchEffect);
                _HuluTouchEffect = null;
            }

            StopAnim();
        }

        public bool SpiritVisible()
        {
            if (jl00002Spirit!=null && _SpiritItem != null && _SpiritItem.mVisible)
            {
                return true;
            }
            return false;
        }

        public void StopViewState()
        {
            if (_SpiritItem!=null )
            {
                _SpiritItem.mStop = true;
            }
        }

        public bool GetHuLuVisible()
        {
            if (_SpiritHuLu != null && _SpiritHuLuItem != null && _SpiritHuLuItem.mVisible)
            {
                return true;
            }
            return false;
        }
        public void StopHuLuViewState()
        {
            if (_SpiritHuLuItem != null)
            {
                _SpiritHuLuItem.mStop = true; 
            }
        }

    }
}

