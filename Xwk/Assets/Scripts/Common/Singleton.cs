using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//public class Singleton<T> : ISingleton where T : ISingleton, new()
//{
//    // Fields
//    private static T _instance;

//    // Properties
//    public static T Instance
//    {
//        get
//        {
//            if (Singleton<T>._instance == null)
//            {
//                Singleton<T>._instance = Activator.CreateInstance<T>();
//            }
//            return Singleton<T>._instance;
//        }
//    }
//}
public class Singleton<T> where T : new()
{
    // Fields
    private static T _instance;

    // Methods
    static Singleton()
    {
        Singleton<T>._instance = default(T);
    }

    public virtual void Release()
    {
        Singleton<T>._instance = default(T);
    }

    // Properties
    public static T Instance
    {
        get
        {
            if (Singleton<T>._instance == null)
            {
                Singleton<T>._instance = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
            }
            return Singleton<T>._instance;
        }
    }
}

 


