using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Rest : C_BaseUI
{
    [SerializeField]
    private Text m_Text_Rest = null;

    private int needRestTime = -1;
    protected override void onUpdate()
    {
        if (C_MonoSingleton<GameLogic>.GetInstance().TimeState == GameLogic.EnumTimeState.Rest)
        {
            needRestTime = C_MonoSingleton<GameLogic>.GetInstance().NeedRestTime;
            if (needRestTime >= 0)
                m_Text_Rest.text = (needRestTime / 60) + ":" + (needRestTime % 60);
        }
    }
}
