using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Effects : C_BaseUI
{
    [SerializeField]
    private GameObject m_Image_CeShi = null;
    [SerializeField]
    private ParticleSystem[] m_ChickParticleSystemVector = null;
    [SerializeField]
    private GameObject m_Image_CeShi_Text = null;
    private int m_nIndex = 0;
    protected override void onInit()
    {
        base.onInit();
      //  m_Image_CeShi.SetActive(true);
      //  m_Image_CeShi.GetComponent<UnityEngine.UI.Text>().text = GameDataMgr.c_UDID.ToString() + "," + GameDataMgr.c_DeviceUID.ToString();

    }
    protected override void onOpenUI(params object[] uiObjParams)
    {
        if (GameDataMgr.c_Debug == 0)
            m_Image_CeShi_Text.SetActive(true);
        else
            m_Image_CeShi_Text.SetActive(false);
    }

    protected override void onUpdate()
    {
        if (C_UIMgr.c_UICameraHigh == null)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/suiyi4.ogg");
            if (m_nIndex >= m_ChickParticleSystemVector.Length)
                m_nIndex = 0;

            Vector3 worldPos = Vector3.zero;
            if (Input.touchCount > 1)
                worldPos = C_UIMgr.c_UICameraHigh.ScreenToWorldPoint(Input.GetTouch(0).position);
            else
                worldPos = C_UIMgr.c_UICameraHigh.ScreenToWorldPoint(Input.mousePosition);

            Vector2 uiPos = UICanvas.transform.InverseTransformPoint(worldPos);
            m_ChickParticleSystemVector[m_nIndex].gameObject.SetActive(true);
            m_ChickParticleSystemVector[m_nIndex].transform.localPosition = uiPos;
            m_ChickParticleSystemVector[m_nIndex].Play();

            m_nIndex++;
        }

        if (C_Math.IsEqual_Float(Time.timeScale, 0))
        {
            for (int i = 0; i < m_ChickParticleSystemVector.Length; i++)
                m_ChickParticleSystemVector[i].gameObject.SetActive(false);
        }

        //if (GameDataMgr.c_Debug == 0)
        //    m_Image_CeShi.SetActive(true);
        //else
        //    m_Image_CeShi.SetActive(false);


    }
}
