using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpdateLookforTime : MonoBehaviour {
    private Text _Text;
    private float _TimeSpeed;
    // Use this for initialization
    void Start () {
        InitText();
    }
    void InitText()
    {
        if (_Text==null)
        {
            _Text = transform.GetAddComponent<Text>();
        }
    }
    private void OnEnable()
    {
        InitText();
        ShowTime();
    }
    private void OnDisable()
    {
        if (!IsInvoking())
        {
            CancelInvoke("ShowTime");
        }
    }
    void ShowTime()
    {
        _TimeSpeed = (float)AppInfoData.FetchNewYearGameLeaveTime();
        if (_TimeSpeed <= 0.1f)
        {
            CancelInvoke("ShowTime");
            _Text.text = "0s";
            return;
        }
        _Text.text =string.Concat( _TimeSpeed.ToString("F0"),"s");

        if (!IsInvoking())
        {
            Invoke("ShowTime", 1.0f);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
