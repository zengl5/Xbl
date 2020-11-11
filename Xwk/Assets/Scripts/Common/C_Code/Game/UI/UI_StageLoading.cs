using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageLoading : C_BaseUI
{
    [SerializeField]
    private Image m_Image_Loading = null;

    protected override void onOpenUI(params object[] uiObjParams)
    {
        if (uiObjParams.Length > 0)
        {
            string stage = uiObjParams[0].ToString();

            if (!string.IsNullOrEmpty(stage))
            {
                C_MonoSingleton<C_SceneMgr>.GetInstance().LoadScene("Play", "", () =>
                {
                    CloseUI();
                });

                return;
            }
        }

        CloseUI();
    }

    protected override void onUpdate()
    {
        m_Image_Loading.fillAmount = C_MonoSingleton<C_SceneMgr>.GetInstance().Progress;
    }
}
