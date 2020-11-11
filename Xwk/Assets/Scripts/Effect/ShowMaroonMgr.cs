using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.XWK.MainScene;

public class ShowMaroonMgr : MonoBehaviour {
    private GameObject _MaroonIdle;
    private GameObject _MaroonBoom;
    private Camera _ActorCamera;
    private bool _Pause = false;
    private float totalTime;
    private C_Event _GameEvent = new C_Event();
    private bool TouchFlag = false;
    // Use this for initialization
    void Start () {
        _MaroonIdle = transform.Find("public_effect_bpdl_changtai").gameObject;
        BoxCollider boxCollider = _MaroonIdle.GetAddComponent<BoxCollider>();
        boxCollider.center = new Vector3(0.43f, 0, 1.6f);
        boxCollider.size = new Vector3(1.06f, 1.02f, 3.05f);
        _MaroonBoom = transform.Find("public_effect_bpdl_bao").gameObject;
        _MaroonIdle.gameObject.SetActive(true);
        _MaroonBoom.gameObject.SetActive(false);
        TouchFlag = true;
        totalTime = 0;

        if (_GameEvent!=null)
        {
            _GameEvent.UnregisterEvent();
        }
        _GameEvent.RegisterEvent(C_EnumEventChannel.Global, "MainGameEvent", (object[] result) => {
            if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_RESUME_GAME
            || (GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_RESUME_GAME_RESUME_FALSE)
            {
                TouchFlag = true;
            }
            else if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_ENTER_WIZARD
            || ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_PAUSE_GAME)
            || ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_PAUSE_GAME_FALSE))
            {
                TouchFlag = false;
            }
        });
    }
	void FindCamera()
    {
        if (_ActorCamera!=null)
        {
            return;
        }
        GameObject camera = GameObject.FindGameObjectWithTag("ActorCamera");
        if (camera != null)
        {
            _ActorCamera = camera.GetComponent<Camera>();
        }
    }
	// Update is called once per frame
	void Update () {
        //if (_ActorCamera != null)
        //{
        //    return;
        //}
     
        if (_Pause)
        {
            totalTime += Time.deltaTime;
            if (totalTime > 25f)
            {
                totalTime = 0;
                _Pause = false;
                _MaroonIdle.SetActive(true);
                _MaroonBoom.SetActive(false);
            }
            return;
        }
        if (!TouchFlag)
        {
            return;
        }
        if (Application.isMobilePlatform)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    TouchHandle(touch.position);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                TouchHandle(Input.mousePosition);
            }
        }
    }

    void TouchHandle(Vector2 startTouchpos)
    {
        FindCamera();
        if (_ActorCamera==null || (_ActorCamera!=null && _ActorCamera.enabled == false))
        {
            return;
        }
        RaycastHit hit;
        Ray ray;
        ray = _ActorCamera.ScreenPointToRay(startTouchpos);
        if (Physics.Raycast(ray, out hit, 1000) && hit.collider != null)
        {
            GameObject obj = hit.collider.gameObject;
            if (obj != null)
            {
                string name = obj.name;
                if (name.Equals(_MaroonIdle.name))
                {
                    C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "main_event", "scenemaroon");

                    _MaroonBoom.SetActive(true);
                    _MaroonIdle.SetActive(false);
                    totalTime = 0;
                    _Pause = true;
                }
            }
        }
    }
    private void OnDestroy()
    {
        if (_GameEvent != null)
        {
            _GameEvent.UnregisterEvent();
        }
    }
}
