using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.AnimCallbacks;
using XBL.Core;
public class HeadlineAnimationGuide : MonoBehaviour {
    private GameObject _JinDouYun;
    private Animator _JinDouYunAnimator;

    private GameObject _CameraGameObject;
    private Camera _Camera;

    private GameObject _Stone;

    private int _TouchTime = 0;

    private GameObject _XwkActor;
    private Animator _XwkActorAim;

    public System.Action OnFinish;

    private enum HeadlineAnimationGuidelState
    {
        showjindouyun,
        dianjist,
        showover,
    }
    private HeadlineAnimationGuidelState _GoldenCudgelState;


    // Use this for initialization
    void Start () {
        OnInit();
    }
    private void OnInit()
    {
        _GoldenCudgelState = HeadlineAnimationGuidelState.showjindouyun;
        _CameraGameObject = GameResMgr.Instance.LoadResource<GameObject>("headline/prefab/camera", true);
        _Camera = _CameraGameObject.GetComponent<Camera>();
        _Stone = GameResMgr.Instance.LoadResource<GameObject>("headline/prefab/Cube", true);
        _XwkActor = GameResMgr.Instance.LoadResource<GameObject>("public/prefab/public_model_XBL@mesh", true);
        _XwkActorAim = _XwkActor.GetAddComponent<Animator>();
        _XwkActorAim.runtimeAnimatorController = GameResMgr.Instance.LoadResource<RuntimeAnimatorController>("headline/animatorcontroller/jkbguide_xwk", true);
        _XwkActor.gameObject.SetActive(false);

        //播放筋斗云动画
        _JinDouYun = GameResMgr.Instance.LoadResource<GameObject>("public/prefab/public_fbx_A@mesh", true);
        _JinDouYunAnimator = _JinDouYun.GetAddComponent<Animator>();
        _JinDouYunAnimator.runtimeAnimatorController = GameResMgr.Instance.LoadResource<RuntimeAnimatorController>("headline/animatorcontroller/jindouyun");
        _JinDouYunAnimator.AddClipEndCallback("anim", () => {
            //_JinDouYunAnimator.RemoveClipEndCallback("anim",()=> { });
            //开始进入点击cube
            InvokeWarring();
            _GoldenCudgelState = HeadlineAnimationGuidelState.dianjist;
            //出现手特效

        });
        _TouchTime = 0;
        
    }
    void InvokeWarring()
    {
        Invoke("PlayWarrningAudio", 3);

    }
    void PlayWarrningAudio()
    {
        AudioManager.Instance.PlayerSound("public/sound/public_sd_164.ogg",false,()=> {
            InvokeWarring();
        });
    }
    void CancleInvokeWarring()
    {
        CancelInvoke("PlayWarrningAudio");
        AudioManager.Instance.StopPlayerSound();
    }

    // Update is called once per frame
    void Update () {
        switch (_GoldenCudgelState)
        {
            case HeadlineAnimationGuidelState.dianjist:
                {
                    if (TouchManager.Instance.IsTouchValid(0))
                    {
                        TouchPhaseEnum phase = (TouchPhaseEnum)TouchManager.Instance.GetTouchPhase(0);
                        if (phase == TouchPhaseEnum.ENDED)
                        {
                            CancleInvokeWarring();

                            Vector2 touchPos;
                            TouchManager.Instance.GetTouchPos(0, out touchPos);
                            RaycastHit hit;
                            Ray ray = _Camera.ScreenPointToRay(touchPos);
                            if (Physics.Raycast(ray, out hit, 1000) && hit.collider != null)
                            {
                                GameObject obj = hit.collider.gameObject;
                                if (obj != null & obj.name.Equals("Cube(Clone)"))
                                {
                                    _TouchTime++;
                                    if (_TouchTime > 5)
                                    {
                                        //出现特效
                                        _GoldenCudgelState =  HeadlineAnimationGuidelState.showover;
                                        //出现人
                                        _XwkActor.gameObject.SetActive(true);
                                        _XwkActorAim.SetTrigger("show");
                                        AudioManager.Instance.PlayerSound("public/sound/public_sd_164.ogg",false,()=> {
                                            //进入到显示小名
                                            OnExit();
                                        });
                                    }
                                    else
                                    {
                                        InvokeWarring();
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            default:break;
        }
	}
    public void OnExit()
    {
        //删除对象
        GameObject.DestroyObject(_CameraGameObject);
        GameObject.DestroyObject(_JinDouYun);
        GameObject.DestroyObject(_Stone);
        GameObject.DestroyObject(_XwkActor);
        //释放ab资源

        if (OnFinish!=null)
        {
            OnFinish();
        }
    }
}
