using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace YB.XWK.MainScene
{
    public class GameIcon : MonoBehaviour
    {
        private RectTransform _RectTransform;
        private Sequence _Sequence;
        private Image _Image;
        private Transform _Coin;
        private bool _IsLock;
        // Use this for initialization
        void Awake()
        {
            _Sequence = DOTween.Sequence();
            _RectTransform = transform.GetComponent<RectTransform>();
            _RectTransform.localScale = Vector3.zero;
            _Image = transform.GetComponent<Image>();
            _Coin = transform.Find("state");
            _Coin.gameObject.SetActive(false);
            _IsLock = false;
        }
        private void FadeImgColor(bool lockstate = false)
        {
            if (lockstate)
            {
                _Image.color = new Color(160f/ 255f, 160f / 255f, 160f / 255f);
            }
            else
            {
                _Image.color = new Color(1f, 1f, 1f);
            }
        }
        public void OnEnable()
        {
            _RectTransform.localScale = Vector3.zero;
        }
        public void Show(bool lockstate = false)
        {
            _IsLock = lockstate;
            FadeImgColor(_IsLock);

            if (_Sequence != null)
            {
                Color tempColor = _Image.color;
                _Sequence.Kill();

                _Sequence.Append(_Image.DOFade(0, 0))
                    .Append(_RectTransform.DOScale(0.5f, 0))
                    .Join(_Image.DOFade(1, 0.167f))
                    .Join(_RectTransform.DOScale(1.25f, 0.167f).OnComplete(() => {

                        _RectTransform.DOScale(1f, 0.125f).OnComplete(ShowBouns);
                    }));
            }
        }

        public void ShowBouns()
        {
            if (_IsLock)
            {
                _Coin.gameObject.SetActive(false);
                return;
            }
            if (DailyBounsData.LeaveBouns(gameObject.name))
            {
                //显示每个金币的状态
                _Coin.gameObject.SetActive(true);
                _Coin.localScale = Vector3.zero;
                _Coin.DOScale(1f, 0.125f);
            }
            else
            {
                //显示每个金币的状态
                _Coin.gameObject.SetActive(false);
               
            }
        }
        public void Hide()
        {
            _Coin.DOKill();
            _Coin.gameObject.SetActive(false);

            if (_Sequence != null)
            {
                _Sequence.Kill();
            }
            _Sequence.Append(_RectTransform.DOScale(1.25f, 0.167f))
               .AppendInterval(0.167f)
               .Join(_RectTransform.DOScale(0f, 0.167f))
                   .Join(_Image.DOFade(0, 0.167f))
                   ;
        }
        public void HideCoin()
        {
            if(_Coin ==null)
            {
                return;
            }
            _Coin.DOKill();
            _Coin.gameObject.SetActive(false);
        }
      
        public void OnDestroy()
        {
            if (_Sequence != null)
            {
                _Sequence.Kill();
            }
            if (_Coin!=null)
            {
                _Coin.DOKill();
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }

}

