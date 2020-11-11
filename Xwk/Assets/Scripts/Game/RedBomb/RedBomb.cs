using Assets.Scripts.C_Framework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XWK.Common.RedBomb
{
    internal class RedBomb : MonoBehaviour, IPointerDownHandler
    {
        //private static uint _TotalNum = 0;
        //private uint _SerialNumber = 0;

        [SerializeField]
        [Header("红包图片")]
        private GameObject _RedBombImgGo;

        private RectTransform _RectTransform = null;

        private FlyAction _FlyAction = null;
        private GameObject _ClickEffect = null;
        private bool _IsHit = false;
        private bool _FlyTag = false;

        private void Awake()
        {
            //_SerialNumber = RedBomb._TotalNum++;
            //C_DebugHelper.LogError(_SerialNumber);
            gameObject.name = RedBombUI.RedBombRes;
            _RectTransform = transform.GetComponent<RectTransform>();
        }

        public void Init()
        {
            _IsHit = false;
            _FlyTag = false;
            if (!_RedBombImgGo.activeSelf)
            {
                _RedBombImgGo.SetActive(true);
            }
            SetFlyAction();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //C_DebugHelper.LogError("OnPointerClick  " + _SerialNumber);
            OnHit();
        }

        //TODO
        /// <summary>
        /// 选择飞行动作
        /// </summary>
        public void SetFlyAction()
        {
            if (_ClickEffect)
            {
                _ClickEffect.SetActive(false);
            }

            _FlyAction = new FlyActionStraightRandom(_RectTransform);
            _FlyAction.Init();
        }

        /// <summary>
        /// 播放飞行动作
        /// </summary>
        public void Fly()
        {
            _FlyTag = true;
        }

        public void Stop()
        {
            _FlyTag = false;
        }

        private void OnHit()
        {
            if (_IsHit)
            {
                return;
            }
            _IsHit = true;
            Hit();
            Recovery(2.0f);
        }

        private void Hit()
        {
            AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>(string.Concat(RedBombConstRes.SoundEffectPath, RedBombConstRes.ClickSoundEffect), "");
            AudioManager.Instance.PlayEffectClipSound(clip, false);

            _RedBombImgGo.SetActive(false);
            RedBombUI.Instance.UpdateRedBombScore(1);
            CreateAndPlayClickEffect();
        }

        /// <summary>
        /// 创建播放点击特效
        /// </summary>
        private void CreateAndPlayClickEffect()
        {
            if (_ClickEffect == null)
            {
                _ClickEffect = Instantiate(RedBombUI.Instance.ClickEffectPrefab, transform);
            }
            //Utility.SetTransformLayer(_ClickEffect.transform, LayerMask.NameToLayer("UI"));
            //_ClickEffect.transform.localPosition = new Vector3(0, 0, 0);
            //_ClickEffect.transform.localScale = new Vector3(108, 108, 108);
            //_ClickEffect.name = RedBombUI.ClickEffectRes;
            _ClickEffect.SetActive(true);
            _ClickEffect.transform.SetAsLastSibling();
            ParticleSystem[] particleSystems = _ClickEffect.GetComponentsInChildren<ParticleSystem>();
            foreach (var particleSystem in particleSystems)
            {
                particleSystem.Play();
            }
        }

        private void Update()
        {
            if (!_FlyTag || _IsHit)
            {
                return;
            }

            if (_FlyAction != null && !_IsHit)
            {
                _FlyAction.Fly();
            }
            if (_RectTransform.anchoredPosition.y < -800f)
            {
                Clear();
            }
        }

        private void Clear()
        {
            Stop();
            Recovery();
        }

        private void Recovery(float delay = 0f)
        {
            if (delay > 0)
            {
                Invoke("DoRecovery", delay);
            }
            else
            {
                DoRecovery();
            }
        }

        private void DoRecovery()
        {
            gameObject.SetActive(false);
            C_MonoSingleton<C_PoolMgr>.GetInstance().Despawn(gameObject, C_PoolChannel.UI);
            //Destroy(gameObject);
        }

        private void OnDestroy()
        {
        }
    }

    internal class FlyAction
    {
        protected RectTransform _RectTransform = null;

        public FlyAction(RectTransform trans)
        {
            _RectTransform = trans;
        }

        public virtual void Init()
        {
            //子类实现
        }

        public virtual void Fly()
        {
            //子类实现
        }
    }

    internal class FlyActionStraightRandom : FlyAction
    {
        private Vector2 _Speed = Vector2.zero;

        private static int _EdgeLeft = -1;
        private static int _EdgeRight = 10;
        private static float _EdgeStep = 200f;
        private static float _SpeedYMin = -2.0f;//往下飞
        private static float _SpeedYMax = -4.0f;//往下飞
        private static float _SpeedScale = 200f;

        public FlyActionStraightRandom(RectTransform trans) : base(trans)
        {
        }

        public override void Init()
        {
            //设定初始位置
            Vector2 pos = new Vector2(Random.Range(_EdgeLeft, _EdgeRight) * _EdgeStep, 700f);
            _RectTransform.anchoredPosition = pos;
            RectTransform rect = _RectTransform.Find("Image").GetComponent<RectTransform>();
            rect.localRotation = Quaternion.Euler(new Vector3(0, 0, -45f));
            float scale = Random.Range(1f, 2f);
            rect.localScale = new Vector3(scale, scale);

            //设定飞行速度
            int weight = Random.Range(1, 10);
            float speedY = _SpeedYMin;
            if (weight < 3)
            {
                //1,2
                speedY = _SpeedYMin;
            }
            else if (weight < 7)
            {
                //3,4,5,6
                speedY = (_SpeedYMin + _SpeedYMax) / 2.0f;
            }
            else
            {
                //7,8,9
                speedY = _SpeedYMax;
            }
            _Speed = new Vector2(speedY, speedY);
        }

        public override void Fly()
        {
            _RectTransform.anchoredPosition += _Speed * Time.deltaTime * _SpeedScale;
        }
    }
}