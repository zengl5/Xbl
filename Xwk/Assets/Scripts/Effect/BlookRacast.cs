using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UGUI禁用父节点下所有射线检测
public class BlookRacast : MonoBehaviour ,ICanvasRaycastFilter
{
    public bool m_Blook = false;

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return m_Blook;
    }
}
