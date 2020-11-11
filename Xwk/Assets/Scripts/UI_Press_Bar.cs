using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.C_Framework;

public class UI_Press_Bar : C_BaseUI
{
    private bool pressDown = false;
    private bool release = false;
    private bool close = false;
    private Image _ProgressBar;
    private Transform _ProgressBar_Bt;
    private ParticleSystem particleSystem_a;
    private ParticleSystem particleSystem_a_1;
    private bool updatePos = true;
    private float totalTime;
    private C_Event _StartEvent = new C_Event();
    protected override void onOpenUI(params object[] uiObjParams)
    {
        pressDown = false;
        release = false;
        close = false;
        _ProgressBar = transform.Find("Canvas/bar/wk_jdt1").GetComponent<Image>();
        particleSystem_a = transform.Find("Canvas/bar/wk_lz4_a").GetComponent<ParticleSystem>();
        particleSystem_a_1 = transform.Find("Canvas/bar/wk_lz4_a_1").GetComponent<ParticleSystem>();
     

        _ProgressBar_Bt = transform.Find("Canvas/bar/wk_jdt");
        _ProgressBar.fillAmount = 0f;
        if (uiObjParams.Length > 0)
        {
            updatePos = false;
            Vector2 uiPos = UICanvas.transform.InverseTransformPoint((Vector3)uiObjParams[0]);
            _ProgressBar.transform.parent.localPosition = new Vector3(uiPos.x , uiPos.y, 0);
        }
        if (uiObjParams.Length > 1)
        {
            updatePos = false;
            totalTime = (float)uiObjParams[1];
        }
        else
        {
            totalTime = 1f;
        }
        
        _ProgressBar_Bt.gameObject.SetActive(false);

        _StartEvent.RegisterEvent(C_EnumEventChannel.Global, "UI_Press_Bar",(b)=> { StartPress(); });
    }
    protected override void onUpdate()
    {
        if (close)
        {
            return;
        }
        base.onUpdate();
        if (pressDown)
        {
            float progress = _ProgressBar.fillAmount * totalTime;
            progress += Time.deltaTime;
            if (progress >= totalTime)
            {

                progress = totalTime;
            }
            _ProgressBar.fillAmount = progress / totalTime;
        }
        return;
        if (Input.GetMouseButtonDown(0))
        {
           
        }
        if (Input.GetMouseButton(0))
        {
            if (!pressDown)
            {
                pressDown = true;
                PressDown();
            }
            _ProgressBar_Bt.gameObject.SetActive(true);
            float progress = _ProgressBar.fillAmount* totalTime;
            progress += Time.deltaTime;
            if (progress >= totalTime)
            {
        
                progress = totalTime;
            }
            _ProgressBar.fillAmount = progress/ totalTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            pressDown = false;
            release = true;
        }
        if (release){
            _ProgressBar_Bt.gameObject.SetActive(true);
            float progress = _ProgressBar.fillAmount;
            if (progress > 0)
            {
                progress -= 2* Time.deltaTime;
                if (progress <= 0)
                {
                    particleSystem_a.Stop();
                    particleSystem_a_1.Stop();
                    progress = 0f;
                    release = false;
                    _ProgressBar_Bt.gameObject.SetActive(false);
                }
                _ProgressBar.fillAmount = progress;
            }
        }
    }
    public void StartPress()
    {
        pressDown = true;
        _ProgressBar_Bt.gameObject.SetActive(true);

    }
    protected void PressDown()
    {
        particleSystem_a.Play();
        particleSystem_a_1.Play();

        release = false;
        _ProgressBar_Bt.gameObject.SetActive(true);

        if (updatePos)
        {
            Vector3 worldPos = Vector3.zero;
            if (Input.touchCount > 1)
                worldPos = C_UIMgr.c_UICameraHigh.ScreenToWorldPoint(Input.GetTouch(0).position);
            else
                worldPos = C_UIMgr.c_UICameraHigh.ScreenToWorldPoint(Input.mousePosition);
            Vector2 uiPos = UICanvas.transform.InverseTransformPoint(worldPos);
#if true
            Debug.Log(worldPos);
            _ProgressBar.transform.parent.localPosition = new Vector3(uiPos.x, uiPos.y, 0);
#else
                _ProgressBar.transform.parent.localPosition = new Vector3(uiPos.x + 100f, uiPos.y, 0);
#endif
        }
    }
    protected override void onCloseUI()
    {
        _StartEvent.UnregisterEvent();
        //停止刷新
        _ProgressBar.fillAmount = 0f;
        release = true;
        close = true;
    }
}
