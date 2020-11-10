using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldGuideConfigItem
{
    public int ID = 0;
    public string Name = "";
    public int Type = 0;
    public int BallType = 0;
}

public class FieldGuideConfig
{
    public static string Name = "field_guide_config";

    public static List<FieldGuideConfigItem> WordBallConfigItemList = new List<FieldGuideConfigItem>();

    public FieldGuideConfig()
    {
        WordBallConfigItemList.Clear();

        FieldGuideConfigItem item1 = new FieldGuideConfigItem();
        item1.ID = 1;
        item1.Name = "a";
        WordBallConfigItemList.Add(item1);

        FieldGuideConfigItem item2 = new FieldGuideConfigItem();
        item2.ID = 2;
        item2.Name = "o";
        WordBallConfigItemList.Add(item2);

        FieldGuideConfigItem item3 = new FieldGuideConfigItem();
        item3.ID = 3;
        item3.Name = "e";
        WordBallConfigItemList.Add(item3);

        FieldGuideConfigItem item4 = new FieldGuideConfigItem();
        item4.ID = 4;
        item4.Name = "i";
        WordBallConfigItemList.Add(item4);

        FieldGuideConfigItem item5 = new FieldGuideConfigItem();
        item5.ID = 5;
        item5.Name = "u";
        WordBallConfigItemList.Add(item5);

        FieldGuideConfigItem item6 = new FieldGuideConfigItem();
        item6.ID = 6;
        item6.Name = "v";
        WordBallConfigItemList.Add(item6);

        //FieldGuideConfigItem item7 = new FieldGuideConfigItem();
        //item7.ID = 7;
        //item7.Name = "v1";
        //WordBallConfigItemList.Add(item7);

        //FieldGuideConfigItem item8 = new FieldGuideConfigItem();
        //item8.ID = 8;
        //item8.Name = "v2";
        //WordBallConfigItemList.Add(item8);

        //FieldGuideConfigItem item9 = new FieldGuideConfigItem();
        //item9.ID = 9;
        //item9.Name = "v3";
        //WordBallConfigItemList.Add(item9);

        FieldGuideConfigItem item10 = new FieldGuideConfigItem();
        item10.ID = 10;
        item10.Name = "b";
        item10.BallType = 1;
        WordBallConfigItemList.Add(item10);

        FieldGuideConfigItem item11 = new FieldGuideConfigItem();
        item11.ID = 11;
        item11.Name = "p";
        item11.BallType = 1;
        WordBallConfigItemList.Add(item11);

        FieldGuideConfigItem item12 = new FieldGuideConfigItem();
        item12.ID = 12;
        item12.Name = "m";
        item12.BallType = 1;
        WordBallConfigItemList.Add(item12);

        FieldGuideConfigItem item13 = new FieldGuideConfigItem();
        item13.ID = 13;
        item13.Name = "f";
        item13.BallType = 1;
        WordBallConfigItemList.Add(item13);

        FieldGuideConfigItem item14 = new FieldGuideConfigItem();
        item14.ID = 14;
        item14.Name = "d";
        item14.BallType = 1;
        WordBallConfigItemList.Add(item14);

        FieldGuideConfigItem item15 = new FieldGuideConfigItem();
        item15.ID = 15;
        item15.Name = "t";
        item15.BallType = 1;
        WordBallConfigItemList.Add(item15);

        FieldGuideConfigItem item16 = new FieldGuideConfigItem();
        item16.ID = 16;
        item16.Name = "n";
        item16.BallType = 1;
        WordBallConfigItemList.Add(item16);

        FieldGuideConfigItem item17 = new FieldGuideConfigItem();
        item17.ID = 17;
        item17.Name = "l";
        item17.BallType = 1;
        WordBallConfigItemList.Add(item17);

        FieldGuideConfigItem item18 = new FieldGuideConfigItem();
        item18.ID = 18;
        item18.Name = "g";
        item18.BallType = 1;
        WordBallConfigItemList.Add(item18);

        FieldGuideConfigItem item19 = new FieldGuideConfigItem();
        item19.ID = 19;
        item19.Name = "k";
        item19.BallType = 1;
        WordBallConfigItemList.Add(item19);

        FieldGuideConfigItem item20 = new FieldGuideConfigItem();
        item20.ID = 20;
        item20.Name = "h";
        item20.BallType = 1;
        WordBallConfigItemList.Add(item20);

        FieldGuideConfigItem item21 = new FieldGuideConfigItem();
        item21.ID = 21;
        item21.Name = "j";
        item21.BallType = 1;
        WordBallConfigItemList.Add(item21);

        FieldGuideConfigItem item22 = new FieldGuideConfigItem();
        item22.ID = 22;
        item22.Name = "q";
        item22.BallType = 1;
        WordBallConfigItemList.Add(item22);

        FieldGuideConfigItem item23 = new FieldGuideConfigItem();
        item23.ID = 23;
        item23.Name = "x";
        item23.BallType = 1;
        WordBallConfigItemList.Add(item23);

        FieldGuideConfigItem item24 = new FieldGuideConfigItem();
        item24.ID = 24;
        item24.Name = "zh";
        item24.BallType = 1;
        WordBallConfigItemList.Add(item24);

        FieldGuideConfigItem item25 = new FieldGuideConfigItem();
        item25.ID = 25;
        item25.Name = "ch";
        item25.BallType = 1;
        WordBallConfigItemList.Add(item25);

        FieldGuideConfigItem item26 = new FieldGuideConfigItem();
        item26.ID = 26;
        item26.Name = "sh";
        item26.BallType = 1;
        WordBallConfigItemList.Add(item26);

        FieldGuideConfigItem item27 = new FieldGuideConfigItem();
        item27.ID = 27;
        item27.Name = "r";
        item27.BallType = 1;
        WordBallConfigItemList.Add(item27);

        FieldGuideConfigItem item28 = new FieldGuideConfigItem();
        item28.ID = 28;
        item28.Name = "z";
        item28.BallType = 1;
        WordBallConfigItemList.Add(item28);

        FieldGuideConfigItem item29 = new FieldGuideConfigItem();
        item29.ID = 29;
        item29.Name = "c";
        item29.BallType = 1;
        WordBallConfigItemList.Add(item29);

        FieldGuideConfigItem item30 = new FieldGuideConfigItem();
        item30.ID = 30;
        item30.Name = "s";
        item30.BallType = 1;
        WordBallConfigItemList.Add(item30);

        FieldGuideConfigItem item31 = new FieldGuideConfigItem();
        item31.ID = 31;
        item31.Name = "y";
        item31.BallType = 1;
        WordBallConfigItemList.Add(item31);

        FieldGuideConfigItem item32 = new FieldGuideConfigItem();
        item32.ID = 32;
        item32.Name = "w";
        item32.BallType = 1;
        WordBallConfigItemList.Add(item32);

        //FieldGuideConfigItem item33 = new FieldGuideConfigItem();
        //item33.ID = 33;
        //item33.Name = "w1";
        //item33.BallType = 1;
        //WordBallConfigItemList.Add(item11);

        //FieldGuideConfigItem item34 = new FieldGuideConfigItem();
        //item34.ID = 34;
        //item34.Name = "w2";
        //item34.BallType = 1;
        //WordBallConfigItemList.Add(item34);

        //FieldGuideConfigItem item35 = new FieldGuideConfigItem();
        //item35.ID = 35;
        //item35.Name = "w3";
        //item35.BallType = 1;
        //WordBallConfigItemList.Add(item35);

        //FieldGuideConfigItem item36 = new FieldGuideConfigItem();
        //item36.ID = 36;
        //item36.Name = "w4";
        //item36.BallType = 1;
        //WordBallConfigItemList.Add(item36);

        FieldGuideConfigItem item37 = new FieldGuideConfigItem();
        item37.ID = 37;
        item37.Name = "ai";
        item37.BallType = 2;
        WordBallConfigItemList.Add(item37);

        FieldGuideConfigItem item38 = new FieldGuideConfigItem();
        item38.ID = 38;
        item38.Name = "ei";
        item38.BallType = 2;
        WordBallConfigItemList.Add(item38);

        FieldGuideConfigItem item39 = new FieldGuideConfigItem();
        item39.ID = 39;
        item39.Name = "ui";
        item39.BallType = 2;
        WordBallConfigItemList.Add(item39);

        FieldGuideConfigItem item40 = new FieldGuideConfigItem();
        item40.ID = 40;
        item40.Name = "ao";
        item40.BallType = 2;
        WordBallConfigItemList.Add(item40);

        FieldGuideConfigItem item41 = new FieldGuideConfigItem();
        item41.ID = 41;
        item41.Name = "ou";
        item41.BallType = 2;
        WordBallConfigItemList.Add(item41);

        FieldGuideConfigItem item42 = new FieldGuideConfigItem();
        item42.ID = 42;
        item42.Name = "iu";
        item42.BallType = 2;
        WordBallConfigItemList.Add(item42);

        FieldGuideConfigItem item43 = new FieldGuideConfigItem();
        item43.ID = 43;
        item43.Name = "ie";
        item43.BallType = 2;
        WordBallConfigItemList.Add(item43);

        FieldGuideConfigItem item44 = new FieldGuideConfigItem();
        item44.ID = 44;
        item44.Name = "ve";
        item44.BallType = 2;
        WordBallConfigItemList.Add(item44);

        FieldGuideConfigItem item45 = new FieldGuideConfigItem();
        item45.ID = 45;
        item45.Name = "er";
        item45.BallType = 2;
        WordBallConfigItemList.Add(item45);

        FieldGuideConfigItem item46 = new FieldGuideConfigItem();
        item46.ID = 46;
        item46.Name = "an";
        item46.BallType = 3;
        WordBallConfigItemList.Add(item46);

        FieldGuideConfigItem item47 = new FieldGuideConfigItem();
        item47.ID = 47;
        item47.Name = "en";
        item47.BallType = 3;
        WordBallConfigItemList.Add(item47);

        FieldGuideConfigItem item48 = new FieldGuideConfigItem();
        item48.ID = 48;
        item48.Name = "in";
        item48.BallType = 3;
        WordBallConfigItemList.Add(item48);

        FieldGuideConfigItem item49 = new FieldGuideConfigItem();
        item49.ID = 49;
        item49.Name = "un";
        item49.BallType = 3;
        WordBallConfigItemList.Add(item49);

        FieldGuideConfigItem item50 = new FieldGuideConfigItem();
        item50.ID = 50;
        item50.Name = "vn";
        item50.BallType = 3;
        WordBallConfigItemList.Add(item50);

        FieldGuideConfigItem item51 = new FieldGuideConfigItem();
        item51.ID = 51;
        item51.Name = "ang";
        item51.BallType = 3;
        WordBallConfigItemList.Add(item51);

        FieldGuideConfigItem item52 = new FieldGuideConfigItem();
        item52.ID = 52;
        item52.Name = "eng";
        item52.BallType = 3;
        WordBallConfigItemList.Add(item52);

        FieldGuideConfigItem item53 = new FieldGuideConfigItem();
        item53.ID = 53;
        item53.Name = "ing";
        item53.BallType = 3;
        WordBallConfigItemList.Add(item53);

        FieldGuideConfigItem item54 = new FieldGuideConfigItem();
        item54.ID = 54;
        item54.Name = "ong";
        item54.BallType = 3;
        WordBallConfigItemList.Add(item54);

        FieldGuideConfigItem item55 = new FieldGuideConfigItem();
        item55.ID = 55;
        item55.Name = "zhi";
        item55.BallType = 4;
        WordBallConfigItemList.Add(item55);

        FieldGuideConfigItem item56 = new FieldGuideConfigItem();
        item56.ID = 56;
        item56.Name = "chi";
        item56.BallType = 4;
        WordBallConfigItemList.Add(item56);

        FieldGuideConfigItem item57 = new FieldGuideConfigItem();
        item57.ID = 57;
        item57.Name = "shi";
        item57.BallType = 4;
        WordBallConfigItemList.Add(item57);

        FieldGuideConfigItem item58 = new FieldGuideConfigItem();
        item58.ID = 58;
        item58.Name = "ri";
        item58.BallType = 4;
        WordBallConfigItemList.Add(item58);

        FieldGuideConfigItem item59 = new FieldGuideConfigItem();
        item59.ID = 59;
        item59.Name = "zi";
        item59.BallType = 4;
        WordBallConfigItemList.Add(item59);

        FieldGuideConfigItem item60 = new FieldGuideConfigItem();
        item60.ID = 60;
        item60.Name = "ci";
        item60.BallType = 4;
        WordBallConfigItemList.Add(item60);

        FieldGuideConfigItem item61 = new FieldGuideConfigItem();
        item61.ID = 61;
        item61.Name = "si";
        item61.BallType = 4;
        WordBallConfigItemList.Add(item61);

        FieldGuideConfigItem item62 = new FieldGuideConfigItem();
        item62.ID = 62;
        item62.Name = "yi";
        item62.BallType = 4;
        WordBallConfigItemList.Add(item62);

        FieldGuideConfigItem item63 = new FieldGuideConfigItem();
        item63.ID = 63;
        item63.Name = "wu";
        item63.BallType = 4;
        WordBallConfigItemList.Add(item63);

        FieldGuideConfigItem item64 = new FieldGuideConfigItem();
        item64.ID = 64;
        item64.Name = "yu";
        item64.BallType = 4;
        WordBallConfigItemList.Add(item64);

        FieldGuideConfigItem item65 = new FieldGuideConfigItem();
        item65.ID = 65;
        item65.Name = "ye";
        item65.BallType = 4;
        WordBallConfigItemList.Add(item65);

        FieldGuideConfigItem item66 = new FieldGuideConfigItem();
        item66.ID = 66;
        item66.Name = "yue";
        item66.BallType = 4;
        WordBallConfigItemList.Add(item66);

        FieldGuideConfigItem item67 = new FieldGuideConfigItem();
        item67.ID = 67;
        item67.Name = "yuan";
        item67.BallType = 4;
        WordBallConfigItemList.Add(item67);

        FieldGuideConfigItem item68 = new FieldGuideConfigItem();
        item68.ID = 68;
        item68.Name = "yin";
        item68.BallType = 4;
        WordBallConfigItemList.Add(item68);

        FieldGuideConfigItem item69 = new FieldGuideConfigItem();
        item69.ID = 69;
        item69.Name = "yun";
        item69.BallType = 4;
        WordBallConfigItemList.Add(item69);

        FieldGuideConfigItem item70 = new FieldGuideConfigItem();
        item70.ID = 70;
        item70.Name = "ying";
        item70.BallType = 4;
        WordBallConfigItemList.Add(item70);
    }

