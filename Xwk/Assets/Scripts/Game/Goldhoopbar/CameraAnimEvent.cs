using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Xbl; 
/// <summary>
/// 该类需要添加到Camera动物Prefab上
/// </summary>
public class CameraAnimEvent : MonoBehaviour {    
    /// <summary>
    /// 动画帧事件，代码添加后自动执行
    /// </summary>
    /// <param name="startFrame"></param>
    /// <param name="endFrame"></param>
    /// <param name="targetFov"></param>
    void FovSetting(string parm)
    {
        //解析framRange
        string[] range = parm.Split('X');//0x100x40
        if (range.Length < 3)
        {
            Debug.Log("输入格式不对");
            return;
        }
        //Debug.LogError(int.Parse(range[0]) + "----" + int.Parse(range[1]));
        CameraFovSetting.Instance.SetCameraFov(GoldhoopbarManager.Instance.getCamera(),int.Parse(range[0]), int.Parse(range[1]), int.Parse(range[2]));
    }
    //火眼金睛相机Fov事件
    void FovSetting_Piercingeye(string parm)
    {
        //解析framRange
        string[] range = parm.Split('X');//0x100x40
        if (range.Length < 3)
        {
            Debug.Log("输入格式不对");
            return;
        }
        //Debug.LogError(int.Parse(range[0]) + "----" + int.Parse(range[1]));
        CameraFovSetting.Instance.SetCameraFov(PiercingeyeManager.Instance.getCamera(),int.Parse(range[0]), int.Parse(range[1]), int.Parse(range[2]));
    }
    void GotoFindHulu_Piercingeye(string parm)
    {
        PiercingeyeManager.Instance.GotoState("FireGameState");
    }
}
