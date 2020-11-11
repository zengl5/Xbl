using UnityEngine;
using System.Collections;
using Assets.Scripts.C_Framework;

public class ShakePhone :MonoBehaviour
{
    public static ShakePhone Instance;
    public delegate void ShakeEvent();
    public event ShakeEvent shakeEvent;
    float old_y = 0;
    float new_y = 0;
    float d_y = 0;
    int shakeTime=0;
    void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        new_y = Input.acceleration.y;
        d_y = new_y - old_y;
        old_y = new_y;
        if (d_y > 1)
        {
            shakeTime++;
            if(shakeTime>=3)
            {
                shakeTime = 0;
                if (shakeEvent != null)
                    shakeEvent();
            }      
        }
    }

    void OnGUI()
    {
        //GUI.skin.label.fontSize = Screen.width / 40;
        //GUILayout.Label("input.gyro.gravity: " + Input.acceleration + "d_y:" + d_y);
        //GUILayout.Label("input.gyro.gravity: " + shakeTime);       
    }
}