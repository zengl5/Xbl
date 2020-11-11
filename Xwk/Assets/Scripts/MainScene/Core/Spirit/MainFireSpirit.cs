using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 精灵管理类
/// </summary>
public class MainFireSpirit : MonoBehaviour {
    private Material[] jlmat;
    Material mat;
    float maxPow = 1.5f;
    float minPow = 0.5f;
    float Pow = 0.2f;
    float speed = 1f;
    bool isbigger = true;
     // Use this for initialization
    void Awake () {
        SkinnedMeshRenderer childMesh = this.transform.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        jlmat = childMesh.materials;
        if (jlmat.Length > 1)
            mat = jlmat[1];
    }
    /// <summary>
    /// 设置特效
    /// </summary>
	public void JlPowerChange()
    {
        if(mat!=null)
            mat.SetFloat("_AllPower", getPow());
    }
    
    // Update is called once per frame
    void Update () {
        JlPowerChange();
	}
    //1.13-0.5 周期两秒的循环
    float getPow()
    {
        if(isbigger)
        {
            if(Pow<maxPow)
            {
                Pow += Time.deltaTime * speed;
            }
            else
            {
                isbigger = false;
            }
        }
        else
        {
            if(Pow > minPow)
            {
                Pow -= Time.deltaTime*speed;
            }
            else
            {
                isbigger = true;
            }
        }
        return Pow;
    }
}
