using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public interface C_ISaveEncoder
    {
        byte[] Encode(byte[] input, string password);
        byte[] Decode(byte[] input, string password);
    }
}