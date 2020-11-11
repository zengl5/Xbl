using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YB.XWK.MainScene;
public class UI_NewYearIcon : MonoBehaviour {
    private Text _Time;
    private RectTransform _State;
    private RectTransform _TimeBar;
    private float _TimeSpeed;
    private Button _ClickBtn;
    // Use this for initialization
    void Start () {
        
        OnInit();
    }
    public void InitNewYearIcon()
    {
        OnInit();
    }
    void OnInit()
    {
        if (_Time == null)
        {
            _Time = transform.Find("time").GetComponent<Text>();
            _State = transform.Find("state").GetComponent<RectTransform>();
            _TimeBar = transform.Find("TimeBar").GetComponent<RectTransform>();
            _ClickBtn = transform.GetComponent<Button>();
        }

        ShowTime();
    }
    void ShowTimeBar()
    {
        _State.gameObject.SetActive(false);
        _TimeBar.gameObject.SetActive(true);
        _Time.gameObject.SetActive(true);
    }
    void ShowState()
    {
        _State.gameObject.SetActive(true);
        _TimeBar.gameObject.SetActive(false);
        _Time.gameObject.SetActive(false);
    }
	void ShowTime()
    {
        _TimeSpeed = (float)AppInfoData.FetchNewYearGameLeaveTime();
        if (_TimeSpeed <=0.1f)
        {
           // AppInfoData.ResumehNewYearGameTime();
            AppInfoData.EnterGameFlag = true;
            CancelInvoke("ShowTime");
            _TimeSpeed = 0;
            ShowState();
            return;
        }
        int hour = (int)_TimeSpeed / 3600;
        int minute = ((int)_TimeSpeed - hour * 3600) / 60;
        int second = (int)_TimeSpeed - hour * 3600 - minute * 60;
        int millisecond = (int)((_TimeSpeed - (int)_TimeSpeed) * 1000);
        _Time.text = string.Format("{0}{1}:{2:D2}", hour, minute, second);

        ShowTimeBar();
        if (!IsInvoking())
        {
            Invoke("ShowTime", 1.0f);
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
    public void CloseUI()
    {
        if (_TimeBar!=null)
        {
            _TimeBar.gameObject.SetActive(false);
        } 
        CancelInvoke("ShowTime");
    }
    public void AddClickListener(UnityAction callback)
    {
        if (_ClickBtn == null)
        {
            OnInit();
        }
        _ClickBtn.onClick.RemoveAllListeners();
        _ClickBtn.onClick.AddListener(callback);
    }
    public void RemoveClickListener()
    {
        if (_ClickBtn == null)
            return;
       _ClickBtn.onClick.RemoveAllListeners();
    }
    private void OnApplicationQuit()
    {
      //AppInfoData.ResumehNewYearGameTime();
    }
}
