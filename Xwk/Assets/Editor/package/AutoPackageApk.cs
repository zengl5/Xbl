using Assets.Scripts.C_Framework;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;using UnityEditor;using System;using System.IO;class AutoPackageApk : Editor{
    //在这里找出你当前工程所有的场景文件，假设你只想把部分的scene文件打包 那么这里可以写你的条件判断 总之返回一个字符串数组。
    static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();
        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;
            if (e.enabled)
                names.Add(e.path);
        }
        return names.ToArray();
    }

    static void BuildForAndroid()
    {        string channel = ChannelName;        string projectRootPath = Application.dataPath + "/../../packageAPK";        string projectName = channel + "_Android_Progect/";

        DeleteFolder(projectRootPath + "/" + projectName);

        if (channel == "null")        {            string path = string.Format(Application.dataPath + "/../../packageAPK/Channel/null/wsds.apk");            BuildPipeline.BuildPlayer(GetBuildScenes(), path, BuildTarget.Android, BuildOptions.None);        }        else        {
            //string path = Application.dataPath +"/" + Function.projectName+".apk";
            //BuildPipeline.BuildPlayer(GetBuildScenes(), path, BuildTarget.Android, BuildOptions.None);
            Debug.Log(channel);        }    }




    //得到渠道的名称
    static string ChannelName    {        get        {            foreach (string arg in System.Environment.GetCommandLineArgs())            {                if (arg.StartsWith("channel"))                {                    return arg.Split('=')[1];                }            }            return "unKnown";        }    }

    static void DeleteFolder(string dir)    {        if (!Directory.Exists(dir))            return;        foreach (string d in Directory.GetFileSystemEntries(dir))        {            if (File.Exists(d))            {                FileInfo fi = new FileInfo(d);                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)                    fi.Attributes = FileAttributes.Normal;                File.Delete(d);            }            else            {                DirectoryInfo d1 = new DirectoryInfo(d);                if (d1.GetFiles().Length != 0)                {                    DeleteFolder(d1.FullName);////递归删除子文件夹
                }                Directory.Delete(d);            }        }    }

    static void CopyDirectory(string sourcePath, string destinationPath)    {        DirectoryInfo info = new DirectoryInfo(sourcePath);        Directory.CreateDirectory(destinationPath);        foreach (FileSystemInfo fsi in info.GetFileSystemInfos())        {            string destName = Path.Combine(destinationPath, fsi.Name);            if (fsi is System.IO.FileInfo)                File.Copy(fsi.FullName, destName);            else            {                Directory.CreateDirectory(destName);                CopyDirectory(fsi.FullName, destName);            }        }    }

}