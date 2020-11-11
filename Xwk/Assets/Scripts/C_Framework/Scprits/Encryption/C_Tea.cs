using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.Scripts.C_Framework
{
    public static class C_Tea
    {
        public static string Encrypt(int target)
        {
            return Encrypt(target.ToString(), string.Empty);
        }

        public static string Encrypt(int target, string key)
        {
            return Encrypt(target.ToString(), key);
        }

        public static string Encrypt(float target)
        {
            return Encrypt(target.ToString(), string.Empty);
        }

        public static string Encrypt(float target, string key)
        {
            return Encrypt(target.ToString(), key);
        }

        public static string Encrypt(double target)
        {
            return Encrypt(target.ToString(), string.Empty);
        }

        public static string Encrypt(double target, string key)
        {
            return Encrypt(target.ToString(), key);
        }

        public static string Encrypt(string target)
        {
            return Encrypt(target, string.Empty);
        }

        public static string Encrypt(string target, string key)
        {
            if (string.IsNullOrEmpty(target))
                return "";

            if (0 == key.Length)
                key = "cai1234567890cai";

            System.Text.Encoding encoder = System.Text.Encoding.UTF8;
            Byte[] data = Encrypt(encoder.GetBytes(target), encoder.GetBytes(key));
            return System.Convert.ToBase64String(data);
        }

        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            byte[] dataBytes;
            if (data.Length % 2 == 0)
            {
                dataBytes = data;
            }
            else
            {
                dataBytes = new byte[data.Length + 1];
                Array.Copy(data, 0, dataBytes, 0, data.Length);
                dataBytes[data.Length] = 0x0;
            }

            byte[] result = new byte[dataBytes.Length * 4];
            uint[] formattedKey = FormatKey(key);
            uint[] tempData = new uint[2];
            for (int i = 0; i < dataBytes.Length; i += 2)
            {
                tempData[0] = dataBytes[i];
                tempData[1] = dataBytes[i + 1];
                code(tempData, formattedKey);
                Array.Copy(ConvertUIntToByteArray(tempData[0]), 0, result, i * 4, 4);
                Array.Copy(ConvertUIntToByteArray(tempData[1]), 0, result, i * 4 + 4, 4);
            }
            return result;
        }

        public static string Decrypt(string target)
        {
            if (string.IsNullOrEmpty(target))
                return "";

            return Decrypt(target, string.Empty);
        }
        public static string Decrypt(string target, string key)
        {
            if (0 == key.Length)
                key = "cai1234567890cai";

            System.Text.Encoding encoder = System.Text.Encoding.UTF8;
            return encoder.GetString(Decrypt(System.Convert.FromBase64String(target), encoder.GetBytes(key)));
        }

        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            uint[] formattedKey = FormatKey(key);
            int x = 0;
            uint[] tempData = new uint[2];
            byte[] dataBytes = new byte[data.Length / 8 * 2];
            for (int i = 0; i < data.Length; i += 8)
            {
                tempData[0] = ConvertByteArrayToUInt(data, i);
                tempData[1] = ConvertByteArrayToUInt(data, i + 4);
                decode(tempData, formattedKey);
                dataBytes[x++] = (byte)tempData[0];
                dataBytes[x++] = (byte)tempData[1];
            }

            //修剪添加的空字符
            if (dataBytes[dataBytes.Length - 1] == 0x0)
            {
                byte[] result = new byte[dataBytes.Length - 1];
                Array.Copy(dataBytes, 0, result, 0, dataBytes.Length - 1);
            }
            return dataBytes;

        }

        static uint[] FormatKey(byte[] key)
        {
            if (key.Length == 0)
                throw new ArgumentException("Key must be between 1 and 16 characters in length");

            byte[] refineKey = new byte[16];
            if (key.Length < 16)
            {
                Array.Copy(key, 0, refineKey, 0, key.Length);
                for (int k = key.Length; k < 16; k++)
                {
                    refineKey[k] = 0x20;
                }
            }
            else
            {
                Array.Copy(key, 0, refineKey, 0, 16);
            }

            uint[] formattedKey = new uint[4];
            int j = 0;
            for (int i = 0; i < refineKey.Length; i += 4)
                formattedKey[j++] = ConvertByteArrayToUInt(refineKey, i);

            return formattedKey;
        }

        #region Tea Algorithm
        static void code(uint[] v, uint[] k)
        {
            uint y = v[0];
            uint z = v[1];
            uint sum = 0;
            uint delta = 0x9e3779b9;
            uint n = 16;
            while (n-- > 0)
            {
                sum += delta;
                y += (z << 4) + k[0] ^ z + sum ^ (z >> 5) + k[1];
                z += (y << 4) + k[2] ^ y + sum ^ (y >> 5) + k[3];
            }
            v[0] = y;
            v[1] = z;
        }

        static void decode(uint[] v, uint[] k)
        {
            uint n = 16;
            uint sum;
            uint y = v[0];
            uint z = v[1];
            uint delta = 0x9e3779b9;

            sum = delta << 4;
            while (n-- > 0)
            {
                z -= (y << 4) + k[2] ^ y + sum ^ (y >> 5) + k[3];
                y -= (z << 4) + k[0] ^ z + sum ^ (z >> 5) + k[1];
                sum -= delta;
            }
            v[0] = y;
            v[1] = z;
        }
        #endregion

        private static byte[] ConvertUIntToByteArray(uint v)
        {
            byte[] result = new byte[4];
            result[0] = (byte)(v & 0xFF);
            result[1] = (byte)((v >> 8) & 0xFF);
            result[2] = (byte)((v >> 16) & 0xFF);
            result[3] = (byte)((v >> 24) & 0xFF);
            return result;
        }

        private static uint ConvertByteArrayToUInt(byte[] v, int offset)
        {
            if (offset + 4 > v.Length)
                return 0;

            uint output;
            output = (uint)v[offset];
            output |= (uint)(v[offset + 1] << 8);
            output |= (uint)(v[offset + 2] << 16);
            output |= (uint)(v[offset + 3] << 24);
            return output;
        }
    }
}
