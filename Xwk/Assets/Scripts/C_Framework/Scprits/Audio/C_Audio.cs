using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.C_Framework
{
    public static class C_Audio
    {
        public static AudioClip FromWavData(byte[] data)
        {
            C_WAV wav = new C_WAV(data);
            AudioClip audioClip = AudioClip.Create("wavclip", wav.SampleCount, 1, wav.Frequency, false);
            audioClip.SetData(wav.LeftChannel, 0);
            return audioClip;
        }
    }
}