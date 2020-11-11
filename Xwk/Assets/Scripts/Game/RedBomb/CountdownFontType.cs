using UnityEngine;
using UnityEngine.UI;

namespace XWK.Common.RedBomb
{
    internal class CountdownFontType : Countdown
    {
        [Header("数字")]
        [SerializeField]
        private GameObject _NumTextGo;

        private Text _NumText = null;

        protected override void Init()
        {
            _NumText = _NumTextGo.GetComponent<Text>();
        }

        protected override void ChangeNum()
        {
            int remainingTime = _Duration - _PassTime;
            _NumText.text = string.Concat(remainingTime.ToString(), "s");
        }
    }
}