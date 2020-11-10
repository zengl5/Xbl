using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using YB.XWK.MainScene;

public class SpriteEditor : Editor
{

    [MenuItem("小伴龙Tool/精灵获取/获取灵娃")]
    public static void GetLingwa()
    {
        PlayerPrefs.SetString(LocalData.m_Collect_Spirit_Prefs,WizardItemName.Wizard_lingwa);
    }

    [MenuItem("小伴龙Tool/精灵获取/获取仙草娃娃")]
    public static void GetXCWW()
    {
        PlayerPrefs.SetString(LocalData.m_Collect_Spirit_Prefs, WizardItemName.Wizard_xcww);
    }

    [MenuItem("小伴龙Tool/精灵获取/获取海宝")]
    public static void GetHaibao()
    {
        PlayerPrefs.SetString(LocalData.m_Collect_Spirit_Prefs, WizardItemName.Wizard_jl_hxjl);
    }

    [MenuItem("小伴龙Tool/精灵获取/获取年兽宝宝")]
    public static void Get1()
    {
        PlayerPrefs.SetString(LocalData.m_Collect_Spirit_Prefs, WizardItemName.Wizard_nianshou);
    }

    [MenuItem("小伴龙Tool/精灵获取/获取红包精灵")]
    public static void Get2()
    {
        PlayerPrefs.SetString(LocalData.m_Collect_Spirit_Prefs, WizardItemName.Wizard_hongbao);
    }

    [MenuItem("小伴龙Tool/精灵获取/获取西西狼")]
    public static void Get3()
    {
        PlayerPrefs.SetString(LocalData.m_Collect_Spirit_Prefs, WizardItemName.Wizard_xixilang);
    }

    [MenuItem("小伴龙Tool/精灵获取/清空3个数据")]
    public static void Clear()
    {
        //File.Delete(Application.dataPath + "/../DownloadFiles/data");

        //DirectoryInfo DIR=new DirectoryInfo(Application.dataPath + "/../DownloadFiles/data");
        //DIR.Delete();
        PlayerPrefs.SetString(LocalData.m_Collect_Spirit_Prefs, "");
        PlayerPrefs.SetString(SpriteLingqiMgr.OfflineTime, "");

    }
}
