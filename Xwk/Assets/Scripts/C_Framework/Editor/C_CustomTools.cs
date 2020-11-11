using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace Assets.Scripts.C_Framework
{
    public class C_CustomTools
    {
        [MenuItem("C_Framework/CustomTools/ClearCache - PlayerPrefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("C_Framework/CustomTools/AddAllDependencies")]
        public static void AddAllDependencies()
        {
            //弹出一个编辑窗口
            AddAllDependenciesWindow.ShowWindow();
        }

        public static void Modification()
        {
            Debug.Log("开始添加依赖！");
            Debug.Log("选择文件：" + AddAllDependenciesWindow.FilePath);

            if (string.IsNullOrEmpty(AddAllDependenciesWindow.FilePath) || !File.Exists(AddAllDependenciesWindow.FilePath))
            {
                Debug.Log("添加依赖失败！文件为空！");
                return;
            }

            if (string.IsNullOrEmpty(AddAllDependenciesWindow.ManifestFilePath) || !File.Exists(AddAllDependenciesWindow.ManifestFilePath))
            {
                Debug.Log("添加依赖失败！Manifest文件为空！");
                return;
            }

            //遍历文件
            string[] tempItems = File.ReadAllLines(AddAllDependenciesWindow.FilePath);

            List<string> itemList = new List<string>();

            //全转为小写,并修改后缀名
            for (int i = 0; i < tempItems.Length; i++)
            {
                tempItems[i] = tempItems[i].ToLower();

                if (!string.IsNullOrEmpty(tempItems[i]))
                {
                    if (!tempItems[i].Contains(C_APP_CONST.AssetBundleExtension))
                        tempItems[i] += C_APP_CONST.AssetBundleExtension;

                    itemList.Add(tempItems[i]);
                }
            }

            //去重复
            itemList = itemList.Distinct().ToList();

            if (itemList.Count == 0)
            {
                Debug.Log("items.Length = 0！");
                return;
            }

            AssetBundle manifestBundle = AssetBundle.LoadFromFile(AddAllDependenciesWindow.ManifestFilePath);
            if (manifestBundle != null)
            {
                AssetBundleManifest assetBundleManifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");

                manifestBundle.Unload(false);

                m_DependencieList.Clear();

                for (int i = 0; i < itemList.Count; i++)
                {
                    if (!m_DependencieList.Contains(itemList[i]) && Hash128.Parse("0") != assetBundleManifest.GetAssetBundleHash(itemList[i]))
                    {
                        m_DependencieList.Add(itemList[i]);

                        RecursionDependencies(assetBundleManifest, itemList[i]);
                    }
                }
            }

            //去重复
            m_DependencieList = m_DependencieList.Distinct().ToList();


            List<string> writeList = new List<string>();

            //全转为小写,并修改后缀名
            for (int i = 0; i < m_DependencieList.Count; i++)
            {
                //if (!string.IsNullOrEmpty(m_DependencieList[i]) && !m_DependencieList[i].Contains("sound"))
                if (!string.IsNullOrEmpty(m_DependencieList[i]))
                    writeList.Add(m_DependencieList[i]);
            }

            //删掉原本的文件
            File.Delete(AddAllDependenciesWindow.FilePath);

            StreamWriter sr = File.CreateText(AddAllDependenciesWindow.FilePath);
            for (int i = 0; i < writeList.Count; i++)
                sr.WriteLine(writeList[i]);
            sr.Close();

            Debug.Log("添加依赖完成！");
        }

        //递归找到所有依赖的AssetBundle
        private static List<string> m_DependencieList = new List<string>();

        private static void RecursionDependencies(AssetBundleManifest assetBundleManifest, string assetBundleName)
        {
            string[] names = assetBundleManifest.GetAllDependencies(assetBundleName);

            for (int i = 0; i < names.Length; i++)
            {
                if (!m_DependencieList.Contains(names[i]))
                {
                    m_DependencieList.Add(names[i]);

                    RecursionDependencies(assetBundleManifest, names[i]);
                }
            }
        }
    }

    public class AddAllDependenciesWindow : EditorWindow
    {
        //文件
        public static string FilePath;
        //Manifest文件
        public static string ManifestFilePath;

        AddAllDependenciesWindow()
        {
            titleContent = new GUIContent("添加依赖");
        }

        public static void ShowWindow()
        {
            GetWindow(typeof(AddAllDependenciesWindow));
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            GUI.skin.label.fontSize = 15;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;

            //绘制文件选择
            GUILayout.Space(10);
            if (GUILayout.Button("文件选择", GUILayout.Width(200)))
            {
                string path = EditorUtility.OpenFilePanel("请选择文件", Application.dataPath, "");

                if (!string.IsNullOrEmpty(path))
                    FilePath = path;
            }

            GUI.skin.label.fontSize = 10;
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            //这里绘制选择结果
            if (string.IsNullOrEmpty(FilePath))
                GUILayout.Label("没有文件！");
            else
                GUILayout.Label("当前文件：" + FilePath);

            //绘制Manifest文件选择
            GUILayout.Space(10);
            if (GUILayout.Button("Manifest文件选择", GUILayout.Width(200)))
            {
                string path = EditorUtility.OpenFilePanel("请选择Manifest文件", Application.dataPath, "");

                if (!string.IsNullOrEmpty(path))
                    ManifestFilePath = path;
            }

            GUI.skin.label.fontSize = 10;
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            //这里绘制选择结果
            if (string.IsNullOrEmpty(ManifestFilePath))
                GUILayout.Label("没有Manifest文件！");
            else
                GUILayout.Label("当前Manifest文件：" + ManifestFilePath);

            //绘制添加依赖按钮
            GUILayout.Space(20);
            if (GUILayout.Button("添加依赖", GUILayout.Width(200)))
                C_CustomTools.Modification();
        }
    }
}