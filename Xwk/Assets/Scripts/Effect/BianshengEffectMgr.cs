using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BianshengEffectMgr : MonoBehaviour {
    private bool start = false;
	// Use this for initialization
	void Start () {
        if (!start)
            PlayEffect();
         start = true;

    }
    void PlayEffect()
    {
        AudioManager.Instance.PlayEffectSound("public/sound/public_sd_019.ogg");

    }
    void StopEffect()
    {
        AudioManager.Instance.StopEffectByKey("public/sound/public_sd_019.ogg");
    }
    private void OnEnable()
    {
        if (start) PlayEffect();
    }
    private void OnDisable()
    {
        if (start) StopEffect();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
