using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.C_Framework
{
    public class C_UIMgr : C_MonoSingleton<C_UIMgr>
    {
        public static Camera c_UICameraHigh = null;
        public static Camera c_UICameraLow = null;

        public static float c_AspectRatio = 1;
        public static float c_Match = 0;
        public static float c_ParticlesScale = 1;

        private EventSystem m_UIInputEventSystem;

        private List<GameObject> m_OpenedUIsList = new List<GameObject>();

        private List<GameObject> m_DirtyUIList = new List<GameObject>();

        protected override void Init()
        {
            CreateCamera();
            Adaptiver();
            CreateEventSystem();
        }

        void Update()
        {
            if (m_DirtyUIList.Count > 0)
            {
                for (int i = m_DirtyUIList.Count - 1; i >= 0; i--)
                    Destroy(m_DirtyUIList[i]);

                m_DirtyUIList.Clear();

                Resources.UnloadUnusedAssets();
            }
        }

        private void CreateCamera()
        {
            GameObject gameObjectHigh = new GameObject("UICameraHigh");
            gameObjectHigh.transform.SetParent(this.transform);
            gameObjectHigh.transform.localPosition = new Vector3(1000.0f, 0, 0);
            gameObjectHigh.transform.localRotation = Quaternion.identity;
            gameObjectHigh.transform.localScale = Vector3.one;
            c_UICameraHigh = gameObjectHigh.AddComponent<Camera>();
            c_UICameraHigh.clearFlags = CameraClearFlags.Depth;
            c_UICameraHigh.cullingMask = LayerMask.GetMask("UI");
            c_UICameraHigh.orthographic = true;
            c_UICameraHigh.orthographicSize = 50f;
            c_UICameraHigh.depth = 50f;
            c_UICameraHigh.allowHDR = false;

            GameObject gameObjectLow = new GameObject("UICameraLow");
            gameObjectLow.transform.SetParent(this.transform);
            gameObjectLow.transform.localPosition = new Vector3(-1000.0f, 0, 0);
            gameObjectLow.transform.localRotation = Quaternion.identity;
            gameObjectLow.transform.localScale = Vector3.one;
            c_UICameraLow = gameObjectLow.AddComponent<Camera>();
            c_UICameraLow.clearFlags = CameraClearFlags.Depth;
            c_UICameraLow.cullingMask = LayerMask.GetMask("UI");
            c_UICameraLow.orthographic = true;
            c_UICameraLow.orthographicSize = 50f;
            c_UICameraLow.depth = 0f;
            c_UICameraLow.allowHDR = false;
        }

        private void Adaptiver()
        {
            c_AspectRatio = (float)Screen.width / (float)Screen.height;

            float widthScale = Screen.width / C_GameFramework.c_DesignWidth;
            float heightScale = Screen.height / C_GameFramework.c_DesignHeight;

            if (widthScale > heightScale)
                c_Match = 1;
            else
                c_Match = 0;

            if (c_AspectRatio < C_GameFramework.c_DesignAspectRatio)
                c_ParticlesScale = c_AspectRatio / C_GameFramework.c_DesignAspectRatio;
        }

        private void CreateEventSystem()
        {
            m_UIInputEventSystem = Object.FindObjectOfType<EventSystem>();
            if (m_UIInputEventSystem == null)
            {
                GameObject gameObject = new GameObject("EventSystem");
                m_UIInputEventSystem = gameObject.AddComponent<EventSystem>();
                gameObject.AddComponent<StandaloneInputModule>();
            }

            m_UIInputEventSystem.transform.SetParent(this.transform);
        }


        #region Get

        public T GetUI<T>(string uiName) where T : C_BaseUI
        {
            GameObject retObj = GetUI(uiName);
            if (retObj != null)
                return retObj.GetComponent<T>();

            return null;
        }

        public GameObject GetUI(string uiName)
        {
            if (string.IsNullOrEmpty(uiName))
                return null;

            for (int i = m_OpenedUIsList.Count - 1; i >= 0; i--)
            {
                if (m_OpenedUIsList[i] == null)
                {
                    m_OpenedUIsList.RemoveAt(i);
                    continue;
                }

                if (m_OpenedUIsList[i].name == uiName)
                    return m_OpenedUIsList[i];
            }

            return null;
        }

        public bool IsOpenedUI(string uiName)
        {
            GameObject uiGO = GetUI(uiName);
            if (uiGO != null && uiGO.activeSelf)
                return true;

            return false;
        }

        public bool IsOpenedUI(GameObject uiGO)
        {
            if (uiGO == null)
                return false;

            for (int i = m_OpenedUIsList.Count - 1; i >= 0; i--)
            {
                if (m_OpenedUIsList[i] == null)
                {
                    m_OpenedUIsList.RemoveAt(i);
                    continue;
                }

                if (m_OpenedUIsList[i] == uiGO && uiGO.activeSelf)
                    return true;
            }

            return false;
        }

        #endregion


        #region Open

        public GameObject OpenUI(string uiName, params object[] uiParams)
        {
            return ReallyOpenUI(uiName, "", false, uiParams);
        }

        public GameObject OpenUICloseOthers(string uiName, params object[] uiParams)
        {
            return ReallyOpenUI(uiName, "", true, uiParams);
        }

        public GameObject OpenUIWithPath(string uiName, string uiPath, params object[] uiParams)
        {
            return ReallyOpenUI(uiName, uiPath, false, uiParams);
        }

        public GameObject OpenUIWithPathCloseOthers(string uiName, string uiPath, params object[] uiParams)
        {
            return ReallyOpenUI(uiName, uiPath, true, uiParams);
        }

        private GameObject ReallyOpenUI(string uiName, string uiPath, bool isCloseOthers, params object[] uiParams)
        {
            if (string.IsNullOrEmpty(uiName))
            {
                C_DebugHelper.LogError("C_UIMgr ReallyOpenUI uiName is null or empty!");
                return null;
            }

            if (isCloseOthers)
                CloseUIAll();

            GameObject UIObject = GetUI(uiName);
            if (UIObject != null)
            {
                C_BaseUI listBaseUI = UIObject.gameObject.GetComponent<C_BaseUI>();
                if (listBaseUI != null)
                    listBaseUI.OpenUI(uiParams);

                return UIObject;
            }

            UIObject = C_Singleton<GameResMgr>.GetInstance().LoadResource_UI(uiName, uiPath);
            if (UIObject != null)
            {
                UIObject.name = uiName;
                m_OpenedUIsList.Add(UIObject);

                C_BaseUI baseUI = UIObject.GetComponent<C_BaseUI>();
                if (baseUI != null)
                    baseUI.OpenUI(uiParams);
            }

            return UIObject;
        }

        #endregion


        #region Close

        public void CloseUIAll()
        {
            for (int i = m_OpenedUIsList.Count - 1; i >= 0; i--)
                CloseUI(m_OpenedUIsList[i]);
        }

        public void CloseUI(string uiName)
        {
            CloseUI(GetUI(uiName));
        }

        public void CloseUI(GameObject uiGO)
        {
            if (uiGO == null)
                return;

            C_BaseUI baseUI = uiGO.GetComponent<C_BaseUI>();
            if (baseUI != null && baseUI.UIDeleteType == Enum_UIDeleteType.Nomal)
                baseUI.CloseUI();
        }

        public void MandatoryCloseUIAll()
        {
            for (int i = m_OpenedUIsList.Count - 1; i >= 0; i--)
                MandatoryCloseUI(m_OpenedUIsList[i]);
        }

        public void MandatoryCloseUI(string uiName)
        {
            MandatoryCloseUI(GetUI(uiName));
        }

        public void MandatoryCloseUI(GameObject uiGO)
        {
            if (uiGO == null)
                return;

            C_BaseUI baseUI = uiGO.GetComponent<C_BaseUI>();
            if (baseUI != null && baseUI.UIDeleteType != Enum_UIDeleteType.Forever)
                baseUI.CloseUI();
        }
        
        public void DestoryAllUI()
        {
            for (int i = m_OpenedUIsList.Count - 1; i >= 0; i--)
                DestoryUI(m_OpenedUIsList[i]);
        }

        public void DestoryUI(GameObject uiGO)
        {
            if (uiGO == null)
                return;

            C_BaseUI baseUI = uiGO.GetComponent<C_BaseUI>();
            if (baseUI != null && baseUI.UIDeleteType != Enum_UIDeleteType.Forever)
            {
                m_DirtyUIList.Add(uiGO);
                m_OpenedUIsList.Remove(uiGO);
            }
        }

        #endregion


        public void ShowUI(string uiName)
        {
            C_BaseUI baseUI = GetUI<C_BaseUI>(uiName);
            if (baseUI != null)
                baseUI.ShowUI();
        }

        public void HideUI(string uiName)
        {
            C_BaseUI baseUI = GetUI<C_BaseUI>(uiName);
            if (baseUI != null)
                baseUI.HideUI();
        }

        public void AddOpenedUIsList(GameObject uiGO)
        {
            foreach (GameObject go in m_OpenedUIsList)
            {
                if (go == uiGO)
                    return;
            }

            m_OpenedUIsList.Add(uiGO);
        }
        
    }
}