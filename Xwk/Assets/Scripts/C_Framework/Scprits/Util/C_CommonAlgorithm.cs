using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_CommonAlgorithm
    {
        public static int GetAge(string birthday)
        {
            if (string.IsNullOrEmpty(birthday))
                return 0;

            DateTime birthdayDT = DateTime.ParseExact(birthday, "yyyy-MM-dd", null);

            DateTime now = DateTime.Now;
            int age = now.Year - birthdayDT.Year;
            if (now.Month < birthdayDT.Month || (now.Month == birthdayDT.Month && now.Day < birthdayDT.Day))
                age--;

            return age < 0 ? 0 : age;
        }

        public static List<T> Clone<T>(object List)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, List);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as List<T>;
            }
        }
    }
}