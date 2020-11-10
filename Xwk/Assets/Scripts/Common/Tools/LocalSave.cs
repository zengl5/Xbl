using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
/// <summary>
/// 本地保存 
/// 2017-11-23 黄志龙
/// 824697930@qq.com
/// </summary>
public static class LocalSave
{
    // Methods
    public static void Clear()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static bool ExistFile(string fileName)
    {
        return File.Exists(GetLocalPath() + fileName);
    }

    public static bool GetBool(string key)
    {
        return (HasKey(key) && (PlayerPrefs.GetInt(key) == 1));
    }

    public static byte[] GetFileByBytes(string fileName)
    {
        byte[] buffer = null;
        try
        {
            if (File.Exists(fileName))
            {
                buffer = File.ReadAllBytes(fileName);
            }
        }
        catch (Exception exception)
        {
            Debug.LogError("GetFile fail" + exception.Message);
        }
        return buffer;
    }

    public static byte[] GetFileByBytesPersist(string fileName)
    {
        byte[] buffer = null;
        try
        {
            string path = GetLocalPath() + fileName;
            if (File.Exists(path))
            {
                buffer = File.ReadAllBytes(path);
            }
        }
        catch (Exception exception)
        {
            Debug.LogError("GetFile fail" + exception.Message);
        }
        return buffer;
    }

    public static string GetFilePersist(string fileName)
    {
        string str = string.Empty;
        try
        {
            str = File.ReadAllText(GetLocalPath() + fileName);
        }
        catch (Exception exception)
        {
            Debug.LogError("GetFile fail" + exception.Message);
        }
        return str;
    }

    public static float GetFloat(string key)
    {
        if (HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }
        return 0f;
    }

    public static int GetInt(string key)
    {
        if (HasKey(key))
        {
            return PlayerPrefs.GetInt(key);
        }
        return 0;
    }

    public static string GetLocalPath()
    {
        return (Application.persistentDataPath + "/");
    }

    public static string GetString(string key)
    {
        if (HasKey(key))
        {
            return PlayerPrefs.GetString(key);
        }
        return string.Empty;
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static bool MakeSureDir(string filePath)
    {
        string directoryName = Path.GetDirectoryName(filePath);
        if (Directory.Exists(directoryName))
        {
            return true;
        }
        bool flag = false;
        try
        {
            Directory.CreateDirectory(directoryName);
            flag = true;
        }
        catch
        {
        }
        return flag;
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }

    public static bool SaveFile(string fileName, byte[] bytes)
    {
        try
        {
            if (MakeSureDir(fileName))
            {
                File.WriteAllBytes(fileName, bytes);
            }
        }
        catch (Exception exception)
        {
            Debug.LogError("saveFile fail" + exception.Message);
            return false;
        }
        return true;
    }

    public static bool SaveFile(string fileName, string content)
    {
        try
        {
            if (MakeSureDir(fileName))
            {
                File.WriteAllText(fileName, content);
            }
        }
        catch (Exception exception)
        {
            Debug.LogError("saveFile fail" + exception.Message);
            return false;
        }
        return true;
    }

    public static bool SaveFilePersist(string fileName, byte[] bytes)
    {
        try
        {
            fileName = GetLocalPath() + fileName;
            if (MakeSureDir(fileName))
            {
                File.WriteAllBytes(fileName, bytes);
            }
        }
        catch (Exception exception)
        {
            Debug.LogError("saveFile fail" + exception.Message);
            return false;
        }
        return true;
    }

    public static bool SaveFilePersist(string fileName, string content)
    {
        try
        {
            fileName = GetLocalPath() + fileName;
            if (MakeSureDir(fileName))
            {
                File.WriteAllText(fileName, content);
            }
        }
        catch (Exception exception)
        {
            Debug.LogError("saveFile fail" + exception.Message);
            return false;
        }
        return true;
    }

    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, !value ? 0 : 1);
    }

    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
}

 

