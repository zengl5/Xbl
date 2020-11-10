using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Assets.Scripts.C_Framework;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(AddDependenciesTools))]
public class EditorAddDependencies : Editor
{
    private AddDependenciesTools m_Component = null;

    private string m_strManifestFilePath = "";

    private string m_strFolderPath = "";
    private string m_strFilePath = "";

    private bool m_bShowBatch = true;
    private bool m_bShowSingle = true;

    public virtual void OnEnable()
    {
        m_Component = (AddDependenciesTools)target;
    }

    public override void OnInspectorGUI()
    {
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUI.skin.label.wordWrap = true;

        GUILayout.Space(20);

        GUILayout.Label("输入或选择Manifest文件:");
        m_strManifestFilePath = EditorToolsMgr.TextField(m_strManifestFilePath, GUILayout.Height(15));
        if (GUILayout.Button("Manifest文件选择", GUILayout.Height(30)))
        {
            string path = EditorUtility.OpenFilePanel("请选择Manifest文件", m_strManifestFilePath ?? Application.dataPath, "");
            if (!string.IsNullOrEmpty(path))
                m_strManifestFilePath = path;
        }

        GUILayout.Space(20);

        m_bShowBatch = EditorGUILayout.Foldout(m_bShowBatch, "选择文件夹");
        if (m_bShowBatch)
        {
            GUILayout.Label("输入或选择文件夹:");
            m_strFolderPath = EditorToolsMgr.TextField(m_strFolderPath, GUILayout.Height(15));
            if (GUILayout.Button("文件夹选择", GUILayout.Height(30)))
            {
                string path = EditorUtility.OpenFolderPanel("请选择文件夹", m_strFolderPath ?? Application.dataPath, "");
                if (!string.IsNullOrEmpty(path))
                    m_strFolderPath = path;
            }

            GUILayout.Space(10);

            if (GUILayout.Button("批量添加依赖文件", GUILayout.Height(30)) && CheckFolderPath(m_strManifestFilePath, m_strFolderPath))
                m_Component.BatchModify(m_strManifestFilePath, m_strFolderPath);
        }

        GUILayout.Space(20);

        m_bShowSingle = EditorGUILayout.Foldout(m_bShowSingle, "选择文件");
        if (m_bShowSingle)
        {
            GUILayout.Label("输入或选择文件:");
            m_strFilePath = GUILayout.TextField(m_strFilePath, GUILayout.Height(15));
            if (GUILayout.Button("文件选择", GUILayout.Height(30))) {
                string path = EditorUtility.OpenFilePanel("请选择文件", m_strFilePath ?? Application.dataPath, "");

                if (!string.IsNullOrEmpty(path))
                    m_strFilePath = path;
            }

            GUILayout.Space(10);

            if (GUILayout.Button("添加依赖文件", GUILayout.Height(30)) && CheckFilePath(m_strManifestFilePath, m_strFilePath))
                m_Component.SingleModify(m_strManifestFilePath, m_strFilePath);
        }

        GUILayout.Space(20);
    }

    private bool CheckFolderPath(string manifestFilePath, string targetPath)
    {
        if (string.IsNullOrEmpty(manifestFilePath))
        {
            Debug.Log("没有选择Manifest文件！");
            return false;
        }

        Debug.Log("当前Manifest文件：" + manifestFilePath);

        if (string.IsNullOrEmpty(targetPath))
        {
            Debug.Log("没有输入文件夹！");
            return false;
        }

        Debug.Log("当前文件夹：" + targetPath);

        return true;
    }

    private bool CheckFilePath(string manifestFilePath, string targetPath)
    {
        if (string.IsNullOrEmpty(manifestFilePath))
        {
            Debug.Log("没有选择Manifest文件！");
            return false;
        }

        Debug.Log("当前Manifest文件：" + manifestFilePath);

        if (string.IsNullOrEmpty(targetPath)) {
            Debug.Log("没有输入文件！");
            return false;
        }

        Debug.Log("当前文件：" + targetPath);

        return true;
    }
}

public class AddDependenciesTools : MonoBehaviour
{
    private List<string> m_DependencieList = new List<string>();

    public void BatchModify(string manifestFilePath, string folderPath)
    {
        if (!File.Exists(manifestFilePath))
        {
            Debug.Log("批量添加依赖失败！Manifest文件不存在！");
            return;
        }

        if (!Directory.Exists(folderPath))
        {
            Debug.Log("批量添加依赖失败！文件夹不存在！");
            return;
        }

        string[] filesPathVec = Directory.GetFiles(folderPath, "*.txt");
        if (filesPathVec == null && filesPathVec.Length == 0)
        {
            Debug.Log("批量添加依赖失败！文件夹内不存在txt文件！");
            return;
        }

        int count = 0;
        foreach (var path in filesPathVec)
        {
            if (PerModify(manifestFilePath, path))
                count++;
        }

        Debug.Log("批量添加依赖结果: 总共" + filesPathVec.Length + "个, 成功" + count + "个");
    }

    public void SingleModify(string manifestFilePath, string filePath)
    {
        if (!File.Exists(manifestFilePath))
        {
            Debug.Log("添加依赖失败！Manifest文件不存在！");
            return;
        }

        if (!File.Exists(filePath))
        {
            Debug.Log("添加依赖失败！文件不存在！");
            return;
        }

        bool result = PerModify(manifestFilePath, filePath);

        Debug.Log("添加依赖结果" + (result ? "成功!" : "失败!"));
    }

    private bool PerModify(string manifestFilePath, string filePath)
    {
        string[] tempItems = File.ReadAllLines(filePath);

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
            Debug.Log(filePath + ">>>>>> items.Length = 0！");
            return false;
        }

        AssetBundle manifestBundle = AssetBundle.LoadFromFile(manifestFilePath);
        if (manifestBundle != null)
        {
            AssetBundleManifest assetBundleManifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");

            manifestBundle.Unload(false);

            m_DependencieList.Clear();

            for (int i = 0; i < itemList.Count; i++)
            {
                string itemName = itemList[i];

                if (string.IsNullOrEmpty(itemName) || m_DependencieList.Contains(itemName) || Hash128.Parse("0") == assetBundleManifest.GetAssetBundleHash(itemName))
                    continue;

                m_DependencieList.Add(itemName);

                string[] names = assetBundleManifest.GetAllDependencies(itemName);

                foreach (var path in names)
                {
                    if (!m_DependencieList.Contains(path))
                        m_DependencieList.Add(path);
                }
            }
        }

        File.Delete(filePath);

        StreamWriter sr = File.CreateText(filePath);
        foreach (var path in m_DependencieList)
        {
            if (!string.IsNullOrEmpty(path))
                sr.WriteLine(path);
        }

        sr.Close();

        return true;
    }
}

#endif