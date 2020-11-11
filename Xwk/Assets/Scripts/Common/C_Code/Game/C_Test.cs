using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class C_Test : MonoBehaviour
{
    //[SerializeField]
    //private InputField m_KnowInputField = null;
    //[SerializeField]
    //private InputField m_ReadInputField = null;
    //[SerializeField]
    //private InputField m_WriteInputField = null;
    //[SerializeField]
    //private InputField m_TestInputField = null;

    void Start()
    {
        //C_MonoSingleton<ExploreMgr>.GetInstance().InitExplore(()=>
        //{
        //    C_MonoSingleton<ExploreMgr>.GetInstance().SetExplore(10, 0, null, null);
        //}, null);

        //m_KnowInputField.text = "700";
        //m_ReadInputField.text = "700";
        //m_WriteInputField.text = "700";
        //m_TestInputField.text = "700";

        #region UI Test

        //C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_Login");
        //C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_Register");

        #endregion


        #region Download Test

        //C_WWWDownloader loader = new C_WWWDownloader();
        //loader.DownloadFile("http://udata.youban.com/xbl3d/TESTPNG.png", C_LocalPath.HotUpdatePath, (byte[] res) =>
        //{
        //    Debug.Log(res.Length);
        //});

        //C_UnityWebRequestDownloader loader = new C_UnityWebRequestDownloader();
        //loader.DownloadFile("http://udata.youban.com/xbl3d/TESTPNG.png", C_LocalPath.HotUpdatePath, (byte[] res) =>
        //{
        //    Debug.Log(res.Length);
        //});

        //C_HttpDownloader loader = new C_HttpDownloader();
        //loader.DownloadFile("http://udata.youban.com/xbl3d/TESTPNG.png", C_LocalPath.HotUpdatePath, () =>
        //{
        //    Debug.Log("1111111111111111111111111");
        //});

        #endregion


        #region GameResMgr Test

        //GameObject xbl = C_Singleton<GameResMgr>.GetInstance().LoadResource<GameObject>("public_model_XBL@mesh", "public", "model", true);
        //xbl.transform.SetParent(this.transform);

        //Animator animator = xbl.GetComponent<Animator>();

        //RuntimeAnimatorController animatorController = C_Singleton<GameResMgr>.GetInstance().LoadResource<RuntimeAnimatorController>("xbl_cam039@mesh_animatorcontroller", "iuv", "AnimatorController");

        //AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animatorController);

        //animator.runtimeAnimatorController = animatorOverrideController;

        //AnimationClip animationClip = C_Singleton<GameResMgr>.GetInstance().LoadResource<AnimationClip>("XBL_run02@anim", "iuv", "anim");
        //animatorOverrideController["XBL_error@anim"] = animationClip;

        //AnimationClip animationClip1 = C_Singleton<GameResMgr>.GetInstance().LoadResource<AnimationClip>("XBL_skill1@anim", "public", "anim");
        //animatorOverrideController["XBL_happy@anim"] = animationClip1;

        //animator.SetBool("anim", true);


        //测试存储AssetBundle
        //C_Singleton<GameResMgr>.GetInstance().LoadResource<GameObject>("public_model_XBL@mesh", "public", "model", "", false, true);

        #endregion


        #region GameHotUpdate Test

        //bool update = C_MonoSingleton<GameHotUpdate>.GetInstance().HaveStageHotUpdate("iuv");
        //if (update)
        //    C_MonoSingleton<GameHotUpdate>.GetInstance().DownloadStageHotUpdate("iuv");

        #endregion

        //Invoke("InvokeTest", 5.0f);

        //C_MonoSingleton<da_qi_qiu_mgr>.GetInstance();
        //C_MonoSingleton<bai_qi_qiu_mgr>.GetInstance();
    }

    private void InvokeTest()
    {
        //C_Singleton<GameResMgr>.GetInstance().UnloadResource("public_model_XBL@mesh", "public", "model");
    }

    void Update()
    {
        //        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        //        {
        //#if IPHONE || ANDROID
        //			if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        //#else
        //            if (EventSystem.current.IsPointerOverGameObject())
        //#endif
        //                Debug.Log("当前触摸在UI上");

        //            else
        //                Debug.Log("当前没有触摸在UI上");
        //        }

        //Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit mHi;
        //if (Physics.Raycast(mRay, out mHi))             //判断是否击中了什么
        //{
        //    Debug.Log("11111111111111111");
        //    if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        //    {
        //        Debug.Log("222222222222222222222222");
        //        if (Vector3.Distance(this.transform.position, mHi.transform.position) < 10)
        //        {
        //            Debug.Log("33333333333333333333333");
        //            if (mHi.collider.gameObject.CompareTag("Player"))//用的是tag进行辨别
        //            {
        //                Debug.Log("4444444444444");
        //                //mHi.collider.gameObject.GetComponent<NPCbasic>().OnMouse();
        //            }
        //        }
        //    }
        //}
    }
    
    public void CheckButton()
    {
        //C_MonoSingleton<ExploreMgr>.GetInstance().StartExplore();

        //int knowScore = 0;
        //if (!string.IsNullOrEmpty(m_KnowInputField.text))
        //    knowScore = int.Parse(m_KnowInputField.text);

        //int readScore = 0;
        //if (!string.IsNullOrEmpty(m_ReadInputField.text))
        //    readScore = int.Parse(m_ReadInputField.text);

        //int writeScore = 0;
        //if (!string.IsNullOrEmpty(m_WriteInputField.text))
        //    writeScore = int.Parse(m_WriteInputField.text);

        //int testScore = 0;
        //if (!string.IsNullOrEmpty(m_TestInputField.text))
        //    testScore = int.Parse(m_TestInputField.text);

        //C_Singleton<StageMgr>.GetInstance().CompleteStage(C_Singleton<StageMgr>.GetInstance().CurStage, knowScore, readScore, writeScore, testScore);

       // C_Singleton<StageMgr>.GetInstance().StoryEnd(1);
    }
}
