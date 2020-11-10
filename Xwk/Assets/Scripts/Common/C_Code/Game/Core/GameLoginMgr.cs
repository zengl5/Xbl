using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLoginMgr
{
    //-------------------------Phone Login-------------------------
    public static void RequestPhoneVerificationCode(string phone)
    {
        if (!CheckoutPhone(phone))
            return;

        char[] charArray = phone.ToCharArray();
        Array.Reverse(charArray);

        string vsign = new string(charArray) + "dDY2eS5jb20";

        vsign = C_MD5.GetMD5_32(vsign);

        char[] vsingCharArray = vsign.ToCharArray();
        char tmp = vsingCharArray[5];
        vsingCharArray[5] = vsingCharArray[23];
        vsingCharArray[23] = tmp;
        string sign = new string(vsingCharArray);

        sign = C_MD5.GetMD5_32(sign);

        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("phone", phone);
        data.Add("sign", sign);
        data.Add("token", PlayerData.Token);

        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_Host + HttpRequestConfig.SMSLoginCode, data, (string result) =>
        {
            C_DebugHelper.Log("RequestPhoneVerificationCode result = " + result);
        });
    }

    public static void RequestPhoneLogin(string phone, string phoneVerificationCode)
    {
        if (!CheckoutPhone(phone))
            return;

        if (phoneVerificationCode.Length < 4)
        {
            Tips.Create("LOACAL_ERROR_LOGIN_WECHAT_QR_CODE_ENTER");
            return;
        }

        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("phone", phone);
        data.Add("code", phoneVerificationCode);
        data.Add("token", PlayerData.Token);
        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_Host + HttpRequestConfig.LoginPhoneVerificationCode, data, (string result) =>
        {
            //``ResponseLoginResult(result);
            C_DebugHelper.Log("RequestPhoneLogin result = " + result);
            if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
            {
                PlayerData.Token = C_Json.GetJsonKeyString(result, "token");
                RequestTokenLogin();
            }

        });
    }

    //-------------------------Phone Binding-------------------------
    public static void RequestPhoneBinding(string phone, string phoneVerificationCode)
    {
        if (!CheckoutPhone(phone))
            return;

        if (phoneVerificationCode.Length < 4)
        {
            Tips.Create("LOACAL_ERROR_LOGIN_WECHAT_QR_CODE_ENTER");
            return;
        }

        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("phone", phone);
        data.Add("code", phoneVerificationCode);
      //  data.Add("uid", PlayerData.UID);
        data.Add("token", PlayerData.Token);

        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_Host + HttpRequestConfig.BindingPhone, data, (string result) =>
        {
            C_DebugHelper.Log("RequestPhoneBinding result = " + result);

            try
            {
                if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
                {
                    UpdateToken(null);

                    PlayerData.Phone = phone;
                    PlayerData.Save();
                    
                    C_EventHandler.SendEvent(C_EnumEventChannel.Global, "PhoneBinding", true);
                    C_EventHandler.SendEvent(C_EnumEventChannel.Global, "PlayerDataChange");

                    return;
                }
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("RequestPhoneBinding Exception e = " + e);
            }

            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "PhoneBinding", false);
        });
    }


    //-------------------------WeChat Login-------------------------
    public static void RequestWeChatLogin(string code)
    {
        if (string.IsNullOrEmpty(code))
            return;

        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("code", code);

        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_Host + HttpRequestConfig.LoginWeChat, data, (string result) =>
        {
            //ResponseLoginResult(result);
            C_DebugHelper.Log("RequestWeChatLogin result = " + result);

            if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
            {
                PlayerData.Token = C_Json.GetJsonKeyString(result, "token");
                RequestTokenLogin();
            }

        });
    }

    //-------------------------WeChat QR Code Login-------------------------
    public static void RequestWeChaQRCodetLogin(string code)
    {
        if (string.IsNullOrEmpty(code))
            return;

        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("code", code);

        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_Host + HttpRequestConfig.LoginWeChatQRCode, data, (string result) =>
        {
            C_DebugHelper.Log("RequestWeChaQRCodetLogin result = " + result);

            if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
            {
                PlayerData.Token = C_Json.GetJsonKeyString(result, "token");
                RequestTokenLogin();
            }
        });
    }

    //-------------------------WeChat Binding-------------------------
    public static void RequestWeChatBinding(string code)
    {
        if (string.IsNullOrEmpty(code))
            return;

        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("code", code);
       // data.Add("uid", PlayerData.UID);
        data.Add("token", PlayerData.Token);

        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_Host + HttpRequestConfig.BindingWeChat, data, (string result) =>
        {
            C_DebugHelper.Log("RequestWeChatBinding result = " + result);

            try
            {
                if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
                {
                    JsonData uwinfo = C_Json.GetJsonKeyJsonData(result, "uwinfo");
                    if (uwinfo != null)
                    {
                        PlayerData.WeChatUnionID = C_Json.GetJsonKeyString(uwinfo, "nickname");
                        PlayerData.Token = C_Json.GetJsonKeyString(result, "token");

                       //  UpdateToken(null);
                        PlayerData.Save();
                        
                        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "WeChatBinding", true);
                        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "PlayerDataChange");

                        return;
                    }
                }
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("RequestWeChatBinding Exception e = " + e);
            }

            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "WeChatBinding", false);
        });
    }

    //-------------------------WeChat QR Code Binding-------------------------
    public static void RequestWeChatQRCodeBinding(string code)
    {
        if (string.IsNullOrEmpty(code))
            return;

        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("code", code);
        //data.Add("uid", PlayerData.UID);
        data.Add("token", PlayerData.Token);

        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_Host + HttpRequestConfig.BindingWeChatQRCode, data, (string result) =>
        {
            C_DebugHelper.Log("RequestWeChatQRCodeBinding result = " + result);

            try
            {
                if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
                {
                    JsonData uwinfo = C_Json.GetJsonKeyJsonData(result, "uwinfo");
                    if (uwinfo != null)
                    {
                        PlayerData.WeChatUnionID = C_Json.GetJsonKeyString(uwinfo, "nickname");
                        PlayerData.Token = C_Json.GetJsonKeyString(result, "token");
                        //UpdateToken(null);
                        PlayerData.Save();

                        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "WeChatBinding", true);
                        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "PlayerDataChange");

                        return;
                    }
                }
            }
            catch (Exception e)
            {
                C_DebugHelper.LogError("RequestWeChatQRCodeBinding Exception e = " + e);
            }

            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "WeChatBinding", false);
        });
    }
    public static void ReuquestTokenVerify(System.Action<bool> callback)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("token", PlayerData.Token);

        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_Host + HttpRequestConfig.TokenVerify, data, (string result) =>
        {
            int flag = FetchTokenVerifyResult(result);
            if (flag == 1)
            {
                callback(true);
            }
            else if (flag == 2)
            {
                UpdateToken(()=> {
                    callback(true);
                });
            }
            else
            {
                callback(false);
            }
        });
    }
    public static void UpdateToken(System.Action callback)
    {
        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_Host + HttpRequestConfig.LoginVisitor, new Dictionary<string, string>(), (string result) =>
        {
            C_DebugHelper.Log("UpdateToken result = " + result);
            try
            {
                if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
                {
                    PlayerData.Token = C_Json.GetJsonKeyString(result, "token");
                    PlayerData.Save();
                     
                }
                if (callback != null)
                {
                    callback();
                }
            }
            catch (Exception e)
            {
                if (callback != null)
                {
                    callback();
                }
            }
        });
    }
    //-------------------------Token Login-------------------------
    //进入游戏的时候会调用Token,UID 和 UDID登录 接口
    public static void RequestTokenLogin()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("token", PlayerData.Token);

        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_Host + HttpRequestConfig.LoginToken, data, (string result) =>
        {
            ResponseLoginResult(result);
        });
    }
    //废弃
    //-------------------------UID Login-----------------------
    public static void RequestUIDLogin()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("uid", PlayerData.UID);

        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_Host + HttpRequestConfig.LoginUID, data, (string result) =>
        {
            ResponseLoginResult(result);
        });
    }

    //-------------------------UDID Login-------------------------
    public static void RequestUDIDLogin()
    {
        C_Singleton<NetworkMgr>.GetInstance().SendHttpPost(GameDataMgr.c_Host + HttpRequestConfig.LoginVisitor, new Dictionary<string, string>(), (string result) =>
        {
            GetVisitorToken(result);
        });
    }
    public static  void GetVisitorToken(string result)
    {
        bool fail = false;
        C_DebugHelper.Log("GetVisitorToken result = " + result);
        try
        {
            if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
            {
                PlayerData.Token = C_Json.GetJsonKeyString(result, "token");
                //申请用户信息
                GameLoginMgr.RequestTokenLogin();
            }
            else
            {
                fail = true;
            }
        }
        catch(Exception e)
        {
            fail = true;
        }
        if (fail)
        {
            Tips.Create("登录失败");
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "LoginComplete", 0);
        }
    }
    private static void ResponseLoginResult(string result)
    {
        C_DebugHelper.Log("ResponseLoginResult result = " + result);

        try
        {
            if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
            {
            //    Tips.Create("LOACAL_LOGIN_SUCCEED");

                JsonData uinfo = C_Json.GetJsonKeyJsonData(result, "uinfo");
                if (uinfo != null)
                {
                   // PlayerData.Token = C_Json.GetJsonKeyString(uinfo, "token");
                    PlayerData.UID = C_Json.GetJsonKeyString(uinfo, "uid");
                    PlayerData.BabyGender = C_Json.GetJsonKeyString(uinfo, "babygender");
                    PlayerData.BabyName = C_Json.GetJsonKeyString(uinfo, "babyname");
                    PlayerData.BabyNameMP3 = C_Json.GetJsonKeyString(uinfo, "namemp3");

                    string strBabyBirthday = C_Json.GetJsonKeyString(uinfo, "birthday");
                    if (strBabyBirthday != "1970-01-01")
                        PlayerData.BabyBirthday = strBabyBirthday;
                    else
                        PlayerData.BabyBirthday = "2013-06-01";

                    PlayerData.HeadImg = C_Json.GetJsonKeyString(uinfo, "headimg");
                    PlayerData.Phone = C_Json.GetJsonKeyString(uinfo, "phone");

                    //服务器默认为空，这时要使用客户端的默认值
                    string tempString = C_Json.GetJsonKeyString(uinfo, "restspan");
                    if (!string.IsNullOrEmpty(tempString))
                        PlayerData.RestSpan = int.Parse(tempString);

                    tempString = C_Json.GetJsonKeyString(uinfo, "resttime");
                    if (!string.IsNullOrEmpty(tempString))
                        PlayerData.RestTime = int.Parse(tempString);

                    tempString = C_Json.GetJsonKeyString(uinfo, "weekup");
                    if (!string.IsNullOrEmpty(tempString))
                        PlayerData.WakeUpTime = tempString;

                    tempString = C_Json.GetJsonKeyString(uinfo, "sleeptime");
                    if (!string.IsNullOrEmpty(tempString))
                        PlayerData.SleepTime = tempString;
                }

                JsonData uwinfo = C_Json.GetJsonKeyJsonData(result, "uwinfo");
                if (uwinfo != null)
                    PlayerData.WeChatUnionID = C_Json.GetJsonKeyString(uwinfo, "nickname");

                PlayerData.Save();

                C_Singleton<GameDataMgr>.GetInstance().RefreshPlayerData();

                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "LoginComplete", 1);
                return;
            }
            else
            {
                Tips.Create("LOACAL_LOGIN_DEFEATED");
            }
        }
        catch (Exception e)
        {
            C_DebugHelper.LogError("ResponseLoginResult : " + e);
        }

        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "LoginComplete", 0);
    }

    public static bool CheckoutPhone(string phone)
    {
        if (phone.Length != 11 || !phone.StartsWith("1", StringComparison.OrdinalIgnoreCase))
        {
            Tips.Create("LOACAL_ERROR_LOGIN_PHONE_ENTER");
            return false;
        }

        return true;
    }

    public static int FetchTokenVerifyResult(string result)
    {
        C_DebugHelper.Log("FetchTokenVerifyResult result = " + result);

        try
        {

            if (ServerHelper.AssessReturnCode(C_Json.GetJsonKeyInt(result, "rc")))
            {
                JsonData verify = C_Json.GetJsonKeyJsonData(result, "verify");
                if (verify ==null)
                {
                    C_DebugHelper.LogError("FetchTokenVerifyResult verify is null");
                }
                int valid = C_Json.GetJsonKeyInt(verify,"valid");
                C_DebugHelper.Log("收到Token 验证成功 ：valid：" + valid);
                if (valid != 1)
                {
                    return 2;
                }
                return 1;
            }
        }
        catch (Exception e)
        {

        }
        return 0;
    }
}
