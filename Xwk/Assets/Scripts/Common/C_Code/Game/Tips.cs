using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;

public static class Tips
{
    public static void Create(string content)
    {
        string strContent = C_Localization.GetLocalization(content);
        if (string.IsNullOrEmpty(strContent))
            return;

        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_Tips", content);
    }
}
