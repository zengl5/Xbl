using DG.Tweening;
using UnityEngine;

namespace XWK.Common.UI_Reward
{
    internal class RewardCountdownNormal : RewardCountdown
    {
        /// <summary>
        /// 倒计时数字
        /// </summary>
        private RectTransform[] _Num = new RectTransform[9];

        private Sequence _Seq = null;

        public override void StartCountDown()
        {
            base.StartCountDown();
            InvokeRepeating("SetNum", 0f, 1.0f);//修改倒计时数字
        }

        public override void SetSuccess()
        {
            base.SetSuccess();
            Clear();
        }

        protected override void Awake()
        {
            base.Awake();
            Transform numP = gameObject.transform.Find("num");
            for (int i = 0; i < 9; i++)
            {
                _Num[i] = numP.GetChild(i).GetComponent<RectTransform>();
            }
        }

        private void SetNum()
        {
            _PassedTime++;
            int curIdx = _WaitTime - 1 - _PassedTime;
            if (curIdx < 0 || curIdx > 8)
            {
                return;
            }
            for (int i = 0; i < 9; i++)
            {
                _Num[i].gameObject.SetActive(i == curIdx);
            }

            _Seq = DOTween.Sequence();
            _Seq.Append(_Num[curIdx].DOScale(1.3f, 0.15f));
            _Seq.Append(_Num[curIdx].DOScale(0.8f, 0.15f));
            _Seq.Append(_Num[curIdx].DOScale(1.0f, 0.1f));
            _Seq.Play();
        }

        private void Clear()
        {
            _Seq.Kill();
            CancelInvoke("SetNum");
        }

        protected override void OnDestroy()
        {
            Clear();
            base.OnDestroy();
        }
    }
}