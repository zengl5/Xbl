using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace Assets.Scripts.C_Framework
{
    public interface C_ISaveSerializer
    {
        void Serialize<T>(T obj, Stream stream, Encoding encoding);

        T Deserialize<T>(Stream stream, Encoding encoding);
    }
}