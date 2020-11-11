using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{

    //实现有问题
    public class C_MultiHttpDownloader
    {
        public int ThreadNum = 5;
        public bool[] ThreadStatus = null;
        public string[] FileNames = null;
        public int[] StartPos = null;
        public int[] FileSize = null;
        public bool IsMerge = false;

        private System.Diagnostics.Stopwatch m_StopWatch = null;

        public void DownloadFile(string url, string savePath, Action callback, int threadNum)
        {
            m_StopWatch = new System.Diagnostics.Stopwatch();
            m_StopWatch.Start();

            ThreadNum = threadNum;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            long fileSizeAll = request.GetResponse().ContentLength;
            InitThread(fileSizeAll);

            System.Threading.Thread[] threads = new System.Threading.Thread[ThreadNum];
            C_HttpMultiThreadDownloadloader[] httpDownloads = new C_HttpMultiThreadDownloadloader[ThreadNum];
            for (int i = 0; i < ThreadNum; i++)
            {
                httpDownloads[i] = new C_HttpMultiThreadDownloadloader(request, this, i);
                threads[i] = new System.Threading.Thread(new System.Threading.ThreadStart(httpDownloads[i].Receive));
                threads[i].Name = string.Format("线程{0}:", i);
                threads[i].Start();
            }

            C_MonoSingleton<C_GameFramework>.GetInstance().StartCoroutine(MergeFile(url, savePath, callback));
        }

        private void InitThread(long fileSizeAll)
        {
            ThreadStatus = new bool[ThreadNum];
            FileNames = new string[ThreadNum];
            StartPos = new int[ThreadNum];                              //下载字节起始点
            FileSize = new int[ThreadNum];                              //该进程文件大小
            int fileThread = (int)fileSizeAll / ThreadNum;                 //单进程文件大小
            int fileThreade = fileThread + (int)fileSizeAll % ThreadNum;   //最后一个进程的资源大小
            for (int i = 0; i < ThreadNum; i++)
            {
                ThreadStatus[i] = false;
                FileNames[i] = i.ToString() + ".dat";
                if (i < ThreadNum - 1)
                {
                    StartPos[i] = fileThread * i;
                    FileSize[i] = fileThread;
                }
                else
                {
                    StartPos[i] = fileThread * i;
                    FileSize[i] = fileThreade;
                }
            }
        }

        private int bufferSize = 1024;
        IEnumerator MergeFile(string url, string savePath, Action callback)
        {
            while (true)
            {
                IsMerge = true;

                for (int i = 0; i < ThreadNum; i++)
                {
                    if (ThreadStatus[i] == false)
                    {
                        IsMerge = false;

                        yield return 0;

                        System.Threading.Thread.Sleep(100);
                        break;
                    }
                }

                if (IsMerge)
                    break;
            }
            byte[] bytes = new byte[bufferSize];

            FileStream fs = new FileStream(C_DownloadMgr.StandardDownloadSavePath(url, savePath), FileMode.OpenOrCreate);
            FileStream fsTemp = null;

            for (int i = 0; i < ThreadNum; i++)
            {
                fsTemp = new FileStream(FileNames[i], FileMode.OpenOrCreate);

                int readBytes;
                while ((readBytes = fsTemp.Read(bytes, 0, bytes.Length)) > 0)
                    fs.Write(bytes, 0, readBytes);

                fsTemp.Close();
            }

            fs.Close();

            if (callback != null)
                callback();

            m_StopWatch.Stop();
            C_DebugHelper.Log("下载完成,耗时: " + m_StopWatch.ElapsedMilliseconds);

            yield return null;

            DeleteCacheFiles();
        }

        private void DeleteCacheFiles()
        {
            for (int i = 0; i < ThreadNum; i++)
            {
                FileInfo info = new FileInfo(FileNames[i]);
                C_DebugHelper.LogFormat("Delete File {0} OK!", FileNames[i]);
                info.Delete();
            }
        }
    }

    public class C_HttpMultiThreadDownloadloader
    {
        private int m_nThreadId = 0;
        private C_MultiHttpDownloader m_C_MultiHttpDownloader = null;
        
        private HttpWebRequest m_Request = null;

        public C_HttpMultiThreadDownloadloader(HttpWebRequest request, C_MultiHttpDownloader loader, int threadId)
        {
            m_Request = request;
            m_nThreadId = threadId;
            m_C_MultiHttpDownloader = loader;
        }

        private int bufferSize = 1024;
        public void Receive()
        {
            string fileName = m_C_MultiHttpDownloader.FileNames[m_nThreadId];

            byte[] bytes = new byte[bufferSize];

            FileStream fs = new FileStream(fileName, System.IO.FileMode.OpenOrCreate);
            Stream ns = null;

            try
            {
                m_Request.AddRange(m_C_MultiHttpDownloader.StartPos[m_nThreadId], m_C_MultiHttpDownloader.StartPos[m_nThreadId] + m_C_MultiHttpDownloader.FileSize[m_nThreadId]);
                ns = m_Request.GetResponse().GetResponseStream();
                
                int readBytes;
                while ((readBytes = ns.Read(bytes, 0, bytes.Length)) > 0)
                    fs.Write(bytes, 0, readBytes);

                fs.Close();
                ns.Close();
            }
            catch (Exception er)
            {
                Debug.LogError(er.Message);
                fs.Close();
            }

            Debug.Log("线程[" + m_nThreadId.ToString() + "] 结束!");
            m_C_MultiHttpDownloader.ThreadStatus[m_nThreadId] = true;
        }
    }
}
