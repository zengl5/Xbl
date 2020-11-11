using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XBL.Core
{
    public class BaseTouchItem : MonoBehaviour
    {
        protected bool _IsTouch = false;
        protected bool _CanTouch = false;
        public bool mPlaySoundOver = false;
        protected Vector2 _StartTouchpos;
        protected float _TouchDistance = 100f;
        protected string _AudioName;
        public Camera CurrentCamera;
        public bool SetTouch { get { return _CanTouch; } set { _CanTouch = value; } }
        public string AudioName { set { _AudioName = value; } get { return _AudioName; } }
        public bool TouchOver { get { return _IsTouch; } }
        private string _Name;
        public string Name { set { _Name = value; } get { return _Name; } }
        public int mCurrentIndex;
        public Collider mCollider;
        public delegate bool TouchDelegete(int index);
        public TouchDelegete OnTouch;
        public TouchDelegete OnPlayOver;
        void Start()
        {
            InitCollider();
        }
        public virtual void InitCollider()
        {
            if (gameObject.GetComponent<Collider>() == null)
            {
                mCollider = gameObject.AddComponent<BoxCollider>();
                mCollider.enabled = false;
            }
            else
            {
                mCollider = gameObject.GetComponent<BoxCollider>();

            }
        }
        protected virtual void TouchEvent()
        {
            if (_CanTouch && TouchManager.Instance.IsTouchValid(0))
            {
                TouchPhaseEnum touchPhase = TouchManager.Instance.GetTouchPhase(0);
                if (touchPhase == TouchPhaseEnum.BEGAN)
                {
                    TouchManager.Instance.GetTouchPos(0, out _StartTouchpos);
                    RaycastHit hit;
                    C_DebugHelper.Log("TouchManager begin..");

                    Ray ray = GetCurrentCamera().ScreenPointToRay(_StartTouchpos);
                    if (Physics.Raycast(ray, out hit, _TouchDistance))
                    {
                        C_DebugHelper.Log("book is touch" + hit.collider.name);

                        if (hit.collider.gameObject == gameObject)
                        {
                            C_DebugHelper.Log("book is touch in：" + hit.collider.name);
                            //播放动画
                            _IsTouch = true;
                            TouchHandle();
                        }
                    }
                }
            }
        }
        void Update()
        {
            TouchEvent();
        }
        public virtual void TouchHandle()
        {
            AudioManager.Instance.PlayEffectSound("public/sound/public_sd_009.ogg");

            mCollider.enabled = false;
            C_DebugHelper.Log(_AudioName);
            if (!string.IsNullOrEmpty(_AudioName))
                AudioManager.Instance.PlayerSound(_AudioName, false, new System.Action(PlaySoundOver));
            if (OnTouch != null)
            {
                OnTouch(mCurrentIndex);
            }
        }


        public virtual void TouchCallback()
        {
            
        }
        protected Camera GetCurrentCamera()
        {
            return CurrentCamera == null ? CurrentCamera = Camera.main : CurrentCamera;
        }
        protected  virtual void PlaySoundOver()
        {
            mPlaySoundOver = true;
            if (OnPlayOver!=null)
            {
                OnPlayOver(mCurrentIndex);
            }
        }
    }
}
