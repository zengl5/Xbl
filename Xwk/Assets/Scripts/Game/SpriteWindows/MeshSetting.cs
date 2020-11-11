using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YB.XWK.MainScene;

public class MeshSetting : MonoBehaviour {

    public string sceneName;
    GameObject Jl;
    string xlnMeshPath= "public/mesh/jl00003/jl00003_1#mesh";
    string xlnTexPath= "public/mesh/jl00003/upTex/jl00003_tx_c_l";
	// Use this for initialization
	void Awake() {
        //根据场景名字动态换装
        InitMeshMat();
    }
    //场景开始刷新一遍贴图
    void InitMeshMat()
    {
        if (sceneName.Equals("byjl_story"))
        {
            Jl = GameObjectTool.Instance.InitPlayer(SpfilePath.Instance.jl00002_2, null, Vector3.zero, Vector3.zero, "Sprite");
            GameObjectTool.Instance.SetSpMainTexture(Jl, SpfilePath.Instance.jlTex + (LocalData.RoleLevel + 1).ToString());//设置升级贴图
        }         
        else if (sceneName.Equals("bbhl_story"))
        {
            Jl = GameObjectTool.Instance.InitPlayer(HlfilePath.Instance.jl00001_1, null, Vector3.zero, Vector3.zero, "Sprite");
            GameObjectTool.Instance.SetSpMainTexture(Jl, HlfilePath.Instance.huluTex + (LocalData.RoleLevel + 1).ToString());//设置升级贴图
        }
        else if (sceneName.Equals("xzlz_story"))
        {
            Jl = GameObjectTool.Instance.InitPlayer(xlnMeshPath, null, Vector3.zero, Vector3.zero, "Sprite");
            GameObjectTool.Instance.SetSpMainTexture(Jl, xlnTexPath + (LocalData.RoleLevel + 1).ToString());//设置升级贴图
        }
    }


}
