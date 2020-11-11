using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using XBL.Core;
using DG.Tweening;
public class BaseEvent : MonoBehaviour
{
    protected BaseEvent()
    {
    }
    public delegate void BaseAction();
    BaseAction Action;
    public void AddEvent(BaseAction action)
    {
        Action += action;
    }
    public void RemoveEvent(BaseAction action)
    {
        Action -= action;
    }
    public void DealEvent()
    {
        if (Action != null)
            Action();
    }
}
//视口区域
struct WindowCenter
{
    public WindowCenter(float l, float r)
    {
        left = l;
        right = r;
    }
    public float left;
    public float right;
}
public class WindowSliderControl : MonoSingleton<WindowSliderControl>
{
    public Camera _Camera;
    public bool isTouchingMoveCamera = false;
    float Cameraboundary = 16.0f;
    float SliderSpeed = 8f;
    WindowCenter center = new WindowCenter(0.33f, 0.66f);
    WindowCenter outview = new WindowCenter(0.2f, 0.8f);
    Vector2 startTouchpos;
    Vector2 endTouchpos;
    Vector2 movingTouchpos;
    private bool frozen = false;
    bool getInitPos = false;
    float initTimer = 0;
    float initPos = 0;
    float endPos = 0;
    float bufferPos = 0.2F;
    float startTime;
    float endTime;
    float moveSpeed = 1;
    bool canMove = false;
    bool canBuffer = true;
    Tween overTween;
    Tween backTween;
    int UpdateIndex;
    WindowSliderRotate wdRotate;
    WindowSliderBoundary wdBundary;
  
    public void InitCharacter(Transform actor, Camera camera)
    {
        frozen = false;
        _Camera = camera;
        wdRotate = new WindowSliderRotate(camera);
        wdBundary = new WindowSliderBoundary(_Camera);
    }
    public void InitCharacter(Camera camera)
    {
        frozen = false;
        _Camera = camera;
        wdRotate = new WindowSliderRotate(camera);
        wdBundary = new WindowSliderBoundary(_Camera);
    }
    public bool GetisTouchingMoveCamera()
    {
        return isTouchingMoveCamera;
    }
    public Vector3 GetCameraPos()
    {
        if (_Camera != null)
            return _Camera.transform.position;
        return Vector3.zero;
    }
    void Update()
    {
        if (frozen)
        {
            return;
        }
        Update_DetectCameraMove();
        if(wdRotate!=null)
        wdRotate.UpdateRotate();
    }


    public void DFrozenCamera()
    {
        frozen = true;
    }
    public void ReleaseCamera()
    {
        frozen = false;
    }

