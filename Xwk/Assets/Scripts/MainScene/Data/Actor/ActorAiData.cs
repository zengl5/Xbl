using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace YB.XWK.MainScene
{
    
    //每一个问题和答案的动画，语音，ui资源配置
    public class ResConfigData
    {
        public string id { set; get; }

        public ActorAnimancerResConfig actorAnimancerResConfig;

        public string uipath { set; get; }
    }
    //每一个问题的选项编号配置属性
    public class OptionData
    {
        public string id { set; get; }

        public string nextid { set; get; }//所有下一道题,隔开每一道题
    }
    //每一道题的编号配置：问题，选项 
    public class QuestionConfigData
    {
        public string questionid { set; get; }//出题的编号

        public string contentid { set; get; }//题目的资源配置编号

        public string actorwaitid { set; get; }//角色等待的资源编号

        public List<OptionData> optiondata { set; get; }//选项的编号

        public ResConfigData questionData { get; set; }//题目的资源

        public List<ResConfigData> optionlist { get; set; }//选项的资源

        public ResConfigData actorwaitdata { get; set; }//角色等待的资源

    }

    public class ActorAiData
    {
        public string _ConfigPath = "mainscreen/question_config.json";
        public string _AiResConfigPath = "mainscreen/ai_res_config.json";
        //
        private List<QuestionConfigData> QuestionConfigDataList = new List<QuestionConfigData>();
        //每一道题的问题，选择题的资源配置
        private List<ResConfigData> ResConfigDataList = new List<ResConfigData>();
        public void Load()
        {
            ResConfigDataList.Clear();
            QuestionConfigDataList.Clear();
            try
            {
                string strData = C_Save.LoadString(_ConfigPath, C_LocalPath.StreamingAssetsConfigPath);
                if (!string.IsNullOrEmpty(strData))
                {
                    JsonData stateData = C_Json.GetJsonKeyJsonData(strData, "question");
                    int length = stateData.Count;
                    for (int i = 0; i < length; i++)
                    {
                        QuestionConfigData data = new QuestionConfigData();
                        data.questionid = C_Json.GetJsonKeyString(stateData[i], "id");
                        data.contentid = C_Json.GetJsonKeyString(stateData[i], "qid");
                        data.actorwaitid = C_Json.GetJsonKeyString(stateData[i], "waitid");
                        data.optiondata = new List<OptionData>();
                        JsonData info = C_Json.GetJsonKeyJsonData(stateData[i], "options");
                        for (int ii = 0; ii < info.Count; ii++)
                        {
                            OptionData optiondata = new OptionData();
                            optiondata.id = C_Json.GetJsonKeyString(info[ii], "id");
                            optiondata.nextid = C_Json.GetJsonKeyString(info[ii], "nextid");
                            data.optiondata.Add(optiondata);
                        }
                        QuestionConfigDataList.Add(data);
                    }
                }
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError(" mainscreen/question_config.json load fail :" + e);
            }
            LoadResConfig();
            //资源和配置资源id对应
            int cout = QuestionConfigDataList.Count;
            int resCount = ResConfigDataList.Count;
            for (int i = 0; i < cout; i++)
            {
                for (int ii = 0; ii < resCount; ii++)
                {
                    if (QuestionConfigDataList[i].contentid.Equals(ResConfigDataList[ii].id))
                    {
                        QuestionConfigDataList[i].questionData = ResConfigDataList[ii];
                    }
                    if (QuestionConfigDataList[i].actorwaitid.Equals(ResConfigDataList[ii].id))
                    {
                        QuestionConfigDataList[i].actorwaitdata = ResConfigDataList[ii];
                    }
                }
            }
            for (int i = 0; i < cout; i++)
            {
                for (int ii = 0; ii < resCount; ii++)
                {
                    int length = QuestionConfigDataList[i].optiondata.Count;
                    for (int iii = 0; iii < length; iii++)
                    {
                        if (QuestionConfigDataList[i].optiondata[iii].id.Equals(ResConfigDataList[ii].id))
                        {
                            if (QuestionConfigDataList[i].optionlist == null)
                            {
                                QuestionConfigDataList[i].optionlist = new List<ResConfigData>();
                            }
                            QuestionConfigDataList[i].optionlist.Add(ResConfigDataList[ii]);
                        }
                    }
                }
            }

            // ResConfigDataList.Clear();
        }
        public void LoadResConfig()
        {
            try
            {
                string strData = C_Save.LoadString(_AiResConfigPath, C_LocalPath.StreamingAssetsConfigPath);
                if (!string.IsNullOrEmpty(strData))
                {
                    JsonData stateData = C_Json.GetJsonKeyJsonData(strData, "res");
                    int length = stateData.Count;
                    for (int i = 0; i < length; i++)
                    {
                        ResConfigData data = new ResConfigData();
                        data.id = C_Json.GetJsonKeyString(stateData[i], "id");
                        data.uipath = C_Json.GetJsonKeyString(stateData[i], "ui");
                        data.actorAnimancerResConfig = new ActorAnimancerResConfig();
                        data.actorAnimancerResConfig.statetype = C_Json.GetJsonKeyString(stateData[i], "statetype");
                        data.actorAnimancerResConfig.aimDatas = new List<ActorAnimancerData>();
                        JsonData animdata = C_Json.GetJsonKeyJsonData(stateData[i], "anim");
                        int animdataCount = animdata.Count;
                        for (int ii = 0; ii < animdataCount; ii++)
                        {
                            ActorAnimancerData actorAnimancerData = new ActorAnimancerData();
                            actorAnimancerData.anim = C_Json.GetJsonKeyString(animdata[ii], "anim");
                            actorAnimancerData.audio = C_Json.GetJsonKeyString(animdata[ii], "audio");
                            actorAnimancerData.playtype = C_Json.GetJsonKeyString(animdata[ii], "playtype");
                            data.actorAnimancerResConfig.aimDatas.Add(actorAnimancerData);
                        }
                        ResConfigDataList.Add(data);
                    }
                }
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError(" mainscreen/question_config.json load fail :" + e);
            }
        }
        public List<QuestionConfigData> FetchQuestionData()
        {
            // int length = QuestionConfigDataList.Count;
            // QuestionConfigData data = QuestionConfigDataList[Random.Range(0,length)];
            return QuestionConfigDataList;
        }
    }

}