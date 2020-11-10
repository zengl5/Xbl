using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GamePublicHotUpdateMgr
{
    private List<HotUpdateItem> m_DownloadList = new List<HotUpdateItem>();

    private C_HttpDownloader m_Downloader = new C_HttpDownloader();

    private long m_TotalDownloadLength = 0;
    private long m_CurDownloadLength = 0;

    public float Progress
    {
        get
        {
            if (m_TotalDownloadLength != 0)
                return (m_CurDownloadLength + m_Downloader.DownloadFileLength) / m_TotalDownloadLength;

            return 0;
        }
    }

    public bool Finish = false;
    private bool m_bDownloading = false;
 //   private int m_nDownloadCount = 0;

    public void OnUpdate()
    {
        if (!Finish && !m_bDownloading)
            Download();
    }

    private string GetServerPublicConfig()
    {
        return C_DownloadMgr.GetUrlString(LocalPath.ServerHotUpdateConfigPath + "public.txt");
    }

    //检查公共配置
    public void CheckPublicConfig()
    {
        SkipDownload();
    }

    private void SkipDownload()
    {
        m_TotalDownloadLength = 1;
        m_CurDownloadLength = 1;

        m_DownloadList.Clear();

        Finish = true;
    }

    //下载开始，从最尾端开始
    private void Download()
    {
    }
}
