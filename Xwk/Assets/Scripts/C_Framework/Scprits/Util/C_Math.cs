using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_Math
    {
        public static bool IsEqual_Float(float a, float b)
        {
            return (Mathf.Abs(a - b) < 0.0001);
        }

        // 输入百分比的概率
        public static bool Percent(int percent)
        {
            return Random.Range(0, 100) <= percent ? true : false;
        }
    }
}