using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class AudioMgr
{
    public static void PlaySoundEffect(string name)
    {
        if (string.IsNullOrEmpty(name))
            return;

        C_MonoSingleton<C_AudioMgr>.GetInstance().PlayClipOneShot(C_Singleton<GameResMgr>.GetInstance().LoadResource_Audio_Effect(name));
    }

    public static string m_strBGM = "";
    public static void PlayBGM(string name)
    {
        if (string.IsNullOrEmpty(name) && m_strBGM == name)
            return;

        m_strBGM = name;

        C_MonoSingleton<C_AudioMgr>.GetInstance().PlayBGM(C_Singleton<GameResMgr>.GetInstance().LoadResource<AudioClip>(name + ".ogg", "public", "sound", "public/sound/"));

        //C_Singleton<GameResMgr>.GetInstance().AsyncLoadResource_Audio_BGM(name, (AudioClip result) =>
        //{
        //    C_MonoSingleton<C_AudioMgr>.GetInstance().PlayBGM(result);
        //});
    }

    public static void StopBGM()
    {
        m_strBGM = "";

        C_MonoSingleton<C_AudioMgr>.GetInstance().StopBGM();
    }

    public static void PlayAudio_URL_MP3(string url)
    {
        if (string.IsNullOrEmpty(url))
            return;

        string filePath = C_LocalPath.DataPath + C_String.GetFileName(url);
        if (!File.Exists(filePath))
            C_UnityWebRequestDownloader.SyncDownloadFile(url, C_LocalPath.DataPath);

        if (File.Exists(filePath))
            C_MonoSingleton<C_AudioMgr>.GetInstance().PlayClipOneShot_MP3(filePath);
    }
}
