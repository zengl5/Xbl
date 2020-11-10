using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginState : C_IState
{
    public string Name { get; set; }

    public LoginState()
    {
        Name = "LoginState";
    }

    public virtual void OnStateEnter()
    {
        C_MonoSingleton<C_UIMgr>.GetInstance().OpenUICloseOthers("UI_Login");
    }

    public virtual void OnStateLeave()
    {
        C_MonoSingleton<C_UIMgr>.GetInstance().CloseUI("UI_Login");
    }

    public virtual void OnStateOverride()
    {
    }

    public virtual void OnStateResume()
    {
    }
}
