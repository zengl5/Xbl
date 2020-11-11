using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class SpiritAdData
    {
        public string name { set; get; }

        public int id { set; get; }

        public string uipath { set; get; }

        public string wizardiconpath { set; get; }

        public ActorAnimancerResConfig resConfig;//播放动画的所有数据

        public string rolerespath { set; get; }

        public Vector3 localPosition { set; get; }

        public Vector3 localScale { set; get; }

        public Vector3 localRotation { set; get; }
    }

    public class SpiritAdModel
    {
        public string configPath = "mainscreen/xwk_main_spirit_recommend.json";

        public List<SpiritAdData> spiritAdDatas = new List<SpiritAdData>();
        public int recommendSpiritSum = 0;
        public void Load()
        {
            string strData = C_Save.LoadString(configPath, C_LocalPath.StreamingAssetsConfigPath);
            if (!string.IsNullOrEmpty(strData))
            {
                JsonData configJD = C_Json.GetJsonKeyJsonData(strData, "data");//JsonMapper.ToObject(strData);
                if (configJD != null)
                {
                    int i = 0;
                    while (i <= configJD.Count - 1)
                    {
                        JsonData item = configJD[i];
                        if (item == null)
                            break;

                        SpiritAdData spirititem = new SpiritAdData();

                        spirititem.id = C_Json.GetJsonKeyInt(item, "id");
                        spirititem.uipath = C_Json.GetJsonKeyString(item, "uipath");
                        if (spirititem.id > 0)
                        {
                            recommendSpiritSum++;
                        }
                        spirititem.wizardiconpath = C_Json.GetJsonKeyString(item, "wizardiconpath");
                        spirititem.name = C_Json.GetJsonKeyString(item, "name");
                        spirititem.localPosition = Utility.StringToVector3(C_Json.GetJsonKeyString(item, "localPosition"));
                        spirititem.localScale = Utility.StringToVector3(C_Json.GetJsonKeyString(item, "localScale"));
                        spirititem.localRotation = Utility.StringToVector3(C_Json.GetJsonKeyString(item, "localRotation"));
                        spirititem.rolerespath = C_Json.GetJsonKeyString(item, "rolerespath");
                        JsonData resConfig = C_Json.GetJsonKeyJsonData(item, "resconfig");
                        if (resConfig != null)
                        {
                            ActorAnimancerResConfig actorAnimancerResConfig = new ActorAnimancerResConfig();
                            actorAnimancerResConfig.statetype = C_Json.GetJsonKeyString(resConfig, "statetype");
                            JsonData animitemData = C_Json.GetJsonKeyJsonData(resConfig, "animdata");
                            actorAnimancerResConfig.aimDatas = new List<ActorAnimancerData>();
                            for (int iii = 0; iii < animitemData.Count; iii++)
                            {
                                ActorAnimancerData infodata = new ActorAnimancerData();
                                infodata.audio = C_Json.GetJsonKeyString(animitemData[iii], "audio");
                                infodata.audiotype = C_Json.GetJsonKeyString(animitemData[iii], "audiotype");
                                infodata.anim = C_Json.GetJsonKeyString(animitemData[iii], "anim");
                                infodata.playtype = C_Json.GetJsonKeyString(animitemData[iii], "playtype");
                                actorAnimancerResConfig.aimDatas.Add(infodata);
                            }
                            spirititem.resConfig = actorAnimancerResConfig;
                        }
                        spiritAdDatas.Add(spirititem);

                        i++;
                    }
                }
            }
        }
        public SpiritAdData getSpiritData(string name)
        {
            SpiritAdData data = null;
            for (int i = 0; i < spiritAdDatas.Count; i++)
            {
                if (spiritAdDatas[i].name.Equals(name))
                {
                    data = spiritAdDatas[i];
                    break;
                }
            }
            return data;
        }
        //返回推荐的icon
        public string getUiPath(string name)
        {
            string path = string.Empty;
            SpiritAdData data = getSpiritData(name);
            if (data != null)
            {
                path = data.uipath;
            }
            return path;
        }
        public string getRolePath(string name)
        {
            string path = string.Empty;
            SpiritAdData data = getSpiritData(name);
            if (data != null)
            {
                path = data.rolerespath;
            }
            return path;
        }

        public ActorAnimancerResConfig getActorAnimancerResConfig(string name)
        {
            SpiritAdData data = getSpiritData(name);
            if (data != null)
            {
                return data.resConfig;
            }
            return null;
        }
        public SpiritAdData getSpiritViaId(int id)
        {
            SpiritAdData data = null;
            for (int i = 0; i < spiritAdDatas.Count; i++)
            {
                if (spiritAdDatas[i].id == id)
                {
                    data = spiritAdDatas[i];
                    break;
                }
            }
            return data;
        }
        public string getSpiritName(int id)
        {
            SpiritAdData data = getSpiritViaId(id);
            if (data != null)
            {
                return data.name;
            }
            return string.Empty;
        }

        public int getSpiritCount()
        {
            return recommendSpiritSum;
           // return spiritAdDatas.Count;
        }
        public Vector3 getLocalPosition(int id)
        {
            SpiritAdData data = getSpiritViaId(id);
            if (data != null)
            {
                return data.localPosition;
            }
            return Vector3.zero;
        }
        public Vector3 getLocalScale(int id)
        {
            SpiritAdData data = getSpiritViaId(id);
            if (data != null)
            {
                return data.localScale;
            }
            return Vector3.zero;
        }
        public Vector3 getLocalRotation(int id)
        {
            SpiritAdData data = getSpiritViaId(id);
            if (data != null)
            {
                return data.localRotation;
            }
            return Vector3.zero;
        }
        public string getSpiritRecommendUI(int id)
        {
            SpiritAdData data = getSpiritViaId(id);
            if (data != null)
            {
                return data.uipath;
            }
            return string.Empty;
        }
        public string getWizardIconUiPath(int id)
        {
            SpiritAdData data = getSpiritViaId(id);
            if (data != null)
            {
                return data.wizardiconpath;
            }
            return string.Empty;
        }
        public string getWizardIconUiPath(string name)
        {
            string path = string.Empty;
            SpiritAdData data = getSpiritData(name);
            if (data != null)
            {
                path = data.wizardiconpath;
            }
            return path;
        }
    }
}


