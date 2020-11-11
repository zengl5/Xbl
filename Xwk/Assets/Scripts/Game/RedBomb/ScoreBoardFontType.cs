using UnityEngine;
using UnityEngine.UI;

namespace XWK.Common.RedBomb
{
    internal class ScoreBoardFontType : MonoBehaviour
    {
        [Header("分数")]
        [SerializeField]
        private GameObject _NumTextGo;

        private Text _NumText = null;

        /// <summary>
        /// 分数
        /// </summary>
        private int _Score = 0;

        private void Awake()
        {
            _NumText = _NumTextGo.GetComponent<Text>();
        }

        public void UpdateRedBombScore(int score)
        {
            if (score > 0)
            {
                _Score += score;
                ChangeNum();
            }
        }

        private void ChangeNum()
        {
            _NumText.text = _Score.ToString();
        }
    }
}