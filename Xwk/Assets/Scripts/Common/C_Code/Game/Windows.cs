using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;

public static class Windows
{
    //private static List<WindowConfigItem> s_WindowList = null;
    //public static void Create(List<WindowConfigItem> list)
    //{
    //    list.Sort(Compare);

    //    s_WindowList = list;

    //    Pull();
    //}

    //public static void Pull()
    //{
    //    if (s_WindowList.Count > 1)
    //    {
    //        WindowConfigItem item = s_WindowList[0];

    //        int uiType = (item.Type % 10000) / 1000;
    //        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_Window" + uiType.ToString(), item);

    //        s_WindowList.RemoveAt(0);
    //    }
    //}

    //private static int Compare(WindowConfigItem item1, WindowConfigItem item2)
    //{

    //    if (item1.Priority < item2.Priority)
    //    {
    //        return -1;
    //    }
    //    else if (item1.Priority > item2.Priority)
    //    {
    //        return 1;
    //    }
    //    else
    //    {
    //        return 0;
    //    }
    //}
}
