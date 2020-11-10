using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ParentsCenterVerification : C_BaseUI
{
    [SerializeField]
    private Image[] m_Image_WordVector = null;
    [SerializeField]
    private GameObject[] m_GO_NumberVector = null;
    [SerializeField]
    private Sprite[] m_Sprite_NumberVector = null;

    private List<int> m_AnswerIndexList = new List<int>();

    private List<int> m_SelectIndexList = new List<int>();

    private int m_VerificationType = 0;

    protected override void onOpenUI(params object[] uiObjParams)
    {
        RefreshVerification();
        if (uiObjParams.Length > 0)
        {
            m_VerificationType = (int)uiObjParams[0];
        }
        else
        {
            m_VerificationType = 0;
        }
    }

    private void RefreshVerification()
    {
        m_SelectIndexList.Clear();

        m_AnswerIndexList.Clear();

        List<int> nums = new List<int>();
        for (int i = 0; i < 9; i++)
            nums.Add(i);

        int index = Random.Range(0, nums.Count);
        m_AnswerIndexList.Add(nums[index]);
        nums.RemoveAt(index);

        index = Random.Range(0, nums.Count);
        m_AnswerIndexList.Add(nums[index]);
        nums.RemoveAt(index);

        index = Random.Range(0, nums.Count);
        m_AnswerIndexList.Add(nums[index]);

        for (int i = 0; i < m_Image_WordVector.Length; i++)
            m_Image_WordVector[i].sprite = m_Sprite_NumberVector[m_AnswerIndexList[i]];

        for (int i = 0; i < m_GO_NumberVector.Length; i++)
            m_GO_NumberVector[i].SetActive(false);
    }
    
    public void OnButtonPressed(int index)
    {
        if (m_GO_NumberVector[index].activeSelf)
            return;

      //  AudioMgr.PlaySoundEffect("public_sd_042");
        AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_068");

        m_GO_NumberVector[index].SetActive(true);

        m_SelectIndexList.Add(index);

        if (m_SelectIndexList.Count >= 3)
        {
            if (m_SelectIndexList[0] == m_AnswerIndexList[0] && m_SelectIndexList[1] == m_AnswerIndexList[1] && m_SelectIndexList[2] == m_AnswerIndexList[2])
            {
                if (m_VerificationType == 0)
                {
                    C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_ParentsCenter");
                    CloseUI();
                }
                else if(m_VerificationType == 1)
                {
                    //  C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_ParentsCenter");
                    if (string.IsNullOrEmpty(PlayerData.HeadImg))
                    {
                        //后续改到配置文本
                        PlayerData.HeadImg = "http://static.yds.youban.com/20190912/5d79ea1130e41.png";
                    }
                    if (string.IsNullOrEmpty(PlayerData.BabyName))
                    {
                        PlayerData.BabyName = C_Localization.GetLocalization("LOACAL_DEFAULT_BABY_NAME");
                    }
                    C_DebugHelper.Log("unity fankui sdk:PlayerData.BabyName" + PlayerData.BabyName+ "---PlayerData.UID:"+ PlayerData.UID+ "-- PlayerData.HeadImg:" + PlayerData.HeadImg);
                    RefreshVerification();
                    GameHelper.Instance.SDK_SendOpenComment(PlayerData.UID, PlayerData.BabyName, PlayerData.HeadImg);
                    PressClose();
                }

            }
            else
            {
                RefreshVerification();
            }
        }
    }

    public void Close()
    {

        //C_EventHandler.SendEvent(C_EnumEventChannel.Global, "ActorResume");

     //   AudioMgr.PlaySoundEffect("public_sd_042");

        CloseUI();
    }
    public void PressClose()
    {
         C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent",1);
        //AudioMgr.PlaySoundEffect("public_sd_042");
        CloseUI();
    }
}
