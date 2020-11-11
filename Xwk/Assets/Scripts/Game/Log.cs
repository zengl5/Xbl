using UnityEngine;
using System.Collections;
using System.IO;

public class Log : MonoBehaviour
{
    static string path;   
    static object obj = new object();
    static StreamWriter sw;

    static bool startLog = true;
    void OnEnable()
    {
         if(Application.isEditor)
        {
            path = Application.dataPath + "/LogFile.txt";
        }
        else
        {          
            path = Application.persistentDataPath + "/LogFile.txt";
        }

        if (startLog)
        {
            //每次进入客户端时候，把该日志删除
            if (File.Exists(path))
            {
                 File.Delete(path);
            }
            else
            {
                File.Create(path).Dispose();
            }
            startLog = true;
        }
        Application.logMessageReceived += SystemLogPrint;
    }
    void OnDisable()
    {
        Application.logMessageReceived -= SystemLogPrint;
    }

    //注意多线程问题
    void SystemLogPrint(string condition, string stackTrace, LogType type)
    {
        //开发者模式关闭日志输出，日志输出是首场景全局Class    
        lock (obj)
        {        
            print(condition);
            print(stackTrace);
        }
    }
    //注意多线程问题
    public static void print(string info)
    {
        //开发者模式关闭日志输出，日志输出是首场景全局Class     
        lock (obj)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(info);
                writer.Flush();
                writer.Close();
            }
        }
       // Ftp.PostToFtp(path,info);
    }
    /// <summary>
    /// 上传日志到服务器
    /// </summary>
    public void UpLoadLog()
    {
        using (StreamWriter writer = new StreamWriter(path, true))
        {       
            writer.Close();
        }
        //Ftp.PostToFtp(path, "上传日志");
    }
    
  
}

