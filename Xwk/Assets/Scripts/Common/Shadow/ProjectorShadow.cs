using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ProjectorShadow : MonoBehaviour
{
    public static ProjectorShadow Instance
    {
        get
        {
            if (_Ins == null)
            {
                Projector projector = GameObject.FindObjectOfType<Projector>();
                if (projector == null)
                {
                    //创建一个projector
                    GameObject shadow = Resources.Load("Shadow/Projector") as GameObject;
                    _Ins = Instantiate(shadow).GetComponent<ProjectorShadow>();
                }
                else
                {
                    _Ins = projector.gameObject.GetComponent<ProjectorShadow>();
                }
                if (_Ins == null)
                {
                    Debug.LogError("Shadow/Projector not ProjectorShadow component ");
                }
            }
            return _Ins;
        }
    }
    private static ProjectorShadow _Ins;
     
    public float mProjectorSize = 23;

    public int mRenderTexSize = 2048;

    public LayerMask mLayerCaster;

    public LayerMask mLayerIgnoreReceiver;

    public Transform mFollowObj;

    private bool mUseCommandBuf = false;

    private Projector mProjector;

    private Camera mShadowCam;

    private RenderTexture mShadowRT;

    private CommandBuffer mCommandBuf;

    private Material mReplaceMat;

    private List<GameObject> _RoleList = new List<GameObject>();

    private Bounds bounds;
    #region 内置函数
    // Use this for initialization
    void Start ()
    {
        // 创建render texture
        mShadowRT = new RenderTexture(mRenderTexSize, mRenderTexSize, 0, RenderTextureFormat.R8);
        mShadowRT.name = "ShadowRT";
        mShadowRT.antiAliasing = 1;
        mShadowRT.filterMode = FilterMode.Bilinear;
        mShadowRT.wrapMode = TextureWrapMode.Clamp;

        //projector初始化
        mProjector = GetComponent<Projector>();
        if (mProjector == null)
        {
            return;
        }
        mProjector.orthographic = true;
        mProjector.orthographicSize = mProjectorSize;
        mProjector.ignoreLayers = mLayerIgnoreReceiver;
        mProjector.material.SetTexture("_ShadowTex", mShadowRT);

        //camera初始化
        mShadowCam = gameObject.AddComponent<Camera>();
        mShadowCam.clearFlags = CameraClearFlags.Color;
        mShadowCam.backgroundColor = Color.black;
        mShadowCam.orthographic = true;
        mShadowCam.orthographicSize = mProjectorSize;
        mShadowCam.depth = -100.0f;
        mShadowCam.nearClipPlane = mProjector.nearClipPlane;
        mShadowCam.farClipPlane = mProjector.farClipPlane;
        mShadowCam.targetTexture = mShadowRT;

        mUseCommandBuf = true;
        SwitchCommandBuffer();

        GameObject light = GameObject.FindGameObjectWithTag("MainLight");
        if (light != null)
        {
            transform.localRotation = light.transform.localRotation;
        }
        else
        {
            transform.localRotation = Quaternion.Euler(new Vector3(50f, -50f, 0));
        }
    }
	public void SetShadow(GameObject actor)
    {
         
        if (actor != null &&
            (actor.name.ToLower().Contains("@mesh")
            || actor.name.ToLower().Contains("_parentnode")))
        {
            //Transform go = Utility.FindChild(actor.transform, "Bip001_PZT");
            //if (go == null)
            //{
            //    Debug.LogError("对象：" + actor.transform.name + "没有Bip001_PZT");
            //    go = actor.transform;
            //}

            Transform go = actor.transform;

            SkinnedMeshRenderer meshRenderer = actor.GetComponentInChildren<SkinnedMeshRenderer>();
            if(meshRenderer!=null)
                InitRole(meshRenderer.gameObject);

            SetFllowOjbect(go);
        }
    }
	// Update is called once per frame
	void Update ()
    {
        // 测试Commander Buffer
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    mUseCommandBuf = !mUseCommandBuf;
        //    SwitchCommandBuffer();
        //}
        // 填充Commander Buffer
        if (mUseCommandBuf)
        {
            FillCommandBuffer();
        }
    }

    private void LateUpdate()
    {
        if(mFollowObj != null)
        {
            //Transform go = Utility.FindChild(mFollowObj.transform, "Bip001_PZT");
            //if (go == null)
            //{
            //    Debug.LogError("对象：" + mFollowObj.transform.name + "没有Bip001_PZT");
            //    return;
            //}
            transform.position = mFollowObj.position - transform.forward * 50.0f;

        }
    }

    #endregion  

    #region 函数

    private void SwitchCommandBuffer()
    {
        Shader replaceshader = Shader.Find("ProjectorShadow/ShadowCaster");
      //  Debug.Log("replaceshader :"+ replaceshader);
        if (!mUseCommandBuf)
        {
            mShadowCam.cullingMask = mLayerCaster;

            mShadowCam.SetReplacementShader(replaceshader, "RenderType");
        }
        else
        {
           // mShadowCam.cullingMask = 0;
            mShadowCam.cullingMask = 2;

            mShadowCam.RemoveAllCommandBuffers();
            if (mCommandBuf != null)
            {
                mCommandBuf.Dispose();
                mCommandBuf = null;
            }
            
            mCommandBuf = new CommandBuffer();
            mShadowCam.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, mCommandBuf);

            if (mReplaceMat == null)
            {
                mReplaceMat = new Material(replaceshader);
                mReplaceMat.hideFlags = HideFlags.HideAndDontSave;
            }
        }
    }
    public void SetFllowOjbect(Transform cam)
    {
        mFollowObj = cam;
    }
    public void InitRole(GameObject obj)
    {
        if (_RoleList != null && !_RoleList.Contains(obj))
        {
            _RoleList.Add(obj);
        }
    }

    private void FillCommandBuffer()
    {
        if (mCommandBuf == null)
            return;
        mCommandBuf.Clear();

        Plane[] camfrustum = GeometryUtility.CalculateFrustumPlanes(mShadowCam);
        if (_RoleList  == null)
        {
            return;
        }
        int j = 0;
        foreach (var go in _RoleList)
        {
            j++;
            if (go == null)
                continue;
            if (!go.gameObject.activeInHierarchy)
            {
                continue;
            }
            Bounds tempBounds = new Bounds();
            Renderer[]  renderers =  go.GetComponentsInChildren<Renderer>();
        
            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer each = renderers[i];

                if (i == 0)
                {
                    tempBounds = each.bounds;
                }
                else
                {
                    tempBounds.Encapsulate(each.bounds);
                }

            }
           // tempBounds.Encapsulate(go.GetComponentInChildren<Renderer>().bounds);

            bounds = tempBounds;
            bool bound = GeometryUtility.TestPlanesAABB(camfrustum, tempBounds);
            if (!bound)
                continue;

            Renderer[] renderlist = go.GetComponentsInChildren<Renderer>();
            if (renderlist.Length <= 0)
                continue;
            foreach(var render in renderlist)
            {
                if (render == null)
                    continue;

                mCommandBuf.DrawRenderer(render, mReplaceMat);
            }           
        }
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(bounds.center, bounds.size);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(bounds.center, 0.1f);
    }
#endif
    #endregion
}
