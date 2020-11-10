using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T s_Instance = null;

    public static T Instance
    {
        get { return GetInstance(); }
    }

    public static T GetInstance()
    {
        if (s_Instance == null)
        {
            s_Instance = FindObjectOfType(typeof(T)) as T;
            if (s_Instance == null)
            {
                s_Instance = new GameObject("_" + typeof(T).Name).AddComponent<T>();
                DontDestroyOnLoad(s_Instance.gameObject);

                GameObject gameObject2 = GameObject.Find("BootObj");
                if (gameObject2 != null)
                    s_Instance.transform.SetParent(gameObject2.transform);
            }
        }

        return s_Instance;
    }

    protected virtual void OnDestroy()
    {
        if (s_Instance != null)
            s_Instance = null;
    }

    public static T CreateInstance()
    {
        if (Instance != null)
            s_Instance.OnCreate();

        return s_Instance;
    }

    protected virtual void OnCreate()
    {
    }
}