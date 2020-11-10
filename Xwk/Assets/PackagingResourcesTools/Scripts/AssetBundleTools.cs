using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.C_Framework;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(AssetBundleTools))]
public class EditorAssetBundleTools : Editor
{
    private AssetBundleTools m_Component;

    private bool m_bShowClear = true;
    private bool m_bShowBuildAll = true;

    private string m_strAsbPath = "";
    private string m_strAsbSavePath = "";

    private int m_nSelectedPlatform = 0;
    private string[] m_Options = { "Windows", "Android", "Apple"};

    public virtual void OnEnable()
    {
        m_Component = (AssetBundleTools)target;
    }

    public override void OnInspectorGUI()
    {
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUI.skin.label.wordWrap = true;

        GUILayout.Space(20);

        m_bShowClear = EditorGUILayout.Foldout(m_bShowClear, "清理所有AB包名字");
        if (m_bShowClear)
        {
            if (GUILayout.Button("开始清理AB包名字", GUILayout.Height(30)))
            {
                string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
                
                string[] oldAssetBundleNames = new string[assetBundleNames.Length];
                for (int i = 0; i < assetBundleNames.Length; i++)
                {
                    oldAssetBundleNames[i] = assetBundleNames[i];

                    EditorUtility.DisplayProgressBar("正在获取已有的AB包名字", "已获取" + string.Format("{0}/{1}", i, assetBundleNames.Length), i / (float)assetBundleNames.Length);
                }

                for (int i = 0; i < assetBundleNames.Length; i++)
                {
                    AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[i], true);

                    EditorUtility.DisplayProgressBar("正在清理所有AB包名字", "已清理" + string.Format("{0}/{1}", i, assetBundleNames.Length), i / (float)assetBundleNames.Length);
                }

                EditorUtility.ClearProgressBar();
                Debug.Log("清理AB包名字完成");
            }
        }

        GUILayout.Space(20);

        m_bShowBuildAll = EditorGUILayout.Foldout(m_bShowBuildAll, "打包文件夹内资源");
        if (m_bShowBuildAll)
        {
            GUILayout.Label("输入或选择[打包]文件夹:");
            m_strAsbPath = EditorToolsMgr.TextField(m_strAsbPath, GUILayout.Height(15));
            if (GUILayout.Button("[打包]文件夹选择", GUILayout.Height(30)))
            {
                string path = EditorUtility.OpenFolderPanel("请选择文件夹", m_strAsbPath ?? Application.dataPath, "");
                if (!string.IsNullOrEmpty(path))
                    m_strAsbPath = path;
            }

            GUILayout.Space(10);

            GUILayout.Label("输入或选择[保存]文件夹:");
            m_strAsbSavePath = EditorToolsMgr.TextField(m_strAsbSavePath, GUILayout.Height(15));
            if (GUILayout.Button("[保存]文件夹选择", GUILayout.Height(30)))
            {
                string path = EditorUtility.OpenFolderPanel("请选择文件夹", m_strAsbSavePath ?? Application.dataPath, "");
                if (!string.IsNullOrEmpty(path))
                    m_strAsbSavePath = path;
            }

            GUILayout.Space(10);

            GUILayout.Label("打包到平台:");
            m_nSelectedPlatform = GUILayout.SelectionGrid(m_nSelectedPlatform, m_Options, m_Options.Length, GUI.skin.toggle);

            GUILayout.Space(10);

            if (GUILayout.Button("重命名所有AB包名字", GUILayout.Height(30)))
                m_Component.RenameAllAssetBundlesName(m_strAsbPath);

            GUILayout.Space(10);

            if (GUILayout.Button("开始打包所有AB包", GUILayout.Height(30)))
                m_Component.StartAllBuild(m_strAsbPath, m_strAsbSavePath, m_nSelectedPlatform);
        }

        GUILayout.Space(20);
    }
}


public class AssetBundleTools : MonoBehaviour
{
    // 支持的资源文件格式
    private static readonly string[] s_IgnoreDirsExts = {
        "c_framework"
        , "g_lagecy"
        , "read_explore"
        , "write_explore"
        , "SceneArt"
        , "Word"
        , "write"
    };

    // 支持的资源文件格式
    private static readonly string[] s_ResourceExts = {
        ".prefab"
        , ".fbx"
        , ".png"
        , ".jpg"
        , ".dds"
        , ".gif"
        , ".psd"
        , ".tga"
        , ".bmp"
        , ".txt"
        , ".bytes"
        , ".xml"
        , ".csv"
        , ".json"
        , ".controller"
        , ".shader"
        , ".anim"
        //, ".unity"
        , ".mat"
        , ".wav"
        , ".mp3"
        , ".ogg"
        , ".ttf"
        , ".shadervariants"
        , ".asset"
    };

    private const string c_Target = "PackagingResources/";

    public void RenameAllAssetBundlesName(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.Log("重命名所有AB包名字失败！文件夹不存在！");
            return;
        }

        List<string> list = new List<string>();

        //递归找到所有文件夹, 忽略指定文件夹
        List<string> dirlist = GetAllLocalSubDirs(folderPath);
        dirlist.Add(folderPath);

        //找到所有资源, 添加指定资源
        for (int i = 0; i < dirlist.Count; i++)
        {
            list.AddRange(GetDirResource(dirlist[i]));

            EditorUtility.DisplayProgressBar("正在获取路径下资源", "已获取" + string.Format("{0}/{1}", i, dirlist.Count), i / (float)dirlist.Count);
        }
        
