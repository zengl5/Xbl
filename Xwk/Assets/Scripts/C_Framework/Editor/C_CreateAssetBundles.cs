using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_CreateAssetBundles
    {
        // 支持的资源文件格式
        private static readonly string[] IgnoreDirsExts = {"c_framework"
                , "g_lagecy"
                , "read_explore"
                , "write_explore"
                , "SceneArt"
                , "Word"
                , "write"
        };

        // 支持的资源文件格式
        private static readonly string[] ResourceExts = {".prefab"
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

        public static UnityEngine.Object[] Objs = new UnityEngine.Object[] { };

        private const string c_Target = "PackagingResources/";


        [MenuItem("C_Framework/AssetBundle/ClearAllAssetBundlesName")]
        static void ClearAllAssetBundlesName()
        {
            Debug.Log("开始清理所有的AssetBundlesName！请等待清理完成！unity会卡住！");

            int length = AssetDatabase.GetAllAssetBundleNames().Length;

            string[] oldAssetBundleNames = new string[length];
            for (int i = 0; i < length; i++)
                oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];

            for (int j = 0; j < oldAssetBundleNames.Length; j++)
                AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);

            Debug.Log("清理所有的AssetBundlesName完成！");
        }

        [MenuItem("C_Framework/AssetBundle/BuildSelectObjects")]
        static void BuildSelect()
        {
            //获取所有选中的对象  
            Objs = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            
            //弹出一个编辑窗口
            AssetBundleWindow.ShowWindow(0);
        }

        [MenuItem("C_Framework/AssetBundle/BuildAllObjects")]
        static void BuildAll()
        {
            //弹出一个编辑窗口
            AssetBundleWindow.ShowWindow(1);
        }

        
        // 开始选择的对象打包
        public static void StartSelectBuild()
        {
            if (string.IsNullOrEmpty(AssetBundleWindow.AssetBudleName))
            {
                Debug.Log("请输入打包文件名！");
                return;
            }

            if (string.IsNullOrEmpty(AssetBundleWindow.AsbSavePath))
            {
                Debug.Log("请输入保存路径！");
                return;
            }

            Debug.Log("开始打包！");
            Debug.Log("保存路径：" + AssetBundleWindow.AsbSavePath);

            //设置出asb[]
            AssetBundleBuild abb = new AssetBundleBuild();
            abb.assetNames = new string[Objs.Length];

            for (int i = 0; i < Objs.Length; i++)
                abb.assetNames[i] = AssetDatabase.GetAssetPath(Objs[i]);

            abb.assetBundleName = AssetBundleWindow.AssetBudleName;

            //创建文件夹
            if (!Directory.Exists(AssetBundleWindow.AsbSavePath))
                Directory.CreateDirectory(AssetBundleWindow.AsbSavePath);

            if (AssetBundleWindow.IsWindows)
            {
                //开始打包
                Debug.Log("将要打包到Windows");
                BuildPipeline.BuildAssetBundles(AssetBundleWindow.AsbSavePath, new AssetBundleBuild[] { abb }, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
                AssetDatabase.Refresh();
            }

            if (AssetBundleWindow.IsAndorid)
            {
                //开始打包
                Debug.Log("将要打包到安卓");
                BuildPipeline.BuildAssetBundles(AssetBundleWindow.AsbSavePath, new AssetBundleBuild[] { abb }, BuildAssetBundleOptions.None, BuildTarget.Android);
                AssetDatabase.Refresh();
            }

            if (AssetBundleWindow.IsApple)
            {
                //开始打包
                Debug.Log("将要打包到苹果");
                BuildPipeline.BuildAssetBundles(AssetBundleWindow.AsbSavePath, new AssetBundleBuild[] { abb }, BuildAssetBundleOptions.None, BuildTarget.iOS);
                AssetDatabase.Refresh();
            }

            Debug.Log("打包完成！");
        }

        // 开始所有对象打包
        public static void StartAllBuild()
        {
            if (string.IsNullOrEmpty(AssetBundleWindow.AsbPath))
            {
                Debug.Log("请输入选择路径！");
                return;
            }

            if (string.IsNullOrEmpty(AssetBundleWindow.AsbSavePath))
            {
                Debug.Log("请输入保存路径！");
                return;
            }

            Debug.Log("开始打包！");
            Debug.Log("选择路径：" + AssetBundleWindow.AsbPath);
            Debug.Log("保存路径：" + AssetBundleWindow.AsbSavePath);

            if (AssetBundleWindow.IsWindows)
            {
                string savePath = AssetBundleWindow.AsbSavePath + "/" + C_LocalPath.PackagingResources_windows + "/";

                //删除老的文件夹
                if (Directory.Exists(savePath))
                    DeleteDirs(new DirectoryInfo(savePath));

                //创建新的文件夹
                Directory.CreateDirectory(savePath);
                

                //开始打包
                Debug.Log("将要打包到Windows");
                BuildPipeline.BuildAssetBundles(savePath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
                AssetDatabase.Refresh();
            }

            if (AssetBundleWindow.IsAndorid)
            {
                string savePath = AssetBundleWindow.AsbSavePath + "/" + C_LocalPath.PackagingResources_android + "/";

                //删除老的文件夹
                if (Directory.Exists(savePath))
                    DeleteDirs(new DirectoryInfo(savePath));

                //创建新的文件夹
                Directory.CreateDirectory(savePath);

                //开始打包
                Debug.Log("将要打包到安卓");
                BuildPipeline.BuildAssetBundles(savePath, BuildAssetBundleOptions.None, BuildTarget.Android);
                AssetDatabase.Refresh();
            }

            if (AssetBundleWindow.IsApple)
            {
                string savePath = AssetBundleWindow.AsbSavePath + "/" + C_LocalPath.PackagingResources_ios + "/";

                //删除老的文件夹
                if (Directory.Exists(savePath))
                    DeleteDirs(new DirectoryInfo(savePath));

                //创建新的文件夹
                Directory.CreateDirectory(savePath);
                
                //开始打包
                Debug.Log("将要打包到苹果");
                BuildPipeline.BuildAssetBundles(savePath, BuildAssetBundleOptions.None, BuildTarget.iOS);
                AssetDatabase.Refresh();
            }

            Debug.Log("打包完成！");
        }
        
        public static void RenameAllAssetBundlesName()
        {
            Debug.Log("开始重命名所有的AssetBundlesName！请等待重命名完成！unity会卡住！");

            List<string> list = GetAllRes();

            for (int i = 0; i < list.Count; i++)
                AssetBundleRename(list[i]);

            Debug.Log("重命名所有的AssetBundlesName完成！");
        }

        private static List<string> GetAllRes()
        {
            List<string> list = new List<string>();

            List<string> dirlist = GetResAllDir();
            for (int i = 0; i < dirlist.Count; i++)
                list.AddRange(GetDirResource(dirlist[i]));

            for (int i = 0; i < list.Count; i++)
                list[i] = GetLocalPath(list[i]);

            return list;
        }

        private static List<string> GetResAllDir()
        {
            List<string> ret = GetAllLocalSubDirs(AssetBundleWindow.AsbPath);
            ret.Add(AssetBundleWindow.AsbPath);

            return ret;
        }

        private static List<string> GetAllLocalSubDirs(string rootPath)
        {
            List<string> ret = new List<string>();

            if (string.IsNullOrEmpty(rootPath))
                return ret;

            string fullRootPath = System.IO.Path.GetFullPath(rootPath);
            if (string.IsNullOrEmpty(fullRootPath))
                return ret;

            string[] dirs = System.IO.Directory.GetDirectories(fullRootPath);
            if ((dirs == null) || (dirs.Length <= 0))
                return ret;

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
                if (list != null)
                    ret.AddRange(list);
            }

            return ret;
        }

        private static bool IgnoreDirs(string path)
        {
            string file = "";
            int end = path.LastIndexOf('\\');
            if (end == -1)
                file = path;
            else
                file = path.Substring(end + 1, path.Length - end - 1);

            for (int i = 0; i < IgnoreDirsExts.Length; i++)
            {
                if (string.Compare(file, IgnoreDirsExts[i], true) == 0)
                    return true;
            }

            return false;
        }

        // 获取目录下资源文件
        private static List<string> GetDirResource(string path)
        {
            List<string> list = new List<string>();

            if (string.IsNullOrEmpty(path))
                return list;

            string fullPath = Path.GetFullPath(path);
            if (string.IsNullOrEmpty(fullPath))
                return list;

            string[] files = System.IO.Directory.GetFiles(fullPath);
            if ((files == null) || (files.Length <= 0))
                return list;

            for (int i = 0; i < files.Length; i++)
            {
                string ext = System.IO.Path.GetExtension(files[i]);
                if (string.IsNullOrEmpty(ext))
                    continue;

                for (int j = 0; j < ResourceExts.Length; j++)
                {
                    if (string.Compare(ext, ResourceExts[j], true) == 0)
                    {
                        list.Add(files[i]);
                        break;
                    }
                }
            }

            return list;
        }

        private static string GetLocalPath(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
                return "";

            fullPath = fullPath.Replace("\\", "/");
            int index = fullPath.IndexOf("Assets/", StringComparison.CurrentCultureIgnoreCase);
            if (index < 0)
                return fullPath;

            return fullPath.Substring(index);
        }

        private static void AssetBundleRename(string path)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(path);
            if (assetImporter != null)
            {
                int index = path.IndexOf(c_Target, StringComparison.CurrentCultureIgnoreCase);

                string assetName = path.Substring(index + c_Target.Length);
                assetName = assetName.Replace(Path.GetExtension(assetName), C_APP_CONST.AssetBundleExtension);

                assetImporter.assetBundleName = assetName;
            }
        }

        private static void DeleteDirs(DirectoryInfo dirs)
        {
            if (dirs == null || (!dirs.Exists))
                return;

            DirectoryInfo[] subDir = dirs.GetDirectories();
            if (subDir != null)
            {
                for (int i = 0; i < subDir.Length; i++)
                {
                    if (subDir[i] != null)
                        DeleteDirs(subDir[i]);
                }

                subDir = null;
            }

            FileInfo[] files = dirs.GetFiles();
            if (files != null)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i] != null)
                    {
                        files[i].Delete();
                        files[i] = null;
                    }
                }
                files = null;
            }

            dirs.Delete();
        }
    }
    
    public class AssetBundleWindow : EditorWindow
    {
        //asb的名字
        public static string AssetBudleName;
        //asb包的路径
        public static string AsbPath;
        //asb包保存的路径
        public static string AsbSavePath;

        //是否在Windows下打包
        public static bool IsWindows = false;
        //是否在安卓下打包
        public static bool IsAndorid = false;
        //是否在苹果下打包
        public static bool IsApple = false;

        private static int WindowType = 0;

        AssetBundleWindow()
        {
            titleContent = new GUIContent("资源打包");
        }

        public static void ShowWindow(int windowType)
        {
            WindowType = windowType;

            GetWindow(typeof(AssetBundleWindow));
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            GUI.skin.label.fontSize = 15;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;

            if (WindowType == 0)
            {
                if (C_CreateAssetBundles.Objs.Length <= 0)
                {
                    GUILayout.Label("当前未选择任何资源！");
                    return;
                }

                GUILayout.Label("当前总共选择：" + C_CreateAssetBundles.Objs.Length + "个资源！");
            }
            else
            {
                //绘制打包文件路径选择
                GUILayout.Space(10);
                if (GUILayout.Button("打包路径选择", GUILayout.Width(200)))
                {
                    string path = EditorUtility.OpenFolderPanel("请选择打包路径", Application.dataPath, "");

                    if (!string.IsNullOrEmpty(path))
                        AsbPath = path;
                }


                GUI.skin.label.fontSize = 10;
                GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                //这里绘制选择结果
                if (string.IsNullOrEmpty(AsbPath))
                    GUILayout.Label("没有选择打包路径！");
                else
                    GUILayout.Label("当前选择打包路径：" + AsbPath);
            }

            //绘制保存文件路径选择
            GUILayout.Space(10);
            if (GUILayout.Button("保存路径选择", GUILayout.Width(200)))
            {
                string path = EditorUtility.OpenFolderPanel("请选择保存路径", Application.dataPath, "");

                if (!string.IsNullOrEmpty(path))
                    AsbSavePath = path;
            }

            GUI.skin.label.fontSize = 10;
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            //这里绘制保存结果
            if (string.IsNullOrEmpty(AsbSavePath))
                GUILayout.Label("没有保存打包路径！");
            else
                GUILayout.Label("当前保存打包路径：" + AsbSavePath);

            //放3个togle
            IsWindows = GUI.Toggle(new Rect(10, 130, 600, 20), IsWindows, "打包到Windows平台");
            IsAndorid = GUI.Toggle(new Rect(10, 150, 600, 20), IsAndorid, "打包到Android平台");
            IsApple = GUI.Toggle(new Rect(10, 170, 600, 20), IsApple, "打包到IOS平台");

            if (WindowType == 0)
            {
                //绘制文件打包按钮
                GUILayout.Space(120);
                GUILayout.Label("请输入导出的包名：");
                //设置一个文字输入框
                string name = EditorGUILayout.TextField(AssetBudleName);
                if (!string.IsNullOrEmpty(name))
                {
                    if (!name.Contains(C_APP_CONST.AssetBundleExtension))
                        name += C_APP_CONST.AssetBundleExtension;

                    AssetBudleName = name;
                }

                GUILayout.Space(10);


                if (GUILayout.Button("开始打包", GUILayout.Width(200)))
                    C_CreateAssetBundles.StartSelectBuild();
            }
            else
            {
                //绘制重命名AssetBundleName按钮
                GUILayout.Space(100);
                if (GUILayout.Button("重命名AssetBundleName", GUILayout.Width(200)))
                    C_CreateAssetBundles.RenameAllAssetBundlesName();

                //绘制文件打包按钮
                GUILayout.Space(20);
                if (GUILayout.Button("开始打包所有AssetBundle", GUILayout.Width(200)))
                    C_CreateAssetBundles.StartAllBuild();
            }
            
        }
    }
}