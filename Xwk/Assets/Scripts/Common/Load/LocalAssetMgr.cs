using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
public class LocalAssetMgr : C_Singleton<LocalAssetMgr>
{
    private string local_music = "Sound/";
    public AudioClip Load_Music(string name, bool preload = true,bool forever = false)
    {
         if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("播放的动画名字为空");
            return null;
        }

        AudioClip clip = null;
        string path = string.Empty;
        //if (fromLocal)
        //{
        //    path = local_music + name;
        //    clip = Resources.Load(path) as AudioClip;
        //    if (null == clip)
        //    {
        //        Debug.LogError("Failed Load_Music from " + path);
        //    }
        //    return clip;
        //}
        if(!name.Contains("/")){
            Debug.LogError("播放的动画名字没有/");
            return clip;
        }
        string extensionName = ".ogg";
        System.Text.StringBuilder audioName = new System.Text.StringBuilder();
        audioName.Append(name.Substring(name.LastIndexOf('/') + 1));
        string ex = audioName.ToString().ToLower();
        if (!ex.Contains(extensionName) 
            && !ex.Contains(".mp3") 
            && !ex.Contains(".wav")
            && !ex.Contains(".aiff")
            && !ex.Contains(".wma")
            && !ex.Contains(".midi"))//传入的语音文件是否包含ogg格式，没有则添加
        {
            audioName.Append(extensionName);
        }
        string moudleName = name.Substring(0,name.IndexOf('/'));
        if (moudleName.Equals("common"))
        {
            moudleName = "public";
        }
        string typePath = name.Substring(name.IndexOf('/') + 1);//第一个表示模块名
        if(typePath.Contains("/"))
            typePath = typePath.Substring(0, typePath.LastIndexOf('/'));//去掉声音的名字

        return C_Singleton<GameResMgr>.GetInstance().LoadResource<AudioClip>(audioName.ToString(), moudleName, typePath, "", false, forever) as AudioClip;
    }
    public void UnLoad_Music(string audioPath)
    {
        return;
        if (string.IsNullOrEmpty(audioPath))
        {
            Debug.LogError("播放的动画名字为空");
            return ;
        }
        string audioName = audioPath.Substring(audioPath.LastIndexOf('/') + 1).ToLower();//语音名字
        string extensionName = ".ogg";

       
        string exResPath = audioPath.Substring(0, audioPath.IndexOf(audioName));
        if (!audioName.Contains(extensionName)
         && !audioName.Contains(".mp3")
         && !audioName.Contains(".wav")
         && !audioName.Contains(".aiff")
         && !audioName.Contains(".wma")
         && !audioName.Contains(".midi"))//传入的语音文件是否包含ogg格式，没有则添加
        {
            audioName += extensionName;
        }
        C_Singleton<GameResMgr>.GetInstance().UnloadResource(audioName,"","", exResPath);
    }

    public static Object LoadGobalRes(string name)  
    {
        return LoadRes(name, C_PoolChannel.Global);
    }

    public static Object LoadUiRes(string name) 
    {
        return LoadRes(name, C_PoolChannel.UI);
    }
    public static Object LoadAvatartRes(string name)
    {
        return LoadRes(name, C_PoolChannel.Avatar);
    }

    private static Object LoadRes(string name, C_PoolChannel chanel) 
    {
        Object go;
     //   string[] nameArr = name.Split('/');
      //  name = nameArr[nameArr.Length - 1];
        string path = string.Empty;
        path = name;
      go  =  Resources.Load(path) as GameObject;
      //  go = C_ResMgr.Instance.LoadAssetBundle(path, chanel) as Object;
        //if (go == null)
        //{
        //    go = C_ResMgr.Instance.LoadResource(path, chanel) as Object;
        //}
        return go;
    }
    private static void UnLoadRes(string name, C_PoolChannel chanel)
    {

    }
}
 
