using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;

public class NetworkData
{
    public string URL = "";
    public byte[] Data = null;
    public Action<string> Callback = null;
    public float Timeout = 2.0f;
}

public class NetworkMgr : C_Singleton<NetworkMgr>
{
    public static bool IsConnected { get { return Application.internetReachability != NetworkReachability.NotReachable; } }

    private List<NetworkData> m_NetworkDataList = new List<NetworkData>();

    private NetworkData m_CurNetworkData = null;
    private Coroutine m_CurCoroutine = null;
    private HttpWebRequest m_Request = null;

    protected override void Init()
    {
        C_MonoSingleton<C_GameFramework>.GetInstance().onRealtimeUpdate += onRealtimeUpdate;
    }

    protected override void Destroy()
    {
        C_MonoSingleton<C_GameFramework>.GetInstance().onRealtimeUpdate -= onRealtimeUpdate;
    }

    protected virtual void onRealtimeUpdate(float deltaTime)
    {
        if (m_CurNetworkData != null)
        {
            m_CurNetworkData.Timeout -= deltaTime;

            if (m_CurNetworkData.Timeout <= 0)
            {
                m_CurNetworkData.Callback("");
                m_CurNetworkData = null;
            }
        }

        if (m_CurNetworkData == null && m_NetworkDataList.Count > 0)
        {
            m_CurNetworkData = m_NetworkDataList[0];
            m_NetworkDataList.RemoveAt(0);

            if (m_CurCoroutine != null)
                C_MonoSingleton<C_GameFramework>.GetInstance().StopCoroutine(m_CurCoroutine);

            //开始请求数据
            m_CurCoroutine = C_MonoSingleton<C_GameFramework>.GetInstance().StartCoroutine(GetResponse(m_CurNetworkData));
        }
    }

    private IEnumerator GetResponse(NetworkData networkData)
    {
        // 情况之前发生错误的请求
        if (m_Request != null)
        {
            m_Request = null;

            System.GC.Collect();
        }

        // 获取HttpWebRequest
        m_Request = (HttpWebRequest)WebRequest.Create(networkData.URL); ;
        m_Request.Timeout = (int)(networkData.Timeout * 1000);

        if (networkData.URL.StartsWith("https", StringComparison.OrdinalIgnoreCase))
        {
            m_Request.ProtocolVersion = HttpVersion.Version10;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
        }

        // 请求资源
        Stream myResponseStream = null;
        StreamReader myStreamReader = null;
        string retString = "";

        try
        {
            if (networkData.Data == null)
            {
                m_Request.Method = "GET";
                m_Request.ContentType = "text/html;charset=UTF-8";
            }
            else
            {
                m_Request.Method = "POST";
                m_Request.ContentType = "application/x-www-form-urlencoded";
                m_Request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                m_Request.CookieContainer = new CookieContainer();
                m_Request.ContentLength = networkData.Data.Length;

                using (Stream stream = m_Request.GetRequestStream()) { stream.Write(networkData.Data, 0, networkData.Data.Length); }
            }

            HttpWebResponse response = (HttpWebResponse)m_Request.GetResponse();
            myResponseStream = response.GetResponseStream();
            myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            retString = myStreamReader.ReadToEnd();

            response.Close();
            response = null;
        }
        catch (WebException we)
        {
            if (we.Status == WebExceptionStatus.ProtocolError)
            {
                myResponseStream = ((HttpWebResponse)we.Response).GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                retString = myStreamReader.ReadToEnd();
            }
        }
        finally
        {
            if (myStreamReader != null)
            {
                myStreamReader.Close();
                myStreamReader = null;
            }


            if (myResponseStream != null)
            {
                myResponseStream.Close();
                myResponseStream = null;
            }

            networkData.Callback(retString);

            if (m_CurNetworkData != null && m_CurNetworkData.URL == networkData.URL)
                m_CurNetworkData = null;
        }

        yield return null;
    }

    public void SendHttpGet(string url, Action<string> callback, float timeout = 2.0f)
    {
        if (string.IsNullOrEmpty(url) || callback == null)
            return;

        NetworkData networkData = new NetworkData();
        networkData.URL = url;
        networkData.Callback = callback;
        networkData.Timeout = timeout;

        m_NetworkDataList.Add(networkData);
    }
    public void PokeRequestHttp(WWWForm form, string url,Action<string> callback, float timeout = 2.0f)
    {
        form.AddField("uid", PlayerData.UID);
        form.AddField("app", APP_CONST.PinYin);
        form.AddField("device", GameDataMgr.c_DeviceType);
        form.AddField("deviceid", GameDataMgr.c_DeviceUID);
        form.AddField("udid", GameDataMgr.c_UDID);
        form.AddField("uid", PlayerData.UID);
        form.AddField("ver", GameConfig.AppVersion);

        C_DebugHelper.Log("NetworkMgr SendHttpPost url = " + url + ", data = " + Encoding.UTF8.GetString(form.data));

        SendHttpPost(url, form.data, callback, timeout);
    }
 
    public void PokeRequestHttp(string url, Dictionary<string, string> data, Action<string> callback, float timeout = 2.0f)
    {
        WWWForm form = new WWWForm();
        if (data != null)
        {
            foreach (KeyValuePair<string, string> kv in data)
                form.AddField(kv.Key, kv.Value);
        }
        form.AddField("uid", PlayerData.UID);
        form.AddField("app", APP_CONST.PinYin);
        form.AddField("device", GameDataMgr.c_DeviceType);
        form.AddField("deviceid", GameDataMgr.c_DeviceUID);
        form.AddField("udid", GameDataMgr.c_UDID);
        form.AddField("uid", PlayerData.UID);
        form.AddField("ver", GameConfig.AppVersion);

        C_DebugHelper.Log("NetworkMgr SendHttpPost url = " + url + ", data = " + Encoding.UTF8.GetString(form.data));

        SendHttpPost(url, form.data, callback, timeout);
    }
    public void SendHttpPost(string url, Dictionary<string, string> data, Action<string> callback, float timeout = 2.0f)
    {
        WWWForm form = new WWWForm();
        if (data!=null)
        {
            foreach (KeyValuePair<string, string> kv in data)
                form.AddField(kv.Key, kv.Value);
        }

        form.AddField("uid", PlayerData.UID);
        form.AddField("app", APP_CONST.PinYin);
        form.AddField("device", GameDataMgr.c_DeviceType);
        form.AddField("deviceid", GameDataMgr.c_DeviceUID);
        form.AddField("udid", GameDataMgr.c_UDID);
        form.AddField("ver", GameConfig.AppVersion);

        C_DebugHelper.Log("NetworkMgr SendHttpPost url = " + url + ", data = " + Encoding.UTF8.GetString(form.data));

        SendHttpPost(url, form.data, callback, timeout);
    }

    public void SendHttpPost(string url, string data, Action<string> callback, float timeout = 2.0f)
    {
        SendHttpPost(url, Encoding.UTF8.GetBytes(data), callback, timeout);
    }

    public void SendHttpPost(string url, byte[] data, Action<string> callback, float timeout = 2.0f)
    {
        if (string.IsNullOrEmpty(url) || callback == null)
            return;

        NetworkData networkData = new NetworkData();
        networkData.URL = url;
        networkData.Data = data;
        networkData.Callback = callback;
        networkData.Timeout = timeout;

        m_NetworkDataList.Add(networkData);
    }

    private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    {
        //总是接受
        return true;
    }
}
