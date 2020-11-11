using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningConfig
{
    public static string Name = "learning_config";

    public static string[] LearningRhythmName;
    public static int[] LearningRhythmCourse;

    public static string[] LearningTimeName;
    public static int[] LearningTime;

    public static string[] RestTimeName;
    public static int[] RestTime;

    public LearningConfig()
    {
        LearningRhythmName = new string[3];
        LearningRhythmName[0] = "LOACAL_LEARN_CONFIG_LEARNING_RHYTHM_NAME_0";
        LearningRhythmName[1] = "LOACAL_LEARN_CONFIG_LEARNING_RHYTHM_NAME_1";
        LearningRhythmName[2] = "LOACAL_LEARN_CONFIG_LEARNING_RHYTHM_NAME_2";

        LearningRhythmCourse = new int[3];
        LearningRhythmCourse[0] = 1;
        LearningRhythmCourse[1] = 3;
        LearningRhythmCourse[2] = -1;

        LearningTimeName = new string[4];
        LearningTimeName[0] = "LOACAL_LEARN_CONFIG_LEARNING_TIME_NAME_0";
        LearningTimeName[1] = "LOACAL_LEARN_CONFIG_LEARNING_TIME_NAME_1";
        LearningTimeName[2] = "LOACAL_LEARN_CONFIG_LEARNING_TIME_NAME_2";
        LearningTimeName[3] = "LOACAL_LEARN_CONFIG_LEARNING_TIME_NAME_3";

        LearningTime = new int[4];
        LearningTime[0] = -1;
        LearningTime[1] = 900;
        LearningTime[2] = 1800;
        LearningTime[3] = 3600;

        RestTimeName = new string[3];
        RestTimeName[0] = "LOACAL_LEARN_CONFIG_LEARNING_REST_TIME_NAME_0";
        RestTimeName[1] = "LOACAL_LEARN_CONFIG_LEARNING_REST_TIME_NAME_1";
        RestTimeName[2] = "LOACAL_LEARN_CONFIG_LEARNING_REST_TIME_NAME_2";

        RestTime = new int[3];
        RestTime[0] = 180;
        RestTime[1] = 600;
        RestTime[2] = 900;
    }

    public static void Load()
    {
        string strData = C_Save.LoadString(Name, C_LocalPath.StreamingAssetsConfigPath);
        if (!string.IsNullOrEmpty(strData))
        {
            JsonData learningRhythmNameJD = C_Json.GetJsonKeyJsonData(strData, "LearningRhythmName");
            if (learningRhythmNameJD != null)
            {
                LearningRhythmName = new string[learningRhythmNameJD.Count];

                for (int i = 0; i < learningRhythmNameJD.Count; i++)
                    LearningRhythmName[i] = (string)learningRhythmNameJD[i];
            }

            JsonData learningRhythmCourseJD = C_Json.GetJsonKeyJsonData(strData, "LearningRhythmCourse");
            if (learningRhythmCourseJD != null)
            {
                LearningRhythmCourse = new int[learningRhythmCourseJD.Count];

                for (int i = 0; i < learningRhythmCourseJD.Count; i++)
                    LearningRhythmCourse[i] = (int)learningRhythmCourseJD[i];
            }

            JsonData learningTimeNameJD = C_Json.GetJsonKeyJsonData(strData, "LearningTimeName");
            if (learningTimeNameJD != null)
            {
                LearningTimeName = new string[learningTimeNameJD.Count];

                for (int i = 0; i < learningTimeNameJD.Count; i++)
                    LearningTimeName[i] = (string)learningTimeNameJD[i];
            }

            JsonData learningTimeJD = C_Json.GetJsonKeyJsonData(strData, "LearningTime");
            if (learningTimeJD != null)
            {
                LearningTime = new int[learningTimeJD.Count];

                for (int i = 0; i < learningTimeJD.Count; i++)
                    LearningTime[i] = (int)learningTimeJD[i];
            }

            JsonData restTimeNameJD = C_Json.GetJsonKeyJsonData(strData, "RestTimeName");
            if (restTimeNameJD != null)
            {
                RestTimeName = new string[restTimeNameJD.Count];

                for (int i = 0; i < restTimeNameJD.Count; i++)
                    RestTimeName[i] = (string)restTimeNameJD[i];
            }

            JsonData restTimeJD = C_Json.GetJsonKeyJsonData(strData, "RestTime");
            if (restTimeJD != null)
            {
                RestTime = new int[restTimeJD.Count];

                for (int i = 0; i < restTimeJD.Count; i++)
                    RestTime[i] = (int)restTimeJD[i];
            }
        }
    }

    public static void Save()
    {
        C_Save.SaveString(Name, C_LocalPath.StreamingAssetsConfigPath, JsonMapper.ToJson(new LearningConfig()), "");
    }
}