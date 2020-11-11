using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Assets.Scripts.C_Framework
{
    public class C_AESEncryption
    {
        private static string m_strKey = "b5feac279c843edbe6d7c4d153787d79";
        private static string m_strIV = "e4717c6b1671411c9d2784729ecc3ca3";


        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="inputStr">明文字符串</param>
        /// <returns>密文</returns>
        public static string AESEncryptString(int input)
        {
            return AESEncryptString(input.ToString());
        }

        public static string AESEncryptString(float input)
        {
            return AESEncryptString(input.ToString());
        }

        public static string AESEncryptString(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
                return "";

            StringBuilder strEncryptPId = new StringBuilder();

            byte[] bytePId = AESEncryptByte(inputStr);
            for (int i = 0; i < bytePId.Length; i++)
                strEncryptPId.Append(bytePId[i].ToString("X2"));

            return strEncryptPId.ToString();
        }

        public static byte[] AESEncryptByte(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
                return null;

            byte[] byteArray = Encoding.UTF8.GetBytes(inputStr);

            //分组加密算法
            SymmetricAlgorithm des = Rijndael.Create();
            //设置密钥及密钥向量
            des.Key = StringToHexByte(m_strKey);
            des.IV = StringToHexByte(m_strIV);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(byteArray, 0, byteArray.Length);
                    cs.FlushFinalBlock();
                    byte[] cipherBytes = ms.ToArray();  //得到加密后的字节数组
                    cs.Close();
                    ms.Close();
                    return cipherBytes;
                }
            }
        }


        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptStr">密文</param>
        /// <returns>明文字符串</returns>

        public static string AESDecryptString(string encryptStr)
        {
            if (string.IsNullOrEmpty(encryptStr))
                return "";

            string strDate = Encoding.ASCII.GetString(AESDecryptByte(encryptStr));
            strDate = strDate.Replace("\0", "");

            return strDate;
        }

        public static byte[] AESDecryptByte(string encryptStr)
        {
            if (string.IsNullOrEmpty(encryptStr))
                return null;

            byte[] byteArray = StringToHexByte(encryptStr);
            if (byteArray == null)
                return null;

            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = StringToHexByte(m_strKey);
            des.IV = StringToHexByte(m_strIV);

            byte[] decryptBytes = new byte[byteArray.Length];
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cs.Read(decryptBytes, 0, decryptBytes.Length);
                    cs.Close();
                    ms.Close();
                }
            }

            return decryptBytes;
        }


        private static byte[] StringToHexByte(string hexString)
        {
            if (string.IsNullOrEmpty(hexString))
                return null;

            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";

            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            return returnBytes;
        }
    }
}
