using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuiltInternelResTool : BuiltABResTool
{
    private List<string> _ResPathList = new List<string>();
    [MenuItem("工具/抽离内置的资源", false, 103)]
    static void DoBuiltInternelResTool()
    {
        EditorWindow.GetWindow<BuiltInternelResTool>("抽离内置的资源");
        _ConfigPath = Application.dataPath + "/Resources/PackagingResources/Config/Loading/";
        _StoryDic.Clear();
        _PlamtformDic.Clear();
        for (int i = 0; i < StoryName.Length; i++)
        {
            if (_StoryDic.ContainsKey(StoryName[i]))
            {
                _StoryDic[StoryName[i]] = false;
            }
            else
                _StoryDic.Add(StoryName[i], false);
        }
        for (int j = 0; j < _Plamtform.Length; j++)
        {
            if (_StoryDic.ContainsKey(_Plamtform[j]))
            {
                _PlamtformDic[_Plamtform[j]] = false;
            }
            else
                _PlamtformDic.Add(_Plamtform[j], false);
        }
        _Init = true;
    }
    void OnGUI()
    {
        MoveAssets();
    }
    protected override bool getResPath(string path)
    {
        _ResPath = Application.dataPath+ "/Resources/PackagingResources/";
        if (!Directory.Exists(_ResPath))
        {
            Debug.LogError("打包路径资源：" + _ResPath + "不存在");
            return false;
        }
        return true;
    }
    public override void Move(int i, bool single = false)
    {
        //if (i < 0)
        //{
        //    Debug.LogError("没有指定平台...");
        //    return;
        //}
        try
        {
            for (int j = 0; j < StoryName.Length; j++)
            {
                if (!_StoryDic[StoryName[j]])
                {
                    continue;
                }

                //查找资源
                string config = _ConfigPath + StoryName[j] + ".txt";
                if (!FileTools.IsFileExited(config))
                {
                    MessageBoxEditor.ShowErrorBox("移动内置资源", config + "不存在，移动内置资源失败", "好的");
                    continue;
                }
                string data = getContent(config);
                string[] result = data.Split('\n');
                for (int k = 0; k < result.Length; k++)
                {
                    //找到这个文件的资源
                    result[k] = result[k].Split('\r')[0];
                    result[k] = Assets.Scripts.C_Framework.C_String.DeleteExpandedName(result[k]);//去掉后缀
                    if (string.IsNullOrEmpty(result[k]))
                        continue;
                    if (!_ResPathList.Contains(result[k]))
                    {
                        _ResPathList.Add(result[k]);
                    }
                }
                
            }

            //删除没有用到的资源
            int deleteSum = 0;
            string PackResPath = Application.dataPath + "/Resources/PackagingResources";
            List<string> ResDirectoryPaths=new List<string>();
            ResDirectoryPaths = GetFile(PackResPath, ResDirectoryPaths);
            for (int resId = 0; resId < ResDirectoryPaths.Count; resId++)
            {
                if (Path.GetExtension(ResDirectoryPaths[resId]).Equals(".meta"))
                {
                    continue;
                }
                ResDirectoryPaths[resId] =  ResDirectoryPaths[resId].Replace("\\","/");
                //删除不存在表格的资源
                if (!DeleteRes(ResDirectoryPaths[resId]) && FileTools.IsFileExited(ResDirectoryPaths[resId]))
                {
                    File.Delete(ResDirectoryPaths[resId]);
                    File.Delete(ResDirectoryPaths[resId]+ ".meta");
                    Debug.Log("删除资源："+ ResDirectoryPaths[resId]);
                    Debug.Log("删除资源："+ ResDirectoryPaths[resId] + ".meta");
                    deleteSum++;
                }
            }
            Debug.Log("cleanup res over....删除资源个数："+ deleteSum);
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }

    }
    string[] fileterDir = { "/Resources/PackagingResources/read_explore"
            ,"/Resources/PackagingResources/RecognizeAudioFiles"
            ,"/Resources/PackagingResources/c_framework"
            ,"/Resources/PackagingResources/write_explore"
            ,"/Resources/PackagingResources/Config"
            ,"/Resources/PackagingResources/public/roleinfo"
            ,"/Resources/PackagingResources/public/SceneArt"
            ,"/Resources/PackagingResources/public/TuJian"
            ,"/Resources/PackagingResources/public/write"
            ,"/Resources/PackagingResources/public/word"
            ,"/Resources/PackagingResources/public/DingDongGu"
            ,"/Resources/PackagingResources/g_lagecy"
            ,"/Resources/PackagingResources/public/effect"
    };
    private bool getFilterDir(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }
        if (!Directory.Exists(path))
        {
            Debug.LogError(path+"is not exited..");
            return false;
        }
        path = path.Replace("\\","/");
        for (int i =0;i < fileterDir.Length;i++)
        {
            string path1 = (Application.dataPath + fileterDir[i]);
            if (path1.ToLower().Equals(path.ToLower()))
            {
                return true;
            }
        }

        return false;
    }
    private  void deletedir(string path, string extName)
    {
        try
        {
            string[] dir = Directory.GetDirectories(path); //文件夹列表   
            DirectoryInfo fdir = new DirectoryInfo(path);
            FileInfo[] file = fdir.GetFiles();
            //FileInfo[] file = Directory.GetFiles(path); //文件列表   
            if (file.Length != 0 || dir.Length != 0) //当前目录文件或文件夹不为空                   
            {
                foreach (string d in dir)
                {
                    if (getFilterDir(d))
                    {
                        continue;
                    }
                    deletedir(d, extName);//递归   
                }
            }
            else if (file.Length == 0 && dir.Length == 0)
            {
                if (fdir.Exists)
                {
                    Directory.Delete(path,true);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            throw ex;
        }
    }
    public  List<string> GetFile(string path, List<string> FileList)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] fil = dir.GetFiles();
        DirectoryInfo[] dii = dir.GetDirectories();
        foreach (FileInfo f in fil)
        {
            if (Path.GetExtension(f.FullName).Equals(".meta"))
            {
                continue;
            }
            FileList.Add(f.FullName);//添加文件路径到列表中  
        }
        //获取子文件夹内的文件列表，递归遍历  
        foreach (DirectoryInfo d in dii)
        {
            if (getFilterDir(d.FullName))
            {
                continue;
            }
            GetFile(d.FullName, FileList);
        }
        return FileList;
    }
    bool DeleteRes(string path)
    {
        string dirpathTemp = path.ToLower();
        string mark = "packagingresources/";
        int id = dirpathTemp.LastIndexOf(mark) + mark.Length;
        string filePath = "";
        if (id == -1)
        {
            filePath = dirpathTemp;
        }
        else if (id + 1 >= dirpathTemp.Length)
        {
            return false;
        }
        else
        {
            filePath = dirpathTemp.Substring(id);
        }
        if (string.IsNullOrEmpty(filePath))
        {
            return false;
        }

        string filePathNoEx = Assets.Scripts.C_Framework.C_String.DeleteExpandedName(filePath);//去掉后缀

        for (int index = 0; index < _ResPathList.Count; index++)
        {
            if (filePathNoEx.ToLower().Equals(_ResPathList[index].ToLower()))
            {
                return true;
            }
        }
        return false;
    }
    public void MoveTest(int i, bool single = false)
    {
        if (i <0 )
        {
            Debug.LogError("没有指定平台...");
            return;
        }

        for (int j = 0; j < StoryName.Length; j++)
        {
            if (!_StoryDic[StoryName[j]])
            {
                continue;
            }
            string AssetDestionResPath = "";
            if (single)
            {
                //每一集单独一个资源文件夹
                _DestionResPath = LocalPath.HotUpdatePath + "builtin/" + _Plamtform[i] + StoryName[j] + "Internal";
                AssetDestionResPath = "Assets/builtin/" + _Plamtform[i] + StoryName[j] + "Internal";

            }
            else
            {
                _DestionResPath = LocalPath.HotUpdatePath + "builtin/" + _Plamtform[i] + _Plamtform[i] + "Internal";
                AssetDestionResPath = "Assets/builtin/" + _Plamtform[i] + _Plamtform[i] + "Internal";

            }
       //     _DestionResPath = Application.dataPath + "/builtin/" + _Plamtform[i] + _Plamtform[i] + "Internal";
            //目标文件夹路径
            FileTools.CreateDir(_DestionResPath);


            //查找资源
            string config = _ConfigPath + StoryName[j] + ".txt";
            if (!FileTools.IsFileExited(config))
            {
                MessageBoxEditor.ShowErrorBox("移动内置资源", config + "不存在，移动内置资源失败", "好的");
                continue;
            }
            string data = getContent(config);
            string[] result = data.Split('\n');
            for (int k = 0; k < result.Length; k++)
            {
                //找到这个文件的资源
                result[k] = result[k].Split('\r')[0];
                result[k] = Assets.Scripts.C_Framework.C_String.DeleteExpandedName(result[k]);//去掉后缀
                if (string.IsNullOrEmpty(result[k]))
                    continue;
                string resDir = Assets.Scripts.C_Framework.C_String.GetSavePath(result[k]);//保存资源的子目录
                string resFileName = Assets.Scripts.C_Framework.C_String.GetFileName(result[k]);//获取资源的名字
                string res = _ResPath + resDir;
                if (!Directory.Exists(res))//在资源项目结构中的位置
                {
                  //  Debug.LogError("工程中不存在资源文件夹："+res);
                    continue;
                }
                string[] DirectoryPaths = Directory.GetFiles(res);
                //获取该资源结构文件夹中的所有文件
                //foreach (string dirpath in DirectoryPaths)
                for(int pathId =0; pathId < DirectoryPaths.Length;pathId++)
                {
                    string dirpath = DirectoryPaths[pathId];
                    if (string.IsNullOrEmpty(dirpath))
                    {
                        continue;
                    }
                    if (Path.GetExtension(dirpath).Equals(".meta"))
                    {
                        continue;
                    }
                    string  dirpathTemp = dirpath.ToLower();
                    string mark = "PackagingResources/";
                    int id = dirpath.LastIndexOf(mark) + mark.Length;
                    string filePath = "";
                    if (id == -1)
                    {
                        filePath = dirpath;
                    }
                    else if (id + 1 >= dirpath.Length)
                    {
                        continue;
                    }
                    else
                    {
                        filePath = dirpath.Substring(id);
                    }
                    if (string.IsNullOrEmpty(filePath))
                    {
                        continue;
                    }
                    string filePathNoEx = Assets.Scripts.C_Framework.C_String.DeleteExpandedName(filePath);//去掉后缀
                    string desFileName = (resDir + resFileName).ToLower();
                    if (!filePathNoEx.ToLower().Equals(desFileName))
                    {
                        continue;
                    }
                    Debug.Log("移动资源：" + dirpath);

                    try
                    {
                        //创建资源移动的目标资源子目录
                        FileTools.CreateDir(_DestionResPath + "/" + resDir);
                        //将资源从项目工程文件夹移动到目标文件夹中，覆盖方式拷贝
                        string destionPathFile = _DestionResPath + "/" + filePath;
                        string destionPathFileMeta = _DestionResPath + "/" + filePath + ".meta";
                        if (FileTools.IsFileExited(destionPathFile))
                        {
                            File.Delete(destionPathFile);
                        }
                        if (FileTools.IsFileExited(destionPathFileMeta))
                        {
                            File.Delete(destionPathFileMeta);
                        }
                        // string tmp =Application.dataPath+ "/Resources/PackagingResources/" + filePath;
                        string tmp = DirectoryPaths[pathId];
                        string tmpMeta = tmp + ".meta";
                        // AssetDatabase.CopyAsset(tmp, destionPathFile);
                        // AssetDatabase.CopyAsset(tmpMeta, destionPathFileMeta);

                        // File.Copy(tmp, destionPathFile,true);
                        //  File.Copy(tmpMeta, destionPathFileMeta, true);
                        FileInfo fileInfo = new FileInfo(tmp);
                        if (!fileInfo.Exists)
                        {
                            Debug.LogError(fileInfo.FullName+"is not exited..");
                        }
                        fileInfo.CopyTo(destionPathFile, true);

                        FileInfo fileInfoMeta = new FileInfo(tmp);
                        fileInfoMeta.CopyTo(destionPathFileMeta, true);
                        if (!fileInfoMeta.Exists)
                        {
                            Debug.LogError(fileInfoMeta.FullName + "is not exited..");
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                        continue;
                    }

                }

            }
             
        }
        
    }


}
