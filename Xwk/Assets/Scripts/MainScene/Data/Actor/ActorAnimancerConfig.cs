using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace YB.XWK.MainScene {

    //动画语音资源
    public class ActorAnimancerData
    {
        public string audio { set; get; }

        public string audiotype { set; get; }

        public string anim { set; get; }

        public string playtype { set; get; }//动画播放的类型

    }
    public class ActorAnimancerResConfig
    {
        public string grouptype { set; get; }

        public List<ActorAnimancerData> aimDatas;//播放动画的所有数据

        public string statetype { set; get; }//动画播放的类型
    }

    public class ActorAnimancerConfig
    {
        private const string IdleState = "idlestate";

        private const string TouchBodyState = "touchbodystate";

        private const string Goldencudgestate = "goldencudgestate";

        private const string TouchGoldencudgestate = "touch_goldencudgestate";

        private const string Separationstate = "separationstate";

        private const string TouchSeparationstate = "touch_separationstate";

        private const string TouchTailState = "touchtailstate";

        private const string FlashState = "flashstate";

        private const string ShowState = "showstate";

        private const string NormalHelloState = "normalhello";

        private const string FreezeState = "freezestate";


        public string _ConfigPath = "mainscreen/main_xwkstate_config.json";

        private List<ActorAnimancerResConfig> IdleStateList;
        private List<ActorAnimancerResConfig> IdleStateTalkList;
        private List<ActorAnimancerResConfig> IdleStateList124;
        private List<ActorAnimancerResConfig> IdleStateList125;
        private List<ActorAnimancerResConfig> IdleStateList126;
        private List<ActorAnimancerResConfig> IdleStateList0207;
        private List<ActorAnimancerResConfig> IdleStateList0208;

        private List<ActorAnimancerResConfig> IdleStatePettyactionList;
        private List<ActorAnimancerResConfig> IdleStateBreatheList;

        private List<ActorAnimancerResConfig> TouchBodyStateList;
        private List<ActorAnimancerResConfig> TouchBodyStateTalkList;
        private List<ActorAnimancerResConfig> TouchBodyStatePettyactionList;

        private List<ActorAnimancerResConfig> TouchTailStateList;

        private List<ActorAnimancerResConfig> FlashStateList;

        private List<ActorAnimancerResConfig> ShowStateList;

        private List<ActorAnimancerResConfig> GoldencudgestateList;

        private List<ActorAnimancerResConfig> TouchGoldencudgestateList;

        private List<ActorAnimancerResConfig> SeparationstateList;

        private List<ActorAnimancerResConfig> TouchSeparationstateList;

        private List<ActorAnimancerResConfig> NormalHelloStateList;

        private List<ActorAnimancerResConfig> FreezeStateList;

        private static int IdleStateTalkid = 0;
        private static int IdleStateTalkid124= 0;
        private static int IdleStateTalkid125 = 0;
        private static int IdleStateTalkid126 = 0;
        private static int IdleStateTalkid0207 = 0;
        private static int IdleStateTalkid0208 = 0;

        private static int IdleStatePettyactionid = 0;
        private static int IdleStateBreatheid = 0;
        private static int TouchBodyStateTalkid = 0;
        private static int TouchBodyStatePettyactionid = 0;

        private static int TouchTailStateid = 0;
        private static int FlashStateid = 0;
        private static int ShowStateid = 0;
        private static int Goldencudgestateid = 0;
        private static int TouchGoldencudgestateid = 0;
        private static int Separationstateid = 0;
        private static int TouchSeparationstateid = 0;
        private static int NormalHelloStateid = 0;
        public void Load()
        {
            try
            {
                string strData = C_Save.LoadString(_ConfigPath, C_LocalPath.StreamingAssetsConfigPath);
                if (!string.IsNullOrEmpty(strData))
                {
                    IdleStateList = InitData(C_Json.GetJsonKeyJsonData(strData, IdleState));
                    IdleStateList124 = InitData(C_Json.GetJsonKeyJsonData(strData, "20200124"));
                    IdleStateList125 = InitData(C_Json.GetJsonKeyJsonData(strData, "20200125"));
                    IdleStateList126 = InitData(C_Json.GetJsonKeyJsonData(strData, "20200126"));
                    IdleStateList0207 = InitData(C_Json.GetJsonKeyJsonData(strData, "20200207"));
                    IdleStateList0208 = InitData(C_Json.GetJsonKeyJsonData(strData, "20200208"));
                    TouchBodyStateList = InitData(C_Json.GetJsonKeyJsonData(strData, TouchBodyState));
                    TouchTailStateList = InitData(C_Json.GetJsonKeyJsonData(strData, TouchTailState));
                    FlashStateList = InitData(C_Json.GetJsonKeyJsonData(strData, FlashState));
                    ShowStateList = InitData(C_Json.GetJsonKeyJsonData(strData, ShowState));

                    GoldencudgestateList = InitData(C_Json.GetJsonKeyJsonData(strData, Goldencudgestate));
                    TouchGoldencudgestateList = InitData(C_Json.GetJsonKeyJsonData(strData, TouchGoldencudgestate));
                    SeparationstateList = InitData(C_Json.GetJsonKeyJsonData(strData, Separationstate));
                    TouchSeparationstateList = InitData(C_Json.GetJsonKeyJsonData(strData, TouchSeparationstate));
                    NormalHelloStateList = InitData(C_Json.GetJsonKeyJsonData(strData, NormalHelloState));
                    FreezeStateList = InitData(C_Json.GetJsonKeyJsonData(strData, FreezeState));

                    Utility.RandListData<ActorAnimancerResConfig>(IdleStateList);
                    Utility.RandListData<ActorAnimancerResConfig>(TouchBodyStateList);

                    IdleStateTalkList = getGroupList(IdleStateList,"talk");
                    IdleStatePettyactionList = getGroupList(IdleStateList, "pettyaction");
                    IdleStateBreatheList = getGroupList(IdleStateList, "breathe");
                    Utility.RandListData<ActorAnimancerResConfig>(IdleStateTalkList);

                    TouchBodyStateTalkList = getGroupList(TouchBodyStateList, "talk");
                    TouchBodyStatePettyactionList = getGroupList(TouchBodyStateList, "pettyaction");

                }
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError(" mainscreen/main_xwkstate_config.json load fail :" + e);
            }

        }
        protected List<ActorAnimancerResConfig> getGroupList(List<ActorAnimancerResConfig> List, string groupName)
        {
            List<ActorAnimancerResConfig> newList=new List<ActorAnimancerResConfig>();
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].grouptype.Equals(groupName))
                {
                    newList.Add(List[i]);
                }
            }
            return newList;
        }
        protected List<ActorAnimancerResConfig> InitData(JsonData stateData)
        {
            if (stateData == null)
            {
                return null;
            }
            List<ActorAnimancerResConfig> list = new List<ActorAnimancerResConfig>();
            for (int i = 0; i < stateData.Count; i++)
            {
                ActorAnimancerResConfig data = new ActorAnimancerResConfig();

                data.aimDatas = new List<ActorAnimancerData>();
                data.grouptype = C_Json.GetJsonKeyString(stateData[i], "grouptype");
                data.statetype = C_Json.GetJsonKeyString(stateData[i], "statetype");
                JsonData info = C_Json.GetJsonKeyJsonData(stateData[i], "info");
                for (int ii = 0; ii < info.Count; ii++)
                {
                    ActorAnimancerData infodata = new ActorAnimancerData();
                    infodata.audio = C_Json.GetJsonKeyString(info[ii], "audio");
                    infodata.audiotype = C_Json.GetJsonKeyString(info[ii], "audiotype");
                    infodata.anim = C_Json.GetJsonKeyString(info[ii], "anim");
                    infodata.playtype = C_Json.GetJsonKeyString(info[ii], "playtype");
                    data.aimDatas.Add(infodata);
                }
                list.Add(data);
            }
            return list;
        }
      

        public ActorAnimancerResConfig FecthData(string stateName)
        {
            ActorAnimancerResConfig info = null;
            switch (stateName)
            {
                case IdleState:
                    {
                        string group = string.Empty;
                        int randomEnemyId = Random.Range(1, 100);
#if UNITY_EDITOR
                        if (true)//说话
#else
                        if (randomEnemyId <= 25)//说话
#endif
                        {
                            DateTime time = System.DateTime.Now;
                            string data = String.Format("{0:D4}{1:D2}{2:D2}", time.Year, time.Month, time.Day);
                            int percent = Random.Range(0,5);
                            if (percent <= 3)
                            {
                                if (data.Equals("20200124"))
                                {
                                    info = getInfo(IdleStateList124, ref IdleStateTalkid124);
                                }
                                else if (data.Equals("20200125"))
                                {
                                    info = getInfo(IdleStateList125, ref IdleStateTalkid125);
                                }
                                else if (data.Equals("20200207"))
                                {
                                    info = getInfo(IdleStateList0207, ref IdleStateTalkid0207);
                                }
                                else if (data.Equals("20200208"))
                                {
                                    info = getInfo(IdleStateList0208, ref IdleStateTalkid0208);
                                }
                                else if (int.Parse(data) >= 20200126 && int.Parse(data) <= 20200130)
                                {
                                    info = getInfo(IdleStateList126, ref IdleStateTalkid126);
                                }
                                else
                                {
                                    info = getInfo(IdleStateTalkList, ref IdleStateTalkid);
                                }
                            }
                            else
                            {
                                info = getInfo(IdleStateTalkList, ref IdleStateTalkid);
                            }
                        }
                        else if (randomEnemyId > 25 && randomEnemyId <= 50)//小动作
                        {
                          
                            info = getInfo(IdleStatePettyactionList, ref IdleStatePettyactionid);
                        }
                        else//呼吸
                        {
                           
                            info = getInfo(IdleStateBreatheList, ref IdleStateBreatheid);

                        }
                    }
                    break;
                case TouchTailState:
                    {
                        info = getInfo(TouchTailStateList, ref TouchTailStateid);

                    }
                    break;
                case TouchBodyState:
                    {
                        string group = string.Empty;
                        int randomEnemyId = Random.Range(1, 100);
                        if (randomEnemyId <= 50)//说话
                        {
                            DateTime time = System.DateTime.Now;
                            string data = String.Format("{0:D4}{1:D2}{2:D2}", time.Year, time.Month, time.Day);
                            if (Random.Range(0, 2) == 1)
                            {
                                if (data.Equals("20200124"))
                                {
                                    info = getInfo(IdleStateList124, ref IdleStateTalkid124);
                                }
                                else if (data.Equals("20200125"))
                                {
                                    info = getInfo(IdleStateList125, ref IdleStateTalkid125);
                                }
                                else if (data.Equals("20200207"))
                                {
                                    info = getInfo(IdleStateList0207, ref IdleStateTalkid0207);
                                }
                                else if (data.Equals("20200208"))
                                {
                                    info = getInfo(IdleStateList0208, ref IdleStateTalkid0208);
                                }
                                else if (int.Parse(data) >= 20200126 && int.Parse(data) <= 20200130)
                                {
                                    info = getInfo(IdleStateList126, ref IdleStateTalkid126);
                                }
                                else
                                {
                                    info = getInfo(TouchBodyStateTalkList, ref TouchBodyStateTalkid);
                                }
                            }
                            else
                            {
                                info = getInfo(TouchBodyStateTalkList, ref TouchBodyStateTalkid);
                            }
                        }
                        else //小动作
                        {
                             
                            info = getInfo(TouchBodyStatePettyactionList, ref TouchBodyStatePettyactionid);

                        }

                    }
                    break;
                case FlashState:
                    {
                        info = getInfo(FlashStateList, ref FlashStateid);

                    }
                    break;
                case ShowState:
                    {
                        info = getInfo(ShowStateList, ref ShowStateid);

                    }
                    break;
                case Goldencudgestate:
                    {
                        info = getInfo(GoldencudgestateList, ref Goldencudgestateid);
                    }
                    break;
                case TouchGoldencudgestate:
                    {
                        info = getInfo(TouchGoldencudgestateList, ref TouchGoldencudgestateid);
                    }
                    break;
                case Separationstate:
                    {
                        info = getInfo(SeparationstateList, ref Separationstateid);
                    }
                    break;
                case TouchSeparationstate:
                    {
                        info = getInfo(TouchSeparationstateList, ref TouchSeparationstateid);
                    }
                    break;
                case NormalHelloState:
                    {
                        info = getInfo(NormalHelloStateList,ref NormalHelloStateid);
                    }
                    break;
                case FreezeState:
                    {
                        info = FreezeStateList[Random.Range(0, FreezeStateList.Count)];
                    }
                    break;
                default: break;
            }
            return info;
        } 
        protected ActorAnimancerResConfig getInfo(List<ActorAnimancerResConfig> list,ref int id)
        {
            if (list==null)
            {
                C_DebugHelper.Log("getInfo //list is null...");
                return null;
            }
            id++;
            if (id > list.Count - 1)
            {
                id = 0;
                Utility.RandListData<ActorAnimancerResConfig>(list);
            }
            return list[id];
        }
    } 
    
}
 


