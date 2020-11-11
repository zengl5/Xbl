using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class huluren : MonoBehaviour {

    Animator anim;
	void Awake () {
        anim = this.GetComponent<Animator>();
    }

    public void Talk()
    {
        anim.SetBool("talkend", false);
        anim.Play("jl00001_1_deyi01_start#anim");
    }
    public void TalkEnd()
    {
        anim.SetBool("talkend", true);
    }
    public void Bianshen()
    {
        anim.Play("jl00001_1_bianshen01#anim");
    }

}
