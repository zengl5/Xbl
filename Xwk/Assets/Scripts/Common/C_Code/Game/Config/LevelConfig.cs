using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfig
{
    public static string Name = "level_config";

    public static int[] LevelMaxStar;
    public static string[] LevelName;
    public static string[] LevelUnlockStage;

    public static int[] GradeMaxStar;
    public static string[] GradeName;
    public static string[] GradeUnlockStage;

    public LevelConfig()
    {
        LevelMaxStar = new int[16];
        LevelMaxStar[0] = 2;
        LevelMaxStar[1] = 11;
        LevelMaxStar[2] = 21;
        LevelMaxStar[3] = 32;
        LevelMaxStar[4] = 44;
        LevelMaxStar[5] = 57;
        LevelMaxStar[6] = 71;
        LevelMaxStar[7] = 86;
        LevelMaxStar[8] = 102;
        LevelMaxStar[9] = 121;
        LevelMaxStar[10] = 143;
        LevelMaxStar[11] = 168;
        LevelMaxStar[12] = 196;
        LevelMaxStar[13] = 227;
        LevelMaxStar[14] = 264;
        LevelMaxStar[15] = 999999999;

        LevelName = new string[16];
        LevelName[0] = "LOACAL_LEVEL_CONFIG_LEVEL_NAME_0";
        LevelName[1] = "LOACAL_LEVEL_CONFIG_LEVEL_NAME_1";
        LevelName[2] = "LOACAL_LEVEL_CONFIG_LEVEL_NAME_2";
        LevelName[3] = "LOACAL_LEVEL_CONFIG_LEVEL_NAME_3";
        LevelName[4] = "LOACAL_LEVEL_CONFIG_LEVEL_NAME_4";
        LevelName[5] = "LOACAL_LEVEL_CONFIG_LEVEL_NAME_5";
        LevelName[6] = "LOACAL_LEVEL_CONFIG_LEVEL_NAME_6";
        LevelName[7] = "LOACAL_LEVEL_CONFIG_LEVEL_NAME_7";
        LevelName[8] = "LOACAL_LEVEL_CONFIG_LEVEL_NAME_8";
        LevelName[9] = "LOACAL_LEVEL_CONFIG_LEVEL_NAME_9";
        LevelName[10] = "LOACAL_LEVEL_CONFIG_LEVEL_NAME_10";
        LevelName[11] = "LOACAL_LEVEL_CONFIG_LEVEL_NAME_11";
        LevelName[12] = "LOACAL_LEVEL_CONFIG_LEVEL_NAME_12";
        LevelName[13] = "LOACAL_LEVEL_CONFIG_LEVEL_NAME_13";
        LevelName[14] = "LOACAL_LEVEL_CONFIG_LEVEL_NAME_14";
        LevelName[15] = "LOACAL_LEVEL_CONFIG_LEVEL_NAME_15";

        LevelUnlockStage = new string[16];
        LevelUnlockStage[0] = "a";
        LevelUnlockStage[1] = "a";
        LevelUnlockStage[2] = "a";
        LevelUnlockStage[3] = "iuv";
        LevelUnlockStage[4] = "iuv";
        LevelUnlockStage[5] = "iuv";
        LevelUnlockStage[6] = "shuangpin";
        LevelUnlockStage[7] = "shuangpin";
        LevelUnlockStage[8] = "shuangpin";
        LevelUnlockStage[9] = "jqx";
        LevelUnlockStage[10] = "jqx";
        LevelUnlockStage[11] = "jqx";
        LevelUnlockStage[12] = "yw";
        LevelUnlockStage[13] = "yw";
        LevelUnlockStage[14] = "yw";
        LevelUnlockStage[15] = "angengingong";

        GradeMaxStar = new int[6];
        GradeMaxStar[0] = 21;
        GradeMaxStar[1] = 57;
        GradeMaxStar[2] = 102;
        GradeMaxStar[3] = 168;
        GradeMaxStar[4] = 264;
        GradeMaxStar[5] = 999999999;

        GradeName = new string[6];
        GradeName[0] = "LOACAL_LEVEL_CONFIG_GRADE_NAME_0";
        GradeName[1] = "LOACAL_LEVEL_CONFIG_GRADE_NAME_1";
        GradeName[2] = "LOACAL_LEVEL_CONFIG_GRADE_NAME_2";
        GradeName[3] = "LOACAL_LEVEL_CONFIG_GRADE_NAME_3";
        GradeName[4] = "LOACAL_LEVEL_CONFIG_GRADE_NAME_4";
        GradeName[5] = "LOACAL_LEVEL_CONFIG_GRADE_NAME_5";

        GradeUnlockStage = new string[6];
        GradeUnlockStage[0] = "a";
        GradeUnlockStage[1] = "iuv";
        GradeUnlockStage[2] = "shuangpin";
        GradeUnlockStage[3] = "jqx";
        GradeUnlockStage[4] = "yw";
        GradeUnlockStage[5] = "angengingong";
    }

    public static void Load()
    {
        string strData = C_Save.LoadString(Name, C_LocalPath.StreamingAssetsConfigPath);
        if (!string.IsNullOrEmpty(strData))
        {
            JsonData levelMaxStarJD = C_Json.GetJsonKeyJsonData(strData, "LevelMaxStar");
            if (levelMaxStarJD != null)
            {
                LevelMaxStar = new int[levelMaxStarJD.Count];

                for (int i = 0; i < levelMaxStarJD.Count; i++)
                    LevelMaxStar[i] = (int)levelMaxStarJD[i];
            }

            JsonData levelNameJD = C_Json.GetJsonKeyJsonData(strData, "LevelName");
            if (levelNameJD != null)
            {
                LevelName = new string[levelNameJD.Count];

                for (int i = 0; i < levelNameJD.Count; i++)
                    LevelName[i] = (string)levelNameJD[i];
            }

            JsonData levelUnlockStageJD = C_Json.GetJsonKeyJsonData(strData, "LevelUnlockStage");
            if (levelUnlockStageJD != null)
            {
                LevelUnlockStage = new string[levelUnlockStageJD.Count];

                for (int i = 0; i < levelUnlockStageJD.Count; i++)
                    LevelUnlockStage[i] = (string)levelUnlockStageJD[i];
            }

            JsonData gradeMaxStarJD = C_Json.GetJsonKeyJsonData(strData, "GradeMaxStar");
            if (gradeMaxStarJD != null)
            {
                GradeMaxStar = new int[gradeMaxStarJD.Count];

                for (int i = 0; i < gradeMaxStarJD.Count; i++)
                    GradeMaxStar[i] = (int)gradeMaxStarJD[i];
            }

            JsonData gradeNameJD = C_Json.GetJsonKeyJsonData(strData, "GradeName");
            if (gradeNameJD != null)
            {
                GradeName = new string[gradeNameJD.Count];

                for (int i = 0; i < gradeNameJD.Count; i++)
                    GradeName[i] = (string)gradeNameJD[i];
            }

            JsonData gradeUnlockStageJD = C_Json.GetJsonKeyJsonData(strData, "GradeUnlockStage");
            if (gradeUnlockStageJD != null)
            {
                GradeUnlockStage = new string[gradeUnlockStageJD.Count];

                for (int i = 0; i < gradeUnlockStageJD.Count; i++)
                    GradeUnlockStage[i] = (string)gradeUnlockStageJD[i];
            }
        }
    }

    public static void Save()
    {
        C_Save.SaveString(Name, C_LocalPath.StreamingAssetsConfigPath, JsonMapper.ToJson(new LevelConfig()), "");
    }
}