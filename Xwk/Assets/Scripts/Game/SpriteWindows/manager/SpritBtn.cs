using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YB.XWK.MainScene;
using DG.Tweening;
using Assets.Scripts.C_Framework;
using XWK.Common.UI_Reward;
/// <summary>
/// 右下角按钮管理类
/// </summary>
public class SpritBtn : MonoBehaviour {
    public static SpritBtn Instance;

    public List<GameObject> StarNormalList;
    public List<GameObject> StarHoverList;
    public Button gameUibutton;
    public Button storyUibutton;
    public Button returnUibutton;
    public Button upgradeUibutton;

    public GameObject CornerMark_Game;
    public GameObject CornerMark_Story;


    RectTransform storyUi;
    RectTransform gameUi;
    public RectTransform ArrowImage;
    Vector2 arrowStPos = new Vector2(282, -51f);
    Vector2 arrowEdPos = new Vector2(300, -51f);
    Vector4 stColor = new Vector4(1, 1, 1, 0.5f);
    Vector3 gamePos;
    Vector3 storyPos;
    Image gameImage;
    Image storyImage;
    string basePath = "game/SpriteWindow/ui/role/";
    bool InitData = false;
    SpriteData data=null;
    // Use this for initialization
    void Awake()
    {
        Instance = this;
      
    }
    void Update()
    {
       
    }
    void Init () {
        gameUibutton.onClick.AddListener(GameOnclickEvent);
        storyUibutton.onClick.AddListener(StoryOnclickEvent);
        returnUibutton.onClick.AddListener(RetrunMainScene);
        
        gameImage = gameUibutton.GetComponent<Image>();
        storyImage = storyUibutton.GetComponent<Image>();
        gameUi = gameUibutton.GetComponent<RectTransform>();
        storyUi = storyUibutton.GetComponent<RectTransform>();
        gamePos = gameUi.anchoredPosition;
        storyPos = storyUi.anchoredPosition;
        gameImage = gameUibutton.GetComponent<Image>();
        storyImage = storyUibutton.GetComponent<Image>();

        CornerMark_Story.SetActive(false);
        CornerMark_Game.SetActive(false);
    }

