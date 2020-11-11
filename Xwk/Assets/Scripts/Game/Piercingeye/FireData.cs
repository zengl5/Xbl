using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using YB.XWK.MainScene;

public class FireData{

    SpritConfig spConfig;

    DialogInfo dialogInfo;
    public void ReadJsonData(LitJson.JsonData data, Action<SpritConfig, DialogInfo> ac)
    {
        spConfig = new SpritConfig();
        dialogInfo = new DialogInfo();
        foreach (LitJson.JsonData temp in data["fireSprite"])
        {
            //假体
            string meshjtPath = temp["jiatiMesh"]["meshPath"].ToString();
            string rtmcjtPath = temp["jiatiMesh"]["rtmcPath"].ToString();          
            //真身
            string meshrealPath = temp["realMesh"]["meshPath"].ToString();
            string rtmcrealPath = temp["realMesh"]["rtmcPath"].ToString();

            string normalTex = temp["realMesh"]["defaultTex"].ToString();

            string meshTitle = temp["iconName"]["titleNamePath"].ToString();

            string rolelockingMat = temp["roleInfo"]["grayMat"].ToString();
            string roleNormalMat = temp["roleInfo"]["normalMat"].ToString();
            string movePostion = temp["realMesh"]["movePosition"].ToString();
            string moveEulerAngles = temp["realMesh"]["moveEulerAngles"].ToString();

            string spNameSound = temp["roleInfo"]["spNameSound"].ToString();

            string spInfoSound = temp["roleInfo"]["spInfoSound"].ToString();

            ///////////////////////////////
            string titleTex = temp["iconName"]["titleNamePath"].ToString();

            string Name = temp["name"].ToString();
            if (temp["name"].Equals(LocalData.m_SpiritType))
            {
                //假体
                spConfig.normalTex = normalTex;
                spConfig.spritePath_Jiati = meshjtPath;
                spConfig.AnimCtrlPath_Jiati = rtmcjtPath;
                SpriteTransform spJiati = new SpriteTransform();
                spJiati.InitSpTransform(getVector(temp["jiatiMesh"]["position"].ToString()), getVector(temp["jiatiMesh"]["eulerAngles"].ToString()), getVector(temp["jiatiMesh"]["localScale"].ToString()));
                spConfig.SpTransfrom_Jiati = spJiati;

                //真身
                spConfig.Real_spritePath = meshrealPath;
                spConfig.Real_AnimCtrlPath = rtmcrealPath;
                SpriteTransform spReal = new SpriteTransform();
                spReal.InitSpTransform(getVector(temp["realMesh"]["position"].ToString()), getVector(temp["realMesh"]["eulerAngles"].ToString()), getVector(temp["realMesh"]["localScale"].ToString()));
                spConfig.Real_SpTransfrom = spReal;

                spConfig.SpNameSound = spNameSound;
                spConfig.RolelockingMat = rolelockingMat;
                spConfig.RoleNormalMat = roleNormalMat;

                spConfig.MovePostion = getVector(movePostion);
                spConfig.MoveEulerAngles = getVector(moveEulerAngles);

                dialogInfo.TitleTex = titleTex;//标题路径
                dialogInfo.SpInfoSound = spInfoSound;
                ac(spConfig, dialogInfo);
            }

        }
    }

        Vector3 getVector(string str)
        {
            string[] sArray = Regex.Split(str, ",", RegexOptions.IgnoreCase);
            return new Vector3(Convert.ToSingle(sArray[0]), Convert.ToSingle(sArray[1]), Convert.ToSingle(sArray[2]));
        }
    }
