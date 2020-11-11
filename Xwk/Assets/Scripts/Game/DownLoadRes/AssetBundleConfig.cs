using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Assets.Scripts.C_Framework;
public class BundleConfig
{
    public BundleConfig(string[] s1, string[] s2)
    {
        pathArray = s1;
        typeArray = s2;
    }
    public string[] pathArray;
    public string[] typeArray;
}
public class ABCommonConfig: C_Singleton<ABCommonConfig>
{
    public static string EfBundleType = "heroeffect";//特效Bundle类型
    public string SepBundleType = "sprite";
    public string SpWindowBundleType = "window";
}
public class AssetBundleConfig : C_MonoSingleton<AssetBundleConfig>{

    public BundleConfig SeparationConfig;
    public BundleConfig GoldhoopbarConfig;
    public BundleConfig SpriteConfig;
    public BundleConfig BaibianHuluConfig;
    public BundleConfig PiercingeyeConfig;

    DirectoryInfo dir;
    string EditordirPath;
    string MobiledirPath;
    // Use this for initialization
    void Start () {
        if (Application.isEditor)
        {
            dir = new DirectoryInfo(Application.dataPath);
            EditordirPath = dir.Parent + "/PackagingResources/Editor";
        }
        else
        {
            MobiledirPath = Application.persistentDataPath + "/PackagingResources/game";
        }
        AddSeparationConfig();
        AddGoldhoopbarConfig();
        AddSpriteConfig();
        AddBaibianHuluConfig();
        AddPiercingeyeConfig();

    }
    void AddSeparationConfig()
    {
        if (Application.isEditor)
        {
            string[] pathArray = new string[] { EditordirPath + "/separationres/separation", EditordirPath + "/hero_effectres/hero_effect" };
            string[] typeArray = new string[] { "separation", "heroeffect" };
            SeparationConfig = new BundleConfig(pathArray, typeArray);
        }
        else
        {
            string[] pathArray = new string[] { MobiledirPath + "/separationres/separation", MobiledirPath + "/hero_effectres/hero_effect" };
            string[] typeArray = new string[] { "separation", "heroeffect" };
            SeparationConfig = new BundleConfig(pathArray, typeArray);
        }
    }
    void AddGoldhoopbarConfig()
    {
        if (Application.isEditor)
        {
            string[] pathArray = new string[] { EditordirPath + "/goldhoopbares/goldhoopbar", EditordirPath + "/hero_effectres/hero_effect" };
            string[] typeArray = new string[] { "goldhoopbar", "heroeffect" };
            GoldhoopbarConfig = new BundleConfig(pathArray, typeArray);
        }
        else
        {
            string[] pathArray = new string[] { MobiledirPath + "/goldhoopbares/goldhoopbar", MobiledirPath + "/hero_effectres/hero_effect" };
            string[] typeArray = new string[] { "goldhoopbar", "heroeffect" };
            GoldhoopbarConfig = new BundleConfig(pathArray, typeArray);
        }
    }
    void AddSpriteConfig()
    {
        if (Application.isEditor)
        {
            string[] pathArray = new string[] { EditordirPath + "/spriteres/sprite", EditordirPath + "/hero_effectres/hero_effect" };
            string[] typeArray = new string[] { "sprite", "heroeffect" };
            SpriteConfig = new BundleConfig(pathArray, typeArray);
        }
        else
        {
            string[] pathArray = new string[] { MobiledirPath + "/spriteres/sprite", MobiledirPath + "/hero_effectres/hero_effect" };
            string[] typeArray = new string[] { "sprite", "heroeffect" };
            SpriteConfig = new BundleConfig(pathArray, typeArray);
        }
    }
    void AddBaibianHuluConfig()
    {
        if (Application.isEditor)
        {
            string[] pathArray = new string[] { EditordirPath + "/baibianhulures/baibianhulu", EditordirPath + "/hero_effectres/hero_effect" };
            string[] typeArray = new string[] { "baibianhulu", "heroeffect" };
            BaibianHuluConfig = new BundleConfig(pathArray, typeArray);
        }
        else
        {
            string[] pathArray = new string[] { MobiledirPath + "/baibianhulures/baibianhulu", MobiledirPath + "/hero_effectres/hero_effect" };
            string[] typeArray = new string[] { "baibianhulu", "heroeffect" };
            BaibianHuluConfig = new BundleConfig(pathArray, typeArray);
        }
    }
    void AddPiercingeyeConfig()
    {
        if (Application.isEditor)
        {
            string[] pathArray = new string[] { EditordirPath + "/piercingeyeres/piercingeye", EditordirPath + "/hero_effectres/hero_effect" };
            string[] typeArray = new string[] { "Piercingeye", "heroeffect" };
            PiercingeyeConfig = new BundleConfig(pathArray, typeArray);
        }
        else
        {
            string[] pathArray = new string[] { MobiledirPath + "/piercingeyeres/piercingeye", MobiledirPath + "/hero_effectres/hero_effect" };
            string[] typeArray = new string[] { "Piercingeye", "heroeffect" };
            PiercingeyeConfig = new BundleConfig(pathArray, typeArray);
        }
    }

}
 
