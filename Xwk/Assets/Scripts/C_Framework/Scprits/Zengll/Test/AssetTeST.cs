using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using System;
using DG.Tweening;
public class AssetTeST : MonoBehaviour {
    public Animator Baseanimator;
    public Camera Mycam;
    public DOTweenPath tween;
    // Use this for initialization
    void Start()
    {
        //WindowSliderControl.Instance.InitCharacter(Mycam);
        //TextAsset tx = Resources.Load("Config/Separation/BeforeSeparation") as TextAsset;
        //Debug.LogError(tx);
        //string[] objInfoArray = tx.text.Split('\n'); // 以\n为分割符将文本分割为一个数组
        //for (int i = 0; i < objInfoArray.Length; i++)
        //{
        //    if (objInfoArray[i] != "")
        //        Debug.LogError(objInfoArray[i]);

        //}




        //Baseanimator = transform.GetComponent<Animator>();
        //ABResMgr.Instance.LoadAsyncAssetBundle(BundlePath.GetBundlePath(BundlePath.SeprationPath), "separation");

        //Debug.LogError(GetPath("a/b/c"));
    }
    private void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tween.DOComplete();
            tween.DOPlayBackwards();
         }           
    }
    string GetPath(string path)
    {
        if (Application.isEditor)
        {
           // return "GameResource/" + path;
            if (path.Contains("/"))
            {
                string[] str = path.Split('/');
                if (str.Length > 0)
                    return (str[str.Length - 1]);
            }
            return path;
        }
        else
        {
            if (path.Contains("/"))
            {
                string[] str = path.Split('/');
                if (str.Length > 0)
                    return (str[str.Length - 1]);
            }
            return path;
        }
    }
}
