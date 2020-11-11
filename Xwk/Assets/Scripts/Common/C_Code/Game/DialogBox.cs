using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using System;

public enum DialogBoxType
{
    OneButton,
    TwoButton,
    Tile_OneButton,
    Tile_TwoButton
}

public static class DialogBox
{
    public static void Create(string title, string content)
    {
        Create(title, content, DialogBoxType.Tile_OneButton, null, null, "", "");
    }

    public static void Create(string title, string content, Action callback)
    {
        Create(title, content, DialogBoxType.Tile_OneButton, callback, null, "", "");
    }

    public static void Create(string title, string content, Action callback, string buttonName)
    {
        Create(title, content, DialogBoxType.Tile_OneButton, callback, null, buttonName, "");
    }

    public static void Create(string title, string content, Action callback1, Action callback2, string oneButtonName, string twoButtonName)
    {
        Create(title, content, DialogBoxType.Tile_TwoButton, callback1, callback2, oneButtonName, twoButtonName);
    }

    public static void Create(string content, Action callback, string buttonName)
    {
        Create("", content, DialogBoxType.OneButton, callback, null, buttonName, "");
    }

    public static void Create(string content, Action callback1, Action callback2, string oneButtonName, string twoButtonName)
    {
        Create("", content, DialogBoxType.TwoButton, callback1, callback2, oneButtonName, twoButtonName);
    }

    public static void Create(string title, string content, DialogBoxType type, Action callback1, Action callback2, string oneButtonName, string twoButtonName)
    {
        switch (type)
        {
            case DialogBoxType.OneButton:
                C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_DialogBox_OneButton", content, callback1, oneButtonName);
                break;
            case DialogBoxType.TwoButton:
                C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_DialogBox_TwoButton", content, callback1, callback2, oneButtonName, twoButtonName);
                break;
            case DialogBoxType.Tile_OneButton:
                C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_DialogBox_Title_OneButton", title, content, callback1, oneButtonName);
                break;
            case DialogBoxType.Tile_TwoButton:
                C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_DialogBox_Title_TwoButton", title, content, callback1, callback2, oneButtonName, twoButtonName);
                break;
        }
    }
}
