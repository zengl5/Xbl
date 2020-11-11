using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_DateTime
    {
        public static DateTime ConvertToDateTime(Int32 d)
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0);
            startTime = startTime.AddSeconds(d).ToLocalTime();

            return startTime;
        }

        public static Int32 ConvertDateTimeToInt32(string dt)
        {
            DateTime dt1 = new DateTime(1970, 1, 1, 8, 0, 0);
            DateTime dt2 = Convert.ToDateTime(dt);

            return Convert.ToInt32((dt2 - dt1).TotalSeconds);
        }

        public static Int32 ConvertDateTimeToInt32(DateTime dt)
        {
            DateTime dt1 = new DateTime(1970, 1, 1, 8, 0, 0);

            return Convert.ToInt32((dt - dt1).TotalSeconds);
        }
    }
}