using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class ServerHelper
{
    public static void ErrorCodeTips(int errorCode)
    {
        Tips.Create("LOACAL_ERROR_" + errorCode);
    }

    public static void ErrorCodeDialogBox(int errorCode)
    {
        DialogBox.Create("LOACAL_HINT", "LOACAL_ERROR_" + errorCode);
    }

    public static bool AssessReturnCode(int code, bool tipsEnabled = true)
    {
        if (code == 0)
            return true;

        if (tipsEnabled)
        {
            if (code == 1285)
                ErrorCodeDialogBox(code);
            else
                ErrorCodeTips(code);
        }

        return false;
    }
}