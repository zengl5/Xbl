using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.Scripts.C_Framework
{
    public static class C_XXTea
    {
        public static string Encrypt(int target, string key)
        {
            return Encrypt(target.ToString(), key);
        }

        public static string Encrypt(float target, string key)
        {
            return Encrypt(target.ToString(), key);
        }

        public static string Encrypt(double target, string key)
        {
            return Encrypt(target.ToString(), key);
        }

        public static string Encrypt(string target, string key)
        {
            byte[] bytes = Encrypt(System.Text.Encoding.UTF8.GetBytes(target), System.Text.Encoding.UTF8.GetBytes(key));
            if (bytes == null)
                return "";

            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static Byte[] Encrypt(Byte[] data, Byte[] key)
        {
            if (data == null || key == null)
                return null;

            return ToByteArray(Encrypt(ToUInt32Array(data, true), ToUInt32Array(key, false)), false);
        }

        public static UInt32[] Encrypt(UInt32[] v, UInt32[] k)
        {
            if (v == null || k == null)
                return null;

            try
            {
                Int32 n = v.Length - 1;
                if (n < 1)
                    return v;

                if (k.Length < 4)
                {
                    UInt32[] key = new UInt32[4];
                    k.CopyTo(key, 0);
                    k = key;
                }

                UInt32 z = v[n], y = v[0], delta = 0x9E3779B9, sum = 0, e;
                Int32 p, q = 6 + 52 / (n + 1);
                while (q-- > 0)
                {
                    sum = unchecked(sum + delta);
                    e = sum >> 2 & 3;
                    for (p = 0; p < n; p++)
                    {
                        y = v[p + 1];
                        z = unchecked(v[p] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                    }
                    y = v[0];
                    z = unchecked(v[n] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                }

                return v;
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("C_XXTea Encrypt e.Message = " + e.Message);
            }

            return null;
        }

        public static string Decrypt(string target, string key)
        {
            if (string.IsNullOrEmpty(target) || string.IsNullOrEmpty(key))
                return "";

            return System.Text.Encoding.UTF8.GetString(Decrypt(System.Text.Encoding.UTF8.GetBytes(target), System.Text.Encoding.UTF8.GetBytes(key)));
        }

        public static Byte[] Decrypt(Byte[] data, Byte[] key)
        {
            if (data == null || key == null)
                return null;

            return ToByteArray(Decrypt(ToUInt32Array(data, false), ToUInt32Array(key, false)), true);
        }

        public static UInt32[] Decrypt(UInt32[] v, UInt32[] k)
        {
            if (v == null || k == null)
                return null;

            try
            {
                Int32 n = v.Length - 1;
                if (n < 1)
                    return v;

                if (k.Length < 4)
                {
                    UInt32[] key = new UInt32[4];
                    k.CopyTo(key, 0);
                    k = key;
                }

                UInt32 z = v[n], y = v[0], delta = 0x9E3779B9, sum, e;
                Int32 p, q = 6 + 52 / (n + 1);
                sum = unchecked((UInt32)(q * delta));
                while (sum != 0)
                {
                    e = sum >> 2 & 3;
                    for (p = n; p > 0; p--)
                    {
                        z = v[p - 1];
                        y = unchecked(v[p] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                    }
                    z = v[n];
                    y = unchecked(v[0] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                    sum = unchecked(sum - delta);
                }

                return v;
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("C_XXTea Decrypt e.Message = " + e.Message);
            }

            return null;
        }

        private static UInt32[] ToUInt32Array(Byte[] data, Boolean includeLength)
        {
            if (data == null)
                return null;

            try
            {
                Int32 n = (((data.Length & 3) == 0) ? (data.Length >> 2) : ((data.Length >> 2) + 1));
                UInt32[] result;

                if (includeLength)
                {
                    result = new UInt32[n + 1];
                    result[n] = (UInt32)data.Length;
                }
                else
                {
                    result = new UInt32[n];
                }

                n = data.Length;
                for (Int32 i = 0; i < n; i++)
                    result[i >> 2] |= (UInt32)data[i] << ((i & 3) << 3);

                return result;
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("C_XXTea ToUInt32Array e.Message = " + e.Message);
            }

            return null;
        }

        private static Byte[] ToByteArray(UInt32[] data, Boolean includeLength)
        {
            if (data == null)
                return null;

            try
            {
                Int32 n;
                if (includeLength)
                    n = (Int32)data[data.Length - 1];
                else
                    n = data.Length << 2;

                Byte[] result = new Byte[n];
                for (Int32 i = 0; i < n; i++)
                    result[i] = (Byte)(data[i >> 2] >> ((i & 3) << 3));

                return result;
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("C_XXTea ToByteArray e.Message = " + e.Message);
            }

            return null;
        }
    }
}
