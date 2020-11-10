using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterActionItem
{
    public string Name = "";
    public int Type = 0;
    public float DurationTime = 0;
}

public class CharacterAction
{
    public string Name = "";
    public int Type = 0;
    public int EndType = 0;
    public float DurationTime = 0;
    public List<CharacterActionItem> AnimatorController = new List<CharacterActionItem>();
    public List<CharacterActionItem> Audio = new List<CharacterActionItem>();
}

public class Character : MonoBehaviour
{
    public string Name = "";
    [SerializeField]
    private Animator m_Animator = null;
    [SerializeField]
    private AudioSource m_AudioSource = null;

    private List<CharacterAction> m_CharacterAction = new List<CharacterAction>();
    public bool Running = false;

    private CharacterAction m_CurCharacterAction = null;

    private List<CharacterActionItem> m_CurPlayAudioItemList = null;
    private float m_fAudioTime = 0;

    void Awake()
    {
        string actionConfig = C_Singleton<GameResMgr>.GetInstance().LoadString("action_config", "entity", "character", "c_framework/entity/character/" + Name + "/config/");
        if (!string.IsNullOrEmpty(actionConfig))
        {
            JsonData actionJD = C_Json.GetJsonKeyJsonData(actionConfig, "Acton");
            if (actionJD != null)
            {
                for (int i = 0; i < actionJD.Count; i++)
                {
                    CharacterAction action = new CharacterAction();
                    action.Name = C_Json.GetJsonKeyString(actionJD[i], "Name");
                    action.Type = C_Json.GetJsonKeyInt(actionJD[i], "Type");
                    action.EndType = C_Json.GetJsonKeyInt(actionJD[i], "EndType");
                    action.DurationTime = C_Json.GetJsonKeyFloat(actionJD[i], "DurationTime");

                    JsonData animatorControllerJD = C_Json.GetJsonKeyJsonData(actionJD[i], "AnimatorController");
                    if (animatorControllerJD != null)
                    {
                        for (int j = 0; j < animatorControllerJD.Count; j++)
                        {
                            CharacterActionItem item = new CharacterActionItem();
                            item.Name = C_Json.GetJsonKeyString(animatorControllerJD[j], "Name");
                            item.Type = C_Json.GetJsonKeyInt(animatorControllerJD[j], "Type");
                            item.DurationTime = C_Json.GetJsonKeyFloat(animatorControllerJD[j], "DurationTime");
                            action.AnimatorController.Add(item);
                        }
                    }

                    JsonData audioJD = C_Json.GetJsonKeyJsonData(actionJD[i], "Audio");
                    if (audioJD != null)
                    {
                        for (int j = 0; j < audioJD.Count; j++)
                        {
                            CharacterActionItem item = new CharacterActionItem();
                            item.Name = C_Json.GetJsonKeyString(audioJD[j], "Name");
                            item.Type = C_Json.GetJsonKeyInt(audioJD[j], "Type");
                            item.DurationTime = C_Json.GetJsonKeyFloat(audioJD[j], "DurationTime");
                            action.Audio.Add(item);
                        }
                    }

                    m_CharacterAction.Add(action);
                }
            }
        }
    }

    void Update()
    {
        if (m_fAudioTime > 0)
            m_fAudioTime -= Time.deltaTime;

        if (m_fAudioTime <= 0)
        {
            if (m_CurPlayAudioItemList != null && m_CurPlayAudioItemList.Count > 0)
            {
                m_fAudioTime = m_CurPlayAudioItemList[0].DurationTime;

                PlayAudio(m_CurPlayAudioItemList[0].Name);

                m_CurPlayAudioItemList.RemoveAt(0);
            }
        }

        if (m_CurCharacterAction != null)
        {
            if (m_CurCharacterAction.EndType == 1)
            {
                AnimatorStateInfo info = m_Animator.GetCurrentAnimatorStateInfo(0);
                if (info.normalizedTime >= 1.0f)
                {
                    Running = false;
                    m_CurCharacterAction = null;
                }
            }
            else if (m_CurCharacterAction.EndType == 2 && m_fAudioTime <= 0)
            {
                Running = false;
                m_CurCharacterAction = null;
            }
        }
    }

    public void Play(int actionType)
    {
        List<CharacterAction> list = new List<CharacterAction>();
        for (int i = 0; i < m_CharacterAction.Count; i++)
        {
            if (m_CharacterAction[i].Type == actionType)
                list.Add(m_CharacterAction[i]);
        }
            
        if (list.Count > 0)
            Play(list[Random.Range(0, list.Count)]);
    }

    public void Play(string actionName)
    {
        for (int i = 0; i < m_CharacterAction.Count; i++)
        {
            if (m_CharacterAction[i].Name == actionName)
                Play(m_CharacterAction[i]);
        }
    }

    public void Play(CharacterAction action)
    {
        //Idle 状态不属于Runing状态
        if (action.Type != 1)
            Running = true;
        else
            Running = false;

        m_CurCharacterAction = action;

        PlayAnimator(action.AnimatorController);
        m_CurPlayAudioItemList = C_CommonAlgorithm.Clone<CharacterActionItem>(action.Audio);
    }

    private void PlayAnimator(List<CharacterActionItem> itemList)
    {
        RuntimeAnimatorController animatorController = C_Singleton<GameResMgr>.GetInstance().LoadResource<RuntimeAnimatorController>(itemList[0].Name, "entity", "character", "c_framework/entity/character/" + Name + "/animator_controller/");

        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animatorController);

        m_Animator.runtimeAnimatorController = animatorOverrideController;
    }

    public void PlayAudio(string name)
    {
        if (name == "babyname")
            m_AudioSource.clip = BabyName.c_BabyNameAudioClip;
        else
            m_AudioSource.clip = C_Singleton<GameResMgr>.GetInstance().LoadResource_Audio_XBL(name);

        m_AudioSource.Play();
    }

    private string m_PlayAudioBabyNameBefore_Name = "";
    public void PlayAudioBabyNameBefore(string name)
    {
        m_PlayAudioBabyNameBefore_Name = name;

        m_AudioSource.clip = BabyName.c_BabyNameAudioClip;
        m_AudioSource.Play();

        Invoke("PlayAudioBabyNameBefore1", 1.5f);
    }

    private void PlayAudioBabyNameBefore1()
    {
        m_AudioSource.clip = C_Singleton<GameResMgr>.GetInstance().LoadResource_Audio_XBL(m_PlayAudioBabyNameBefore_Name);
        m_AudioSource.Play();
    }
}
