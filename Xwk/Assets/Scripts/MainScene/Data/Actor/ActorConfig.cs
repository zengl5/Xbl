using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace YB.XWK.MainScene
{ 
    public class InfoData
    {
        public string audio { set; get; }

        public string audiotype { set; get; }

        public string effect { set; get; }

        public string anim { set; get; }

        public string acname { set; get; }

        //每一个动作的播放类型
        public string statetype { set; get; }

    }
    public class ActorStateInfo
    {
        //用来标记是否需要顺序播放一组配置的动作结束之后，再进行随机--说话的类型
        public string grouptype { set; get; }
        //表示当前的动作表示的角色状态类型
        public string statetype { set; get; }

        public List<InfoData> infodata { get; set; }
    }
    public class ConfigBase {
        protected List<ActorStateInfo> InitData(JsonData stateData)
        {
            if (stateData == null)
            {
                return null;
            }
            List<ActorStateInfo> list = new List<ActorStateInfo>();
            for (int i = 0; i < stateData.Count; i++)
            {
                ActorStateInfo data = new ActorStateInfo();

                data.infodata = new List<InfoData>();
                data.grouptype = C_Json.GetJsonKeyString(stateData[i], "grouptype");
                data.statetype = C_Json.GetJsonKeyString(stateData[i], "statetype");
                JsonData info = C_Json.GetJsonKeyJsonData(stateData[i], "info");
                for (int ii = 0; ii < info.Count; ii++)
                {
                    InfoData infodata = new InfoData();
                    infodata.audio = C_Json.GetJsonKeyString(info[ii], "audio");
                    infodata.audiotype = C_Json.GetJsonKeyString(info[ii], "audiotype");
                    infodata.effect = C_Json.GetJsonKeyString(info[ii], "effect");
                    infodata.anim = C_Json.GetJsonKeyString(info[ii], "anim");
                    infodata.acname = C_Json.GetJsonKeyString(info[ii], "acname");
                    infodata.statetype = C_Json.GetJsonKeyString(info[ii], "statetype");
                    data.infodata.Add(infodata);
                }
                list.Add(data);
            }
            return list;
        }
    }

    public class CloudConfig : ConfigBase
    {
        private const string IdleState = "idlestate";
        private const string WalkAroundState = "walkaroundstate";
        private const string TouchState = "touchstate";
        private const string RainState = "rainstate";
        private const string HelloState = "hellostate";
        public string _ConfigPath = "mainscreen/cloudstate_config.json";
        private List<ActorStateInfo> IdleStateList;
        private List<ActorStateInfo> WalkAroundList;
        private List<ActorStateInfo> TouchStateList;
        private List<ActorStateInfo> RainStateList;
        private List<ActorStateInfo> HelloStateList;

        public void Load(string path= "mainscreen/cloudstate_config.json")
        {
            try
            {
                _ConfigPath = path;
                string strData = C_Save.LoadString(_ConfigPath, C_LocalPath.StreamingAssetsConfigPath);
                if (!string.IsNullOrEmpty(strData))
                {
                    IdleStateList = InitData(C_Json.GetJsonKeyJsonData(strData, IdleState));
                    WalkAroundList = InitData(C_Json.GetJsonKeyJsonData(strData, WalkAroundState));
                    TouchStateList = InitData(C_Json.GetJsonKeyJsonData(strData, TouchState));
                    RainStateList = InitData(C_Json.GetJsonKeyJsonData(strData, RainState));
                    HelloStateList = InitData(C_Json.GetJsonKeyJsonData(strData, HelloState));
                }
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError(" mainscreen/cloudstate_config.json load fail :" + e);
            }
        }
        public ActorStateInfo FecthData(string stateName)
        {
            ActorStateInfo info = null;
            switch (stateName)
            {
                case IdleState:
                    {
                        info = IdleStateList[Random.Range(0, IdleStateList.Count)];
                    }
                    break;
                case WalkAroundState:
                    {
                        info = WalkAroundList[Random.Range(0, WalkAroundList.Count)];
                    }
                    break;
                case TouchState:
                    {
                        info = TouchStateList[Random.Range(0, TouchStateList.Count)];
                    }
                    break;
                case RainState:
                    {
                        info = RainStateList[Random.Range(0, RainStateList.Count)];
                    }
                    break;
                case HelloState:
                    {
                        info = HelloStateList[Random.Range(0, HelloStateList.Count)];
                    }
                    break;
                default:break;
            }
            return info;
        }
    }
    public class ActorConfig: ConfigBase
    {
        private const string NightFallhello = "nightfallhello";

        private const string DeepNightHello = "deepnighthello";

        private const string Noonhello = "noonhello";

        private const string IdleState = "idlestate";

        private const string TouchBodyState = "touchbodystate";

        private const string TouchHeadState = "touchheadstate";

        private const string TouchTailState = "touchtailstate";

        private const string AskState = "askstate";

        private const string NormalhelloState = "normalhello";

        private const string MoringhelloState = "morninghello";

        private const string FlashState = "flashstate";

        private const string ShowState = "showstate";

        private const string FreezeState = "freezestate";

        public string _ConfigPath = "mainscreen/xwkstate_config.json";

        private List<ActorStateInfo>  NormalHello ;

        private List<ActorStateInfo>  Moringhello;

        private List<ActorStateInfo> Nightfallhello;

        private List<ActorStateInfo> DeepNighthello;

        private List<ActorStateInfo> Noonfallhello;

        private List<ActorStateInfo> IdleStateList;

        private List<ActorStateInfo> TouchBodyStateList;

        private List<ActorStateInfo> TouchHeadStateList;

        private List<ActorStateInfo> TouchTailStateList;

        private List<ActorStateInfo> AskStateList;

        private List<ActorStateInfo> FlashStateList;

        private List<ActorStateInfo> ShowStateList;

        private List<ActorStateInfo> FreezeStateList;


        public void Load()
        {
            try
            {
                string strData =  C_Save.LoadString(_ConfigPath, C_LocalPath.StreamingAssetsConfigPath);
                if (!string.IsNullOrEmpty(strData))
                {
                    NormalHello = InitData(C_Json.GetJsonKeyJsonData(strData, NormalhelloState));
                    Moringhello = InitData(C_Json.GetJsonKeyJsonData(strData, MoringhelloState));
                    Nightfallhello = InitData(C_Json.GetJsonKeyJsonData(strData, NightFallhello));
                    DeepNighthello = InitData(C_Json.GetJsonKeyJsonData(strData, DeepNightHello));
                    Noonfallhello = InitData(C_Json.GetJsonKeyJsonData(strData, Noonhello));
                    IdleStateList = InitData(C_Json.GetJsonKeyJsonData(strData, IdleState));
                    TouchBodyStateList = InitData(C_Json.GetJsonKeyJsonData(strData, TouchBodyState));
                    TouchHeadStateList = InitData(C_Json.GetJsonKeyJsonData(strData, TouchHeadState));
                    TouchTailStateList = InitData(C_Json.GetJsonKeyJsonData(strData, TouchTailState));
                    AskStateList = InitData(C_Json.GetJsonKeyJsonData(strData, AskState));
                    FlashStateList = InitData(C_Json.GetJsonKeyJsonData(strData, FlashState));
                    ShowStateList = InitData(C_Json.GetJsonKeyJsonData(strData, ShowState));
                    FreezeStateList = InitData(C_Json.GetJsonKeyJsonData(strData, FreezeState));
                }
                
            }
            catch(Exception e)
            {
                C_DebugHelper.LogError(" mainscreen/xwkstate_config.json load fail :"+e);
            }
            
        }
       
        public ActorStateInfo FecthData(string stateName)
        {
            ActorStateInfo info = null;
            switch (stateName)
            {
                case NormalhelloState:
                    {
                        info =  NormalHello[Random.Range(0, NormalHello.Count)];
                        
                    }
                    break;
                case MoringhelloState:
                    {
                        info = Moringhello[Random.Range(0, Moringhello.Count)];

                    }
                    break;
               
                case Noonhello:
                    {
                        info = Noonfallhello[Random.Range(0, Noonfallhello.Count)];

                    }
                    break;
                case NightFallhello:
                    {
                        info = Nightfallhello[Random.Range(0, Nightfallhello.Count)];

                    }
                    break;
                case DeepNightHello:
                    {
                        info = DeepNighthello[Random.Range(0, DeepNighthello.Count)];

                    }
                    break;
                case IdleState:
                    {
                        string group = string.Empty;
                        int randomEnemyId = Random.Range(1, 100);
                        if (randomEnemyId <= 25)//说话
                        {
                            group = "talk";
                        }
                        else if (randomEnemyId > 25 && randomEnemyId <= 60)//小动作
                        {
                            group = "pettyaction";
                        }
                        else
                        {
                            group = "breathe";
                        }

                        List<int> reslut = new List<int>();
                        for (int i = 0; i < IdleStateList.Count; i++)
                        {
                            if (IdleStateList[i].grouptype.Equals(group))
                            {
                                reslut.Add(i);
                            }
                        }
                        if (reslut.Count <= 0)
                        {
                            int id = Random.Range(0, IdleStateList.Count);
                            info = IdleStateList[id];
                        }
                        else
                        {
                            int id = Random.Range(0, reslut.Count);
                            info = IdleStateList[reslut[id]];
                            reslut.Clear();
                        }

                      //  info =  IdleStateList[3];
                    }
                    break;
                case TouchHeadState:
                    {
                        info = TouchHeadStateList[Random.Range(0, TouchHeadStateList.Count)];

                    }
                    break;
                case TouchTailState:
                    {
                        info = TouchTailStateList[Random.Range(0, TouchTailStateList.Count)];
                    }
                    break;
                case TouchBodyState:
                    {
                        info = TouchBodyStateList[Random.Range(0, TouchBodyStateList.Count)];
                    }
                    break;
                case AskState:
                    {
                        info = AskStateList[Random.Range(0, AskStateList.Count)];

                    }
                    break;
                case FlashState:
                    {
                        info = FlashStateList[Random.Range(0, FlashStateList.Count)];
                    }
                    break;
                case ShowState:
                    {
                        info = ShowStateList[Random.Range(0, ShowStateList.Count)];
                    }
                    break;
                case FreezeState:
                    {
                        info = FreezeStateList[Random.Range(0, FreezeStateList.Count)];
                    }
                    break;
                default:break;

            }
            return info;
        }

         
    }
}

