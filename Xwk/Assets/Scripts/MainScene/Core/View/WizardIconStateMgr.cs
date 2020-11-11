using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Assets.Scripts.C_Framework;

namespace YB.XWK.MainScene {

    public class WizardIconStateMgr : MonoBehaviour
    {
        private RectTransform _IconStateRtf;
        private RectTransform _IconStarRtf;
        private string _CurrentCollectSpiritName;
        // Use this for initialization
        void Awake()
        {
            //显示icon上下跳动
            Init();
        }
        private void Init()
        {
            _IconStateRtf = transform.Find("state").GetComponent<RectTransform>();
            _IconStarRtf = transform.Find("star").GetComponent<RectTransform>();
          //  _IconStarRtf.gameObject.SetActive(false);
          //  _IconStateRtf.gameObject.SetActive(false);
        }
        public void InitStateIcon(SpiritAdModel spiritAdModel)
        {
            if (_IconStarRtf == null)
            {
                Init();
            }

         //   StateIcon();
            ShowStateIcon(spiritAdModel);
        }
        private void StateIcon()
        {
            //每日灵气值都未收集，离线受益都满了两个状态需要提示红点
            //if (DailyBounsData.UnCollectAllBouns())//所有离线灵气都满，未收集
            //{
            //    _IconStarRtf.gameObject.SetActive(true);
            //    _IconStateRtf.gameObject.SetActive(false);
            //}
            //else
            {
                _IconStarRtf.gameObject.SetActive(false);
                _IconStateRtf.gameObject.SetActive(false);
            }
        }
        public void ShowStateIcon(SpiritAdModel spiritAdModel)
        {
            if (_IconStarRtf == null)
            {
                Init();
            }

            bool showWizardIcon = false;
            if (WizardData.DosShowRecommendIcon(ref _CurrentCollectSpiritName))
            {
                string uiPath = spiritAdModel.getWizardIconUiPath(_CurrentCollectSpiritName);
                

                Sprite sprite = GameResMgr.Instance.LoadResource<Sprite>(uiPath, true);
                if (sprite==null)
                {
                    C_DebugHelper.LogError("wizard show recommend icon uiPath is null:" + uiPath);
                    showWizardIcon = false;
                }
                else
                {
                    showWizardIcon = true;
                    _IconStateRtf.GetComponent<Image>().sprite = sprite;
                    _IconStateRtf.transform.gameObject.SetActive(true);
                    _IconStarRtf.transform.gameObject.SetActive(false);

                    IconUp();
                }
            }
            if (!showWizardIcon)
            {
                StateIcon();
            }
        }
        public void HideStateIcon()
        {
            //已读当前收集的精灵
            WizardData.SetRecommendIconState();
            if (_IconStateRtf != null)
            {
                _IconStateRtf.DOKill();
            }
        }
        void IconUp()
        {
            _IconStateRtf.gameObject.SetActive(true);
            _IconStateRtf.DOAnchorPosY(85f, 0.3f).OnComplete(() => {
                IconDown();
            });
        }
        void IconDown()
        {
            _IconStateRtf.gameObject.SetActive(true);
            _IconStateRtf.DOAnchorPosY(53f, 0.3f).OnComplete(() => {
                IconUp();
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnDestroy()
        {
            if (_IconStateRtf!=null)
            {
                _IconStateRtf.DOKill();
            }
        }
    }

}

