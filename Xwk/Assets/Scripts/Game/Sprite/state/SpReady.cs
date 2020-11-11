using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using DG.Tweening;
using Xbl;

public class SpReady :C_IState, IRubbish
{
    public string Name { get; set; }
    Spxwk xwk;
    Spjl jl;
    public SpReady(GameObject xwkobj,GameObject jlobj)
    {
        xwk = xwkobj.GetComponent<Spxwk>();
        jl = jlobj.GetComponent<Spjl>();
    }
    public virtual void OnStateEnter()
    {
        Name = "SpReady";
        Init();
    }
    public void RecycleRubbish()
    {
      
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
    void Init()
    {
        jl.Nanguo();
        SpriteManager.Instance.PlayCharacterAudio("byjlyx_3", XwkTalk);
    }

    void XwkTalk()
    {
        jl.NanguoOver();
        xwk.StartTalk();
        SpriteManager.Instance.PlayUserNameAudio("byjlyx_4",GotoGame);//帮帮他们吧
    }
    void GotoGame()
    {
        xwk.EndTalk();
        SpriteManager.Instance.GotoState("SpGame");
    }
}