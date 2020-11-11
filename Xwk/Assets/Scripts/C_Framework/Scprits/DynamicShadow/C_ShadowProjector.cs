using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_ShadowProjector : C_MonoSingleton<C_ShadowProjector>
    {
        [SerializeField]
        private Projector m_Projecto = null;

        private Camera m_LightCamera = null;

        private RenderTexture m_ShadowRenderTexture = null;

        private List<GameObject> m_ShadowCasterList = new List<GameObject>();

        private BoxCollider m_BoundsCollider = null;

        [SerializeField]
        private Shader m_ShadowReplaceShader = null;

        public float BoundsOffset = 1.0f;

        void Start()
        {
            if (m_LightCamera == null)
            {
                m_LightCamera = gameObject.AddComponent<Camera>();
                m_LightCamera.orthographic = true;
                m_LightCamera.cullingMask = LayerMask.GetMask("ShadowCaster");
                m_LightCamera.clearFlags = CameraClearFlags.SolidColor;
                m_LightCamera.backgroundColor = new Color(0, 0, 0, 0);

                m_ShadowRenderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
                m_ShadowRenderTexture.filterMode = FilterMode.Bilinear;

                m_LightCamera.targetTexture = m_ShadowRenderTexture;
                m_LightCamera.SetReplacementShader(m_ShadowReplaceShader, "RenderType");

                m_Projecto.material.SetTexture("_ShadowTex", m_ShadowRenderTexture);
                m_Projecto.ignoreLayers = LayerMask.GetMask("ShadowCaster");
            }

            m_BoundsCollider = new GameObject("Test use to show bounds").AddComponent<BoxCollider>();
        }


        void LateUpdate()
        {
            //求阴影产生物体的包围盒
            Bounds b = new Bounds();
            for (int i = 0; i < m_ShadowCasterList.Count; i++)
            {
                if (i == 0)
                {
                    b = m_ShadowCasterList[i].GetComponent<Renderer>().bounds;
                }
                else
                {
                    if (m_ShadowCasterList[i] != null && m_ShadowCasterList[i].GetComponent<Renderer>() != null)
                        b.Encapsulate(m_ShadowCasterList[i].GetComponent<Renderer>().bounds);
                }
                
            }

            b.extents += Vector3.one * BoundsOffset;

#if UNITY_EDITOR
            m_BoundsCollider.center = b.center;
            m_BoundsCollider.size = b.size;
#endif
            
            C_ShadowUtils.SetLightCamera(b, m_LightCamera);

            m_Projecto.aspectRatio = m_LightCamera.aspect;
            m_Projecto.orthographicSize = m_LightCamera.orthographicSize;
            m_Projecto.nearClipPlane = m_LightCamera.nearClipPlane;
            m_Projecto.farClipPlane = m_LightCamera.farClipPlane;
        }

        public void AddShadowCaster(GameObject gOParam)
        {
            if (gOParam == null)
            {
                C_DebugHelper.LogError("C_ShadowProjector AddShadowCaster: gOParam is Null!");
                return;
            }

            gOParam.layer = LayerMask.NameToLayer("ShadowCaster");
            foreach (Transform child in gOParam.transform)
                child.gameObject.layer = LayerMask.NameToLayer("ShadowCaster");


            foreach (GameObject gO in m_ShadowCasterList)
                if (gO == gOParam)
                    return;

            m_ShadowCasterList.Add(gOParam);
        }

        public void RemoveShadowCaster(GameObject gOParam)
        {
            if (gOParam == null)
            {
                C_DebugHelper.LogError("C_ShadowProjector AddShadowCaster: gOParam is Null!");
                return;
            }

            if (gOParam.layer == LayerMask.NameToLayer("ShadowCaster"))
            {
                C_DebugHelper.LogError("C_ShadowProjector AddShadowCaster: gOParam layer != ShadowCaster");
                return;
            }

            for (int i = m_ShadowCasterList.Count - 1; i >= 0; i--)
                if (m_ShadowCasterList[i] == gOParam)
                    m_ShadowCasterList.RemoveAt(i);
        }
    }
}
