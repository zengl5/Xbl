using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class BabyName
{
    private static AudioClip BabyNameClip = null;
    public static AudioClip c_BabyNameAudioClip {
        set {
            BabyNameClip = value;
        }
        get {
            if (BabyNameClip == null)
                BabyNameClip = C_Singleton<GameResMgr>.GetInstance().LoadResource_Audio_XBL("baby_name.mp3");

            return BabyNameClip;
        } }
}