        //处理, 取得相对assets地址
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = GetLocalPath(list[i]);

            EditorUtility.DisplayProgressBar("正在获取相对assets地址", "已获取" + string.Format("{0}/{1}", i, list.Count), i / (float)list.Count);
        }

        //开始重命名
        for (int i = 0; i < list.Count; i++)
        {
            RenameSingleABName(list[i]);

            EditorUtility.DisplayProgressBar("正在重命名", "进度" + string.Format("{0}/{1}", i, list.Count), i / (float)list.Count);
        }

        EditorUtility.ClearProgressBar();
        Debug.Log("重命名所有AB包名字完成");
    }

    private List<string> GetAllLocalSubDirs(string rootPath)
    {
        List<string> ret = new List<string>();

        if (string.IsNullOrEmpty(rootPath))
            return ret;

        string fullRootPath = Path.GetFullPath(rootPath);
        if (string.IsNullOrEmpty(fullRootPath))
            return ret;

        string[] dirs = Directory.GetDirectories(fullRootPath);

        for (int i = 0; i < dirs.Length; i++)
        {
            if (!IgnoreDirs(dirs[i]))
                ret.Add(dirs[i]);
            else
                dirs[i] = "";
        }

        for (int i = 0; i < dirs.Length; i++)
        {
            List<string> list = GetAllLocalSubDirs(dirs[i]);

            ret.AddRange(list);
        }

        return ret;
    }

    private List<string> GetDirResource(string path)
    {
        List<string> list = new List<string>();

        if (string.IsNullOrEmpty(path))
            return list;

        string fullPath = Path.GetFullPath(path);
        if (string.IsNullOrEmpty(fullPath))
            return list;

        string[] files = Directory.GetFiles(fullPath);
        for (int i = 0; i < files.Length; i++)
        {
            string ext = Path.GetExtension(files[i]);
            if (string.IsNullOrEmpty(ext))
                continue;

            for (int j = 0; j < s_ResourceExts.Length; j++)
            {
                if (string.Compare(ext, s_ResourceExts[j], true) == 0)
                {
                    list.Add(files[i]);
                    break;
                }
            }
        }

        return list;
    }

    private string GetLocalPath(string fullPath)
    {
        if (string.IsNullOrEmpty(fullPath))
            return "";

        fullPath = fullPath.Replace("\\", "/");

        int end = fullPath.IndexOf("Assets/", StringComparison.CurrentCultureIgnoreCase);

        return fullPath.Substring(end == -1 ? 0 : end);
    }

    private void RenameSingleABName(string path)
    {
        AssetImporter assetImporter = AssetImporter.GetAtPath(path);
        if (assetImporter != null)
        {
            int end = path.IndexOf(c_Target, StringComparison.CurrentCultureIgnoreCase);

            string assetName = path.Substring((end == -1 ? 0 : end) + c_Target.Length);

            assetName = assetName.Replace(Path.GetExtension(assetName), C_APP_CONST.AssetBundleExtension);

            assetImporter.assetBundleName = assetName;
        }
    }

    private bool IgnoreDirs(string folderPath)
    {
        int end = folderPath.LastIndexOf('\\');

        if (end != -1)
            folderPath = folderPath.Substring(end + 1, folderPath.Length - end - 1);

        for (int i = 0; i < s_IgnoreDirsExts.Length; i++)
        {
            if (string.Compare(folderPath, s_IgnoreDirsExts[i], true) == 0)
                return true;
        }

        return false;
    }

    public void StartAllBuild(string asbPath, string asbSavePath, int selectedPlatform)
    {
        if (string.IsNullOrEmpty(asbPath))
        {
            Debug.Log("请输入选择路径！");
            return;
        }

        if (string.IsNullOrEmpty(asbSavePath))
        {
            Debug.Log("请输入保存路径！");
            return;
        }

        Debug.Log("开始打包！");
        Debug.Log("选择路径：" + asbPath);
        Debug.Log("保存路径：" + asbSavePath);

        string localPath = C_LocalPath.PackagingResources_android;
        BuildTarget buildTarget = BuildTarget.Android;

        if (selectedPlatform == 0)
        {
            localPath = C_LocalPath.PackagingResources_windows;
            buildTarget = BuildTarget.StandaloneWindows64;
        }
        else if(selectedPlatform == 1)
        {
            localPath = C_LocalPath.PackagingResources_android;
            buildTarget = BuildTarget.Android;
        }
        else if (selectedPlatform == 2)
        {
            localPath = C_LocalPath.PackagingResources_ios;
            buildTarget = BuildTarget.iOS;
        }

        string savePath = asbSavePath + "/" + localPath + "/";

        if (Directory.Exists(savePath))
            DeleteDirs(new DirectoryInfo(savePath));

        Directory.CreateDirectory(savePath);

        BuildPipeline.BuildAssetBundles(savePath, BuildAssetBundleOptions.None, buildTarget);

        AssetDatabase.Refresh();
        Debug.Log("打包完成！");
    }

    private void DeleteDirs(DirectoryInfo dirs)
    {
        if (dirs == null || (!dirs.Exists))
            return;

        DirectoryInfo[] subDir = dirs.GetDirectories();
        for (int i = 0; i < subDir.Length; i++)
        {
            if (subDir[i] != null)
                DeleteDirs(subDir[i]);
        }

        FileInfo[] files = dirs.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i] != null)
            {
                files[i].Delete();
                files[i] = null;
            }
        }

        dirs.Delete();
    }
}

#endif