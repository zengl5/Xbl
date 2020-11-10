using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class FileTools : MonoBehaviour {

    /** 
  * path：读取文件的路径 
  * name：读取文件的名称 
  */
   public static ArrayList LoadFile(string path, string name)
    {
        //使用流的形式读取  
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path + "//" + name);
        }
        catch (Exception e)
        {
            //路径与名称未找到文件则直接返回空  
            Debug.LogError("LoadFile error "+e.Message);
            return null;
        }
        string line;
        ArrayList arrlist = new ArrayList();
        while ((line = sr.ReadLine()) != null)
        {
            //一行一行的读取  
            //将每一行的内容存入数组链表容器中  
            arrlist.Add(line);
        }
        //关闭流  
        sr.Close();
        //销毁流  
        sr.Dispose();
        //将数组链表容器返回  
        return arrlist;
    }
   public static string ReadIOFile(string path)
   {
       if (string.IsNullOrEmpty(path))
       {
           Debug.LogError("ReadIOFile path is null");
           return null;
       }
       string arrlist ="";//= new ArrayList();
       try
       {
           FileStream aFile = new FileStream(path, FileMode.Open);
           StreamReader sr = new StreamReader(aFile);
           string line;
           while ((line = sr.ReadLine()) != null)
           {
               //一行一行的读取  
               //将每一行的内容存入数组链表容器中  
               //arrlist.Add(line);
               arrlist += line;
           }
           sr.Close();
           sr.Dispose();
       }
       catch (IOException ex)
       {
           Debug.LogError("ReadIOFile error"+ex.Message);
           return null;
       }
       return arrlist;
   }
   public static bool CreateDir(string path)
   {
       if (string.IsNullOrEmpty(path))
       {
           Debug.LogWarning("path is null" + path);
           return false;
       }
       if (!Directory.Exists(path))
       {
           Directory.CreateDirectory(path);
       }
       return true;
   }
   public static bool DeleteFile(string path)
   {
       if (string.IsNullOrEmpty(path) || !File.Exists(path))
       {
           Debug.LogWarning("path= " + path + "  is no exit or null");
           return false;
       }
       File.Delete(path);
       return true;
   }
   
   static void CreateFile(string path,string name,string info)
    {
        //文件流信息
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            //如果此文件不存在则创建
            sw = t.CreateText();
        }
        else
        {
            //如果此文件存在则打开
            sw = t.AppendText();
        }
        //以行的形式写入信息
        sw.WriteLine(info);
        //关闭流
        sw.Close();
        //销毁流
        sw.Dispose();
    }
   public static void CreateFile(string path,string content,bool append = false)
   {
       if(string.IsNullOrEmpty(path)){
           Debug.LogError("文件路径是空");
           return;
       }
       FileStream filestream ;
       if (!append)
       {
           filestream = new FileStream(path, FileMode.Create);
       }
       else
       {
           filestream = new FileStream(path, FileMode.Append);
       }
       StreamWriter sw = new StreamWriter(filestream, new UTF8Encoding(false));
       sw.WriteLine(content);
       sw.Close();
       sw.Dispose();
       filestream.Close();
       filestream.Dispose();
   }

   public static void GetFileNameWithoutExtionAndFullPath(string fullPathName,ref string result)
   {
       if (!string.IsNullOrEmpty(fullPathName))
       {
          fullPathName = Path.GetFileNameWithoutExtension(fullPathName);
          if (result.Contains("/"))
          {
              result = fullPathName.Substring(fullPathName.LastIndexOf("/") + 1);
           }
           return;
       }
       result = "";
   }
   public static string GetResourcePath(string fileName)
   {
       string path  = GetDataPath()+"Resources/"+fileName;
       if(File.Exists(path)){
           return "";
       }
       return path;
   }
   public static string GetDataPath()
   {
       string filePath = string.Empty;
#if UNITY_EDITOR
       filePath = Application.dataPath + "/";
#elif UNITY_IPHONE
    filePath= Application.dataPath+ "/";
#elif UNITY_ANDROID
        filePath = Application.persistentDataPath+ "/";
#endif
       return filePath;
   }
   public static string GetStreamingAssets()
   {
       string filePath = string.Empty;
#if UNITY_ANDROID && !UNITY_EDITOR  
        filePath ="jar:file://" + Application.dataPath + "!/assets/" ;  
#elif UNITY_IPHONE && !UNITY_EDITOR  
        filePath =Application.dataPath + "/Raw/";  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
       filePath = "file://" + Application.dataPath + "/StreamingAssets" + "/";
#else  
       filePath = string.Empty;  
#endif
       return filePath;
   }

    public static bool IsFileExited(string fileName){
        return File.Exists(fileName);
    }

}
