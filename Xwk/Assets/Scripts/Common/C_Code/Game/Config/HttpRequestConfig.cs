using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class HttpRequestConfig
{
    public static string Name = "http_request_config";

    //正式热更新地址
    public static string FormalHotUpdate = "https://xblpinyinsrc.youban.com/";
    //测试热更新地址
    public static string TestHotUpdate = "https://testpinyinsrc.youban.com/";
    
    ////正式环境
    //public static string FormalHost = "https://xbluser.youban.com/";
    ////测试环境
    //public static string TestHost = "https://testxbluser.youban.com/";

    //小悟空正式环境--登录
    public static string FormalHost = "https://xblacct.youban.com/";
    //小悟空测试环境--登录
    public static string TestHost = "https://testxblacct.youban.com/";
    //临时登录
    //public static string TestHost = "http://angela.xblacct.youban.com/";

    //小悟空测试环境--数据请求
    public static string TestDataHost = "https://testxblgoku.youban.com/";
    //小悟空正式环境--数据请求
    public static string FormalDataHost = "https://xblgoku.youban.com/";

    //正式环境--渠道信息等配置文件的域名
    public static string FormalPinYinHost = "https://xblgoku.youban.com/";
    //测试环境
    public static string TestPinYinHost = "https://testxblgoku.youban.com/";

    //正式环境--vip信息
    public static string FormalPayHost = "https://xblpay.youban.com/";
    //测试环境
    public static string TestPayHost = "https://testxblpay.youban.com/";

    //小悟空正式环境--一个公共的域名，但是没有使用
    public static string FormalCommonHost = "https://xblacct.youban.com"; 
    //小悟空测试环境
  // public static string TestCommonHost = "https://testxblacct.youban.com"; 
   public static string TestCommonHost = "https://angle.xblacct.youban.com"; 
    #region pinyin
    //正式环境
   // public static string FormalCommonHost =  "https://xblcommon.youban.com/";
    //测试环境
  //  public static string TestCommonHost = "https://testxblcommon.youban.com/";
    #endregion
    //小名网址
    public static string BabyNameMP3Url = "https://media.youban.com/gsmp3/bnamemp3/";

    //手机验证码登录
    public static string SMSLoginCode = "sms/logincode";
    //手机验证码登录
    public static string LoginPhoneVerificationCode = "/session/login/phone";
    //微信登录
    public static string LoginWeChat = "session/login/weixin";
    //微信扫码登录
    public static string LoginWeChatQRCode = "session/login/qrchange";
    //UID登录
    public static string LoginUID = "session/login/uid";
    //手机密码登录
    public static string LoginVisitor = "session/login/visitor";
    //注销游客
    public static string LogoutVisitor = "session/logon/uid";
    //绑定手机
    public static string BindingPhone = "account/binphone";
    //绑定微信
    public static string BindingWeChat = "account/binweixin";
    //扫码绑定微信
    public static string BindingWeChatQRCode = "account/binqrweixin";
    //Token获取用户信息
    public static string LoginToken = "user/getuserinfo";
    //Token验证
    public static string TokenVerify = "session/login/token";

    //设置用户头像
    public static string SetHeadImg = "user/setheadimg";

    //设置宝贝信息
    public static string SetBabyInfo = "user/setbabyinfo";

    //获取名字音频信息
    public static string GetNameVideo = "user/getnamemp3";
    //获取精灵数据
    public static string GetUelfin = "user/getuelfin";
    //设置精灵数据
    public static string SetUelfin = "user/setuelfin";
    //设置用户初始信息
    public static string SetUinitial = "user/setuinitial";
    //获取用户初始信息
    public static string GetUinitial = "user/getuinitial";

    //设置用户灵气值
    public static string SetUnimbus = "user/setunimbus";
    //获取用户灵气值
    public static string GetUnimbus = "user/getunimbus";
    //设置用户每日奖励
    public static string SetUreward = "user/setureward";
    //获取用户每日奖励
    public static string GetUreward = "user/getureward";

    //设置用户宝箱数据
    public static string Setureward = "user/setutreasure";
    //设置用户初始信息
    public static string Getureward = "user/getutreasure";
    //------------旧数据

    //获取星星数
    public static string GetUserStar = "user/getuserstars";
    //获取关卡配置信息
    public static string GetStageConfig = "user/getbagurl";
    //获取关卡数据
    public static string GetStageData = "user/getuserbaginfo";
    //设置关卡数据
    public static string SetStageData = "user/setuserbaginfo"; 
    //获取图鉴数据
    public static string GetFieldGuideData = "user/getuserchartinfo";
    //设置图鉴数据
    public static string SetFieldGuideData = "user/setuserchartinfo"; 
    //获取会员信息
    public static string GetVIPInfo = "user/member";
     
    //获取渠道配置
    public static string GetChannelConfig = "config/getchconfig";
    //获取商城配置
    public static string GetStoreConfig = "config/getstoreconfig";


    public static void Load()
    {
        string strData = C_Save.LoadString(Name, C_LocalPath.StreamingAssetsConfigPath);
        if (!string.IsNullOrEmpty(strData))
        {
            FormalHotUpdate = C_Json.GetJsonKeyString(strData, "FormalHotUpdate");
            TestHotUpdate = C_Json.GetJsonKeyString(strData, "TestHotUpdate");

            FormalHost = C_Json.GetJsonKeyString(strData, "FormalHost");
            TestHost = C_Json.GetJsonKeyString(strData, "TestHost");

            FormalPinYinHost = C_Json.GetJsonKeyString(strData, "FormalPinYinHost");
            TestPinYinHost = C_Json.GetJsonKeyString(strData, "TestPinYinHost");

            FormalPayHost = C_Json.GetJsonKeyString(strData, "FormalPayHost");
            TestPayHost = C_Json.GetJsonKeyString(strData, "TestPayHost");

            FormalCommonHost = C_Json.GetJsonKeyString(strData, "FormalCommonHost");
            TestCommonHost = C_Json.GetJsonKeyString(strData, "TestCommonHost");


            BabyNameMP3Url = C_Json.GetJsonKeyString(strData, "BabyNameMP3Url");


            SMSLoginCode = C_Json.GetJsonKeyString(strData, "SMSLoginCode");
            LoginPhoneVerificationCode = C_Json.GetJsonKeyString(strData, "LoginPhoneVerificationCode");
            LoginWeChat = C_Json.GetJsonKeyString(strData, "LoginWeChat");
            LoginWeChatQRCode = C_Json.GetJsonKeyString(strData, "LoginWeChatQRCode");
            LoginUID = C_Json.GetJsonKeyString(strData, "LoginUID");
            LoginVisitor = C_Json.GetJsonKeyString(strData, "LoginVisitor");
            LogoutVisitor = C_Json.GetJsonKeyString(strData, "LogoutVisitor");
            BindingPhone = C_Json.GetJsonKeyString(strData, "BindingPhone");
            BindingWeChat = C_Json.GetJsonKeyString(strData, "BindingWeChat");


            SetHeadImg = C_Json.GetJsonKeyString(strData, "SetHeadImg");

            SetBabyInfo = C_Json.GetJsonKeyString(strData, "SetBabyInfo");

            GetNameVideo = C_Json.GetJsonKeyString(strData, "GetNameVideo");

            GetVIPInfo = C_Json.GetJsonKeyString(strData, "GetVIPInfo");


            GetChannelConfig = C_Json.GetJsonKeyString(strData, "GetChannelConfig");
            GetStoreConfig = C_Json.GetJsonKeyString(strData, "GetStoreConfig");
             
        }
    }

    public static void Save()
    {
       // C_Save.SaveString(Name, C_LocalPath.StreamingAssetsConfigPath, JsonMapper.ToJson(new HttpRequestConfig()), "");
        C_Save.SaveString(Name, C_LocalPath.StreamingAssetsConfigPath, JsonMapper.ToJson(new HttpRequestConfig()), "");
    }
}

