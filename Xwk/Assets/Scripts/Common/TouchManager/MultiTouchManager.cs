using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace XBL.Core
{
    public class MultiTouchManager : C_MonoSingleton<MultiTouchManager>
    {
        Vector2 multFingerdeltaPos;
        int movingCount = 0;
        static bool bFirstClick = true;
        static bool ButtonDown = false;
        const int MAX_TOUCHES = 5;
        public static Vector2 PreviousClickPosition;
        public static Vector3 PreviousProjectedClickPosition;
        public const float RAYCAST_MAX_DISTANCE = 20f;
        static TouchItem[] touchCache = new TouchItem[5];
        bool InitState = false;

        public Dictionary<int, Touch> touchCacheDic = new Dictionary<int, Touch>();
        void Update()
        {
            UpDatetouchCacheDic();
            ProcessTouches();
        }

        #region ##公共接口
        /// <summary>
        /// 获取所有手指滑动幅度最大的值
        /// </summary>
        public Vector2 GetTouchMaxDeltaPos()//获取滑动最大值
        {
            if (Application.isEditor)
            {
                Vector2 deltaPos;
                GetTouchDeltaPos(0, out deltaPos);
                return deltaPos;
            }
            else
            {
                if (Input.touches.Length == 1)
                {
                    return new Vector2(Input.touches[0].deltaPosition.x, Input.touches[0].deltaPosition.y);
                }
                else if (Input.touches.Length == 2)
                {
                    if (Mathf.Abs(Input.touches[0].deltaPosition.x) > Mathf.Abs(Input.touches[1].deltaPosition.x))
                    {
                        return new Vector2(Input.touches[0].deltaPosition.x, Input.touches[0].deltaPosition.y);
                    }
                    else
                    {
                        return new Vector2(Input.touches[1].deltaPosition.x, Input.touches[1].deltaPosition.y);
                    }
                }
                else
                {
                    return Vector2.zero;
                }
            }
        }
        public Touch GetMovingTouch()//获取滑动最大值
        {
            //if (Application.isEditor)
            //{
            //    Vector2 deltaPos;
            //    GetTouchDeltaPos(0, out deltaPos);
            //    return deltaPos;
            //}
            //else
            {
                if (Input.touches.Length == 1)
                {
                    return Input.touches[0];
                }
                else if (Input.touches.Length == 2)
                {
                    if (Mathf.Abs(Input.touches[0].deltaPosition.x) > Mathf.Abs(Input.touches[1].deltaPosition.x))
                    {
                        return Input.touches[0];
                    }
                    else
                    {
                        return Input.touches[1];
                    }
                }
                else
                {
                    return new Touch();
                }
            }
        }
        public bool GetFingerMoving()
        {
            if (Application.isEditor)
            {
                Vector2 pos;
                TouchPhaseEnum touch=GetTouchPhase(0);
                if (touch==TouchPhaseEnum.MOVED)
                    return true;
                else
                    return false;
            }
            else
            {
                if (Input.touches.Length > 2)
                    return false;
                else
                {
                    movingCount = 0;
                    for (int i = 0; i < Input.touches.Length; i++)
                    {
                        if (Input.touches[i].phase == TouchPhase.Moved)
                        {
                            movingCount++;
                        }
                    }

                    if (movingCount > 0)
                        return true;
                    else
                        return false;
                }
            }
        }
        public bool GetFingerEnded()//结束状态
        {
            if (touchCacheDic.Count == 0)
            {
                return true;
            }
            else if (touchCacheDic.Count == 1)
            {
                foreach (Touch t in touchCacheDic.Values)
                {
                    if (t.phase == TouchPhase.Ended)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                int total = 0;
                foreach (Touch t in touchCacheDic.Values)
                {
                    if (t.phase == TouchPhase.Ended)
                    {
                        total++;
                    }
                }
                if (total >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 右滑动
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="isSmallLimt"></param>
        /// <returns></returns>
        public bool SwipUpLimitX(int limit, bool isSmallLimt = true)
        {
            return SwipUpLimitXFun(limit, isSmallLimt);
        }

        public bool SwipDownLimitX(int limit, bool isSmallLimt = true)
        {
            return SwipDownLimitXFun(limit, isSmallLimt);
        }


        public bool SwipeRight()
        {
            return SwipRightFun();
        }
        public bool SwipeLeft()
        {
            return SwipLeftFun();
        }
        public bool SwipeUp()
        {
            return SwipUpFun();
        }
        public bool SwipeDown()
        {
            return SwipDownFun();
        }
        #endregion


        #region##内部方法 
        bool SwipUpLimitXFun(int limit, bool isSmallLimt = true)
        {
            if (Application.isEditor)
            {
                return SwipeUp();
            }
            else
            {
                if (GetFingerMoving())
                {
                    if (isSmallLimt)
                    {
                        if (GetMovingTouch().position.x < limit && GetMovingTouch().deltaPosition.y > 10)
                        {
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        if (GetMovingTouch().position.x > limit && GetMovingTouch().deltaPosition.y > 10)
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
            return false;
        }

        bool SwipDownLimitXFun(int limit, bool isSmallLimt = true)
        {
            if (Application.isEditor)
            {
                return SwipeDown();
            }
            else
            {
                if (GetFingerMoving())
                {
                    if (isSmallLimt)
                    {
                        if (GetMovingTouch().position.x < limit && GetMovingTouch().deltaPosition.y < -10)
                        {
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        if (GetMovingTouch().position.x > limit && GetMovingTouch().deltaPosition.y < -10)
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
            return false;
        }

        bool SwipUpFun()
        {
            if (GetFingerMoving())
            {
                if (GetTouchMaxDeltaPos().y > 10)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        bool SwipDownFun()
        {
            if (GetFingerMoving())
            {
                if (GetTouchMaxDeltaPos().y < -10)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        bool SwipRightFun()
        {
            if (GetFingerMoving())
            {
                if (GetTouchMaxDeltaPos().x > 10)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        bool SwipLeftFun()
        {
            if (GetFingerMoving())
            {
                if (GetTouchMaxDeltaPos().x < -10)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        void UpDatetouchCacheDic()
        {
            if (Input.touches.Length == 0)
            {
                touchCacheDic.Clear();
            }
            //时刻记录fingerId
            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (!touchCacheDic.ContainsKey(Input.touches[i].fingerId))
                {
                    touchCacheDic.Add(Input.touches[i].fingerId, Input.touches[i]);
                }
                else
                {
                    touchCacheDic[Input.touches[i].fingerId] = Input.touches[i];
                }
            }
        }
        #endregion


        #region##单点操作 手指点击与模拟

        public bool Dragging(int id)
        {
            if (bFirstClick)
            {
                return false;
            }
            return (((touchCache[id].phase == TouchPhaseEnum.BEGAN) || (touchCache[id].phase == TouchPhaseEnum.MOVED)) || (touchCache[id].phase == TouchPhaseEnum.STATIONARY));
        }

        public bool GetTouchDeltaPos(int id, out Vector2 deltaPos)
        {
            bool flag = Instance.IsTouchValid(id);
            if (flag)
            {
                deltaPos = touchCache[id].deltaPosition;
                return flag;
            }
            deltaPos = Vector2.zero;
            return flag;
        }

        public TouchPhaseEnum GetTouchPhase(int id)
        {
            if (Instance.IsTouchValid(id))
            {
                return touchCache[id].phase;
            }
            return TouchPhaseEnum.INVALID;
        }

        public bool GetTouchPos(int id, out Vector2 pos)
        {
            bool flag = Instance.IsTouchValid(id);
            if (flag)
            {
                pos = touchCache[id].position;
                return flag;
            }
            pos = Vector2.zero;
            return flag;
        }

        public bool IsTouchHappening()
        {
            for (int i = 0; i < 5; i++)
            {
                switch (touchCache[i].phase)
                {
                    case TouchPhaseEnum.BEGAN:
                    case TouchPhaseEnum.MOVED:
                    case TouchPhaseEnum.STATIONARY:
                        return true;
                }
            }
            return false;
        }

        public bool IsTouchValid(int id)
        {
            return (id <= 5);
        }

        public Vector3 MouseClickHit(int id)
        {
            RaycastHit hit;
            if ((touchCache[id].position == PreviousClickPosition) && !bFirstClick)
            {
                return PreviousProjectedClickPosition;
            }
            PreviousClickPosition = touchCache[id].position;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 20f))
            {
                PreviousProjectedClickPosition = hit.point;
            }
            else
            {
                hit.point = PreviousProjectedClickPosition;
            }
            return hit.point;
        }

        public Vector3 TouchClickHit(int id)
        {
            if (id < Input.touches.Length)
            {
                RaycastHit hit;

                Touch touch = Input.GetTouch(id);
                if ((touchCache[id].position == PreviousClickPosition) && !bFirstClick)
                {
                    return PreviousProjectedClickPosition;
                }
                PreviousClickPosition = touchCache[id].position;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(touchCache[id].position), out hit, 20f))
                {
                    PreviousProjectedClickPosition = hit.point;
                }
                else
                {
                    hit.point = PreviousProjectedClickPosition;
                }
            }
            return PreviousProjectedClickPosition;
        }

        public bool IsOverUi()
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                return IsOverUi_Android();
            else
                return IsOverUi_Standalone();
        }

        //PC端判断方法
        public bool IsOverUi_Standalone()
        {
            //  EventSystem.current.IsPointerOverGameObject()
            //  该方法只要鼠标悬浮在UI（带有Image组件即可）上就会有相应
            bool isOverUI = false;
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log("鼠标处于UI上");
                isOverUI = true;
            }
            else
            {
                isOverUI = false;
                //Debug.Log("鼠标不处于UI上");
            }
            return isOverUI;
        }
        //安卓判断方法
        //http://blog.csdn.net/qq_27124771/article/details/54882774
        public bool IsOverUi_Android()
        {
            bool isOverUI = false;
            for (int i = 0; i < Input.touches.Length; i++)
            {
                Touch touch = Input.GetTouch(i);
                int fingerId = touch.fingerId;
                if (fingerId >= 5)
                {
                    fingerId = fingerId % 5;
                }
                if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Moved)
                {
                    //该方法可以判断触摸点是否在UI上
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                    {
                        isOverUI = true;
                        //Debug.Log("鼠标处于UI上");
                    }
                    else
                    {
                        isOverUI = false;
                        //Debug.Log("鼠标不处于UI上");
                    }
                }
            }
            return isOverUI;
        }

        /// <summary>
        ///鼠标模拟 不同的阶段
        /// </summary>
        /// <returns></returns>
        int ProcessMouseEvents()
        {
            if (Input.GetMouseButtonDown(0) && !ButtonDown)
            {
                bFirstClick = false;
                ButtonDown = true;
                touchCache[0].phase = TouchPhaseEnum.BEGAN;
                touchCache[0].position = Input.mousePosition;
                touchCache[0].deltaPosition = Vector2.zero;
            }
            else if (Input.GetMouseButtonUp(0) && ButtonDown)
            {
                UpdatePosition(ref touchCache[0]);
                ButtonDown = false;
                touchCache[0].phase = TouchPhaseEnum.ENDED;
            }
            else if (ButtonDown)
            {
                UpdatePosition(ref touchCache[0]);
            }
            else
            {
                touchCache[0].phase = TouchPhaseEnum.INVALID;
            }
            return Input.touchCount;
        }


        int ProcessRealTouches()
        {

            for (int i = 0; i < Input.touches.Length; i++)
            {
                Touch touch = Input.GetTouch(i);
                int fingerId = touch.fingerId;
                if (fingerId >= 5)
                {
                    fingerId = fingerId % 5;
                }
                touchCache[fingerId].deltaPosition = touch.deltaPosition;
                touchCache[fingerId].position = touch.position;
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        bFirstClick = false;
                        touchCache[fingerId].position = touch.position;
                        touchCache[fingerId].phase = TouchPhaseEnum.BEGAN;
                        ButtonDown = true;
                        break;

                    case TouchPhase.Moved:
                        touchCache[fingerId].phase = TouchPhaseEnum.MOVED;
                        break;

                    case TouchPhase.Stationary:
                        touchCache[fingerId].phase = TouchPhaseEnum.STATIONARY;
                        break;

                    case TouchPhase.Ended:
                        touchCache[fingerId].phase = TouchPhaseEnum.ENDED;
                        ButtonDown = false;
                        break;

                    case TouchPhase.Canceled:
                        touchCache[fingerId].phase = TouchPhaseEnum.CANCELED;
                        break;
                }
            }
            return Input.touchCount;
        }

        public int ProcessTouches()
        {
            if (((Application.platform == RuntimePlatform.IPhonePlayer) || (Application.platform == RuntimePlatform.Android)) && !Application.isEditor)
            {
                return ProcessRealTouches();
            }
            return ProcessMouseEvents();
        }

        public bool Stationary(int id)
        {
            return (((touchCache[id].phase == TouchPhaseEnum.STATIONARY) || (touchCache[id].phase == TouchPhaseEnum.BEGAN)) || (touchCache[id].phase == TouchPhaseEnum.ENDED));
        }



        void UpdatePosition(ref TouchItem touch)
        {
            bFirstClick = false;
            Vector2 mousePosition = Input.mousePosition;
            touch.deltaPosition = mousePosition - touch.position;
            if (touch.deltaPosition.sqrMagnitude > 0f)
            {
                touch.phase = TouchPhaseEnum.MOVED;
            }
            else
            {
                touch.phase = TouchPhaseEnum.STATIONARY;
            }
            touch.position = mousePosition;
        }

        // Properties
        public int touchCount
        {
            get
            {
                return Input.touchCount;
            }
        }
        #endregion##单点操作##单点
    }
}

