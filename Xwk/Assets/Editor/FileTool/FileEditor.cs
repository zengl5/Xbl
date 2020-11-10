using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
public class FileEditor : MonoBehaviour {

    static List<string> pathList = new List<string>();//"iuv/anim/iuv_003@cam.unity3d"
    static List<string> pathSplitList = new List<string>();//iuv_003@cam"

    static List<string> GUIDList = new List<string>();
    static string SceneName = "aoe";
    static string Savepath;
    static StreamWriter writer = null;
    [MenuItem("小伴龙Tool/分身术/存储分身前位置")]   
    public static void WriteFileName()
    {
        StreamWriter writer = new StreamWriter(Application.dataPath + "/Resources/Config/Separation/BeforeSeparation.txt");
        Object[] select = Selection.objects;
        for (int i = 0; i < select.Length; i++)
        {
            GameObject temp = select[i] as GameObject;
            string posinfo =temp.name+","+temp.transform.position.x + "," + temp.transform.position.y + "," + temp.transform.position.z;
            writer.WriteLine(posinfo);
        }
        writer.Close();
    }



    [MenuItem("小伴龙Tool/分身术/存储分身之后【位置-旋转-动画Name】信息")]
    public static void WriteSepName()
    {          
        writer = new StreamWriter(Application.dataPath + "/Resources/Config/Separation/AfterSeparationInfo.txt");      
        Object[] select = Selection.objects;
        for (int i = 0; i < select.Length; i++)
        {
            
            GameObject temp = select[i] as GameObject;
            string name =temp.gameObject.name;
            string posinfo = temp.transform.position.x + "," + temp.transform.position.y + "," + temp.transform.position.z;
            string rotation = temp.transform.eulerAngles.x + "," + temp.transform.eulerAngles.y + "," + temp.transform.eulerAngles.z;
            writer.WriteLine(name+":"+posinfo+":"+rotation);
            Debug.LogError(name);
        }
        writer.Close();
    }
    [MenuItem("小伴龙Tool/场景/分身术")]
    public static void Separation()
    {
        string path= Application.dataPath + "/Scene/zengll/Separation";
       // if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene(path);
    }
    [MenuItem("小伴龙Tool/场景/金箍棒")]
    public static void JGB()
    {
        string path = Application.dataPath + "/Scene/zengll/Goldhoopbar";
       // if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene(path);
    }


    [MenuItem("小伴龙Tool/文件拷贝/aoe")]
    public static void FileCopy()
    {
        CreatSavaPath();
        string[] target;
        //找到文件目标路径集合
        string aoePath = Application.dataPath + "/Data/Config/Loading/aoe.txt";
        AddFileList(aoePath, SceneName);

        //存储文件绝对路径
        for (int i = 0; i < pathSplitList.Count; i++)
        {
            target = AssetDatabase.FindAssets(pathSplitList[i]);
            GUIDList.Add(AssetDatabase.GUIDToAssetPath(target[0]));
        }
        //开始备份文件
        for (int i = 0; i < GUIDList.Count; i++)
        {
            string NowSavepath = GetSavaPath(pathSplitList[i]);
            AssetDatabase.CopyAsset(GUIDList[i], NowSavepath);
           // AssetDatabase.Refresh();
            Debug.LogError("source::"+GUIDList[i]);
            Debug.LogError("sava::"+ NowSavepath);
        }
        AssetDatabase.Refresh();

        Debug.LogError("统计拷贝完成文件个数:" + GUIDList.Count);     
    }

    static void AddFileList(string ConfigPath,string nowScene)
    {
        pathList.Clear();
        StreamReader reader = new StreamReader(ConfigPath);
        string rx = reader.ReadLine();
        while (rx != null)
        {
            pathList.Add(rx);
            rx = reader.ReadLine();
            if (rx == null)
            {
                for (int i = pathList.Count - 1; i >= 0; i--)
                {
                    if (pathList[i].StartsWith("public") || pathList[i].StartsWith(nowScene))
                    {
                        pathList.Remove(pathList[i]);
                    }
                }
                for (int i = 0; i < pathList.Count; i++)
                {
                    string[] ts = pathList[i].Split('/');
                    string fileName=ts[ts.Length-1];//iuv_003@cam.unity3d
                    string sf = fileName.Replace(".unity3d", "");
                    pathSplitList.Add(sf);
                }
                Debug.LogError("统计需要拷贝文件个数" + pathSplitList.Count);            
            }
        }
    }  
    static void CreatSavaPath()
    {
        Savepath = Application.dataPath + "/Resources/PackagingResources/" + SceneName + "/AddFile";
        DirectoryInfo dir = new DirectoryInfo(Savepath);
        if (!dir.Exists)
        {
            dir.Create();
        }
    }
    static string GetSavaPath(string name)
    {
        if (name.Contains("animatorcontroller"))
        {
            return Savepath + "/"+name+".controller";
        }
        else if (name.Contains("anim"))
        {
            return Savepath + "/" + name +".FBX";
        }
        else if (name.Contains("@mesh"))
        {
            return Savepath + "/" + name + ".FBX";
        }
        else
        {
            Debug.LogError(name);
            return Savepath + "/"+name +".FBX";
        }
    }
    
        
    

}
