using Assets.Scripts.C_Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XWK.Common.RedBomb
{
    internal class RedBombScoreBoard : MonoBehaviour
    {
        [Header("数字展示位")]
        [SerializeField]
        protected List<RectTransform> _NumDisplays;

        private string _NumPath = string.Empty;
        protected List<Sprite> _NumImages = new List<Sprite>();

        /// <summary>
        /// 分数
        /// </summary>
        private int _Score = 0;

        private void Awake()
        {
            _NumPath = "c_framework/ui/package_ui_sprite/reward/score/story/number/";
            for (int i = 0; i < 10; i++)
            {
                Sprite tex = GameResMgr.Instance.LoadResource<Sprite>(string.Concat(_NumPath, i.ToString(), ".png"), false);
                if (tex == null)
                {
                    C_DebugHelper.LogError("分数板图片错误" + _NumPath + " " + i);
                }
                else
                {
                    _NumImages.Add(tex);
                }
            }
        }

        public void UpdateRedBombScore(int score)
        {
            if (score > 0)
            {
                _Score += score;
                ChangeNumImage();
            }
        }

        private void ChangeNumImage()
        {
            int score = _Score;
            List<int> bits = new List<int>(_NumDisplays.Count);
            for (int i = 0; i < bits.Capacity; i++)
            {
                bits.Add(score % 10);
                score /= 10;
            }

            score = _Score;
            int bitNum = 0;
            for (; score > 0; bitNum++)
            {
                score /= 10;
            }

            for (int i = 0; i < _NumDisplays.Count; i++)
            {
                if (bitNum > i)
                {
                    _NumDisplays[i].gameObject.SetActive(true);
                    Image image = _NumDisplays[i].GetAddComponent<Image>();
                    image.sprite = _NumImages[bits[i]];
                    image.SetNativeSize();
                }
                else
                {
                    _NumDisplays[i].gameObject.SetActive(false);
                }
            }
        }

        private void OnDestroy()
        {
            _NumImages.Clear();
            Resources.UnloadUnusedAssets();
        }
    }
}