    /// <summary>
    /// 视野之外
    /// </summary>
    /// <param name="objTransform"></param>
    /// <returns></returns>
    public bool OutView(Transform objTransform)
    {
        if (_Camera == null)
        {
            return false;
        }
        Vector2 viewPos = _Camera.WorldToViewportPoint(objTransform.position);
        if (viewPos.x < outview.left | viewPos.x > outview.right)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 在视野中间1/3位置
    /// </summary>
    /// <param name="objTransform"></param>
    /// <returns></returns>
    public bool IsInCenterView(Transform objTransform)
    {
        Vector2 viewPos = _Camera.WorldToViewportPoint(objTransform.position);
        if (viewPos.x < center.right && viewPos.x > center.left)
        {
            return true;
        }
        return false;
    }
    bool isBoundaryRight = false;
    bool isBoundaryLeft = false;

    /// <summary>
    /// 检测相机移动
    /// </summary>
    void Update_DetectCameraMove()
    {
        Vector2 deltaPos = Vector2.zero;
        if (_Camera == null)
        {
            return;
        }
        if (TouchManager.Instance.IsTouchValid(0))
        {
            TouchPhaseEnum phase = (TouchPhaseEnum)TouchManager.Instance.GetTouchPhase(0);
            if (phase == TouchPhaseEnum.BEGAN)
            {
                ReInit();
                TouchManager.Instance.GetTouchPos(0, out startTouchpos);
                startTime = Time.realtimeSinceStartup;
                isTouchingMoveCamera = true;
                overTween.Pause();
                if (_Camera.transform.position.x <= -16f)
                {
                    isBoundaryRight = true;
                }
                else if (_Camera.transform.position.x >= 16f)
                {
                    isBoundaryLeft = true;
                }
                else
                {
                    isBoundaryRight = false;
                    isBoundaryLeft = false;
                }
            }
            else if (phase == TouchPhaseEnum.MOVED)
            {
                TouchManager.Instance.GetTouchDeltaPos(0, out movingTouchpos);
                if (isBoundaryRight)
                {
                    if (movingTouchpos.x < 0)
                    {
                        wdBundary.MoveBoundaryRight();
                    }
                    else
                    {
                        NormalMove();
                    }
                }
                else if (isBoundaryLeft)
                {
                    if (movingTouchpos.x > 0)
                    {
                        wdBundary.MoveBoundaryLeft();
                    }
                    else
                    {
                        NormalMove();
                    }
                }
                else
                {
                    NormalMove();
                }
            }

            else if (phase == TouchPhaseEnum.ENDED)
            {
                endTime = Time.realtimeSinceStartup;
                isTouchingMoveCamera = false;
                getInitPos = false;
                if (Mathf.Abs(_Camera.transform.position.x) > 16)
                {
                    wdBundary.AutoBack();
                }
                else
                {
                    BufferMove();
                }
            }
        }
    }
    void ReInit()
    {
        if (backTween != null)
        {
            if (backTween.IsPlaying())
            {
                backTween.Pause();
                if (_Camera.transform.position.x > 0)
                {
                    _Camera.transform.position = new Vector3(16, _Camera.transform.position.y, _Camera.transform.position.z);
                }
                else
                {
                    _Camera.transform.position = new Vector3(-16, _Camera.transform.position.y, _Camera.transform.position.z);
                }
            }
        }
    }
    /// <summary>
    /// 正常区间范围移动[-16,16]
    /// </summary>
    void NormalMove()
    {
        isTouchingMoveCamera = true;
        TouchManager.Instance.GetTouchDeltaPos(0, out movingTouchpos);
        CalculateSliderSpeed(movingTouchpos.x, Time.realtimeSinceStartup);
        if (Mathf.Abs(movingTouchpos.x) < 10)
            return;
        if (movingTouchpos.x > 0)//手指右滑动
        {
            //Debug.LogError("右边滑动");
            if (Math.Abs(_Camera.transform.position.x + movingTouchpos.x * SliderSpeed * 0.001F) >= Cameraboundary)//位移超标
            {
                if (_Camera.transform.position.x > 0)
                {
                    Tween tween = _Camera.transform.DOMoveX(Cameraboundary, 0.1f);
                    tween.SetEase(Ease.Linear);
                    canBuffer = false;
                }
            }
            else
            {
                canBuffer = true;
                float targetX = movingTouchpos.x * SliderSpeed * 0.001F;
                Vector3 targetPos = new Vector3(targetX, 0, 0);
                if (_Camera.transform.position.x + targetX < 16)
                    _Camera.transform.position += new Vector3(targetX, 0, 0);
                else
                    _Camera.transform.position += new Vector3(-0.1f, 0, 0);
            }
            canMove = true;
        }
        else
        {
            if (Math.Abs(_Camera.transform.position.x + movingTouchpos.x * SliderSpeed * 0.001F) >= Cameraboundary)//位移超标
            {
                if (_Camera.transform.position.x < 0)
                {
                    Tween tween = _Camera.transform.DOMoveX(-Cameraboundary, 0.1f);
                    tween.SetEase(Ease.Linear);
                    canBuffer = false;
                }
            }
            else
            {
                canBuffer = true;
                float targetX = movingTouchpos.x * SliderSpeed * 0.001F;
                Vector3 targetPos = new Vector3(targetX, 0, 0);
                if (_Camera.transform.position.x + targetX < 16)
                    _Camera.transform.position += new Vector3(targetX, 0, 0);
                else
                    _Camera.transform.position += new Vector3(0.1f, 0, 0);
            }
            canMove = true;
        }


    }
    /// <summary>
    /// 动态计算滑动速度
    /// </summary>
    /// <param name="init_Pos"></param>
    /// <param name="init_Timer"></param>
    void CalculateSliderSpeed(float init_Pos, float init_Timer)
    {
        if (!getInitPos)
        {
            this.initTimer = init_Timer;
            getInitPos = true;
            this.initPos = init_Pos;
        }
        if (Time.frameCount % 3 == 0)
        {
            SliderSpeed = 7F;
            TouchManager.Instance.GetTouchDeltaPos(0, out movingTouchpos);
            endPos = movingTouchpos.x;
            float moveDistance = Mathf.Abs(endPos - this.initPos);
            if (Time.realtimeSinceStartup - this.initTimer > 0 && moveDistance > 0)
            {
                float moveDate = moveDistance / (Time.realtimeSinceStartup - this.initTimer);
                int date = (int)moveDate / 1000;
                if (date >= 1)
                {
                    SliderSpeed = 7 + date * 0.5f;
                    if (SliderSpeed >= 10)
                        SliderSpeed = 10;
                }
                else
                {
                    SliderSpeed = 7F;
                }
            }
            getInitPos = false;
        }
    }
    /// <summary>
    /// 缓存计算
    /// </summary>
    void BufferMove()
    {
        if (!canMove)
            return;
        canMove = false;
        if (!canBuffer)
            return;
        //if (Mathf.Abs(_Camera.transform.position.x) >= Cameraboundary)
        //    return;
        TouchManager.Instance.GetTouchPos(0, out endTouchpos);
        //缓冲量计算=移动速度*移动距离
        float movedistance = Mathf.Abs(endTouchpos.x - startTouchpos.x);//距离
        float timer = endTime - startTime;
        float bufferCount = (movedistance / (timer * timer)) * 0.00008f;//0.65  1.3  2 
        if ((endTouchpos.x - startTouchpos.x) > 0)//右边滑动
        {
            if (bufferCount >= 2.0f)
                bufferCount = 2.0f + bufferCount * 0.1F;
            else if (bufferCount <= 0.25f)
                bufferCount = 0.25f;
            Vector3 targetPos = _Camera.transform.position + new Vector3(bufferCount, 0, 0);
            overTween = _Camera.transform.DOMove(targetPos, 0.85f);
            overTween.SetEase(Ease.OutCirc);
        }
        else
        {
            if (bufferCount >= 2.0f)
                bufferCount = 2.0f + bufferCount * 0.1F;
            else if (bufferCount <= 0.25f)
                bufferCount = 0.25f;
            Vector3 targetPos = _Camera.transform.position - new Vector3(bufferCount, 0, 0);
            overTween = _Camera.transform.DOMove(targetPos, 1.2f);
            overTween.SetEase(Ease.OutCirc);
        }
    }
}
class WindowSliderBoundary
{
    Camera _Camera;
    Vector2 movingTouchpos;
    bool isBoundaryRight;
    bool isBoundaryLeft;
    Tween backTween;
    public WindowSliderBoundary(Camera cam)
    {
        _Camera = cam;
    }
    public void MoveBoundaryRight()
    {
        TouchManager.Instance.GetTouchDeltaPos(0, out movingTouchpos);
        if (movingTouchpos.x < 0)//手指右滑动 -16
        {
            if (Math.Abs(_Camera.transform.position.x + movingTouchpos.x * 1.5f * 0.001F) <= 17)//位移超标
            {
                float targetX = movingTouchpos.x * 1.5f * 0.001F;
                Vector3 targetPos = new Vector3(targetX, 0, 0);
                _Camera.transform.position += new Vector3(targetX, 0, 0);
            }
        }
    }
    public void MoveBoundaryLeft()
    {
        TouchManager.Instance.GetTouchDeltaPos(0, out movingTouchpos);
        if (movingTouchpos.x > 0)//手指右滑动 -16
        {
            if (Math.Abs(_Camera.transform.position.x + movingTouchpos.x * 1.5f * 0.001F) <= 17)//位移超标
            {
                float targetX = movingTouchpos.x * 1.5f * 0.001F;
                Vector3 targetPos = new Vector3(targetX, 0, 0);
                _Camera.transform.position += new Vector3(targetX, 0, 0);
            }
        }
    }
    public void AutoBack()
    {
        isBoundaryRight = false;
        isBoundaryLeft = false;
        if (_Camera.transform.position.x > 16)
        {
            backTween = _Camera.transform.DOMoveX(16, 0.2f).OnComplete(
                delegate
                {
                    _Camera.transform.position = new Vector3(16, _Camera.transform.position.y, _Camera.transform.position.z);
                }
                );
        }
        else
        {
            backTween = _Camera.transform.DOMoveX(-16, 0.2f).OnComplete(
                   delegate
                   {
                       _Camera.transform.position = new Vector3(-16, _Camera.transform.position.y, _Camera.transform.position.z);
                   });
        }
    }
}
class WindowSliderRotate
{
    Camera _Camera;
    Vector2 startTouchpos;
    Vector2 movingTouchpos;
    float SliderSpeed = 1;
    public WindowSliderRotate(Camera cam)
    {
        _Camera = cam;
    }
    public void UpdateRotate()
    {
        if (TouchManager.Instance.IsTouchValid(0))
        {
            TouchPhaseEnum phase = (TouchPhaseEnum)TouchManager.Instance.GetTouchPhase(0);
            if (phase == TouchPhaseEnum.BEGAN)
            {
                TouchManager.Instance.GetTouchPos(0, out startTouchpos);
            }
            else if (phase == TouchPhaseEnum.MOVED)
            {
                NormalRotate();
            }
            else if (phase == TouchPhaseEnum.ENDED)
            {
                AutoRotateBack();
            }
        }
    }
    public void NormalRotate()
    {
        if (_Camera == null)
            return;
        TouchManager.Instance.GetTouchDeltaPos(0, out movingTouchpos);
        if (movingTouchpos.y < 0)//手指上滑动
        {
            float targetX = movingTouchpos.y * SliderSpeed * 0.004F;
            _Camera.transform.eulerAngles += new Vector3(targetX, 0, 0);
        }
        else
        {
            float targetX = movingTouchpos.y * SliderSpeed * 0.004F;
            _Camera.transform.eulerAngles += new Vector3(targetX, 0, 0);
        }
    }
    public void AutoRotateBack()
    {
        Vector3 initRotate = new Vector3(358, 180, 0);
        if(_Camera!=null)
        _Camera.transform.DORotate(initRotate, 0.25f);
    }
}





