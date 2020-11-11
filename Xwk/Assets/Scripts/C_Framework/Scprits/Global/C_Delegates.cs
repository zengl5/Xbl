using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_VoidDelegate
    {
        public delegate void WithVoid();

        public delegate void WithInt(int param);

        public delegate void WithDouble(double param);

        public delegate void WithFloat(float param);

        public delegate void WithString(string param);

        public delegate void WithBool(bool param);

        public delegate void WithObject(Object param);

        public delegate void WithParamList(params object[] paramList);

        public delegate void WithObjectParamList(Object param1, params object[] paramList);

        public delegate void WithBytesParamList(byte[] param1, params object[] paramList);
    }
}
