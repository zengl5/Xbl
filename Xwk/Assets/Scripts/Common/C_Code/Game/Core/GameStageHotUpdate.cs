using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class GameStageHotUpdate
{
    private C_HttpDownloader m_Downloader = new C_HttpDownloader();

    public string StageName = "";

    private List<HotUpdateItem> m_DownloadList = new List<HotUpdateItem>();

    public long TotalDownloadLength = 0;
    private long m_CurDownloadLength = 0;
    public long CurDownloadLength { get { return m_CurDownloadLength + m_Downloader.DownloadFileLength; } }

    //0是未下载，1是下载中，2是下载完成
    public int DownloadState = 0;

    private bool m_bDownloading = false;
    private int m_nDownloadCount = 0;
    private bool m_bDownloadEnabled = false;

    public GameStageHotUpdate(string stageName, List<HotUpdateItem> downloadList)
    {
        StageName = stageName;
        m_DownloadList = downloadList;

        DownloadState = 0;

        m_nDownloadCount = m_DownloadList.Count;

        TotalDownloadLength = 0;
        for (int i = 0; i < m_DownloadList.Count; i++)
            TotalDownloadLength += C_DownloadMgr.GetLength(GameDataMgr.c_HotUpdate + GetDownloadName(m_DownloadList[i]));

        m_bDownloadEnabled = false;
    }

    public void OnUpdate()
    {
        if (DownloadState != 2 && !m_bDownloading && m_bDownloadEnabled)
            Download();
    }

    public void StartDownload()
    {
        m_bDownloadEnabled = true;

        if (DownloadState == 0)
            DownloadState = 1;
    }

    public void StopDownload()
    {
        m_bDownloadEnabled = false;
        m_bDownloading = false;

        DownloadState = 0;

        m_Downloader.Close();
    }

    //下载开始，从最尾端开始
    private void Download()
    {
        if (m_nDownloadCount > 0)
        {
            m_bDownloading = true;

            string url = GameDataMgr.c_HotUpdate + GetDownloadName(m_DownloadList[m_nDownloadCount - 1]);

            string localFilePath = LocalPath.LocalPackagingResources + GetDownloadName(m_DownloadList[m_nDownloadCount - 1]);

            m_Downloader.DownloadFile(url, C_String.GetSavePath(localFilePath), () =>
            {
                m_CurDownloadLength += m_Downloader.DownloadFileLength;

                m_Downloader.Reset();

                m_nDownloadCount--;

                m_bDownloading = false;
            });
        }
        else
        {
            m_bDownloading = true;

            //如果都下载完了，就需要删除原来的文件，替换下载的文件
            if (ReplaceAllDownloadFile())
            {
                //配置文件替换，整个下载过程完成
                string stageConfigName = GameStageHotUpdateMgr.GetStageConfigName(StageName);

                string localFilePath = LocalPath.LocalHotUpdateConfigPath + stageConfigName;
                if (File.Exists(localFilePath))
                    File.Delete(localFilePath);

                m_Downloader.DownloadFile(LocalPath.ServerHotUpdateConfigPath + stageConfigName, LocalPath.LocalHotUpdateConfigPath, () =>
                {
                    m_bDownloading = false;

                    DownloadState = 2;
                });
            }
        }
    }

    private bool ReplaceAllDownloadFile()
    {
        for (int i = 0; i < m_DownloadList.Count; i++)
        {
            string sourceFile = LocalPath.LocalPackagingResources + GetDownloadName(m_DownloadList[i]);

            if (!C_ZipUtility.UnzipFile(sourceFile, LocalPath.LocalPackagingResources))
            {
                C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI("UI_StageDownload");
                C_MonoSingleton<GameStageHotUpdateMgr>.GetInstance().CloseDownload();

                DialogBox.Create("LOACAL_MAIN_DOWNLOAD_STAGE_NO_MEMORY_UNZIP_HINT", "LOACAL_MAIN_DOWNLOAD_STAGE_NO_MEMORY_UNZIP");

                return false;
            }
        }

        for (int i = 0; i < m_DownloadList.Count; i++)
        {
            string sourceFile = LocalPath.LocalPackagingResources + GetDownloadName(m_DownloadList[i]);
            if (File.Exists(sourceFile))
                File.Delete(sourceFile);
        }

        m_DownloadList.Clear();

        return true;
    }

    private string GetDownloadName(HotUpdateItem item)
    {

#if UNITY_IPHONE
        return item.Name + "_ios.zip";
#elif UNITY_EDITOR
        return item.Name + "_windows.zip";
#elif UNITY_ANDROID
        return item.Name + "_android.zip";
#else
        return item.Name + "_windows.zip";
#endif
    }
}
