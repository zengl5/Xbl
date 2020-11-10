using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Assets.Scripts.C_Framework;
public class FileHelp : MonoBehaviour {
    public delegate void  FileList(List<string>FL);
    public event FileList fileList;
    
    static List<string> pathList = new List<string>();	
    public static void ReadFile(string path, FileList fl)
    {
        TextAsset tx = Resources.Load("Config/Separation/BeforeSeparation") as TextAsset;
        string[] objInfoArray = tx.text.Split('\n'); // 以\n为分割符将文本分割为一个数组
        for (int i = 0; i < objInfoArray.Length; i++)
        {
            if (objInfoArray[i] != "")
                pathList.Add(objInfoArray[i]);
        }
                if (fl != null)
                   fl(pathList);
        //pathList = new List<string>();
        //StreamReader reader = new StreamReader(path);
        //string rx = reader.ReadLine();
        //while (rx != null)
        //{
        //    //Debug.LogError(rx);
        //    pathList.Add(rx);
        //    rx = reader.ReadLine();
        //    if (rx == null)
        //        if (fl != null)
        //            fl(pathList);
        //}       
    }
    void Update()
    {

    }
    

}
