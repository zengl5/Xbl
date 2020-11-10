using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class UI_FirstLoginGuide : C_BaseUI
{
    [SerializeField]
    private GameObject m_Step0 = null;
    [SerializeField]
    private GameObject m_Step1 = null;

    [SerializeField]
    private InputField m_InputField = null;
    [SerializeField]
    private Button m_Button_Next = null;
    [SerializeField]
    private GameObject m_Image_Unused = null;

    [SerializeField]
    private Toggle m_BabyToggle0 = null;
    [SerializeField]
    private Toggle m_BabyToggle1 = null;

    [SerializeField]
    private Text m_PinYin0 = null;
    [SerializeField]
    private Text m_PinYin1 = null;

    private List<BabyNameSettingData> m_BabyNameList = new List<BabyNameSettingData>();

    private float m_fDefaultDepth = 0;

    private bool m_bEditNickname = false;

    private Camera _MainCamera;
    protected override void onOpenUI(params object[] uiObjParams)
    {
        C_MonoSingleton<PlayerController>.GetInstance().AutoPlayEnabled = false;
        C_MonoSingleton<PlayerController>.GetInstance().PlayAudio("common_406");

        m_Button_Next.interactable = false;
        m_Image_Unused.SetActive(true);
        //GameObject Cam = GameObject.FindGameObjectWithTag("ActorCamera");
        //if (Cam !=null)
        //{
        //    _MainCamera = Cam.GetComponent<Camera>();
        //    m_fDefaultDepth = _MainCamera.depth;
        //    _MainCamera.depth = 100f;
        //}

        Invoke("PlayAudioTips", 10.0f);

        GoStep0();
    }

    protected override void onCloseUI()
    {
        AudioMgr.PlaySoundEffect("public_sd_042");

        C_MonoSingleton<PlayerController>.GetInstance().AutoPlayEnabled = true;
        //if(_MainCamera!=null)
        //     _MainCamera.depth = m_fDefaultDepth;


        //C_MonoSingleton<GuideMgr>.GetInstance().TriggerGuide("FirstStage");

       // Close();

    }

    private void PlayAudioTips()
    {
        if (!m_bEditNickname)
            C_MonoSingleton<PlayerController>.GetInstance().PlayAudio("common_407");
    }

    public void GoStep0()
    {
        m_Step0.SetActive(true);
        m_Step1.SetActive(false);
    }

    public void GoStep1()
    {
        if (!NetworkMgr.IsConnected)
        {
            Tips.Create("LOACAL_BABY_NAME_NO_NETWORK_TIPS");
            return;
        }

        m_Step0.SetActive(false);
        m_Step1.SetActive(true);

        RequestBabyName();
    }

    public void InputFieldEndEdit()
    {
        if (string.IsNullOrEmpty(m_InputField.text))
        {
            m_Image_Unused.SetActive(true);
            return;
        }

        m_bEditNickname = true;

        if (!C_String.CheckStringChinese(m_InputField.text))
        {
            m_Button_Next.interactable = false;
            m_Image_Unused.SetActive(true);

            Tips.Create("LOACAL_BABY_NAME_SETTING_IMPROPER_FORMAT");

            return;
        }

        string temp = m_InputField.text[m_InputField.text.Length - 1].ToString();
        m_InputField.text = temp + temp;

        m_Button_Next.interactable = true;
        m_Image_Unused.SetActive(false);
    }

    private void RequestBabyName()
    {
        if (string.IsNullOrEmpty(m_InputField.text))
            return;

        string url = GameDataMgr.c_Host + HttpRequestConfig.GetNameVideo;
        C_DebugHelper.Log("url = " + url);

        WWWForm form = new WWWForm();
        form.AddField("babyname", m_InputField.text);
      //  form.AddField("blid", 1);
        //form.AddField("uid", PlayerData.UID);
        form.AddField("app", APP_CONST.PinYin);
        form.AddField("device", GameDataMgr.c_DeviceType);
        form.AddField("deviceid", GameDataMgr.c_DeviceUID);
        form.AddField("udid", GameDataMgr.c_UDID);
        form.AddField("token", PlayerData.Token);
        form.AddField("ver", GameConfig.AppVersion);
        C_DebugHelper.Log(Encoding.UTF8.GetString(form.data));

        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(url, form.data, (string result) =>
        {
            C_DebugHelper.Log("UI_FirstLoginGuide RequestBabyName result = " + result);

            try
            {
                if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
                    ResponseResult(result);
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("RequestBabyName : " + e);
            }
        });
    }

    public void SaveBabyName()
    {
        if (m_BabyNameList.Count > 0)
        {
            BabyNameSettingData data = null;
            if (m_BabyToggle0.isOn)
                data = m_BabyNameList[0];
            else if (m_BabyToggle1.isOn)
                data = m_BabyNameList[1];

            if (data != null)
            {
                PlayerData.BabyName = m_InputField.text;
                PlayerData.BabyNameMP3 = data.BabyNameMP3;
                PlayerData.Save();

                C_Singleton<GameDataMgr>.GetInstance().ReportedBabyInfo();
                C_Singleton<GameDataMgr>.GetInstance().LoadBabyNameAudioClip();

                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "PlayerDataChange");
            }
        }

        //需要晚点读
        C_MonoSingleton<C_AudioMgr>.GetInstance().PlayClipOneShot_MP3(C_LocalPath.DataPath + PlayerData.BabyNameMP3);
        Invoke("PlayEndAudio", 1.5f);

        CloseUI();
    }

    private void PlayEndAudio()
    {
        C_MonoSingleton<PlayerController>.GetInstance().PlayAudio("common_408");
    }

    public void PlayBabyName(int index)
    {
        if (!string.IsNullOrEmpty(m_BabyNameList[index].BabyNameMP3))
        {
            string filePath = C_LocalPath.DataPath + m_BabyNameList[index].BabyNameMP3;
            if (!File.Exists(filePath))
                C_UnityWebRequestDownloader.SyncDownloadFile(HttpRequestConfig.BabyNameMP3Url + m_BabyNameList[index].BabyNameMP3, C_LocalPath.DataPath);

            if (File.Exists(filePath))
                C_MonoSingleton<C_AudioMgr>.GetInstance().PlayClipOneShot_MP3(filePath);
        }
    }

    private void ResponseResult(string result)
    {
        JsonData pinYinListJD = C_Json.GetJsonKeyJsonData(result, "list", "pinyin");
        if (pinYinListJD != null)
        {
            m_BabyNameList.Clear();

            for (int i = 0; i < pinYinListJD.Count; i++)
            {
                BabyNameSettingData data = new BabyNameSettingData();
                data.BabyNameMP3 = C_Json.GetJsonKeyString(pinYinListJD[i], "namemp3");
                data.PinYin = C_Json.GetJsonKeyString(pinYinListJD[i], "pinyin");

                bool find = false;
                for (int j = 0; j < m_BabyNameList.Count; j++)
                {
                    if (m_BabyNameList[j].PinYin == data.PinYin)
                        find = true;
                }

                if (!find)
                    m_BabyNameList.Add(data);
            }

            if (m_BabyNameList.Count > 2)
            {
                for (int i = m_BabyNameList.Count - 1; i >= 0; i--)
                {
                    string[] astr = m_BabyNameList[i].PinYin.Split(',');
                    if (astr.Length > 1 && astr[0] != astr[1])
                        m_BabyNameList.RemoveAt(i);
                }
            }

            UpdateBabyNameSetting();
        }
    }

    private void UpdateBabyNameSetting()
    {
        if (m_BabyNameList.Count == 1)
        {
            m_BabyToggle0.gameObject.SetActive(true);
            m_PinYin0.text = m_BabyNameList[0].PinYin;

            m_BabyToggle1.gameObject.SetActive(false);
        }
        else if (m_BabyNameList.Count > 1)
        {
            m_BabyToggle0.gameObject.SetActive(true);
            m_PinYin0.text = m_BabyNameList[0].PinYin;

            m_BabyToggle1.gameObject.SetActive(true);
            m_PinYin1.text = m_BabyNameList[1].PinYin;
        }
        else
        {
            m_BabyToggle0.gameObject.SetActive(false);
            m_BabyToggle1.gameObject.SetActive(false);
        }
    }

    public void Close()
    {
        C_MonoSingleton<PlayerController>.GetInstance().PlayAudio("common_409");

        CloseUI();
    }
}
