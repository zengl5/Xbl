using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Singleton<T> where T : class, new()
{
    private static T s_Instance = null;

    public static T Instance
    {
        get
        {
            if (s_Instance == null)
                CreateInstance();

            return s_Instance;
        }
    }
   

    protected C_Singleton()
    {
    }

    public static void CreateInstance()
    {
        if (s_Instance == null)
        {
            s_Instance = Activator.CreateInstance<T>();
            (s_Instance as C_Singleton<T>).Init();
        }
    }

    public static void DestroyInstance()
    {
        if (s_Instance != null)
        {
            (s_Instance as C_Singleton<T>).Destroy();
            s_Instance = (T)((object)null);
        }
    }

    public static T GetInstance()
    {
        if (s_Instance == null)
            CreateInstance();

        return s_Instance;
    }

    public static bool HasInstance()
    {
        return s_Instance != null;
    }

    protected virtual void Init()
    {
    }

    protected virtual void Destroy()
    {
    }
}