using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EffectProgressUI : MonoBehaviour {

    public Canvas canvas;
    Camera EffecUICam;
    public Image ProgressUI;
    bool showProgress = false;
    float timer = 0;
    float Speed = 0.005f;
    // Use this for initialization
    void Awake () {
         ProgressUI.fillAmount = 0;
    }

	// Update is called once per frame
	void Update () {
		if(showProgress)
        {            
            timer += Time.deltaTime;
            ProgressUI.fillAmount += timer*Speed;
            if (ProgressUI.fillAmount >= 1.0F)
            {
                showProgress = false;
                Destroy(EffecUICam.transform.gameObject);
                Destroy(this.gameObject);
            }
        }
	}
    void SetCam()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        EffecUICam = GameObject.Find("EffectUICamera").GetComponent<Camera>();//GC
        canvas.worldCamera = EffecUICam;
    }
    public void  ShowUIProgress()
    {
        SetCam();
        showProgress = true;
    }
}
