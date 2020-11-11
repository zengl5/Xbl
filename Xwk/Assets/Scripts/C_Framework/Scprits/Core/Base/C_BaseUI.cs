using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.C_Framework
{
    public enum Enum_UICameraType
    {
        High,
        Low,
        Story = 9,
        Userdefined
    }

    public enum Enum_UICanvasScaler
    {
        Width,
        Height,
        Nomal
    }

    public enum Enum_UIDeleteType
    {
        Nomal,
        DonotDeletedByOthers,
        Forever
    }

    public enum Enum_UIActionType
    {
        Nomal,
        ScaleAction
    }

    public abstract class C_BaseUI : MonoBehaviour
    {
        public int Layer = 5;

        public Enum_UICameraType UICameraType = Enum_UICameraType.High;

        public Enum_UICanvasScaler UICanvasScaler = Enum_UICanvasScaler.Nomal;

        public Enum_UIDeleteType UIDeleteType = Enum_UIDeleteType.Nomal;

        public Enum_UIActionType UIActionType = Enum_UIActionType.Nomal;

        public Text[] Text_LocalizationVector;

        [HideInInspector]
        public Canvas UICanvas = null;

        protected Transform m_MainLayer = null;

        private const float s_UIActionTime = 0.5f;

        private bool m_IsOnAdaption = false;

        void Awake()
        {
            if (this.transform.parent == null)
                this.transform.SetParent(C_MonoSingleton<C_UIMgr>.GetInstance().transform);

            UICanvas = this.transform.Find("Canvas").GetComponent<Canvas>();
            UICanvas.renderMode = RenderMode.ScreenSpaceCamera;
            
            if (UICameraType == Enum_UICameraType.Low)
                UICanvas.worldCamera = C_UIMgr.c_UICameraLow;
            else if(UICameraType==Enum_UICameraType.High)
                UICanvas.worldCamera = C_UIMgr.c_UICameraHigh;
            
            UICanvas.sortingOrder = Layer;
            UICanvas.planeDistance = 50;

            CanvasScaler canvasScaler = UICanvas.transform.GetComponent<CanvasScaler>();
            if (canvasScaler != null)
            {
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.referenceResolution = new Vector2(C_GameFramework.c_DesignWidth, C_GameFramework.c_DesignHeight);
                if (UICanvasScaler == Enum_UICanvasScaler.Nomal)
                    canvasScaler.matchWidthOrHeight = C_UIMgr.c_Match;
                else if (UICanvasScaler == Enum_UICanvasScaler.Height)
                {
                    canvasScaler.matchWidthOrHeight = 1.0f;
                }
                else
                {
                    canvasScaler.matchWidthOrHeight = 0f;
                }
            }

            m_MainLayer = UICanvas.transform.Find("MainLayer");

            Localization();

            C_MonoSingleton<C_GameFramework>.GetInstance().onRealtimeUpdate += onRealtimeUpdate;
            
            onInit();
        }

        void Start()
        {
            if (!m_IsOnAdaption)
            {
                onAdaption();

                m_IsOnAdaption = true;
            }
        }

        void Update()
        {
            onUpdate();
        }

        void OnDestroy()
        {
            onDestroy();

            C_MonoSingleton<C_GameFramework>.GetInstance().onRealtimeUpdate -= onRealtimeUpdate;
        }

        public void OpenUI(params object[] uiObjParams)
        {
            ShowUI();

            onOpenUI(uiObjParams);

            onPlayOpenUIAudio();
        }

        public void CloseUI()
        {
            HideUI();
            
            onCloseUI();

            onPlayCloseUIAudio();
        }

        public void ShowUI()
        {
            this.gameObject.SetActive(true);
            onShowUI();
            switch (UIActionType)
            {
                case Enum_UIActionType.ScaleAction:
                    
                    if (m_MainLayer != null)
                    {
                        m_MainLayer.localScale = Vector3.zero;
                        m_MainLayer.DOScale(Vector3.one, s_UIActionTime);
                    }

                    break;
            }
        }

        public void HideUI()
        {
            switch (UIActionType)
            {
                case Enum_UIActionType.ScaleAction:

                    if (m_MainLayer != null)
                    {
                        Sequence sq = DOTween.Sequence();
                        sq.Append(m_MainLayer.DOScale(Vector3.zero, s_UIActionTime));
                        sq.AppendCallback(() => { this.gameObject.SetActive(false); });

                        return;
                    }

                    break;
            }

            this.gameObject.SetActive(false);
        }

        public void Rrefresh()
        {
            if (this.transform.parent == null)
                this.transform.SetParent(C_UIMgr.Instance.transform);

            Transform tf = this.transform.Find("Canvas");
            if (tf != null)
            {
                Canvas canvas = tf.GetComponent<Canvas>();
                if (canvas != null)
                    canvas.sortingOrder = Layer;
            }

            C_MonoSingleton<C_UIMgr>.GetInstance().AddOpenedUIsList(this.gameObject);
        }

        private void Localization()
        {
            foreach (Text Text in Text_LocalizationVector)
            {
                if (Text != null)
                    Text.text = C_Localization.GetLocalization(Text.text);
            }
        }

        protected virtual void onInit()
        {
        }

        protected virtual void onAdaption()
        {
        }

        protected virtual void onUpdate()
        {
        }

        protected virtual void onRealtimeUpdate(float deltaTime)
        {
        }

        protected virtual void onOpenUI(params object[] uiObjParams)
        {
        }

        protected virtual void onCloseUI()
        {
        }

        protected virtual void onDestroy()
        {
        }

        protected virtual void onPlayOpenUIAudio()
        {
        }

        protected virtual void onPlayCloseUIAudio()
        {
        }
        protected virtual void onShowUI()
        {

        }
    }
}