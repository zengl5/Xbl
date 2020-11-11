using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.C_Framework
{
    public class C_UnityWebRequestDownloader
    {
        public void AsyncDownloadFile(string url, string savePath, Action<byte[]> callback)
        {
            C_MonoSingleton<C_GameFramework>.GetInstance().StartCoroutine(Execute(url, savePath, callback));
        }

        private IEnumerator Execute(string url, string savePath, Action<byte[]> callback)
        {
            C_DebugHelper.Log("AsyncDownloadFile 下载开始 url = " + url + ", savePath = " + savePath);

            //System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            //stopWatch.Start();

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.isHttpError || request.isNetworkError)
                {
                    C_DebugHelper.LogWarning("request.isError" + request.isNetworkError);
                }
                else
                {
                    byte[] bytes = request.downloadHandler.data;

                    if (callback != null)
                        callback(bytes);

                    C_DownloadMgr.CreatFile(url, savePath, bytes);
                }

                C_DebugHelper.Log("下载完成");

               //stopWatch.Stop();
               // C_DebugHelper.Log("下载完成,耗时: " + stopWatch.ElapsedMilliseconds);
            }
        }

        public static void SyncDownloadFile(string url, string savePath)
        {
            C_DebugHelper.Log("SyncDownloadFile 下载开始 url = " + url + ", savePath = " + savePath);

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SendWebRequest();

                while (!request.isDone)
                {
                    if (request.isHttpError || request.isNetworkError)
                    {
                        C_DebugHelper.LogWarning("request.isError" + request.isNetworkError);
                        return;
                    }
                }

                if (request.isHttpError || request.isNetworkError)
                    C_DebugHelper.LogWarning("request.isError" + request.isNetworkError);
                else
                    C_DownloadMgr.CreatFile(url, savePath, request.downloadHandler.data);
            }
        }
    }
}
