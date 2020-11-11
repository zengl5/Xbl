using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ConversionChineseToPinyin {
    static string _PYFilePath = "Word/word";
    static bool _ReadFileFlag = true;
    //汉字对应拼音数据队列
    public static Hashtable m_HashTable = new Hashtable();
	//读取配置的汉字，拼音库
    private static bool ReadFile()
    {
        string strAsset;
        TextAsset textAsset = Resources.Load(_PYFilePath) as TextAsset;
        if (textAsset == null)
        {
            Debug.LogError(_PYFilePath + "不存在，请检查该汉字拼音库的路径");
            return false;
        }
        strAsset = textAsset.text;
        if (string.IsNullOrEmpty(strAsset))
        {
            Debug.LogError(_PYFilePath + "剧情的汉字拼音库文件内容为空");
            return false;
        }
        string[] temp = strAsset.Split(new char[] { '|' });

        foreach (string var in temp)
        {
            string key = var.Substring(0, var.IndexOf(","));
            string value = var.Substring(1 + var.IndexOf(","));
            m_HashTable[key] = value;
        }
        return true;
    }
    static void InitHashTable()
    {
        if (m_HashTable != null && m_HashTable.Count <= 0)
        {
            ReadFile();
        }
    }
    //汉字转化为拼音
    public static string ToPinYin(string hanzi)
    {
        InitHashTable();
        StringBuilder sb = new StringBuilder();

        foreach (char var in hanzi)
        {
            string s = m_HashTable[var.ToString()] as string;

            if (s != null)
            {
                sb.Append(s);
            }
        }

        return sb.ToString();
    }
    public static string ToPinYin(char hanzi)
    {
        return ToPinYin(hanzi.ToString());
    }
}
