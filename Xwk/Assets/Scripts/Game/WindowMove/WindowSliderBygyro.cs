using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class WindowSliderBygyro : MonoBehaviour{
    public static WindowSliderBygyro Instance;
    public float MoveFactor;
    float CameraRotationRangeX=15;
    float CameraRotationRangeY=60;
    bool haveInit=false;
    bool rotateByGyro = false;
    float initY;
    Vector3 Tragetpos;
    GameObject centerTarget;
    Camera cam;
    void Awake()
    {
        Instance = this;
        centerTarget = new GameObject("centerTarget");
        centerTarget.transform.parent = this.transform;
        centerTarget.transform.localPosition = new Vector3(0, -120, 750);
        centerTarget.transform.localScale = Vector3.one;
        cam = this.GetComponent<Camera>();
    }
    void Start()
    {
      
     }
    public Vector3 GetViewCenterPos()
    {
        if (centerTarget)
            return centerTarget.transform.position;
        else
            return Vector3.zero;
    }
    /// <summary>
    /// 退出的时候记得移除事件
    /// </summary>
    void RemoveEvent()
    {
    }
    /// <summary>
    /// 火眼金睛开始之后初始化
    /// </summary>
    public void InitGyro()
    {
        if (centerTarget)
            centerTarget.SetActive(true);
        Input.gyro.enabled = true;
        rotateByGyro = true;
        haveInit = true;
    }
    public Camera GetGyroCam()
    {
        return cam;
    }
    public void CancelGyro()
    {
        rotateByGyro = false;
    }
    void Update()
    {
        if(rotateByGyro)
        GyroModifyCamera();     
    }
    void LateUpdate()
    {
        if (haveInit)
        {
            haveInit = false;
            initY = Mathf.Abs(Input.gyro.gravity.y);//不能放在Start里面，同时Input.gyro.enabled输出一直是false
            //Debug.LogError("初始化位置:" + initY);
            if (initY == 0)//如果是0 强制重新赋值
            {
                haveInit = true;
            }
        }
    }  
    //void OnGUI()
    //{
    //    //GUI.skin.label.fontSize = Screen.width / 40;
    //    //GUILayout.Label("input.gyro.gravity: " + Input.gyro.gravity);
    //}
    float gyroY = 0;
    void GyroModifyCamera()
    {
        //计算方式
        //X轴旋转 ，对于陀螺仪[-1,1]重力感应Y方向取值范围，获取初始值initY
        //Y轴旋转 ，对于陀螺仪[-1,1] 重力感应X方向取值范围
       // Debug.LogError("当前上下移动范围::"+CameraRotationRangeX * (Input.gyro.gravity.y + initY));//0.75
        if (Application.isEditor)
        {
            if (true)
                return;
            initY = 0;
            if(gyroY>=1.0f)
            {
                gyroY = -1f;
            }
            else
            {
                gyroY += Time.deltaTime*0.3f;
            }
            Tragetpos = new Vector3(-2.332f + CameraRotationRangeX * (gyroY + initY), 180 - (CameraRotationRangeY * Input.gyro.gravity.x), transform.eulerAngles.z);
        }
        else
        {
            float Xrotate = -2.332f + CameraRotationRangeX * (Input.gyro.gravity.y + initY);
            float Yrotate = 180 - (CameraRotationRangeY * Input.gyro.gravity.x);
            if (Xrotate >= 7)
                Xrotate = 7;
            if (Xrotate <= -7)
                Xrotate = -7;
            Tragetpos = new Vector3(Xrotate, Yrotate, transform.eulerAngles.z);
        }
        transform.DORotate(Tragetpos,Time.deltaTime);      
    }     
}