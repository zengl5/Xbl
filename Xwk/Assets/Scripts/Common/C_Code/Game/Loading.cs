using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;

public class Loading
{
    public static string c_Description = "";
    public static float c_Rate = 0;

    public static void Create(string description = "", float rate = 0)
    {
        c_Description = description;
        c_Rate = rate;

        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_Loading");
    }

    public static void Close()
    {
        c_Description = "";
        c_Rate = 0;

        C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI("UI_Loading");
    }
}