    public SpriteData GetSpriteData()
    {
        return data;
    }
    void RefreshCornerMark(SpriteData spData)//刷新角标
    {
        if (spData.lockIcon)
        {
            if (spData.Name.Equals(WizardItemName.Wizard_BaiYin))
            {
                if (spData.spLvel >= 1)
                {
                    if (DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Game_byjl))//没有获取奖励
                    {
                        CornerMark_Game.SetActive(true);
                    }
                    else
                    {
                        CornerMark_Game.SetActive(false);
                    }
                }
                else
                {
                    CornerMark_Game.SetActive(false);
                }

                if (DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Story_Byjl))//没有获取奖励
                {
                    CornerMark_Story.SetActive(true);
                }
                else
                {
                    CornerMark_Story.SetActive(false);
                }
                
            }
            else if (spData.Name.Equals(WizardItemName.Wizard_Hulu))
            {
                if (DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Story_Hulu))//故事没有获取奖励
                {
                    CornerMark_Story.SetActive(true);
                }
                else
                {
                    CornerMark_Story.SetActive(false);
                }
                if (spData.spLvel >= 1)
                {                   
                    if (DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Game_bbhl))//游戏没有获取奖励
                    {
                        CornerMark_Game.SetActive(true);
                    }
                    else
                    {
                        CornerMark_Game.SetActive(false);
                    }
                }
                else
                {
                    CornerMark_Game.SetActive(false);
                }

            }
            else if (spData.Name.Equals(WizardItemName.Wizard_Xln))
            {                
                if (DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Story_Ln))//没有获取奖励
                {
                    CornerMark_Story.SetActive(true);
                }
                else
                {
                    CornerMark_Story.SetActive(false);
                }
            }
        }
        else
        {
            CornerMark_Story.SetActive(false);
            CornerMark_Game.SetActive(false);
        }
    }
    public void RefreshUI(SpriteData spData)
    {
        if (spData == null)
            return;
        if (!InitData)
        {
            InitData = true;
            Init();
        }
        data = spData;
        storyUi.anchoredPosition = storyPos;
        gameUi.anchoredPosition = gamePos;
        SetGameUI(spData);
        SetStoryUI(spData);
        SetImageEffect();
        SetStarMode(true);
        RefreshCornerMark(spData);
        RefreshButtonInteractable();
    }
    public void RefreshButtonInteractable()
    {
        if(DirectorMgr.DirectorAllState)
        {
            gameUibutton.interactable = true;
            storyUibutton.interactable = true;
            returnUibutton.interactable = true;
            upgradeUibutton.interactable = true;
        }
        else
        {
            gameUibutton.interactable = false;
            storyUibutton.interactable = false;
            returnUibutton.interactable =false;
            upgradeUibutton.interactable = false;
        }
    }
    public void CloseButtonInteractable()
    {
        gameUibutton.interactable = false;
        storyUibutton.interactable = false;
        returnUibutton.interactable = false;
        upgradeUibutton.interactable = false;
    }
    //外部动态刷新，升级之后
    public void RefreshGameUI(SpriteData data)
    {
        SetGameUI(data);
        RefreshCornerMark(data);
    }
    #region##UI表现
    void SetStoryUI(SpriteData spData)
    {
        if (spData == null)
            return;
        if(spData.Name.Equals("future")|!spData.haveStory)
        {
            gameUibutton.transform.gameObject.SetActive(false);
            storyUibutton.transform.gameObject.SetActive(false);
            //if(gameUibutton.transform.gameObject.activeInHierarchy)
            //{
            //    CommonUIMgr.Instance.CloseUI(gameUibutton.transform.gameObject);
            //}          
            //if(storyUibutton.transform.gameObject.activeInHierarchy)
            //{
            //    CommonUIMgr.Instance.CloseUI(storyUibutton.transform.gameObject);
            //}
        }
        else
        {
            //storyUibutton.transform.gameObject.SetActive(true);
            CommonUIMgr.Instance.ShowUI(storyUibutton.transform.gameObject);
            if (spData.lockIcon)
            {
                Sprite tex2 = ABResMgr.Instance.LoadResource<Sprite>("game/SpriteWindow/ui/btn_story_1", "", false, true);
                storyImage.sprite = tex2;
            }
            else
            {
                Sprite tex2 = ABResMgr.Instance.LoadResource<Sprite>("game/SpriteWindow/ui/btn_story_3", "", false, true);
                storyImage.sprite = tex2;
            }           
            SetImageSpriteState(storyUibutton,"game/SpriteWindow/ui/btn_story_2");
        }

    }
    void SetGameUI(SpriteData spData)//设置游戏图标样式（是否解锁+是否升级）
    {
        if (spData == null)
            return;

        if (spData.Name.Equals(WizardItemName.Wizard_BaiYin))
        {
            if (spData.lockIcon && spData.spLvel >= 1)
            {
                Sprite tex = ABResMgr.Instance.LoadResource<Sprite>(basePath + "byjl/btn_game_byjl_1", "", false, true);
                gameImage.sprite = tex;
                Sprite tex2 = ABResMgr.Instance.LoadResource<Sprite>("game/SpriteWindow/ui/btn_story_1", "", false, true);
                storyImage.sprite = tex2;
                //gameUibutton.transform.gameObject.SetActive(true);
                CommonUIMgr.Instance.ShowUI(gameUibutton.transform.gameObject);
            }
            else
            {
                Sprite tex = ABResMgr.Instance.LoadResource<Sprite>(basePath + "byjl/btn_game_byjl_3", "", false, true);
                gameImage.sprite = tex;

                Sprite tex2 = ABResMgr.Instance.LoadResource<Sprite>("game/SpriteWindow/ui/btn_story_3", "", false, true);
                storyImage.sprite = tex2;
                CommonUIMgr.Instance.ShowUI(gameUibutton.transform.gameObject);
            }
            SetImageSpriteState(gameUibutton, basePath+"byjl/btn_game_byjl_2");
        }
        else if (spData.Name.Equals(WizardItemName.Wizard_Hulu))
        {
            if (spData.lockIcon&&spData.spLvel>=1)
            {
                Sprite tex = ABResMgr.Instance.LoadResource<Sprite>(basePath + "hulu/btn_game_hl_1", "", false, true);//btn_game_hl_1
                gameImage.sprite = tex;
                //gameUibutton.transform.gameObject.SetActive(true);
                CommonUIMgr.Instance.ShowUI(gameUibutton.transform.gameObject);
            }
            else
            {
                Sprite tex = ABResMgr.Instance.LoadResource<Sprite>(basePath + "hulu/btn_game_hl_3", "", false, true);//btn_game_hl_1
                gameImage.sprite = tex;
                CommonUIMgr.Instance.ShowUI(gameUibutton.transform.gameObject);
            }
            SetImageSpriteState(gameUibutton, basePath+"hulu/btn_game_hl_2");
        }
        else
        {          
            gameUibutton.transform.gameObject.SetActive(false);           
            storyUi.anchoredPosition = gamePos;
        }
    }


   
    public void SetStarMode(bool isNormal)//设置星星模式，要么一颗，要么三颗
    {
        if (GetSpriteData() == null)
            return;
        if (isNormal)
        {
            for (int i = 0; i < StarHoverList.Count; i++)
                StarHoverList[i].SetActive(false);           
         
            if (GetSpriteData().starLevel == 3)
            {
                for (int i = 0; i < StarNormalList.Count; i++)
                    StarNormalList[i].SetActive(true);
            }
            else if(GetSpriteData().starLevel == 1)
            {
                for (int i = 0; i < StarNormalList.Count; i++)
                    StarNormalList[i].SetActive(false);
                StarNormalList[0].SetActive(true);
            }
            else
            {
                for (int i = 0; i < StarNormalList.Count; i++)
                    StarNormalList[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < StarNormalList.Count; i++)
                StarNormalList[i].SetActive(false);       
             
            if (GetSpriteData().starLevel == 3)
            {
                for (int i = 0; i < StarHoverList.Count; i++)
                    StarHoverList[i].SetActive(true);
            }
            else if (GetSpriteData().starLevel == 1)
            {
                for (int i = 0; i < StarHoverList.Count; i++)
                    StarHoverList[i].SetActive(false);
                StarHoverList[0].SetActive(true);
            }
            else
            {
                for (int i = 0; i < StarHoverList.Count; i++)
                    StarHoverList[i].SetActive(false);
            }
        }       
    }
    
    void SetImageSpriteState(Button button,string path)
    {
        SpriteState state = new SpriteState();
        Sprite tex = ABResMgr.Instance.LoadResource<Sprite>(path, "", false, true);//btn_game_hl_1
        state.highlightedSprite = tex;
        state.pressedSprite = tex;
        state.disabledSprite = tex;
        button.spriteState = state;
    }

    //按钮切换，渐入渐出
    void SetImageEffect()
    {
        storyImage.color = stColor;
        gameImage.color = stColor;
        storyImage.DOColor(Vector4.one, 0.5f);
        gameImage.DOColor(Vector4.one, 0.5f);
    }
    
    //滑块中间箭头
    public void OpenArrowImage(bool flag)
    {
        if (!InitData)
        {
            InitData = true;
            Init();
        }
        if (flag)
        {
            ArrowImage.gameObject.SetActive(true);
            ArrowImage.anchoredPosition = arrowStPos;
            ArrowImage.DOAnchorPos(arrowEdPos, 0.5f);
        }
        else
        {
            CloseArrowImage();
        }    
    }
    public void CloseArrowImage()
    {
        ArrowImage.gameObject.SetActive(false);
    }

    #endregion


    #region##游戏按键
    void GameOnclickEvent()
    {
        if (!DirectorMgr.DirectorAllState)
            return;
        if (SpriteIconMgr.Instance.GetSpriteData()==null)
            return;
        if (SpriteIconMgr.Instance.GetSpriteData().lockIcon&& SpriteIconMgr.Instance.GetSpriteData().spLvel>=1)//解锁 +等级大于1级
        {
            CloseButtonInteractable();
            LocalData.RoleLevel = SpriteIconMgr.Instance.GetSpriteData().nowRoleState;
            LocalData.GotoStoryOrGameBy_SpWindow = true;
            Spwindow.PlaySmallClipSound("public_xwkyx_068");
           
            string iconName = SpriteIconMgr.Instance.GetSpriteData().Name;
            if (iconName.Equals(WizardItemName.Wizard_Hulu))
            {
                if (DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Game_bbhl))//没有获取奖励
                {
                    RewardUIManager.Instance.RegisterSpriteWindow(5, GotoGameHulu);
                    RewardUIManager.GetInstance().SetSuccess();
                    DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Game_bbhl);
                    AddDailyBounsReport(DailyBounsName.DailyBouns_Game_bbhl);
                }
                else
                {
                    GotoGameHulu(true);
                }
            }
            else if (iconName.Equals(WizardItemName.Wizard_BaiYin))
            {
                if (DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Game_byjl))//没有获取奖励
                {
                    RewardUIManager.Instance.RegisterSpriteWindow(5, GotoGameByjl);
                    RewardUIManager.GetInstance().SetSuccess();
                    DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Game_byjl);
                    AddDailyBounsReport(DailyBounsName.DailyBouns_Game_byjl);
                }
                else
                {
                    GotoGameByjl(true);
                }
            }         
        }
        else
        {
            Spwindow.PlayCharacterAudio("xwk_jlej_11");
        }
    }
    void GotoGameByjl(bool flag)
    {
        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 7);
        C_UIMgr.Instance.CloseUI("UI_SpriteWindow");
        LocalData.m_SpiritGameMode = true;
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.byjlgameUIclick, "ClickbyjlGame");
        YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "Spirit", () => { Utility.SetMainScene("Spirit"); });
    }
    void GotoGameHulu(bool flag)
    {
        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 7);
        C_UIMgr.Instance.CloseUI("UI_SpriteWindow");
        LocalData.m_SpiritGameMode = true;
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.hulugameUIclick, "ClickbbhlGame");
        YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "BaibianHulu", () => { Utility.SetMainScene("BaibianHulu"); });
    }




    #endregion


    #region##故事按钮
    public void GotoStoryByDir()//通过新手引导去故事环节
    {
        //判断是否有每日奖励
        if (DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Game_byjl))//每日获取奖励
        {
            DirectorMgr.Instance.CloseAllDirecStep();
            RewardUIManager.Instance.RegisterSpriteWindow(5, NewUserGotoByjlStory);
            RewardUIManager.GetInstance().SetSuccess();
            DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Story_Byjl);
        }
        else
        {
            NewUserGotoByjlStory(true);
        }
    }
    void NewUserGotoByjlStory(bool flag)
    {
        LocalData.RoleLevel = 0;//新用户默认是0
        LocalData.GotoStoryOrGameBy_SpWindow = true;
        Spwindow.PlaySmallClipSound("public_xwkyx_068");
        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 7);
        C_UIMgr.Instance.CloseUI("UI_SpriteWindow");
        YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "byjl_story", () => { Utility.SetMainScene("wk_scene_01"); });
    }
    void StoryOnclickEvent()
    {
        if (!DirectorMgr.DirectorAllState)
            return;
        if (SpriteIconMgr.Instance.GetSpriteData() == null)
            return;
        if (SpriteIconMgr.Instance.GetSpriteData().lockIcon)
        {
            CloseButtonInteractable();
            LocalData.RoleLevel = SpriteIconMgr.Instance.GetSpriteData().nowRoleState;
            LocalData.GotoStoryOrGameBy_SpWindow = true;
            Spwindow.PlaySmallClipSound("public_xwkyx_068");
          
            string iconName = SpriteIconMgr.Instance.GetSpriteData().Name;

            if (iconName.Equals(WizardItemName.Wizard_Hulu))
            {
                if (DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Story_Hulu))//每日获取奖励
                {
                    RewardUIManager.Instance.RegisterSpriteWindow(5, GotoHuluStory, "huluGame");
                    RewardUIManager.GetInstance().SetSuccess();
                    DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Story_Hulu);
                    AddDailyBounsReport(DailyBounsName.DailyBouns_Story_Hulu);
                }
                else
                {
                    GotoHuluStory(true);
                }                
            }
            else if (iconName.Equals(WizardItemName.Wizard_BaiYin))
            {
                if (DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Story_Byjl))//每日获取奖励
                {
                    RewardUIManager.Instance.RegisterSpriteWindow(5, GotoByjlStory,"byjiGame");
                    RewardUIManager.GetInstance().SetSuccess();
                    DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Story_Byjl);
                    AddDailyBounsReport(DailyBounsName.DailyBouns_Story_Byjl);
                }
                else
                {
                    GotoByjlStory(true);
                }                           
            }
            else if (iconName.Equals(WizardItemName.Wizard_Xln))
            {
                if (DailyBounsData.IsUnLock(DailyBounsName.DailyBouns_Story_Ln))//每日获取奖励
                {
                    RewardUIManager.Instance.RegisterSpriteWindow(5, GotoXlnStory);
                    RewardUIManager.GetInstance().SetSuccess();
                    DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Story_Ln);
                    AddDailyBounsReport(DailyBounsName.DailyBouns_Story_Ln);
                }
                else
                {
                    GotoXlnStory(true);
                }
            }
        }
        else
        {
            //点击为解锁、语音提示
            Spwindow.PlayCharacterAudio("xwk_jlej_7");
        }
    }
    void AddDailyBounsReport(string name)
    {
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_dailybouns, "SpWindowDailybouns:"+name);
    }

    void GotoByjlStory(bool flag)
    {
        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 7);
        C_UIMgr.Instance.CloseUI("UI_SpriteWindow");
        if (DirectorMgr.DirectorAllState)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick,LocalData.byjlstoryUIclick, "ClickbyjlStory");
        }
        else
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "ClickbyjlStory_NewUser", "ClickbyjlStory_NewUser");
        }
        LocalData.m_SpiritType = WizardItemName.Wizard_BaiYin;
        YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "byjl_story", () => { Utility.SetMainScene("wk_scene_01"); });
    }
    void GotoHuluStory(bool flag)
    {
        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 7);
        C_UIMgr.Instance.CloseUI("UI_SpriteWindow");
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick,LocalData.hulustoryUIclick, "ClickbbhlStory");
        LocalData.m_SpiritType = WizardItemName.Wizard_Hulu;
        YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "bbhl_story", () => { Utility.SetMainScene("wk_scene_01"); });
    }
    void GotoXlnStory(bool flag)
    {
        C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 7);
        C_UIMgr.Instance.CloseUI("UI_SpriteWindow");
        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick,LocalData.xlnstoryUIclick, "ClickxlnStory");
        LocalData.m_SpiritType = WizardItemName.Wizard_Xln;
        YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_02", "xzlz_story", () => { Utility.SetMainScene("wk_scene_02"); });
    }

    #endregion

    void RetrunMainScene()
    {       
        if (!SpriteIconMgr.Instance.IsMoving())
        {
            C_UIMgr.Instance.CloseUI("UI_SpriteWindow");
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 1);
        }
    }   
}
