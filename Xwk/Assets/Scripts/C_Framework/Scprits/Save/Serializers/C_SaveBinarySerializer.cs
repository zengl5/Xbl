using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Assets.Scripts.C_Framework
{
    public class C_SaveBinarySerializer : C_ISaveSerializer
    {
        public void Serialize<T>(T obj, Stream stream, Encoding encoding)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public T Deserialize<T>(Stream stream, Encoding encoding)
        {
            T result = default(T);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                result = (T)formatter.Deserialize(stream);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            return result;
        }
    }
}