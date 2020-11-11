using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelepManager : MonoBehaviour {
    public static bool UseInternalResource = true;//使用内置资源路径
    public static bool BuildMyAPK = true;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.LogError(ABResMgr.Instance.GetLoadProgress());
        //if (ABResMgr.Instance.GetLoadProgress() >= 1.0F)
        //    ABResMgr.Instance.DebugBundleTypeString("separation");
	}
}
