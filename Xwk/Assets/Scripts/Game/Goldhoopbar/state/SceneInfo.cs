using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SceneInfo
{
    public SceneInfo(Animator xwkPrefab, Animator goldHoopBarPrefab, Animator cameraPrefab)
    {
        XwkPrefab = xwkPrefab;
        GoldHoopBarPrefab = goldHoopBarPrefab;
        CameraPrefab = cameraPrefab;
    } 
    public Animator XwkPrefab;
    public Animator GoldHoopBarPrefab;
    public Animator CameraPrefab;  
}
