using Assets.Scripts.C_Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XWK.Common.RedBomb
{
    internal class CountdownImageType : Countdown
    {
        [Header("数字展示位")]
        [SerializeField]
        private List<RectTransform> _NumDisplays;

        private List<Sprite> _NumImages = new List<Sprite>();

        protected string _Path = "";

        protected override void Init()
        {
            LoadImage(_Path);
        }

        /// <summary>
        /// 数字图片路径 动态加载
        /// </summary>
        protected void LoadImage(string path)
        {
            for (int i = 0; i < 10; i++)
            {
                Sprite tex = GameResMgr.Instance.LoadResource<Sprite>(string.Concat(path, i.ToString(), ".png"), false);
                if (tex == null)
                {
                    C_DebugHelper.LogError("倒计时图片错误" + path + " " + i);
                }
                else
                {
                    _NumImages.Add(tex);
                }
            }
        }

        protected override void ChangeNum()
        {
            int remainingTime = _Duration - _PassTime;
            List<int> bits = new List<int>(_NumDisplays.Count);
            for (int i = 0; i < _NumDisplays.Count; i++)
            {
                bits.Add(remainingTime % 10);
                remainingTime /= 10;
            }

            for (int i = 0; i < _NumDisplays.Count; i++)
            {
                Image image = _NumDisplays[i].GetAddComponent<Image>();
                image.sprite = _NumImages[bits[i]];
                image.SetNativeSize();
            }
        }

        protected override void OnDestroy()
        {
            _NumImages.Clear();
            Resources.UnloadUnusedAssets();
            base.OnDestroy();
        }
    }
}