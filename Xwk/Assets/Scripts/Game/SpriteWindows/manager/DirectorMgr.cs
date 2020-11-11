using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YB.XWK.MainScene;
using System;
/// <summary>
/// 新手引导管理类
/// </summary>
 
public class DirectorMgr : C_BaseUI
{
    public Action DirectorAllStateAction=null;//通关事件
    public static DirectorMgr Instance;
    public static bool DirectorAllState = false;
    bool FreePass = true;
    public GameObject Step1Obj;
    public GameObject Step2Obj;
    public Button Story;
    DirectorStep1 step1;
    DirectorStep2 step2;
    static List<baseDirector> DirList = new List<baseDirector>();
    static int lastId = 0;
    void Start()
    {
        AddReport();
    }
    void Update()
    {
        for (int i = 0; i < DirList.Count; i++)
            DirList[i].Update();
    }

    protected override void onOpenUI(params object[] uiObjParams)
    {
        Instance = this;
        Step1Obj.SetActive(false);
        Step2Obj.SetActive(false);
        Story.onClick.AddListener(GotoStory);
        if (Application.isEditor)
        {
            if (WizardData.IsNewUser())//新用户
            {
                NewUser();
            }
            else
            {
                StartDir();
            }
            //DirectorAllState = true;
            //DirectorAllStateAction -= SpriteIconMgr.Instance.ShowHand;
            //DirectorAllStateAction += SpriteIconMgr.Instance.ShowHand;
            //if (!DirectorAllState)
            //{
            //    if (DirectorAllStateAction != null)
            //        DirectorAllStateAction();
            //}
        }
        else
        {
            if (WizardData.IsNewUser())//新用户
            {
                NewUser();
            }
            else
            {
                StartDir();
            }
        }              
    }

    void NewUser()
    {
        DirectorAllStateAction -= SpriteIconMgr.Instance.ShowHand;
        DirectorAllStateAction += SpriteIconMgr.Instance.ShowHand;
        DirectorAllState = false;
        PiercingeyeManager.LockFinish = false;
        DirList.Clear();
        lastId = 0;
        step1 = new DirectorStep1("step1", Step1Obj);
        step2 = new DirectorStep2("step2", Step2Obj);
        DirList.Add(step1);
        DirList.Add(step2);
        ShowNextDir(0);
    }

    void StartDir()
    {
        if (lastId >= DirList.Count)//通关
        {
            DirectorAllStateAction -= SpriteIconMgr.Instance.ShowHand;
            DirectorAllStateAction += SpriteIconMgr.Instance.ShowHand;
            if (!DirectorAllState)
            {
                if (DirectorAllStateAction != null)
                    DirectorAllStateAction();
            }
            DirectorAllState = true;
            CloseAllDirecStep();
        }
        else
        {
            DirList.Clear();
            step1 = new DirectorStep1("step1", Step1Obj);
            step2 = new DirectorStep2("step2", Step2Obj);
            DirList.Add(step1);
            DirList.Add(step2);
            CloseAllDirecStep();
            ShowNextDir(lastId);
        }
    }
    
    void ShowNextDir(string name)//执行下一步新手引导
    {
        for (int i = 0; i < DirList.Count; i++)
            if (DirList[i].DirectorName.Equals(name))
                DirList[i].ShowStep();
    }
    void ShowNextDir(int ID)
    {
        DirList[ID].ShowStep();
    } 
  
    public void ClickDirector(string name)
    {
        for (int i = 0; i < DirList.Count; i++)
        {
            if (DirList[i].DirectorName.Equals(name))
                DirList[i].ClickDirector();
        }
    }
    public void DirectStepFinish()//通过新手引导 
    {
        lastId++;      
    }
    public void CloseAllDirecStep()
    {
        for (int i = 0; i < DirList.Count; i++)
                DirList[i].CloseStep();
    }


    //点击故事
    void GotoStory()
    {
        DirectStepFinish();//编辑器模拟点击了按钮
        SpritBtn.Instance.GotoStoryByDir();
        SpritBtn.Instance.CloseButtonInteractable();
    }


    void AddReport()
    {
        if (DirectorAllState)
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_spriteWindow, "EnterSpriteWindow");
        }
        else
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_spriteWindow, "EnterSpriteWindow_NewUser");
        }
    }
}