    public static void Load()
    {
        string strData = C_Save.LoadString(Name, C_LocalPath.StreamingAssetsConfigPath);
        if (!string.IsNullOrEmpty(strData))
        {
            WordBallConfigItemList.Clear();

            JsonData WordBallConfigItemListJD = C_Json.GetJsonKeyJsonData(strData, "WordBallConfigItemList");
            if (WordBallConfigItemListJD != null)
            {
                for (int i = 0; i < WordBallConfigItemListJD.Count; i++)
                {
                    if (WordBallConfigItemListJD[i] != null)
                    {
                        FieldGuideConfigItem item = new FieldGuideConfigItem();

                        item.ID = C_Json.GetJsonKeyInt(WordBallConfigItemListJD[i], "ID");
                        item.Name = C_Json.GetJsonKeyString(WordBallConfigItemListJD[i], "Name");
                        item.Type = C_Json.GetJsonKeyInt(WordBallConfigItemListJD[i], "Type");
                        item.BallType = C_Json.GetJsonKeyInt(WordBallConfigItemListJD[i], "BallType");

                        WordBallConfigItemList.Add(item);
                    }
                }
            }
        }
    }

    public static void Save()
    {
        C_Save.SaveString(Name, C_LocalPath.StreamingAssetsConfigPath, JsonMapper.ToJson(new FieldGuideConfig()), "");
    }

    public static FieldGuideConfigItem GetFieldGuideConfigItem(string tuJianName)
    {
        for (int i = 0; i < WordBallConfigItemList.Count; i++)
        {
            if (WordBallConfigItemList[i] != null && WordBallConfigItemList[i].Name == tuJianName)
                return WordBallConfigItemList[i];
        }

        return null;
    }

    public static List<FieldGuideConfigItem> GetWordBallConfigList(int ballType)
    {
        List<FieldGuideConfigItem> list = new List<FieldGuideConfigItem>();

        for (int i = 0; i < WordBallConfigItemList.Count; i++)
        {
            if (WordBallConfigItemList[i] != null && WordBallConfigItemList[i].BallType == ballType)
                list.Add(WordBallConfigItemList[i]);
        }

        return list;
    }
}