using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class C_MonoSingleton<T> : MonoBehaviour where T : Component
{
    private static T s_Instance = null;

    public static T Instance
    {
        get { return GetInstance(); }
        set { s_Instance = value; }
    }

    public static T GetInstance()
    {
        if (s_Instance == null)
        {
            s_Instance = FindObjectOfType(typeof(T)) as T;
            if (s_Instance == null)
            {
                s_Instance = new GameObject(typeof(T).Name).AddComponent<T>();

                GameObject gameObject2 = GameObject.Find("C_BootObj");
                if (gameObject2 == null)
                    gameObject2 = new GameObject("C_BootObj");

                if(Application.isPlaying)
                    DontDestroyOnLoad(gameObject2);

                s_Instance.transform.SetParent(gameObject2.transform);
            }
        }

        return s_Instance;
    }

    protected virtual void Awake()
    {
        if (s_Instance != null && s_Instance.gameObject != base.gameObject)
        {
            if (Application.isPlaying)
                Destroy(base.gameObject);
            else
                DestroyImmediate(base.gameObject);
        }
        else if (s_Instance == null)
        {
            s_Instance = base.GetComponent<T>();
        }

        DontDestroyOnLoad(base.gameObject);

        Init();
    }

    protected virtual void OnDestroy()
    {
        if (s_Instance != null)
            s_Instance = null;
    }

    public static bool HasInstance()
    {
        return s_Instance != null;
    }

    protected virtual void Init()
    {
    }
}