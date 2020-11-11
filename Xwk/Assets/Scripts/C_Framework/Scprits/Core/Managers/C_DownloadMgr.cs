using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.C_Framework
{
    public static class C_DownloadMgr
    {
        public static string StandardDownloadSavePath(string url, string resPath)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(resPath))
                return "";

            return resPath + url.Substring(url.LastIndexOf("/") + 1).ToLower();
        }

        public static string StandardDownloadName(string url)
        {
            if (string.IsNullOrEmpty(url))
                return "";

            return url.Substring(url.LastIndexOf("/") + 1).ToLower();
        }

        public static void CreatFile(string url, string savePath, byte[] bytes)
        {
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            FileStream fs = new FileStream(StandardDownloadSavePath(url, savePath), FileMode.OpenOrCreate);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }

        public static long GetLength(string url)
        {
            HttpWebRequest requet = GetWebRequest(url);
            requet.Method = "HEAD";
            HttpWebResponse response = requet.GetResponse() as HttpWebResponse;
            long ret = response.ContentLength;
            response.Close();//这里是新加的，切记response必须要关闭。
            return ret;
        }

        public static string GetUrlString(string url)
        {
            byte[] bytes = GetUrlByte(url);
            if (bytes != null)
                return Encoding.UTF8.GetString(bytes);
            
            return "";
        }

        public static byte[] GetUrlByte(string url)
        {
            C_DebugHelper.Log("GetUrlByte url = " + url);

            byte[] bytes = null;

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SendWebRequest();

                while (!request.isDone)
                {
                    if (request.isHttpError || request.isNetworkError)
                    {
                        C_DebugHelper.LogWarning("request.isError" + request.isNetworkError);
                        return bytes;
                    }
                }

                if (request.isHttpError || request.isNetworkError)
                    C_DebugHelper.LogWarning("request.isError" + request.isNetworkError);
                else
                    bytes = request.downloadHandler.data;
            }

            return bytes;
        }

        public static HttpWebRequest GetWebRequest(string url)
        {
            HttpWebRequest request = null;
            //添加https
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create(url);
            }

            return request;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //总是接受
            return true;
        }
    }
}
