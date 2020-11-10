using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PrefabEditor : MonoBehaviour {
    [MenuItem("小伴龙Tool/预制管理/批量添加预制")]
    public static void BatchPrefab()
    {
        Transform tParent = ((GameObject)Selection.activeObject).transform;
        Object tempPrefab;
        int i = 1;
        foreach (Transform t in tParent)
        {
            tempPrefab = PrefabUtility.CreateEmptyPrefab("Assets/Resources/PackagingResources/game/BaibianHulu/prefabs/dj0000" + i + ".prefab");
            tempPrefab = PrefabUtility.ReplacePrefab(t.gameObject, tempPrefab);
            i++;
        }
    }

    [MenuItem("小伴龙Tool/预制管理/位置初始化")]
    public static void InitPos()
    {
        Transform[] tParent =Selection.transforms;
        for(int i=0;i<tParent.Length;i++)
        {
            tParent[i].transform.position = new Vector3(2+2*i, 0.83f, 0.065f);
        }
    }
}
