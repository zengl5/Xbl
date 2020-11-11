using Assets.Scripts.C_Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StarItem : MonoBehaviour
{
    private UI_Star m_UI_Star = null;

    [SerializeField]
    private Transform m_Star = null;

    [SerializeField]
    private ParticleSystem m_TuoWei = null;
    [SerializeField]
    private ParticleSystem m_HuoDe = null;

    [SerializeField]
    private DOTweenAnimation m_DOTweenAnimation = null;

    private Vector3 m_MovePos = Vector3.zero;

    public void Init(UI_Star ui_star, Vector3 movePos)
    {
        m_UI_Star = ui_star;
        m_MovePos = movePos;

        m_Star.gameObject.SetActive(true);
        m_Star.localScale = Vector3.one;
    }

    public void RunBegin()
    {
        m_DOTweenAnimation.DORestartById("0");
    }

    private bool m_bCheck = false;
    public void OnButtonDown()
    {
        if (m_bCheck)
            return;

        m_bCheck = true;

        m_TuoWei.Play();
        this.transform.DOMove(m_MovePos, 0.8f);
        m_Star.DOScale(Vector3.zero, 0.8f);

        Invoke("RunEnd", 0.8f);

        AudioMgr.PlaySoundEffect("public_sd_046");
    }

    private void RunEnd()
    {
        m_TuoWei.Stop();
        m_HuoDe.Play();

        Invoke("End", 0.3f);
    }

    private void End()
    {
        PlayerData.StarCount += 1;
        PlayerData.Save();

        m_UI_Star.RunStarEnd(this);
    }
}
