using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BundlePath
{
    public static string SeprationPath = "sepeRation/separation";
    public static string GetBundlePath(string path)
    {
        if(Application.isEditor)
        {
             //if (DevelepManager.DevelepeMode)
               // return Application.streamingAssetsPath + "/Data/Android/" + path;
           return Application.streamingAssetsPath + "/Data/PC/" + path;
        }
        else if(Application.platform==RuntimePlatform.Android)
        {
                 return Application.streamingAssetsPath + "/Data/Android/" + path;
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
                return Application.streamingAssetsPath + "/Data/IOS/" + path;
        }
        else
        {
            return null;
        }
    }
}
 
