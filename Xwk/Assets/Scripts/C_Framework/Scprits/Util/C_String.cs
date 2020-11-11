using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public static class C_String
    {
        public static string FirstLetterToUpper(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public static string DeleteExpandedName(string str)
        {
            int end = str.LastIndexOf('.');

            return str.Substring(0, (end == -1 ? str.Length : end));
        }

        public static string GetSavePath(string filePath)
        {
            int end = filePath.LastIndexOf('/');

            return filePath.Substring(0, (end == -1 ? filePath.Length : end + 1));
        }

        public static string GetFileName(string filePath)
        {
            int end = filePath.LastIndexOf('/');
            if (end == -1)
                return filePath;

            if (end + 1 >= filePath.Length)
                return "";

            return filePath.Substring(end + 1, filePath.Length - end - 1);
        }

        public static bool CheckStringChinese(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            foreach (char t in text)
            {
                if ((int)t <= 127)
                    return false;
            }

            return true;
        }
    }
}