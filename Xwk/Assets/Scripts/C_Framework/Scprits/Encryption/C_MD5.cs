using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Assets.Scripts.C_Framework
{
    public class C_MD5
    {
        /// <summary>
        /// MD5 16位加密
        /// </summary>
        /// <param name="convertString"></param>
        /// <returns></returns>
        public static string GetMD5_16(string convertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string strPwd = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(convertString)), 4, 8);
            strPwd = strPwd.Replace("-", "");
            return strPwd;
        }

        /// <summary>
        /// MD5 32位加密
        /// </summary>
        /// <param name="convertString"></param>
        /// <returns></returns>
        public static string GetMD5_32(string convertString)
        {
            StringBuilder strPwd = new StringBuilder();

            //实例化一个md5对像
            MD5 md5 = MD5.Create();

            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] byteMD5 = md5.ComputeHash(Encoding.UTF8.GetBytes(convertString));

            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < byteMD5.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                strPwd.Append(byteMD5[i].ToString("x2"));
                //strPwd.Append(byteMD5[i].ToString("X2"));
            }

            return strPwd.ToString();
        }

        public static string GetMD5_32(byte[] bytes)
        {
            StringBuilder strPwd = new StringBuilder();

            //实例化一个md5对像
            MD5 md5 = MD5.Create();

            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] byteMD5 = md5.ComputeHash(bytes);

            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < byteMD5.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                strPwd.Append(byteMD5[i].ToString("x2"));
                //strPwd.Append(byteMD5[i].ToString("X2"));
            }

            return strPwd.ToString();
        }
    }
}