using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using FullSerializer;
using System.Text;

namespace Assets.Scripts.C_Framework
{
    public class C_SaveJsonSerializer : C_ISaveSerializer
    {
        public void Serialize<T>(T obj, Stream stream, Encoding encoding)
        {
            try
            {
                StreamWriter writer = new StreamWriter(stream, encoding);
                fsSerializer serializer = new fsSerializer();
                fsData data = new fsData();
                serializer.TrySerialize(obj, out data);
                writer.Write(fsJsonPrinter.CompressedJson(data));
                writer.Flush();
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
                StreamReader reader = new StreamReader(stream, encoding);
                fsSerializer serializer = new fsSerializer();
                fsData data = fsJsonParser.Parse(reader.ReadToEnd());
                serializer.TryDeserialize(data, ref result);
                if (result == null)
                    result = default(T);

                reader.Close();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            return result;
        }
    }
}