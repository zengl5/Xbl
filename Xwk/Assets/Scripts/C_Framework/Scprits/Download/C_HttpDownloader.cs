using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_HttpDownloader
    {
        //子线程负责下载，否则会阻塞主线程，Unity界面会卡主
        private Thread m_Thread = null;

        //下载进度
        public float Progress { get; private set; }

        public long DownloadFileLength { get; private set; }

        //涉及子线程要注意,Unity关闭的时候子线程不会关闭，所以要有一个标识
        private bool m_bIsStop = true;

        private const int m_nReadWriteTimeOut = 2 * 1000;//超时等待时间
        private const int m_nTimeOutWait = 5 * 1000;//超时等待时间

        public void DownloadFile(string url, string savePath, Action callback, System.Threading.ThreadPriority threadPriority = System.Threading.ThreadPriority.Normal)
        {
            C_DebugHelper.Log("C_HttpDownloader DownloadFile url = " + url);

            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

            Progress = 0;
            DownloadFileLength = 0;
            m_bIsStop = false;

            m_Thread = new Thread(delegate ()
            {
                stopWatch.Start();

                //判断保存路径是否存在
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);

                string filePath = C_DownloadMgr.StandardDownloadSavePath(url, savePath);
                //string fileName = C_DownloadMgr.StandardDownloadName(url);

                //使用流操作文件
                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

                //获取文件现在的长度
                DownloadFileLength = fs.Length;

                //获取下载文件的总长度
                long totalLength = C_DownloadMgr.GetLength(url);
                //Debug.LogFormat("<color=red>文件:{0} 已下载{1}M，剩余{2}M</color>", fileName, fileLength / 1024 / 1024, (totalLength - fileLength) / 1024 / 1024);

                //如果没下载完
                if (DownloadFileLength < totalLength)
                {
                    //断点续传核心，设置本地文件流的起始位置
                    fs.Seek(DownloadFileLength, SeekOrigin.Begin);

                    HttpWebRequest request = C_DownloadMgr.GetWebRequest(url);

                    request.ReadWriteTimeout = m_nReadWriteTimeOut;
                    request.Timeout = m_nTimeOutWait;

                    //断点续传核心，设置远程访问文件流的起始位置
                    request.AddRange((int)DownloadFileLength);

                    Stream stream = request.GetResponse().GetResponseStream();
                    byte[] buffer = new byte[4096];

                    //使用流读取内容到buffer中
                    //注意方法返回值代表读取的实际长度,并不是buffer有多大，stream就会读进去多少
                    int length = stream.Read(buffer, 0, buffer.Length);
                    while (length > 0)
                    {
                        //如果Unity客户端关闭，停止下载
                        if (m_bIsStop)
                            break;

                        //将内容再写入本地文件中
                        fs.Write(buffer, 0, length);

                        //计算进度
                        DownloadFileLength += length;
                        Progress = (float)DownloadFileLength / (float)totalLength;

                        //类似尾递归
                        length = stream.Read(buffer, 0, buffer.Length);
                    }

                    stream.Close();
                    stream.Dispose();
                }
                else
                {
                    Progress = 1;
                }

                fs.Close();

                stopWatch.Stop();
                C_DebugHelper.Log("下载完成,耗时: " + stopWatch.ElapsedMilliseconds);

                //如果下载完毕，执行回调
                if (Progress == 1)
                {
                    if (callback != null)
                        callback();

                    m_Thread.Abort();
                }
            });

            //开启子线程
            m_Thread.IsBackground = true;
            m_Thread.Priority = threadPriority;
            m_Thread.Start();
        }

        public void Close()
        {
            m_bIsStop = true;
        }

        public void Reset()
        {
            Progress = 0;
            DownloadFileLength = 0;
        }
    }
}
