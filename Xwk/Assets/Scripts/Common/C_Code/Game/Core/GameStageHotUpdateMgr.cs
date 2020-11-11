using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStageHotUpdateMgr : C_MonoSingleton<GameStageHotUpdateMgr>
{
    private List<GameStageHotUpdate> m_StageDownloadList = new List<GameStageHotUpdate>();

    private bool m_bDownloading = false;
    
    public const string c_AnyStage = "any_stage";

    public static float c_FillAmount = 0;
    public static string c_Percent = "";
    public static string c_Desc = "";
    public static int c_State = 1;

    private bool m_bReachableViaCarrierDataNetworkEnabled = false;

    private float m_fMark = 0.99f;
    private float m_fFillAmountMark = 0;
    private int m_nCountMark = 0;

    void Update()
    {
        if (m_StageDownloadList.Count > 0)
        {
            if (m_StageDownloadList[0].TotalDownloadLength != 0)
            {
                c_FillAmount = m_StageDownloadList[0].CurDownloadLength / (float)m_StageDownloadList[0].TotalDownloadLength;

                if (c_FillAmount > m_fMark)
                {
                    c_Percent = "正在安装，请稍后...";
                    c_Desc = (m_StageDownloadList[0].TotalDownloadLength / 1048576) + "MB / " + (m_StageDownloadList[0].TotalDownloadLength / 1048576) + "MB";
                }
                else
                {
                    if (c_FillAmount != 0)
                        c_Percent = (c_FillAmount * 100).ToString("F2") + "%";

                    c_Desc = (m_StageDownloadList[0].CurDownloadLength / 1048576) + "MB / " + (m_StageDownloadList[0].TotalDownloadLength / 1048576) + "MB";
                }
            }

            m_StageDownloadList[0].OnUpdate();

            if (m_StageDownloadList[0].DownloadState == 2)
            {
                m_bDownloading = false;

                //一定要这么做，先移除下载列表，再更新UI
                string stageName = m_StageDownloadList[0].StageName;

                m_StageDownloadList.RemoveAt(0);

                if (m_StageDownloadList.Count == 0)
                    C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI("UI_StageDownload");

                C_Singleton<GameResMgr>.GetInstance().InitMainAssetBundleManifest();

                //一定要先处理数据再刷新界面，如果数据没有刷新，会引发BUG
               // C_Singleton<StageMgr>.GetInstance().RefreshUITag(stageName);
            }
            else
            {
                if (!m_bDownloading)
                {
                    m_bDownloading = true;

                    m_StageDownloadList[0].StartDownload();
                }
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (m_StageDownloadList.Count > 0)
            m_StageDownloadList[0].StopDownload();
    }

    public static string GetStageConfigName(string stageName)
    {
        string serverStageName = stageName.ToLower() + "@" + GameHotUpdateMgr.CommonChannel;
        string serverStageVersion = C_Json.GetJsonKeyString(GameHotUpdateMgr.ServerHotUpdateConfig, serverStageName);

        return serverStageName + "@" + serverStageVersion;
    }

    private string GetServerStageConfig(string stageName)
    {
        return C_DownloadMgr.GetUrlString(LocalPath.ServerHotUpdateConfigPath + GetStageConfigName(stageName));
    }

    private string GetLocalStageConfig(string stageName)
    {
        //特殊处理1.6.1 和 1.6.2下载的本质是一样的，如果线上资源发生变化，这一段代码就可以删掉
        string stageConfigName = GetStageConfigName(stageName);
        if (stageConfigName == "any_stage@xblpy_common@1.6.1")
        {
            string result = C_Singleton<GameResMgr>.GetInstance().LoadString(stageConfigName, GameHotUpdateMgr.HotUpdateConfig + "/");
            if (string.IsNullOrEmpty(result))
                result = C_Singleton<GameResMgr>.GetInstance().LoadString("any_stage@xblpy_common@1.6.2", GameHotUpdateMgr.HotUpdateConfig + "/");

            return result;
        }

        return C_Singleton<GameResMgr>.GetInstance().LoadString(stageConfigName, GameHotUpdateMgr.HotUpdateConfig + "/");
    }

    //3代表正在下载，2代表需要下载，1代表需要更新，0代表是最新的
    public int GetStageHotUpdateState(string stageName)
    {
        if (GameConfig.LocalResources != 0)
            return 0;

        //临时办法
        if (stageName == "a")
            return 0;

        stageName = c_AnyStage;

        foreach (GameStageHotUpdate stageHotUpdate in m_StageDownloadList)
        {
            if (stageHotUpdate.StageName == stageName)
            {
                if (stageHotUpdate.DownloadState == 0)
                    return 2;
                else if (stageHotUpdate.DownloadState == 1)
                    return 3;
            }
        }

        string localStageConfig = GetLocalStageConfig(stageName);
        if (string.IsNullOrEmpty(localStageConfig))
            return 2;

        return 0;
    }

    public void DownloadStageHotUpdate(string stageName, Action action)
    {
        if (GameDataMgr.c_FreeSpace < 600)
        {
            DialogBox.Create("LOACAL_MAIN_DOWNLOAD_STAGE_NO_MEMORY_HINT", "LOACAL_MAIN_DOWNLOAD_STAGE_NO_MEMORY");
            return;
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            DialogBox.Create("LOACAL_HINT", "LOACAL_MAIN_DOWNLOAD_STAGE_NO_NETWORK");
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            DialogBox.Create("LOACAL_HINT", "LOACAL_MAIN_DOWNLOAD_STAGE_DATA_NETWORK", null, () =>
            {
                m_bReachableViaCarrierDataNetworkEnabled = true;

                ExecuteDownloadStageHotUpdate(stageName);

                if (action != null)
                    action();

            }, "LOACAL_DOWNLOAD_CANCLE", "LOACAL_DOWNLOAD_CONFIRM");
        }
        else
        {
            ExecuteDownloadStageHotUpdate(stageName);

            if (action != null)
                action();
        }
    }

    private void ExecuteDownloadStageHotUpdate(string stageName)
    {
        //临时办法
        stageName = c_AnyStage;

        foreach (GameStageHotUpdate stageHotUpdate in m_StageDownloadList)
        {
            if (stageHotUpdate.StageName == stageName)
            {
                if (stageHotUpdate.DownloadState == 0)
                {
                    stageHotUpdate.StartDownload();

                    C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_StageDownload");

                    StartCheckDownload();
                }

                return;
            }
        }

        string serverStageConfig = GetServerStageConfig(stageName);
        List<HotUpdateItem> serverList = GameHotUpdateMgr.ParseHotUpdateConfig(serverStageConfig);

        string localStageConfig = GetLocalStageConfig(stageName);
        List<HotUpdateItem> localList = GameHotUpdateMgr.ParseHotUpdateConfig(localStageConfig);

        List<HotUpdateItem> list = new List<HotUpdateItem>();

        for (int i = 0; i < serverList.Count; i++)
        {
            for (int j = 0; j < localList.Count; j++)
            {
                if (serverList[i].Name == localList[j].Name)
                    break;
            }

            list.Add(serverList[i]);
        }

        if (list.Count > 0)
        {
            m_StageDownloadList.Add(new GameStageHotUpdate(stageName, list));

            C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_StageDownload");

            StartCheckDownload();
        }
    }

    private int m_nCheckDownloadTimer = 0;
    private void StartCheckDownload()
    {
        if (m_nCheckDownloadTimer == 0)
        {
            m_nCheckDownloadTimer = C_Singleton<C_TimerMgr>.GetInstance().AddTimer(5, () =>
            {
                CheckDownload();

                if (m_StageDownloadList.Count == 0 && m_nCheckDownloadTimer != 0)
                {
                    C_Singleton<C_TimerMgr>.GetInstance().RemoveTimer(m_nCheckDownloadTimer);

                    m_nCheckDownloadTimer = 0;
                }
            }, -1);
        }
    }

    private void CheckDownload()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            c_State = 0;

            C_Singleton<C_TimerMgr>.GetInstance().PauseTimer(m_nCheckDownloadTimer);
            if (m_StageDownloadList.Count > 0)
                m_StageDownloadList[0].StopDownload();

            DialogBox.Create("LOACAL_HINT", "LOACAL_MAIN_DOWNLOAD_STAGE_NO_NETWORK", () =>
            {
                C_Singleton<C_TimerMgr>.GetInstance().ResumeTimer(m_nCheckDownloadTimer);
            });

            return;
        }

        c_State = 1;

        if (!m_bReachableViaCarrierDataNetworkEnabled && Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            C_Singleton<C_TimerMgr>.GetInstance().PauseTimer(m_nCheckDownloadTimer);
            if (m_StageDownloadList.Count > 0)
                m_StageDownloadList[0].StopDownload();

            DialogBox.Create("LOACAL_HINT", "LOACAL_MAIN_DOWNLOAD_STAGE_DATA_NETWORK", () =>
            {
                C_Singleton<C_TimerMgr>.GetInstance().RemoveTimer(m_nCheckDownloadTimer);
                m_nCheckDownloadTimer = 0;

                C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI("UI_StageDownload");

                //C_Singleton<StageMgr>.GetInstance().RefreshUITag(m_StageDownloadList[0].StageName);

            }, () =>
            {
                if (m_StageDownloadList.Count > 0)
                    m_StageDownloadList[0].StartDownload();

                m_bReachableViaCarrierDataNetworkEnabled = true;

                C_Singleton<C_TimerMgr>.GetInstance().ResumeTimer(m_nCheckDownloadTimer);

            }, "LOACAL_DOWNLOAD_CANCLE", "LOACAL_DOWNLOAD_CONFIRM");

            return;
        }

        if (c_FillAmount < m_fMark)
        {
            if (c_FillAmount == m_fFillAmountMark)
            {
                m_nCountMark++;
            }
            else
            {
                m_fFillAmountMark = c_FillAmount;
                m_nCountMark = 0;
            }

            if (m_nCountMark == 3)
            {
                DialogBox.Create("LOACAL_MAIN_DOWNLOAD_STAGE_BAD_NETWORK_HINT", "LOACAL_MAIN_DOWNLOAD_STAGE_BAD_NETWORK");

                if (m_StageDownloadList.Count > 0)
                    m_StageDownloadList[0].StopDownload();

                m_nCountMark = 0;

                return;
            }
        }

        if (m_StageDownloadList.Count > 0)
            m_StageDownloadList[0].StartDownload();
    }

    public void CloseDownload()
    {
        if (m_nCheckDownloadTimer != 0)
        {
            C_Singleton<C_TimerMgr>.GetInstance().RemoveTimer(m_nCheckDownloadTimer);

            m_nCheckDownloadTimer = 0;
        }

        if (m_StageDownloadList.Count > 0)
        {
            m_StageDownloadList[0].StopDownload();

          //  C_Singleton<StageMgr>.GetInstance().RefreshUITag(m_StageDownloadList[0].StageName);
        }
    }
}
