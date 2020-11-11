using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    [System.Serializable]
    public class C_Point
    {
        public Vector3 position;
        public Vector3 eulerAngles;
        public Quaternion rotation
        {
            get { return Quaternion.Euler(eulerAngles); }
            set { eulerAngles = value.eulerAngles; }
        }

        public C_Point() { }

        public C_Point(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public C_Point(Vector3 position, Vector3 eulerAngles)
        {
            this.position = position;
            this.eulerAngles = eulerAngles;
        }
    }
}