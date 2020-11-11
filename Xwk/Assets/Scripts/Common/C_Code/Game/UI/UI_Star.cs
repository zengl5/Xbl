using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Star : C_BaseUI
{
    [SerializeField]
    private Transform m_Star_type0 = null;
    [SerializeField]
    private Transform m_Star_type1 = null;

    [SerializeField]
    private ParticleSystem m_Bao = null;

    [SerializeField]
    private AudioSource m_AudioSource = null;

    private int m_nStarCount = 0;

    private Transform m_PosTransform = null;

    private GameObject m_UI_StarItem = null;

    private Vector3 m_StarPos = Vector3.zero;
    private Action m_Callback = null;

    private List<UI_StarItem> m_UI_StarItemScriptList = new List<UI_StarItem>();

    protected override void onInit()
    {
        m_UI_StarItem = C_Singleton<GameResMgr>.GetInstance().LoadResource_UI("UI_StarItem");
        m_UI_StarItem.SetActive(false);
        m_UI_StarItem.transform.SetParent(this.transform);
    }

    protected override void onOpenUI(params object[] uiObjParams)
    {
        C_MonoSingleton<PlayerController>.GetInstance().AutoPlayEnabled = false;

        RunAction();

        C_MonoSingleton<C_AudioMgr>.GetInstance().PlayClip(m_AudioSource, C_Singleton<GameResMgr>.GetInstance().LoadResource_Audio_Effect("common_377"));
    }

    protected override void onCloseUI()
    {
        C_MonoSingleton<PlayerController>.GetInstance().AutoPlayEnabled = true;
    }
    
    public void InitStar(int starCount, Vector3 starPos, Action callback)
    {
        m_nStarCount = starCount;
        m_StarPos = starPos;
        m_Callback = callback;

        //找到当前的Transform
        if (m_nStarCount == 1)
            m_PosTransform = m_Star_type0;
        else
            m_PosTransform = m_Star_type1;

        RefreshStar();
    }

    private void RefreshStar()
    {
        m_UI_StarItemScriptList.Clear();

        for (int i = 1; i <= m_nStarCount; i++)
        {
            Transform tf = m_PosTransform.Find("StarPos_" + i);
            if (tf != null)
            {
                float scale = UnityEngine.Random.Range(0.9f, 1.0f);

                tf.localEulerAngles = new Vector3(1, 1, UnityEngine.Random.Range(-30.0f, 30.0f));
                tf.localScale = new Vector3(scale, scale, 1);

                GameObject tempObject = GameObject.Instantiate(m_UI_StarItem);
                if (tempObject != null)
                {
                    tempObject.SetActive(true);
                    tempObject.transform.SetParent(tf);

                    tempObject.transform.localPosition = Vector3.zero;
                    tempObject.transform.localScale = Vector3.zero;
                    tempObject.transform.localEulerAngles = Vector3.zero;

                    UI_StarItem ui_staritem = tempObject.GetComponent<UI_StarItem>();
                    if (ui_staritem != null)
                    {
                        m_UI_StarItemScriptList.Add(ui_staritem);

                        ui_staritem.Init(this, m_StarPos);
                    }
                }
            }
        }

        AudioMgr.PlaySoundEffect("public_sd_044");
    }

    private void RunAction()
    {
        StopCoroutine("UI_StarCoroutine");

        StartCoroutine("UI_StarCoroutine");
    }

    private IEnumerator UI_StarCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        RunBaoParticleSystem();

        yield return new WaitForSeconds(0.5f);

        RunStarBegin();

        yield return new WaitForSeconds(9.0f);

        CollectionAllStar();
    }

    private void RunBaoParticleSystem()
    {
        m_Bao.Play(true);
    }

    private void RunStarBegin()
    {
        for (int i = 0; i < m_UI_StarItemScriptList.Count; i++)
            m_UI_StarItemScriptList[i].RunBegin();
    }

    public void RunStarEnd(UI_StarItem item)
    {
        //自动收集需要计算还剩下多少星星
        for (int i = 0; i < m_UI_StarItemScriptList.Count; i++)
        {
            if (m_UI_StarItemScriptList[i] == item)
            {
                m_UI_StarItemScriptList.RemoveAt(i);

                break;
            }
        }

        //删除
        if (--m_nStarCount == 0)
        {
            CloseUI();

            if (m_Callback != null)
                m_Callback();
        }
    }

    private void CollectionAllStar()
    {
        for (int i = 0; i < m_UI_StarItemScriptList.Count; i++)
            m_UI_StarItemScriptList[i].OnButtonDown();
    }
}
