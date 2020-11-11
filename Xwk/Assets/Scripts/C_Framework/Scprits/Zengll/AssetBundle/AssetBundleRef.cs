using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AssetBundleRef
{
    public AssetBundleRef(string url)
    {
        bundlePath = url;
    }
    public void BundleUnload()
    {
        bundle.Unload(true);
    }
    public T LoadAsset<T>(string bundlePath) where T : UnityEngine.Object
    {
        return bundle.LoadAsset<T>(bundlePath);
    }
    public AssetBundleCreateRequest abcr;
    public AssetBundle bundle;
    string bundlePath;
    public float Progress;
}
