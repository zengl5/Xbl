using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QApp : MonoBehaviour {
    public delegate void UpdateHandler();
    public UpdateHandler onUpdate;
    public delegate void onGUIHandler();
    public onGUIHandler onGUI;

    private static QApp _QApp;

    public static QApp Instance()
    {
        if (_QApp == null)
        {
            GameObject app = new GameObject("AppDebug");
            _QApp = app.gameObject.AddComponent<QApp>();
        }
        return _QApp;
    }
    void Awake()
    {
        _QApp = this;
    }
	// Use this for initialization
	void Start () {
        _QApp = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (onUpdate!= null)
            onUpdate();
	}
    void OnGUI()
    {
        if(onGUI != null){
            onGUI();
        }
    }
}
