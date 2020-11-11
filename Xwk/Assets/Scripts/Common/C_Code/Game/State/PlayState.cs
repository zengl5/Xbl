using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayState : C_IState
{
    public string Name { get; set; }

    public PlayState()
    {
        Name = "PlayState";
    }

    public virtual void OnStateEnter()
    {
        C_MonoSingleton<C_UIMgr>.GetInstance().DestoryAllUI();
        C_MonoSingleton<GuideMgr>.GetInstance().c_CurGuide = "";
    }

    public virtual void OnStateLeave()
    {
    }

    public virtual void OnStateOverride()
    {
    }

    public virtual void OnStateResume()
    {
    }
}
