using Assets.Scripts.C_Framework;
using System;
using UnityEngine;

namespace XWK.Common.RedBomb
{
    internal class Countdown : MonoBehaviour
    {
        [Header("倒计时时长")]
        [SerializeField]
        [Range(0, 99)]
        protected int _Duration = 3;

        protected int _PassTime = 0;

        private bool _StartTag = false;
        private bool _EndTag = false;

        protected Action _EndAction = null;

        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            //子类实现
        }

        public void SetDuration(int seconds)
        {
            _Duration = seconds;
        }

        public void SetEndAction(Action action)
        {
            _EndAction = action;
        }

        public void StartTimer()
        {
            if (_StartTag)
            {
                return;
            }
            _StartTag = true;

            ChangeNum();
            InvokeRepeating("Timer", 1f, 1f);
        }

        public void StopTimer()
        {
            _StartTag = false;
            _EndTag = true;
            CancelInvoke("Timer");
        }

        private void Timer()
        {
            if (!_StartTag)
            {
                return;
            }
            if (_EndTag)
            {
                return;
            }
            _PassTime++;
            ChangeNum();
            if (_PassTime >= _Duration)
            {
                C_DebugHelper.Log("倒计时结束");
                _EndTag = true;
                if (_EndAction != null)
                {
                    Action action = _EndAction;
                    action();
                    _EndAction = null;
                }

                return;
            }
        }

        protected virtual void ChangeNum()
        {
            C_DebugHelper.LogError("需要子类实现");
        }

        protected virtual void OnDestroy()
        {
            StopTimer();
        }
    }
}